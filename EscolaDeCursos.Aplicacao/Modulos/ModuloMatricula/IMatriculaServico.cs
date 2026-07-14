using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;

public interface IMatriculaServico
{
    Result<Matricula> Matricular(Matricula matricula);
    Result Cancelar(Guid idSelecionado);
    Matricula? SelecionarPorId(Guid idSelecionado);
    List<Matricula> SelecionarTodos();
    List<Matricula> SelecionarPorAluno(Guid alunoId);
    List<Matricula> SelecionarPorTurma(Guid turmaId);
}
