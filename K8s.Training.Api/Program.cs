using K8s.Training.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace K8s.Training.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            _ = builder.Host.UseSerilog((context, services, configuration) =>
            {
                _ = configuration.ReadFrom.Configuration(context.Configuration, "Serilog");
            });

            // for debugging purpose
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            _ = builder.Services.AddDbContext<UsersDbContext>(options =>
            {
                _ = options
                    .UseSqlServer(builder.Configuration.GetConnectionString("Default"))
                    .LogTo(Console.WriteLine)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            });

            _ = builder.Services.AddAutoMapper(typeof(Program));
            _ = builder.Configuration.AddEnvironmentVariables();
            _ = builder.Services.AddControllers();
            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "K8s.Training.Api",
                    Version = "v1",
                    Description = "Description: K8s Training Api"
                });
            });

            _ = builder.Services.AddHealthChecks();

            WebApplication app = builder.Build();

            if (!app.Environment.IsProduction())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI(options =>
                {
                    options.DocumentTitle = "K8s.Training.Api";
                });

                _ = app.MapFallback(() => Results.Redirect("/swagger"));
            }

            using IServiceScope serviceScope = app.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;
            UsersDbContext usersDbContext = serviceProvider.GetRequiredService<UsersDbContext>();
            _ = usersDbContext.Database.EnsureDeleted();
            _ = usersDbContext.Database.EnsureCreated();

            usersDbContext.Users.AddRange(
                new Domain.Entities.User { Username = "testuser1", FirstName = "Test1", LastName = "User1", Email = "test.user@email.com", PhoneNumber = "972-000-0000" },
                 new Domain.Entities.User { Username = "testuser2", FirstName = "Test2", LastName = "User2", Email = "test2.user2@email.com", PhoneNumber = "214-000-0000" }
                );
            _ = usersDbContext.SaveChanges();

            _ = app.UseHttpsRedirection();

            _ = app.UseAuthorization();

            _ = app.MapControllers();
            _ = app.MapHealthChecks("/health").AllowAnonymous();

            app.Run();
        }
    }
}