using Domain.Core.Entity;
using Domain.Core.Interfaces.Repository;
using Infra.Reposiory.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Reposiory
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(RepositoryContext context) : base(context)
        {
        }
    }
}
