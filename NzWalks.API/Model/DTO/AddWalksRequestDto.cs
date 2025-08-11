using System.ComponentModel.DataAnnotations;

namespace NzWalks.API.Model.DTO
{
    public class AddWalksRequestDto
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = "Description has to be a maximum of 1000 characters")]
        public string Description { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0,500, ErrorMessage = "Length must be between 0 and 500 kilometers")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
