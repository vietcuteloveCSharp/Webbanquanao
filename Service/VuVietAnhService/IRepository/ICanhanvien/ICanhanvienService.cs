using DTO.VuvietanhDTO.Calamviecs;
using DTO.VuvietanhDTO.Canhanviens;
using DTO.VuvietanhDTO.NhanViens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.ICanhanvien
{
    public interface ICanhanvienService
    {
        Task<IEnumerable<NhanvienDTO>> GetNhanVienByCa(int idCaLamViec);
        Task<IEnumerable<CaLamViecDTO>> GetCaLamViecByNhanVien(int idNhanVien);
        Task<IEnumerable<CanhanviensDTO>> GetAllCanhanvien();
        Task<bool> CreatCanhanvien(int idNhanVien, int idCaLamViec);
        Task<bool> UpdateCanhanvien(DoiCaDTO doiCaDTO);

    }
}
