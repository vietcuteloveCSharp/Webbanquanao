using AutoMapper;
using DAL.Context;
using DTO.VuvietanhDTO.NhanViens;
using Service.NTTuyenServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.Services
{
    public class NhanVienServices : INhanVienServices
    {
        private  WebBanQuanAoDbContext _context;
        private  IMapper _mapper;
        public NhanVienServices(WebBanQuanAoDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<NhanVienProfileDTO> GetById(int id)
        {
            
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                throw new Exception("Nhân viên không tồn tại");
            }
            return _mapper.Map<NhanVienProfileDTO>(nhanVien);
        }
    }
}
