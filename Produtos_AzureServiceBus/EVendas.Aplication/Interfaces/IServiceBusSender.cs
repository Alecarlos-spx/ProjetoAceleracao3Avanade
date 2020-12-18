using EVendas.Aplication.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EVendas.Aplication.Interfaces
{
    public interface IServiceBusSender
    {
        Task SendCreateProdutoMessage(ProdutoCriadoModel request);
        Task SendUpdateProdutoMessage(string codigoProduto, ProdutoEditadoModel request);
        Task SendProdutoVendidoMessage(string codigoProduto, ProdutoVendidoModel request);
    }
}
