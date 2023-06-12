﻿using System;
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
using Repository.CustomerRepo;
using HoTanThanhSignalR.Utils;
using Microsoft.AspNetCore.Authorization;

namespace HoTanThanhSignalR.Pages.Customers
{
    [Authorize(Roles = nameof(eRole.Admin))]
    public class DetailsModel : PageModel
    {
        private readonly ICustomerRepo repo = new CustomerRepo();
        public Customer Customer { get; set; }

        public DetailsModel() { }

        public IActionResult OnGetAsync(int id)
        {
            Customer = repo.GetCustomer(id);
            return Page();
        }
    }
}
