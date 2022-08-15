using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Parky2API.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
        [NotMapped]
        [ValidateNever]
        public string? Token { get; set; }
    }
}
