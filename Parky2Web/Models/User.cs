using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Parky2Web.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
