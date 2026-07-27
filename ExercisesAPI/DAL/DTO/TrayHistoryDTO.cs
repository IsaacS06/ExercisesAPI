namespace ExercisesAPI.DAL.DTO
{
    public class TrayHistoryDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TotalCalories { get; set; }
        public int ItemCount { get; set; }
    }
}
