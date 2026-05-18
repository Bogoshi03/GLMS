using GLMS.Core.Entities;
using GLMS.Core.Enums;
using GLMS.Infrastructure.Data;
using GLMS.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly GlmsDbContext _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(
            GlmsDbContext context,
            CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index()
        {
            var requests = _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client);

            return View(await requests.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.ContractId = new SelectList(
                _context.Contracts,
                "Id",
                "Id");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            ModelState.Remove("CostZAR");

            serviceRequest.CostZAR =
    await _currencyService
        .ConvertUsdToZar(serviceRequest.CostUSD);

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Id == serviceRequest.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "Contract not found.");
            }

            else if (contract.Status == ContractStatus.Expired
                || contract.Status == ContractStatus.OnHold)
            {
                ModelState.AddModelError(
                    "",
                    "Cannot create Service Request for Expired or On Hold contracts.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequest);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ContractId = new SelectList(
     _context.Contracts,
     "Id",
     "Id",
     serviceRequest.ContractId);


            return View(serviceRequest);
        }
    }
}
