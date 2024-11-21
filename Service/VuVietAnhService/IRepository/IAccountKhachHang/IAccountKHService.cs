using Reponses.Response;
using DTO.KhachHangs;
using Reponses.Resquest;
using Service.GeneralService.IContactValidation;

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
