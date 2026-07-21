using EscolaDeCursos.Dominio.Modulos.ModuloAluno;
using EscolaDeCursos.Dominio.Modulos.ModuloAula;
using EscolaDeCursos.Dominio.Modulos.ModuloCategoria;
using EscolaDeCursos.Dominio.Modulos.ModuloCurso;
using EscolaDeCursos.Dominio.Modulos.ModuloInstrutor;
using EscolaDeCursos.Dominio.Modulos.ModuloMatricula;
using EscolaDeCursos.Dominio.Modulos.ModuloTurma;
using EscolaDeCursos.Dominio.Modulos.ModuloUsuario;
using EscolaDeCursos.Infra.Comartilhado.Logging;
using EscolaDeCursos.Infra.Compartilhado.Identidade;
using EscolaDeCursos.Infra.Compartilhado.Orm;
using EscolaDeCursos.Infra.Modulos.ModuloAluno;
using EscolaDeCursos.Infra.Modulos.ModuloAula;
using EscolaDeCursos.Infra.Modulos.ModuloCategoria;
using EscolaDeCursos.Infra.Modulos.ModuloCurso;
using EscolaDeCursos.Infra.Modulos.ModuloInstrutor;
using EscolaDeCursos.Infra.Modulos.ModuloMatricula;
using EscolaDeCursos.Infra.Modulos.ModuloTurma;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EscolaDeCursos.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(
        this IServiceCollection services,
        IConfiguration configuration,
        ILoggingBuilder logging
    )
    {
        // Injeta logs do Serilog
        Log.Logger = SerilogFactory.Create(configuration);

        logging.ClearProviders();

        services.AddSerilog(Log.Logger);

        // Injeta o DbContext do EF
        services.AddDbContext<EscolaDeCursosDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("SqlServerEF");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"A connection string \"SqlServerEF\" não foi encontrada."
                );
            }

            options.UseSqlServer(connectionString, opt =>
            {
                opt.EnableRetryOnFailure(3);
            });
        });

        // Injeta os repositórios de cada módulo
        services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
        services.AddScoped<ICursoRepositorio, CursoRepositorio>();
        services.AddScoped<IAulaRepositorio, AulaRepositorio>();
        services.AddScoped<IInstrutorRepositorio, InstrutorRepositorio>();
        services.AddScoped<IAlunoRepositorio, AlunoRepositorio>();
        services.AddScoped<ITurmaRepositorio, TurmaRepositorio>();
        services.AddScoped<IMatriculaRepositorio, MatriculaRepositorio>();

        // Injeta o ASP.NET Core Identity (autenticação, roles e MFA via
        // aplicativo autenticador - TOTP). Somente usuários com cadastro
        // já existente conseguem autenticar; não há acesso anônimo à
        // aplicação além das telas de Login/Cadastro (ver ExigirAutenticacaoFilter).
        services
            .AddIdentity<Usuario, IdentityRole<Guid>>(options =>
            {
                // Política de senha
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;

                // Bloqueio de conta por tentativas inválidas de login
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;

                // E-mail é o identificador único de login
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<EscolaDeCursosDbContext>()
            .AddClaimsPrincipalFactory<UsuarioClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        // Configura o cookie de autenticação: usuários não autenticados que
        // tentarem acessar qualquer tela da aplicação são redirecionados
        // para o Login; usuários autenticados sem permissão, para AcessoNegado.
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Conta/Login";
            options.LogoutPath = "/Conta/Sair";
            options.AccessDeniedPath = "/Conta/AcessoNegado";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(4);
        });
    }
}
