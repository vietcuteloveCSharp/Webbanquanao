using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.NTTuyen.HoaDons;
using Microsoft.EntityFrameworkCore;
using Service.NTTuyenServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly WebBanQuanAoDbContext _context;
        private IMapper _mapper;

        public HoaDonService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HoaDonDTO> Add(HoaDonDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Hóa đơn không được để trống");
            }
            var newHD = _mapper.Map<HoaDon>(obj);
            await _context.HoaDons.AddAsync(newHD);
            await _context.SaveChangesAsync();
            return _mapper.Map<HoaDonDTO>(newHD);
        }

        public async Task<HoaDonDTO> Delete(int id)
        {

            if (id == null)
            {
                throw new ArgumentNullException("id là bắt buộc");
            }
            var hd = _context.HoaDons.Find(id);
            if (hd == null)
            {
                throw new NullReferenceException("Hóa đơn không tồn tại");
            }
            else
            {
                _context.HoaDons.Remove(hd);
                await _context.SaveChangesAsync();
                return _mapper.Map<HoaDonDTO>(hd);
            }
        }


        public async Task<List<FullHoaDonDTO>> GetAll()
        {

            var hdlst = await _context.HoaDons.ToListAsync();
            if (!hdlst.Any())
            {
                return new List<FullHoaDonDTO>();
            }
            else
            {
                return _mapper.Map<List<FullHoaDonDTO>>(hdlst);
            }
        }

        public async Task<HoaDonDTO> GetById(int id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id không được để trống");
            }
            else
            {
                var hd = await _context.HoaDons.FindAsync(id);
                if (hd == null)
                {
                    throw new NullReferenceException("Hóa đơn không tồn tại");
                }
                else
                {
                    return _mapper.Map<HoaDonDTO>(hd);
                }
            }
        }

            public async Task<HoaDonDTO> Update(int id, HoaDonDTO obj)
            {
                if (id == null)
                {
                    throw new ArgumentNullException("id không được để trống");
                }
                else
                {
                    var hd = _context.HoaDons.FindAsync(id);
                    if (hd == null)
                    {
                        throw new NullReferenceException("Hóa đơn không tồn tại");
                    }
                    else
                    {
                        var newHD = _mapper.Map(hd, obj);
                        await _context.SaveChangesAsync();
                        return _mapper.Map<HoaDonDTO>(newHD);
                    }
                }
            }
    }


}

