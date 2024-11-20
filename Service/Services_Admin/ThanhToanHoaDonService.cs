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
    public class ThanhToanHoaDonService : IThanhToanHoaDonService
    {
        private readonly ThanhToanHoaDonRepository _repository;

        public ThanhToanHoaDonService(ThanhToanHoaDonRepository repository)
        {
            _repository = repository;
        }

        public string Add(ThanhToanHoaDon obj)
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

        public List<ThanhToanHoaDon> GetAll()
        {
            return _repository.GetAll();
        }

        public ThanhToanHoaDon GetById(int id)
        {
            return _repository.GetById(id);
        }

        public string Update(ThanhToanHoaDon obj)
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
