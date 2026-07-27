using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using ExercisesAPI.DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrayController : ControllerBase
    {

        readonly AppDbContext? _ctx;

        public TrayController(AppDbContext context)
        {
            _ctx = context;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<int>> Index(TrayItem[] trayItems, string email)
        {
            int trayId = -1;
            try
            {
                TrayDAO trayDAO = new(_ctx!);
                trayId = await trayDAO.CreateTray(trayItems, email);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return trayId;
        }

        [HttpGet("history")]
        [Produces("application/json")]
        public async Task<ActionResult<List<TrayHistoryDTO>>> GetTrayHistory([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Email is required");

                var userDAO = new UserDAO(_ctx!);
                var trayDAO = new TrayDAO(_ctx!, userDAO);

                var history = await trayDAO.GetUserTrayHistory(email);

                if (history == null)
                    return NotFound($"User with email {email} not found");

                return Ok(history);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Error retrieving tray history: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<TrayDetailsDTO>> GetTrayDetails(int id)
        {
            try
            {
                var userDAO = new UserDAO(_ctx!);
                var trayDAO = new TrayDAO(_ctx!, userDAO);

                var trayDetails = await trayDAO.GetTrayDetails(id);

                if (trayDetails == null)
                    return NotFound($"Tray with ID {id} not found");

                return Ok(trayDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Error retrieving tray details: {ex.Message}");
            }
        }

        [HttpGet("last/foruser")]
        [Produces("application/json")]
        public async Task<ActionResult<int>> GetLastTrayIdForUser([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Email is required");

                var userDAO = new UserDAO(_ctx!);
                var user = await userDAO.GetByEmail(email);

                if (user == null)
                    return NotFound($"User with email {email} not found");

                // Get the most recent tray for this user
                var lastTray = await _ctx!.Trays!
                    .Where(t => t.UserID == user.Id)
                    .OrderByDescending(t => t.DateCreated)
                    .FirstOrDefaultAsync();

                if (lastTray == null)
                    return NotFound($"No trays found for user {email}");

                return Ok(lastTray.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Error retrieving last tray ID: {ex.Message}");
            }
        }
    }
}