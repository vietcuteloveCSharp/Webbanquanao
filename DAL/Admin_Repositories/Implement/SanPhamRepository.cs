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
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly WebBanQuanAoDbContext _context;
        public SanPhamRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        public bool Add(SanPham obj)
        {
           if (obj == null)
            {
                return false;
            }
            else
            {
                _context.SanPhams.Add(obj);
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
               
                udobj.TrangThai = false;/// trangj thái Xóa của sản phẩm 
               
                _context.SaveChanges();
                return true;
            }
        }

        public List<SanPham> GetAll()
        {
           return _context.SanPhams.ToList();
        }

        public SanPham GetById(int id)
        {
            return _context.SanPhams.Find(id);
        }

        public bool Update(SanPham obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj == null)
            {
                return false;

            }
            else
            {
                udobj.Ten = obj.Ten;
                udobj.MoTa = obj.MoTa;
                udobj.Gia = obj.Gia;
                udobj.SoLuong = obj.SoLuong;
                udobj.NgayCapNhat = obj.NgayCapNhat;
                udobj.NgayTao = obj.NgayTao;
                udobj.TrangThai = obj.TrangThai;
                udobj.HinhAnh = obj.HinhAnh;
                udobj.ThuongHieu = obj.ThuongHieu;
                udobj.DanhMuc = obj.DanhMuc;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
//public string Ten { get; set; } = string.Empty;
//public string MoTa { get; set; } = string.Empty;
//public string Gia { get; set; }
//public int SoLuong { get; set; } = 1;
//public DateTime NgayTao { get; set; } = DateTime.Now;
//public DateTime NgayCapNhat { get; set; }
//public bool TrangThai { get; set; } = true;
//public string HinhAnh { get; set; } = string.Empty;

//[ForeignKey("ThuongHieu")]
//public int Id_ThuongHieu { get; set; }
//[ForeignKey("DanhMuc")]
//public int Id_DanhMuc { get; set; }
