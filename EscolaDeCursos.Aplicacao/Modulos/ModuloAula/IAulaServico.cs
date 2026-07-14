using EscolaDeCursos.Dominio.Modulos.ModuloAula;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloAula;

public interface IAulaServico
{
    Result<Aula> Cadastrar(Aula aula);
    Result<Aula> Editar(Guid idSelecionado, Aula aulaAtualizada);
    Result Excluir(Guid idSelecionado);
    Aula? SelecionarPorId(Guid idSelecionado);
    List<Aula> SelecionarPorCurso(Guid cursoId);
}
