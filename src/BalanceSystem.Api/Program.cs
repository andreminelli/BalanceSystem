using BalanceSystem.Api.Services;
using BalanceSystem.DataAccess;
using BalanceSystem.DataAccess.PostgreSql;
using BalanceSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
		options =>
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = false,
				IssuerSigningKeys = new SecurityKey[]
				{
					new SymmetricSecurityKey(Convert.FromBase64String("NTNv7j0TuYARvmNMmWXo6fKvM4o6nv/aUi9ryX38ZH+L1bkrnD1ObOQ8JAUmHCBq7Iy7otZcyAagBLHVKvvYaIpmMuxmARQ97jUVG16Jkpkp1wXOPsrF9zwew6TpczyHkHgX5EuLg2MeBuiT/qJACs1J0apruOOJCg/gOtkjB4c=")),
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopasdfghjklzxcvbnm123456"))
				},
				TryAllIssuerSigningKeys = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true
			};
		});

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
builder.Services.AddSwaggerGen(setup =>
	{
		setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "JWT Authorization header using the Bearer scheme.",
			});
		setup.AddSecurityRequirement(new OpenApiSecurityRequirement
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
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigrations(app);

app.Run();

void ApplyMigrations(WebApplication app)
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