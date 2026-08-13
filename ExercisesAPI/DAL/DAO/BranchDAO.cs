using ExercisesAPI.DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace ExercisesAPI.DAL.DAO
{
    public class BranchDAO : IBranchDAO
    {
        readonly AppDbContext _db;

        public BranchDAO(AppDbContext context)
        {
            _db = context;
        }

        // Method 1: Get all active branches
        public async Task<List<BranchDTO>> GetAllActiveBranchesAsync()
        {
            return await _db.RestaurantBranches!
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .Select(b => new BranchDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    PhoneNumber = b.PhoneNumber ?? string.Empty,
                    OpeningHours = b.OpeningHours ?? string.Empty
                })
                .ToListAsync();
        }

        // Method 2: Get a specific branch by id
        public async Task<BranchDTO?> GetBranchByIdAsync(int id)
        {
            return await _db.RestaurantBranches!
                .Where(b => b.Id == id && b.IsActive)
                .Select(b => new BranchDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    PhoneNumber = b.PhoneNumber ?? string.Empty,
                    OpeningHours = b.OpeningHours ?? string.Empty
                })
                .FirstOrDefaultAsync();
        }

        // Method 3: Get all active branches with distance from the user, nearest first
        public async Task<List<BranchDTO>> GetBranchesWithDistanceAsync(double userLat, double userLon)
        {
            var branches = await GetAllActiveBranchesAsync();

            foreach (var branch in branches)
            {
                branch.DistanceFromUser = CalculateDistance(
                    userLat, userLon,
                    (double)branch.Latitude, (double)branch.Longitude);
            }

            return branches.OrderBy(b => b.DistanceFromUser).ToList();
        }

        // Method 4: Haversine formula - returns the distance between two coordinates in kilometers
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
