using EscolaDeCursos.Dominio.Modulos.ModuloAula;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursos.Infra.Modulos.ModuloAula;

public class AulaRepositorio(EscolaDeCursosDbContext contexto)
    : RepositorioBase<Aula>(contexto), IAulaRepositorio
{
    public bool ExisteOrdemNoCurso(Guid cursoId, int ordem, Guid? ignorarId = null)
    {
        return DbSet.Any(a =>
            a.CursoId == cursoId && a.Ordem == ordem && (ignorarId == null || a.Id != ignorarId));
    }

    public List<Aula> SelecionarPorCurso(Guid cursoId)
    {
        return [.. DbSet.AsNoTracking()
            .Where(a => a.CursoId == cursoId)
            .OrderBy(a => a.Ordem)];
    }
}
