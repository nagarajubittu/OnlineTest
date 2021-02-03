using Dapper.AmbientContext;

namespace AccountManager.Api.Repositories
{
    public class AbstractRepository
    {
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;
        protected AbstractRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        protected IAmbientDbContextQueryProxy Context => _ambientDbContextLocator.Get();
    }
}