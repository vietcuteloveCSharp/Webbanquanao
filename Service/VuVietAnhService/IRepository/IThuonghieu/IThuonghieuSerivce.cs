using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Thuonghieus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.IThuonghieu
{
    public interface IThuonghieuSerivce
    {
        Task<IEnumerable<FullThuongHieuDTO>> GetAllThuongHieu();
        Task<CreateThuongHieuDTO> AddThuongHieu(CreateThuongHieuDTO createThuongHieuDTO);
        Task<ThuongHieuDTO> UpdateThuongHieu(int id,ThuongHieuDTO thuongHieuDTO);
        Task<bool> DeleteThuongHieu(int id);
        Task<ThuongHieuDTO> GetThuongHieuById(int id);
        Task<int> GetIdByTenThuongHieu(string tenThuongHieu);
    }
}
