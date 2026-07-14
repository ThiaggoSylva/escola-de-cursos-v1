using EscolaDeCursos.Aplicacao.Modulos.ModuloAluno;
using EscolaDeCursos.Aplicacao.Modulos.ModuloAula;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCategoria;
using EscolaDeCursos.Aplicacao.Modulos.ModuloCurso;
using EscolaDeCursos.Aplicacao.Modulos.ModuloInstrutor;
using EscolaDeCursos.Aplicacao.Modulos.ModuloMatricula;
using EscolaDeCursos.Aplicacao.Modulos.ModuloTurma;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EscolaDeCursos.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ICategoriaServico, CategoriaServico>();
        services.AddScoped<ICursoServico, CursoServico>();
        services.AddScoped<IAulaServico, AulaServico>();
        services.AddScoped<IInstrutorServico, InstrutorServico>();
        services.AddScoped<IAlunoServico, AlunoServico>();
        services.AddScoped<ITurmaServico, TurmaServico>();
        services.AddScoped<IMatriculaServico, MatriculaServico>();
    }
}
