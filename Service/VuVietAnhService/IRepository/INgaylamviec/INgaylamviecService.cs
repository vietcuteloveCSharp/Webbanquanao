using DTO.VuvietanhDTO.Calamviecs;
using DTO.VuvietanhDTO.Ngaylamviecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.INgaylamviec
{
    public interface INgaylamviecService
    {
        Task<IEnumerable<NgayLamViecDTO>> GetAllNgayLamViec();
        Task<NgayLamViecDTO> GetNgayLamViecById(int id);
        Task<bool> CreateNgayLamviec(CreateNgayLamViecDTO createNgayLamViecDTO);
        Task<bool> DeleteNgayLamViec(int id);
        Task<IEnumerable<CaLamViecByNgayLamViecDTO>> GetCaLamViecByNgayLamViecIdAsync(int ngayLamViecId);
    }
}
