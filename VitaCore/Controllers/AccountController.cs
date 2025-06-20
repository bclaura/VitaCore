using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Data;
using System.Numerics;
using System.Security.Claims;
using VitaCore.Data;
using VitaCore.Models;
using VitaCore.Models.ViewModel;
using VitaCore.Services;

namespace VitaCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(model);
            }

            string assignedRole = "User";
            string password = model.Password; // Default password from the model

            if (model.Email.ToLower() == "admin@admin.com")
            {
                assignedRole = "Admin";
                password = "AdminSibiu3#";
            }
            else if (model.Email.ToLower().Contains("@doctor"))
            {
                assignedRole = "Doctor";
            }
            else if (model.Email.ToLower().Contains("@patient"))
            {
                assignedRole = "Patient";
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Role = assignedRole ?? "User", // Default role is User
                MobileNumber = model.MobileNumber,
                DateOfBirth = DateTime.SpecifyKind((DateTime)model.DateOfBirth, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            if (assignedRole == "Doctor")
            {
                var doctor = new DoctorModel
                {
                    UserId = user.Id,
                    Specialization = "General"
                };
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();
            }
            else if (assignedRole == "Patient")
            {
                var patient = new PatientModel
                {
                    UserId = user.Id,
                    email = user.Email,
                    cnp = Guid.NewGuid().ToString().Substring(0, 13),
                    address_city = string.Empty,
                    address_county = string.Empty,
                    address_street = string.Empty,
                    occupation = string.Empty,
                    phone_number = string.Empty,
                    workplace = string.Empty,
                    age = 0
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Login", "Account");
        }



        // LOGIN
        [HttpGet]
        public IActionResult Login() //bool isRegister = false)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Role, user.Role) // asigură-te că e exact "Admin", "Doctor", "Patient"
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (user.Role.Contains("Patient"))
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                if (patient == null ||
                    string.IsNullOrWhiteSpace(user.FirstName) ||
                    string.IsNullOrWhiteSpace(user.LastName) ||
                    user.DateOfBirth == null ||
                    string.IsNullOrWhiteSpace(patient.cnp) ||
                    string.IsNullOrWhiteSpace(patient.phone_number))
                {
                    WebSessionManager.SetLoggedInUser(HttpContext, user);
                    return RedirectToAction("Edit", "Patients", new { id = patient.id });
                }
            }

            else if (user.Role.Contains("Doctor"))
            {
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.UserId == user.Id);

                if (doctor == null ||
                    string.IsNullOrWhiteSpace(user.FirstName) ||
                    string.IsNullOrWhiteSpace(user.LastName) ||
                    user.DateOfBirth == null ||
                    string.IsNullOrWhiteSpace(doctor.Specialization) ||
                    string.IsNullOrWhiteSpace(doctor.clinic_address))
                {
                    WebSessionManager.SetLoggedInUser(HttpContext, user);
                    return RedirectToAction("Edit", "Doctors", new { id = doctor.id });
                }
            }

            WebSessionManager.SetLoggedInUser(HttpContext, user);

            if (user.Role.ToLower() == "admin")
                return RedirectToAction("Index", "Admin");

            if (user.Role.ToLower() == "doctor")
                return RedirectToAction("Index", "Doctors");

            if (user.Role.ToLower() == "patient")
                return RedirectToAction("Index", "Patients");

            return RedirectToAction("Index", "Home"); // fallback

            /*
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    _logger.LogInformation($"User {user.Email} is locked out and cannot log in.");
                    _logger.LogInformation($"User {user.UserName} is locked out and cannot log in.");
                    //notificare
                    return View(model);
                }

                if (user.LockoutEnabled == false)
                {
                    _logger.LogInformation($"User {user.Email} is locked out and cannot log in.");
                    return RedirectToAction("Login", "Account");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {


                    _logger.LogInformation($"utilizatorul cu numele {user.Email} s-a logat pe pagina");
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Unknown";
                    _logger.LogInformation("LOGIN_EVENT | {Email} | {Role} | {Time}", user.Email, role, DateTime.UtcNow);

                    user.LastLoginTime = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Patient"))
                    {
                        var patient = await _context.Patients
                            .FirstOrDefaultAsync(p => p.UserId == user.Id);

                        if (patient == null ||
                            string.IsNullOrWhiteSpace(user.FirstName) ||
                            string.IsNullOrWhiteSpace(user.LastName) ||
                            user.Birthday == null ||
                            string.IsNullOrWhiteSpace(patient.cnp) ||
                            string.IsNullOrWhiteSpace(patient.phone_number))
                        {
                            return RedirectToAction("Edit", "Patients", new { id = patient.id });
                        }
                    }

                    else if (roles.Contains("Doctor"))
                    {
                        var doctor = await _context.Doctors
                            .FirstOrDefaultAsync(d => d.UserId == user.Id);

                        if (doctor == null ||
                            string.IsNullOrWhiteSpace(user.FirstName) ||
                            string.IsNullOrWhiteSpace(user.LastName) ||
                            user.Birthday == null ||
                            string.IsNullOrWhiteSpace(doctor.Specialization) ||
                            string.IsNullOrWhiteSpace(doctor.clinic_address))
                        {
                            return RedirectToAction("Edit", "Doctors", new { id = doctor.id });
                        }
                    }


                    return RedirectToAction("Index", "Home");
                }
                
                _logger.LogInformation("Utilizatorul nu se poate loga");
                return RedirectToAction("Login", "Account");
            }
            _logger.LogInformation("Modelul nu este valid");
            return RedirectToAction("Login", "Account");
            */
        }

        /*//LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // extra, în caz că ai alte chei
            return RedirectToAction("Login", "Account");
        }




        //[HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            /*var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            return View(new CompleteProfileModel());*/
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(CompleteProfileModel model)
        {
            /*
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Birthday = model.Birthday;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
            */

            return View();
        }
    }
}