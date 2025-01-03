using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebView.NghiaDTO
{
    public class ChiTietKhuyenMaiDTO
    {
        public int Id { get; set; }

  
        [ForeignKey("KhuyenMai")]
        public int? Id_KhuyenMai { get; set; } = null;
        [ForeignKey("DanhMuc")]
        public int? Id_DanhMuc { get; set; } 

        public virtual KhuyenMaiDTO KhuyenMaiDTO { get; set; }
        public virtual DanhMucDTO DanhMucDTO { get; set; }
    }
}
