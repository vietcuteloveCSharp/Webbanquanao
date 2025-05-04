using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebView.Extensions;
using WebView.Services;
using WebView.Services.Vnpay;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IVnPayService, VnPayService>();
// add httpclient
builder.Services.AddHttpClient<GetHttpClient>("SystemApiClient", clients =>
{
    clients.BaseAddress = new Uri("https://localhost:7169");
});
builder.Services.AddHttpClient<ApiService>();
// DI class GetHttpClient
builder.Services.AddScoped<GetHttpClient>();
//connectDB
builder.Services.AddDbContext<DAL.Context.WebBanQuanAoDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectionDb"]);
});
// Thêm Authentication Service
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "MyAppApi",        // Issuer giống với WebAPI
        ValidAudience = "MyAppClient",    // Audience giống với WebAPI
        IssuerSigningKey = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes("85DDE810rPlo2MkVgXYhfmfXo83z7qW5")) // SecretKey giống với WebAPI
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Đọc token từ Cookie 
            context.Token = context.Request.Cookies["JWTToken"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(); // Thêm dịch vụ phân quyền

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
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapAreaControllerRoute(
    name: "BanHangOnline",
    areaName: "BanHangOnline",
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "BanTaiQuay",
    areaName: "BanTaiQuay",
    pattern: "{area:exists}/{controller=BanNhanh}/{action=Index}/{id?}");
//app.MapAreaControllerRoute(
//    name: "Admin",
//    areaName: "Admin",
//    pattern: "{area:exists}/{controller=SanPham}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");
//app.MapControllerRoute(
//               name: "default",
//               pattern: "{controller=Home}/{action=Index}/{id?}");
//Seedingdata
//var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<WebBanQuanAoDbContext>();
//SeedData.SeedingData(context);
app.Run();
