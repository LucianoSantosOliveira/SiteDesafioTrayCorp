using Newtonsoft.Json;

namespace SiteDesafioTrayCorp.Models
{
    public class ProdutoViewModel
    {
        [JsonProperty("id")]
        public Guid id { get; set; }
        [JsonProperty("nome")]
        public string? nome { get; set; }
        [JsonProperty("estoque")]
        public int estoque { get; set; }
        [JsonProperty("value")]
        public float value { get; set; }

        [JsonIgnore]
        public string Valor { get; set; }
    }
}
