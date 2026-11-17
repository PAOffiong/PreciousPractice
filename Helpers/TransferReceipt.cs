namespace PreciousPractice.Helpers;

public class TransferReceipt 
{
	public static string SenderName { get; set; } = "Janet Doe Kehinde";
	public static string SenderAccountNumber { get; set; } = "8123456789";

	public static string RecipientName { get; set; } = "John Doe Kehinde";
    public static string RecipientAccountNumber { get; set; } = "8124365870";
    public static string RecipientBankName { get; set; } = string.Empty;


    public static decimal Amount { get; set; } = 0m;

    public static decimal Charge { get; set; } = 10m;
    public static string TransactionType {  get; set; } = "Transfer";

	public static string TransactionNumber { get; set; } = "234567890123456789012345";
    public static string SessionId { get; set; } = "100004234567890123456789012345";
    public static  DateTime  Date{ get; set;} = new DateTime(2026, 9, 30, 18, 20, 11);

    public  static string NewReference() =>
		DateTime.Now.ToString("yyyyMMddHHmmss") + Random.Shared.Next(100000000, 999999999);

	//8123456789 - 812****789
	public static string Mask(string accountNumber)=> accountNumber.Length<7
		?accountNumber
        : $"{accountNumber[..3]}****{accountNumber[^3..]}"; //first 3 digit should show and the last 3 digits should show
}