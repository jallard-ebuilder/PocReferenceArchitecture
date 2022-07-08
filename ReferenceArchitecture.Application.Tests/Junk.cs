using FluentAssertions;
using PPM.Eventing.Core;

namespace ReferenceArchitecture.Application.Tests;

public class Junk
{
    [Fact]
    public void CanSetDataToNull()
    {
        var evt = new PpmCloudEvent
        {
            Data = null,
            Source = new Uri("https://blah"),
        };

        evt.Data.Should().BeNull();
        evt.CloudEvent.Data.Should().BeNull();
    }
}