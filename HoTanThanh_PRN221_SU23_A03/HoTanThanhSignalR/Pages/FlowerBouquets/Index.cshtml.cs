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
using Repository.FlowerBouquetRepo;
using HoTanThanhSignalR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRAssignment_SE151098;

namespace HoTanThanhSignalR.Pages.FlowerBouquets
{
    [Authorize(Roles = nameof(eRole.Admin))]
    public class IndexModel : PageModel
    {
        public IList<FlowerBouquet> FlowerBouquet { get; set; }
        private readonly IFlowerRepo repo = new FlowerRepo();

        [BindProperty]
        public string SearchString { get; set; }

        private readonly IHubContext<SignalrServer> _signalRHub;

        public IndexModel(IHubContext<SignalrServer> signalRHub)
        {
            _signalRHub = signalRHub;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            FlowerBouquet = repo.GetFlowers();
            await _signalRHub.Clients.All.SendAsync("LoadFlowers");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            FlowerBouquet = repo.SearchByName(SearchString);
            await _signalRHub.Clients.All.SendAsync("LoadFlowers");
            return Page();
        }

        public IActionResult OnGetFlowers()
        {
            var bouquets = repo.GetFlowers();
            return new JsonResult(bouquets);
        }
    }
}
