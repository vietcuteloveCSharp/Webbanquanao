using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.GeneralService
{
    public interface IContactValidationService
    {
        Task<bool> CheckEmailExitst(string email);
        Task<bool> CheckTaikhoanExitst(string taikhoan);
        Task<bool> CheckSdtExitst(string sdt);
    }
}
