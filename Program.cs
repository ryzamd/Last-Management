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
                policy.WithOrigins("http://localhost:3000") // Development - Next.js
                                                            // TODO: Add production origins: .WithOrigins("https://yourdomain.com")
                      .AllowAnyMethod() // GET, POST, PUT, DELETE
                      .AllowAnyHeader() // Including Authorization for JWT Bearer token
                      .AllowCredentials(); // For future cookies support if needed
            });
        });

        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddJwtAuth(builder.Configuration);
        builder.Services.AddApplicationServices();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowFrontend");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}