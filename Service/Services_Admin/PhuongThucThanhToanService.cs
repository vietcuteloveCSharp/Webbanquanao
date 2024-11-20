using DAL.Admin_Repositories.Implement;
using DAL.Entities;
using Service.IServices_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services_Admin
{
    public class PhuongThucThanhToanService : IPhuongThucThanhToanService
    {
        private readonly PhuongThucThanhToanRepository _repository;
        public List<PhuongThucThanhToan> GetAll()
        {
            return _repository.GetAll();
        }

        public PhuongThucThanhToan GetById(int id)
        {
            return _repository.GetById(id);
        }

        public string Add(PhuongThucThanhToan obj)
        {
            if (_repository.Add(obj) == true)
            {
                return "Thêm thành công";

            }
            else
            {
                return "Thêm thất bại";
            }
        }

        public string Update(PhuongThucThanhToan obj)
        {
            if (_repository.Update(obj) == true)
            {
                return "Sửa thành công";

            }
            else
            {
                return "Sửa thất bại";
            }
        }

        public string Delete(int id)
        {
            if (_repository.Delete(id) == true)
            {
                return "Xóa thành công";

            }
            else
            {
                return "Xóa thất bại";
            }
        }


    }
}
