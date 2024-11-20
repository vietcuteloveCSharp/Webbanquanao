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
    public class PhuongThucThanhToanRepository : IPhuongThucThanhToanRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public PhuongThucThanhToanRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(PhuongThucThanhToan obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.PhuongThucThanhToans.Add(obj);
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

                udobj.TrangThai = false;///Xoa phuwowng thuc thanh toan
                _context.SaveChanges();
                return true;
            }
        }

        public List<PhuongThucThanhToan> GetAll()
        {
            return _context.PhuongThucThanhToans.ToList();
        }

        public PhuongThucThanhToan GetById(int id)
        {
           return _context.PhuongThucThanhToans.Find(id);
        }

        public bool Update(PhuongThucThanhToan obj)
        {
            var udobj=GetById(obj.Id);
            if (udobj==null)
            {
                return false;
            }
            else
            {
                udobj.Ten = obj.Ten;
                udobj.Mota = obj.Mota;
                udobj.NgayTao = obj.NgayTao;
                udobj.TrangThai = obj.TrangThai;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
