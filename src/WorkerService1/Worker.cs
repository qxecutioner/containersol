using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using KafkaBatchConsumerApp.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaBatchConsumerApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IKafkaConsumerService _kafkaConsumerService;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IKafkaConsumerService kafkaConsumerService)
        {
            _logger = logger;
            _configuration = configuration;
            _kafkaConsumerService = kafkaConsumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string topicName = _configuration["Kafka:TopicName"];
            string groupId = _configuration["Kafka:GroupId"];
            int batchSize = _configuration.GetValue<int>("Kafka:BatchSize", 10); // Default batch size of 10

            if (string.IsNullOrEmpty(topicName) || string.IsNullOrEmpty(groupId))
            {
                _logger.LogError("Kafka:TopicName and Kafka:GroupId must be configured in appsettings.json.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _kafkaConsumerService.ConsumeBatchAsync(topicName, groupId, batchSize, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An unexpected error occurred during consumption: {ex.Message}");
                    // Consider implementing a backoff strategy here
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            _logger.LogInformation("Worker stopped.");
        }
    }
}