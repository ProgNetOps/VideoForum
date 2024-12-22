using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using VideoForum.Core.Entities;
using VideoForum.Core.IRepo;
using VideoForum.DataAccess.Data;

namespace VideoForum.DataAccess.Repo;

public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
{
    private readonly AppDbContext appDbContext;
    internal DbSet<T> context => appDbContext.Set<T>();

    public BaseRepo(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public void Add(T entity)
    {
        context.Add(entity);
        //appDbContext.SaveChanges();
    }

    public void Update(T source, T destination)
    {
        context.Entry(source).CurrentValues.SetValues(destination);
        //appDbContext.SaveChanges();
    }

    public void Remove(T entity)
    {
        context.Remove(entity);
        //appDbContext.SaveChanges();
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        context.RemoveRange(entities);
        //appDbContext.SaveChanges();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria)
    {
        IQueryable<T> query = context.Where(criteria);
        return await query.AnyAsync();
    }

    public async Task<T?> GetByIdAsync(int id, string? includeProperties)
    {
        IQueryable<T> query = context;
        if(string.IsNullOrWhiteSpace(includeProperties) is false)
        {
            query = GetQueryWithIncludedProperties(query,includeProperties);
        }

        return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
    }


    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> criteria, string? includeProperties)
    {

        IQueryable<T> query = context;
        if (string.IsNullOrWhiteSpace(includeProperties) is false)
        {
            query = GetQueryWithIncludedProperties(query, includeProperties);
        }

        return await query.Where(criteria).FirstOrDefaultAsync();
    }


    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? criteria, string? includeProperties, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy)
    {
        IQueryable<T> query = context;
        if(criteria is not null)
        {
            query = query.Where(criteria);
        }

        if (string.IsNullOrWhiteSpace(includeProperties) is false)
        {
            query = GetQueryWithIncludedProperties(query, includeProperties);
        }

        if(orderBy is not null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }


    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
    {
        IQueryable<T> query = context;

        if (criteria is not null)
        {
            query = query.Where(criteria);
        }

        return await query.CountAsync();
    }

    #region Static Methods
    public static IQueryable<T> GetQueryWithIncludedProperties(IQueryable<T> query, string includeProperties)
    {
        List<string> propertiesIncluded = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

        foreach(var property in propertiesIncluded)
        {
            query = query.Include(property);
        }

        return query;
    }

    #endregion

}
