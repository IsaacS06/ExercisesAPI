using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExercisesAPI.DAL.DomainClasses
{
    public class Tray
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }

        public DateTime DateCreated { get; set; }

        public int TotalCalories { get; set; }
        public int TotalCarbs { get; set; }
        public int TotalCholesterol { get; set; }
        [Column(TypeName = "decimal(9, 2)")]
        public decimal TotalFat { get; set; }
        public int TotalFibre { get; set; }
        public int TotalProtein { get; set; }
        public int TotalSalt { get; set; }
        public byte[]? Timer { get; set; }

        public ICollection<TrayItem>? TrayItems { get; set; }
    }
}
