using DTO.NTTuyen.ChiTietHoaDon;
using Service.NTTuyenServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.Services
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        public Task<ChiTietHoaDonDTO> Add(ChiTietHoaDonDTO obj)
        {
            if (obj == null) throw new ArgumentNullException("Chi tiết hóa đơn không được để trống");
            throw new NotImplementedException();

        }

        public Task<ChiTietHoaDonDTO> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<FullChiTietHoaDonDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ChiTietHoaDonDTO> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ChiTietHoaDonDTO> Update(int id, ChiTietHoaDonDTO obj)
        {
            throw new NotImplementedException();
        }
    }
}
