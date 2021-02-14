using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Api.DataModels;
using AccountManager.Api.Models;
using AccountManager.Api.Repositories.Interfaces;
using AccountManager.Api.Services.Interfaces;
using AccountManager.Api.Shared;
using AccountManager.Api.Validators;
using CsvHelper;
using Dapper.AmbientContext;
using Microsoft.Extensions.Logging;

namespace AccountManager.Api.Services
{
    public class MeterService : IMeterService
    {
        private readonly ILogger<MeterService> _logger;
        private readonly IAmbientDbContextFactory _ambientDbContextFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterRepository _meterRepository;

        public MeterService(
            ILogger<MeterService> logger,
            IAmbientDbContextFactory ambientDbContextFactory,
            IAccountRepository accountRepository,
            IMeterRepository meterRepository)
        {
            _logger = logger;
            _ambientDbContextFactory = ambientDbContextFactory;
            _accountRepository = accountRepository;
            _meterRepository = meterRepository;
        }

        public async Task<PostMeterReadingUploadsResponse> LoadFromCsvAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var rawMeterReadings = new List<MeterReadingRequestModel>();
            var invalidMeterReadingsCount = 0;
            var validMeterReadingsCount = 0;

            //By using CsvHelper reading the Csv file data and converting as list
            using (var reader = new System.IO.StreamReader(file.OpenReadStream()))
            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                rawMeterReadings = csv.GetRecords<MeterReadingRequestModel>().ToList();
            }
            //Connection string
            using (var context = _ambientDbContextFactory.Create())
            {
                foreach (var rawMeterReading in rawMeterReadings)
                {
                    //Validation check by using fluentvalidation
                    var csvMeterReaderValidator = new MeterReadingRequestModelValidator(_ambientDbContextFactory, _accountRepository);
                    var validationResult = await csvMeterReaderValidator.ValidateAsync(rawMeterReading);
                    if (!validationResult.IsValid)
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            _logger.LogInformation($"Property {error.PropertyName} failed validation. Error was: {error.ErrorMessage}");
                        }
                        _logger.LogInformation($"Failed validation, AccountId: {rawMeterReading.AccountId}, DateTime: {rawMeterReading.MeterReadingDateTime}, Value: {rawMeterReading.MeterReadValue}");
                        invalidMeterReadingsCount++;
                        continue;
                    }
                    var accountId = int.Parse(rawMeterReading.AccountId.Trim());
                    var readingDatetime = DateTime.ParseExact(rawMeterReading.MeterReadingDateTime.Trim(), AppConstants.MeterReaderDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
                    var readingValue = int.Parse(rawMeterReading.MeterReadValue.Trim());
                    var meterReading = new MeterReading
                    {
                        AccountId = accountId,
                        ReadingDatetime = readingDatetime,
                        ReadingValue = readingValue
                    };

                    var meterReadingExist = await _meterRepository.ReadAsync(meterReading);
                    //meter reading entry already exist or not
                    if (meterReadingExist != null && meterReadingExist.ReadingId > 0)
                    {
                        _logger.LogInformation($"Failed meter reading entry already exist, AccountId: {rawMeterReading.AccountId}, DateTime: {rawMeterReading.MeterReadingDateTime}, Value: {rawMeterReading.MeterReadValue}");
                        invalidMeterReadingsCount++;
                        continue;
                    }

                    var result = await _meterRepository.CreateAsync(meterReading);
                    context.Commit();
                    validMeterReadingsCount++;
                    _logger.LogInformation($"Success AccountId: {rawMeterReading.AccountId}, DateTime: {rawMeterReading.MeterReadingDateTime}, Value: {rawMeterReading.MeterReadValue}");
                }

            }
            return new PostMeterReadingUploadsResponse
            {
                FailedReadings = invalidMeterReadingsCount,
                SuccessfullReadings = validMeterReadingsCount
            };
        }

        public async Task CreateAsync(MeterReadingRequestModel meterReadingRequestModel)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                var accountId = int.Parse(meterReadingRequestModel.AccountId.Trim());
                var readingDatetime = DateTime.ParseExact(meterReadingRequestModel.MeterReadingDateTime.Trim(), AppConstants.MeterReaderDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
                var readingValue = int.Parse(meterReadingRequestModel.MeterReadValue.Trim());
                var meterReading = new MeterReading
                {
                    AccountId = accountId,
                    ReadingDatetime = readingDatetime,
                    ReadingValue = readingValue
                };
                var meterReadingExist = await _meterRepository.ReadAsync(meterReading);

                if (meterReadingExist != null && meterReadingExist.ReadingId > 0)
                {
                    throw new Exception("Already exist");
                }
                await _meterRepository.CreateAsync(meterReading);
                context.Commit();
            }
        }

        public async Task<List<GetMeterReadingResponseModel>> GetAsync(int accountId = 0)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                var result = accountId > 0 ? await _meterRepository.ReadByAccountAsync(accountId) : await _meterRepository.ReadAllAsync();
                return result.Select(x => new GetMeterReadingResponseModel
                {
                    AccountId = x.AccountId,
                    MeterId = x.ReadingId,
                    ReadingDatetime = x.ReadingDatetime,
                    ReadingValue = x.ReadingValue
                }).ToList();
            }
        }

        public async Task DeleteAsync(int meterId)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                await _meterRepository.DeleteAsync(meterId);
                context.Commit();
            }
        }

        public async Task DeleteByAccount(int accountId)
        {
            using (var context = _ambientDbContextFactory.Create())
            {
                await _meterRepository.DeleteByAccountAsync(accountId);
                context.Commit();
            }
        }

    }
}