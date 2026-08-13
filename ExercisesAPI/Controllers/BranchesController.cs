using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        readonly IBranchDAO _branchDAO;

        public BranchesController(IBranchDAO branchDAO)
        {
            _branchDAO = branchDAO;
        }

        // GET /api/branches - Get all active branches
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<BranchDTO>>> GetAll()
        {
            try
            {
                var branches = await _branchDAO.GetAllActiveBranchesAsync();
                return Ok(branches);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Error retrieving branches: {ex.Message}");
            }
        }

        // GET /api/branches/{id} - Get a specific branch
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<BranchDTO>> GetById(int id)
        {
            try
            {
                var branch = await _branchDAO.GetBranchByIdAsync(id);

                if (branch == null)
                    return NotFound($"Branch with ID {id} not found");

                return Ok(branch);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Error retrieving branch: {ex.Message}");
            }
        }

        // POST /api/branches/nearest - Find the nearest branch to the user
        [HttpPost("nearest")]
        [Produces("application/json")]
        public async Task<ActionResult<NearestBranchResponseDTO>> FindNearest(LocationRequestDTO request)
        {
            try
            {
                var branches = await _branchDAO.GetBranchesWithDistanceAsync(
                    request.Latitude, request.Longitude);

                if (branches == null || branches.Count == 0)
                    return NotFound("No branches found");

                // branches are already ordered nearest-first by the DAO
                var response = new NearestBranchResponseDTO
                {
                    NearestBranch = branches.First(),
                    AllBranches = branches,
                    MinimumDistance = branches.First().DistanceFromUser
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Error finding nearest branch: {ex.Message}");
            }
        }

        // POST /api/branches/nearby - Find the closest branches (limited by MaxResults)
        [HttpPost("nearby")]
        [Produces("application/json")]
        public async Task<ActionResult<List<BranchDTO>>> FindNearby(LocationRequestDTO request)
        {
            try
            {
                var branches = await _branchDAO.GetBranchesWithDistanceAsync(
                    request.Latitude, request.Longitude);

                int max = request.MaxResults ?? 10;
                var nearby = branches.Take(max).ToList();

                return Ok(nearby);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Error finding nearby branches: {ex.Message}");
            }
        }
    }
}
