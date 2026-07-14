using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloTurma;

public interface ITurmaServico
{
    Result<Turma> Cadastrar(Turma turma);
    Result<Turma> Editar(Guid idSelecionado, Turma turmaAtualizada);
    Result Excluir(Guid idSelecionado);
    Turma? SelecionarPorId(Guid idSelecionado);
    List<Turma> SelecionarTodos();
}
