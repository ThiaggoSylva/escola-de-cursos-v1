using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloInstrutor;

public class InstrutorRepositorio(EscolaDeCursosDbContext contexto)
    : RepositorioBase<Instrutor>(contexto), IInstrutorRepositorio
{
    public bool ExisteComEmail(string email, Guid? ignorarId = null)
    {
        return DbSet.Any(i =>
            i.Email.ToLower() == email.ToLower() && (ignorarId == null || i.Id != ignorarId));
    }

    public bool ExisteComTelefone(string telefone, Guid? ignorarId = null)
    {
        return DbSet.Any(i => i.Telefone == telefone && (ignorarId == null || i.Id != ignorarId));
    }
}
