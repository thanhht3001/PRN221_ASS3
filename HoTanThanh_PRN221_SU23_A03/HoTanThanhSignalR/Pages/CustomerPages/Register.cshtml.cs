using BusinessObject.Models;
using HoTanThanhSignalR.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Repository.CustomerRepo;
using Microsoft.AspNetCore.SignalR;
using SignalRAssignment_SE151098;

namespace HoTanThanhSignalR.Pages.CustomerPages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public CustomerViewModel Customer { get; set; }

        private readonly ICustomerRepo repo = new CustomerRepo();
        private readonly IHubContext<SignalrServer> _signalRHub;

        public RegisterModel(IHubContext<SignalrServer> signalRHub)
        {
            _signalRHub = signalRHub;
        }


        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
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
            if (repo.Exist(customer.CustomerId))
            {
                ViewData["Message"] = "Customer ID already exist!";
                return Page();
            }
            else
            {
                repo.Save(customer);
                await _signalRHub.Clients.All.SendAsync("LoadAppUsers");
                return RedirectToPage("/Index");
            }

        }
    }
}
