using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Repository.FlowerBouquetRepo;
using HoTanThanhSignalR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRAssignment_SE151098;

namespace HoTanThanhSignalR.Pages.FlowerBouquets
{
    [Authorize(Roles = nameof(eRole.Admin))]
    public class DeleteModel : PageModel
    {
        private readonly IFlowerRepo repo = new FlowerRepo();

        [BindProperty]
        public FlowerBouquet FlowerBouquet { get; set; }

        private readonly IHubContext<SignalrServer> _signalRHub;

        public DeleteModel(IHubContext<SignalrServer> signalRHub)
        {
            _signalRHub = signalRHub;
        }

        public IActionResult OnGetAsync(int id)
        {
            FlowerBouquet = repo.GetFlower(id);
            await _signalRHub.Clients.All.SendAsync("LoadFlower");
            if (FlowerBouquet == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            else
            {
                FlowerBouquet = repo.GetFlower(id);
                repo.Delete(FlowerBouquet);
                await _signalRHub.Clients.All.SendAsync("LoadFlower");
                return RedirectToPage("./Index");
            }
        }
    }
}
