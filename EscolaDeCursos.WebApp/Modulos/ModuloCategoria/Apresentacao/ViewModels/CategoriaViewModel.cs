using System.ComponentModel.DataAnnotations;

namespace EscolaDeCursos.WebApp.Modulos.ModuloCategoria.Apresentacao.ViewModels;

public class CategoriaFormularioViewModel
{
    public Guid? Id { get; set; }

    [Display(Name = "Título")]
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve ter entre 2 e 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
}

public class CategoriaListarViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int QuantidadeCursos { get; set; }
}

public class CategoriaExcluirViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
}
