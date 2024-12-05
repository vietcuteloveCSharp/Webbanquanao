using DAL.Admin_Repositories.Interface;
using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Implement
{
    public class KhuyenMaiRepository : IKhuyenMaiRepository
    {
        private readonly WebBanQuanAoDbContext _context;

        public KhuyenMaiRepository(WebBanQuanAoDbContext context)
        {
            _context = context;
        }
        
        public bool Add(KhuyenMai obj)
        {
            if (obj == null)
            {
                return false;

            }
            else
            {
                _context.KhuyenMais.Add(obj);
                return true;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<KhuyenMai> GetAll()
        {
           return _context.KhuyenMais.ToList();
        }

        public KhuyenMai GetById(int id)
        {
           return _context.KhuyenMais.Find(id);
        }

        public bool Update(KhuyenMai obj)
        {
            var udobj= GetById(obj.Id);
            if (udobj==null)
            {
                return false;
            }
            else
            {
                udobj.Ten = obj.Ten;
                udobj.MoTa = obj.MoTa;
                udobj.NgayTao = obj.NgayTao;    
                udobj.NgayBatDau = obj.NgayBatDau;
                udobj.NgayKetThuc  = obj.NgayKetThuc;   
                udobj.TrangThai = obj.TrangThai;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
//public string Ten { get; set; } = string.Empty;
//public string MoTa { get; set; } = string.Empty;
//public DateTime NgayTao { get; set; } = DateTime.Now;
//public DateTime NgayBatDau { get; set; }
//public DateTime NgayKetThuc { get; set; }
//public bool TrangThai { get; set; } // false - ngừng khuyến mại || true - đang khuyến mại