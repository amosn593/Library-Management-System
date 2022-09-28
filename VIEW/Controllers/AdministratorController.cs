using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using USERVIEW.Data;
using Microsoft.EntityFrameworkCore;
using USERVIEW.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace USERVIEW.Controllers
{
    
    [Authorize(Policy = "Admin")]
    public class AdministratorController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly USERVIEWContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<USERVIEWUser> _signInManager;
        private readonly UserManager<USERVIEWUser> _userManager;
        private readonly IUserStore<USERVIEWUser> _userStore;
        private readonly IUserEmailStore<USERVIEWUser> _emailStore;
        private readonly ILogger<USERVIEWUser> _logger;
        public AdministratorController(
            UserManager<USERVIEWUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<USERVIEWUser> userStore,
            SignInManager<USERVIEWUser> signInManager,
            ILogger<USERVIEWUser> logger,
            USERVIEWContext context,
            INotyfService notyf
            )
        {
            _context = context;
            _notyf = notyf;
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;

        }
        // GET: AdministratorController
        public async Task<IActionResult> Index()
        {
            try
            {
                //first we create Admin rool
                //IdentityRole role = new()
                //{
                //    Name = "Captain"
                //};
                //await _roleManager.CreateAsync(role);

                //Add the user lowest role , captain

                


                var users = await _context.Users
                    .OrderByDescending(d => d.RegisterDate)
                    .ToListAsync();


                return View(users);

            }
            catch (Exception)
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }

        }

              

        // GET: AdministratorController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdministratorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,StaffNumber, Email, Password," +
            " ConfirmPassword")] RegisterUser newuser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loggeduser = await _userManager.GetUserAsync(User);
                   
                    var user = CreateUser();

                    user.FirstName = newuser.FirstName;
                    user.LastName = newuser.LastName;
                    user.StaffNumber = newuser.StaffNumber;
                    user.RegisteredBy = loggeduser.UserName;

                    user.RegisterDate = DateTime.Now;


                    await _userStore.SetUserNameAsync(user, newuser.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, newuser.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, newuser.Password);


                    if (result.Succeeded)
                    {
                        //Add the user lowest role , captain
                        await _userManager.AddToRoleAsync(user, "Captain");

                        _logger.LogInformation("User created a new account with password.");
                        _notyf.Success("User Registered Successfully.");
                        return RedirectToAction(nameof(Index));

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }



                }
                catch (Exception)
                {
                    _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                    return RedirectToAction(nameof(Index));

                    //throw;
                }

            }

            _notyf.Error("Correct below errors, and try Again!!!");
            return View(newuser);
        }


        // GET: AdministratorController/Roles/5
     
        public async Task<IActionResult> Roles(string? id)
        {
            if (id == null)
            {
                _notyf.Error("User not found, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
            try
            {
                //Get User
                var user = await _context.Users.FindAsync(id);

                //Get Roles assigned to that user
                var user_roles = await _userManager.GetRolesAsync(user);

               
            
                //Get User's current roles as a select list
                

                ViewData["CurrentRoles"] = new SelectList(
                    _context.Roles.Where(n => user_roles.Contains(n.Name)),
                     "Name", "Name");


                //Get all available Roles as a select list
                ViewData["RoleId"] = new SelectList(_context.Roles, "Name", "Name");

                return View(user);

            }
            catch (Exception)
            {
                 _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                 return RedirectToAction(nameof(Index));
                
            }
            
        }

        // POST: AdministratorController/AddRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string? id, IFormCollection col)
        {
            if (id == null)
            {
                _notyf.Error("User not found, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
            try
            {
                //Getting psted form data
                //Getting the role from posted form
                var role = col["role"];

                //Getting the user the 
                var user = await _userManager.FindByIdAsync(id);
                
                                                             

                //Add user the role
                await _userManager.AddToRoleAsync(user, role);

                _notyf.Success("User Added the Role Successfully."); 
                return RedirectToAction("Roles", "Administrator", new { id });
            }
            catch
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
               
            }
        }

        // POST: AdministratorController/RemoveRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string? id, IFormCollection col)
        {
            if (id == null)
            {
                _notyf.Error("User not found, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
            try
            {
                //Getting the user
                var user = await _userManager.FindByIdAsync(id);

                //Getting the role from posted form
                var role = col["role"];

                //remove user the role
                await _userManager.RemoveFromRoleAsync(user, role);

                _notyf.Success("User Removed the Role Successfully.");
                return RedirectToAction("Roles", "Administrator", new { id });
            }
            catch
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
        }



        // POST: AdministratorController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? id)
        {
            
            if (id == null)
            {
                _notyf.Error("User not found, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var user = await _context.Users.FindAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                _notyf.Success("User Deleted Successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _notyf.Error("Something Went Wrong, Kindly Try Again!!!");
                return RedirectToAction(nameof(Index));
                //throw;
            }
        }

        private USERVIEWUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<USERVIEWUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(USERVIEWUser)}'. " +
                    $"Ensure that '{nameof(USERVIEWUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<USERVIEWUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<USERVIEWUser>)_userStore;
        }

    }
}
