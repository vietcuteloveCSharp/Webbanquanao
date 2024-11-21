using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.Context;
using DAL.Entities;
using DTO.Chucvus;
using DTO.VuvietanhDTO.Cuahangs;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.ICuahang;

namespace Service.VuVietAnhService.Repository.Cuahang
{   //hoàn thiện CRUD Serivce cửa hàng
    public class CuahangService : ICuahangService
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        public CuahangService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //add cửa hàng
        public async Task<CuahangDTO> AddCuahang(CuahangDTO cuahangDTO)
        {
            if (!await CuahangExists(cuahangDTO.Ten, cuahangDTO.Sdt))
            {
                throw new InvalidOperationException("Cửa hàng đã tồn tại.");
            }
            var newCuahang = _mapper.Map<CuaHang>(cuahangDTO);
            await _context.CuaHangs.AddAsync(newCuahang);
            await _context.SaveChangesAsync();
            // Map lại đối tượng Cuahang đã lưu thành CuahangDTO để trả về
            return _mapper.Map<CuahangDTO>(newCuahang);
        }

        public async Task<bool> CuahangExists(string? ten, string? sdt)
        {
            var cuahang = await _context.CuaHangs.AnyAsync(c => c.Ten == ten && c.Sdt == sdt);
            if (cuahang != null) return true;
            return false;
        }

        public async Task<bool> DeleteCuahang(int id)
        {

            // Tìm cửa hàng theo ID
            var cuahang = await _context.CuaHangs.FirstOrDefaultAsync(c => c.Id == id);
            if (cuahang != null)
            {
                _context.CuaHangs.Remove(cuahang);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Không tìm thấy cửa hàng với ID này.");


        }
        //get list cửa hàng
        public async Task<IEnumerable<CuahangDTO>> GetAllCuaHang()
        {
            var AllCuahang = await _context.CuaHangs.ToListAsync();
            if (!AllCuahang.Any()) return new List<CuahangDTO>();

            var AllCuaHangDTO = _mapper.Map<List<CuahangDTO>>(AllCuahang);
            return AllCuaHangDTO;
        }
        //tìm cửa hàng theo id
        public async Task<CuahangDTO> GetCuaHangById(int id)
        {
            var existingCuahang = await _context.CuaHangs.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCuahang == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy cửa hàng với ID {id}.");
            }
            var cuaHangDTO = _mapper.Map<CuahangDTO>(existingCuahang);
            return cuaHangDTO;
        }

        public async Task<CuahangDTO> UpdateCuahang(int id, CuahangDTO updateCuahangDTO)
        {

            // Tìm cửa hàng theo ID
            var existingCuahang = await _context.CuaHangs.FirstOrDefaultAsync(c => c.Id == id);
            // Nếu không tìm thấy, báo lỗi
            if (existingCuahang == null)
            {
                throw new KeyNotFoundException("Không tìm thấy cửa hàng với ID này.");
            }
            //check sdt xem có ở cửa hàng khác không
            if (await _context.CuaHangs.AnyAsync(c => c.Sdt == updateCuahangDTO.Sdt && c.Id != id))
            {
                throw new InvalidOperationException("Số điện thoại đã được sử dụng bởi cửa hàng khác.");
            }
            if (await _context.CuaHangs.AnyAsync(c => c.Email == updateCuahangDTO.Email && c.Id != id))
            {
                throw new InvalidOperationException("Email đã được sử dụng bởi cửa hàng khác.");
            }
            // Sử dụng AutoMapper để cập nhật các thuộc tính
            _mapper.Map(updateCuahangDTO, existingCuahang);

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            // Map lại đối tượng sau khi cập nhật sang DTO và trả về
            return _mapper.Map<CuahangDTO>(existingCuahang);
        }
    }
}
