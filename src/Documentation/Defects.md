# Defects

- [Defects](#defects)
  - [PPMCloudEvent allows for NULL data, but the CloudEvent kafka conversion does not:](#ppmcloudevent-allows-for-null-data-but-the-cloudevent-kafka-conversion-does-not)

## PPMCloudEvent allows for NULL data, but the CloudEvent kafka conversion does not:

```bash
System.ArgumentException: Only CloudEvents with data can be converted to Kafka messages (Parameter 'cloudEvent')
   at CloudNative.CloudEvents.Core.Validation.CheckArgument(Boolean condition, String paramName, String message)
   at CloudNative.CloudEvents.Kafka.KafkaExtensions.ToKafkaMessage(CloudEvent cloudEvent, ContentMode contentMode, CloudEventFormatter formatter)
   at PPM.Eventing.Kafka.Producer.KafkaAsyncProducer.PublishAsync(PpmCloudEvent cloudEvent, String topic, CancellationToken token)
```

Either change PpmCloudEvent to disallow nulls, or set it to an empty document when data is null. (I think there are valid use cases for there to not be data... the source/type could be enough for simple events)

