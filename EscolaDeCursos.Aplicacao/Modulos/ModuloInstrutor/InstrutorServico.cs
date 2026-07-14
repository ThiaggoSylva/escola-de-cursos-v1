using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloInstrutor;

public class InstrutorServico(
    IInstrutorRepositorio instrutorRepositorio,
    ITurmaRepositorio turmaRepositorio
) : ServicoBase<Instrutor>, IInstrutorServico
{
    public Result<Instrutor> Cadastrar(Instrutor instrutor)
    {
        Result resultadoValidacao = ValidarEntidade(instrutor);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Instrutor>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(instrutor);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Instrutor>(resultadoRegras);

        instrutorRepositorio.Cadastrar(instrutor);

        return Result.Ok(instrutor);
    }

    public Result<Instrutor> Editar(Guid idSelecionado, Instrutor instrutorAtualizado)
    {
        Result resultadoValidacao = ValidarEntidade(instrutorAtualizado);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Instrutor>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(instrutorAtualizado, idSelecionado);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Instrutor>(resultadoRegras);

        bool editado = instrutorRepositorio.Editar(idSelecionado, instrutorAtualizado);

        if (!editado)
            return Falha<Instrutor>(string.Empty, "Instrutor não encontrado.");

        return Result.Ok(instrutorAtualizado);
    }

    public Result Excluir(Guid idSelecionado)
    {
        if (turmaRepositorio.ExisteTurmaComInstrutor(idSelecionado))
            return Falha(string.Empty, "Não é possível excluir um instrutor vinculado a turmas.");

        bool excluido = instrutorRepositorio.Excluir(idSelecionado);

        if (!excluido)
            return Falha(string.Empty, "Instrutor não encontrado.");

        return Result.Ok();
    }

    public Instrutor? SelecionarPorId(Guid idSelecionado)
    {
        return instrutorRepositorio.SelecionarPorId(idSelecionado);
    }

    public List<Instrutor> SelecionarTodos()
    {
        return instrutorRepositorio.SelecionarTodos();
    }

    private Result ValidarRegrasDeNegocio(Instrutor instrutor, Guid? ignorarId = null)
    {
        if (instrutorRepositorio.ExisteComEmail(instrutor.Email, ignorarId))
            return Falha("Email", "Já existe um instrutor cadastrado com este e-mail.");

        if (instrutorRepositorio.ExisteComTelefone(instrutor.Telefone, ignorarId))
            return Falha("Telefone", "Já existe um instrutor cadastrado com este telefone.");

        return Result.Ok();
    }
}
