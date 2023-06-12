using BusinessObject;
using BusinessObject.Models;
using HoTanThanhSignalR.Utils;
using HoTanThanhSignalR.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository.CustomerRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HoTanThanhSignalR.Pages
{
    public class IndexModel : PageModel
    {                                                                       
        private readonly ICustomerRepo repository = new CustomerRepo();
        private readonly AdminAccount adminAccount;

        [BindProperty]
        public LoginViewModel LoginModel { get; set; }
        public string Message { get; set; }

        public IndexModel(IOptions<AdminAccount> logger)
        {
            adminAccount = logger.Value;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var role = HttpContext.User.FindFirst(ClaimType.Role.ToString());
                return RedirectToPage(role.Value == eRole.Admin.ToString() ? "/Customers/Index" : "/CustomerPages/FlowerBouquets");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = LoginModel.Email;
            var password = LoginModel.Password;
            if (email == adminAccount.Email && password == adminAccount.Password)
            {
                var adminIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimType.Role, eRole.Admin.ToString()),
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                var adminPrinciple = new ClaimsPrincipal(adminIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminPrinciple);
                return RedirectToPage("/Customers/Index");
            }

            try
            {
                Customer customer = repository.Login(email, password);
                if (customer is null)
                {
                    ModelState.AddModelError("", "Incorrect email or password");
                    return Page();
                }

                var claims = new List<Claim>
                    {
                        new(ClaimType.Role, eRole.Customer.ToString()),
                        new(ClaimType.AccountId, customer.CustomerId.ToString()),
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetInt32("id", customer.CustomerId);
                return RedirectToPage("/CustomerPages/FlowerBouquets");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ModelState.AddModelError("", "Something went wrong, please try again later!");
                return Page();
            }
        }
    }
}
