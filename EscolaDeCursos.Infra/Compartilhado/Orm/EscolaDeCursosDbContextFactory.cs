using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EscolaDeCursos.Infra.Compartilhado.Orm;

public class EscolaDeCursosDbContextFactory
    : IDesignTimeDbContextFactory<EscolaDeCursosDbContext>
{
    public EscolaDeCursosDbContext CreateDbContext(string[] args)
    {
        string? connectionString =
            Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "AZURE_SQL_CONNECTION_STRING não foi encontrada.");

        DbContextOptionsBuilder<EscolaDeCursosDbContext> optionsBuilder = new();

        optionsBuilder.UseSqlServer(
    connectionString,
    options =>
    {
        options.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    });

        return new EscolaDeCursosDbContext(optionsBuilder.Options);
    }
}
