using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscolaDeCursos.Infra.Modulos.ModuloUsuario;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        // Cada usuário pode estar vinculado a NO MÁXIMO um Aluno e/ou um
        // Instrutor. É esse vínculo que a aplicação usa para decidir quais
        // dados aquele usuário pode enxergar (segregação de dados).
        builder.HasOne(u => u.Aluno)
            .WithOne()
            .HasForeignKey<Usuario>(u => u.AlunoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.Instrutor)
            .WithOne()
            .HasForeignKey<Usuario>(u => u.InstrutorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
