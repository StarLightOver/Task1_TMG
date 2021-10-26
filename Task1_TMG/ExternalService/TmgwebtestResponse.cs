using System.Text.Json.Serialization;

namespace Task1_TMG
{
    public class TmgwebtestResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}