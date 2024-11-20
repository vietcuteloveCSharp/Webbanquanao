using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Implement
{
    public class ThanhToanHoaDonRepository : IThanhToanHoaDonRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ThanhToanHoaDonRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(ThanhToanHoaDon obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _context.ThanhToanHoaDons.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        
        public List<ThanhToanHoaDon> GetAll()
        {
           return _context.ThanhToanHoaDons.ToList();
        }

        public ThanhToanHoaDon GetById(int id)
        {
            return _context.ThanhToanHoaDons.Find(id);
        }

        public bool Update(ThanhToanHoaDon obj)
        {
            var udobj= GetById(obj.Id);
            if (udobj== null)
            {
                return false;
            }
            else
            {
                udobj.TongTien = obj.TongTien;  
                udobj.SoTienDaThanhToan =obj.SoTienDaThanhToan;
                udobj.NgayThanhToan = obj.NgayThanhToan;
                udobj.MaGiaoDich  = obj.MaGiaoDich;
                udobj.Id_HoaDon = obj.Id_HoaDon;
                udobj.Id_PhuongThucThanhToan = obj.Id_PhuongThucThanhToan;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
//public string TongTien { get; set; }
//public string SoTienDaThanhToan { get; set; }
//public DateTime NgayThanhToan { get; set; } = DateTime.Now;
//public string MaGiaoDich { get; set; } = string.Empty;

//[ForeignKey("HoaDon")]
//public int Id_HoaDon { get; set; }
//[ForeignKey("PhuongThucThanhToan")]
//public int Id_PhuongThucThanhToan { get; set; }