using System;
using System.Collections.Generic;
using System.Text;

namespace EVendas.Aplication.Model
{
    public class ProdutoModel
    {
        public long Id { get; set; }
        public string CodigoProduto { get; set; }
        public string Nome { get; set; }
        public Decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
