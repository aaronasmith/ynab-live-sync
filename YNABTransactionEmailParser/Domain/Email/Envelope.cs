using System.Collections.Generic;

namespace YNABTransactionEmailParser.Domain.Email
{
    public class Envelope    {
        public string to { get; set; } 
        public string from { get; set; } 
        public string helo_domain { get; set; } 
        public string remote_ip { get; set; } 
        public List<string> recipients { get; set; } 
        public Spf spf { get; set; } 
        public bool tls { get; set; } 
    }
}
