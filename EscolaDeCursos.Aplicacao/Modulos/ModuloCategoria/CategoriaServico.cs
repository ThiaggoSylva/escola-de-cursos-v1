using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloCategoria;

public class CategoriaServico(
    ICategoriaRepositorio categoriaRepositorio,
    ICursoRepositorio cursoRepositorio
) : ServicoBase<Categoria>, ICategoriaServico
{
    public Result<Categoria> Cadastrar(Categoria categoria)
    {
        Result resultadoValidacao = ValidarEntidade(categoria);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Categoria>(resultadoValidacao);

        if (categoriaRepositorio.ExisteComTitulo(categoria.Titulo))
            return Falha<Categoria>("Titulo", "Já existe uma categoria cadastrada com este título.");

        categoriaRepositorio.Cadastrar(categoria);

        return Result.Ok(categoria);
    }

    public Result<Categoria> Editar(Guid idSelecionado, Categoria categoriaAtualizada)
    {
        Result resultadoValidacao = ValidarEntidade(categoriaAtualizada);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Categoria>(resultadoValidacao);

        if (categoriaRepositorio.ExisteComTitulo(categoriaAtualizada.Titulo, idSelecionado))
            return Falha<Categoria>("Titulo", "Já existe uma categoria cadastrada com este título.");

        bool editado = categoriaRepositorio.Editar(idSelecionado, categoriaAtualizada);

        if (!editado)
            return Falha<Categoria>(string.Empty, "Categoria não encontrada.");

        return Result.Ok(categoriaAtualizada);
    }

    public Result Excluir(Guid idSelecionado)
    {
        if (cursoRepositorio.ExisteCursoNaCategoria(idSelecionado))
            return Falha(string.Empty, "Não é possível excluir uma categoria que possua cursos vinculados.");

        bool excluido = categoriaRepositorio.Excluir(idSelecionado);

        if (!excluido)
            return Falha(string.Empty, "Categoria não encontrada.");

        return Result.Ok();
    }

    public Categoria? SelecionarPorId(Guid idSelecionado)
    {
        return categoriaRepositorio.SelecionarPorId(idSelecionado);
    }

    public List<Categoria> SelecionarTodos()
    {
        return categoriaRepositorio.SelecionarTodos();
    }
}
