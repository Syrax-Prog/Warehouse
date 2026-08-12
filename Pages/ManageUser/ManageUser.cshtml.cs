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
    public int totalAll {get; set;} = 0;
    public int totalSuper {get; set;} = 0;
    public int totalAdmin {get; set;} = 0;
    public int totalUser {get; set;} = 0;

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

        totalAll = await _context.Users.CountAsync();
        totalSuper = await _context.Users.CountAsync(x => x.role == "Superadmin");
        totalAdmin = await _context.Users.CountAsync(x => x.role == "Admin");
        totalUser = await _context.Users.CountAsync(x => x.role == "User");

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if(id < 0) HttpContext.Session.SetString("Message", "User " + id + " Was Deleted Successfully");

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            HttpContext.Session.SetString("Message", "User " + id + " Not Found");
            return RedirectToPage("/ManageUser/ManageUser");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("Message", "User " + id + " Was Deleted Successfully");

        return RedirectToPage("/ManageUser/ManageUser");
    }

    public async Task<IActionResult> OnPostEditAsync(int id, string name, string email, string role, string password)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            HttpContext.Session.SetString("Message", "User " + id + " Not Found");
            return RedirectToPage("/ManageUser/ManageUser");
        }

        user.name = name;
        // user.email = email;
        user.role = role;
        user.password = password;

        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("Message", "User " + id + " Was Updated Successfully");

        return RedirectToPage("/ManageUser/ManageUser");
    }
}