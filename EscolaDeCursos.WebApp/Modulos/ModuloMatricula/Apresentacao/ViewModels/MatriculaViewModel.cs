using System.ComponentModel.DataAnnotations;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;

namespace EscolaDeCursos.WebApp.Modulos.ModuloMatricula.Apresentacao.ViewModels;

public class MatriculaFormularioViewModel
{
    [Display(Name = "Aluno")]
    [Required(ErrorMessage = "O aluno é obrigatório.")]
    public Guid AlunoId { get; set; }

    [Display(Name = "Turma")]
    [Required(ErrorMessage = "A turma é obrigatória.")]
    public Guid TurmaId { get; set; }

    [Display(Name = "Data da Matrícula")]
    [Required(ErrorMessage = "A data da matrícula é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataMatricula { get; set; } = DateTime.Today;
}

public class MatriculaListarViewModel
{
    public Guid Id { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public string TurmaNome { get; set; } = string.Empty;
    public string CursoTitulo { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public SituacaoMatricula Situacao { get; set; }
}

public class MatriculaCancelarViewModel
{
    public Guid Id { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public string TurmaNome { get; set; } = string.Empty;
}
