using Domain.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Reposiory
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.Property(o => o.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(o => o.CodigoProduto).IsRequired().HasMaxLength(20);
            builder.Property(o => o.Nome).IsRequired().HasMaxLength(80);
            builder.Property(o => o.Preco).IsRequired();
            builder.Property(o => o.QuantidadeEstoque).IsRequired();
        }

    }
}