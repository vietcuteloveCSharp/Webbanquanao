using DTO.VuvietanhDTO.HoadonsDTO;
using Enum.EnumVVA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.IHoadon
{
    public interface IHoadonService
    { 
        Task<HoaDonDTO> GetHoaDonById(int id);
        Task<UpdateTrangThaiDTO> UpdateTrangThai(int id, ETrangThaiHD eTrangThaiHD, string diaChiGiaoHang);
        Task<IEnumerable<FullHoaDonDTO>> GetAllHoaDon();

    }
}
