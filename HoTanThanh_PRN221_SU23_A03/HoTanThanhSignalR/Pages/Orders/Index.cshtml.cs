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
using Repository.OrderRepo;
using HoTanThanhSignalR.Utils;
using Microsoft.AspNetCore.Authorization;

namespace HoTanThanhSignalR.Pages.Orders
{
    [Authorize(Roles = nameof(eRole.Admin))]
    public class IndexModel : PageModel
    {
        public IList<Order> Order { get;set; }
        private readonly IOrderRepo repo = new OrderRepo();

        public IndexModel() { }

        public IActionResult OnGetAsync()
        {
            Order = repo.GetOrders();
            return Page();
        }
    }
}
