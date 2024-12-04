using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Kichthuocs;
using DTO.VuvietanhDTO.Thuonghieus;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.IThuonghieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Thuonghieu
{
    public class ThuonghieuService : IThuonghieuSerivce
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        public ThuonghieuService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<CreateThuongHieuDTO> AddThuongHieu(CreateThuongHieuDTO createThuongHieuDTO)
        {
            if (await _context.ThuongHieus.AnyAsync(ms => ms.Ten == createThuongHieuDTO.Ten))
            {
                throw new InvalidOperationException($"Tên thương hiệu {createThuongHieuDTO.Ten} đã tồn tại.");
            }

            // Thêm mới
            var newThuongHieu = _mapper.Map<ThuongHieu>(createThuongHieuDTO);
            await _context.ThuongHieus.AddAsync(newThuongHieu);
            await _context.SaveChangesAsync();

            return _mapper.Map<CreateThuongHieuDTO>(newThuongHieu);
        }

        public async Task<bool> DeleteThuongHieu(int id)
        {
            // Tìm cửa hàng theo ID
            var existingThuonghieu = await _context.ThuongHieus.FirstOrDefaultAsync(c => c.Id == id);
            if (existingThuonghieu != null)
            {
                existingThuonghieu.TrangThai = false;
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException(nameof(existingThuonghieu), new($"Không tìm thấy thương hiệu với ID {id}."));
        }

        public async Task<IEnumerable<FullThuongHieuDTO>> GetAllThuongHieu()
        {
            var AllThuongHieu = await _context.ThuongHieus.ToListAsync();
            if (!AllThuongHieu.Any()) return new List<FullThuongHieuDTO>();
            var AllThuongHieuDTO = _mapper.Map<List<FullThuongHieuDTO>>(AllThuongHieu);
            return AllThuongHieuDTO;
        }


        public async Task<ThuongHieuDTO> GetThuongHieuById(int id)
        {
            var existingThuongHieu = await _context.ThuongHieus.FirstOrDefaultAsync(c => c.Id == id);
            if (existingThuongHieu == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy thương thiệu với ID:{id}.");
            }
            var thuongHieuDTO = _mapper.Map<ThuongHieuDTO>(existingThuongHieu);
            return thuongHieuDTO;
        }

        public async Task<ThuongHieuDTO> UpdateThuongHieu(int id , ThuongHieuDTO thuongHieuDTO)
        {
            // Kiểm tra xem bản ghi có tồn tại không
            var existingThuongHieu = await _context.ThuongHieus.FirstOrDefaultAsync(th => th.Id == id);
            if (existingThuongHieu == null)
            {
                throw new KeyNotFoundException("Thương hiệu không tồn tại.");
            }

            // Kiểm tra xem tên đã được sử dụng chưa (loại trừ bản ghi hiện tại)
            if (await _context.ThuongHieus.AnyAsync(kt => kt.Ten == thuongHieuDTO.Ten && kt.Id != id))
            {
                throw new InvalidOperationException($"Tên thương hiệu '{thuongHieuDTO.Ten}' đã được sử dụng.");
            }

            // Cập nhật giá trị
            existingThuongHieu = _mapper.Map(thuongHieuDTO,existingThuongHieu);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về DTO đã cập nhật
            return _mapper.Map<ThuongHieuDTO>(existingThuongHieu);
        }
        public async Task<int> GetIdByTenThuongHieu(string tenThuongHieu)
        {
            var thuongHieu = await _context.ThuongHieus.FirstOrDefaultAsync(th => th.Ten == tenThuongHieu);
            ArgumentNullException.ThrowIfNull(thuongHieu, $"Thương hiệu '{tenThuongHieu}' không tồn tại.");
            return thuongHieu?.Id ?? 0;
        }

    }
}
