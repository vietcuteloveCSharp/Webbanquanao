using AutoMapper;
using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using DTO.TuyenNT;
using Microsoft.VisualBasic;
using Service.IServices_Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services_Admin
{
    public class DanhMucService : IDanhMucService
    {
        private readonly IDanhMucRepository _repository;
        private IMapper _mapper;
        public DanhMucService(IDanhMucRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<DanhMucDTO> Add(DanhMucDTO obj)
        {
            var listDanhMuc = await _repository.GetAll();
            if (listDanhMuc.Any(c=>c.TenDanhMuc==obj.TenDanhMuc))
            {
                throw new InvalidOperationException("Tên danh mục đã tồn tại ");
            }
            var danhmuc = _mapper.Map<DanhMuc>(obj);
            await _repository.Add(danhmuc);
            return _mapper.Map<DanhMucDTO>(danhmuc);

        }

        public Task<DanhMucDTO> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DanhMucDTO>> GetAll()
        {
            var listDanhMuc = await _repository.GetAll();
            if (!listDanhMuc.Any())
            {
                return new List<DanhMucDTO>();  
            }
            var listDanhMucDto = _mapper.Map<List<DanhMucDTO>>(listDanhMuc);
            return listDanhMucDto;
        }

        public async Task<DanhMucDTO> GetById(int id)
        {
            var danhMuc = await _repository.GetById (id);
            if (danhMuc == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy danh mục có id: {id}");
            }
            return _mapper.Map<DanhMucDTO>(danhMuc);
        }

        public async Task<DanhMucDTO> Update(int id, DanhMucDTO obj)
        {
            var exsistingDanhmuc = await _repository.GetById(id);
            if (exsistingDanhmuc == null )
            {
                throw new KeyNotFoundException($"Không tìm thấy danh mục có id: {id}");
            }
            var listDanhMuc = await _repository.GetAll();
            if (listDanhMuc.Any(c=>c.TenDanhMuc == obj.TenDanhMuc))
            {
                throw new InvalidOperationException("Tên danhh mục đã tồn tại");
            }
            _mapper.Map(obj,exsistingDanhmuc);
            return _mapper.Map<DanhMucDTO>(exsistingDanhmuc);
        }
    }
}
