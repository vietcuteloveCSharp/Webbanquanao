using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Chucvus;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.IChucvu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Chucvu
{
    public class ChucvuService : IChucvuService
    {
        private WebBanQuanAoDbContext _context;
        private IMapper _mapper;
        public ChucvuService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //lấy tên chức vụ = id
        public async Task<string> GetTenChucVuById(int idChucVu)
        {
            var chucVu = await _context.ChucVus.FirstOrDefaultAsync(c => c.Id == idChucVu);
            return chucVu?.Ten ?? "unknow";
        }
        //thêm chức vụ
        public async Task<ChucvuDTO> AddChucVu(ChucvuDTO chucvuDTO)
        {   //check tên tồn tại
            if (await _context.ChucVus.AnyAsync(c => c.Ten == chucvuDTO.Ten))
            {
                throw new InvalidOperationException($"Tên chức vụ {chucvuDTO.Ten} đã tồn tại.");

            }
            var newChucVu = _mapper.Map<ChucVu>(chucvuDTO);
            await _context.ChucVus.AddAsync(newChucVu);
            await _context.SaveChangesAsync();
            // Map lại đối tượng ChucVu đã lưu thành ChucvuDTO để trả về
            return _mapper.Map<ChucvuDTO>(newChucVu);

        }
        //xoá
        public async Task<bool> DeleteChucVu(int id)
        {
            // Tìm cửa hàng theo ID
            var existingChucvu = await _context.ChucVus.FirstOrDefaultAsync(c => c.Id == id);
            if (existingChucvu != null)
            {
                _context.ChucVus.Remove(existingChucvu);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException(nameof(existingChucvu), new($"Không tìm thấy chức vụ với ID {id}."));
        }


        public async Task<IEnumerable<FullChucVuDTO>> GetAllChucVu()
        {
            var AllChucVu = await _context.ChucVus.ToListAsync();
            if (!AllChucVu.Any()) return new List<FullChucVuDTO>(); //trả về list rỗng nếu không có
            var AllChucVuDTO = _mapper.Map<List<FullChucVuDTO>>(AllChucVu);
            return AllChucVuDTO;
        }
        // lấy chucvu theo id
        public async Task<ChucvuDTO> GetChucVuById(int id)
        {
            var chucVu = await _context.ChucVus.FirstOrDefaultAsync(c => c.Id == id);
            if (chucVu == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy chức vụ với ID {id}.");
            }
            var chucVuDTO = _mapper.Map<ChucvuDTO>(chucVu);
            return chucVuDTO;
        }
        //cập nhật chức vụ
        public async Task<ChucvuDTO> UpdateChucVu(int id, ChucvuDTO updateChucvuDTO)
        {

            // Tìm chức vụ theo ID
            var existingChucvu = await _context.ChucVus.FirstOrDefaultAsync(c => c.Id == id);
            // Nếu không tìm thấy, báo lỗi
            if (existingChucvu == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy chức vụ với ID {id}.");
            }
            if (await _context.ChucVus.AnyAsync(c => c.Ten == updateChucvuDTO.Ten && c.Id != id))
            {
                throw new InvalidOperationException($"Tên chức vụ{updateChucvuDTO.Ten} đã được sử dụng.");
            }
            // Sử dụng AutoMapper để cập nhật các thuộc tính
            existingChucvu = _mapper.Map(updateChucvuDTO, existingChucvu);

            // Lưu thay đổi vào database
             await _context.SaveChangesAsync();

            // Map lại đối tượng sau khi cập nhật sang DTO và trả về
            return _mapper.Map<ChucvuDTO>(existingChucvu);

        }
    }
}
