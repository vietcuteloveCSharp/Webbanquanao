using DAL.Context;
using HelperMap.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.NTTuyenServices.IServices;
using Service.NTTuyenServices.Services;
using Service.VuVietAnhService.IRepository.IAccount;
using Service.VuVietAnhService.IRepository.IAccountKhachHang;
using Service.VuVietAnhService.IRepository.IAuthentication;
using Service.VuVietAnhService.IRepository.IChucvu;
using Service.VuVietAnhService.IRepository.ICuahang;
using Service.VuVietAnhService.IRepository.IDanhmuc;
using Service.VuVietAnhService.IRepository.IHoadon;
using Service.VuVietAnhService.IRepository.IKichthuoc;
using Service.VuVietAnhService.IRepository.IMausac;
using Service.VuVietAnhService.IRepository.ISanpham;
using Service.VuVietAnhService.IRepository.IThuonghieu;
using Service.VuVietAnhService.Repository.Hoadon;
using Service.VuVietAnhService.Repository.Account;
using Service.VuVietAnhService.Repository.AccountKhachhang;
using Service.VuVietAnhService.Repository.Authentication;
using Service.VuVietAnhService.Repository.Chucvu;
using Service.VuVietAnhService.Repository.Cuahang;
using Service.VuVietAnhService.Repository.Danhmuc;
using Service.VuVietAnhService.Repository.Kichthuoc;
using Service.VuVietAnhService.Repository.Mausac;
using Service.VuVietAnhService.Repository.Sanpham;
using Service.VuVietAnhService.Repository.Thuonghieu;
using System.Text;
using WebAPI.CheckEnpoint;
using Service.VuVietAnhService.IRepository.ICalamviec;
using Service.VuVietAnhService.Repository.Calamviec;
using Service.VuVietAnhService.IRepository.INgaylamviec;
using Service.VuVietAnhService.Repository.Ngaylamviec;

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

            // Đăng kí Automapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(typeof(MapperDTO_Entity), typeof(MapperEntity_DTO));
           

            // Đăng ký dịch vụ trong DI
            builder.Services.AddScoped<IAccountKHService, AccountKHService>();
            builder.Services.AddScoped<IChucvuService, ChucvuService>();
            builder.Services.AddScoped<IAuthenService, AuthenService>();
            builder.Services.AddScoped<IAccountService, AccountSerivce>();
            builder.Services.AddScoped<ICuahangService, CuahangService>();
            builder.Services.AddScoped<IMausacService, MausacService>();
            builder.Services.AddScoped<IDanhmucService, DanhmucSerivce>();
            builder.Services.AddScoped<IKichthuocService, Kichthuocservice>();
            builder.Services.AddScoped<ISanphamSerivce, SanphamService>();
            builder.Services.AddScoped<IThuonghieuSerivce, ThuonghieuService>();
            builder.Services.AddScoped<ICalamviecService,CalamviecService>();
            builder.Services.AddScoped<INgaylamviecService,NgaylamviecService>();
            builder.Services.AddScoped<INhanVienServices, NhanVienServices>();


            builder.Services.AddScoped<IChiTietHoaDonService, ChiTietHoaDonService>();
            builder.Services.AddScoped<IChiTietSanPhamServices,     ChiTietSanPhamServices>();
            
            builder.Services.AddScoped<IHoadonService, HoadonService>();
            
            //cấu hình jwt
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            // Sử dụng Authentication và JWT
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "MyAppApi",
                        ValidAudience = "MyAppClient",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });

            builder.Services.AddControllers();

            // Cấu hình Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập 'Bearer' [khoảng trắng] và sau đó là token của bạn.\n\nVí dụ: 'Bearer abcdef12345'"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
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
}
