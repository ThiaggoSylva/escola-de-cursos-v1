using EscolaDeCursos.Dominio.Compartilhado;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;

namespace EscolaDeCursos.Dominio.Modulos.ModuloCurso;

public class Curso : EntidadeBase<Curso>
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
    public NivelCurso Nivel { get; set; }

    public Guid CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O título do curso é obrigatório.");
        else if (Titulo.Length is < 2 or > 100)
            erros.Add("O título do curso deve ter entre 2 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(Descricao))
            erros.Add("A descrição do curso é obrigatória.");

        if (CargaHoraria <= 0)
            erros.Add("A carga horária deve ser maior que zero.");

        if (CategoriaId == Guid.Empty)
            erros.Add("O curso deve possuir uma categoria.");

        return erros;
    }

    public override void Atualizar(Curso entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        Descricao = entidadeAtualizada.Descricao;
        CargaHoraria = entidadeAtualizada.CargaHoraria;
        Nivel = entidadeAtualizada.Nivel;
        CategoriaId = entidadeAtualizada.CategoriaId;
    }
}
