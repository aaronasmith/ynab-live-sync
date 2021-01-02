using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YNABTransactionEmailParser.Domain.YNAB;

namespace YNABTransactionEmailParser
{
    public class YNABClient
    {
        private readonly HttpClient _httpClient;
        public YNABClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<string> GetAccountId(string last4Digits){
            var response = await _httpClient.GetAsync("accounts");
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound){
                var error = await response.Content.ReadAsAsync<GetAccountResponseError>();
                throw new System.Exception($"Error retrieving accounts: {error.Error.Detail}");
            }
            response.EnsureSuccessStatusCode();
            var account = (await response.Content.ReadAsAsync<GetAccountsResponse>()).Data.Accounts.FirstOrDefault(a => a.Note.Contains(last4Digits));
            return account.Id;
        }

        internal async Task PostTransaction(Transaction transaction)
        {
            var accountId = await GetAccountId(transaction.Last4);
            if(accountId == null){
                throw new Exception($"No account found for {transaction.Last4}");
            }
            await _httpClient.PostAsJsonAsync("transactions", new AddTransactionRequest{
                transaction = {
                    account_id = accountId,
                    date = transaction.Date.ToString("yyyy-MM-dd"),
                    amount = (int)(transaction.Amount * 1000),
                    payee_name = transaction.Payee,
                    cleared = "uncleared"
                }
            });
        }
    }
}