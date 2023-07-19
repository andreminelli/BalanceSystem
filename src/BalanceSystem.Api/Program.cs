using BalanceSystem.Api.Services;
using BalanceSystem.DataAccess;
using BalanceSystem.DataAccess.PostgreSql;
using BalanceSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Set logging
var logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.CreateLogger();
builder.Host.UseSerilog(logger);

// Add services to the container.
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
		(Action<JwtBearerOptions>)(options =>
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKeys = GetKeysFrom(builder.Configuration.GetRequiredSection("Jwt:Keys")),
				TryAllIssuerSigningKeys = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true
			};
		}));

builder.Services
	.AddDbContext<BalanceDbContext>(options =>
		options.UseNpgsql(builder.Configuration.GetConnectionString("BalanceConnectionString")));

builder.Services
	.AddScoped<IEntryRepository, PqSqlEntryRepository>()
	.AddScoped<IAccountRepository, PqSqlAccountRepository>()
	.AddScoped<IBalanceService, BalanceService>();

builder.Services
	.AddScoped<IAccountRetrievalService, HttpContextAccountRetrievalService>()
	.Decorate<IAccountRetrievalService, AutoInsertAccountRetrievalServiceDecorator>();

builder.Services
	.AddHttpContextAccessor()
	.AddControllers(
		options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
	.AddJsonOptions(opts =>
	{
		var enumConverter = new JsonStringEnumConverter();
		opts.JsonSerializerOptions.Converters.Add(enumConverter);
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "BalanceSystem API",
	});
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme.",
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});

	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigrations(app);

app.Run();

static void ApplyMigrations(WebApplication app)
{
	var logger = app.Services.GetRequiredService<ILoggerProvider>().CreateLogger("Startup");
	using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
	using var context = scope.ServiceProvider.GetRequiredService<BalanceDbContext>();

	var pendingMigrations = context.Database.GetPendingMigrations();
	if (pendingMigrations.Any())
	{
		logger.LogInformation("Applying {PendingMigrations} migrations", pendingMigrations.Count());
		context.Database.Migrate();
	}
}

static SecurityKey[] GetKeysFrom(IConfigurationSection configurationSection)
{
	var stringKeys = configurationSection.Get<string[]>();
	var keys = stringKeys
		.Select(key => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)))
		.ToArray<SecurityKey>();
	return keys;
}