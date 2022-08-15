using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Parky2API.Model
{
    public class NationalParkDto
    {
        public int Id { get; set; }
        [Required]      
        public string Name { get; set; }
        [ValidateNever]
        public byte[]? Picture { get; set; }
        [Required]
        public string State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
    }
}
