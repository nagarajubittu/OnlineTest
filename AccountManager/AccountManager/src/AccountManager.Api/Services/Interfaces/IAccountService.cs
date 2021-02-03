using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Api.Models;

namespace AccountManager.Api.Services.Interfaces
{
    public interface IAccountService
    {
         Task CreateAsync(PostAccountRequestModel model);
         Task UpdateAsync(PutAccountRequestModel model);
         Task<List<AccountModel>> GetAllAsync();
         Task<AccountModel> GetAsync(int accountId);
         Task DeleteAsync(int accountId);
    }
}