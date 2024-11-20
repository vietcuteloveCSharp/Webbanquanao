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
    public class KichThuocRepository : IKichThuocRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public KichThuocRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(KichThuoc obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.KichThuocs.Add(obj);
                return true;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<KichThuoc> GetAll()
        {
           return _context.KichThuocs.ToList();
        }

        public KichThuoc GetById(int id)
        {
            return _context.KichThuocs.Find(id);
        }

        public bool Update(KichThuoc obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj== null)
            {
               
                return false;
            }
            else
            {
                udobj.Ten = obj.Ten;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
