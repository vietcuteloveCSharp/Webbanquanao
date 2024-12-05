using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShoppingDemo.Repository.Componnents
{
	public class ThuongHieuViewComponent : ViewComponent
	{
        private readonly DAL.Context.WebBanQuanAoDbContext _context;
        public ThuongHieuViewComponent(WebBanQuanAoDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var thuongHieus = await _context.ThuongHieus.ToListAsync();
            return View(thuongHieus);
        }   
    }
}
