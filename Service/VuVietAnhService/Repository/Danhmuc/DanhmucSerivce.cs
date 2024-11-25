using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Chucvus;
using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Danhmucs;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.IDanhmuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Danhmuc
{
    public class DanhmucSerivce : IDanhmucService
    {
        private readonly WebBanQuanAoDbContext _context ;
        private readonly IMapper _mapper ;
        public DanhmucSerivce(WebBanQuanAoDbContext context,IMapper mapper)
        {
            this._context = context; 
            this._mapper = mapper;
        }
        public async Task<CreatDanhMucDTO> CreateDanhMuc(CreatDanhMucDTO creatDanhMucDTO)
        {
            if (await _context.DanhMucs.AnyAsync(c => c.TenDanhMuc == creatDanhMucDTO.TenDanhMuc))
            {
                throw new InvalidOperationException("Tên danh mục đã tồn tại.");

            }
            var newDanhMuc = _mapper.Map<DanhMuc>(creatDanhMucDTO);
            await _context.DanhMucs.AddAsync(newDanhMuc);
            await _context.SaveChangesAsync();
            // Map lại đối tượng ChucVu đã lưu thành ChucvuDTO để trả về
            return _mapper.Map<CreatDanhMucDTO>(newDanhMuc);
        }

        public Task<bool> DeleteDanhMuc(int id)
        {
            throw new NotImplementedException();
        }
        //get all danh mục
        public async Task<IEnumerable<DanhMucDTO>> GetAllDanhMuc()
        {
            var AllDanhmuc = await _context.DanhMucs.ToListAsync();
            if (!AllDanhmuc.Any()) return new List<DanhMucDTO>();
            var AllDanhMucDTO = _mapper.Map<List<DanhMucDTO>>(AllDanhmuc);
            return AllDanhMucDTO;
        }
        //tìm danh mục theo id
        public async Task<DanhMucDTO> GetDanhMucById(int id)
        {
            var existingDanhMuc = await _context.DanhMucs.FirstOrDefaultAsync(c => c.Id == id);
            if (existingDanhMuc == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy Danh mục với ID:{id}.");
            }
            var danhMucDTO = _mapper.Map<DanhMucDTO>(existingDanhMuc);
            return danhMucDTO;
        }

        public async Task<UpdateDanhMucDTO> UpdateDanhMuc(int id, UpdateDanhMucDTO updateDanhMucDTO)
        {
            // Tìm danh mục theo ID
            var existingDanhMuc = await _context.DanhMucs.FirstOrDefaultAsync(c => c.Id == id);
            // Nếu không tìm thấy, báo lỗi
            if (existingDanhMuc == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy danh mục với ID:{id}.");
            }
            if (await _context.DanhMucs.AnyAsync(c => c.TenDanhMuc == updateDanhMucDTO.TenDanhMuc && c.Id != id))
            {
                throw new InvalidOperationException("Tên Danh mục đã được sử dụng.");
            }
            // Sử dụng AutoMapper để cập nhật các thuộc tính
            existingDanhMuc= _mapper.Map(updateDanhMucDTO, existingDanhMuc);

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            // Map lại đối tượng sau khi cập nhật sang DTO và trả về
            return _mapper.Map<UpdateDanhMucDTO>(existingDanhMuc);

        }
    }
}
