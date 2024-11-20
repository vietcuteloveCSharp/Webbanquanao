
using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
using DAL.Context;
using HelperMap.Mapping;
using Microsoft.EntityFrameworkCore;
using Service.IServices_Admin;
using Service.Services_Admin;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //đăng ký DB
            builder.Services.AddDbContext<WebBanQuanAoDbContext>(
                option=>option.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=TUYEN_DEV\\SQLEXPRESS;Initial Catalog=Final_project;Integrated Security=True;Trust Server Certificate=True"))
                );
            //Đăng ký automapping
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(typeof(MapppingDTO_Entity), typeof(MappingEntity_DTO));
            //Đăng ký Depndency Injection cho Repository
            builder.Services.AddScoped<IChucVuRepository, ChucVuRepository>();

            builder.Services.AddScoped<IDanhMucRepository, DanhMucRepository>();

            //Đăng ký Depndency Injection cho Service
            builder.Services.AddScoped<IChucVuService, ChucVuService>();
            builder.Services.AddScoped<IDanhMucService, DanhMucService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorWasm",
                    builder => builder
                    .WithOrigins("https://localhost:7043", "http://localhost:5264")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

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
