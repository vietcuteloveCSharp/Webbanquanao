﻿using DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebView
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //connectDB
            builder.Services.AddDbContext<DAL.Context.WebBanQuanAoDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectionDb"]);
            });
            // Cấu hình session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session là 30 phút
                options.Cookie.HttpOnly = true; // Bảo mật session thông qua cookie
                options.Cookie.IsEssential = true; // Bắt buộc sử dụng session ngay cả khi người dùng tắt cookie
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            // Kích hoạt session
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=SanPham}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //Seedingdata
            var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<WebBanQuanAoDbContext>();
            SeedData.SeedingData(context);
            app.Run();
        }
    }
}
