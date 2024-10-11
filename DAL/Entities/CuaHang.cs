using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class CuaHang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        [MaxLength(15)]
        public string Sdt { get; set; } = string.Empty;
        [MaxLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; } = DateTime.Now;

    }
}
