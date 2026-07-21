using EscolaDeCursos.WebApp.Compartilhado.Filters;
using EscolaDeCursos.WebApp.Compartilhado.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace EscolaDeCursos.WebApp.Compartilhado;

public static class InjecaoDependencia
{
    public static void AddPresentationConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllersWithViews(options =>
        {
            // Por padrão TODA a aplicação exige usuário autenticado; somente
            // quem já possui cadastro consegue navegar em qualquer tela.
            // As telas de Login/Cadastro usam [AllowAnonymous] explicitamente.
            AuthorizationPolicy politicaPadrao = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.Filters.Add(new AuthorizeFilter(politicaPadrao));

            // Obriga a configuração do MFA logo após o primeiro login.
            options.Filters.Add<ExigirMfaFilter>();
        }).AddRazorOptions(options =>
        {
            // Reseta a configuração padrão do MVC
            options.ViewLocationFormats.Clear();

            // Localização das Views dos módulos: Modulos/ModuloCaixa/Apresentacao/Views/Listar.cshtml
            options.ViewLocationFormats.Add("/Modulos/Modulo{1}/Apresentacao/Views/{0}.cshtml");

            // Localização das Views compartilhadas: /Compartilhado/Apresentacao/Views/_Layout.cshtml
            options.ViewLocationFormats.Add("/Compartilhado/Apresentacao/Views/{0}.cshtml");
        });

        services.AddAutoMapper(mapperConfig =>
        {
            AutoMapperOptions autoMapperOptions = configuration
                .GetSection(AutoMapperOptions.SectionName)
                .Get<AutoMapperOptions>() ?? new AutoMapperOptions();

            string? licenseKey = autoMapperOptions.LicenseKey;

            if (!string.IsNullOrWhiteSpace(licenseKey))
                mapperConfig.LicenseKey = licenseKey;

            mapperConfig.AddMaps(typeof(Program));
        });
    }
}
