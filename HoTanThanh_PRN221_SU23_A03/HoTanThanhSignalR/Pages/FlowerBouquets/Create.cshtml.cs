using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using HoTanThanhSignalR.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Repository.FlowerBouquetRepo;
using Repository.CategoryRepo;
using Repository.SupplierRepo;
using HoTanThanhSignalR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRAssignment_SE151098;

namespace HoTanThanhSignalR.Pages.FlowerBouquets
{
    [Authorize(Roles = nameof(eRole.Admin))]
    public class CreateModel : PageModel
    {
        private readonly IFlowerRepo repo = new FlowerRepo();
        private readonly ICategoryRepo categoryRepo = new CategoryRepo();
        private readonly ISupplierRepo supplierRepo = new SupplierRepo();
        [BindProperty]
        public FlowerBouquetViewModel FlowerBouquet { get; set; }
        public IList<Category> Category { get; set; }
        public IList<Supplier> Supplier { get; set; }

        private readonly IHubContext<SignalrServer> _signalRHub;

        public CreateModel(IHubContext<SignalrServer> signalRHub)
        {
            _signalRHub = signalRHub;
        }

        public IActionResult OnGet()
        {
            Category = categoryRepo.GetCategories();
            Supplier = supplierRepo.GetSuppliers();
            ViewData["CategoryId"] = new SelectList(Category, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(Supplier, "SupplierId", "SupplierName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Category = categoryRepo.GetCategories();
                Supplier = supplierRepo.GetSuppliers();
                ViewData["CategoryId"] = new SelectList(Category, "CategoryId", "CategoryName");
                ViewData["SupplierId"] = new SelectList(Supplier, "SupplierId", "SupplierName");
                return Page();
            }
            var flowerBouquet = new FlowerBouquet
            {
                FlowerBouquetId = FlowerBouquet.FlowerBouquetId,
                FlowerBouquetName = FlowerBouquet.FlowerBouquetName,
                Description = FlowerBouquet.Description,
                UnitPrice = FlowerBouquet.UnitPrice,
                UnitsInStock = FlowerBouquet.UnitsInStock,
                CategoryId = FlowerBouquet.CategoryId,
                SupplierId = FlowerBouquet.SupplierId,
                FlowerBouquetStatus = FlowerBouquet.FlowerBouquetStatus,
            };
            if (repo.Exist(flowerBouquet.FlowerBouquetId))
            {
                Category = categoryRepo.GetCategories();
                Supplier = supplierRepo.GetSuppliers();
                ViewData["CategoryId"] = new SelectList(Category, "CategoryId", "CategoryName");
                ViewData["SupplierId"] = new SelectList(Supplier, "SupplierId", "SupplierName");
                ViewData["Message"] = "FlowerBouquet ID already exist!";
                return Page();
            }
            else
            {
                repo.Save(flowerBouquet);
                await _signalRHub.Clients.All.SendAsync("LoadFlower");
                return RedirectToPage("./Index");
            }
        }
    }
}
