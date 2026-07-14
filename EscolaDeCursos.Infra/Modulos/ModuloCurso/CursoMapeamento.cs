using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloCurso;

public class CursoMapeamento : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(c => c.CargaHoraria)
            .IsRequired();

        builder.Property(c => c.Nivel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(c => c.Categoria)
            .WithMany()
            .HasForeignKey(c => c.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.CategoriaId, c.Titulo }).IsUnique();
    }
}
