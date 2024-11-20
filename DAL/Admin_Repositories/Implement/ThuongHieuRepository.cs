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
    public class ThuongHieuRepository : IThuongHieuRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ThuongHieuRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(ThuongHieu obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.ThuongHieus.Add(obj);
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
                udobj.TrangThai = false;
                _context.SaveChanges();
                return true;
            }
        }

        public List<ThuongHieu> GetAll()
        {
            return _context.ThuongHieus.ToList();
        }

        public ThuongHieu GetById(int id)
        {
            return _context.ThuongHieus.Find(id);
        }

        public bool Update(ThuongHieu obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj == null) 
            {
                return false;
            }
            else
            {
                udobj.Ten = obj.Ten;
                udobj.MoTa = obj.MoTa;
                udobj.TrangThai = obj.TrangThai;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
