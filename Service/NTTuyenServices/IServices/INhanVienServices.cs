using DTO.VuvietanhDTO.NhanViens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NTTuyenServices.IServices
{
    public interface INhanVienServices
    {
        Task<NhanVienProfileDTO> GetById(int id);
    }
}
