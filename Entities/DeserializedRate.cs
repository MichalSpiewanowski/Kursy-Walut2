using System.Text.Json.Serialization;

namespace KursyWalut2.Entities
{
    public class DeserializedRate
    {
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("mid")]
        public double? Mid { get; set; }
        [JsonPropertyName("bid")]
        public double? Bid { get; set; }
        [JsonPropertyName("ask")]
        public double? Ask { get; set; }
    }
}
