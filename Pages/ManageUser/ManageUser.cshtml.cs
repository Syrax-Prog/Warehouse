using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Models;

namespace Warehouse.Pages;

public class ManageUserModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ManageUserModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<User> users {get; set;} = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("id");

        if(userId == null)
        {
            return RedirectToPage("/Login/Login");
        }

        users = await _context.Users
                .OrderBy(x => x.name)
                .ToListAsync();

        return Page();
    }
}