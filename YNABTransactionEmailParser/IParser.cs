namespace YNABTransactionEmailParser
{
    public interface IParser
    {
        Transaction ParseEmail(string contents);
    }
}