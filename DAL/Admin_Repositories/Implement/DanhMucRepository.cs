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
    public class DanhMucRepository : IDanhMucRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public DanhMucRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(DanhMuc obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.DanhMucs.Add(obj);
                _context .SaveChanges();
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
               
                udobj.TrangThai =false;/////False là trạng thái xóa của danh mục
                _context.SaveChanges();
                return true;
            }
        }

        public List<DanhMuc> GetAll()
        {
            return _context.DanhMucs.ToList();
        }

        public DanhMuc GetById(int id)
        {
            return _context.DanhMucs.Find(id);
        }

        public bool Update(DanhMuc obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj == null)
            {
             return false ;
            }
            else
            {
                udobj.Id_DanhMucCha =obj.Id_DanhMucCha;
                udobj.TenDanhMuc = obj.TenDanhMuc;
                udobj.NgayTao = obj.NgayTao;
                udobj.TrangThai = obj.TrangThai;
                _context .SaveChanges();
                return true;
            }
        }
    }
}
