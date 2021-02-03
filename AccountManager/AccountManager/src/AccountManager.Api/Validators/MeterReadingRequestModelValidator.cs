using System;
using System.Globalization;
using System.Threading.Tasks;
using AccountManager.Api.Models;
using AccountManager.Api.Repositories.Interfaces;
using AccountManager.Api.Shared;
using Dapper.AmbientContext;
using FluentValidation;

namespace AccountManager.Api.Validators
{
    public class MeterReadingRequestModelValidator : AbstractValidator<MeterReadingRequestModel>
    {
        private readonly IAmbientDbContextFactory _ambientDbContextFactory;
        private readonly IAccountRepository _accountRepository;

        public MeterReadingRequestModelValidator(IAmbientDbContextFactory ambientDbContextFactory, IAccountRepository accountRepository)
        {
            _ambientDbContextFactory = ambientDbContextFactory;
            _accountRepository = accountRepository;

            RuleFor(m => m.AccountId)
                .NotEmpty()
                .MustAsync((v, c) => MustExistAccountAsync(v)).WithMessage(m => $"AccountId {m.AccountId} does not exist");
            RuleFor(m => m.MeterReadingDateTime)
                .NotEmpty()
                .Must(v => MustValidDatetime(v)).WithMessage(m => $"MeterReadingDateTime {m.MeterReadingDateTime} is not valid datetime, format example: dd/MM/yyyy HH:mm");
            RuleFor(m => m.MeterReadValue)
                .NotEmpty()
                .Length(5)
                .Must(v => MustValidReadingValue(v)).WithMessage(m => $"MeterReadValue {m.MeterReadValue} is not valid int");
        }

        private async Task<bool> MustExistAccountAsync(string strAccountId)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                var isValidAccountIdDataType = int.TryParse(strAccountId.Trim(), out int accountId);
                if (!isValidAccountIdDataType)
                {
                    return false;
                }
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

        private bool MustValidReadingValue(string strValue)
        {
            var isValidDataType = int.TryParse(strValue.Trim(), out int id);
            if (!isValidDataType || (isValidDataType && id < 0))
            {
                return false;
            }
            return true;
        }

        private bool MustValidDatetime(string strDatetime)
        {
            var isValidDateTimeType = DateTime.TryParseExact(strDatetime.Trim(), AppConstants.MeterReaderDateFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime readingDatetime);
            return isValidDateTimeType;
        }
    }
}