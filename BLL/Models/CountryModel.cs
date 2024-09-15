#nullable disable

using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class CountryQuery
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class CountryCommand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
    }
}
