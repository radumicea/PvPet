using Microsoft.EntityFrameworkCore.Query;
using PvPet.Data.Entities;
using System.Linq.Expressions;

namespace Crop360.Business.Services.Generic;

public interface IBaseService<TEntity, TDto>
    where TEntity : Entity
    where TDto : class
{
    Task<IEnumerable<TDto>> QueryAsync(
        Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>? include = null,
        Expression<Func<TDto, bool>>? predicate = null,
        Expression<Func<IQueryable<TDto>, IOrderedQueryable<TDto>>>? orderBy = null
        );
    Task<TDto?> QuerySingleAsync(
        Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>? include = null,
        Expression<Func<TDto, bool>>? predicate = null
        );
    Task<bool> AddRangeAsync(IEnumerable<TDto> models);
    Task<Guid> AddAsync(TDto model);
    Task<bool> UpdateRangeAsync(IEnumerable<TDto> models);
    Task<bool> UpdateAsync(TDto model);
    Task<int> CountAync();
    Task<bool> DeleteRangeAsync(IEnumerable<TDto> models);
    Task<bool> DeleteAsync(TDto model);
}