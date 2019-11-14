using Newtonsoft.Json;

namespace MicroServicos.Autentica.API.Models
{
    public class AutenticaViewModel
    {
        [JsonProperty("usuario")]
        public string Usuario { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }
    }
}
