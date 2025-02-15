using Microsoft.EntityFrameworkCore;

namespace SocialMediaAPI.DataAccess.Repositories
{
    public class Repository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : class
    {
        private readonly SocialMediaDbContext _dbContext;

        public DbSet<TEntity> EntitySet => _dbContext.Set<TEntity>();

        public Repository(SocialMediaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> InsertAsync(TEntity entity)
        {
            _dbContext.Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
