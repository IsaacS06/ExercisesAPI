namespace ExercisesAPI.DAL.DTO
{
    public class NearestBranchResponseDTO
    {
        public BranchDTO? NearestBranch { get; set; }
        public List<BranchDTO>? AllBranches { get; set; }
        public double MinimumDistance { get; set; }
    }
}
