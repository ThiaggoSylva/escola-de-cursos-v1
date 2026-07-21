using EscolaDeCursos.WebApp.Compartilhado.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EscolaDeCursos.WebApp.Compartilhado.Filters;

// A aplicação exige autenticação multifator (TOTP) de TODOS os usuários.
// Este filtro roda em toda requisição autenticada e, se o usuário ainda não
// tiver configurado o autenticador, força o redirecionamento para a tela de
// configuração antes de liberar qualquer outra tela da aplicação.
public class ExigirMfaFilter : IAsyncActionFilter
{
    // Ações do próprio módulo de Conta que precisam ficar acessíveis mesmo
    // sem o MFA configurado (login, cadastro, configuração do MFA, logout).
    private static readonly string[] ControllersIsentos = ["Conta"];

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool autenticado = context.HttpContext.User.Identity?.IsAuthenticated ?? false;

        string controller = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;

        bool isento = ControllersIsentos.Contains(controller, StringComparer.OrdinalIgnoreCase);

        if (autenticado && !isento && !context.HttpContext.User.MfaConfigurado())
        {
            context.Result = new RedirectToActionResult("ConfigurarAutenticador", "Conta", null);
            return;
        }

        await next();
    }
}
