using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Parky2API.Model.Dtos
{
    public class TrailUpdateDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }

        public Trail.DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        [Required]
        public double Elevation { get; set; }
    }
}
