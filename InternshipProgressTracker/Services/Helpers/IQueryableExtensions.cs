using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InternshipProgressTracker.Services.Extensions
{
    public static class IQueryableExtensions
    {
        internal static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IEnumerable<Expression<Func<T, bool>>> filters) where T : class
        {
            foreach (var criteria in filters)
            {
                query = query.Where(criteria);
            }

            return query;
        }
    }
}
