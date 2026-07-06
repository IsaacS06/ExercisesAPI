SELECT
ti.Id AS TrayItemId,
ti.TrayId,
ti.MenuItemId,
ti.Quantity,
mi.Description,
mi.Calories,
mi.Protein
FROM TrayItems ti
INNER JOIN MenuItems mi on ti.MenuItemId = mi.Id
ORDER BY ti.TrayId