<<<<<<< Updated upstream
﻿
using DAL.Context;
using Microsoft.EntityFrameworkCore;

=======
﻿using DAL.Context;
using Microsoft.EntityFrameworkCore;
using HelperMap.Mapping;
using Service.IRepository.IChucvuService;
using Service.Repository.AccountRole;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using WebAPI.CheckEnpoint;
using Service.IRepository.ICuahang;
using Service.Repository.Cuahang;
using Service.IRepository.IMausac;
using Service.Repository.Mausac;
using Service.VuVietAnhService.IRepository.IAuthentication;
using Service.VuVietAnhService.IRepository.IAccountKhachHang;
using Service.VuVietAnhService.IRepository.IAccount;
using Service.VuVietAnhService.Repository.AccountKhachhang;
using Service.VuVietAnhService.Repository.Account;
using Service.VuVietAnhService.Repository.Authentication;
using Service.VuVietAnhService.IRepository.IChucvu;
using Service.VuVietAnhService.Repository.Chucvu;
using Service.VuVietAnhService.IRepository.ICuahang;
using Service.VuVietAnhService.Repository.Cuahang;
using Service.VuVietAnhService.IRepository.IMausac;
using Service.VuVietAnhService.Repository.Mausac;
>>>>>>> Stashed changes
namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorWasm",
                    builder => builder
                    .WithOrigins("https://localhost:7043", "http://localhost:5264")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
              // Đăng ký DbContext
            builder.Services.AddDbContext<WebBanQuanAoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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
