using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Kichthuocs;
using DTO.VuvietanhDTO.Mausacs;
using DTO.VuvietanhDTO.Sanphams;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.IKichthuoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Kichthuoc
{
    public class Kichthuocservice : IKichthuocService
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        public Kichthuocservice(WebBanQuanAoDbContext context, IMapper mapper)
        {
            this._mapper = mapper;
            _context = context;
        }
        public async Task<KichThuocDTO> AddKichThuoc(KichThuocDTO kichThuocDTO)
        {
            if (await _context.KichThuocs.AnyAsync(ms => ms.Ten == kichThuocDTO.Ten))
            {
                throw new InvalidOperationException($"Tên kích thước{kichThuocDTO.Ten} đã tồn tại.");
            }

            // Thêm mới
            var newKichThuoc = _mapper.Map<KichThuoc>(kichThuocDTO);
            await _context.KichThuocs.AddAsync(newKichThuoc);
            await _context.SaveChangesAsync();

            return _mapper.Map<KichThuocDTO>(newKichThuoc);
        }

        public Task<bool> DeleteKichThuoc(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<KichThuocDTO>> GetAllKichThuoc()
        {
            var AllKichthuoc = await _context.KichThuocs.ToListAsync();
            if (!AllKichthuoc.Any()) return new List<KichThuocDTO>();
            var AllKichthuocDTO = _mapper.Map<List<KichThuocDTO>>(AllKichthuoc);
            return AllKichthuocDTO;
        }

        public async Task<KichThuocDTO> GetKickThuocId(int id)
        {
            var existingKichthuoc = await _context.KichThuocs.FirstOrDefaultAsync(c => c.Id == id);
            if (existingKichthuoc == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID:{id}.");
            }
            var kichThuocDTO = _mapper.Map<KichThuocDTO>(existingKichthuoc);
            return kichThuocDTO;
        }

        public async Task<KichThuocDTO> UpdateKichThuoc(int id, KichThuocDTO kichThuocDTO)
        {
            // Kiểm tra xem bản ghi có tồn tại không
            var existingKichThuoc = await _context.KichThuocs.FirstOrDefaultAsync(kt => kt.Id == id);
            if (existingKichThuoc == null)
            {
                throw new KeyNotFoundException("Kích thước không tồn tại.");
            }

            // Kiểm tra xem tên đã được sử dụng chưa (loại trừ bản ghi hiện tại)
            if (await _context.KichThuocs.AnyAsync(kt => kt.Ten == kichThuocDTO.Ten && kt.Id != id))
            {
                throw new InvalidOperationException($"Tên kích thước '{kichThuocDTO.Ten}' đã được sử dụng.");
            }

            // Cập nhật giá trị
            existingKichThuoc= _mapper.Map(kichThuocDTO, existingKichThuoc);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về DTO đã cập nhật
            return _mapper.Map<KichThuocDTO>(existingKichThuoc);
        }
    }
}
