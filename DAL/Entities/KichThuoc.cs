using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class KichThuoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;

        public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; }
    }
}
