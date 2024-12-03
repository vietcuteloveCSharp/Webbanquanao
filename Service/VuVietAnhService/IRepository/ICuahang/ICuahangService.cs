using DTO.VuvietanhDTO.Cuahangs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.ICuahang
{
    public interface ICuahangService
    {
        Task<IEnumerable<FullCuahangDTO>> GetAllCuaHang();
        Task<CuahangDTO> GetCuaHangById(int id);
        Task<CuahangDTO> AddCuahang(CuahangDTO cuahangDTO);
        Task<CuahangDTO> UpdateCuahang(int id, CuahangDTO updateCuahangDTO);
        Task<bool> DeleteCuahang(int id);
        Task<bool> CuahangExists(string ten, string sdt);
    }
}
