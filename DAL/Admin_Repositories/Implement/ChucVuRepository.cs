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
    public class ChucVuRepository : IChucVuRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ChucVuRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(ChucVu obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.ChucVus.Add(obj);
                _context.SaveChanges(); 
                return true;
            } 
            
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<ChucVu> GetAll()
        {
            return _context.ChucVus.ToList();   
        }

        public ChucVu GetById(int id)
        {
            return _context.ChucVus.Find(id);
        }

        public bool Update(ChucVu obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj == null)
            {
                return false;
            }
            else 
            {
                udobj.Ten= obj.Ten;
                udobj.Mota= obj.Mota;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
