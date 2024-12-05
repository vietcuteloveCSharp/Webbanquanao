using AutoMapper;
using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
using DAL.Entities;
using DTO.TuyenNT;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        private readonly IThanhToanHoaDonRepository _repository;
        private readonly IMapper _mapper;

        public ThanhToanHoaDonService(IThanhToanHoaDonRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<ThanhToanHoaDonDTO> Add(ThanhToanHoaDonDTO obj)
        {
            var tthd = _mapper.Map<ThanhToanHoaDon>(obj);
            await _repository.Add(tthd);
            return _mapper.Map<ThanhToanHoaDonDTO>(tthd);
        }

        public async Task<List<ThanhToanHoaDonDTO>> GetAll()
        {
           var list = await _repository.GetAll();
            if (!list.Any()) 
            { 
                return new List<ThanhToanHoaDonDTO>();
            }
            var listDto = _mapper.Map<List<ThanhToanHoaDonDTO>>(list);
            return listDto;
        }

        public async Task<ThanhToanHoaDonDTO> GetById(int id)
        {
            var tthd = await _repository.GetById(id);
            if (tthd==null)
            {
                throw new KeyNotFoundException("Không tìm thấy thanh toán hóa đơn có id là :" + id);
            }
            return _mapper.Map<ThanhToanHoaDonDTO>(tthd) ;
        }

        public async Task<ThanhToanHoaDonDTO> Update(int id, ThanhToanHoaDonDTO obj)
        {
            var tthd = await _repository.GetById(id);
            if (tthd == null)
            {
                throw new KeyNotFoundException("Không tìm thấy thanh toán hóa đơn có id là :" + id);
            }
            await _repository.Update(id, _mapper.Map<ThanhToanHoaDon>(obj));
            return _mapper.Map<ThanhToanHoaDonDTO>(tthd);
        }
    }
}
