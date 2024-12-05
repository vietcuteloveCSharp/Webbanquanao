using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
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
            this._context = context;
        }
        public async Task<bool> Add(SanPham obj)
        {
           if (obj == null)
            {
                return false;
            }
            else
            {
                await _context.SanPhams.AddAsync(obj);
                await _context.SaveChangesAsync();
                return true;
            }

        }

        public async Task<bool> Delete(int id)
        {
            var udobj = await GetById(id);
            if (udobj == null)
            {
                return false;

            }
            else
            {
               
                udobj.TrangThai = false;/// trangj thái Xóa của sản phẩm 
               
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<SanPham>> GetAll()
        {
           return await _context.SanPhams.ToListAsync();
        }

        public async Task<SanPham> GetById(int id)
        {
            return await _context.SanPhams.FindAsync(id);
        }

        public async Task<bool> Update(int id,SanPham obj)
        {
            var udobj = await GetById(id);
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
                await _context.SaveChangesAsync();
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
