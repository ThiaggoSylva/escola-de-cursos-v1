using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloTurma;

public class TurmaServico(
    ITurmaRepositorio turmaRepositorio,
    ICursoRepositorio cursoRepositorio,
    IInstrutorRepositorio instrutorRepositorio,
    IMatriculaRepositorio matriculaRepositorio
) : ServicoBase<Turma>, ITurmaServico
{
    public Result<Turma> Cadastrar(Turma turma)
    {
        Result resultadoValidacao = ValidarEntidade(turma);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Turma>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(turma);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Turma>(resultadoRegras);

        turmaRepositorio.Cadastrar(turma);

        return Result.Ok(turma);
    }

    public Result<Turma> Editar(Guid idSelecionado, Turma turmaAtualizada)
    {
        Result resultadoValidacao = ValidarEntidade(turmaAtualizada);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Turma>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(turmaAtualizada, idSelecionado);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Turma>(resultadoRegras);

        bool editado = turmaRepositorio.Editar(idSelecionado, turmaAtualizada);

        if (!editado)
            return Falha<Turma>(string.Empty, "Turma não encontrada.");

        return Result.Ok(turmaAtualizada);
    }

    public Result Excluir(Guid idSelecionado)
    {
        if (matriculaRepositorio.SelecionarPorTurma(idSelecionado).Count > 0)
            return Falha(string.Empty, "Não é possível excluir uma turma que possua matrículas vinculadas.");

        bool excluido = turmaRepositorio.Excluir(idSelecionado);

        if (!excluido)
            return Falha(string.Empty, "Turma não encontrada.");

        return Result.Ok();
    }

    public Turma? SelecionarPorId(Guid idSelecionado)
    {
        return turmaRepositorio.SelecionarPorId(idSelecionado);
    }

    public List<Turma> SelecionarTodos()
    {
        return turmaRepositorio.SelecionarTodos();
    }

    private Result ValidarRegrasDeNegocio(Turma turma, Guid? ignorarId = null)
    {
        Curso? curso = cursoRepositorio.SelecionarPorId(turma.CursoId);

        if (curso is null)
            return Falha("CursoId", "O curso informado não existe.");

        Instrutor? instrutor = instrutorRepositorio.SelecionarPorId(turma.InstrutorId);

        if (instrutor is null)
            return Falha("InstrutorId", "O instrutor informado não existe.");

        if (turmaRepositorio.ExisteComNome(turma.Nome, ignorarId))
            return Falha("Nome", "Já existe uma turma cadastrada com este nome ou código.");

        return Result.Ok();
    }
}
