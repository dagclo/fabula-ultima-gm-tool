using Newtonsoft.Json;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    public class Special
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("effect")]
        public string effect { get; set; }
    }
}
