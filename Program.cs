using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using testAPIs.Models;

namespace testAPIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //var connection = builder.Configuration.GetConnectionString("TestDataBase");
            //builder.Services.AddDbContext<TestDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("TestDatabase"));
            //        //sqlServerOptionsAction:sqlOptions =>
            //        //{
            //        //    sqlOptions.EnableRetryOnFailure();
            //        //}
            //        //);
            //});



            builder.Services.AddDbContextPool<TestDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("TestDatabase"));
            });
            // Add services to the container.

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}