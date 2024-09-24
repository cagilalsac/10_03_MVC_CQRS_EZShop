#nullable disable

using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class CategoryQuery
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class CategoryCommand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
