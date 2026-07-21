using System.Security.Claims;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EscolaDeCursos.Infra.Compartilhado.Identidade;

// Nomes das claims customizadas usadas em toda a aplicação para aplicar
// a segregação de dados sem precisar consultar o banco a cada requisição.
public static class ClaimsUsuario
{
    public const string AlunoId = "EscolaDeCursos:AlunoId";
    public const string InstrutorId = "EscolaDeCursos:InstrutorId";
    public const string MfaConfigurado = "EscolaDeCursos:MfaConfigurado";
}

// Ao autenticar, adiciona ao ClaimsPrincipal o Id do Aluno/Instrutor vinculado
// ao usuário logado, além do perfil (role). Essas claims viajam dentro do
// cookie de autenticação e são a base da segregação de dados entre usuários.
public class UsuarioClaimsPrincipalFactory(
    UserManager<Usuario> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IOptions<IdentityOptions> options)
    : UserClaimsPrincipalFactory<Usuario, IdentityRole<Guid>>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
    {
        ClaimsIdentity identidade = await base.GenerateClaimsAsync(user);

        identidade.AddClaim(new Claim(ClaimTypes.GivenName, user.Nome));

        if (user.AlunoId is not null)
            identidade.AddClaim(new Claim(ClaimsUsuario.AlunoId, user.AlunoId.Value.ToString()));

        if (user.InstrutorId is not null)
            identidade.AddClaim(new Claim(ClaimsUsuario.InstrutorId, user.InstrutorId.Value.ToString()));

        // Claim usada pelo filtro global que obriga a configuração do MFA
        // (aplicativo autenticador) antes de liberar o acesso ao restante
        // da aplicação. É recalculada a cada novo login/RefreshSignInAsync.
        identidade.AddClaim(new Claim(ClaimsUsuario.MfaConfigurado, user.TwoFactorEnabled.ToString()));

        return identidade;
    }
}
