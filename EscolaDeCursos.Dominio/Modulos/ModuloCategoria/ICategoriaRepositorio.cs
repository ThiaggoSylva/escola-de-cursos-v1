using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloCategoria;

public interface ICategoriaRepositorio : IRepositorio<Categoria>
{
    bool ExisteComTitulo(string titulo, Guid? ignorarId = null);
}
