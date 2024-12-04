using DAL.Entities;
using DTO.VuvietanhDTO.Chucvus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.IChucvu
{
    public interface IChucvuService
    {
        Task<IEnumerable<FullChucVuDTO>> GetAllChucVu();
        Task<ChucvuDTO> GetChucVuById(int id);
        Task<string> GetTenChucVuById(int idChucVu); // lấy được tên chức vụ qua id
        Task<ChucvuDTO> AddChucVu(ChucvuDTO chucvuDTO);
        Task<ChucvuDTO> UpdateChucVu(int id, ChucvuDTO updateChucvuDTO);
        Task<bool> DeleteChucVu(int id);
    }
}
