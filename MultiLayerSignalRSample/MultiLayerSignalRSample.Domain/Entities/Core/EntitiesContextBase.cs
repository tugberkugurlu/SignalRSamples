using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public abstract class EntitiesContextBase : DbContext, IEntitiesContext
    {
        /// <summary>
        /// Constructs a new context instance using conventions to create the name of
        /// the database to which a connection will be made. The by-convention name is
        /// the full name (namespace + class name) of the derived context class.  See
        /// the class remarks for how this is used to create a connection. 
        /// </summary>
        protected EntitiesContextBase()
            : base()
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of
        /// the database to which a connection will be made, and initializes it from
        /// the given model.  The by-convention name is the full name (namespace + class
        /// name) of the derived context class.  See the class remarks for how this is
        /// used to create a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        protected EntitiesContextBase(DbCompiledModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made.  See the class
        /// remarks for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public EntitiesContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect
        /// to a database.  The connection will not be disposed when the context is disposed.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">
        /// If set to true the connection is disposed when the context is disposed, otherwise
        /// the caller must dispose the connection.
        /// </param>
        public EntitiesContextBase(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance around an existing ObjectContext.  An existing
        /// ObjectContext to wrap with the new context.  If set to true the ObjectContext
        /// is disposed when the EntitiesContext is disposed, otherwise the caller must dispose
        /// the connection.
        /// </summary>
        /// <param name="objectContext">An existing ObjectContext to wrap with the new context.</param>
        /// <param name="EntitiesContextOwnsObjectContext">
        /// If set to true the ObjectContext is disposed when the EntitiesContext is disposed,
        /// otherwise the caller must dispose the connection.
        /// </param>
        public EntitiesContextBase(ObjectContext objectContext, bool EntitiesContextOwnsObjectContext)
            : base(objectContext, EntitiesContextOwnsObjectContext)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes
        /// it from the given model.  See the class remarks for how this is used to create
        /// a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public EntitiesContextBase(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect
        /// to a database, and initializes it from the given model.  The connection will
        /// not be disposed when the context is disposed.  An existing connection to
        /// use for the new context.  The model that will back this context.  If set
        /// to true the connection is disposed when the context is disposed, otherwise
        /// the caller must dispose the connection.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// If set to true the connection is disposed when the context is disposed, otherwise
        /// the caller must dispose the connection.
        /// </param>
        public EntitiesContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            DbEntityEntry dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = EntityState.Added;
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : class
        {
            DbEntityEntry dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            DbEntityEntry dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        // privates
        private DbEntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : class
        {
            DbEntityEntry dbEntityEntry = base.Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }

            return dbEntityEntry;
        }
    }
}
