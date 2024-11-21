using DTO.NhanViens;
using Reponses.Response;
using Reponses.Resquest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.GeneralService.IContactValidation;


namespace Service.VuVietAnhService.IRepository.IAccount
{
    public interface IAccountService : IContactValidationService
    {

        Task<ResponseObj<NhanvienRegisterDTO>> RegisterAccount(NhanvienRegisterDTO nhanvienRegisterDTO);
        Task<ResponseText> LoginAccount(LoginResquest loginResquest);

    }
}
