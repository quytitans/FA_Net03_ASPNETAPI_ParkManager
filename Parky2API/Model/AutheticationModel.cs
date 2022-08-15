using System.ComponentModel.DataAnnotations;

namespace Parky2API.Model
{
    public class AutheticationModel
    {
        [Required]
        public string UserName { get; set; }
        [Required] 
        public string Password { get; set; }
    }
}
