using System.Linq.Expressions;
using Fms.Application;
using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Enums;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class TransactionRepository : BaseCrudRepository<TransactionEntity, int>, ITransactionRepository
{
    private readonly TransactionCategoryKindRepository _categoryKindRepository;

    public TransactionRepository(FmsDbContext ctx, TransactionCategoryKindRepository categoryKindRepository) : base(ctx)
    {
        _categoryKindRepository = categoryKindRepository;
    }
    
    public async Task<(int total, IEnumerable<TransactionEntity> items)> ListWorkspaceTransactions(int workspaceId,
        TransactionCriteriaDto criteria, Pagination pagination)
    {
        var query = Ctx.Transactions
            .Where(entity => entity.WorkspaceId == workspaceId);

        if (criteria.CategoryKind is { } categoryKindEnum)
        {
            if (categoryKindEnum == TransactionCategoryKind.Mixed)
            {
                var categoryKind = await _categoryKindRepository.Read(categoryKindEnum);
                query = query.Where(entity => entity.Category.Kind.Id == categoryKind.Id);
            }
            else if (categoryKindEnum == TransactionCategoryKind.Income)
                query = query.Where(entity => entity.Amount > 0);
            else if (categoryKindEnum == TransactionCategoryKind.Expense)
                query = query.Where(entity => entity.Amount < 0);
        }
        
        if (criteria.UserId is { } userId)
            query = query.Where(entity => entity.UserId == userId);
        if (criteria.CategoryId is { } categoryId) // TODO: Add hashset of categories
            query = query.Where(entity => entity.CategoryId == categoryId);
        if (criteria.StartDate is { } startDate)
            query = query.Where(entity => entity.Timestamp >= startDate);
        if (criteria.EndDate is { } endDate)
            query = query.Where(entity => entity.Timestamp <= endDate);
        if (criteria.MinAmount is { } minAmount)
            query = query.Where(entity => Math.Abs(entity.Amount) >= minAmount);
        if (criteria.MaxAmount is { } maxAmount)
            query = query.Where(entity => Math.Abs(entity.Amount) <= maxAmount);

        query = Sort(query, criteria.SortField, criteria.SortDirection); ;
        
        return (
            query.Count(),
            criteria.IncludeAll.GetValueOrDefault(false)
                ? await query.ToListAsync()
                : await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }
    
    // TODO: Refactor
    private IQueryable<TransactionEntity> Sort(IQueryable<TransactionEntity> query, TransactionSortFieldDto? sortField, SortDirectionDto? sortDirection)
    {
        var parameter = Expression.Parameter(typeof(TransactionEntity), "x");
        Expression property = sortField switch
        {
            TransactionSortFieldDto.Timestamp => Expression.Property(parameter, nameof(TransactionEntity.Timestamp)),
            TransactionSortFieldDto.Amount => Expression.Property(parameter, nameof(TransactionEntity.Amount)),
            _ => Expression.Property(parameter, nameof(TransactionEntity.Timestamp))
        };

        var lambda = Expression.Lambda(property, parameter);
        var methodName = sortDirection == SortDirectionDto.Ascending ? "OrderBy" : "OrderByDescending";

        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { query.ElementType, property.Type },
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<TransactionEntity>(resultExpression);
    }
}