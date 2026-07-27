using ExercisesAPI.DAL.DomainClasses;
using ExercisesAPI.DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace ExercisesAPI.DAL.DAO
{
    public class TrayDAO
    {
        readonly AppDbContext? _db;
        readonly UserDAO? _userDAO;

        public TrayDAO(AppDbContext context)
        {
            _db = context;
        }

        public TrayDAO(AppDbContext context, UserDAO userDAO)
        {
            _db = context;
            _userDAO = userDAO;
        }

        public async Task<int> CreateTray(TrayItem[] trayItems, string? email)
        {
            int trayId = -1;

            using (var transaction = _db!.Database.BeginTransaction())
            {
                try
                {
                    UserDAO userDAO = new(_db!);
                    User? user = await userDAO.GetByEmail(email);

                    if (user == null)
                    {
                        throw new Exception("User not found");
                    }

                    // Create the tray
                    Tray tray = new();
                    tray.DateCreated = DateTime.Now;
                    tray.UserID = user!.Id;

                    // Calculate totals
                    tray.TotalCalories = 0;
                    tray.TotalCarbs = 0;
                    tray.TotalCholesterol = 0;
                    tray.TotalFat = 0;
                    tray.TotalFibre = 0;
                    tray.TotalProtein = 0;
                    tray.TotalSalt = 0;

                    foreach (TrayItem item in trayItems)
                    {
                        // Get the full MenuItem details from database
                        var menuItem = await _db.MenuItems!.FindAsync(item.MenuItem!.Id);
                        if (menuItem != null)
                        {
                            tray.TotalCalories += menuItem.Calories * item.Quantity;
                            tray.TotalCarbs += menuItem.Carbs * item.Quantity;
                            tray.TotalCholesterol += menuItem.Cholesterol * item.Quantity;
                            tray.TotalFat += (decimal)(menuItem.Fat * item.Quantity);
                            tray.TotalFibre += menuItem.Fibre * item.Quantity;
                            tray.TotalProtein += menuItem.Protein * item.Quantity;
                            tray.TotalSalt += menuItem.Salt * item.Quantity;
                        }
                    }

                    // Save the tray with calculated totals
                    _db.Trays!.Add(tray);
                    await _db.SaveChangesAsync();

                    // Save each tray item
                    foreach (TrayItem item in trayItems)
                    {
                        item.TrayId = tray.Id;
                        item.MenuItemId = item.MenuItem!.Id;
                        item.MenuItem = null;

                        if (item.Timer == null)
                        {
                            item.Timer = new byte[8];
                        }

                        _db.TrayItems!.Add(item);
                        await _db.SaveChangesAsync();
                    }

                    trayId = tray.Id;
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error saving tray: {e.Message}");
                    transaction.Rollback();
                    throw;
                }
            }

            return trayId;
        }

        // Get tray with all items
        public async Task<Tray?> GetTrayWithItems(int trayId)
        {
            return await _db!.Trays!
                .Include(t => t.TrayItems)  //Tray has TrayItems property
                    .ThenInclude(ti => ti.MenuItem)
                .FirstOrDefaultAsync(t => t.Id == trayId);
        }

        // Get all trays for a user
        public async Task<List<Tray>> GetUserTrays(string email)
        {
            UserDAO userDAO = new(_db!);
            User? user = await userDAO.GetByEmail(email);

            if (user == null)
            {
                return new List<Tray>();
            }

            return await _db!.Trays!
                .Where(t => t.UserID == user.Id)
                .Include(t => t.TrayItems)
                    .ThenInclude(ti => ti.MenuItem)
                .OrderByDescending(t => t.DateCreated)
                .ToListAsync();
        }

        //  Get all trays with totals only (no items)
        public async Task<List<Tray>> GetUserTraysSummary(string email)
        {
            UserDAO userDAO = new(_db!);
            User? user = await userDAO.GetByEmail(email);

            if (user == null)
            {
                return new List<Tray>();
            }

            return await _db!.Trays!
                .Where(t => t.UserID == user.Id)
                .OrderByDescending(t => t.DateCreated)
                .ToListAsync();
        }

        // Get user tray history with DTO
        public async Task<List<TrayHistoryDTO>> GetUserTrayHistory(string email)
        {
            UserDAO userDAO = new(_db!);
            User? user = await userDAO.GetByEmail(email);
            if (user == null)
                return null;

            // Query the database for trays belonging to this user
            // Include TrayItems so we can count them
            var trays = await _db!.Trays!
                .Where(t => t.UserID == user.Id)
                .Include(t => t.TrayItems)
                .OrderByDescending(t => t.DateCreated)
                .Select(t => new TrayHistoryDTO
                {
                    Id = t.Id,
                    DateCreated = t.DateCreated,
                    TotalCalories = t.TotalCalories,
                    ItemCount = t.TrayItems.Count()
                })
                .ToListAsync();

            return trays;
        }

        // Get detailed tray information with DTO
        public async Task<TrayDetailsDTO> GetTrayDetails(int trayId)
        {
            var tray = await _db!.Trays!
                .Where(t => t.Id == trayId)
                .Include(t => t.TrayItems)
                    .ThenInclude(ti => ti.MenuItem)
                .FirstOrDefaultAsync();

            if (tray == null)
                return null;

            var trayDetails = new TrayDetailsDTO
            {
                Id = tray.Id,
                DateCreated = tray.DateCreated,
                TotalCalories = tray.TotalCalories,
                TotalProtein = tray.TotalProtein,
                TotalCarbs = tray.TotalCarbs,
                TotalFat = tray.TotalFat,
                Items = tray.TrayItems.Select(ti => new TrayItemDetailDTO
                {
                    MenuItemName = ti.MenuItem.Description,
                    Quantity = ti.Quantity,
                    Calories = ti.MenuItem.Calories * ti.Quantity,
                    Protein = ti.MenuItem.Protein * ti.Quantity,
                    Carbs = ti.MenuItem.Carbs * ti.Quantity,
                    Fat = ti.MenuItem.Fat * ti.Quantity
                }).ToList()
            };

            return trayDetails;
        }
    }
}