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
    public class PhuongThucThanhToanService : IPhuongThucThanhToanService
    {
        private readonly IPhuongThucThanhToanRepository _repository;
        private readonly IMapper _mapper;

        public PhuongThucThanhToanService(IPhuongThucThanhToanRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public async Task<PhuongThucThanhToanDTO> Add(PhuongThucThanhToanDTO obj)
        {
            var list = await _repository.GetAll();
            if (list.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên phương thức thanh toán đã tồn tại");

            }
            var newDto = _mapper.Map<PhuongThucThanhToan>(obj);
            await _repository.Add(newDto);
            return _mapper.Map<PhuongThucThanhToanDTO>(newDto);
        }

        public async Task<List<PhuongThucThanhToanDTO>> GetAll()
        {
            var list = await _repository.GetAll();
            if (!list.Any())
            {
                return new List<PhuongThucThanhToanDTO>();
            }
            return _mapper.Map<List<PhuongThucThanhToanDTO>>(list);
        }
        public async Task<PhuongThucThanhToanDTO> GetById(int id)
        {
            var pttt = await _repository.GetById(id);
            if (pttt == null)
            {
                throw new KeyNotFoundException("không tìm thấy phương thức thanh toán với id:" + id);
            }
            return _mapper.Map<PhuongThucThanhToanDTO>(pttt);
        }

        public async Task<PhuongThucThanhToanDTO> Update(int id, PhuongThucThanhToanDTO obj)
        {
            // Tìm chức vụ theo ID
            var existing = await _repository.GetById(id);
            // Nếu không tìm thấy, báo lỗi
            if (existing == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy phương thức thanh toán với ID {id}.");
            }
            var listChucVu = await _repository.GetAll();
            if (listChucVu.Any(c => c.Ten == obj.Ten))
            {
                throw new InvalidOperationException("Tên phương thức thanh toán đã được sử dụng");

            }
            // Sử dụng AutoMapper để cập nhật các thuộc tính (obj là DTO và existingChucvu là entity map tương đương gán các giá trị của DTO cho entity tức là cập nhật)
            _mapper.Map(obj, existing);
            // Map lại đối tượng sau khi cập nhật sang DTO và trả về
            return _mapper.Map<PhuongThucThanhToanDTO>(existing);

        }
        public async Task<PhuongThucThanhToanDTO> Delete(int id)
        {
            // Tìm chức vụ theo ID
            var ptttdlt = await _repository.GetById(id);
            // Nếu không tìm thấy, báo lỗi
            if (ptttdlt == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy phương thức thanh toán  với ID {id}.");
            }
            await _repository.Delete(id);
            return _mapper.Map<PhuongThucThanhToanDTO>(ptttdlt);
        }
    }
}
