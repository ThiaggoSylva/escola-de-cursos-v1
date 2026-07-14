using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloAluno;

public class AlunoMapeamento : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.ToTable("Alunos");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(a => a.Cpf).IsUnique();
        builder.HasIndex(a => a.Email).IsUnique();
        builder.HasIndex(a => a.Telefone).IsUnique();
    }
}
