using AutoMapper;
using DAL.Admin_Repositories.Implement;
using DAL.Admin_Repositories.Interface;
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
    public class HoaDonServices : IHoaDonService
    {
        private readonly IHoaDonRepository _repository;
        private readonly IMapper _mapper;

        public HoaDonServices(IHoaDonRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<HoaDonDTO> Add( HoaDonDTO obj)
        {
            var hoaDon = _mapper.Map<HoaDon>(obj);
            await _repository.Add(hoaDon);
            return _mapper.Map<HoaDonDTO>(hoaDon); 
        }

        public async Task<HoaDonDTO> Delete(int id)
        {
            var hoaDon = await _repository.GetById(id);
            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy hóa đơn có id là: {id}");
            }
            await _repository.Delete(id);
            return _mapper.Map<HoaDonDTO>(hoaDon);
        }

        public async Task<List<HoaDonDTO>> GetAll()
        {
            var listHoaDon = await _repository.GetAll();
            if (!listHoaDon.Any())
            {
                return new List<HoaDonDTO>();
            }
            var listHoaDonDto = _mapper.Map<List<HoaDonDTO>>(listHoaDon);   
            return listHoaDonDto;
        }

        public async Task<HoaDonDTO> GetById(int id)
        {
            var hoaDon = await _repository.GetById(id);
            if (hoaDon== null)
            {
                throw new KeyNotFoundException($"Không tìm thấy hóa đơn có id là: {id}");
            }
            return _mapper.Map<HoaDonDTO>(hoaDon) ;
        }

        public async Task<HoaDonDTO> Update(int id, HoaDonDTO obj)
        {
            var hoaDon = await _repository.GetById(id);
            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy hóa đơn có id là: {id}");
            }
            await _repository.Update(id,_mapper.Map<HoaDon>(obj));
            return _mapper.Map<HoaDonDTO>(hoaDon);
        }
    }
}
