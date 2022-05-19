using System.ComponentModel.DataAnnotations;

namespace UsersCrud.Response.UserResponse
{
    public class UserMainInfoResponse
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
