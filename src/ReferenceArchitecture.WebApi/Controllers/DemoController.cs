using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using PPM.Eventing.Core.Publisher;
using ReferenceArchitecture.Core;

namespace ReferenceArchitecture.Presentation.Controllers;

[ApiController]
[Route("/api/v1/demo/stuff")]
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
    public async Task<PublishResult> SendAMessage(TestEventTypes testEventType, string message)
    {
        var m = new { message };
        var evt = EventUtility.Create(testEventType.ToString());
        var result = await _publisher.PublishAsync(evt, "demo-topic-1");
        return result;
    }
}