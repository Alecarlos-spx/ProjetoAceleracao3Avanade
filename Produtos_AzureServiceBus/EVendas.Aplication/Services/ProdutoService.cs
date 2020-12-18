using EVendas.Aplication.Interfaces;
using EVendas.Aplication.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Interfaces.Repository;
using AutoMapper;
using System.Linq;
using Domain.Core.Entity;

namespace EVendas.Aplication.Services
{
    public class ProdutoService : IProdutoService
    {
        
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(ProdutoCriadoModel produtoCriadoModel)
        {
            if (produtoCriadoModel.Preco > 0 && produtoCriadoModel.QuantidadeEstoque > 0)
            {
                if(_produtoRepository.GetAll().Where(x => x.CodigoProduto.ToUpper() == produtoCriadoModel.CodigoProduto.ToUpper()).FirstOrDefault() == null)
                {
                    await _produtoRepository.AddAsync(_mapper.Map<Produto>(produtoCriadoModel));
                    await _unitOfWork.CommitAsync();
                }
            }
        }

        public IEnumerable<ProdutoModel> GetAll()
        {
            return _mapper.Map<List<ProdutoModel>>(_produtoRepository.GetAll());
        }

        public async Task<ProdutoModel> GetAsync(long id)
        {
            return _mapper.Map<ProdutoModel>(await _produtoRepository.GetAsync(id));
        }

        public ProdutoModel GetCodigo(string codigoProduto)
        {
            var retorno = _produtoRepository.GetAll().Where(x => x.CodigoProduto.ToUpper() == codigoProduto.ToUpper()).FirstOrDefault();

            return _mapper.Map<ProdutoModel>(retorno);
        }

        public IEnumerable<ProdutoModel> GetStock()
        {
            return _mapper.Map<List<ProdutoModel>>(_produtoRepository.GetAll().Where(x => x.QuantidadeEstoque > 0));
        }

        public async Task Update(string codigoProduto, ProdutoEditadoModel produtoEditadoModel)
        {
            var produto = _produtoRepository.GetAll().Where(x => x.CodigoProduto.ToUpper() == codigoProduto.ToUpper()).FirstOrDefault();
            _mapper.Map(produtoEditadoModel, produto);
            _produtoRepository.Update(produto);
            await _unitOfWork.CommitAsync();
        }

        public async Task VenderProduto(string codigoProduto, int quantidade)
        {
            var produto = _produtoRepository.GetAll().Where(x => x.CodigoProduto.ToUpper() == codigoProduto.ToUpper()).FirstOrDefault();

            if (produto.QuantidadeEstoque - quantidade >= 0)
            {
                produto.QuantidadeEstoque -= quantidade;
                _produtoRepository.Update(produto);
                await _unitOfWork.CommitAsync();
            }
        }

      
    }
}
