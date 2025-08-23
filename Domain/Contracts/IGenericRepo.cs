using Domain.Entities;

namespace Domain.Contracts;

public interface IGenericRepo<TEntity, in TKey> where TEntity : BaseEntity<TKey>
{
    public Task<TEntity?> GetByIdAsync(TKey? id);
    public Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);
    public Task<TEntity?> GetByIdAsync(Specifications<TEntity> specifications);
    public Task<IEnumerable<TEntity>> GetAllAsync(Specifications<TEntity> specifications,bool asNoTracking = false);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<int> CountAsync(Specifications<TEntity> specifications);
}