using System.Threading.Tasks;
using Domain_inventory.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Domain_inventory.Infrastructure
{
    public class UserRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public async Task<User> GetUser(string username) => await _databaseContext
            .GetCollection<User>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.UserName == username);

        public async Task<bool> Create(User user)
        {
            var existingUser = await GetUser(user.UserName);
            if (existingUser != null && existingUser.UserName == user.UserName)
            {
                return false;
            }

            await _databaseContext
                .GetCollection<User>()
                .InsertOneAsync(user);
            return true;
        }

        public async Task<bool> Update(User user)
        {
            var entity = await GetUser(user.UserName);

            if (entity == null)
                return false;

            entity.Password = user.Password;

            await _databaseContext
                .GetCollection<User>()
                .ReplaceOneAsync(x => x.UserName == user.UserName, entity);

            return true;
        }
        
        public async Task Delete(User user)
        {
            await _databaseContext
                .GetCollection<User>()
                .DeleteOneAsync(x => x.UserName == user.UserName);
        }
    }
}