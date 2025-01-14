﻿using AutoMapper;
using DAL.Context;
using DAL.Entities;
using DTO.VuvietanhDTO.HoadonsDTO;
using Enum.EnumVVA;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.IHoadon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.Repository.Hoadon
{
    public class HoadonService : IHoadonService
    {
        private readonly WebBanQuanAoDbContext _context;
        private readonly IMapper _mapper;
        public HoadonService(WebBanQuanAoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<HoaDonDTO> GetHoaDonById(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy hóa đơn với ID = {id}");
            }

            // Sử dụng AutoMapper để ánh xạ sang DTO
            return _mapper.Map<HoaDonDTO>(hoaDon);
        }

        public async Task<UpdateTrangThaiDTO> UpdateTrangThai(int id, ETrangThaiHD nextTrangThaiHD)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy hóa đơn với ID = {id}");
            }
            // Kiểm tra xem trạng thái chuyển có hợp lệ không
            // Kiểm tra trạng thái chuyển đổi, bắn exception nếu không hợp lệ
            IsValidTrangThaiTransition(hoaDon.TrangThai, nextTrangThaiHD);
            // Cập nhật trạng thái mới
            hoaDon.TrangThai = nextTrangThaiHD;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();

            // Trả về DTO cập nhật
            // Sử dụng AutoMapper để ánh xạ từ HoaDon sang UpdateTrangThaiDTO
            var updateResult = _mapper.Map<UpdateTrangThaiDTO>(hoaDon);

            return updateResult;
        }
        public void IsValidTrangThaiTransition(ETrangThaiHD current, ETrangThaiHD next)
        {
            // Ngăn không cho quay lại trạng thái trước
            if ((int)next <= (int)current)
            {
                throw new InvalidOperationException(
                $"Không thể chuyển trạng thái từ '{current}' sang '{next}'.");
            }
            var isValid = current switch
            {
                //HoanThanhDon = 5,      // Đơn hàng đã hoàn thành
                //HuyDon = 6,            // Đơn hàng bị hủy
                //                       // Trạng thái dành riêng cho bán online
                //ChoXacNhan = 1,        // Đơn hàng chờ xác nhận từ người bán
                //ChoThanhToan = 2,      // Đơn hàng chờ thanh toán 
                //DaXacNhan = 3,         // Đơn hàng đang được vận chuyển (đã thanh toán trước)
                //DangVanChuyen = 4,     // Đơn hàng  đang được vận chuyển
              

                // Đã thanh toán: có thể chuyển sang "Hoàn thành đơn"
                ETrangThaiHD.DangVanChuyen =>
                    next == ETrangThaiHD.HoanThanhDon,

                // Chờ xác nhận: Có thể chuyển sang "Chờ thanh toán", "Đang vận chuyển COD" hoặc "Hủy đơn"
                ETrangThaiHD.ChoXacNhan =>
                    next == ETrangThaiHD.DaXacNhan ||
                    next == ETrangThaiHD.DangVanChuyen ||
                    next == ETrangThaiHD.HoanThanhDon ||
                    next == ETrangThaiHD.HuyDon,

                // Chờ thanh toán: Có thể chuyển sang "Đang vận chuyển" hoặc "Hủy đơn"
                ETrangThaiHD.ChoThanhToan =>
                    next == ETrangThaiHD.DaXacNhan ||
                    next == ETrangThaiHD.DangVanChuyen ||
                    next == ETrangThaiHD.HoanThanhDon ||
                    next == ETrangThaiHD.HuyDon,

                // Hoàn thành đơn: Không thể chuyển tiếp
                ETrangThaiHD.HoanThanhDon => false,
                // Hủy đơn: Không thể chuyển tiếp
                ETrangThaiHD.HuyDon => false,
                // Mặc định
                _ => false
            };
            if (!isValid)
            {
                throw new InvalidOperationException(
                    $"Chuyển trạng thái từ '{current}' sang '{next}' không hợp lệ."
                );
            }
        }

        public async Task<IEnumerable<FullHoaDonDTO>> GetAllHoaDon()
        {
            var AllHoaDon = await _context.HoaDons.ToListAsync();
            if (!AllHoaDon.Any()) return new List<FullHoaDonDTO>();
            var AllHoaDonDTO = _mapper.Map<List<FullHoaDonDTO>>(AllHoaDon);
            return AllHoaDonDTO;
        }
    }
}

