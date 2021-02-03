using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Api.DataModels;
using AccountManager.Api.Repositories.Interfaces;
using Dapper.AmbientContext;

namespace AccountManager.Api.Repositories
{
    public class MeterRepository : AbstractRepository, IMeterRepository
    {
        private const string tableName = "MeterReadings";
        public MeterRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }

        public async Task<int> CreateAsync(MeterReading meterReading)
        {
            return await Context.ExecuteAsync($"INSERT INTO {tableName}(AccountId, ReadingDatetime, ReadingValue) VALUES (@AccountId, @ReadingDatetime, @ReadingValue)", meterReading);
        }

        public async Task<IEnumerable<MeterReading>> ReadByAccountAsync(int accountId)
        {
            return await Context.QueryAsync<MeterReading>($"SELECT ReadingId, AccountId, ReadingDatetime, ReadingValue FROM {tableName} WHERE AccountId=@AccountId", new { AccountId = accountId });
        }

        public async Task<MeterReading> ReadAsync(MeterReading meterReading)
        {
            return await Context.QueryFirstOrDefaultAsync<MeterReading>($"SELECT ReadingId, AccountId, ReadingDatetime, ReadingValue FROM {tableName} WHERE AccountId=@AccountId AND ReadingDatetime=@ReadingDatetime AND ReadingValue=@ReadingValue", meterReading);
        }

        public async Task<IEnumerable<MeterReading>> ReadAllAsync()
        {
            return await Context.QueryAsync<MeterReading>($"SELECT ReadingId, AccountId, ReadingDatetime, ReadingValue FROM {tableName}");
        }

        public async Task<int> UpdateAsync(MeterReading meterReading)
        {
            return await Context.ExecuteAsync($"UPDATE {tableName} SET ReadingDatetime=@FirstName, ReadingValue=@LastName WHERE ReadingId=@ReadingId AND AccountId=@AccountId", meterReading);
        }

        public async Task<int> DeleteByAccountAsync(int accountId)
        {
            return await Context.ExecuteAsync($"DELETE FROM {tableName} WHERE AccountId=@AccountId", new { AccountId = accountId });
        }

        public async Task<int> DeleteAsync(int readingId)
        {
            return await Context.ExecuteAsync($"DELETE FROM {tableName} WHERE ReadingId=@ReadingId", new { ReadingId = readingId });
        }
    }
}