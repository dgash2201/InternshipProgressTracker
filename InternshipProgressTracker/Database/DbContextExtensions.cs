using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for the database context
    /// </summary>
    public static partial class DbContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="keyValues">Keys for search</param>
        public static TEntity FindTracked<TEntity>(this DbContext context, params object[] keyValues)
            where TEntity : class
        {
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();
            var stateManager = context.GetDependencies().StateManager;
            var entry = stateManager.TryGetEntry(key, keyValues);
            return entry?.Entity as TEntity;
        }
    }
}
