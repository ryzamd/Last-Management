using LastManagement.Extensions;
using LastManagement.Middleware;

namespace LastManagement;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.NumberHandling =
                    System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // CORS Configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .WithExposedHeaders("Authorization"); ;
            });
        });

        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddJwtAuth(builder.Configuration);
        builder.Services.AddApplicationServices();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseCors("AllowFrontend");

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
    }
}