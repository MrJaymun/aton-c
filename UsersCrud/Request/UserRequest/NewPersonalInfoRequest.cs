using System.ComponentModel.DataAnnotations;

namespace UsersCrud.Request.UserRequest
{
    public class NewPersonalInfoRequest
    {
       [Required]
       public string CreatorLogin { get; set; }
       [Required]
       public string CreatorPassword { get; set; }
       [Required]
       public string UserLogin { get; set; }
       public string? NewUserName { get; set; }
       public DateTime? NewUserDateOfBirth { get; set; }
       public int? NewUserGender { get; set; }

    }
}
