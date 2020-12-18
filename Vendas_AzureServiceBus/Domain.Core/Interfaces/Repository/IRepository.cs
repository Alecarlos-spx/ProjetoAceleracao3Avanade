using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Interfaces.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task AddAsync(TEntity obj);
        Task<TEntity> GetAsync(long id);
        
        Task<TEntity> GetCodigoAsync(string codigoProduto);

        IQueryable<TEntity> GetAll();
        void Update(TEntity obj);
        Task DeleteAsync(long id);

    }
}
