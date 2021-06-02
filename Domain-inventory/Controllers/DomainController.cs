using System.Diagnostics;
using System.Threading.Tasks;
using Domain_inventory.Infrastructure;
using Domain_inventory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Domain_inventory.Controllers
{
    public class DomainController : Controller
    {
        private readonly ILogger<DomainController> _logger;
        private readonly DomainRepository _domainRepository;

        public DomainController(ILogger<DomainController> logger, DomainRepository domainRepository)
        {
            _logger = logger;
            _domainRepository = domainRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateDomainViewModel createDomainViewModel)
        {
            if (!ModelState.IsValid)
                return View(createDomainViewModel);
            var domain = new Domain
            {
                buyTime = createDomainViewModel.buyTime,
                name = createDomainViewModel.name,
                expiresOn = createDomainViewModel.expiresOn
            };

            if (await _domainRepository.CreateDomain(domain))
            {
                return RedirectToAction("Index");    
            }

            return RedirectToAction("Index");   

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}