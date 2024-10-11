using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ChucVu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string Mota { get; set; } = string.Empty;

        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
