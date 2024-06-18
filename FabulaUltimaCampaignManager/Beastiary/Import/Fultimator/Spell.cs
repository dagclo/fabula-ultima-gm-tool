using Newtonsoft.Json;
using System.Collections.Generic;

namespace FabulaUltimaGMTool.Beastiary.Import.Fultimator
{
    public class Spell
    {
        [JsonProperty("special")]
        public List<object> special { get; set; }
        [JsonProperty("name")]
        public string duration { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("effect")]
        public string effect { get; set; }
        [JsonProperty("target")]
        public string target { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("attr2")]
        public string attr2 { get; set; }
        [JsonProperty("attr1")]
        public string attr1 { get; set; }
        [JsonProperty("mp")]
        public string mp { get; set; }
        [JsonProperty("range")]
        public string range { get; set; }
    }    
}
