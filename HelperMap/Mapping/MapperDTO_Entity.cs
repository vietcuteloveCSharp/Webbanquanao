using AutoMapper;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.VuvietanhDTO.Mausacs;
using DTO.VuvietanhDTO.Chucvus;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.NhanViens;
using DTO.VuvietanhDTO.Danhmucs;
using DTO.VuvietanhDTO.Sanphams;
using DTO.VuvietanhDTO.Thuonghieus;
using DTO.VuvietanhDTO.Kichthuocs;
using DTO.VuvietanhDTO.Giohangs;
using Microsoft.EntityFrameworkCore;
using Service.VuVietAnhService.IRepository.IThuonghieu;
using Service.VuVietAnhService.IRepository.IDanhmuc;
using DTO.VuvietanhDTO.HoadonsDTO;




namespace HelperMap.Mapping
{
    public class MapperDTO_Entity : Profile
    {
       
       
        public MapperDTO_Entity()
        {
            
            #region Map Khachhang
            CreateMap<KhachhangDTO, KhachHang>().ReverseMap();
            #endregion
            #region Map chucvu
            CreateMap<ChucvuDTO, ChucVu>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.Mota, opt => opt.MapFrom(src => src.Mota));

            #endregion
            #region Mapping nhanvien login
            CreateMap<NhanvienDTO, NhanVien>()
                .ForMember(dest => dest.TaiKhoan, opt => opt.MapFrom(src => src.TaiKhoan))
                .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.TenNhanVien))
                .ForMember(dest => dest.Id_ChucVu, opt => opt.MapFrom(src => src.Id_ChucVu));
            #endregion
            #region Mapping Đăng kí--nhân viên DTO->Entity nhân Viên
            CreateMap<NhanvienRegisterDTO, NhanVien>()
                .ForMember(dest => dest.TaiKhoan, opt => opt.MapFrom(src => src.TaiKhoan))
                .ForMember(dest => dest.MatKhau, opt => opt.MapFrom(src => src.MatKhau))
                .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.TenNhanVien))
                .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src => src.NgaySinh))
                .ForMember(dest => dest.DiaChi, opt => opt.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.GhiChu, opt => opt.MapFrom(src => src.GhiChu))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dest => dest.Id_ChucVu, opt => opt.MapFrom(src => src.Id_ChucVu));
            #endregion
            #region Map cuahang
            CreateMap<CuahangDTO, CuaHang>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.DiaChi, opt => opt.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao));

            #endregion
            #region Map mausac
            CreateMap<MauSacDTO, MauSac>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MaHex, opt => opt.MapFrom(src => src.MaHex));
            CreateMap<FullMauSacDTO, MauSac>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten)) 
                .ForMember(dest => dest.MaHex, opt => opt.MapFrom(src => src.MaHex));

            #endregion
            #region Map Danhmuc
            CreateMap<DanhMucDTO, DanhMuc>()
               .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
               .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
               .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
               .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
                
            CreateMap<CreatDanhMucDTO, DanhMuc>()
               .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
               .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
               .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao));
            CreateMap<UpdateDanhMucDTO, DanhMuc>()
              .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
              .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
              .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
            CreateMap< FullDanhMucDTO, DanhMuc>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Id_DanhMucCha, opt => opt.MapFrom(src => src.Id_DanhMucCha))
              .ForMember(dest => dest.TenDanhMuc, opt => opt.MapFrom(src => src.TenDanhMuc))
              .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
              .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
            #endregion
            #region Map SanPham
            CreateMap<CreatSanPhamDTO, SanPham>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
                .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
                .ForMember(dest => dest.Id_ThuongHieu, opt => opt.MapFrom(src => src.Id_ThuongHieu))
                .ForMember(dest => dest.Id_DanhMuc, opt => opt.MapFrom(src => src.Id_DanhMuc));
            CreateMap<UpdateSanPhamDTO, SanPham>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
                .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(src => src.NgayCapNhat))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
                .ForMember(dest => dest.Id_ThuongHieu, opt => opt.MapFrom(src => src.Id_ThuongHieu))
                .ForMember(dest => dest.Id_DanhMuc, opt => opt.MapFrom(src => src.Id_DanhMuc));
            CreateMap<SanPhamDTO, SanPham>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
                .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(src => src.NgayCapNhat))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
                .ForMember(dest => dest.Id_ThuongHieu, opt => opt.MapFrom(src => src.Id_ThuongHieu))
                .ForMember(dest => dest.Id_DanhMuc, opt => opt.MapFrom(src => src.Id_DanhMuc));
            CreateMap<FullSanPhamDTO, SanPham>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
                .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.Gia))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(src => src.NgayCapNhat))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
                .ForMember(dest => dest.Id_ThuongHieu, opt => opt.MapFrom(src => src.Id_ThuongHieu))
                .ForMember(dest => dest.Id_DanhMuc, opt => opt.MapFrom(src => src.Id_DanhMuc));
               
            #endregion
            #region Map Thuonghieu
            CreateMap<ThuongHieuDTO, ThuongHieu>()
                  .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                  .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                  .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
             CreateMap<FullThuongHieuDTO, ThuongHieu>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                  .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa))
                  .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai));
            CreateMap<CreateThuongHieuDTO, ThuongHieu>()
                 .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                 .ForMember(dest => dest.MoTa, opt => opt.MapFrom(src => src.MoTa));

            #endregion
            #region Map Kichthuoc
            CreateMap<KichThuocDTO, KichThuoc>()
              .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten));
            CreateMap<FullKichThuocDTO, KichThuoc>()
              .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            #endregion
            #region Map GioHang
            CreateMap<GioHangDTO, GioHang>()
                 .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
                 .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                 .ForMember(dest => dest.Id_KhachHang, opt => opt.MapFrom(src => src.Id_KhachHang))
                 .ForMember(dest => dest.Id_ChiTietSanPham, opt => opt.MapFrom(src => src.Id_ChiTietSanPham));
            #endregion
            #region hoadon
            CreateMap<FullHoaDonDTO, HoaDon>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TongTien, opt => opt.MapFrom(src => src.TongTien))
                .ForMember(dest => dest.PhiVanChuyen, opt => opt.MapFrom(src => src.PhiVanChuyen))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.Id_KhachHang, opt => opt.MapFrom(src => src.Id_KhachHang))
                .ForMember(dest => dest.Id_NhanVien, opt => opt.MapFrom(src => src.Id_NhanVien));
            CreateMap<UpdateTrangThaiDTO, HoaDon>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.Id_NhanVien, opt => opt.MapFrom(src => src.Id_NhanVien));
            CreateMap<HoaDonDTO, HoaDon>()
              .ForMember(dest => dest.TongTien, opt => opt.MapFrom(src => src.TongTien))
                .ForMember(dest => dest.PhiVanChuyen, opt => opt.MapFrom(src => src.PhiVanChuyen))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.Id_KhachHang, opt => opt.MapFrom(src => src.Id_KhachHang))
                .ForMember(dest => dest.Id_NhanVien, opt => opt.MapFrom(src => src.Id_NhanVien));
            #endregion




        }
    }
}
