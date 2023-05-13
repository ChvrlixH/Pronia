using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels
{
    public class SliderVM
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }

        [Required, MaxLength(60)]
        public string Description { get; set; }

        public int Offer { get; set; }

        public IFormFile? Image { get; set; }
    }
}
