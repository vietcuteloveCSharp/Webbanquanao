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
    public class ThuongHieuRepository : IThuongHieuRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ThuongHieuRepository(WebBanQuanAoDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> Add(ThuongHieu obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                await _context.ThuongHieus.AddAsync(obj);
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
                udobj.TrangThai = false;
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<ThuongHieu>> GetAll()
        {
            return await _context.ThuongHieus.ToListAsync();
        }

        public async Task<ThuongHieu> GetById(int id)
        {
            return await _context.ThuongHieus.FindAsync(id);
        }

        public async Task<bool> Update(int id,ThuongHieu obj)
        {
            var udobj = await GetById(obj.Id);
            if (udobj == null) 
            {
                return false;
            }
            else
            {
                udobj.Ten = obj.Ten;
                udobj.MoTa = obj.MoTa;
                udobj.TrangThai = obj.TrangThai;
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
