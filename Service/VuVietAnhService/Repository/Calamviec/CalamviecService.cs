using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Calamviecs;
using DTO.VuvietanhDTO.Chucvus;
using Enum.EnumVVA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.VuVietAnhService.IRepository.ICalamviec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.DataSeed.EnumableClass;

namespace Service.VuVietAnhService.Repository.Calamviec
{
    public class CalamviecService :ICalamviecService
    {
        private WebBanQuanAoDbContext _context;
        private IMapper _mapper;
        private readonly ILogger<CalamviecService> _logger;
        public CalamviecService(WebBanQuanAoDbContext context, IMapper _mapper, ILogger<CalamviecService> logger)
        {
            this._context = context;
            this._mapper = _mapper;
            this._logger = logger;
        }
        //mặc định gán giờ ở dưới vào tên ca
        public static CaLamViec TaocalamviecInstance(EnumTenCa tenCa)
        {
            var (gioBatDau, gioKetThuc) = GetGioLamViec(tenCa);
            return new CaLamViec
            {
                TenCa = tenCa,
                GioBatDau = gioBatDau,
                GioKetThuc = gioKetThuc,
              
                
            };
        }
        // set giờ cho enum ca
        public static (TimeSpan, TimeSpan) GetGioLamViec(EnumTenCa tenCa)
        {
            return tenCa switch
            {
                EnumTenCa.CaSang => (new TimeSpan(07, 0, 0), new TimeSpan(12, 0, 0)),
                EnumTenCa.CaChieu => (new TimeSpan(12, 0, 1), new TimeSpan(17, 0, 0)),
                EnumTenCa.CaToi => (new TimeSpan(17, 0, 1), new TimeSpan(22, 0, 0)),
                _ => throw new ArgumentException("loại ca không hợp lệ")
            };
         }

        public async Task<IEnumerable<FullCaLamViecDTO>> GetAllCaLamViec()
        {
            var ALLCaLamViec = await _context.CaLamViecs.ToListAsync();
            if (!ALLCaLamViec.Any()) return new List<FullCaLamViecDTO>(); //trả về list rỗng nếu không có
             var ALLCaLamViecDTO = _mapper.Map<List<FullCaLamViecDTO>>(ALLCaLamViec);
            return ALLCaLamViecDTO;
        }

        public async Task<CaLamViecDTO> GetCaLamViecById(int id)
        {
            var caLamViec = await _context.CaLamViecs.FirstOrDefaultAsync(c => c.Id == id);
            if (caLamViec == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy ca làm việc với ID {id}.");
            }
            var caLamViecDTO = _mapper.Map<CaLamViecDTO>(caLamViec);
            return caLamViecDTO;
        }

        public async Task<bool> CreateCaLamViec(CreateCaLamViecDTO createCaLamViecDTO)
        {
            ArgumentNullException.ThrowIfNull(createCaLamViecDTO);
            try
            {
                var caLamViec = TaocalamviecInstance(createCaLamViecDTO.TenCa);
                caLamViec.Id_NhanVien = createCaLamViecDTO.Id_NhanVien;
                await _context.CaLamViecs.AddAsync(caLamViec);
                await _context.SaveChangesAsync();
                return true;  // Thành công
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Lỗi database: {dbEx.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Lỗi không xác định khi tạo ca làm việc. DTO: {@DTO}", createCaLamViecDTO);
                throw;
            }
        }

        public async Task<bool> UpdateCaLamViec(int id, UpdateCaLamViecDTO updateCaLamViecDTO)
        {
            try
            {
                var existingCalamviec = await _context.CaLamViecs.FirstOrDefaultAsync(c => c.Id == id);
                // Nếu không tìm thấy, báo lỗi
                if (existingCalamviec == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy chức vụ với ID {id}.");
                }
                var caLamViec = TaocalamviecInstance(existingCalamviec.TenCa);

                // Sử dụng AutoMapper để cập nhật các thuộc tính
                _mapper.Map(updateCaLamViecDTO, existingCalamviec);

                var (gioBatDau, gioKetThuc) = GetGioLamViec(existingCalamviec.TenCa);
                existingCalamviec.GioBatDau = gioBatDau;
                existingCalamviec.GioKetThuc = gioKetThuc;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();

                // Map lại đối tượng sau khi cập nhật sang DTO và trả về
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi update ca làm việc. DTO: {@DTO}", updateCaLamViecDTO);
                
                throw;
            }
            
            
        }

        public async Task<bool> DoiCaLamViec(int Idnv1, int Idnv2)
        {
            // tìm nv xem có cùng role không
            try
            {
                var nv1 = await _context.NhanViens.Include(nv => nv.CaLamViecs).FirstOrDefaultAsync(nv => nv.Id == Idnv1);
                var nv2 = await _context.NhanViens.Include(nv => nv.CaLamViecs).FirstOrDefaultAsync(nv => nv.Id == Idnv2);
                // Kiểm tra nếu không tìm thấy nhân viên nào
                if (nv1 == null || nv2 == null)
                {
                    throw new KeyNotFoundException("Không tìm thấy nhân viên.");
                }
                // Kiểm tra nếu 2 nhân viên có cùng chức vụ
                if (nv1.Id_ChucVu != nv2.Id_ChucVu)
                {
                    throw new InvalidOperationException("Hai nhân viên phải có cùng chức vụ mới được đổi ca.");
                }
                // Đổi ca làm việc
                var tempCa = nv1.CaLamViecs;
                nv1.CaLamViecs = nv2.CaLamViecs;
                nv2.CaLamViecs = tempCa;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đổi ca làm việc");
                return false;
            }
        }
    }
}
