namespace YNABTransactionEmailParser
{
    public interface IParser
    {
        Transaction ParseEmail(string contents);
        string GetText(Domain.Email.EmailMessage email){
            return email.plain ?? email.html;
        }
    }
}