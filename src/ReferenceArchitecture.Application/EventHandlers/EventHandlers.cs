using Ardalis.GuardClauses;
using PPM.Eventing.Core.Dispatcher;
using PPM.Eventing.Core.Publisher;
using PPM.Eventing.Dispatcher.Hosting;
using ReferenceArchitecture.Core;

namespace ReferenceArchitecture.Application.EventHandlers;

/// <summary>
/// A file was created.
/// In FMS: The fms endpoint publishes an event when the CreateFile endpoint is called.
/// This prints a message to STDOUT.
/// In FMS, nothing processes the event (yet).
/// </summary>
[EventHandler("FileCreated")]
public class FileCreatedEventHandler : IEventHandler
{
    public Task HandleAsync(CloudEventContainer evt, CancellationToken cancellationToken)
    {
        Printer.Print(evt);
        return Task.CompletedTask;
    }
}

/// <summary>
/// IN FMS:
/// After a user created a file in FMS,
/// they then upload the file to S3.
/// The S3 event is converted (by lambda) to a Kafka event.
/// Then, the PostProcessing service does some work.
/// Upon completion, it publishes the FilePostProcessingCompleted event. 
/// </summary>
[EventHandler("FileUploaded")]
public class PostProcessingEventHandler : IEventHandler
{
    private readonly IAsyncEventPublisher _publisher;

    public PostProcessingEventHandler(IAsyncEventPublisher publisher)
    {
        _publisher = Guard.Against.Null(publisher);
    }

    public async Task HandleAsync(CloudEventContainer evt, CancellationToken cancellationToken)
    {
        Printer.Print(evt);
        // do some post processing
        // ...
        // ...
        // ...
        // all done!
        var completed = EventUtility.CreateFromObject("FilePostProcessingCompleted");
        await _publisher.PublishAsync(completed, "demo-topic-1", cancellationToken);
    }
}

/// <summary>
/// When post processing is complete, the FilePostProcessingCompleted event is fired
/// from the PostProcessing event handler.
/// Receive that event and print it.
/// In FMS, nothing processes the event (yet).
/// </summary>
[EventHandler("FilePostProcessingCompleted")]
public class PostProcessingCompletedEventHandler : IEventHandler
{
    public Task HandleAsync(CloudEventContainer evt, CancellationToken cancellationToken)
    {
        Printer.Print(evt);
        return Task.CompletedTask;
    }
}

public static class Printer
{
    public static void Print(CloudEventContainer evt)
    {
        Console.WriteLine("---------- Event received: " + evt.EventType);
    }
}