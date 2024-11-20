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
    public class HoaDonRepository: IHoaDonRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public HoaDonRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(HoaDon obj)
        {
            if (obj== null )
            {
                return false;
            }
            else
            {
                _context.HoaDons.Add(obj);
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

                udobj.TrangThai = 0;///0 là hóa đơn bị hủy, 1 là chưa xác nhận , 2 là đã xác nhận
                _context.SaveChanges();
                return true;
            }
        }

        public List<HoaDon> GetAll()
        {
            return _context.HoaDons.ToList();
        }

        public HoaDon GetById(int id)
        {
           return _context.HoaDons.Find(id);
        }

        public bool Update(HoaDon obj)
        {
           var udobj= GetById(obj.Id);
            if (udobj==null)
            {
                return false;
            }
            else
            {
                udobj.TongTien= obj.TongTien;
                udobj.NgayTao= obj.NgayTao;
                udobj.TrangThai= obj.TrangThai;
                udobj.Id_NhanVien= obj.Id_NhanVien;
                udobj.Id_KhachHang  = obj.Id_KhachHang;
                _context.SaveChanges();
                return true;
            }
        }
    }
}

