using System.Text.RegularExpressions;
using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloAluno;

public partial class Aluno : EntidadeBase<Aluno>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O nome do aluno é obrigatório.");
        else if (Nome.Length is < 2 or > 100)
            erros.Add("O nome do aluno deve ter entre 2 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(Email))
            erros.Add("O e-mail do aluno é obrigatório.");
        else if (!RegexEmail().IsMatch(Email))
            erros.Add("O e-mail informado não é válido.");

        if (string.IsNullOrWhiteSpace(Telefone))
            erros.Add("O telefone do aluno é obrigatório.");

        if (string.IsNullOrWhiteSpace(Cpf))
            erros.Add("O CPF do aluno é obrigatório.");

        return erros;
    }

    public override void Atualizar(Aluno entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        Email = entidadeAtualizada.Email;
        Telefone = entidadeAtualizada.Telefone;
        Cpf = entidadeAtualizada.Cpf;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex RegexEmail();
}
