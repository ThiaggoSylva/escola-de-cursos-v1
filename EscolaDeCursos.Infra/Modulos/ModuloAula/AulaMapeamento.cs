using EscolaDeCursos.Dominio.Modulos.ModuloAula;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloAula;

public class AulaMapeamento : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.ToTable("Aulas");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Titulo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Ordem)
            .IsRequired();

        builder.Property(a => a.Duracao)
            .IsRequired();

        builder.HasOne(a => a.Curso)
            .WithMany()
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new { a.CursoId, a.Ordem }).IsUnique();
    }
}
