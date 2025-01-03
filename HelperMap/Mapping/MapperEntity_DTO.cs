using AutoMapper;
using DAL.Entities;
using DTO.NTTuyen.HoaDons;
using DTO.VuvietanhDTO.Chucvus;
using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Danhmucs;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.Kichthuocs;
using DTO.VuvietanhDTO.Mausacs;
using DTO.VuvietanhDTO.NhanViens;
using DTO.VuvietanhDTO.Sanphams;
using DTO.VuvietanhDTO.Thuonghieus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperMap.Mapping
{
    public class MapperEntity_DTO : Profile
    {
       
       
        public MapperEntity_DTO()
        {
            

            #region Map Nhanvien
            CreateMap<NhanVien, NhanvienDTO>()
             .ForMember(dest => dest.TaiKhoan, opt => opt.MapFrom(src => src.TaiKhoan))
             .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.TenNhanVien))
             .ForMember(dest => dest.Id_ChucVu, opt => opt.MapFrom(src => src.Id_ChucVu));

            #endregion
            #region MapKhachhang
            CreateMap<KhachHang, KhachhangDTO>()
               .ForMember(dest => dest.TaiKhoan, opt => opt.MapFrom(src => src.TaiKhoan))
               .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
               .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            #endregion
            #region Map Chucvu
            CreateMap<ChucVu, ChucvuDTO>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.Mota, opt => opt.MapFrom(src => src.Mota));
            CreateMap<ChucVu, FullChucVuDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
               .ForMember(dest => dest.Mota, opt => opt.MapFrom(src => src.Mota));

            #endregion
            #region Map Cuahang
            CreateMap<CuaHang, CuahangDTO>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DiaChi, opt => opt.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao));
            CreateMap<CuaHang, FullCuahangDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DiaChi, opt => opt.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao));

            #endregion
            #region Map SanPham
            CreateMap<SanPham, SanPhamDTO>()
               .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
               .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
               .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
               .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
               .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
               .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
               .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
               .ForMember(dest => dest.Id_ThuongHieu, opt => opt.MapFrom(src => src.Id_ThuongHieu))
               .ForMember(dest => dest.Id_DanhMuc, opt => opt.MapFrom(src => src.Id_DanhMuc)); 
            CreateMap<SanPham, FullSanPhamDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
               .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
               .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
               .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
               .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
               .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
               .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
               .ForMember(dest => dest.Id_ThuongHieu, opt => opt.MapFrom(src => src.Id_ThuongHieu))
               .ForMember(dest => dest.Id_DanhMuc, opt => opt.MapFrom(src => src.Id_DanhMuc));
            CreateMap<SanPham, CreatSanPhamDTO>()
                 .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                 .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                 .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
                 .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
                 .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                 .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                 .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
                 .ForMember(dest => dest.Id_ThuongHieu, opt => opt.MapFrom(src => src.Id_ThuongHieu))
                 .ForMember(dest => dest.Id_DanhMuc, opt => opt.MapFrom(src => src.Id_DanhMuc));
                 

            #endregion
            #region Map mausac
            CreateMap<MauSac, MauSacDTO>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MaHex, opt => opt.MapFrom(src => src.MaHex));
            CreateMap<MauSac, FullMauSacDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MaHex, opt => opt.MapFrom(src => src.MaHex));
            #endregion
            #region Map Danhmuc
            CreateMap<DanhMuc, DanhMucDTO>()
               .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
               .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
               .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
               .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
            CreateMap<DanhMuc, FullDanhMucDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
               .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
               .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
               .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
               
            CreateMap<DanhMuc, UpdateDanhMucDTO>()
              .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
              .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
              .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
            CreateMap<DanhMuc, CreatDanhMucDTO>()
             .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
             .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
             .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao));
            #endregion
            #region Map Thuonghieu
            CreateMap<ThuongHieu, ThuongHieuDTO>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
            CreateMap<ThuongHieu,FullThuongHieuDTO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                 .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                 .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
            CreateMap<ThuongHieu,CreateThuongHieuDTO>()
                 .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                 .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa));
            #endregion
            #region Map Kichthuoc
            CreateMap<KichThuoc, KichThuocDTO>()
              .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten));
            CreateMap<KichThuoc, FullKichThuocDTO>()
              .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            #endregion
            #region Map HoaDon
            CreateMap<HoaDon, HoaDonDTO>()
                .ForMember(dto => dto.TongTien, opt => opt.MapFrom(src => src.TongTien))
                .ForMember(dto => dto.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dto => dto.PhiVanChuyen, opt => opt.MapFrom(src => src.PhiVanChuyen))
                .ForMember(dto => dto.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dto => dto.Id_NhanVien, opt => opt.MapFrom(src => src.Id_NhanVien))
                .ForMember(dto => dto.Id_KhachHang, opt => opt.MapFrom(src => src.Id_KhachHang));
            CreateMap< HoaDon, FullHoaDonDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.TongTien, opt => opt.MapFrom(src => src.TongTien))
                .ForMember(dto => dto.PhiVanChuyen, opt => opt.MapFrom(src => src.PhiVanChuyen))
                .ForMember(dto => dto.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dto => dto.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dto => dto.Id_NhanVien, opt => opt.MapFrom(src => src.Id_NhanVien))
                .ForMember(dto => dto.Id_KhachHang, opt => opt.MapFrom(src => src.Id_KhachHang));
            #endregion
        }
    }
}
