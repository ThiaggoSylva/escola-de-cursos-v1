using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAula;

public interface IAulaRepositorio : IRepositorio<Aula>
{
    bool ExisteOrdemNoCurso(Guid cursoId, int ordem, Guid? ignorarId = null);
    List<Aula> SelecionarPorCurso(Guid cursoId);
}
