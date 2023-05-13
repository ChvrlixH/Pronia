using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Info
    {
        public int Id { get; set; }


        [MaxLength(100), Required]
        public string Title { get; set; }


        [MaxLength(100), Required]
        public string Description { get; set; }


        [Required]
        public string Image { get; set; }
   
    }
}
