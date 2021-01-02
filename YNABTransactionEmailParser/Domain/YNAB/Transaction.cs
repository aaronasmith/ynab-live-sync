using System.Collections.Generic;

namespace YNABTransactionEmailParser.Domain.YNAB
{
    public class Transaction    {
        public string account_id { get; set; } 
        public string date { get; set; } 
        public int amount { get; set; } 
        public string payee_id { get; set; } 
        public string payee_name { get; set; } 
        public string category_id { get; set; } 
        public string memo { get; set; } 
        public string cleared { get; set; } 
        public bool approved { get; set; } 
        public string flag_color { get; set; } 
        public string import_id { get; set; } 
    }

}