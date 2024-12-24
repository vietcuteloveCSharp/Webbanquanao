using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class HinhAnh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? ImageData { get; set; }
        public int? Id_SanPham { get; set; }
    }
}
