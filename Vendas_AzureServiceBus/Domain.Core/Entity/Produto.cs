using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Entity
{
    public class Produto
    {
        public long Id { get; set; }
        public string CodigoProduto { get; set; }
        public string Nome { get; set; }
        public Decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
