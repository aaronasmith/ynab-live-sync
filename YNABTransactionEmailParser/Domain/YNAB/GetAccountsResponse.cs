using Newtonsoft.Json;

namespace YNABTransactionEmailParser.Domain.YNAB
{
    public partial class GetAccountsResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("accounts")]
        public Account[] Accounts { get; set; }

        [JsonProperty("server_knowledge")]
        public long ServerKnowledge { get; set; }
    }

    public partial class Account
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("on_budget")]
        public bool OnBudget { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("balance")]
        public long Balance { get; set; }

        [JsonProperty("cleared_balance")]
        public long ClearedBalance { get; set; }

        [JsonProperty("uncleared_balance")]
        public long UnclearedBalance { get; set; }

        [JsonProperty("transfer_payee_id")]
        public string TransferPayeeId { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}