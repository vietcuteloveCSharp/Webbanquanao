using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
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
    public class ChucVuService:IChucVuService
    {
        private readonly IChucVuRepository _repository;
    
        public ChucVuService(IChucVuRepository repository)
        {
            _repository = repository;
        }
        public ChucVuDTO ConvertToDTO(ChucVu obj)
        {
            return new ChucVuDTO()
            {
                Id= obj.Id,
                Ten = obj.Ten,
                Mota = obj.Mota,
            };

        }
        public ChucVu ConvertToEntity(ChucVuDTO obj)
        {
            return new ChucVu()
            {
                Id = obj.Id ,
                Ten = obj.Ten,
                Mota = obj.Mota,
            };

        }
        public string Add(ChucVuDTO obj)
        {
            ChucVu chucVu = ConvertToEntity(obj);
            if (_repository.Add(chucVu)==true)
            {
                return "Thêm thành công";
            }
            else
            {
                return "Thêm thất bại";
            }
            
        }

        public List<ChucVuDTO> GetAll()
        {
            var entities = _repository.GetAll();
            var dtos = new List<ChucVuDTO>();
            foreach (var entity in entities) 
            { 
                dtos.Add(ConvertToDTO(entity));
            }
            return dtos;
        }

        public ChucVuDTO GetById(int id)
        {
            ChucVu chucVu= _repository.GetById(id);

            return ConvertToDTO(chucVu);
        }

        public string Update(ChucVuDTO obj)
        {
            ChucVu chucVu = ConvertToEntity(obj);
            if (_repository.Update(chucVu) == true)
            {
                return "Sửa thành công";
            }
            else
            {
                return "Sủa thất bại";
            }
        }
    }
}
