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
    public class NhanVienService : INhanVienServices
    {
        private readonly NhanVienRepository _repository;

        public NhanVienService(NhanVienRepository repository)
        {
            _repository = repository;
        }

        public string Add(NhanVien obj)
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

        public List<NhanVien> GetAll()
        {
            return _repository.GetAll();
        }

        public NhanVien GetById(int id)
        {
            return _repository.GetById(id);
        }

        public string Update(NhanVien obj)
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
