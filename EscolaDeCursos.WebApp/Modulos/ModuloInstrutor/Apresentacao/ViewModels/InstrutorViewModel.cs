using System.ComponentModel.DataAnnotations;

namespace EscolaDeCursos.WebApp.Modulos.ModuloInstrutor.Apresentacao.ViewModels;

public class InstrutorFormularioViewModel
{
    public Guid? Id { get; set; }

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Telefone")]
    [Required(ErrorMessage = "O telefone é obrigatório.")]
    public string Telefone { get; set; } = string.Empty;

    [Display(Name = "Especialidade")]
    [Required(ErrorMessage = "A especialidade é obrigatória.")]
    public string Especialidade { get; set; } = string.Empty;
}

public class InstrutorListarViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
}

public class InstrutorExcluirViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
