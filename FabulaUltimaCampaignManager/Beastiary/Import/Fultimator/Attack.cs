using Newtonsoft.Json;
using System.Collections.Generic;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    public class Attack
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("range")]
        public string range { get; set; }
        [JsonProperty("attr2")]
        public string attr2 { get; set; }
        [JsonProperty("attr1")]
        public string attr1 { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("special")]
        public List<object> special { get; set; }
    }
}
