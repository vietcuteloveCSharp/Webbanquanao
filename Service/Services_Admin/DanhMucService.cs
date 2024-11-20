using DAL.Admin_Repositories.Implement;
using DAL.Context;
using DAL.Entities;
using DTO.TuyenNT;
using Service.IServices_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services_Admin
{
    public class DanhMucService /*: IDanhMucService*/
    {
        private readonly DanhMucRepository _repository;
 

        public DanhMucService(DanhMucRepository repository)
        {
            _repository = repository;
        }
       

        //public string Add(DanhMucDTO obj)
        //{
        //    DanhMuc danhmuc = ConvertToEntity(obj);
        //    if (_repository.Add(danhmuc)== true)
        //    {
        //        return "Thêm thành công";

        //    }
        //    else
        //    {
        //        return "Thêm thất bại";
        //    }
        //}

        //public string Delete(int id)
        //{

        //    if (_repository.Delete(id) == true)
        //    {
        //        return "Xóa thành công";

        //    }
        //    else
        //    {
        //        return "Xóa thất bại";
        //    }
        //}

        //public List<DanhMucDTO> GetAll()
        //{
        //    var entities= _repository.GetAll();
        //    var dtos= new List<DanhMucDTO>();
        //    foreach (var item in entities)
        //    {
        //        dtos.Add(ConvertToDTO(item));
        //    }
        //    return dtos;
        //}

        //public DanhMucDTO GetById(int id)
        //{
        //    DanhMuc danhmuc= _repository.GetById(id);
        //    return ConvertToDTO(danhmuc);
        //}

        //public string Update(DanhMucDTO obj)
        //{
        //    if (_repository.Update(obj) == true)
        //    {
        //        return "Sửa thành công";

        //    }
        //    else
        //    {
        //        return "Sửa thất bại";
        //    }
        //}
    }
}
