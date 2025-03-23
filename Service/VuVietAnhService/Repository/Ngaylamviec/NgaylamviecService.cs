using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.Calamviecs;
using DTO.VuvietanhDTO.Ngaylamviecs;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.INgaylamviec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Ngaylamviec
{
    public class NgaylamviecService : INgaylamviecService
    {   public readonly WebBanQuanAoDbContext _context;
        public readonly IMapper _mapper;

        public NgaylamviecService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> CreateNgayLamviec(CreateNgayLamViecDTO createNgayLamViecDTO)
        {
            // Kiểm tra tham số đầu vào
            ArgumentNullException.ThrowIfNull(createNgayLamViecDTO);

            // Kiểm tra ngày làm việc đã tồn tại chưa
            var existingNgayLamViec = await _context.NgayLamviecs
                .FirstOrDefaultAsync(n => n.Ngay.Date == createNgayLamViecDTO.Ngay.Date);

            if (existingNgayLamViec != null)
            {
                throw new InvalidOperationException($"Ngày làm việc {createNgayLamViecDTO.Ngay:yyyy-MM-dd} đã tồn tại.");
            }



            var newNgayLamViec = _mapper.Map<NgayLamViec>(createNgayLamViecDTO);
            newNgayLamViec.Ngay = createNgayLamViecDTO.Ngay.Date;
            await _context.NgayLamviecs.AddAsync(newNgayLamViec);
            await _context.SaveChangesAsync();

            return true; // Thành công
        }


        //public async Task<bool> DeleteNgayLamViec(int id)
        //{
        //    try
        //    {
        //        // Tìm ngày làm việc cần xóa
        //        var ngayLamViec = await _context.NgayLamviecs
        //            .Include(n => n.CaLamViecs) // Load tất cả ca làm việc liên quan
        //            .FirstOrDefaultAsync(n => n.Id == id);

        //        // Nếu không tìm thấy, báo lỗi
        //        if (ngayLamViec == null)
        //            throw new KeyNotFoundException($"Không tìm thấy ngày làm việc với ID {id}");

        //        // **Không cho xóa nếu ngày làm việc là ngày trong quá khứ**
        //        if (ngayLamViec.Ngay < DateTime.Today)
        //            throw new InvalidOperationException("Không thể xóa ngày làm việc đã qua.");

        //        // Xóa tất cả ca làm việc của ngày này
        //        _context.CaLamViecs.RemoveRange(ngayLamViec.CaLamViecs);

        //        // Xóa ngày làm việc
        //        _context.NgayLamviecs.Remove(ngayLamViec);

        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log lỗi nếu cần
        //        Console.WriteLine($"Lỗi khi xóa ngày làm việc: {ex.Message}");
        //        return false;
        //    }
        //}

        public async Task<IEnumerable<NgayLamViecDTO>> GetAllNgayLamViec()
        {
            var ALLNgayLamViec = await _context.NgayLamviecs.ToListAsync();
            if (!ALLNgayLamViec.Any()) return new List<NgayLamViecDTO>(); //trả về list rỗng nếu không có
            var ALLNgayLamViecDTO = _mapper.Map<List<NgayLamViecDTO>>(ALLNgayLamViec);
            return ALLNgayLamViecDTO;
        }

        public async Task<NgayLamViecDTO> GetNgayLamViecById(int id)
        {
            var ngayLamViec = await _context.NgayLamviecs.FirstOrDefaultAsync(c => c.Id == id);
            if (ngayLamViec == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy ngày làm việc với ID {id}.");
            }
            var ngayLamViecDTO = _mapper.Map<NgayLamViecDTO>(ngayLamViec);
            return ngayLamViecDTO;
        }
        
    }
}
