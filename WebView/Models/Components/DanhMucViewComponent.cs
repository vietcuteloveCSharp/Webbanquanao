using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ShoppingDemo.Repository.Componnents
{
	public class DanhMucViewComponent : ViewComponent
	{
        private readonly DAL.Context.WebBanQuanAoDbContext _context;
        public DanhMucViewComponent(WebBanQuanAoDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhMucs = await _context.DanhMucs.ToListAsync();
            return View(danhMucs);
        }
    }
}
