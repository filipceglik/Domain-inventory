using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Domain_inventory.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Domain_inventory.Infrastructure
{
    public class DomainRepository
    {
        private readonly DatabaseContext _databaseContext;

        public DomainRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Domain> GetDomain(string domainName) => await _databaseContext
            .GetCollection<Domain>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.name == domainName);

        public async Task<ICollection<Domain>> GetDomains() => await _databaseContext
            .GetCollection<Domain>()
            .AsQueryable()
            .ToListAsync();

        public async Task<bool> CreateDomain(Domain domain)
        {
            var existingDomain = await GetDomain(domain.name);
            if (existingDomain == null || existingDomain.name != domain.name)
            {
                await _databaseContext
                    .GetCollection<Domain>()
                    .InsertOneAsync(domain);
                return true;
            }
            
            return false;
        }

        public async Task<bool> RenewDomain(Domain domain, int years)
        {
            var entity = await GetDomain(domain.name);
            
            if (entity == null)
                return false;
            
            entity.expiresOn = entity.expiresOn.AddYears(years);

            await _databaseContext
                .GetCollection<Domain>()
                .ReplaceOneAsync(x => x.name == domain.name, entity);
            
            return true;
        }

        public async Task DeleteDomain(Domain domain)
        {
            await _databaseContext
                .GetCollection<Domain>()
                .DeleteOneAsync(x => x.name == domain.name);
        }
    }
}