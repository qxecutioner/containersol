using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class TransferData
    {
        [JsonPropertyName(nameof(Date))]
        public string Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }
}
