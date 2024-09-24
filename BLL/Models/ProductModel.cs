#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class ProductQuery
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPrice { get; set; }

        [DisplayName("Stock Amount")]
        public string StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDate { get; set; }

        public string Category { get; set; }
        public string Stores { get; set; }
    }

    public class ProductCommand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Length(2, 150, ErrorMessage = "{0} must have minimum {1} maxiumum {2} characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be a positive number!")]
        [DisplayName("Unit Price")]
        public decimal? UnitPrice { get; set; }

        [Range(0, 100000, ErrorMessage = "{0} must be minimum {1} maximum {2}!")]
        [DisplayName("Stock Amount")]
        public int? StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Stores")]
        public List<int> StoreIds { get; set; }
    }
}
