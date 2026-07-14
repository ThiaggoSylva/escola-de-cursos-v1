using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloMatricula;

public class MatriculaMapeamento : IEntityTypeConfiguration<Matricula>
{
    public void Configure(EntityTypeBuilder<Matricula> builder)
    {
        builder.ToTable("Matriculas");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.DataMatricula).IsRequired();

        builder.Property(m => m.Situacao)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(m => m.Aluno)
            .WithMany()
            .HasForeignKey(m => m.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Turma)
            .WithMany()
            .HasForeignKey(m => m.TurmaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => new { m.AlunoId, m.TurmaId }).IsUnique();
    }
}
