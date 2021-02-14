using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Api.DataModels;
using AccountManager.Api.Models;
using AccountManager.Api.Repositories.Interfaces;
using AccountManager.Api.Services.Interfaces;
using Dapper.AmbientContext;
using Microsoft.Extensions.Logging;


namespace AccountManager.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAmbientDbContextFactory _ambientDbContextFactory;
        private readonly IAccountRepository _accountRepository;

        public AccountService(
            ILogger<AccountService> logger,
            IAmbientDbContextFactory ambientDbContextFactory,
            IAccountRepository accountRepository,
            IMeterRepository meterRepository)
        {
            _logger = logger;
            _ambientDbContextFactory = ambientDbContextFactory;
            _accountRepository = accountRepository;
        }

        public async Task CreateAsync(PostAccountRequestModel model)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                await _accountRepository.CreateAsync(new Account
                {
                    AccountId = model.AccountId,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                });

                context.Commit();
            }
        }

        public async Task UpdateAsync(PutAccountRequestModel model)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                await _accountRepository.UpdateAsync(new Account
                {
                    AccountId = model.AccountId,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                });

                context.Commit();
            }
        }

        public async Task<List<AccountModel>> GetAllAsync()
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                var result = await _accountRepository.ReadAllAsync();
                return result.Select(x => new AccountModel
                {
                    AccountId = x.AccountId,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                }).ToList();
            }
        }

        public async Task<AccountModel> GetAsync(int accountId)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                var result = await _accountRepository.ReadAsync(accountId);
                return new AccountModel
                {
                    AccountId = result.AccountId,
                    FirstName = result.FirstName,
                    LastName = result.LastName
                };
            }
        }

        public async Task DeleteAsync(int accountId)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                await _accountRepository.DeleteAsync(accountId);
                context.Commit();
            }
        }
    }
}