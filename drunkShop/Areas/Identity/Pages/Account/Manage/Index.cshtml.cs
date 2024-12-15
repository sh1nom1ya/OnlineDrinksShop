using drunkShop.Data;
using drunkShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class IndexModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _db;

    public IndexModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        AppDbContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
    }

    public string FullName { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Phone]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ФИО")]
        public string FullName { get; set; }
    }

    private async Task LoadAsync(IdentityUser user)
    {
        var applicationUser = await _db.Users
            .OfType<ApplicationUser>()
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        var fullName = applicationUser.FullName;
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        FullName = fullName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber,
            FullName = fullName // Заполняем FullName
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        // Обновление PhoneNumber
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                foreach (var error in setPhoneResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await LoadAsync(user);
                return Page();
            }
        }

        // Обновление FullName
        var applicationUser = await _db.Users
            .OfType<ApplicationUser>()
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (applicationUser != null && applicationUser.FullName != Input.FullName)
        {
            applicationUser.FullName = Input.FullName;
            _db.Users.Update(applicationUser);
            await _db.SaveChangesAsync();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Ваш профиль обновлён";
        return RedirectToPage();
    }
}
