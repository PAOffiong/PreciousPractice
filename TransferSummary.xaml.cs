
using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class TransferSummary : ContentPage
{
    private readonly string _accountName;
    private readonly string _accountNumber;
    private readonly string _bankIcon;
    private readonly string _bankName;

    public TransferSummary(string accountName ="", string accountNumber ="", string bankName ="", string bankIcon = "flower_green.png" )
    {
		InitializeComponent();

        _accountName = accountName;
        _accountNumber = accountNumber;
        _bankIcon = bankIcon;
        _bankName = bankName;

        DetailsAccountNameLabel.Text = accountName;
        DetailsAccountNumberLabel.Text = accountNumber;
        BankIconImage.Source = bankIcon;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        AccountNumberLabel.Text = DefaultAccountNumber.TemporaryAccountNumber;
        AvailableBalance.Text = DefaultAccountBalance.Format(DefaultAccountBalance.SelectedBalance);
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void OnChangeTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void OnAmountEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        bool amountEnteredValid = !String.IsNullOrWhiteSpace(AmountEntry.Text);

        ProceedBorder.IsEnabled = amountEnteredValid;

        if(int.TryParse(AmountEntry.Text,out int amount))
        {
            if(amount <500)
            {

                //invalid
            }
            else
            {
                //valid amount
            }
        }
    }
    private void OnAmountTapped(object sender, TappedEventArgs e)
    {
        if(sender == FiveHundred)
        {
            AmountEntry.Text = "500";
        }
        else if(sender == OneThousand)
        {
            AmountEntry.Text = "1000";
        }
        else if(sender == TwoThousand)
        {
            AmountEntry.Text = "2000";
        }
        else if(sender == FiveThousand)
        {
            AmountEntry.Text = "5000";
        }
        else if(sender == TenThousand)
        {
            AmountEntry.Text = "10000";
        }
        else if(sender == FifteenThousand)
        {
            AmountEntry.Text = "15000";
        }
        return;
    }
    private void OnRemarkTapped(object sender,TappedEventArgs e)
    {
        if(sender == Purchase)
        {
            RemarkEntry.Text = "Purchase";
        }
        else if(sender == Personal)
        {
            RemarkEntry.Text = "Personal";
        }
        else if(sender == Savings)
        {
            RemarkEntry.Text = "Savings";
        }
        return;
    }
    private async void NavToTransferSummaryPage(object sender, EventArgs e)
    {
        if (Loading.IsLoading)
            return;
        String amountEntered = AmountEntry.Text.Trim() ?? "";

        // turn the text to a number and check minimum

        if(!decimal.TryParse(amountEntered, out decimal amount) || amount < 500)
        {
            await DisplayAlert("Invalid Amount", "Minimum transfer is N500", "OK");
            return;
        }

        try
        {
            ProceedTxt.IsVisible = false;

            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

            await Loading.ShowAsync("Verifying transaction...");
            await Task.Delay(500);
            await Navigation.PushAsync(new TransferSummaryPage(_accountName, _accountNumber, amount, _bankIcon, _bankName));

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Failed to continue with transfer.", "OK");
        }
        finally
        {
            await Loading.HideAsync();

            ProceedTxt.IsVisible = true;

            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
    }
}