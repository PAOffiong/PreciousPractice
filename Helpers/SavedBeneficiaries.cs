using System.Collections.ObjectModel;

namespace PreciousPractice.Helpers;

public class SavedBeneficiary
{
    public string BankIcon { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string PersonName { get; set; } = string.Empty;
    public string PersonSurName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
}

public static class SavedBeneficiaries
{
    public static ObservableCollection<SavedBeneficiary> All { get; } = new()
    {
        new SavedBeneficiary { BankIcon = "wema_icon.png",       BankName = "Wema Bank",              PersonName = "Sarah",    PersonSurName = "Chijioke", AccountNumber = "2718281828" },
        new SavedBeneficiary { BankIcon = "first_bank.png",      BankName = "First Bank",             PersonName = "John Doe", PersonSurName = "Kehinde",  AccountNumber = "2468135790" },
        new SavedBeneficiary { BankIcon = "access_bank.png",     BankName = "Access Bank",            PersonName = "John Doe", PersonSurName = "Kehinde",  AccountNumber = "0123456789" },
        new SavedBeneficiary { BankIcon = "moniepoint_bank.png", BankName = "MoniePoint Bank",        PersonName = "Sarah",    PersonSurName = "Chijioke", AccountNumber = "4512327626" },
        new SavedBeneficiary { BankIcon = "alat_wema.png",       BankName = "Alat by Wema Bank",      PersonName = "Sarah",    PersonSurName = "Chijioke", AccountNumber = "3526278937" },
        new SavedBeneficiary { BankIcon = "okpe_bank.png",       BankName = "Okpe Microfinance Bank", PersonName = "John Doe", PersonSurName = "Kehinde",  AccountNumber = "2652789298" },
    };
}