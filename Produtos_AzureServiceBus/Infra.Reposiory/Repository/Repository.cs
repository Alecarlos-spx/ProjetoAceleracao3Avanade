using Domain.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Reposiory.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly RepositoryContext _Context;
        private readonly DbSet<TEntity> _DbSet;

        public Repository(RepositoryContext context)
        {
            _Context = context;
            _DbSet = context.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity obj)
        {
            await _DbSet.AddAsync(obj);
        }

        public virtual async Task DeleteAsync(long id)
        {
            _DbSet.Remove(await _DbSet.FindAsync(id));
        }

        public virtual IQueryable<TEntity> GetAllAsync()
        {
            return _DbSet;
        }

        public virtual async Task<TEntity> GetAsync(long id)
        {
            return await _DbSet.FindAsync(id);
        }

        public virtual void Update(TEntity obj)
        {
            _DbSet.Update(obj);
        }
        public void Dispose()
        {
            _Context.Dispose();
            //GC.SuppressFinalize(this);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _DbSet;
        }

        public TEntity GetCodigoAsync(string codigoProduto)
        {
            throw new NotImplementedException();
        }

       
    }
}
