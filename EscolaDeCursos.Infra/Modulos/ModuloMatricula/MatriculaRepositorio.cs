using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursos.Infra.Modulos.ModuloMatricula;

public class MatriculaRepositorio(EscolaDeCursosDbContext contexto)
    : RepositorioBase<Matricula>(contexto), IMatriculaRepositorio
{
    public bool AlunoJaMatriculadoNaTurma(Guid alunoId, Guid turmaId, Guid? ignorarId = null)
    {
        return DbSet.Any(m =>
            m.AlunoId == alunoId &&
            m.TurmaId == turmaId &&
            m.Situacao != SituacaoMatricula.Cancelada &&
            (ignorarId == null || m.Id != ignorarId));
    }

    public int ContarMatriculasNaTurma(Guid turmaId)
    {
        return DbSet.Count(m => m.TurmaId == turmaId && m.Situacao != SituacaoMatricula.Cancelada);
    }

    public List<Matricula> SelecionarPorAluno(Guid alunoId)
    {
        return [.. DbSet.AsNoTracking()
            .Include(m => m.Turma)
            .ThenInclude(t => t!.Curso)
            .Where(m => m.AlunoId == alunoId)
            .OrderByDescending(m => m.DataMatricula)];
    }

    public List<Matricula> SelecionarPorTurma(Guid turmaId)
    {
        return [.. DbSet.AsNoTracking()
            .Include(m => m.Aluno)
            .Where(m => m.TurmaId == turmaId)
            .OrderByDescending(m => m.DataMatricula)];
    }

    public override Matricula? SelecionarPorId(Guid idSelecionado)
    {
        return DbSet.AsNoTracking()
            .Include(m => m.Aluno)
            .Include(m => m.Turma)
            .ThenInclude(t => t!.Curso)
            .FirstOrDefault(m => m.Id == idSelecionado);
    }

    public override List<Matricula> SelecionarTodos()
    {
        return [.. DbSet.AsNoTracking()
            .Include(m => m.Aluno)
            .Include(m => m.Turma)
            .ThenInclude(t => t!.Curso)
            .OrderByDescending(m => m.DataMatricula)];
    }
}
