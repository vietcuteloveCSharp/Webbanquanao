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
    public class ChiTietSanPhamRepository : IChiTietSanPhamRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ChiTietSanPhamRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(ChiTietSanPham obj)
        {
            if (obj == null) 
            {   
                return false;
            }
            else
            {
                _context.ChiTietSanPhams.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        public ChiTietSanPham Get(int id)
        {
            return _context.ChiTietSanPhams.Find(id);
        }

        public List<ChiTietSanPham> GetAll()
        {
            return _context.ChiTietSanPhams.ToList();
        }

        public bool Update(ChiTietSanPham obj)
        {
            var udobj= Get(obj.Id);
            if (udobj== null)
            {
                return false;
            }
            else
            {
                udobj.SoLuong = obj.SoLuong;
                udobj.NgayTao = obj.NgayTao;
                udobj.TrangThai = obj.TrangThai;
                udobj.Id_SanPham = obj.Id_SanPham;
                udobj.Id_MauSac = obj.Id_MauSac;
                udobj.Id_KichThuoc  = obj.Id_KichThuoc;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
//public int Id { get; set; }
//public int SoLuong { get; set; }
//public DateTime NgayTao { get; set; } = DateTime.Now;
//public bool TrangThai { get; set; } = true;

//[ForeignKey("SanPham")]
//public int Id_SanPham { get; set; }
//[ForeignKey("MauSac")]
//public int Id_MauSac { get; set; }
//[ForeignKey("KichThuoc")]
//public int Id_KichThuoc { get; set; }