using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;

public class CursoServico(
    ICursoRepositorio cursoRepositorio,
    ICategoriaRepositorio categoriaRepositorio,
    ITurmaRepositorio turmaRepositorio
) : ServicoBase<Curso>, ICursoServico
{
    public Result<Curso> Cadastrar(Curso curso)
    {
        Result resultadoValidacao = ValidarEntidade(curso);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Curso>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(curso);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Curso>(resultadoRegras);

        cursoRepositorio.Cadastrar(curso);

        return Result.Ok(curso);
    }

    public Result<Curso> Editar(Guid idSelecionado, Curso cursoAtualizado)
    {
        Result resultadoValidacao = ValidarEntidade(cursoAtualizado);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Curso>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(cursoAtualizado, idSelecionado);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Curso>(resultadoRegras);

        bool editado = cursoRepositorio.Editar(idSelecionado, cursoAtualizado);

        if (!editado)
            return Falha<Curso>(string.Empty, "Curso não encontrado.");

        return Result.Ok(cursoAtualizado);
    }

    public Result Excluir(Guid idSelecionado)
    {
        if (turmaRepositorio.ExisteTurmaComCurso(idSelecionado))
            return Falha(string.Empty, "Não é possível excluir um curso que possua turmas vinculadas.");

        bool excluido = cursoRepositorio.Excluir(idSelecionado);

        if (!excluido)
            return Falha(string.Empty, "Curso não encontrado.");

        return Result.Ok();
    }

    public Curso? SelecionarPorId(Guid idSelecionado)
    {
        return cursoRepositorio.SelecionarPorId(idSelecionado);
    }

    public List<Curso> SelecionarTodos()
    {
        return cursoRepositorio.SelecionarTodos();
    }

    public List<Curso> SelecionarPorCategoria(Guid categoriaId)
    {
        return cursoRepositorio.SelecionarPorCategoria(categoriaId);
    }

    private Result ValidarRegrasDeNegocio(Curso curso, Guid? ignorarId = null)
    {
        Categoria? categoria = categoriaRepositorio.SelecionarPorId(curso.CategoriaId);

        if (categoria is null)
            return Falha("CategoriaId", "A categoria informada não existe.");

        if (cursoRepositorio.ExisteComTitulo(curso.Titulo, curso.CategoriaId, ignorarId))
            return Falha("Titulo", "Já existe um curso com este título nesta categoria.");

        return Result.Ok();
    }
}
