using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Api.DataModels;

namespace AccountManager.Api.Repositories.Interfaces
{
    public interface IMeterRepository
    {
         Task<int> CreateAsync(MeterReading meterReading);
         Task<IEnumerable<MeterReading>> ReadByAccountAsync(int accountId);
         Task<MeterReading> ReadAsync(MeterReading meterReading);
         Task<IEnumerable<MeterReading>> ReadAllAsync();
         Task<int> UpdateAsync(MeterReading meterReading);
         Task<int> DeleteByAccountAsync(int accountId);
         Task<int> DeleteAsync(int readingId);
    }
}