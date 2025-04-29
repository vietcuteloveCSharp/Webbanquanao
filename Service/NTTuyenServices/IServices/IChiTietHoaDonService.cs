using DTO.NTTuyen.ChiTietHoaDon;

namespace Service.NTTuyenServices.IServices
{
    public interface IChiTietHoaDonService
    {
        Task<List<FullChiTietHoaDonDTO>> GetAll();
        Task<ChiTietHoaDonDTO> GetById(int id);
        Task<List<ChiTietHoaDonDTO>> GetByHoaDonId(int id);
        Task<ChiTietHoaDonDTO> Add(ChiTietHoaDonDTO obj);
        Task<ChiTietHoaDonDTO> Update(int id, ChiTietHoaDonDTO obj);
    }
}
