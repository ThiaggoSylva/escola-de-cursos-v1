using EscolaDeCursos.Dominio.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace EscolaDeCursos.Infra.Compartilhado.Orm;

// Implementação genérica de IRepositorio<T> usando o Entity Framework Core
// como ORM de acesso ao banco de dados (EscolaDeCursosDbContext).
public abstract class RepositorioBase<T>(EscolaDeCursosDbContext contexto) : IRepositorio<T>
    where T : EntidadeBase<T>
{
    protected readonly EscolaDeCursosDbContext Contexto = contexto;
    protected readonly DbSet<T> DbSet = contexto.Set<T>();

    public virtual void Cadastrar(T entidade)
    {
        DbSet.Add(entidade);
        Contexto.SaveChanges();
    }

    public virtual bool Editar(Guid idSelecionado, T entidadeAtualizada)
    {
        T? entidade = DbSet.Find(idSelecionado);

        if (entidade is null)
            return false;

        entidade.Atualizar(entidadeAtualizada);

        Contexto.SaveChanges();

        return true;
    }

    public virtual bool Excluir(Guid idSelecionado)
    {
        T? entidade = DbSet.Find(idSelecionado);

        if (entidade is null)
            return false;

        DbSet.Remove(entidade);

        Contexto.SaveChanges();

        return true;
    }

    public virtual T? SelecionarPorId(Guid idSelecionado)
    {
        return DbSet.AsNoTracking().FirstOrDefault(e => e.Id == idSelecionado);
    }

    public virtual List<T> SelecionarTodos()
    {
        return [.. DbSet.AsNoTracking()];
    }

    public virtual List<T> Filtrar(Func<T, bool> filtro)
    {
        // O contrato do repositório (IRepositorio<T>) trabalha com Func<T, bool>,
        // por isso a filtragem é resolvida em memória sobre o resultado da consulta.
        return [.. DbSet.AsNoTracking().AsEnumerable().Where(filtro)];
    }
}
