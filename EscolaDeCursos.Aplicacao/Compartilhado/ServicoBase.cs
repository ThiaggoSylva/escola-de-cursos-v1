using EscolaDeCursos.Dominio.Compartilhado;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Compartilhado;

public abstract class ServicoBase<T> where T : EntidadeBase<T>
{
    protected static Result ValidarEntidade(T entidade)
    {
        List<string> erros = entidade.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        return Falha(string.Empty, erros.First());
    }

    protected static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    protected static Result<TValor> Falha<TValor>(string campo, string mensagem)
    {
        return Result.Fail<TValor>(new Error(mensagem).WithMetadata("Campo", campo));
    }

    protected static Result<TValor> ConverterFalha<TValor>(Result resultado)
    {
        return Result.Fail<TValor>(resultado.Errors);
    }
}
