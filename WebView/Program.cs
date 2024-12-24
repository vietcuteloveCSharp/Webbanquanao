using Microsoft.EntityFrameworkCore;
using WebView.Extensions;
using WebView.Services.Vnpay;

namespace WebView
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            // add httpclient
            builder.Services.AddHttpClient<GetHttpClient>("SystemApiClient", clients =>
            {
                clients.BaseAddress = new Uri("https://localhost:7169");
            });
            // DI class GetHttpClient
            builder.Services.AddScoped<GetHttpClient>();
            //connectDB
            builder.Services.AddDbContext<DAL.Context.WebBanQuanAoDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectionDb"]);
            });

            // Cấu hình session
            builder.Services.AddDistributedMemoryCache();
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

            // Kích hoạt session
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapAreaControllerRoute(
             name: "BanTaiQuay",
            areaName: "BanTaiQuay",
            pattern: "{area:exists}/{controller=BanNhanh}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "BanHangOnline",
                areaName: "BanHangOnline",
                pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
             name: "Admin",
            areaName: "Admin",
            pattern: "{area:exists}/{controller=SanPham}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            //Seedingdata
            //var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<WebBanQuanAoDbContext>();
            //SeedData.SeedingData(context);
            app.Run();
        }
    }
}
