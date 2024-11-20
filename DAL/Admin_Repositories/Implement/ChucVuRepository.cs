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
    public class ChucVuRepository : IChucVuRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public ChucVuRepository(WebBanQuanAoDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> Add(ChucVu obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                await _context.ChucVus.AddAsync(obj);
                await _context.SaveChangesAsync(); 
                return true;
            } 
            
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChucVu>> GetAll()
        {
            return await _context.ChucVus.ToListAsync();   
        }

        public async Task<ChucVu> GetById(int id)
        {
            return await _context.ChucVus.FindAsync(id);
        }

        public async Task<bool> Update(ChucVu obj)
        {
            var udobj = await _context.ChucVus.FindAsync(obj.Id);
            if (udobj == null)
            {
                return false;
            }
            else 
            {
                udobj.Ten= obj.Ten;
                udobj.Mota= obj.Mota;
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
