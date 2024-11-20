using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Implement
{
    public class MaGiamGiaRepository:IMaGiamGiaRepository   
    {
        private readonly WebBanQuanAoDbContext _context;

        public MaGiamGiaRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }

        public bool Add(MaGiamGia obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
              _context.MaGiamGias.Add(obj);
                _context.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<MaGiamGia> GetAll()
        {
            return _context.MaGiamGias.ToList();
        }

        public MaGiamGia GetById(int id)
        {
            return _context.MaGiamGias.Find(id);
        }

        public bool Update(MaGiamGia obj)
        {
            var udobj = GetById(obj.Id);
            if (udobj == null)
            {
                return false;
            }
            else
            {
                udobj.LoaiGiamGia=obj.LoaiGiamGia;
                udobj.DieuKienGiamGia  = obj.DieuKienGiamGia;
                udobj.GiaTriGiam= obj.GiaTriGiam;
                udobj.MenhGia=obj.MenhGia;
                udobj.NoiDung=obj.NoiDung;
                udobj.GiaTriToiDa=obj.GiaTriToiDa;
                udobj.TrangThai=obj.TrangThai;
                udobj.ThoiGianTao=obj.ThoiGianTao;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
//public int LoaiGiamGia { get; set; } // 0 - coupon || 1 - voucher
//public string DieuKienGiamGia { get; set; }// > 0 thì yêu cầu đơn hàng trên dk thì mới áp đc MaGiamGia || = 0 thì ko cần check đơn hàng
//public string GiaTriGiam { get; set; } // GiaTriGiam sẽ là %. 10, 20, 30 % đơn hàng
//public string MenhGia { get; set; } // Giá trị của MaGiamGia. 20000 vnd, 50000 vnd
//public string NoiDung { get; set; } = string.Empty;
//public string GiaTriToiDa { get; set; } // Giá trị tối đa của mã giảm giá
//public int TrangThai { get; set; } // 0 - chưa phát hành || 1 - đang phát hành || 2 - kết thúc
//public DateTime ThoiGianTao { get; set; } = DateTime.Now;
