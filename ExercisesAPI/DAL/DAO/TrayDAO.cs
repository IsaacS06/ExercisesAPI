using ExercisesAPI.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace ExercisesAPI.DAL.DAO
{
    public class TrayDAO
    {
        readonly AppDbContext? _db;

        public TrayDAO(AppDbContext context)
        {
            _db = context;
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
    }
}