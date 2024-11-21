using DAL.Context;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.NhanViens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.IRepository.IChucvuService;
using Service.VuVietAnhService.IRepository.IAuthentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Authentication
{
    public class AuthenService : IAuthenService
    {
        private readonly IConfiguration _configuration;
        private readonly IChucvuService _chucvuService;
        public AuthenService(IConfiguration configuration, IChucvuService chucvuService)
        {
            _configuration = configuration;
            _chucvuService = chucvuService;
        }
        public async Task<string> GenerateJwtToken(NhanvienDTO nhanvienDTO)
        {
            // Lấy tên chức vụ từ service
            var roleName = await _chucvuService.GetTenChucVuById(nhanvienDTO.Id_ChucVu);
            if (roleName == null)
            {
                throw new Exception("Không tìm thấy vai trò của nhân viên này.");
            }
            // Tạo claims dựa trên thông tin từ NhanVienDTO
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, nhanvienDTO.TaiKhoan),
            new Claim(JwtRegisteredClaimNames.Name, nhanvienDTO.TenNhanVien),
            new Claim("RoleId", nhanvienDTO.Id_ChucVu.ToString()), // lưu id_chucvu
            new Claim(ClaimTypes.Role, roleName) // Lưu `ten_chucvu` sau khi tra cứu
            };

            var jwtSettings = _configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("SecretKey không được cấu hình"); //lấy key trong appsetting
            var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("Issuer không được cấu hình");// lấy key trong appsetting
            var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("Audience không được cấu hình");// lấy key trong appsetting
            // Tạo key và credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));//tạo key giống trong appsetting
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //mã hoá

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<string> GenerateJwtToken(KhachhangDTO khachhangDTO)
        {
            var claims = new[]
           {
            new Claim(JwtRegisteredClaimNames.Sub, khachhangDTO.TaiKhoan),
            new Claim(JwtRegisteredClaimNames.Name, khachhangDTO.Ten),
            new Claim(JwtRegisteredClaimNames.Email, khachhangDTO.Ten),
            new Claim("Phone_number", khachhangDTO.Sdt.ToString()), // phone number
            };
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("SecretKey không được cấu hình");
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            // Tạo key và credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));//tạo key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //mã hoá

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
