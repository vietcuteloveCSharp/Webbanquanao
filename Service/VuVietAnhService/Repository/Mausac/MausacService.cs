using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Kichthuocs;
using DTO.VuvietanhDTO.Mausacs;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.IMausac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Mausac
{
    public class MausacService : IMausacService
    {   private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        public MausacService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            this._mapper = mapper;
            _context = context;
        }
        public async Task<MauSacDTO> AddMauSac(MauSacDTO mauSacDTO)
        {
            // Kiểm tra tên trùng
            if (await _context.MauSacs.AnyAsync(ms => ms.Ten == mauSacDTO.Ten))
            {
                throw new InvalidOperationException("Tên màu sắc đã tồn tại.");
            }

            // Thêm mới
            var newMauSac = _mapper.Map<MauSac>(mauSacDTO);
            await _context.MauSacs.AddAsync(newMauSac);
            await _context.SaveChangesAsync();

            return _mapper.Map<MauSacDTO>(newMauSac);
        }
        
        public async Task<bool> DeleteMauSac(int id)
        {
            // Kiểm tra tồn tại
            var existingMauSac = await _context.MauSacs.FirstOrDefaultAsync(ms => ms.Id == id);
            if (existingMauSac == null)
            {
                throw new KeyNotFoundException("Màu sắc không tồn tại.");
            }

            // Xóa cứng
            _context.MauSacs.Remove(existingMauSac); // Xóa cứng
            await _context.SaveChangesAsync();
            return true;
        }
        //get all 
        public async Task<IEnumerable<FullMauSacDTO>> GetAllMausac()
        {
            var mauSacs = await _context.MauSacs.ToListAsync();
            if (!mauSacs.Any()) return new List<FullMauSacDTO>();
            var AllMauSacDTO = _mapper.Map<List<FullMauSacDTO>>(mauSacs);
            return AllMauSacDTO;
        }
        //get mausac by id
        public async Task<MauSacDTO> GetMauSacById(int id)
        {
            // Tìm màu sắc
            var mauSac = await _context.MauSacs.FirstOrDefaultAsync(ms => ms.Id == id);
            if (mauSac == null)
            {
                throw new KeyNotFoundException("Màu sắc không tồn tại.");
            }

            return _mapper.Map<MauSacDTO>(mauSac);
        }
        //cập nhật màu sắc
        public async Task<MauSacDTO> UpdateMauSac(int id, MauSacDTO mauSacDTO)
        {
            // Kiểm tra tồn tại
            var existingMauSac = await _context.MauSacs.FirstOrDefaultAsync(ms => ms.Id == id);
            if (existingMauSac == null)
            {
                throw new KeyNotFoundException("Màu sắc không tồn tại.");
            }

            // Kiểm tra tên trùng
            if (await _context.MauSacs.AnyAsync(c => c.Ten == mauSacDTO.Ten && c.Id != id))
            {
                throw new InvalidOperationException($"Tên màu{mauSacDTO.Ten} đã sử dụng.");
            }
            existingMauSac = _mapper.Map(mauSacDTO, existingMauSac);
            await _context.SaveChangesAsync();

            return _mapper.Map<MauSacDTO>(existingMauSac);

        }
    }
}
