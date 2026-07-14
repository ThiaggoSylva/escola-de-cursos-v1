using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloInstrutor;

public interface IInstrutorServico
{
    Result<Instrutor> Cadastrar(Instrutor instrutor);
    Result<Instrutor> Editar(Guid idSelecionado, Instrutor instrutorAtualizado);
    Result Excluir(Guid idSelecionado);
    Instrutor? SelecionarPorId(Guid idSelecionado);
    List<Instrutor> SelecionarTodos();
}
