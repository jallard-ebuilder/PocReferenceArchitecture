using System.Text.Json;
using PPM.Eventing.Core;

namespace ReferenceArchitecture.Core;

/*
 * The Application, or Core, need to create events.
 * 
 * The event spec is here: https://github.com/e-buildernoc/development-docs/blob/main/specs/microservices/event-spec.md
 *
 * The details of that event is up to the project, particularly the data property (aka:  event payload). The spec provides no guidance
 * on the contents of the data property, except that it must be JSON or NULL.
 *
 * This demonstrates a class that creates events for you. This does not need to be reusable or configurable
 * for multiple applications. Modify it as you need to for your use cases. Or, don't use it at all.
 * One way or another, you need code that generates your events, and this is
 * one simple means of doing it.
 */

public static class EventUtility
{
    private static readonly Uri EventSource = new("https://e-builder.net/demo/reference-arch");
    /*
     * 
     */
    
    public static PpmCloudEvent Create(string eventType, JsonElement? data = null)
    {
        return new PpmCloudEvent
        {
            Type = eventType,
            Data = data,
            Id = Guid.NewGuid().ToString(),
            Source = EventSource,
            Time = DateTimeOffset.UtcNow, 
            
            // you are generating an event for something, such as a FILE, USER,
            // or any other entity. The subject is the id of that entity.
            Subject = "demo"
        };
    }

    public static PpmCloudEvent CreateFromObject(string eventType, object? data = null) =>
        data == null
            ? Create(eventType, null)
            : Create(eventType, JsonSerializer.SerializeToElement(data));
}

