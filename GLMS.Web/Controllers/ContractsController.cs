using GLMS.Core.Entities;
using GLMS.Core.Enums;
using GLMS.Core.Services;
using GLMS.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class ContractsController : Controller
    {
        private readonly GlmsDbContext _context;

        private readonly IWebHostEnvironment _environment;

        private readonly FileValidationService _fileValidationService;
        public ContractsController(
    GlmsDbContext context,
    FileValidationService fileValidationService,
    IWebHostEnvironment environment)
        {
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
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (status.HasValue)
            {
                contracts = contracts.Where(c =>
                    c.Status == status.Value);
            }

            if (startDate.HasValue)
            {
                contracts = contracts.Where(c =>
                    c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                contracts = contracts.Where(c =>
                    c.StartDate <= endDate.Value);
            }

            return View(await contracts.ToListAsync());
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Name");
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

                    ViewBag.ClientId = new SelectList(
    _context.Clients,
    "Id",
    "Name",
    contract.ClientId);

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

            _context.Add(contract);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
