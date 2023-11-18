using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PvPet.Data.Entities;
using System.Linq.Expressions;

namespace Crop360.Business.Services.Generic;

public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
    where TEntity : Entity
    where TDto : class
{
    protected readonly DbContext _context;
    protected readonly IMapper _mapper;
    protected readonly DbSet<TEntity> _entitiesSet;

    public BaseService(DbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _entitiesSet = _context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TDto>> QueryAsync(
        Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>? include = null,
        Expression<Func<TDto, bool>>? predicate = null,
        Expression<Func<IQueryable<TDto>, IOrderedQueryable<TDto>>>? orderBy = null
        )
    {
        var entities = await QueryInternal(include, predicate, orderBy).ToListAsync();
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<TDto?> QuerySingleAsync(
        Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>? include = null,
        Expression<Func<TDto, bool>>? predicate = null
        )
    {
        var entity = await QueryInternal(include, predicate).SingleOrDefaultAsync();
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<bool> AddRangeAsync(IEnumerable<TDto> models)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(models);
        _entitiesSet.AddRange(entities);
        return await _context.SaveChangesAsync() >= 0;
    }

    public virtual async Task<Guid> AddAsync(TDto model)
    {
        var entity = _mapper.Map<TEntity>(model);
        _entitiesSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task<bool> UpdateRangeAsync(IEnumerable<TDto> models)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(models);
        _context.UpdateRange(entities);
        return await _context.SaveChangesAsync() >= 0;
    }

    public virtual async Task<bool> UpdateAsync(TDto model)
    {
        var entity = _mapper.Map<TEntity>(model);
        _context.Attach(entity);
        var entry = _context.Entry(entity);

        foreach (var prop in typeof(TDto).GetProperties())
        {
            var value = prop.GetValue(model, null);
            if (value is null)
            {
                continue;
            }
             
            if (entry.Properties.Any(p => p.Metadata.Name == prop.Name && !p.Metadata.IsKey()))
            {
                entry.Property(prop.Name).IsModified = true;
            }
        }

        return await _context.SaveChangesAsync() >= 0;
    }

    public virtual async Task<int> CountAync()
    {
        return await _entitiesSet.CountAsync();
    }

    public virtual async Task<bool> DeleteRangeAsync(IEnumerable<TDto> models)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(models);
        _entitiesSet.RemoveRange(entities);
        return await _context.SaveChangesAsync() >= 0;
    }

    public virtual async Task<bool> DeleteAsync(TDto model)
    {
        var entity = _mapper.Map<TEntity>(model);
        _entitiesSet.Remove(entity);
        return await _context.SaveChangesAsync() >= 0;
    }

    protected IQueryable<TEntity> QueryInternal(
        Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>? include = null,
        Expression<Func<TDto, bool>>? predicate = null,
        Expression<Func<IQueryable<TDto>, IOrderedQueryable<TDto>>>? orderBy = null
        )
    { 
        var query = _entitiesSet.AsNoTracking();

        if (include is not null)
        {
            var mappedInclude = _mapper
                .MapExpressionAsInclude<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(include)
                .Compile();

            query = mappedInclude(query);
        }

        if (predicate is not null)
        {
            var mappedPredicate = _mapper
                .MapExpression<Expression<Func<TEntity, bool>>>(predicate);

            query = query.Where(mappedPredicate);
        }


        if (orderBy is not null)
        {
            var mappedOrderBy = _mapper
                .MapExpression<Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>>(orderBy)
                .Compile();

            query = mappedOrderBy(query);
        }

        return query;
    }
}
