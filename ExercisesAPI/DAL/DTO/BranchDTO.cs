namespace ExercisesAPI.DAL.DTO
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string OpeningHours { get; set; } = string.Empty;
        public double DistanceFromUser { get; set; }
    }
}
