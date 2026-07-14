using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloTurma;

public interface ITurmaRepositorio : IRepositorio<Turma>
{
    bool ExisteComNome(string nome, Guid? ignorarId = null);
    bool ExisteTurmaComCurso(Guid cursoId);
    bool ExisteTurmaComInstrutor(Guid instrutorId);
}
