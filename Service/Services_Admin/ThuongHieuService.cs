using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
using DAL.Entities;
using Service.IServices_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services_Admin
{
    public class ThuongHieuService : IThuongHieuService
    {
        private readonly ThuongHieuRepository _repository;

        public ThuongHieuService(ThuongHieuRepository repository)
        {
            _repository = repository;
        }

        public string Add(ThuongHieu obj)
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

        public List<ThuongHieu> GetAll(ThuongHieu obj)
        {
            return _repository.GetAll();
        }

        public ThuongHieu GetById(int id)
        {
            return _repository.GetById(id);
        }

        public string Update(ThuongHieu obj)
        {
            if (_repository.Add(obj) == true)
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
