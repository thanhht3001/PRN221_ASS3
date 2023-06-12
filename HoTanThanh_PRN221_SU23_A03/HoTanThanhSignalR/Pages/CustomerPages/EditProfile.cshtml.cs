using BusinessObject.Models;
using HoTanThanhSignalR.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using Repository.CustomerRepo;
using HoTanThanhSignalR.Utils;
using Microsoft.AspNetCore.Authorization;

namespace HoTanThanhSignalR.Pages.CustomerPages
{
    [Authorize(Roles = nameof(eRole.Customer))]
    public class EditProfileModel : PageModel
    {
        [BindProperty]
        public CustomerViewModel Customer { get; set; }
        private readonly ICustomerRepo repo = new CustomerRepo();

        public EditProfileModel() { }

        public IActionResult OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
             var customer = repo.GetCustomer(id);

            var customerViewmodel = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                City = customer.City,
                Country = customer.Country,
                Password = customer.Password,
                Birthday = customer.Birthday
            };
            Customer = customerViewmodel;

            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            var customer = new Customer
            {
                CustomerId = Customer.CustomerId,
                CustomerName = Customer.CustomerName,
                Email = Customer.Email,
                City = Customer.City,
                Country = Customer.Country,
                Password = Customer.Password,
                Birthday = Customer.Birthday
            };
            repo.Update(customer);
            return RedirectToPage("./Profile", new { id = Customer.CustomerId });
        }
    }
}
