using EscolaDeCursos.Aplicacao;
using EscolaDeCursos.Infra;
using EscolaDeCursos.Infra.Compartilhado.Identidade;
using EscolaDeCursos.WebApp.Compartilhado;

var builder = WebApplication.CreateBuilder(args);

// Configuração do container de injeção de dependência

builder.Services.AddInfraRepositories(builder.Configuration, builder.Logging);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPresentationConfig(builder.Configuration);

var app = builder.Build();

// Garante que as roles e o usuário Administrador padrão existam
await IdentitySeeder.SeedAsync(app.Services);

// Middlewares de roteamento e segurança
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

// Execução do Servidor
app.Run();
