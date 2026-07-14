using System.ComponentModel.DataAnnotations;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;

namespace EscolaDeCursos.WebApp.Modulos.ModuloCurso.Apresentacao.ViewModels;

public class CursoFormularioViewModel
{
    public Guid? Id { get; set; }

    [Display(Name = "Título")]
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve ter entre 2 e 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Descrição")]
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Carga Horária (h)")]
    [Range(1, int.MaxValue, ErrorMessage = "A carga horária deve ser maior que zero.")]
    public int CargaHoraria { get; set; }

    [Display(Name = "Nível")]
    public NivelCurso Nivel { get; set; }

    [Display(Name = "Categoria")]
    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public Guid CategoriaId { get; set; }
}

public class CursoListarViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string CategoriaTitulo { get; set; } = string.Empty;
    public int CargaHoraria { get; set; }
    public NivelCurso Nivel { get; set; }
}

public class CursoExcluirViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
}
