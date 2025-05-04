using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Calamviecs;
using DTO.VuvietanhDTO.Canhanviens;
using DTO.VuvietanhDTO.NhanViens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.VuVietAnhService.IRepository.ICanhanvien;
using Service.VuVietAnhService.Repository.Calamviec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Canhanvien
{
    public class CanhanvienService : ICanhanvienService
    {
        private WebBanQuanAoDbContext _context;
        private IMapper _mapper;
        private readonly ILogger<CalamviecService> _logger;
        public CanhanvienService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<bool> CreatCanhanvien(int idNhanVien, int idCaLamViec)
        {
            // Kiểm tra xem nhân viên đã có trong ca chưa
            bool exists = await _context.Canhanviens
                .AnyAsync(cnv => cnv.IdNhanVien == idNhanVien && cnv.IdCaLamViec == idCaLamViec);
            if (exists) return false;
            var caNhanVien = new CaNhanVien
            {
                IdNhanVien = idNhanVien,
                IdCaLamViec = idCaLamViec
            };

            _context.Canhanviens.Add(caNhanVien);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<CanhanviensDTO>> GetAllCanhanvien()
        {
            // Lấy toàn bộ danh sách nhân viên từ database
                var allNhanVien = await _context.Canhanviens.ToListAsync();

            // Kiểm tra nếu không có nhân viên nào thì trả về danh sách rỗng
            if (!allNhanVien.Any()) return new List<CanhanviensDTO>();

            // Dùng AutoMapper để map sang DTO
            var allNhanVienDTO = _mapper.Map<List<CanhanviensDTO>>(allNhanVien);

            return allNhanVienDTO;
        }     

        public async Task<bool> UpdateCanhanvien(DoiCaDTO doiCaDTO)
        {
            // Lấy ca làm việc của nhân viên 1 trong ngày đó
            var caNV1 = await _context.Canhanviens
                .Include(c => c.CaLamViec)
                .FirstOrDefaultAsync(c =>
                    c.IdNhanVien == doiCaDTO.IdNhanVien1 &&
                    c.CaLamViec.IdNgaylamviec == doiCaDTO.IdNgayLamViec);

            // Lấy ca làm việc của nhân viên 2 trong cùng ngày
            var caNV2 = await _context.Canhanviens
                .Include(c => c.CaLamViec)
                .FirstOrDefaultAsync(c =>
                    c.IdNhanVien == doiCaDTO.IdNhanVien2 &&
                    c.CaLamViec.IdNgaylamviec == doiCaDTO.IdNgayLamViec);
            // Kiểm tra tồn tại
            if (caNV1 == null || caNV2 == null)
                return false;
            // Lấy thông tin nhân viên để kiểm tra chức vụ
            var nhanVien1 = await _context.NhanViens.FindAsync(doiCaDTO.IdNhanVien1);
            var nhanVien2 = await _context.NhanViens.FindAsync(doiCaDTO.IdNhanVien2);

            if (nhanVien1 == null || nhanVien2 == null) return false;
            if (nhanVien1.Id_ChucVu != nhanVien2.Id_ChucVu)
                return false; // Không cùng chức vụ → không cho đổi

            // Lấy thông tin ca làm việc mới
            // Đổi ca làm việc cho nhau
            var temp = caNV1.IdCaLamViec;
            caNV1.IdCaLamViec = caNV2.IdCaLamViec;
            caNV2.IdCaLamViec = temp;

            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task<IEnumerable<NhanvienDTO>> GetNhanVienByCa(int idCaLamViec)
        {
            var nhanViens = await _context.Canhanviens
            .Where(cnv => cnv.IdCaLamViec == idCaLamViec)   
            .Select(cnv => cnv.NhanVien)
            .ToListAsync();
            var data = _mapper.Map<IEnumerable<NhanvienDTO>>(nhanViens);
            return data;
        }

        public async Task<IEnumerable<CaLamViecDTO>> GetCaLamViecByNhanVien(int idNhanVien)
        {
            var caLamViecs = await _context.Canhanviens
                 .Where(cnv => cnv.IdNhanVien == idNhanVien)
                .Select(cnv => cnv.CaLamViec)
                .ToListAsync();
            var data = _mapper.Map<IEnumerable<CaLamViecDTO>>(caLamViecs);
            return data;
        }
    }
}
