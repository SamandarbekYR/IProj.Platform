using IProj.DataAccess.Data;
using IProj.DataAccess.Interfaces;
using IProj.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IProj.DataAccess.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly AppDbContext _appDbContext;
        public BaseRepository(AppDbContext appDb)
        {
            _appDbContext = appDb;
            _dbSet = appDb.Set<TEntity>();
        }
        public async Task<bool> Add(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                var result = await _appDbContext.SaveChangesAsync();

                return result > 0;
            }
            catch 
            {
                return false;
            }
        }
        public IQueryable<TEntity> GetAll()
        =>  _dbSet.AsQueryable();
    }
}
