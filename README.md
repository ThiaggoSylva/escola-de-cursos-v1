# Escola de Cursos

## Projeto

Desenvolvido durante o curso Fullstack da [Academia do Programador](https://www.academiadoprogramador.net) 2026

Sistema de gestão de uma escola de cursos: categorias, cursos, módulos (aulas), instrutores, alunos, turmas e matrículas — conforme a especificação de requisitos e o protótipo de telas/ERD do projeto.

## Arquitetura

Solução em camadas (.NET 10), organizada por módulo dentro de cada camada (`Modulos/Modulo<Entidade>`):

- **EscolaDeCursos.Dominio** — entidades (`EntidadeBase<T>`), regras de validação (`Validar()`), contratos de repositório (`IRepositorio<T>` + interfaces específicas por módulo).
- **EscolaDeCursos.Aplicacao** — serviços de caso de uso (`ServicoBase<T>`), regras de negócio (unicidade, vínculos entre módulos, etc.), retorno via **FluentResults** (`Result` / `Result<T>`).
- **EscolaDeCursos.Infra** — **Entity Framework Core** (SQL Server) como ORM: `EscolaDeCursosDbContext`, mapeamentos Fluent API (`IEntityTypeConfiguration<T>`) e implementação dos repositórios (`RepositorioBase<T>`); logging com **Serilog** (console, arquivo e **New Relic Logs** via `Serilog.Sinks.NewRelic.Logs`).
- **EscolaDeCursos.WebApp** — ASP.NET Core MVC, Controllers/ViewModels/Views por módulo, **AutoMapper** para conversão Entidade ⇄ ViewModel, Bootstrap 5.

### Módulos implementados

| Módulo | Entidade | Regras principais |
| --- | --- | --- |
| Categorias | `Categoria` | título único; não exclui com cursos vinculados |
| Cursos | `Curso` | título único por categoria; carga horária > 0; exige categoria e nível |
| Módulos (aulas) | `Aula` | ordem única dentro do curso; duração > 0 |
| Instrutores | `Instrutor` | e-mail e telefone únicos; não exclui vinculado a turmas |
| Alunos | `Aluno` | CPF, e-mail e telefone únicos; não exclui com matrículas |
| Turmas | `Turma` | data término > data início; capacidade > 0; não exclui com matrículas |
| Matrículas | `Matricula` | sem duplicidade aluno/turma; respeita capacidade máxima; data ≤ início da turma; matrícula concluída não é excluída |

Relações 1:N: `Categoria → Curso`, `Curso → Aula`, `Curso → Turma`, `Instrutor → Turma`, `Turma → Matricula`, `Aluno → Matricula`.

## Como utilizar

1. Clone o repositório (branch `v1`) ou baixe o código-fonte.
2. Restaure as dependências:

   ```bash
   dotnet restore
   ```

3. Configure a chave de licença do New Relic (obrigatória — o `SerilogFactory` lança exceção sem ela):

   ```bash
   dotnet user-secrets set "Infra:NewRelic:LicenseKey" "SUA_LICENSE_KEY" --project EscolaDeCursos.WebApp
   ```

4. Ajuste a connection string `SqlServerEF` em `appsettings.Development.json` se necessário (usa LocalDB por padrão).
5. Gere e aplique as migrations do Entity Framework Core (ainda não geradas neste commit):

   ```bash
   dotnet tool install --global dotnet-ef   # se ainda não tiver o CLI do EF
   dotnet ef migrations add InicialCreate --project EscolaDeCursos.Infra --startup-project EscolaDeCursos.WebApp
   dotnet ef database update --project EscolaDeCursos.Infra --startup-project EscolaDeCursos.WebApp
   ```

6. Execute o projeto:

   ```bash
   dotnet run --project EscolaDeCursos.WebApp
   ```

## Requisitos

- .NET 10.0 SDK
- SQL Server / LocalDB
- Conta New Relic (para o sink de logs `Serilog.Sinks.NewRelic.Logs`)
