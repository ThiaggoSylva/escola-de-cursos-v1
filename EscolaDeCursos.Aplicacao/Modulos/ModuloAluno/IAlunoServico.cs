using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;

public interface IAlunoServico
{
    Result<Aluno> Cadastrar(Aluno aluno);
    Result<Aluno> Editar(Guid idSelecionado, Aluno alunoAtualizado);
    Result Excluir(Guid idSelecionado);
    Aluno? SelecionarPorId(Guid idSelecionado);
    List<Aluno> SelecionarTodos();
}
