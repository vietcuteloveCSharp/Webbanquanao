using AutoMapper;
using Azure;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Chucvus;
using DTO.VuvietanhDTO.NhanViens;
using DTO.VuvietanhDTO.KhachHangs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Responses.Responses;
using Responses.Resquests;
using Service.VuVietAnhService.IRepository.IAccountKhachHang;
using Service.VuVietAnhService.IRepository.IAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.AccountKhachhang
{
    public class AccountKHService : IAccountKHService

    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenService _authenService;
        public AccountKHService(WebBanQuanAoDbContext context, IMapper mapper, IAuthenService authenService)
        {
            _context = context;
            _mapper = mapper;
            _authenService = authenService;
        }
        //tìm tài khoản có tồn tại không
        public async Task<bool> IsExsitAccount(string taikhoan)
        {
            try
            {
                var account = await _context.KhachHangs.FirstOrDefaultAsync(x => x.TaiKhoan == taikhoan);
                return account == null ? true : false;
            }
            catch (Exception ex)
            {

                throw new Exception($"Có lỗi xảy ra:{ex.Message}");
            }
        }
        //login tài khoản khách hàng
        public async Task<ResponseText> LoginKH([FromBody] LoginResquest loginResquest)
        {
            var response = new ResponseText();
            try
            {
                var account = await _context.KhachHangs.SingleOrDefaultAsync(a => a.TaiKhoan == loginResquest.TaiKhoan);
                //kiểm tra account có tồn tại không
                if (account == null)
                {
                    response.Success = false;
                    response.Message = "Tài khoản không tồn tại.";
                    return response;
                }
                // Kiểm tra mật khẩu có đúng không
                if (account.MatKhau != loginResquest.MatKhau)
                {
                    response.Success = false;
                    response.Message = "Mật khẩu không đúng.";
                    return response;
                }
                // Chuyển khachhang thành khachhangDTO
                var accountDTO = _mapper.Map<KhachhangDTO>(account);
                // Tạo token JWT nếu đăng nhập thành công
                var token = await _authenService.GenerateJwtToken(accountDTO);
                // Nếu thông tin đăng nhập hợp lệ
                response.Success = true;
                response.Message = "Đăng nhập thành công.";
                response.Token = token;
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Có lỗi xảy ra: {ex.Message}";
                return response;
            }
        }
        public async Task<ResponseObj<KhachhangDTO>> RegisterAccountKH([FromBody] KhachhangDTO khachhangDTO)
        {
            // Khởi tạo đối tượng phản hồi
            var response = new ResponseObj<KhachhangDTO>();
            try
            {
                if (!await CheckEmailExitst(khachhangDTO.Email))
                {
                    response.Success = false;
                    response.Message = "Email đã tồn tại.";
                    response.Errors.Add("Email đã đăng kí");
                    return response;
                }
                if (!await CheckSdtExitst(khachhangDTO.Sdt))
                {
                    response.Success = false;
                    response.Message = "Số điện thoại đã tồn tại.";
                    response.Errors.Add("Số điện thoại đã đăng kí");
                    return response;
                }
                if (!await CheckTaikhoanExitst(khachhangDTO.TaiKhoan))
                {
                    response.Success = false;
                    response.Message = "Tài khoản đã tồn tại.";
                    response.Errors.Add("Tài khoản  đã đăng kí");
                    return response;
                }
                var khachHang = _mapper.Map<KhachHang>(khachhangDTO);
                await _context.AddAsync(khachHang);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Đăng ký thành công.";
                response.Data = khachhangDTO;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Đã xảy ra lỗi trong quá trình đăng ký.";
                response.Errors.Add(ex.Message);
            }
            return response;
        }
        //kiểm tra email có tồn tại không
        public async Task<bool> CheckEmailExitst(string email)
        {
            var findEmail = await _context.NhanViens.FirstOrDefaultAsync(x => x.Email == email);
            if (findEmail != null) return false;
            return true;
        }
        //kiểm tra sđt đã tồn tại không
        public async Task<bool> CheckSdtExitst(string sdt)
        {
            var findSdt = await _context.NhanViens.FirstOrDefaultAsync(x => x.Sdt == sdt);
            if (findSdt != null) return false;
            return true;
        }
        //kiểm tra taikhoan đã tồn tại không
        public async Task<bool> CheckTaikhoanExitst(string taikhoan)
        {
            var findTaikhoan = await _context.NhanViens.FirstOrDefaultAsync(x => x.TaiKhoan == taikhoan);
            if (findTaikhoan != null) return false;
            return true;
        }

        public async Task<List<KhachhangDTO>> GetAllAccountKH()
        {
            var AllAccountKH = await _context.KhachHangs.ToListAsync();
            if (AllAccountKH == null || !AllAccountKH.Any()) return new List<KhachhangDTO>();

            var AllAccountKHDTO = _mapper.Map<List<KhachhangDTO>>(AllAccountKH);
            return AllAccountKHDTO;
        }

        public async Task<KhachhangDTO> GetAccountKHById(int id)
        {
            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(c => c.Id == id);
            if (khachHang == null)
            {
                throw new KeyNotFoundException(nameof(khachHang), new($"Không tìm thấy chức vụ với ID {id}."));
            }
            var khachHangDTO = _mapper.Map<KhachhangDTO>(khachHang);
            return khachHangDTO;

        }
    }
}
