using EVendas.Aplication.Interfaces;
using EVendas.Aplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Vendas_AzureServiceBus.Controllers
{
    [Route("api/[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IServiceBusSender _serviceBusSender;

        public VendaController(IProdutoService produtoService, IServiceBusSender serviceBusSender)
        {
            _produtoService = produtoService;
            _serviceBusSender = serviceBusSender;
        }

        [HttpGet("Listagem")]
        [ProducesResponseType(statusCode: 200, Type = typeof(ProdutoModel))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<ProdutoModel>> ListaEstoque()
        {
            return Ok(_produtoService.GetStock());
        }

        [HttpPost("{codigoProduto}")]
        [ProducesResponseType(statusCode: 200, Type = typeof(ProdutoEditadoModel))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Venda(string codigoProduto, [FromBody][Required] ProdutoVendidoModel produtoVendidoModel)
        {
            await _produtoService.VenderProduto(codigoProduto, produtoVendidoModel.Quantidade);
            await _serviceBusSender.SendProdutoVendidoMessage(codigoProduto, produtoVendidoModel);
            return Ok();
        }
    }
}
