using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Implement
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly WebBanQuanAoDbContext _context;
        public bool Add(GioHang obj)
        {
            if (obj == null) { return false; }
            else
            {
                _context.GioHangs.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        public bool Delete(GioHang obj)
        {
            throw new NotImplementedException();
        }

        public List<GioHang> GetAll()
        {
            return _context.GioHangs.ToList();
        }

        public GioHang GetById(int id)
        {
            return _context.GioHangs.Find(id);
        }

        public bool Update(GioHang obj)
        {
           var udobj= GetById(obj.Id);
            if (udobj== null)
            {
                return false;
            }
            else
            {
                udobj.SoLuong = obj.SoLuong;
                udobj.TrangThai = obj.TrangThai;
                udobj.Id_KhachHang= obj.Id_KhachHang;
                udobj.Id_ChiTietSanPham = obj.Id_ChiTietSanPham;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
