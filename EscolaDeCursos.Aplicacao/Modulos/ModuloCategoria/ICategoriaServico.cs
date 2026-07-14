using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloCategoria;

public interface ICategoriaServico
{
    Result<Categoria> Cadastrar(Categoria categoria);
    Result<Categoria> Editar(Guid idSelecionado, Categoria categoriaAtualizada);
    Result Excluir(Guid idSelecionado);
    Categoria? SelecionarPorId(Guid idSelecionado);
    List<Categoria> SelecionarTodos();
}
