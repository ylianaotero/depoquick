using System.Linq.Expressions;
using DepoQuick.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BusinessLogic;

public interface IRepository<T>
{
    void Add(T element);
    
    T? GetById(int id);

    List<T> GetBy(Func<T, bool> predicate);
    
    List<T> GetAll();
    
    void Update(T element);
    
    void Delete(int id);

    void Reload(T element);

    List<T> GetFilteredAndIncludedRelatedEntities<T>(
        Expression<Func<T, bool>> filterExpression,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : class;
}