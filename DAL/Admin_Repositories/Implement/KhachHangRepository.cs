using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Implement
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public KhachHangRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(KhachHang obj)
        {
            if (obj== null)
            {
                return false;
            }
            else
            {
                _context.KhachHangs.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<KhachHang> GetAll()
        {
           return _context.KhachHangs.ToList();
        }

        public KhachHang GetById(int id)
        {
           return _context.KhachHangs.Find(id);
        }

        public bool Update(KhachHang obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj == null)
            {
                return false;
            }
            else
            {
                udobj.TaiKhoan=obj.TaiKhoan;
                udobj.MatKhau=obj.MatKhau;
                udobj.NgaySinh=obj.NgaySinh;
                udobj.Ten=obj.Ten;
                udobj.Sdt=obj.Sdt;
                udobj.Avatar=obj.Avatar;
                udobj.Email=obj.Email;
                udobj.TrangThai=obj.TrangThai;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
//public string TaiKhoan { get; set; } = null!;
//public string MatKhau { get; set; } = null!;
//[DataType(DataType.Date)]
//public DateTime? NgaySinh { get; set; } = null;
//public string Ten { get; set; } = string.Empty;
//public string Sdt { get; set; } = string.Empty;
//public string Avatar { get; set; } = string.Empty;
//public string Email { get; set; } = string.Empty;
//public bool TrangThai { get; set; } = true;