using System.Text.Json;
using System.Text.Json.Serialization;
using CommentsApp.Api.Configs;
using CommentsApp.Api.Data;
using CommentsApp.Api.Repositories;
using CommentsApp.Api.Repositories.Implementations;
using CommentsApp.Api.Services;
using CommentsApp.Api.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseConfig>();
builder.Services.AddSingleton<JwtConfig>();
builder.Services.AddSingleton<RedisConfig>();

builder.Services.AddScoped<IUserService, DbUserService>();

builder.Services.AddScoped<IUserRepository, DbUserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, DbRefreshTokenRepository>();

builder.Services.AddSingleton<IConfigureNamedOptions<JwtBearerOptions>, JwtConfigureOptions>();

builder.Services.AddDbContext<DataContext>((provider, options) =>
{
    var config = provider.GetRequiredService<DatabaseConfig>();
    var connectionString = new SqlConnectionStringBuilder()
    {
        UserID = config.User,
        Password = config.Password,
        InitialCatalog = config.Database,
        DataSource = config.Server,
        TrustServerCertificate = true
    }.ConnectionString;

    options.UseSqlServer(connectionString).UseSnakeCaseNamingConvention();
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

builder.Services.AddScoped<IConnectionMultiplexer>(provider =>
{
    var config = provider.GetRequiredService<RedisConfig>();
    return ConnectionMultiplexer.Connect(new ConfigurationOptions()
    {
        EndPoints = config.EndPoints
    });
});

var app = builder.Build();

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

internal class JwtConfigureOptions(IServiceProvider provider, JwtConfig config) : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options) => Configure(JwtBearerDefaults.AuthenticationScheme, options);

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name is not JwtBearerDefaults.AuthenticationScheme) return;
        options.TokenValidationParameters = config.TokenValidationParameters;
    }
}