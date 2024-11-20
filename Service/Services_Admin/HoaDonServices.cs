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
    public class HoaDonServices : IHoaDonService
    {
        private readonly HoaDonRepository _repository;

        public HoaDonServices(HoaDonRepository repository)
        {
            _repository = repository;
        }

        public string Add(HoaDon obj)
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

        public List<HoaDon> GetAll()
        {
            return _repository.GetAll();
        }

        public HoaDon GetById(int id)
        {
            return _repository.GetById(id);
        }

        public string Update(HoaDon obj)
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
    }
}
