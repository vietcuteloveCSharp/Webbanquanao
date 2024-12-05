using AutoMapper;
using DAL.Admin_Repositories.Interface;
using DAL.Entities;
using DTO.TuyenNT;
using Service.IServices_Admin;
using System.Xml;

namespace Service.Services_Admin
{

    public class NhanVienService : INhanVienServices
    {
        private readonly INhanVienRepository _repository;
        private readonly IMapper _mapper;

        public NhanVienService(INhanVienRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<NhanVienDTO> Add(NhanVienDTO obj)
        {
            var listNhanVien = await _repository.GetAll();
            if (listNhanVien.Any(c =>c.TaiKhoan == obj.TaiKhoan))
            {
                throw new InvalidOperationException("Tên tài khoản đã tồn tại");
            }
            if (listNhanVien.Any(c => c.Sdt == obj.Sdt))
            {
                throw new InvalidOperationException("SDT đã tồn tại");
            }
            if (listNhanVien.Any(c => c.Email == obj.Email))
            {
                throw new InvalidOperationException("Email đã tồn tại");
            }
            var nhanvien = _mapper.Map<NhanVien>(obj);
            await _repository.Add(nhanvien);
            return _mapper.Map<NhanVienDTO>(nhanvien);
        }

        public async Task<NhanVienDTO> Delete(int id)
        {
            var nhanvien = await _repository.GetById(id);
            if (nhanvien == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhân viên có id: {id}");
            }
            await _repository.Delete(id);
            return _mapper.Map<NhanVienDTO>(nhanvien);
        }

        public async Task<List<NhanVienDTO>> GetAll()
        {
            var listNhanVien = await _repository.GetAll();
            if (!listNhanVien.Any())
            {
                return new List<NhanVienDTO>(); 
            }
            var listNhanVienDTO = _mapper.Map<List<NhanVienDTO>>(listNhanVien);
            return listNhanVienDTO;
        }

        public async Task<NhanVienDTO> GetById(int id)
        {
            var nhanvien = await _repository.GetById(id);
            if (nhanvien == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhân viên có id: {id}");
            }
            await _repository.GetById(id);
            return _mapper.Map<NhanVienDTO>(nhanvien);
        }

        public async Task<NhanVienDTO> Update(int id,NhanVienDTO obj)
        {
            var nhanvien = await _repository.GetById(id);
            if (nhanvien == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhân viên có id: {id}");
            }
            var listNhanVien = await _repository.GetAll();
            if (listNhanVien.Any(c => c.TaiKhoan == obj.TaiKhoan))
            {
                throw new InvalidOperationException("Tên tài khoản đã tồn tại");
            }
            if (listNhanVien.Any(c => c.Sdt == obj.Sdt))
            {
                throw new InvalidOperationException("SDT đã tồn tại");
            }
            if (listNhanVien.Any(c => c.Email == obj.Email))
            {
                throw new InvalidOperationException("Email đã tồn tại");
            }

            await _repository.Update(id, _mapper.Map<NhanVien>(obj));
            return _mapper.Map<NhanVienDTO>(nhanvien);
        }
    }
}
