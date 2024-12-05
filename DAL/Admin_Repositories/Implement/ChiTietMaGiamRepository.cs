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
    public class ChiTietMaGiamRepository : IChiTietMaGiamRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ChiTietMaGiamRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(ChiTietMaGiamGia obj)
        {
            if (obj == null) 
            { 
                return false;
            }
            else 
            {
                _context.ChiTietMaGiamGias.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        public List<ChiTietMaGiamGia> GetAll()
        {
            return _context.ChiTietMaGiamGias.ToList();
        }

        public ChiTietMaGiamGia GetById(int id)
        {
            return _context.ChiTietMaGiamGias.Find(id);
        }

        public bool Update(ChiTietMaGiamGia obj)
        {
            if (obj==null)
            {
                return false;
            }
            else
            {
                int id = obj.Id;
                var newobj = GetById(id);
                if (newobj == null)
                {
                    return false;
                }
                else
                {
                    newobj.Id_KhachHang = obj.Id_KhachHang;
                    newobj.NgaySuDung = obj.NgaySuDung;
                    newobj.NoiDung = obj.NoiDung;
                    newobj.Id_MaGiamGia = obj.Id_MaGiamGia;
                    _context.SaveChanges();
                    return true;
                }
            }
        }
    }
}
