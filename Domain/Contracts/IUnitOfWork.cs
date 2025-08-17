using Domain.Entities;

namespace Domain.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    IGenericRepo<TEntity,TKey> GetRepo<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
}