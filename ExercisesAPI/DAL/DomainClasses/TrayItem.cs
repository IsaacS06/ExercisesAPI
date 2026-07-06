using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExercisesAPI.DAL.DomainClasses
{
    public class TrayItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("TrayId")]
        public Tray? Tray { get; set; }
        [Required]
        public int TrayId { get; set; }


        [ForeignKey("MenuItemId")]
        public MenuItem? MenuItem { get; set; }
        [Required]
        public int MenuItemId { get; set; }

        public int Quantity { get; set; }
        public byte[]? Timer { get; set; }
    }
}
