using System.Threading.Channels;

namespace PetFamily.Infrastructure.MessageQueues;

public class FilesCleanerMessageQueue
{
    private readonly Channel<string[]> _channel = Channel.CreateUnbounded<string[]>();

    public async Task WriteAsync(string[] paths, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(paths, cancellationToken);
    }

    public async Task ReadAsync(string[] paths, CancellationToken cancellationToken)
    {
        await _channel.Reader.ReadAsync(cancellationToken);
    }
}