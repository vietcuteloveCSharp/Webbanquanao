using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Kichthuocs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.IKichthuoc
{
    public interface IKichthuocService
    {
        Task<IEnumerable<FullKichThuocDTO>> GetAllKichThuoc();
        Task<KichThuocDTO> GetKickThuocId(int id);
        Task<KichThuocDTO> AddKichThuoc(KichThuocDTO kichThuocDTO);
        Task<KichThuocDTO> UpdateKichThuoc(int id, KichThuocDTO kichThuocDTO);
        Task<bool> DeleteKichThuoc(int id);
    }
}
