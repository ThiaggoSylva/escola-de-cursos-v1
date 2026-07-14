using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Infra.Compartilhado.Orm;

namespace EscolaDeCursos.Infra.Modulos.ModuloCategoria;

public class CategoriaRepositorio(EscolaDeCursosDbContext contexto)
    : RepositorioBase<Categoria>(contexto), ICategoriaRepositorio
{
    public bool ExisteComTitulo(string titulo, Guid? ignorarId = null)
    {
        return DbSet.Any(c =>
            c.Titulo.ToLower() == titulo.ToLower() && (ignorarId == null || c.Id != ignorarId));
    }
}
