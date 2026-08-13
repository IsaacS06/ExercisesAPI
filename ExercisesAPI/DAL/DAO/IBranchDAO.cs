using ExercisesAPI.DAL.DTO;

namespace ExercisesAPI.DAL.DAO
{
    public interface IBranchDAO
    {
        Task<List<BranchDTO>> GetAllActiveBranchesAsync();
        Task<BranchDTO?> GetBranchByIdAsync(int id);
        Task<List<BranchDTO>> GetBranchesWithDistanceAsync(double userLat, double userLon);
    }
}
