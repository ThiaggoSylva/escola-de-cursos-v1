using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;

public interface ICursoServico
{
    Result<Curso> Cadastrar(Curso curso);
    Result<Curso> Editar(Guid idSelecionado, Curso cursoAtualizado);
    Result Excluir(Guid idSelecionado);
    Curso? SelecionarPorId(Guid idSelecionado);
    List<Curso> SelecionarTodos();
    List<Curso> SelecionarPorCategoria(Guid categoriaId);
}
