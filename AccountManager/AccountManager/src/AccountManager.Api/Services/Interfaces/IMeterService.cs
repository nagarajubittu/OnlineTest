using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Api.Models;

namespace AccountManager.Api.Services.Interfaces
{
    public interface IMeterService
    {
        Task<PostMeterReadingUploadsResponse> LoadFromCsvAsync(Microsoft.AspNetCore.Http.IFormFile file);
        Task CreateAsync(MeterReadingRequestModel meterReadingRequestModel);
        Task<List<GetMeterReadingResponseModel>> GetAsync(int accountId = 0);
        Task DeleteAsync(int meterId);
        Task DeleteByAccount(int accountId);
    }
}