using AutoMapper;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Mausacs;
using DTO.VuvietanhDTO.Chucvus;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.NhanViens;


namespace HelperMap.Mapping
{
    public class MapperDTO_Entity : Profile
    {
        public MapperDTO_Entity()
        {
            CreateMap<KhachhangDTO, KhachHang>().ReverseMap();
            CreateMap<ChucvuDTO, ChucVu>().ReverseMap();
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
            CreateMap<CuahangDTO, CuaHang>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.DiaChi, opt => opt.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao));
            CreateMap<MausacDTO, MauSac>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.MaHex, opt => opt.MapFrom(src => src.MaHex));

        }
    }
}
