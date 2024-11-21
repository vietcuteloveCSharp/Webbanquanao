using Reponses.Response;
using Reponses.Resquest;
using Service.GeneralService.IContactValidation;
using DTO.VuvietanhDTO.KhachHangs;

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
