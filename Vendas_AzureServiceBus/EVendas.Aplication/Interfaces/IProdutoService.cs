using EVendas.Aplication.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EVendas.Aplication.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoModel> GetAsync(long id);
        ProdutoModel GetCodigoAsync(string codigoProduto);
        Task Create(ProdutoCriadoModel produtoCriadoModel);
        Task Update(string codigoProduto, ProdutoEditadoModel produtoEditadoModel);
        IEnumerable<ProdutoModel> GetAll();
        IEnumerable<ProdutoModel> GetStock();
        Task VenderProduto(string codigoProduto, int quantidade);
    }
}
