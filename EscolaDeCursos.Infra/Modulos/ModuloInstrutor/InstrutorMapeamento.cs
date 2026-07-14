using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloInstrutor;

public class InstrutorMapeamento : IEntityTypeConfiguration<Instrutor>
{
    public void Configure(EntityTypeBuilder<Instrutor> builder)
    {
        builder.ToTable("Instrutores");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(i => i.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(i => i.Especialidade)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(i => i.Email).IsUnique();
        builder.HasIndex(i => i.Telefone).IsUnique();
    }
}
