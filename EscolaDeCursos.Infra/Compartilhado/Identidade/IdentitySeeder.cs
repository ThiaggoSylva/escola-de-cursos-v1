using EscolaDeCursos.Dominio.Compartilhado.Identidade;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EscolaDeCursos.Infra.Compartilhado.Identidade;

// Garante que as roles do sistema existam e que exista pelo menos um
// usuário Administrador, para que a aplicação não fique "sem porta de
// entrada" na primeira execução (já que o cadastro público só cria Alunos).
public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using IServiceScope scope = services.CreateScope();

        RoleManager<IdentityRole<Guid>> roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        UserManager<Usuario> userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();

        foreach (Perfil perfil in Enum.GetValues<Perfil>())
        {
            string nomeRole = perfil.ToString();

            if (!await roleManager.RoleExistsAsync(nomeRole))
                await roleManager.CreateAsync(new IdentityRole<Guid>(nomeRole));
        }

        const string emailAdmin = "admin@escoladecursos.com";

        if (await userManager.FindByEmailAsync(emailAdmin) is not null)
            return;

        Usuario administrador = new()
        {
            UserName = emailAdmin,
            Email = emailAdmin,
            EmailConfirmed = true,
            Nome = "Administrador",
            Perfil = Perfil.Administrador
        };

        // Senha temporária: o administrador deve alterá-la e configurar o
        // MFA (obrigatório em toda a aplicação) no primeiro acesso.
        IdentityResult resultado = await userManager.CreateAsync(administrador, "Admin@123456");

        if (resultado.Succeeded)
            await userManager.AddToRoleAsync(administrador, nameof(Perfil.Administrador));
    }
}
