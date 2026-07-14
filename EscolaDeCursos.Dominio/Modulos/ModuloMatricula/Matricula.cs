using EscolaDeCursos.Dominio.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;

namespace EscolaDeCursos.Dominio.Modulos.ModuloMatricula;

public class Matricula : EntidadeBase<Matricula>
{
    public DateTime DataMatricula { get; set; }
    public SituacaoMatricula Situacao { get; set; } = SituacaoMatricula.Ativa;

    public Guid AlunoId { get; set; }
    public Aluno? Aluno { get; set; }

    public Guid TurmaId { get; set; }
    public Turma? Turma { get; set; }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (AlunoId == Guid.Empty)
            erros.Add("A matrícula deve possuir um aluno.");

        if (TurmaId == Guid.Empty)
            erros.Add("A matrícula deve possuir uma turma.");

        if (DataMatricula == default)
            erros.Add("A data da matrícula é obrigatória.");

        if (Turma is not null && DataMatricula > Turma.DataInicio)
            erros.Add("A data da matrícula deve ser igual ou anterior ao início da turma.");

        return erros;
    }

    public override void Atualizar(Matricula entidadeAtualizada)
    {
        DataMatricula = entidadeAtualizada.DataMatricula;
        Situacao = entidadeAtualizada.Situacao;
        AlunoId = entidadeAtualizada.AlunoId;
        TurmaId = entidadeAtualizada.TurmaId;
    }
}
