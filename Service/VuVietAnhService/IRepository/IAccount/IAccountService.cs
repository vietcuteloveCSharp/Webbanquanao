using DTO.VuvietanhDTO.NhanViens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.VuVietAnhService.GeneralService;
using Responses.Responses;
using Responses.Resquests;


namespace Service.VuVietAnhService.IRepository.IAccount
{
    public interface IAccountService : IContactValidationService
    {

        Task<ResponseObj<NhanvienRegisterDTO>> RegisterAccount(NhanvienRegisterDTO nhanvienRegisterDTO);
        Task<ResponseText> LoginAccount(LoginResquest loginResquest);

    }
}
