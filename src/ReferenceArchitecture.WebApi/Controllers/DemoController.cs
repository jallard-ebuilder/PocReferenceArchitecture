using System.Text.Json;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PPM.Eventing.Core.Publisher;
using ReferenceArchitecture.Core;

namespace ReferenceArchitecture.Presentation.Controllers;

[ApiController]
[Route("/api/v1/demo/events")]
public class DemoController : Controller
{
    private readonly IAsyncEventPublisher _publisher;
    
    
    // GET
    public DemoController(IAsyncEventPublisher publisher) =>
        _publisher = Guard.Against.Null(publisher);

    [HttpGet]
    public IActionResult Index()
    {
        return Json(new
        {
            hi = "there"
        });
    }

    public enum TestEventTypes
    {
        FileUploaded,
        FileCreated
    }

    /// <summary>
    /// Publish an event.
    /// A real application wouldn't expose this functionality; this is a demo so that
    /// you can publish CloudEventMessages. Alternatively, you may use
    /// tooling such as `kafka-producer`, or your own test code.
    /// This is dev/test/demo convenience only.
    /// </summary>
    /// <param name="testEventType"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> SendAMessage(TestEventTypes testEventType, string message)
    {
        var m = JsonSerializer.SerializeToElement(new { message });
        var evt = EventUtility.Create(testEventType.ToString(), m);
        var result = await _publisher.PublishAsync(evt, "demo-topic-1");
        return result.Success;
    }
}