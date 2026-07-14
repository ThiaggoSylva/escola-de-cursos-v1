using EscolaDeCursos.Aplicacao.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using FluentResults;

namespace EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;

public class MatriculaServico(
    IMatriculaRepositorio matriculaRepositorio,
    IAlunoRepositorio alunoRepositorio,
    ITurmaRepositorio turmaRepositorio
) : ServicoBase<Matricula>, IMatriculaServico
{
    public Result<Matricula> Matricular(Matricula matricula)
    {
        Result resultadoValidacaoBasica = ValidarEntidade(matricula);

        if (resultadoValidacaoBasica.IsFailed)
            return ConverterFalha<Matricula>(resultadoValidacaoBasica);

        Aluno? aluno = alunoRepositorio.SelecionarPorId(matricula.AlunoId);

        if (aluno is null)
            return Falha<Matricula>("AlunoId", "Somente alunos cadastrados podem ser matriculados.");

        Turma? turma = turmaRepositorio.SelecionarPorId(matricula.TurmaId);

        if (turma is null)
            return Falha<Matricula>("TurmaId", "Somente turmas cadastradas podem receber matrículas.");

        if (matriculaRepositorio.AlunoJaMatriculadoNaTurma(matricula.AlunoId, matricula.TurmaId))
            return Falha<Matricula>(string.Empty, "Este aluno já está matriculado nesta turma.");

        int matriculasNaTurma = matriculaRepositorio.ContarMatriculasNaTurma(matricula.TurmaId);

        if (matriculasNaTurma >= turma.CapacidadeMaxima)
            return Falha<Matricula>(string.Empty, "A turma já atingiu a capacidade máxima de alunos.");

        if (matricula.DataMatricula.Date > turma.DataInicio.Date)
            return Falha<Matricula>("DataMatricula", "A data da matrícula deve ser igual ou anterior ao início da turma.");

        matriculaRepositorio.Cadastrar(matricula);

        return Result.Ok(matricula);
    }

    public Result Cancelar(Guid idSelecionado)
    {
        Matricula? matricula = matriculaRepositorio.SelecionarPorId(idSelecionado);

        if (matricula is null)
            return Falha(string.Empty, "Matrícula não encontrada.");

        if (matricula.Situacao == SituacaoMatricula.Concluida)
            return Falha(string.Empty, "Não é possível excluir matrículas concluídas sem registro histórico.");

        matricula.Situacao = SituacaoMatricula.Cancelada;

        bool editado = matriculaRepositorio.Editar(idSelecionado, matricula);

        if (!editado)
            return Falha(string.Empty, "Matrícula não encontrada.");

        return Result.Ok();
    }

    public Matricula? SelecionarPorId(Guid idSelecionado)
    {
        return matriculaRepositorio.SelecionarPorId(idSelecionado);
    }

    public List<Matricula> SelecionarTodos()
    {
        return matriculaRepositorio.SelecionarTodos();
    }

    public List<Matricula> SelecionarPorAluno(Guid alunoId)
    {
        return matriculaRepositorio.SelecionarPorAluno(alunoId);
    }

    public List<Matricula> SelecionarPorTurma(Guid turmaId)
    {
        return matriculaRepositorio.SelecionarPorTurma(turmaId);
    }
}
