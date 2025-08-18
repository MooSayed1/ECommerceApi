using Domain.Contracts;
using Persistance.Data.Contexts;

namespace Persistance.Repositories;

public class GenericRepo<TEntity, TKey> : IGenericRepo<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    private readonly AppDbContext _context;

    public GenericRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByIdAsync(TKey? id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
    {
        return asNoTracking
            ? await _context.Set<TEntity>().AsNoTracking().ToListAsync()
            : await _context.Set<TEntity>().ToListAsync();
    }

    public Task<TEntity?> GetByIdAsync(Specifications<TEntity> specifications)
    {
        var query = ApplySpecification(specifications);
        return query!.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Specifications<TEntity> specifications,
        bool asNoTracking = false)
    {
        return await ApplySpecification(specifications)!.ToListAsync();
    }

    public async Task<int> CountAsync(Specifications<TEntity> specifications)
        => await ApplySpecification(specifications)!.CountAsync();

    private IQueryable<TEntity>? ApplySpecification(Specifications<TEntity> specifications)
        => SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specifications);

    public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);

    public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}