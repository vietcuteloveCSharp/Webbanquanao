using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.NTTuyenDTO.ChiTietSanPhams;
using Microsoft.EntityFrameworkCore;
using Service.NTTuyenServices.IServices;
using Service.VuVietAnhService.IRepository.IAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.Services
{
    public class ChiTietSanPhamServices : IChiTietSanPhamServices
    {

        private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenService _authenService;

        public ChiTietSanPhamServices(WebBanQuanAoDbContext context, IMapper mapper, IAuthenService authenService)
        {
            _context = context;
            _mapper = mapper;
            _authenService = authenService;
        }

        public async Task<ChiTietSanPhamDTO> Add(ChiTietSanPhamDTO chitietsanpham)
        {
            var newCTSP = _mapper.Map<ChiTietSanPham>(chitietsanpham);  
                await _context.ChiTietSanPhams.AddAsync(newCTSP);
                await _context.SaveChangesAsync();
            return _mapper.Map<ChiTietSanPhamDTO>(newCTSP) ;
        }

        public async Task<ChiTietSanPhamDTO> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FullChiTietSanPhamDTO>> GetAllChiTietSanPham()
        {
            var value =await _context.ChiTietSanPhams.ToListAsync();
            return _mapper.Map<List<FullChiTietSanPhamDTO>>(value);
        }

        public async Task<ChiTietSanPhamDTO> GetChiTietSanPhamById(int id)
        {
            if (id== null)
            {
                throw new ArgumentException("Id không đc để trống");
            }
            else
            {
                var chitietsanpham = await _context.ChiTietSanPhams.FindAsync(id);
                return _mapper.Map<ChiTietSanPhamDTO>(chitietsanpham);
            }
           
        }

        public async Task<ChiTietSanPhamDTO> Update(int id, ChiTietSanPhamDTO chitietsanpham)
        {
            if (id == null)
            {
                throw new ArgumentException("id Không được để trống");
            }
            else
            {
                var udCTSP = await _context.ChiTietSanPhams.FindAsync(id);
                if (udCTSP == null)
                { 
                    throw new ArgumentException("chi tiet sản phẩm không tồn tại ");
                }
                else
                { 
                    var newCTSP = _mapper.Map(udCTSP, chitietsanpham); 
                    return _mapper.Map<ChiTietSanPhamDTO>(newCTSP); 
                } 
            }
        }
    }
}
