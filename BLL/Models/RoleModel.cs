#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class RoleQuery
    {
        public int Id { get; set; }

        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }

    public class RoleCommand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(5, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string RoleName { get; set; }
    }
}
