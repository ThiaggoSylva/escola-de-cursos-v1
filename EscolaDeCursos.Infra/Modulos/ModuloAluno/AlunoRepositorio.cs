using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloAluno;

public class AlunoRepositorio(EscolaDeCursosDbContext contexto)
    : RepositorioBase<Aluno>(contexto), IAlunoRepositorio
{
    public bool ExisteComCpf(string cpf, Guid? ignorarId = null)
    {
        return DbSet.Any(a => a.Cpf == cpf && (ignorarId == null || a.Id != ignorarId));
    }

    public bool ExisteComEmail(string email, Guid? ignorarId = null)
    {
        return DbSet.Any(a =>
            a.Email.ToLower() == email.ToLower() && (ignorarId == null || a.Id != ignorarId));
    }

    public bool ExisteComTelefone(string telefone, Guid? ignorarId = null)
    {
        return DbSet.Any(a => a.Telefone == telefone && (ignorarId == null || a.Id != ignorarId));
    }
}
