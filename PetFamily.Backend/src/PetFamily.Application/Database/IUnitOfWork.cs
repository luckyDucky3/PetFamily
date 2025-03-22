using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace PetFamily.Application.Database;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);
    Task SaveChanges(CancellationToken cancellationToken = default);
}