using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursos.Infra.Modulos.ModuloCurso;

public class CursoRepositorio(EscolaDeCursosDbContext contexto)
    : RepositorioBase<Curso>(contexto), ICursoRepositorio
{
    public bool ExisteComTitulo(string titulo, Guid categoriaId, Guid? ignorarId = null)
    {
        return DbSet.Any(c =>
            c.CategoriaId == categoriaId &&
            c.Titulo.ToLower() == titulo.ToLower() &&
            (ignorarId == null || c.Id != ignorarId));
    }

    public bool ExisteCursoNaCategoria(Guid categoriaId)
    {
        return DbSet.Any(c => c.CategoriaId == categoriaId);
    }

    public List<Curso> SelecionarPorCategoria(Guid categoriaId)
    {
        return [.. DbSet.AsNoTracking().Where(c => c.CategoriaId == categoriaId).OrderBy(c => c.Titulo)];
    }

    public override Curso? SelecionarPorId(Guid idSelecionado)
    {
        return DbSet.AsNoTracking()
            .Include(c => c.Categoria)
            .FirstOrDefault(c => c.Id == idSelecionado);
    }

    public override List<Curso> SelecionarTodos()
    {
        return [.. DbSet.AsNoTracking().Include(c => c.Categoria).OrderBy(c => c.Titulo)];
    }
}
