using System;
using System.Linq;
using System.Linq.Expressions;

namespace BLL.Services
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query,
            string orderProperty, bool isDescending) 
        {
            var type = typeof(TEntity);
            var property = type.GetProperty(orderProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(

                typeof(Queryable),
                isDescending ? "OrderByDescending" : "OrderBy",
                new Type[] { type, property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExp)
                );
            return query.Provider.CreateQuery<TEntity>(resultExp);
        }
    }
}
