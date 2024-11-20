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
    public class MauSacRepository : IMauSacRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public MauSacRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(MauSac obj)
        {
           if (obj == null) { return false;}
            else
            {
                _context.MauSacs.Add(obj);
                return true;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<MauSac> GetAll()
        {
            return _context.MauSacs.ToList();
        }

        public MauSac GetById(int id)
        {
            return _context.MauSacs.Find(id);
        }

        public bool Update(MauSac obj)
        {
            var udobj = GetById(obj.Id);

            if (udobj== null)
            {
                return false;
            }
            else
            {
                udobj.Ten= obj.Ten;
                udobj.MaHex= obj.MaHex;
                _context.SaveChanges();
                return true;    
            }
        }
    }
}
