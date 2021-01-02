namespace YNABTransactionEmailParser.Domain.Email
{
    public class Attachment    {
        public string content { get; set; } 
        public string file_name { get; set; } 
        public string content_type { get; set; } 
        public int size { get; set; } 
        public string disposition { get; set; } 
    }
}
