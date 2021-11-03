using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PIM.Api.Data.Context;

namespace PIM.Api.Data.Repositories.Base
{
    public class BaseRepository<TEntity> : IDisposable where TEntity : class
    {
        protected readonly ApplicationContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(ApplicationContext db)
        {
            Db = db;
            DbSet = Db.Set<TEntity>();
        }

        public virtual async Task SaveAsync(TEntity entity) => await DbSet.AddAsync(entity);
        public virtual async Task SaveAndCommitAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await SaveChangesAsync();
        }
        public virtual async Task<TEntity> GetAsync(int id) => await DbSet.FindAsync(id);
        public virtual void Update(TEntity obj) => DbSet.Update(obj);
        public virtual void Remove(int id) => DbSet.Remove(DbSet.Find(id));
        public async Task SaveChangesAsync() => await Db.SaveChangesAsync();

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
