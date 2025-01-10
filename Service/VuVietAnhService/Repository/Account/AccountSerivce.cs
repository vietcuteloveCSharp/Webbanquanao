using AutoMapper;
using Azure;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.NhanViens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Responses.Responses;
using Responses.Resquests;
using Service.VuVietAnhService.IRepository.IChucvu;
using Service.VuVietAnhService.IRepository.IAccount;
using Service.VuVietAnhService.IRepository.IAuthentication;



namespace Service.VuVietAnhService.Repository.Account
{
    public class AccountSerivce : IAccountService
    {
        private readonly WebBanQuanAoDbContext _context;
        private IMapper _mapper;
        private readonly IAuthenService _authenService;
        private readonly IChucvuService _chucvuService;
        public AccountSerivce(WebBanQuanAoDbContext context, IMapper mapper, IAuthenService authenService, IChucvuService chucvuService)
        {
            _context = context;
            _mapper = mapper;
            _authenService = authenService;
            _chucvuService = chucvuService;
        }
        //kiểm tra email đã tồn tại chưa
        public async Task<bool> CheckEmailExitst(string email)
        {
            var findEmail = await _context.NhanViens.FirstOrDefaultAsync(x => x.Email == email);
            if (findEmail != null) return false;
            return true;
        }
        //check sđt đã tồn tại chưa
        public async Task<bool> CheckSdtExitst(string sdt)
        {

            var findSdt = await _context.NhanViens.FirstOrDefaultAsync(x => x.Sdt == sdt);
            if (findSdt != null) return false;
            return true;

        }
        // check taikhoan đã tồn tại chưa
        public async Task<bool> CheckTaikhoanExitst(string taikhoan)
        {
            var findTaikhoan = await _context.NhanViens.FirstOrDefaultAsync(x => x.TaiKhoan == taikhoan);
            if (findTaikhoan != null) return false;
            return true;
        }
        public async Task<ResponseText> LoginAccount([FromBody] LoginResquest loginResquest)
        {
            // khởi tạo phản hồi
            var response = new ResponseText();
            try
            {
                var account = await _context.NhanViens.SingleOrDefaultAsync(a => a.TaiKhoan == loginResquest.TaiKhoan);
                
                //check account có tồn tại không
                if (account == null)
                {
                    response.Success = false;
                    response.Message = "Tài khoản không tồn tại.";
                    return response;
                }
                ///USE KHAC
                var role = await _chucvuService.GetChucVuById(account.Id_ChucVu);
                // Kiểm tra mật khẩu
                if (account.MatKhau != loginResquest.MatKhau)
                {
                    response.Success = false;
                    response.Message = "Mật khẩu không đúng.";
                    return response;
                }
                // Chuyển NhanVien sang NhanvienDTO
                var accountDTO = _mapper.Map<NhanvienDTO>(account);
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

        public async Task<ResponseObj<NhanvienRegisterDTO>> RegisterAccount([FromBody] NhanvienRegisterDTO nhanvienRegisterDTO)
        {
            // Khởi tạo đối tượng phản hồi
            var response = new ResponseObj<NhanvienRegisterDTO>();
            try
            {
                if (!await CheckEmailExitst(nhanvienRegisterDTO.Email))
                {
                    response.Success = false;
                    response.Message = "Email đã tồn tại.";
                    response.Errors.Add("Email đã đăng kí");
                    return response;
                }
                if (!await CheckSdtExitst(nhanvienRegisterDTO.Sdt))
                {
                    response.Success = false;
                    response.Message = "Số điện thoại đã tồn tại.";
                    response.Errors.Add("Số điện thoại đã đăng kí");
                    return response;
                }
                if (!await CheckTaikhoanExitst(nhanvienRegisterDTO.TaiKhoan))
                {
                    response.Success = false;
                    response.Message = "Tài khoản đã tồn tại.";
                    response.Errors.Add("Tài khoản  đã đăng kí");
                    return response;
                }
                //add thêm nhanvien
                var newNhanVien = _mapper.Map<NhanVien>(nhanvienRegisterDTO);
                await _context.NhanViens.AddAsync(newNhanVien);
                await _context.SaveChangesAsync();
                // Thành công, trả về thông tin người dùng
                response.Success = true;
                response.Message = "Đăng ký thành công.";
                response.Data = nhanvienRegisterDTO;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Đã xảy ra lỗi trong quá trình đăng ký.";
                response.Errors.Add(ex.Message);
            }

            return response;
        }
    }
}
