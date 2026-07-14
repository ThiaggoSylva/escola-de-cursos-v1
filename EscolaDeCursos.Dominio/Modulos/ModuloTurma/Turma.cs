using EscolaDeCursos.Dominio.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;

namespace EscolaDeCursos.Dominio.Modulos.ModuloTurma;

public class Turma : EntidadeBase<Turma>
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public int CapacidadeMaxima { get; set; }

    public Guid CursoId { get; set; }
    public Curso? Curso { get; set; }

    public Guid InstrutorId { get; set; }
    public Instrutor? Instrutor { get; set; }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O nome ou código da turma é obrigatório.");

        if (CursoId == Guid.Empty)
            erros.Add("A turma deve possuir exatamente um curso.");

        if (InstrutorId == Guid.Empty)
            erros.Add("A turma deve possuir exatamente um instrutor.");

        if (DataTermino <= DataInicio)
            erros.Add("A data de término deve ser posterior à data de início.");

        if (CapacidadeMaxima <= 0)
            erros.Add("A capacidade máxima de alunos deve ser maior que zero.");

        return erros;
    }

    public override void Atualizar(Turma entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        DataInicio = entidadeAtualizada.DataInicio;
        DataTermino = entidadeAtualizada.DataTermino;
        CapacidadeMaxima = entidadeAtualizada.CapacidadeMaxima;
        CursoId = entidadeAtualizada.CursoId;
        InstrutorId = entidadeAtualizada.InstrutorId;
    }
}
