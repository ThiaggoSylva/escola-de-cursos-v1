using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloAula;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloAula;

public class AulaServico(
    IAulaRepositorio aulaRepositorio,
    ICursoRepositorio cursoRepositorio
) : ServicoBase<Aula>, IAulaServico
{
    public Result<Aula> Cadastrar(Aula aula)
    {
        Result resultadoValidacao = ValidarEntidade(aula);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Aula>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(aula);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Aula>(resultadoRegras);

        aulaRepositorio.Cadastrar(aula);

        return Result.Ok(aula);
    }

    public Result<Aula> Editar(Guid idSelecionado, Aula aulaAtualizada)
    {
        Result resultadoValidacao = ValidarEntidade(aulaAtualizada);

        if (resultadoValidacao.IsFailed)
            return ConverterFalha<Aula>(resultadoValidacao);

        Result resultadoRegras = ValidarRegrasDeNegocio(aulaAtualizada, idSelecionado);

        if (resultadoRegras.IsFailed)
            return ConverterFalha<Aula>(resultadoRegras);

        bool editado = aulaRepositorio.Editar(idSelecionado, aulaAtualizada);

        if (!editado)
            return Falha<Aula>(string.Empty, "Aula não encontrada.");

        return Result.Ok(aulaAtualizada);
    }

    public Result Excluir(Guid idSelecionado)
    {
        bool excluido = aulaRepositorio.Excluir(idSelecionado);

        if (!excluido)
            return Falha(string.Empty, "Aula não encontrada.");

        return Result.Ok();
    }

    public Aula? SelecionarPorId(Guid idSelecionado)
    {
        return aulaRepositorio.SelecionarPorId(idSelecionado);
    }

    public List<Aula> SelecionarPorCurso(Guid cursoId)
    {
        return aulaRepositorio.SelecionarPorCurso(cursoId);
    }

    private Result ValidarRegrasDeNegocio(Aula aula, Guid? ignorarId = null)
    {
        Curso? curso = cursoRepositorio.SelecionarPorId(aula.CursoId);

        if (curso is null)
            return Falha("CursoId", "Não é possível cadastrar uma aula sem um curso associado.");

        if (aulaRepositorio.ExisteOrdemNoCurso(aula.CursoId, aula.Ordem, ignorarId))
            return Falha("Ordem", "Já existe uma aula com esta ordem neste curso.");

        return Result.Ok();
    }
}
