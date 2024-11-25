using Responses.Responses;
using Responses.Resquests;
using DTO.VuvietanhDTO.KhachHangs;
using Service.VuVietAnhService.GeneralService;

namespace Service.VuVietAnhService.IRepository.IAccountKhachHang
{
    public interface IAccountKHService : IContactValidationService
    {
        Task<ResponseObj<KhachhangDTO>> RegisterAccountKH(KhachhangDTO khachhangDTO);
        Task<ResponseText> LoginKH(LoginResquest loginResquest);
        Task<List<KhachhangDTO>> GetAllAccountKH();
        Task<KhachhangDTO> GetAccountKHById(int id);



    }
}
