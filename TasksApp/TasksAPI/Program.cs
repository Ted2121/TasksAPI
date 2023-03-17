using Microsoft.Extensions.Configuration;
using Serilog;
using TasksAPI.Data;

namespace TasksAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        #region Data Access Middleware
        var localConnectionString = builder.Configuration["Tasks: LocalConnectionString"];

        builder.Services.AddScoped((conf) => SqlRepositoryFactory.CreateRepo<IPinnedTaskRepository>(localConnectionString));
        builder.Services.AddScoped((conf) => SqlRepositoryFactory.CreateRepo<ISuggestedLabelRepository>(localConnectionString));
        builder.Services.AddScoped((conf) => SqlRepositoryFactory.CreateRepo<ISuggestedTaskRepository>(localConnectionString));
        #endregion

        builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseSerilogRequestLogging();
        app.MapControllers();

        app.Run();
    }
}
