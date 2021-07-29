using System;

namespace YNABTransactionEmailParser
{
    public class Transaction{
        public string Last4 {get;set;}
        public decimal Amount {get;set;}
        public string Payee {get;set;}
        public DateTime Date {get;set;}
        public bool IgnoreTransaction {get;set;}
    }
}
