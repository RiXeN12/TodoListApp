using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models
{
    public class EditProfileViewModel
    {

        [Display(Name = "First Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }

}
