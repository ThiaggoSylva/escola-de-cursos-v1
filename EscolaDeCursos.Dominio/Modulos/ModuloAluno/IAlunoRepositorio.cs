using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public interface IAlunoRepositorio : IRepositorio<Aluno>
{
    bool ExisteComCpf(string cpf, Guid? ignorarId = null);
    bool ExisteComEmail(string email, Guid? ignorarId = null);
    bool ExisteComTelefone(string telefone, Guid? ignorarId = null);
}
