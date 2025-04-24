using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaBatchConsumerApp.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(IConfiguration configuration, ILogger<KafkaConsumerService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task ConsumeBatchAsync(string topic, string groupId, int batchSize, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false // We will manually commit offsets after processing a batch
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topic);
                _logger.LogInformation($"Subscribed to topic: {topic} with group ID: {groupId}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var messages = new List<ConsumeResult<Ignore, string>>();
                    try
                    {
                        for (int i = 0; i < batchSize; i++)
                        {
                            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5)); // Adjust timeout as needed

                            if (consumeResult != null)
                            {
                                messages.Add(consumeResult);
                            }
                            else
                            {
                                // No more messages in the current poll interval
                                break;
                            }
                        }

                        if (messages.Any())
                        {
                            _logger.LogInformation($"Received a batch of {messages.Count} messages.");

                            // **Process your batch of messages here**
                            foreach (var message in messages)
                            {
                                _logger.LogInformation($"Processing message: Partition: {message.Partition}, Offset: {message.Offset}, Value: {message.Message?.Value}");
                                // Your batch processing logic for each message
                            }

                            // Manually commit the offsets for the processed batch
                            try
                            {
                                var lastMessage = messages.Last();
                                var topicPartitionOffset = new TopicPartitionOffset(
                                    lastMessage.Topic,
                                    lastMessage.Partition,
                                    lastMessage.Offset + 1 // Commit the offset of the *next* message to be consumed
                                );
                                consumer.Commit(new List<TopicPartitionOffset> { topicPartitionOffset });
                                _logger.LogInformation($"Committed offset {lastMessage.Offset + 1} for partition {lastMessage.Partition} on topic {lastMessage.Topic}.");
                            }
                            catch (KafkaException e)
                            {
                                _logger.LogError($"Error committing offsets: {e.Error.Reason}");
                                // Consider implementing retry or other error handling for commit failures
                            }
                        }
                        else
                        {
                            // No messages consumed in the poll interval
                            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken); // Wait before the next poll
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Consume error: {e.Error.Reason}");
                        // Consider error handling strategies
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Consumption cancelled.");
                    }
                    finally
                    {
                        // Ensure the consumer is closed properly
                        try
                        {
                            consumer.Close();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error closing consumer: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}