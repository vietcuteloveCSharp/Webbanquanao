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
    public class ChiTietKhuyenMaiRepository : IChiTietKhuyenMaiRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ChiTietKhuyenMaiRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(ChiTietKhuyenMai obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            { 
                _context.ChiTietKhuyenMais.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        

        public List<ChiTietKhuyenMai> GetAll()
        {
           return  _context.ChiTietKhuyenMais.ToList();
        }

        public ChiTietKhuyenMai GetById(int id)
        {
            return _context.ChiTietKhuyenMais.Find(id);
        }

        public bool Update(ChiTietKhuyenMai obj)
        {
            if (obj ==null)
            {
                return false;
            }
            else
            {
                int id  = obj.Id;
                var newobj = GetById(id);
                if (newobj == null)
                {
                    return false;
                }
                else 
                {
                    
                    newobj.LoaiKhuyenMai = obj.LoaiKhuyenMai;
                    newobj.GiaTriGiam = obj.GiaTriGiam;
                    newobj.GiaTriToiDa=obj.GiaTriToiDa;
                    newobj.MenhGia=obj.MenhGia;
                    newobj.Id_KhuyenMai=obj.Id_KhuyenMai;
                    newobj.Id_DanhMuc=obj.Id_DanhMuc;
                    _context.SaveChanges();
                    return true;
                }

            }
        }
    }
}
