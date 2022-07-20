# TODO

- [TODO](#todo)

- when kafka config is missing, responds with "map" error
- `enable.auto.commit` must a string of `false`... why doesn't boolean work?
- health endpoint not in swagger
  - https://www.codenesium.com/blog/posts/how-to-add-health-checks-asp-net-core-with-swagger-support/
- need try/catch around service start (for logging). sample code shows `ServiceUtility.RunSafe`, but I couldn't find that... revisit/evaluate
- throws exception if kafka topic doesn't exist, but keeps running. add configuration to make it optionally crash.
  - the exception is coming from consumer. we'll see that during startup.
  - but, producer won't throw an exception until something actually produces, so that'll be at runtime.
- duplicate event types on handlers result in a DI exception. it should error, but it needs to be a better exception
- PpmCloudEvent 
- PPM Cloud Event
  - is in `PPM.Eventing.Core`, which contains the Dispatcher and Publisher. It's not appropriate for core to reference it, but it must because that's where `PpmCloudEvent` is. We need to move PpmCloudEvent to a different package.
  - should we default the id to `Guid.NewGuid().ToString()`?
  - should we default the time to `DateTimeOffset.UtcNow`? (note that the ppm cloud event doc says `.Now`, so need to settle that)
  - not serializable - should it be? See if the underlying CloudEvent is
  - time is nullable - make it not nullable
    - review other properties for the same
