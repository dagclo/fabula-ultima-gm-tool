using Newtonsoft.Json;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Affinities
    {
        // add remaining affinities
        [JsonProperty("wind")]
        public string? Air { get; set; }
        [JsonProperty("bolt")]
        public string? Bolt { get; set; }
        [JsonProperty("physical")]
        public string? Physical { get; set; }
        [JsonProperty("dark")]
        public string? Dark { get; set; }
        [JsonProperty("earth")]
        public string? Earth { get; set; }
        [JsonProperty("fire")]
        public string? Fire { get; set; }
        [JsonProperty("ice")]
        public string? Ice { get; set; }
        [JsonProperty("light")]
        public string? Light { get; set; }
        [JsonProperty("poison")]
        public string? Poison { get; set; }
    }
}
