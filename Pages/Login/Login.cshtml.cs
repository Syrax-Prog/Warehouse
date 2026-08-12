using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Models;

namespace Warehouse.Pages;

public class LoginModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public LoginModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string email { get; set; } = string.Empty;
    [BindProperty]
    public string password { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string Message {get; set;} = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        Message = TempData["Message"] as string ?? "";

        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.email == email && u.password == password
        );

        if (user == null)
        {
            ErrorMessage = "Invalid Email Or Password";
            return Page();
        }

        HttpContext.Session.SetInt32("id", user.id);
        HttpContext.Session.SetString("email", user.email);
        HttpContext.Session.SetString("name", user.name);
        HttpContext.Session.SetString("role", user.role);

        return RedirectToPage("/Inventory/Inventory", new { page = 4 });
    }

    public void OnGet()
    {
        Message = TempData["Message"] as string ?? "";
    }

    public IActionResult OnGetLogout()
    {
        HttpContext.Session.Clear();
        TempData["Message"] = "You Have Been Logged Out Succesfully";
        return RedirectToPage("/Login/Login");
    }
}