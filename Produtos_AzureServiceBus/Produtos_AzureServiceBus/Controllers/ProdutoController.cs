using EVendas.Aplication.Interfaces;
using EVendas.Aplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Produtos_AzureServiceBus.Controllers
{
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IServiceBusSender _serviceBusSender;

        public ProdutoController(IProdutoService produtoService, IServiceBusSender serviceBusSender)
        {
            _produtoService = produtoService;
            _serviceBusSender = serviceBusSender;
        }

        [HttpGet("Listagem")]
        [ProducesResponseType(statusCode: 200, Type = typeof(ProdutoModel))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, Type = typeof(ErrorResponse))]
        public ActionResult<List<ProdutoModel>> GetAll()
        {
            return Ok(_produtoService.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: 200, Type = typeof(ProdutoModel))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetAsync(long id)
        {
            return Ok(await _produtoService.GetAsync(id));
        }

        [HttpGet]
        [Route("{codProduto}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCodigoProduto(string codProduto)
        {
            return Ok(_produtoService.GetCodigo(codProduto));
        }



        [HttpPost]
        [ProducesResponseType(statusCode: 200, Type = typeof(ProdutoCriadoModel))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Create([FromBody][Required] ProdutoCriadoModel produtoCriado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _produtoService.Create(produtoCriado);
            await _serviceBusSender.SendCreateProdutoMessage(produtoCriado);

            return Ok(produtoCriado);
        }

        [HttpPut("{codProduto}")]
        [ProducesResponseType(statusCode: 200, Type = typeof(ProdutoCriadoModel))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Update(string codProduto, [FromBody][Required] ProdutoEditadoModel produtoEditado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _produtoService.Update(codProduto, produtoEditado);
            await _serviceBusSender.SendUpdateProdutoMessage(codProduto, produtoEditado);

            return Ok(produtoEditado);
        }
    }
}
