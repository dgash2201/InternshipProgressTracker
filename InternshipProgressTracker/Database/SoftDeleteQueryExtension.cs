using InternshipProgressTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace InternshipProgressTracker.Database
{
    /// <summary>
    /// Contains extension method, which adds a filter for entities marked as deleted
    /// </summary>
    public static class SoftDeleteQueryExtension
    {
        /// <summary>
        /// Adds soft deleted entities filter for this entity type
        /// </summary>
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension)
                .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityData.ClrType);

            var filter = methodToCall.Invoke(null, new object[] { });

            entityData.SetQueryFilter((LambdaExpression)filter);
            entityData.AddIndex(entityData.
                 FindProperty(nameof(ISoftDeletable.IsDeleted)));
        }

        /// <summary>
        /// Filters entities marked as deleted
        /// </summary>
        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, ISoftDeletable
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }
}
