using DAL.Entities;
using DTO.VuvietanhDTO.Calamviecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.ICalamviec
{
    public interface ICalamviecService
    {
        Task<IEnumerable<FullCaLamViecDTO>> GetAllCaLamViec();
        Task<CaLamViecDTO> GetCaLamViecById(int id);
        Task<bool> CreateCaLamViec(CreateCaLamViecDTO createCaLamViecDTO);
        Task<bool> UpdateCaLamViec(int id,UpdateCaLamViecDTO updateCaLamViec);
        Task<bool> DoiCaLamViec(int nv1, int nv2);
        
    }
}
