using System.ComponentModel.DataAnnotations;

namespace EscolaDeCursos.WebApp.Modulos.ModuloTurma.Apresentacao.ViewModels;

public class TurmaFormularioViewModel
{
    public Guid? Id { get; set; }

    [Display(Name = "Nome ou Código da Turma")]
    [Required(ErrorMessage = "O nome ou código da turma é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Curso")]
    [Required(ErrorMessage = "O curso é obrigatório.")]
    public Guid CursoId { get; set; }

    [Display(Name = "Instrutor")]
    [Required(ErrorMessage = "O instrutor é obrigatório.")]
    public Guid InstrutorId { get; set; }

    [Display(Name = "Data de Início")]
    [Required(ErrorMessage = "A data de início é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataInicio { get; set; } = DateTime.Today;

    [Display(Name = "Data de Término")]
    [Required(ErrorMessage = "A data de término é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataTermino { get; set; } = DateTime.Today.AddMonths(4);

    [Display(Name = "Capacidade Máxima de Alunos")]
    [Range(1, int.MaxValue, ErrorMessage = "A capacidade máxima deve ser maior que zero.")]
    public int CapacidadeMaxima { get; set; }
}

public class TurmaListarViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CursoTitulo { get; set; } = string.Empty;
    public string InstrutorNome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public int CapacidadeMaxima { get; set; }
    public int MatriculasAtivas { get; set; }
}

public class TurmaExcluirViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
