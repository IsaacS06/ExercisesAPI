using ExercisesAPI.DAL.DomainClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MenuItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; } // generates FK
    [Required]
    public int CategoryId { get; set; }
    public int Calories { get; set; }
    public int Carbs { get; set; }
    public int Cholesterol { get; set; }
    public float Fat { get; set; }
    public int Fibre { get; set; }
    public string? Description { get; set; }
    public int Protein { get; set; }
    public int Salt { get; set; }
}
