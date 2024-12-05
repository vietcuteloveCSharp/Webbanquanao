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
    public class DanhMucRepository : IDanhMucRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public DanhMucRepository(WebBanQuanAoDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> Add(DanhMuc obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                await _context.DanhMucs.AddAsync(obj);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {

            var udobj = await _context.DanhMucs.FindAsync(id);
            if (udobj == null)
            {
                return false;
            }
            else
            {
                udobj.TrangThai =false;/////False là trạng thái xóa của danh mục
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<DanhMuc>> GetAll()
        {

            return  await _context.DanhMucs.ToListAsync();
        }

        public async Task<DanhMuc>GetById(int id)
        {
            return await _context.DanhMucs.FindAsync(id);
        }

        public async Task<bool> Update(int id, DanhMuc obj)
        {
            var udobj = await _context.DanhMucs.FindAsync(id);
            if (udobj == null)
            {
             return false ;
            }
            else
            {
                udobj.Id_DanhMucCha =obj.Id_DanhMucCha;
                udobj.TenDanhMuc = obj.TenDanhMuc;
                udobj.NgayTao = obj.NgayTao;
                udobj.TrangThai = obj.TrangThai;
                await _context .SaveChangesAsync();
                return true;
            }
        }
    }
}
