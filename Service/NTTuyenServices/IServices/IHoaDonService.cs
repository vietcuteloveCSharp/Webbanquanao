using DTO.NTTuyen.HoaDons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.IServices
{
    public interface  IHoaDonService
    {
        Task<List<FullHoaDonDTO>> GetAll();
        Task<HoaDonDTO> GetById(int id);
        Task<HoaDonDTO> Add(HoaDonDTO obj);
        Task<HoaDonDTO> Update(int id, HoaDonDTO obj);
        Task<HoaDonDTO> Delete(int id);
    }
}
