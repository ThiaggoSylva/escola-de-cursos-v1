using EscolaDeCursos.Dominio.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAula;

// Corresponde ao "Módulo" (aula) da especificação do curso.
// Renomeado para "Aula" para não conflitar com o termo "Módulo" usado
// pela arquitetura do projeto (Modulos/ModuloX).
public class Aula : EntidadeBase<Aula>
{
    public string Titulo { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public double Duracao { get; set; }

    public Guid CursoId { get; set; }
    public Curso? Curso { get; set; }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O título da aula é obrigatório.");
        else if (Titulo.Length is < 2 or > 100)
            erros.Add("O título da aula deve ter entre 2 e 100 caracteres.");

        if (Duracao <= 0)
            erros.Add("A duração da aula deve ser maior que zero.");

        if (CursoId == Guid.Empty)
            erros.Add("A aula deve pertencer a um curso.");

        return erros;
    }

    public override void Atualizar(Aula entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        Ordem = entidadeAtualizada.Ordem;
        Duracao = entidadeAtualizada.Duracao;
        CursoId = entidadeAtualizada.CursoId;
    }
}
