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
    public class PhuongThucThanhToanRepository : IPhuongThucThanhToanRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public PhuongThucThanhToanRepository(WebBanQuanAoDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> Add(PhuongThucThanhToan obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
               await _context.PhuongThucThanhToans.AddAsync(obj);
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
                udobj.TrangThai = false;///Xoa phuwowng thuc thanh toan
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<PhuongThucThanhToan>> GetAll()
        {
            return await _context.PhuongThucThanhToans.ToListAsync();
        }

        public async Task<PhuongThucThanhToan> GetById(int id)
        {
           return await _context.PhuongThucThanhToans.FindAsync(id);
        }

        public async Task<bool> Update(PhuongThucThanhToan obj)
        {
            var udobj=await GetById(obj.Id);
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
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
