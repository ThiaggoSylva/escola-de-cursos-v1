using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursos.Infra.Modulos.ModuloTurma;

public class TurmaRepositorio(EscolaDeCursosDbContext contexto)
    : RepositorioBase<Turma>(contexto), ITurmaRepositorio
{
    public bool ExisteComNome(string nome, Guid? ignorarId = null)
    {
        return DbSet.Any(t =>
            t.Nome.ToLower() == nome.ToLower() && (ignorarId == null || t.Id != ignorarId));
    }

    public bool ExisteTurmaComCurso(Guid cursoId)
    {
        return DbSet.Any(t => t.CursoId == cursoId);
    }

    public bool ExisteTurmaComInstrutor(Guid instrutorId)
    {
        return DbSet.Any(t => t.InstrutorId == instrutorId);
    }

    public override Turma? SelecionarPorId(Guid idSelecionado)
    {
        return DbSet.AsNoTracking()
            .Include(t => t.Curso)
            .Include(t => t.Instrutor)
            .FirstOrDefault(t => t.Id == idSelecionado);
    }

    public override List<Turma> SelecionarTodos()
    {
        return [.. DbSet.AsNoTracking().Include(t => t.Curso).Include(t => t.Instrutor).OrderBy(t => t.Nome)];
    }
}
