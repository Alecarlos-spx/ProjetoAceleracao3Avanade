using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVendas.Aplication.Model
{
    public class ProdutoEditadoModel
    {
        [MaxLength(100, ErrorMessage = "{0} não deve ser maior que {1}")]
        public string CodigoProduto { get; set; }

        [MaxLength(100, ErrorMessage = "{0} não deve ser maior que {1}")]
        public string Nome { get; set; }

        [Range(typeof(decimal), "1", "999999", ErrorMessage = "{0} deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} deve ser maior que zero")]
        public int QuantidadeEstoque { get; set; }
    }
}
