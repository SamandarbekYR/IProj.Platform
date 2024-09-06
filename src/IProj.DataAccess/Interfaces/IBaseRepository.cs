using IProj.Domain.Entities;

namespace IProj.DataAccess.Interfaces;

public interface IBaseRepository<TEntity> 
    where TEntity : BaseEntity
{
    Task<bool> Add(TEntity entity);
    IQueryable<TEntity> GetAll();
}
