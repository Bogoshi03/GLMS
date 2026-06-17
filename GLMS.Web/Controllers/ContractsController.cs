using GLMS.Core.Entities;
using GLMS.Core.Enums;
using GLMS.Core.Services;
using GLMS.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace GLMS.Web.Controllers
{
    public class ContractsController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly IWebHostEnvironment _environment;

        private readonly FileValidationService _fileValidationService;

        private readonly GlmsDbContext _context;

        public ContractsController(
    IHttpClientFactory factory,
    GlmsDbContext context,
    FileValidationService fileValidationService,
    IWebHostEnvironment environment)
        {
            _httpClient = factory.CreateClient();

            _httpClient.BaseAddress =
                new Uri("https://localhost:7049/");

            _context = context;

            _fileValidationService = fileValidationService;
            _environment = environment;
        }

        // INDEX
        public async Task<IActionResult> Index(
    ContractStatus? status,
    DateTime? startDate,
    DateTime? endDate)
        {
            var contracts =
                await _httpClient.GetFromJsonAsync<List<Contract>>(
                    "api/contracts");

            if (contracts == null)
                contracts = new List<Contract>();

            if (status.HasValue)
            {
                contracts = contracts
                    .Where(c => c.Status == status.Value)
                    .ToList();
            }

            if (startDate.HasValue)
            {
                contracts = contracts
                    .Where(c => c.StartDate >= startDate.Value)
                    .ToList();
            }

            if (endDate.HasValue)
            {
                contracts = contracts
                    .Where(c => c.StartDate <= endDate.Value)
                    .ToList();
            }

            return View(contracts);
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewBag.ClientId =
                new SelectList(
                    _context.Clients,
                    "Id",
                    "Name");

            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
     Contract contract,
     IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (!_fileValidationService.IsValidFile(uploadedFile))
                {
                    ModelState.AddModelError("", "Only PDF files are allowed.");

                    return View(contract);
                }

                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads");

                string uniqueFileName = Guid.NewGuid().ToString()
                                        + "_"
                                        + uploadedFile.FileName;

                string filePath = Path.Combine(
                    uploadsFolder,
                    uniqueFileName);

                using (var fileStream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                contract.FilePath = $"/uploads/{uniqueFileName}";
            }

            var response =
    await _httpClient.PostAsJsonAsync(
        "api/contracts",
        contract);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "API Error creating contract.");

                return View(contract);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
