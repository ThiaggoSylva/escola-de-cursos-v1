using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EscolaDeCursos.Infra.Compartilhado.Orm;

public class EscolaDeCursosDbContextFactory : IDesignTimeDbContextFactory<EscolaDeCursosDbContext>
{
    public EscolaDeCursosDbContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? "Server=(localdb)\\mssqllocaldb;Database=EscolaDeCursos;Trusted_Connection=True;TrustServerCertificate=True;";

        DbContextOptionsBuilder<EscolaDeCursosDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(connectionString);

        return new EscolaDeCursosDbContext(optionsBuilder.Options);
    }
}