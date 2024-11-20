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
    public class ChiTietHoaDonRepository : IChiTietHoaDonRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ChiTietHoaDonRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        public bool Add(ChiTietHoaDon obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.ChiTietHoaDons.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }
        public List<ChiTietHoaDon> GetAll()
        {
           return _context.ChiTietHoaDons.ToList();
        }
        public ChiTietHoaDon GetById(int id)
        {
           return _context.ChiTietHoaDons.Find(id);
        }
        public bool Update(ChiTietHoaDon obj)
        {
            if (obj== null)
            {
                return false;
            }
            else
            {
                int id=  obj.Id;
                var newobj=  GetById(id);
                if (newobj == null)
                {
                    return false;
                }
                else 
                { 
                    newobj.SoLuong = obj.SoLuong;
                    newobj.Gia=obj.Gia;
                    newobj.TrangThai = obj.TrangThai;
                    newobj.Id_ChiTietSanPham=obj.Id_ChiTietSanPham;
                    newobj.Id_HoaDon=obj.Id_HoaDon;
                    _context.SaveChanges();
                    return true;
                }
            }
        }
    }
}
