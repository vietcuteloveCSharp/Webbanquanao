using AutoMapper;
using DAL.Entities;
using DTO.VuvietanhDTO.Chucvus;
using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.KhachHangs;
using DTO.VuvietanhDTO.NhanViens;
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
            CreateMap<NhanVien, NhanvienDTO>()
             .ForMember(dest => dest.TaiKhoan, opt => opt.MapFrom(src => src.TaiKhoan))
             .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.TenNhanVien))
             .ForMember(dest => dest.Id_ChucVu, opt => opt.MapFrom(src => src.Id_ChucVu));
            CreateMap<KhachHang, KhachhangDTO>()
               .ForMember(dest => dest.TaiKhoan, opt => opt.MapFrom(src => src.TaiKhoan))
               .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
               .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<ChucVu, ChucvuDTO>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.Mota , opt => opt.MapFrom(src => src.Mota));
            CreateMap<CuaHang, CuahangDTO>()
                .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Ten))
                .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DiaChi, opt => opt.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.NgayTao));

        }
    }
}
