using System.Text.Json.Serialization;

namespace KursyWalut2.Entities
{
    public class DeserializedRequest
    {
        [JsonPropertyName("table")]
        public string Table { get; set; }
        [JsonPropertyName("no")]
        public string No { get; set; }
        [JsonPropertyName("effectiveDate")]
        public DateOnly EffectiveDate { get; set; }
        [JsonPropertyName("rates")]
        public List<DeserializedRate> Rates { get; set; }
    }
}
