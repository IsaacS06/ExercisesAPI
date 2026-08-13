using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExercisesAPI.DAL.DomainClasses
{
    public class RestaurantBranch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        // Stored as decimals; precision configured via Fluent API in AppDbContext
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string? PhoneNumber { get; set; }

        public string? OpeningHours { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
