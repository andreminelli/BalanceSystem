using BalanceSystem.Api.Services;
using BalanceSystem.DataAccess;
using BalanceSystem.DataAccess.PostgreSql;
using BalanceSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

builder.Services
	.AddDbContext<BalanceDbContext>(options =>
		options.UseNpgsql(builder.Configuration.GetConnectionString("BalanceConnectionString")));

builder.Services
	.AddScoped<IEntryRepository, PqSqlEntryRepository>()
	.AddScoped<IBalanceService, BalanceService>();

builder.Services
	.AddHttpContextAccessor()
	.AddControllers()
	.AddJsonOptions(opts =>
	{
		var enumConverter = new JsonStringEnumConverter();
		opts.JsonSerializerOptions.Converters.Add(enumConverter);
	});

builder.Services
	.AddScoped<IAccountRetrievalService, HttpContextAccountRetrievalService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
