using System.ComponentModel.DataAnnotations;

namespace UsersCrud.Request.UserRequest
{
    public class NewUserRequest
    {
        [Required]
        public string CreatorLogin { get; set; }
        [Required]
        public string CreatorPassword { get; set; }
        [Required]
        public string NewUserLogin { get; set; }
        [Required]
        public string NewUserPassword { get; set; }
        [Required]
        public string NewUserName { get; set; }
        [Required]
        public int NewUserGender { get; set; }
        public DateTime? NewUserDateOfBirth { get; set; }
        [Required]
        public bool NewUserIsAdmin { get; set; }
    }
}
