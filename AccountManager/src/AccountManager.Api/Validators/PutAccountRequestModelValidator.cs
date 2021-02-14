using System.Threading.Tasks;
using AccountManager.Api.Models;
using AccountManager.Api.Repositories.Interfaces;
using Dapper.AmbientContext;
using FluentValidation;

namespace AccountManager.Api.Validators
{
    public class PutAccountRequestModelValidator : AbstractValidator<PutAccountRequestModel>
    {
        private readonly IAmbientDbContextFactory _ambientDbContextFactory;
        private readonly IAccountRepository _accountRepository;

        public PutAccountRequestModelValidator(IAmbientDbContextFactory ambientDbContextFactory, IAccountRepository accountRepository)
        {
            _ambientDbContextFactory = ambientDbContextFactory;
            _accountRepository = accountRepository;

            RuleFor(m => m.AccountId)
                .NotEmpty()
                .MustAsync((v, c) => MustExistAccountAsync(v)).WithMessage(m => $"AccountId {m.AccountId} does not exist");
            RuleFor(m => m.FirstName)
                .NotEmpty();
            RuleFor(m => m.LastName)
                .NotEmpty();
        }

        private async Task<bool> MustExistAccountAsync(int accountId)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                try
                {
                    await _accountRepository.ReadAsync(accountId);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }

            }
        }
    }
}