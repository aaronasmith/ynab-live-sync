using System.Collections.Generic;

namespace YNABTransactionEmailParser.Domain.Email
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Headers    {
        public string return_path { get; set; } 
        public string date { get; set; } 
        public string from { get; set; } 
        public string to { get; set; } 
        public string message_id { get; set; } 
        public string subject { get; set; } 
        public string mime_version { get; set; } 
        public string content_type { get; set; } 
        public string delivered_to { get; set; } 
        public string received_spf { get; set; } 
        public string authentication_results { get; set; } 
        public string user_agent { get; set; } 
    }
}
