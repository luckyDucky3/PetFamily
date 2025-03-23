using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _writeDbContext;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("A transaction has already been started.");

        _currentTransaction = await _writeDbContext.Database.BeginTransactionAsync(cancellationToken);

        return _currentTransaction;
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _writeDbContext.DisposeAsync();
    }
}