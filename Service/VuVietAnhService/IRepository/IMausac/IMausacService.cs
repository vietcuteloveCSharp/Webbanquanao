using DTO.VuvietanhDTO.Danhmucs;
using DTO.VuvietanhDTO.Mausacs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.IMausac
{
    public interface IMausacService
    {
        Task<IEnumerable<FullMauSacDTO>> GetAllMausac();
        Task<MauSacDTO> GetMauSacById(int id);
        Task<MauSacDTO> AddMauSac(MauSacDTO mauSacDTO);
        Task<MauSacDTO> UpdateMauSac(int id, MauSacDTO mauSacDTO);
        Task<bool> DeleteMauSac(int id);
    }
}
