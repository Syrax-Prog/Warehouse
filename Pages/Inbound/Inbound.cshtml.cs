using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Models;

namespace Warehouse.Pages;

public class InboundModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public InboundModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Inbound> inbounds { get; set; } = new();
    public List<Item> items { get; set; } = new();
    public List<Supplier> suppliers { get; set; } = new();
    public List<InboundItem> inbound_items {get; set;} = new();
    public double totalInbounds { get; set; } = 0;
    public int pendingInbounds { get; set; } = 0;
    public int receivedInbounds { get; set; } = 0;
    public int totalUnits { get; set; } = 0;

    public async Task<IActionResult> OnGetAsync()
    {
        var userid = HttpContext.Session.GetString("id");
        if (userid == null)
        {
            return RedirectToPage("/Login/Login");
        }

        items = await _context.Items.ToListAsync();
        suppliers = await _context.Suppliers.ToListAsync();
        inbounds = await _context.Inbounds.ToListAsync();
        inbound_items = await _context.InboundItems.ToListAsync();

        totalInbounds = inbounds.Count();
        pendingInbounds = inbounds.Count(x => x.status == "Pending");
        receivedInbounds = inbounds.Count(x => x.status == "Received");
        totalUnits = inbound_items.Sum(x => x.quantity);

        var unitsByInbound = inbound_items.GroupBy(x => x.inbound_id).Select(x => new {inbound_id = x.Key, total_quantity = x.Sum(y => y.quantity)}).ToList();
        return Page();
    }
}