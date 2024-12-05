
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
                option=>option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
            //Đăng ký automapping
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(typeof(MapppingDTO_Entity), typeof(MappingEntity_DTO));
            //Đăng ký Depndency Injection cho Repository
            builder.Services.AddScoped<IChucVuRepository, ChucVuRepository>();
            builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
            builder.Services.AddScoped<IDanhMucRepository, DanhMucRepository>();
            builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
            builder.Services.AddScoped<IPhuongThucThanhToanRepository, PhuongThucThanhToanRepository>();
            builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
            builder.Services.AddScoped<IThanhToanHoaDonRepository, ThanhToanHoaDonRepository>();
            builder.Services.AddScoped<IThuongHieuRepository, ThuongHieuRepository>();

            //Đăng ký Depndency Injection cho Service
            builder.Services.AddScoped<IChucVuService, ChucVuService>();
            builder.Services.AddScoped<IDanhMucService, DanhMucService>();
            builder.Services.AddScoped<IHoaDonService, HoaDonServices>();
            builder.Services.AddScoped<INhanVienServices, NhanVienService>();
            builder.Services.AddScoped<IPhuongThucThanhToanService, PhuongThucThanhToanService>();
            builder.Services.AddScoped<ISanPhamService, SanPhamService>();
            builder.Services.AddScoped<IThanhToanHoaDonService, ThanhToanHoaDonService>();
            builder.Services.AddScoped<IThuongHieuService, ThuongHieuService>();

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
