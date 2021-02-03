using System.Threading.Tasks;
using System.Collections.Generic;
using AccountManager.Api.DataModels;
using AccountManager.Api.Repositories.Interfaces;
using Dapper.AmbientContext;

namespace AccountManager.Api.Repositories
{
    public class AccountRepository : AbstractRepository, IAccountRepository
    {
        private const string tableName = "Accounts";
        public AccountRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }

        public async Task<int> CreateAsync(Account account)
        {
            return await Context.ExecuteAsync($"INSERT INTO {tableName}(AccountId, FirstName, LastName) VALUES (@AccountId, @FirstName, @LastName)", account);
        }

        public async Task<Account> ReadAsync(int accountId)
        {
            return await Context.QuerySingleAsync<Account>($"SELECT AccountId, FirstName, LastName FROM {tableName} WHERE AccountId=@AccountId", new { AccountId = accountId });
        }

        public async Task<IEnumerable<Account>> ReadAllAsync()
        {
            return await Context.QueryAsync<Account>($"SELECT AccountId, FirstName, LastName FROM {tableName}");
        }

        public async Task<int> UpdateAsync(Account account)
        {
            return await Context.ExecuteAsync($"UPDATE {tableName} SET FirstName=@FirstName, LastName=@LastName WHERE AccountId=@AccountId", account);
        }

        public async Task<int> DeleteAsync(int accountId)
        {
            return await Context.ExecuteAsync($"DELETE FROM {tableName} WHERE AccountId=@AccountId", new { AccountId = accountId });
        }
    }
}