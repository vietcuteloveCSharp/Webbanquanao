using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.VuvietanhDTO.Mausacs
{
    public class FullMauSacDTO
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
       
        public string MaHex { get; set; } = string.Empty;
    }
}
