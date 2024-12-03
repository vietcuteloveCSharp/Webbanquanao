using DTO.VuvietanhDTO.Danhmucs;
using DTO.VuvietanhDTO.Sanphams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.ISanpham
{
    public interface ISanphamSerivce
    {
        Task<IEnumerable<FullSanPhamDTO>> GetAllSanPham();
        Task<SanPhamDTO> GetSanPhamById(int id);
        Task<CreatSanPhamDTO> CreateSanPham(CreatSanPhamDTO creatSanPhamDTO);
        Task<UpdateSanPhamDTO> UpdateSanPham(int id, UpdateSanPhamDTO updateSanPhamDTO);
        Task<bool> DeleteSanPham(int id);
    }
}
