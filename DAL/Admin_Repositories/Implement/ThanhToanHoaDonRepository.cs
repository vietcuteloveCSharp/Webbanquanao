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
    public class ThanhToanHoaDonRepository : IThanhToanHoaDonRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ThanhToanHoaDonRepository(WebBanQuanAoDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> Add(ThanhToanHoaDon obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                await _context.ThanhToanHoaDons.AddAsync(obj);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        
        public async Task<List<ThanhToanHoaDon>> GetAll()
        {
           return await _context.ThanhToanHoaDons.ToListAsync();
        }

        public async Task<ThanhToanHoaDon> GetById(int id)
        {
            return  await _context.ThanhToanHoaDons.FindAsync(id);
        }

        public async Task<bool> Update(int id ,ThanhToanHoaDon obj)
        {
            var udobj= await GetById(id);
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
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
