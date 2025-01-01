using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebView.NghiaDTO
{
    public class ChiTietKhuyenMaiDTO
    {
        public int Id { get; set; }

        public int LoaiKhuyenMai { get; set; } // 0 - theo % || 1 - theo Giá
        public string GiaTriGiam { get; set; } // % giảm
        public decimal MenhGia { get; set; } // số tiền giảm
        public decimal GiaTriToiDa { get; set; }  // số tiền tối đa có thể giảm

        [ForeignKey("KhuyenMai")]
        public int? Id_KhuyenMai { get; set; } = null;
        [ForeignKey("DanhMuc")]
        public int? Id_DanhMuc { get; set; } = null;

        public virtual KhuyenMaiDTO KhuyenMaiDTO { get; set; }
        public virtual DanhMucDTO DanhMucDTO { get; set; }
    }
}
