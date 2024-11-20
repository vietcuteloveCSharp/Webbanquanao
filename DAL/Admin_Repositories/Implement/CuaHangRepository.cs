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

    public class CuaHangRepository : ICuaHangRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public CuaHangRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(CuaHang obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.CuaHangs.Add(obj);
                _context.SaveChanges();
                return true;
            }   
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CuaHang> GetAll()
        {
            return _context.CuaHangs.ToList();  
        }

        public CuaHang GetById(int id)
        {
            return _context.CuaHangs.Find(id);
        }

        public bool Update(CuaHang obj)
        {
            var udobj=  GetById(obj.Id);
            if (udobj== null)
            {
                return false;

            }
            else
            {
                udobj.Ten= obj.Ten;
                udobj.DiaChi= obj.DiaChi;
                udobj.Sdt = obj.Sdt;
                udobj.Email = obj.Email;
                udobj.NgayTao= obj.NgayTao;
                _context.SaveChanges();
                return true;

            }
        }
    }
}
