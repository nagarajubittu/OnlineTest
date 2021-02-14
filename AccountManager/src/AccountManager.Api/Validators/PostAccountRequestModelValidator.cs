using System.Threading.Tasks;
using AccountManager.Api.Models;
using AccountManager.Api.Repositories.Interfaces;
using Dapper.AmbientContext;
using FluentValidation;

namespace AccountManager.Api.Validators
{
    public class PostAccountRequestModelValidator : AbstractValidator<PostAccountRequestModel>
    {
        private readonly IAmbientDbContextFactory _ambientDbContextFactory;
        private readonly IAccountRepository _accountRepository;

        public PostAccountRequestModelValidator(IAmbientDbContextFactory ambientDbContextFactory, IAccountRepository accountRepository)
        {
            _ambientDbContextFactory = ambientDbContextFactory;
            _accountRepository = accountRepository;

            RuleFor(m => m.AccountId)
                .NotEmpty()
                .MustAsync((v, c) => MustNotExistAccountAsync(v)).WithMessage(m => $"AccountId {m.AccountId} already exist");
            RuleFor(m => m.FirstName)
                .NotEmpty();
            RuleFor(m => m.LastName)
                .NotEmpty();
        }

        private async Task<bool> MustNotExistAccountAsync(int accountId)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                try
                {
                    await _accountRepository.ReadAsync(accountId);
                    return false;
                }
                catch (System.Exception)
                {
                    return true;
                }

            }
        }
    }
}