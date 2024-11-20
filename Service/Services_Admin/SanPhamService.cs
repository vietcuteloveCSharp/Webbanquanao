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
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _repository;
        private readonly IMapper _mapper;

        public SanPhamService(ISanPhamRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<SanPhamDTO> Add(SanPhamDTO obj)
        {
            var list = await _repository.GetAll();
            if (list.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên sản phẩm đã tồn tại");
            }
            
            var sanPham = _mapper.Map<SanPham>(obj);
            await _repository.Add(sanPham);
            return _mapper.Map<SanPhamDTO>(sanPham);
        }

        public async Task<SanPhamDTO> Delete(int id)
        {
            var sanPham = await _repository.GetById(id);
            if (sanPham == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm có id: {id}");
            }
            await _repository.Delete(id);
            return _mapper.Map<SanPhamDTO>(sanPham);
        }

        public async Task<List<SanPhamDTO>> GetAll()
        {
            var listNhanVien = await _repository.GetAll();
            if (!listNhanVien.Any())
            {
                return new List<SanPhamDTO>();
            }
            var listSanPhamDTO = _mapper.Map<List<SanPhamDTO>>(listNhanVien);
            return listSanPhamDTO;
        }

        public async Task<SanPhamDTO> GetById(int id)
        {
            var sanPham = await _repository.GetById(id);
            if (sanPham == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm có id: {id}");
            }
            await _repository.GetById(id);
            return _mapper.Map<SanPhamDTO>(sanPham);
        }

        public async Task<SanPhamDTO> Update(int id, SanPhamDTO obj)
        {
            var sanPham = await _repository.GetById(id);
            if (sanPham == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhân viên có id: {id}");
            }
            var listNhanVien = await _repository.GetAll();
            if (listNhanVien.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên sản phẩm  đã tồn tại");
            }
            

            await _repository.Update(id, _mapper.Map<SanPham>(obj));
            return _mapper.Map<SanPhamDTO>(sanPham);
        }
    }
}
