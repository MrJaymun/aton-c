using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCrudModels
{
    public class UserDto
    {
       
         [Required]
        public Guid Guid { get; set; }
      
        [Required]
        public string Login { get; set; }
       
        [Required]
        public string Password { get; set; }
       
        [Required]
        public string Name { get; set; }
       
        [Required]
        public int Gender { get; set; }
      
        public DateTime? Birthday { get; set; }
       
        [Required]
        public bool Admin { get; set; }
       
        [Required]
        public DateTime CreatedOn { get; set; }
       
        [Required]
        public string CreatedBy { get; set; }
        
        public DateTime? ModifiedOn { get; set; }
       
        public string? ModifiedBy { get; set; }
       
        public DateTime? RevokedOn { get; set; }

        public string? RevokedBy { get; set; }
    }
}
