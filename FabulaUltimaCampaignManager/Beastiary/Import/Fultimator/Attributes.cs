using Newtonsoft.Json;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    public class Attributes
    {
        [JsonProperty("will")]
        public int Willpower { get; set; }
        [JsonProperty("insight")]
        public int Insight { get; set; }
        [JsonProperty("dexterity")]
        public int Dexterity { get; set; }
        [JsonProperty("might")]
        public int Might { get; set; }
    }
}
