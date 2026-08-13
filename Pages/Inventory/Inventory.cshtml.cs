using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Models;

namespace Warehouse.Pages;

public class InventoryModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public InventoryModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int allItems { get; set; } = 0;
    public int lowStock { get; set; } = 0;
    public decimal invValue { get; set; } = 0;
    public DateTime lastCreate { get; set; } = DateTime.Now;

    public List<Item> items { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string? filter)
    {
        var userId = HttpContext.Session.GetInt32("id");

        if (userId == null)
        {
            return RedirectToPage("/Login/Login");
        }

        Name = HttpContext.Session.GetString("name") ?? "";
        Email = HttpContext.Session.GetString("email") ?? "";

        // Build the base query once; nothing executes yet.
        IQueryable<Item> query = _context.Items;

        if (filter == "lowstock")
        {
            query = query.Where(x => x.quantity < 20);
        }

        items = await query
            .OrderBy(x => x.id)
            .ToListAsync();

        // These always reflect the WHOLE inventory, regardless of filter —
        // the summary cards shouldn't shrink just because you're viewing a filtered list.
        allItems = await _context.Items.CountAsync();
        lowStock = await _context.Items.CountAsync(x => x.quantity < 20);
        invValue = await _context.Items.SumAsync(x => x.price * x.quantity);
        lastCreate = await _context.Items.AnyAsync()
            ? await _context.Items.MaxAsync(x => x.createdate)
            : DateTime.MinValue;

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (id <= 0)
        {
            HttpContext.Session.SetString("Message", "Invalid item id.");
            return RedirectToPage("/Inventory/Inventory");
        }

        var item = await _context.Items.FindAsync(id);
        if (item == null)
        {
            HttpContext.Session.SetString("Message", "Item " + id + " Not Found");
            return RedirectToPage("/Inventory/Inventory");
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("Message", "Item " + id + " Was Deleted Successfully");

        return RedirectToPage("/Inventory/Inventory");
    }

    public async Task<IActionResult> OnPostEditAsync(int id, string name, int quantity, decimal price)
    {
        if (id < 0) HttpContext.Session.SetString("Message", "Item " + id + " Was Deleted Successfully");

        var item = await _context.Items.FindAsync(id);
        if (item == null)
        {
            HttpContext.Session.SetString("Message", "Item " + id + " Not Found");
            return RedirectToPage("/Inventory/Inventory");
        }

        if (string.IsNullOrWhiteSpace(name) || quantity < 0 || price < 0)
        {
            HttpContext.Session.SetString("Message", "Invalid item data.");
            return RedirectToPage("/Inventory/Inventory");
        }

        item.name = name;
        item.quantity = quantity;
        item.price = price;
        item.updateby = HttpContext.Session.GetString("name") ?? "Unknown";
        item.updatedate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("Message", "User " + id + " Was Updated Successfully");

        return RedirectToPage("/Inventory/Inventory");
    }
}