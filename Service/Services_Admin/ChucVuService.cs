using AutoMapper;
using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using DTO.TuyenNT;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        private IMapper _mapper;
    
        public ChucVuService(IChucVuRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ChucVuDTO> Add(ChucVuDTO obj)
        {
            var listChucVu = await _repository.GetAll();
            if (listChucVu.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên chức vụ đã tồn tại");

            }
            var  newchucvuDto = _mapper.Map<ChucVu>(obj);
            await _repository.Add(newchucvuDto);
            return _mapper.Map<ChucVuDTO>(newchucvuDto);
        }

        public async Task<List<ChucVuDTO>> GetAll()
        {
            var listChucVu = await _repository.GetAll();
            if (!listChucVu.Any())
            {
                return new List<ChucVuDTO>();
            }
            return _mapper.Map<List<ChucVuDTO>>(listChucVu);
        }
        public async Task<ChucVuDTO> GetById(int id)
        {
            var chucvu = await _repository.GetById(id);
            if (chucvu == null)
            {
                throw new KeyNotFoundException("không tìm thấy chức vụ với id:"+id);
            }
            return _mapper.Map<ChucVuDTO>(chucvu);
        }

        public async Task<ChucVuDTO> Update(int id,ChucVuDTO obj)
        {
            // Tìm chức vụ theo ID
            var existingChucvu = await _repository.GetById(id);
            // Nếu không tìm thấy, báo lỗi
            if (existingChucvu == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy chức vụ với ID {id}.");
            }
            var listChucVu = await _repository.GetAll();
            if (listChucVu.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên chức vụ đã được sử dụng");

            }
            // Sử dụng AutoMapper để cập nhật các thuộc tính (obj là DTO và existingChucvu là entity map tương đương gán các giá trị của DTO cho entity tức là cập nhật)
            _mapper.Map(obj, existingChucvu);
            // Map lại đối tượng sau khi cập nhật sang DTO và trả về
            return _mapper.Map<ChucVuDTO>(existingChucvu);

        }

    }

}
