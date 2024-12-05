using DAL.Entities;
using DTO.TuyenNT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices_Admin
{
    public interface IThuongHieuService
    {
        Task<List<ThuongHieuDTO>> GetAll();
        Task<ThuongHieuDTO> GetById(int id);
        Task<ThuongHieuDTO> Add (ThuongHieuDTO obj);
        Task<ThuongHieuDTO> Update (int id,ThuongHieuDTO obj);
        Task<ThuongHieuDTO>  Delete (int id);
    }
}
