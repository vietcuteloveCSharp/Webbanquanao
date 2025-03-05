using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebView.NghiaDTO
{
    public class ChucVuDTO
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string Mota { get; set; } = string.Empty;

        public virtual ICollection<NhanVienDTO> NhanVienDTOs{ get; set; }
    }
}
