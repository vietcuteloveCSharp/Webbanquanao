using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.Context;
using DAL.Entities;
using DTO.NTTuyen.ChiTietHoaDon;
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
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly WebBanQuanAoDbContext _context; 
        private readonly IMapper _mapper;
        private readonly IAuthenService _authenService;

        public ChiTietHoaDonService(WebBanQuanAoDbContext context, IMapper mapper,IAuthenService authenService)
        {
            _context = context;
            _mapper = mapper;
            _authenService = authenService;
        }

        public async Task<ChiTietHoaDonDTO> Add(ChiTietHoaDonDTO obj)
        {
             _context.ChiTietHoaDons.Add(_mapper.Map<ChiTietHoaDon>(obj));
            _context.SaveChangesAsync();
            return obj;
        }

        public async Task<List<FullChiTietHoaDonDTO>> GetAll()
        {
            
            var value =await  _context.ChiTietHoaDons.ToListAsync();
            return _mapper.Map<List<FullChiTietHoaDonDTO>>(value);
        }

        public async Task<ChiTietHoaDonDTO> GetById(int id)
        {
          if (id == null)
                {
                        throw new ArgumentException("id không được để trống");  
                }
          else
            {
                var hd = await _context.ChiTietHoaDons.FindAsync(id);
                if (hd == null)
                {
                    throw new NullReferenceException("Chi tiết hóa đơn không tồn tại");
                }
                else
                {
                    return _mapper.Map<ChiTietHoaDonDTO>(hd);
                }
            }
        }
        public  async Task<List<ChiTietHoaDonDTO>> GetByHoaDonId(int id)
        {
            if (id == null)
            {
                throw new ArgumentException("id không được để trống");
            }
            else
            {
                var hd = await _context.ChiTietHoaDons.Where(hd => hd.Id_HoaDon == id).ProjectTo<ChiTietHoaDonDTO>(_mapper.ConfigurationProvider).ToListAsync();
                if (hd == null)
                {
                    throw new NullReferenceException("Chi tiết hóa đơn không tồn tại");
                }
                else
                {
                    return _mapper.Map<List<ChiTietHoaDonDTO>>(hd);
                }
            }
        }

        public async Task<ChiTietHoaDonDTO> Update(int id, ChiTietHoaDonDTO obj)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id không được để trống");
            }
            else
            {
                var hd =await _context.ChiTietHoaDons.FindAsync(id);
                if (hd == null)
                {
                    throw new NullReferenceException("Hóa đơn không tồn tại");
                }
                else
                {
                    var newHD = _mapper.Map(hd, obj);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<ChiTietHoaDonDTO>(newHD);
                }
            }
        }
    }
}
