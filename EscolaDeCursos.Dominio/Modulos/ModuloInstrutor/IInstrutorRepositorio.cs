using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;

public interface IInstrutorRepositorio : IRepositorio<Instrutor>
{
    bool ExisteComEmail(string email, Guid? ignorarId = null);
    bool ExisteComTelefone(string telefone, Guid? ignorarId = null);
}
