using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using YNABTransactionEmailParser.Domain.YNAB;

namespace YNABTransactionEmailParser
{
    public class YNABClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<YNABClient> _logger;
        public YNABClient(ILogger<YNABClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        private async Task<string> GetAccountId(string last4Digits)
        {
            var response = await _httpClient.GetAsync("accounts");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var error = await response.Content.ReadAsAsync<GetAccountResponseError>();
                throw new System.Exception($"Error retrieving accounts: {error.Error.Detail}");
            }
            response.EnsureSuccessStatusCode();
            var account = (await response.Content.ReadAsAsync<GetAccountsResponse>())?.Data.Accounts.FirstOrDefault(a => a.Note?.Contains(last4Digits) ?? false);
            return account?.Id;
        }

        internal async Task PostTransaction(Transaction localTransaction)
        {
            var accountId = await GetAccountId(localTransaction.Last4);
            if (accountId == null)
            {
                throw new Exception($"No account found for {localTransaction.Last4}");
            }
            _logger.LogInformation("Found account: {id}", accountId);

            _logger.LogDebug("Date: {date}", localTransaction?.Date);
            _logger.LogDebug("Amount: {amount}", localTransaction?.Amount);
            _logger.LogDebug("Payee: {payee_name}", localTransaction?.Payee);

            var request = new AddTransactionRequest
            {
                transaction = new Domain.YNAB.Transaction {
                    account_id = accountId,
                    date = localTransaction.Date.ToString("yyyy-MM-dd"),
                    amount = (int)(localTransaction.Amount * -1000),
                    payee_name = localTransaction.Payee,
                    cleared = "uncleared",
                    memo = "Instant Sync",
                }
            };

            await _httpClient.PostAsJsonAsync("transactions", request);
        }
    }
}