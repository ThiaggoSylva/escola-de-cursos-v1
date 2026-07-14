using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloTurma;

public class TurmaMapeamento : IEntityTypeConfiguration<Turma>
{
    public void Configure(EntityTypeBuilder<Turma> builder)
    {
        builder.ToTable("Turmas");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.DataInicio).IsRequired();
        builder.Property(t => t.DataTermino).IsRequired();
        builder.Property(t => t.CapacidadeMaxima).IsRequired();

        builder.HasOne(t => t.Curso)
            .WithMany()
            .HasForeignKey(t => t.CursoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Instrutor)
            .WithMany()
            .HasForeignKey(t => t.InstrutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Nome).IsUnique();
    }
}
