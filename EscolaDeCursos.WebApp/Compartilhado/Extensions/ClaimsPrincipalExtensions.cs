using System.Security.Claims;
using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Infra.Compartilhado.Identidade;

namespace EscolaDeCursos.WebApp.Compartilhado.Extensions;

// Ponto único de leitura das claims que sustentam a segregação de dados.
// Nenhum controller deve confiar em valores vindos do cliente (query string,
// formulário) para decidir "de quem" são os dados: o Id do Aluno/Instrutor
// logado vem sempre do ClaimsPrincipal, montado no momento do login.
public static class ClaimsPrincipalExtensions
{
    public static bool EhAdministrador(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(nameof(Perfil.Administrador));

    public static bool EhInstrutor(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(nameof(Perfil.Instrutor));

    public static bool EhAluno(this ClaimsPrincipal usuario) =>
        usuario.IsInRole(nameof(Perfil.Aluno));

    public static Guid? ObterAlunoId(this ClaimsPrincipal usuario)
    {
        string? valor = usuario.FindFirstValue(ClaimsUsuario.AlunoId);
        return Guid.TryParse(valor, out Guid alunoId) ? alunoId : null;
    }

    public static Guid? ObterInstrutorId(this ClaimsPrincipal usuario)
    {
        string? valor = usuario.FindFirstValue(ClaimsUsuario.InstrutorId);
        return Guid.TryParse(valor, out Guid instrutorId) ? instrutorId : null;
    }

    public static bool MfaConfigurado(this ClaimsPrincipal usuario) =>
        bool.TryParse(usuario.FindFirstValue(ClaimsUsuario.MfaConfigurado), out bool configurado)
        && configurado;
}
