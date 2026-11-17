
using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class TransferSummaryPage : ContentPage
{
    //fixed transfer charge
    private const decimal transferCharge = 10m;

    private readonly string _accountName;
    private readonly string _accountNumber;
    private readonly decimal _amount;
    private readonly decimal _totalDebit;
    private readonly string _bankIcon;
    private readonly string _bankName;
    // Off until the user taps the toggle
    private bool _addToBeneficiaries = false;
    public TransferSummaryPage(string accountName ="", string accountNumber="", decimal amount = 0, string bankIcon = "flower_green.png", string bankName = "")
    {
        InitializeComponent();

        _accountName = accountName;
        _accountNumber = accountNumber;
        _amount = amount;
        _totalDebit = _amount + transferCharge;

        //beneficiary
        DetailsAccountNameLabel.Text = accountName;
        DetailsAccountNumberLabel.Text = accountNumber;

        //amount
        TransferAmount.Text = Format(_amount);
        AmountTransfered.Text = Format(_amount);
        ChargeLabel.Text = Format(transferCharge);
        TotalDebit.Text = Format(_totalDebit);
        TransferReceipt.Amount = _amount;

        _bankIcon = bankIcon;
        BankIconImage.Source = bankIcon;

        _bankName = bankName;
        AddBeneficiaryRow.IsVisible = !SavedBeneficiaries.All.Any(b => b.AccountNumber == accountNumber);
    }

    // how money should look like in this page
    private static string Format(decimal value)
    {
        return $"N{value:N2}";
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void OnChangeTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private void OnAddBeneficiaryToggleTapped(object sender, TappedEventArgs e)
    {
        // Flip on ↔ off
        _addToBeneficiaries = !_addToBeneficiaries;

        // Show the picture that matches the new state
        AddBeneficiaryToggle.Source = _addToBeneficiaries
            ? "dark_add_beneficiaries.png"
            : "light_add_beneficiaries.png";
    }
    private void SaveAsBeneficiary()
    {
        // Don't add the same account twice
        if (SavedBeneficiaries.All.Any(b => b.AccountNumber == _accountNumber))
            return;

        // Split "John Doe Kehinde" into "John Doe" + "Kehinde"
        string[] parts = _accountName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string surname = parts.Length > 1 ? parts[^1] : string.Empty;
        string firstNames = parts.Length > 1 ? string.Join(" ", parts[..^1]) : _accountName;

        SavedBeneficiaries.All.Add(new SavedBeneficiary
        {
            BankIcon = string.IsNullOrEmpty(_bankIcon) ? "flower_green.png" : _bankIcon,
            BankName = _bankName,
            PersonName = firstNames,
            PersonSurName = surname,
            AccountNumber = _accountNumber
        });
    }
    private async void NavToTransactionPin(object sender, EventArgs e)
    {
        if (Loading.IsLoading)
            return;   // ignore double taps

        try
        {
            ProceedTxt.IsVisible = false;
            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

            await Loading.NavigateAsync(Navigation,
                () => new TransactionPin(onPinVerified: CompleteTransferAsync));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Failed to continue with transfer.", "OK");
        }
        finally
        {
            ProceedTxt.IsVisible = true;
            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
    }

    // Runs only after the correct PIN: the transfer actually happens here
    private async Task CompleteTransferAsync()
    {
        DefaultAccountBalance.Deduct(_totalDebit);
        //Save the beneficiary, if the toggle was on
        if (_addToBeneficiaries)
            SaveAsBeneficiary();

        //Fill in the receipt, with the real time of the transfer
        TransferReceipt.SenderName = Preferences.Get("FullName", "Janet Doe Kehinde");
        TransferReceipt.SenderAccountNumber = DefaultAccountNumber.TemporaryAccountNumber;

        TransferReceipt.RecipientName = _accountName;
        TransferReceipt.RecipientAccountNumber = _accountNumber;
        TransferReceipt.RecipientBankName = _bankName;

        TransferReceipt.Amount = _amount;
        TransferReceipt.Charge = transferCharge;
        TransferReceipt.TransactionType = "Transfer";

        TransferReceipt.TransactionNumber = TransferReceipt.NewReference();
        TransferReceipt.SessionId = "100004" + TransferReceipt.NewReference();
        TransferReceipt.Date = DateTime.Now;

        // 3. Show the success page
        await Navigation.PushAsync(new TransferSuccessful());
    }
}