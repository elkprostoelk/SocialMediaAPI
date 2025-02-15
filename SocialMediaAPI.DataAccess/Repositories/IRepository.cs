using Microsoft.EntityFrameworkCore;

namespace SocialMediaAPI.DataAccess.Repositories
{
    public interface IRepository<TEntity, TIdentifier> where TEntity : class
    {
        DbSet<TEntity> EntitySet { get; }

        Task<bool> InsertAsync(TEntity entity);
    }
}
