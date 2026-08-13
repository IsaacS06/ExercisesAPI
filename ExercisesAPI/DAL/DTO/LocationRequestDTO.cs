namespace ExercisesAPI.DAL.DTO
{
    public class LocationRequestDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? MaxResults { get; set; } = 10;
    }
}
