using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloCategoria;

public class CategoriaMapeamento : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(c => c.Titulo).IsUnique();
    }
}
