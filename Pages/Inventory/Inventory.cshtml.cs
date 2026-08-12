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
    public int allItems {get; set;} = 0; 
    public int lowStock {get; set;} = 0;
    public decimal invValue {get; set;} = 0;
    public DateTime lastCreate { get; set; } = DateTime.Now;

    public List<Item> items {get; set;} = new();

    public async Task<IActionResult> OnGetAsync(int pageNum)
    {
        var userId = HttpContext.Session.GetInt32("id");

        if(userId == null)
        {
            return RedirectToPage("/Login/Login");
        }

        Name = HttpContext.Session.GetString("name") ?? "";
        Email = HttpContext.Session.GetString("email") ?? "";

        //ambil total row
        var totalItems = await _context.Items.CountAsync();

        items = await _context.Items
                    .OrderBy(x => x.id)
                    .ToListAsync();

        //for the card
        allItems = await _context.Items.CountAsync();

        //count low stock
        lowStock = await _context.Items.CountAsync(x => x.quantity < 20);

        //inventory value
        invValue = await _context.Items.SumAsync(x => x.price * x.quantity);

        //get last created date
        lastCreate = await _context.Items.MaxAsync(x => x.createdate);

        return Page();
    }
}