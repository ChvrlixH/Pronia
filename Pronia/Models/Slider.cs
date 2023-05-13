using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Pronia.Models
{
    public class Slider
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }

        [Required, MaxLength(60)]
        public string Description { get; set; }

        public int Offer { get; set; }

        public string Image { get; set; }

    }
}
