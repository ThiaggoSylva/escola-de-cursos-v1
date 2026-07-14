using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;

public class AlunoServico(
    IAlunoRepositorio alunoRepositorio,
    IMatriculaRepositorio matriculaRepositorio
) : ServicoBase<Aluno>, IAlunoServico
{
    public Result<Aluno> Cadastrar(Aluno aluno)
    {
        Result resultadoValidacao = ValidarEntidade(aluno);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Aluno>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(aluno);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Aluno>(resultadoRegras);

        alunoRepositorio.Cadastrar(aluno);

        return Result.Ok(aluno);
    }

    public Result<Aluno> Editar(Guid idSelecionado, Aluno alunoAtualizado)
    {
        Result resultadoValidacao = ValidarEntidade(alunoAtualizado);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Aluno>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(alunoAtualizado, idSelecionado);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Aluno>(resultadoRegras);

        bool editado = alunoRepositorio.Editar(idSelecionado, alunoAtualizado);

        if (!editado)
            return Falha<Aluno>(string.Empty, "Aluno não encontrado.");

        return Result.Ok(alunoAtualizado);
    }

    public Result Excluir(Guid idSelecionado)
    {
        if (matriculaRepositorio.SelecionarPorAluno(idSelecionado).Count > 0)
            return Falha(string.Empty, "Não é possível excluir um aluno que possua matrículas vinculadas.");

        bool excluido = alunoRepositorio.Excluir(idSelecionado);

        if (!excluido)
            return Falha(string.Empty, "Aluno não encontrado.");

        return Result.Ok();
    }

    public Aluno? SelecionarPorId(Guid idSelecionado)
    {
        return alunoRepositorio.SelecionarPorId(idSelecionado);
    }

    public List<Aluno> SelecionarTodos()
    {
        return alunoRepositorio.SelecionarTodos();
    }

    private Result ValidarRegrasDeNegocio(Aluno aluno, Guid? ignorarId = null)
    {
        if (alunoRepositorio.ExisteComCpf(aluno.Cpf, ignorarId))
            return Falha("Cpf", "Já existe um aluno cadastrado com este CPF.");

        if (alunoRepositorio.ExisteComEmail(aluno.Email, ignorarId))
            return Falha("Email", "Já existe um aluno cadastrado com este e-mail.");

        if (alunoRepositorio.ExisteComTelefone(aluno.Telefone, ignorarId))
            return Falha("Telefone", "Já existe um aluno cadastrado com este telefone.");

        return Result.Ok();
    }
}
