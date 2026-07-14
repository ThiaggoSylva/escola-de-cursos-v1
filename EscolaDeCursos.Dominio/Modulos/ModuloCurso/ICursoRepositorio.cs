using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloCurso;

public interface ICursoRepositorio : IRepositorio<Curso>
{
    bool ExisteComTitulo(string titulo, Guid categoriaId, Guid? ignorarId = null);
    bool ExisteCursoNaCategoria(Guid categoriaId);
    List<Curso> SelecionarPorCategoria(Guid categoriaId);
}
