using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using PPM.Eventing.Core.Dispatcher;
using PPM.Eventing.Core.Publisher;
using PPM.Eventing.Dispatcher.Hosting;
using ReferenceArchitecture.Core;

namespace ReferenceArchitecture.Application.EventHandlers;

// DEMO: unnecessary - used by the
// program's call to AddAllEventHandlersFromAssembly to get a reference
// to this assembly. any of the single event handlers can be used instead,
// and should. For demo purposes, this is used so that it doesn't look like
// only a single event handler is being added.
// IE:    AddAllEventHandlersFromAssembly<FileCreatedEventHandler> would work
// just fine and is the proper way to do it. But, a new person might think
// that it adds only the one handler, and not all of the handlers in that
// class's assembly.
public class AllDemoEventHandlers{}


/// <summary>
/// A file was created.
/// In FMS: The fms endpoint publishes an event when the CreateFile endpoint is called.
/// This prints a message to STDOUT.
/// In FMS, nothing processes the event (yet).
/// </summary>
[EventHandler("FileCreated")]
public class FileCreatedLoggerEventHandler : IEventHandler
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
        var m = JsonSerializer.SerializeToElement(new { });
        var completed = EventUtility.CreateFromObject("FilePostProcessingCompleted", m);
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
public class PostProcessingCompletedLoggingEventHandler : IEventHandler
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
        // calculate the difference between when the message was published,
        // and when this method received it
        var now = DateTimeOffset.UtcNow;
        var diff = now.Subtract(evt.GetEvent().Time!.Value).TotalMilliseconds;
        var log = new StringBuilder();
        log.AppendLine("\n\n----------------------------------------------------------");
        log.AppendLine("           Event received: " + evt.EventType);
        log.AppendLine("           Time between PUBLISH and CONSUME: " + diff + "ms");
        log.AppendLine("----------------------------------------------------------\n\n");
        Console.WriteLine(log);
    }
}