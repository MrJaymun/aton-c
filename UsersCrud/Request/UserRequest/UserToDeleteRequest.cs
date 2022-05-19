using System.ComponentModel.DataAnnotations;

namespace UsersCrud.Request.UserRequest
{
    public class UserToDeleteRequest
    {
        [Required]
        public string CreatorLogin { get; set; }
        [Required]
        public string CreatorPassword { get; set; }
        [Required]
        public string UserLogin { get; set; }
        [Required]
        public bool IsSoft { get; set; }
    }
}
