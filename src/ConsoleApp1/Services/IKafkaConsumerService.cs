using Confluent.Kafka;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaBatchConsumerApp.Services
{
    public interface IKafkaConsumerService
    {
        Task ConsumeBatchAsync(string topic, string groupId, int batchSize, CancellationToken cancellationToken);
    }
}