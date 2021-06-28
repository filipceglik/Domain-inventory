using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var domains = await _domainRepository.GetDomains();

            var vm = new DomainsViewModel
            {
                Domains = domains.ToArray()
            };
            
            return View(vm);
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

        public async Task<IActionResult> Renew()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Renew(RenewDomainViewModel renewDomainViewModel)
        {
            if (!ModelState.IsValid)
                return View(renewDomainViewModel);
            var domain = new Domain
            {
                buyTime = renewDomainViewModel.buyTime,
                name = renewDomainViewModel.name,
                expiresOn = renewDomainViewModel.expiresOn
            };

            if (await _domainRepository.RenewDomain(domain,renewDomainViewModel.RenewalYears))
            {
                return RedirectToAction("Index");    
            }

            return RedirectToAction("Index");
        }
        
        /*[Route("Domain/Renew/{name}")]
        public async Task<IActionResult> Renew(string name)
        {
            var data = await _domainRepository.GetDomain(name);

            return View(new RenewDomainViewModel
            {
                name = data.name,
                buyTime = data.buyTime,
                expiresOn = data.expiresOn
            });
        }*/


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}