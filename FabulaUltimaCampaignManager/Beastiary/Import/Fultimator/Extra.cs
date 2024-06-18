using Newtonsoft.Json;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    public class Extra
    {
        [JsonProperty("mDef")]
        public int mDef { get; set; }
        [JsonProperty("def")]
        public int def { get; set; }
    }
}
