using Domain.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Reposiory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _context;

        public UnitOfWork(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
