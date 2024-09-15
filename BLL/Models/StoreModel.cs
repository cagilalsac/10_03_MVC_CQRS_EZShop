#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class StoreQuery
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Virtual")]
        public string IsVirtual { get; set; }

        [DisplayName("Product Count")]
        public string ProductCount { get; set; }

        public string Products { get; set; }

        [DisplayName("Based Country")]
        public string Country { get; set; }

        [DisplayName("Based City")]
        public string City { get; set; }
    }

    public class StoreCommand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(5, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(150, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [DisplayName("Virtual")]
        public bool IsVirtual { get; set; }

        [DisplayName("Based Country")]
        public int? CountryId { get; set; }

        [DisplayName("Based City")]
        public int? CityId { get; set; }
    }
}
