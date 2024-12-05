using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> Add(HoaDon obj)
        {
            if (obj== null )
            {
                return false;
            }
            else
            {
                await _context.HoaDons.AddAsync(obj);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var udobj= await _context.HoaDons.FindAsync(id);
            if (udobj == null)
            {
                return false;
            }
            else
            {

                udobj.TrangThai = 0;///0 là hóa đơn bị hủy, 1 là chưa xác nhận , 2 là đã xác nhận
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<HoaDon>> GetAll()
        {
            return await _context.HoaDons.ToListAsync();
        }

        public async Task<HoaDon> GetById(int id)
        {
            return await _context.HoaDons.FindAsync(id);
        }

        public async Task<bool> Update(int id, HoaDon obj)
        {
           var udobj= await _context.HoaDons.FindAsync(id);
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
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}

