using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using SocialMediaAPI.Application.Extensions;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.RegisterServices(configuration);
builder.Services.ConfigureJwtAuth(configuration);

builder.Host.UseSerilog(Log.Logger);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler(x => x.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature!.Error;
    Log.Error(exception, "An exception occured while processing the request");
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(new { error = "Unexpected error happened. Please try again later or contact the Production Support team." });
}));

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
