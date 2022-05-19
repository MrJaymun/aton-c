using System.ComponentModel.DataAnnotations;

namespace UsersCrud.Request.UserRequest
{
    public class NewPasswordRequest
    {
        [Required]
        public string CreatorLogin { get; set; }
        [Required]
        public string CreatorPassword { get; set; }
        [Required]
        public string UserLogin { get; set; }
        [Required]
        public string NewPassword { get; set; }
        
    }
}
