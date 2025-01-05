using DTO.NTTuyenDTO.ChiTietSanPhams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.IServices
{
    public interface IChiTietSanPhamServices
    {
        Task<List<FullChiTietSanPhamDTO>> GetAllChiTietSanPham();
        Task<ChiTietSanPhamDTO> GetChiTietSanPhamById(int id);
        
        Task<ChiTietSanPhamDTO> Add(ChiTietSanPhamDTO chitietsanpham);
        Task<ChiTietSanPhamDTO> Update(int id, ChiTietSanPhamDTO chitietsanpham);
        Task<ChiTietSanPhamDTO> Delete(int id);

    }
}
