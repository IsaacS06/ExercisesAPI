namespace ExercisesAPI.DAL.DTO
{
    public class TrayDetailsDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TotalCalories { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalCarbs { get; set; }
        public decimal TotalFat { get; set; }
        public List<TrayItemDetailDTO> Items { get; set; }
    }

    public class TrayItemDetailDTO
    {
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public float Fat { get; set; }
    }
}
