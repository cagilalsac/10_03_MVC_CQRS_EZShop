#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class UserQuery
    {
        public int Id { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Password { get; set; }

        [DisplayName("Status")]
        public string IsActive { get; set; }

        public string Role { get; set; }
    }

    public class UserCommand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Length(3, 10, ErrorMessage = "{0} must have minimum {1} maxiumum {2} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Length(3, 8, ErrorMessage = "{0} must have minimum {1} maxiumum {2} characters!")]
        public string Password { get; set; }

        [DisplayName("Status")]
        public bool IsActive { get; set; }

        [DisplayName("Role")]
        public int RoleId { get; set; }
    }
}
