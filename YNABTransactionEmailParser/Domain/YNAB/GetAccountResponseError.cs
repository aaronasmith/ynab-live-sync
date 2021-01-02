using Newtonsoft.Json;

namespace YNABTransactionEmailParser.Domain.YNAB
{
       public partial class GetAccountResponseError
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public partial class Error
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }
}