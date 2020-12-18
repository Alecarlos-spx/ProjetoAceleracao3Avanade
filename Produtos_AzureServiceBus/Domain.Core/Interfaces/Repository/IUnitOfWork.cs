using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
