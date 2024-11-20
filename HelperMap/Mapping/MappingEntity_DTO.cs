using AutoMapper;
using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMap.Mapping
{
    public class MappingEntity_DTO:Profile
    {
        public MappingEntity_DTO()
        {
            CreateMap<DanhMuc, DanhMucDTO>()
               .ForMember(dto => dto.Id_DanhMucCha, entity => entity.MapFrom(src => src.Id_DanhMucCha))
               .ForMember(dto => dto.TenDanhMuc, entity => entity.MapFrom(src => src.TenDanhMuc))
               .ForMember(dto => dto.NgayTao, entity => entity.MapFrom(src => src.NgayTao))
               .ForMember(dto => dto.TrangThai, entity => entity.MapFrom(src => src.TrangThai));
            CreateMap<HoaDon, HoaDonDTO>()
                .ForMember(dto => dto.TongTien, entity => entity.MapFrom(src => src.TongTien))
                .ForMember(dto => dto.NgayTao, entity => entity.MapFrom(src => src.NgayTao))
                .ForMember(dto => dto.TrangThai, entity => entity.MapFrom(src => src.TrangThai))
                .ForMember(dto => dto.Id_KhachHang, entity => entity.MapFrom(src => src.Id_KhachHang))
                .ForMember(dto => dto.Id_NhanVien, entity => entity.MapFrom(src => src.Id_NhanVien));
            CreateMap<NhanVien, NhanVienDTO>()
                .ForMember(dto => dto.TaiKhoan, entity => entity.MapFrom(src => src.TaiKhoan))
                .ForMember(dto => dto.MatKhau, entity => entity.MapFrom(src => src.MatKhau))
                .ForMember(dto => dto.TenNhanVien, entity => entity.MapFrom(src => src.TenNhanVien))
                .ForMember(dto => dto.Sdt, entity => entity.MapFrom(src => src.Sdt))
                .ForMember(dto => dto.Email, entity => entity.MapFrom(src => src.Email))
                .ForMember(dto => dto.NgaySinh, entity => entity.MapFrom(src => src.NgaySinh))
                .ForMember(dto => dto.DiaChi, entity => entity.MapFrom(src => src.DiaChi))
                .ForMember(dto => dto.GhiChu, entity => entity.MapFrom(src => src.GhiChu))
                .ForMember(dto => dto.TrangThai, entity => entity.MapFrom(src => src.TrangThai))
                .ForMember(dto => dto.NgayTao, entity => entity.MapFrom(src => src.NgayTao))
                .ForMember(dto => dto.NgayCapNhat, entity => entity.MapFrom(src => src.NgayCapNhat))
                .ForMember(dto => dto.Id_ChucVu, entity => entity.MapFrom(src => src.Id_ChucVu));
            CreateMap<PhuongThucThanhToan, PhuongThucThanhToanDTO>()
                .ForMember(dto => dto.Ten, entity => entity.MapFrom(src => src.Ten))
                .ForMember(dto => dto.TrangThai, entity => entity.MapFrom(src => src.TrangThai))
                .ForMember(dto => dto.Mota, entity => entity.MapFrom(src => src.Mota))
                .ForMember(dto => dto.NgayTao, entity => entity.MapFrom(src => src.NgayTao));
            CreateMap<SanPham, SanPhamDTO>()
                .ForMember(dto => dto.Ten, entity => entity.MapFrom(src => src.Ten))
                .ForMember(dto => dto.MoTa, entity => entity.MapFrom(src => src.MoTa))
                .ForMember(dto => dto.Gia, entity => entity.MapFrom(src => src.Gia))
                .ForMember(dto => dto.SoLuong, entity => entity.MapFrom(src => src.SoLuong))
                .ForMember(dto => dto.NgayTao, entity => entity.MapFrom(src => src.NgayTao))
                .ForMember(dto => dto.NgayCapNhat, entity => entity.MapFrom(src => src.NgayCapNhat))
                .ForMember(dto => dto.TrangThai, entity => entity.MapFrom(src => src.TrangThai))
                .ForMember(dto => dto.HinhAnh, entity => entity.MapFrom(src => src.HinhAnh))
                .ForMember(dto => dto.Id_DanhMuc, entity => entity.MapFrom(src => src.Id_DanhMuc))
                .ForMember(dto => dto.Id_ThuongHieu, entity => entity.MapFrom(src => src.Id_ThuongHieu));
            CreateMap<ThanhToanHoaDon, ThanhToanHoaDonDTO >()
                 .ForMember(dto => dto.TongTien, entity => entity.MapFrom(src => src.TongTien))
                 .ForMember(dto => dto.SoTienDaThanhToan, entity => entity.MapFrom(src => src.SoTienDaThanhToan))
                 .ForMember(dto => dto.NgayThanhToan, entity => entity.MapFrom(src => src.NgayThanhToan))
                 .ForMember(dto => dto.MaGiaoDich, entity => entity.MapFrom(src => src.MaGiaoDich))
                 .ForMember(dto => dto.Id_HoaDon, entity => entity.MapFrom(src => src.Id_HoaDon))
                 .ForMember(dto => dto.Id_PhuongThucThanhToan, entity => entity.MapFrom(src => src.Id_PhuongThucThanhToan));
            CreateMap<ChucVu, ChucVuDTO>()
                .ForMember(dto => dto.Ten, entity => entity.MapFrom(src => src.Ten))
                .ForMember(dto => dto.Mota, entity => entity.MapFrom(src => src.Mota));

        }
    }
}
