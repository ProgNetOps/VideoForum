using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.Entities;

namespace VideoForum.Core.IRepo;

public interface IBaseRepo<T> where T : BaseEntity
{
    void Add(T entity);

    void Update(T source, T destination);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);

    Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);

    Task<T> GetByIdAsync(int id, string? includeProperties);

    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> criteria, string? includeProperties);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> criteria, string? includeProperties, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy);

    Task<int> CountAsync(Expression<Func<T, bool>>? criteria);
}
