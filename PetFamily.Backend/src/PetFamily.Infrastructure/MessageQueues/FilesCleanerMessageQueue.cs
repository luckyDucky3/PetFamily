using System.Threading.Channels;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Infrastructure.MessageQueues;

public class FilesCleanerMessageQueue : IMessageQueue<IEnumerable<FileInfo>>
{
    private readonly Channel<IEnumerable<FileInfo>> _channel = Channel.CreateUnbounded<IEnumerable<FileInfo>>();

    public async Task WriteAsync(IEnumerable<FileInfo> paths, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(paths, cancellationToken);
    }

    public async Task<IEnumerable<FileInfo>> ReadAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }
}

public interface IMessageQueue<TMessage>
{
    public Task WriteAsync(TMessage paths, CancellationToken cancellationToken);
    public Task<TMessage> ReadAsync(CancellationToken cancellationToken);
}