using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Implement
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public NhanVienRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(NhanVien obj)
        {
            if (obj== null)
            {
                return false;
            }
            else
            {
                _context.NhanViens.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            var udobj = GetById(id);
            if (udobj == null)
            {
                return false;
            }
            else
            {
                udobj.TrangThai = false;/// Xoa nhan vien
               
                _context.SaveChanges();
                return true;
            }
        }

        public List<NhanVien> GetAll()
        {
           return _context.NhanViens.ToList();
        }

        public NhanVien GetById(int id)
        {
            return _context.NhanViens.Find(id);
        }

        public bool Update(NhanVien obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj== null)
            {
                return false;
            }
            else
            {
                udobj.TaiKhoan = obj.TaiKhoan;
                udobj.MatKhau = obj.MatKhau;
                udobj.TenNhanVien = obj.TenNhanVien;
                udobj.Sdt = obj.Sdt;    
                udobj.Email = obj.Email;
                udobj.NgaySinh = obj.NgaySinh;
                udobj.DiaChi = obj.DiaChi;
                udobj.GhiChu = obj.GhiChu;
                udobj.TrangThai = obj.TrangThai;
                udobj.NgayTao   = obj.NgayTao;
                udobj.NgayCapNhat = obj.NgayCapNhat;
                udobj.Id_ChucVu = obj.Id_ChucVu;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
//public string TaiKhoan { get; set; } = null!;
//public string MatKhau { get; set; } = null!;
//public string TenNhanVien { get; set; } = string.Empty;
//public string Sdt { get; set; } = string.Empty;
//public string Email { get; set; } = string.Empty;
//[DataType(DataType.Date)]
//public DateTime? NgaySinh { get; set; } = null;
//public string DiaChi { get; set; } = string.Empty;
//public string GhiChu { get; set; } = string.Empty;
//public bool TrangThai { get; set; } = true;
//public DateTime NgayTao { get; set; } = DateTime.Now;
//public DateTime NgayCapNhat { get; set; }

//[ForeignKey("ChucVu")]
//public int Id_ChucVu { get; set; }
