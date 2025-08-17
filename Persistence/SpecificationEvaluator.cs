namespace Persistance;

public static class SpecificationEvaluator
{
    public static IQueryable<T>? GetQuery<T>(IQueryable<T> inputQuery, Specifications<T> specifications) where T : class
    {
        var query = inputQuery;

        if (specifications.Criteria != null)
            query = query.Where(specifications.Criteria);

        if (specifications.IncludeExpressions.Any())
        {
            foreach (var includeExpression in specifications.IncludeExpressions)
                query = query.Include(includeExpression);
        }

        if (specifications.OrderByAscending is not null)
            query = query.OrderBy(specifications.OrderByAscending);
        else if (specifications.OrderByDescending is not null)
            query = query.OrderByDescending(specifications.OrderByDescending);

        if (specifications.IsPaginated)
            query = query.Skip(specifications.Skip).Take(specifications.Take);

        return query;
    }
}