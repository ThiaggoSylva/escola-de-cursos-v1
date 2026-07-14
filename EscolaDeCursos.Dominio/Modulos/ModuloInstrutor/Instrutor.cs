using System.Text.RegularExpressions;
using EscolaDeCursos.Dominio.Compartilhado;

namespace EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;

public partial class Instrutor : EntidadeBase<Instrutor>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O nome do instrutor é obrigatório.");
        else if (Nome.Length is < 2 or > 100)
            erros.Add("O nome do instrutor deve ter entre 2 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(Email))
            erros.Add("O e-mail do instrutor é obrigatório.");
        else if (!RegexEmail().IsMatch(Email))
            erros.Add("O e-mail informado não é válido.");

        if (string.IsNullOrWhiteSpace(Telefone))
            erros.Add("O telefone do instrutor é obrigatório.");

        if (string.IsNullOrWhiteSpace(Especialidade))
            erros.Add("A especialidade do instrutor é obrigatória.");

        return erros;
    }

    public override void Atualizar(Instrutor entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        Email = entidadeAtualizada.Email;
        Telefone = entidadeAtualizada.Telefone;
        Especialidade = entidadeAtualizada.Especialidade;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex RegexEmail();
}
