using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Danhmucs;
using DTO.VuvietanhDTO.Sanphams;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.ISanpham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Sanpham
{
    public class SanphamService : ISanphamSerivce
    {   private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        public SanphamService(WebBanQuanAoDbContext context,IMapper mapper)
        {
            this._mapper = mapper;
            _context = context;
        }
        public async Task<CreatSanPhamDTO> CreateSanPham(CreatSanPhamDTO creatSanPhamDTO)
        {
            if (await _context.SanPhams.AnyAsync(c => c.Ten == creatSanPhamDTO.Ten))
            {
                throw new InvalidOperationException($"Tên:{creatSanPhamDTO.Ten} đã tồn tại.");

            }
            if (!await _context.DanhMucs.AnyAsync(dm => dm.Id == creatSanPhamDTO.Id_DanhMuc))
            {
                throw new InvalidOperationException("Danh mục không tồn tại.");
            }

            if (!await _context.ThuongHieus.AnyAsync(th => th.Id == creatSanPhamDTO.Id_ThuongHieu))
            {
                throw new InvalidOperationException("Thương hiệu không tồn tại.");
            }
            if (!decimal.TryParse(creatSanPhamDTO.Gia, out var giaDecimal))
            {
                throw new InvalidOperationException("Giá không hợp lệ.");
            }
            var newSanPham = _mapper.Map<SanPham>(creatSanPhamDTO);
            await _context.SanPhams.AddAsync(newSanPham);
            await _context.SaveChangesAsync();
            // Map lại đối tượng Sanpham đã lưu thành sanphamdto để trả về
            return _mapper.Map<CreatSanPhamDTO>(newSanPham);
        }

        public async Task<bool> DeleteSanPham(int id)
        {
            // Tìm sản phẩm dựa vào id
            var existingSanPham = await _context.SanPhams.FirstOrDefaultAsync(sp => sp.Id == id);

            // Nếu không tìm thấy sản phẩm
            if (existingSanPham == null)
            {
               throw new KeyNotFoundException("Sản phẩm không tồn tại.");
             
            }
            existingSanPham.TrangThai=false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SanPhamDTO>> GetAllSanPham()
        {
            var AllSanpham = await _context.SanPhams.ToListAsync();
            if (!AllSanpham.Any()) return new List<SanPhamDTO>();
            var AllSanPhamDTO = _mapper.Map<List<SanPhamDTO>>(AllSanpham);
            return AllSanPhamDTO;
        }

        public async Task<SanPhamDTO> GetSanPhamById(int id)
        {
            var existingSanPham = await _context.SanPhams.FirstOrDefaultAsync(c => c.Id == id);
            if (existingSanPham == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID:{id}.");
            }
            var sanPhamDTO = _mapper.Map<SanPhamDTO>(existingSanPham);
            return sanPhamDTO;
        }

        public async Task<UpdateSanPhamDTO> UpdateSanPham(int id, UpdateSanPhamDTO updateSanPhamDTO)
        {
            // Kiểm tra sự tồn tại của sản phẩm
            var existingSanPham = await _context.SanPhams.FirstOrDefaultAsync(sp => sp.Id == id);
            if (existingSanPham == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }
            if (await _context.SanPhams.AnyAsync(c => c.Ten == updateSanPhamDTO.Ten && c.Id != id))
            {
                throw new InvalidOperationException($"Tên Danh mục:{updateSanPhamDTO.Ten} đã được sử dụng.");
            }
            // Kiểm tra Danh Mục tồn tại
            if (!await _context.DanhMucs.AnyAsync(dm => dm.Id == updateSanPhamDTO.Id_DanhMuc))
            {
                throw new InvalidOperationException("Danh mục không tồn tại.");
            }
            // Kiểm tra Thương Hiệu tồn tại
            if (!await _context.ThuongHieus.AnyAsync(th => th.Id == updateSanPhamDTO.Id_ThuongHieu))
            {
                throw new InvalidOperationException("Thương hiệu không tồn tại.");
            }
            // Kiểm tra giá hợp lệ
            if (!decimal.TryParse(updateSanPhamDTO.Gia, out var giaDecimal))
            {
                throw new InvalidOperationException("Giá không hợp lệ.");
            }
            // Sử dụng AutoMapper để cập nhật các thuộc tính
            existingSanPham = _mapper.Map(updateSanPhamDTO, existingSanPham);

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            // Map lại đối tượng sau khi cập nhật sang DTO và trả về
            return _mapper.Map<UpdateSanPhamDTO>(existingSanPham);
        }

    }
}
