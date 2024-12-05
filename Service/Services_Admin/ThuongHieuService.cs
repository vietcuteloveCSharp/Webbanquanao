using AutoMapper;
using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
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
    public class ThuongHieuService : IThuongHieuService
    {
        private readonly IThuongHieuRepository _repository;
        private readonly IMapper _mapper;

        public ThuongHieuService(IThuongHieuRepository repository,IMapper  mapper)
        {
           this._repository = repository;
            this._mapper = mapper;
        }
        public async Task<ThuongHieuDTO> Add(ThuongHieuDTO obj)
        {
            var list = await _repository.GetAll();
            if (list.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên thương hiệu đã tồn tại");

            }
            var newDto = _mapper.Map<ThuongHieu>(obj);
            await _repository.Add(newDto);
            return _mapper.Map<ThuongHieuDTO>(newDto);
        }

        public async Task<List<ThuongHieuDTO>> GetAll()
        {
            var list = await _repository.GetAll();
            if (!list.Any())
            {
                return new List<ThuongHieuDTO>();
            }
            return _mapper.Map<List<ThuongHieuDTO>>(list);
        }
        public async Task<ThuongHieuDTO> GetById(int id)
        {
            var th = await _repository.GetById(id);
            if (th == null)
            {
                throw new KeyNotFoundException("không tìm thấy thương hiệu với id:" + id);
            }
            return _mapper.Map<ThuongHieuDTO>(th);
        }

        public async Task<ThuongHieuDTO> Update(int id, ThuongHieuDTO obj)
        {
            // Tìm chức vụ theo ID
            var existing = await _repository.GetById(id);
            // Nếu không tìm thấy, báo lỗi
            if (existing == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy thương hiệu với ID {id}.");
            }
            var listChucVu = await _repository.GetAll();
            if (listChucVu.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên thương hiệu đã được sử dụng");

            }
            // Sử dụng AutoMapper để cập nhật các thuộc tính (obj là DTO và existingChucvu là entity map tương đương gán các giá trị của DTO cho entity tức là cập nhật)
            _mapper.Map(obj, existing);
            // Map lại đối tượng sau khi cập nhật sang DTO và trả về
            return _mapper.Map<ThuongHieuDTO>(existing);

        }
        public async Task<ThuongHieuDTO> Delete(int id)
        {
            // Tìm chức vụ theo ID
            var thdlt = await _repository.GetById(id);
            // Nếu không tìm thấy, báo lỗi
            if (thdlt == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy thương hiệu với ID {id}.");
            }
            await _repository.Delete(id);
            return _mapper.Map<ThuongHieuDTO>(thdlt);

        }

    }
}
