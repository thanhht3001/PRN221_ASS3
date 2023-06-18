using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Repository.CustomerRepo;
using HoTanThanhSignalR.Utils;
using Microsoft.AspNetCore.Authorization;

namespace HoTanThanhSignalR.Pages.Customers
{
    [Authorize(Roles = nameof(eRole.Admin))]
    public class IndexModel : PageModel
    {
        public IList<Customer> Customer { get;set; }
        private readonly ICustomerRepo repo = new CustomerRepo();

        [BindProperty]
        public string SearchString { get; set; }
        public IndexModel() { }

        public IActionResult OnGetAsync()
        {
            Customer = repo.GetCustomers();
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            Customer = repo.SearchByName(SearchString);
            return Page();
        }
        public IActionResult OnGetCustomers()
        {
            var customers = repo.GetCustomers();
            return new JsonResult(customers);
        }
    }
}
