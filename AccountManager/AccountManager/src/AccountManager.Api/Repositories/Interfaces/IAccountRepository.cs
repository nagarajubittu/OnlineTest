using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Api.DataModels;

namespace AccountManager.Api.Repositories.Interfaces
{
    public interface IAccountRepository
    {
         Task<int> CreateAsync(Account account);
         Task<Account> ReadAsync(int accountId);
         Task<IEnumerable<Account>> ReadAllAsync();
         Task<int> UpdateAsync(Account account);
         Task<int> DeleteAsync(int accountId);
    }
}