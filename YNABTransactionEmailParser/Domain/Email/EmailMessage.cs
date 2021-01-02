using System.Collections.Generic;

namespace YNABTransactionEmailParser.Domain.Email
{
    public class EmailMessage    {
        public Headers headers { get; set; } 
        public Envelope envelope { get; set; } 
        public string plain { get; set; } 
        public string html { get; set; } 
        public string reply_plain { get; set; } 
        public List<Attachment> attachments { get; set; } 
    }
}
