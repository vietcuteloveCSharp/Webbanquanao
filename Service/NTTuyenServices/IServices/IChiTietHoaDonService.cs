using DTO.NTTuyen.ChiTietHoaDon;
using DTO.NTTuyen.HoaDons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.IServices
{
    public interface IChiTietHoaDonService
    {
        Task<List<FullChiTietHoaDonDTO>> GetAll();
        Task<ChiTietHoaDonDTO> GetById(int id);
        Task<ChiTietHoaDonDTO> Add(ChiTietHoaDonDTO obj);
        Task<ChiTietHoaDonDTO> Update(int id, ChiTietHoaDonDTO obj);
        Task<ChiTietHoaDonDTO> Delete(int id);
    }
}
