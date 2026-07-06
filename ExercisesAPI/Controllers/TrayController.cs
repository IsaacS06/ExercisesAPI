using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}