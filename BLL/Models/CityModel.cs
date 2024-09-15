#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class CityQuery
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }
    }

    public class CityCommand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(125, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [DisplayName("Country")]
        public int CountryId { get; set; }
    }
}
