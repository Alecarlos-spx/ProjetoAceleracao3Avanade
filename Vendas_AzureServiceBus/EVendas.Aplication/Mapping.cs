using AutoMapper;
using Domain.Core.Entity;
using EVendas.Aplication.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVendas.Aplication
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProdutoCriadoModel, Produto>();
            CreateMap<ProdutoEditadoModel, Produto>();
            CreateMap<Produto, ProdutoModel>().ReverseMap();
        }
    }
}
