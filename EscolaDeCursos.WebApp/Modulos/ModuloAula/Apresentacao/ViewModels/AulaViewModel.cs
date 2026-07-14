using System.ComponentModel.DataAnnotations;

namespace EscolaDeCursos.WebApp.Modulos.ModuloAula.Apresentacao.ViewModels;

public class AulaFormularioViewModel
{
    public Guid? Id { get; set; }

    [Display(Name = "Título")]
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve ter entre 2 e 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Ordem")]
    [Range(1, int.MaxValue, ErrorMessage = "A ordem deve ser maior que zero.")]
    public int Ordem { get; set; }

    [Display(Name = "Duração (h)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "A duração deve ser maior que zero.")]
    public double Duracao { get; set; }

    public Guid CursoId { get; set; }
}

public class AulaListarViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public double Duracao { get; set; }
}

public class AulaExcluirViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public Guid CursoId { get; set; }
}
