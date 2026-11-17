
using CommunityToolkit.Maui.Core.Platform;

namespace PreciousPractice;

public partial class AddMoneytoBank : ContentPage
{
	private readonly string _bankName;
	public AddMoneytoBank(string bankName = "", string accountNumber = "")
	{

		InitializeComponent();

		_bankName = bankName;

		BankLabel.Text = bankName;
		AccountNumberLabel.Text = accountNumber;
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnAmountEntryTextChanged(object sender, TextChangedEventArgs e)
	{
		bool amountEnteredValid = !String.IsNullOrWhiteSpace(AmountEntry.Text);

		ConfirmBorder.IsEnabled = amountEnteredValid;
		//if(amountEnteredValid)
		//{
			//await AmountEntry.HideKeyboardAsync();
			//AmountEntry.Unfocus();
		//}
		if(int. TryParse(AmountEntry.Text,out int amount))
		{
			if(amount < 1000)
			{
				// Inavalid amount
			}
			else
			{
				// Valid amount
			}
		}
	}
	private void OnAmountTapped(object sender, TappedEventArgs e)
	{
		if(sender == FiveThousand)
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
		else if(sender == TwentyThousand)
		{
			AmountEntry.Text = "20000";
		}
		else if(sender == TwentyfiveThousand)
		{
			AmountEntry.Text = "25000";
		}
		return;
		
	}
	private async void NavToAddMoneySummary(object sender, EventArgs e)
	{
		String amountEntered = AmountEntry.Text;

		if (Loading.IsLoading)
			return;
		try
		{
			ConfirmTxt.IsVisible = false;

			ConfirmIndicator.IsVisible = true;
			ConfirmIndicator.IsRunning = true;

			await Loading.ShowAsync("Processing transfer...");
			
			await Task.Delay(1000);
			await Navigation.PushAsync(new AddMoneySummaryPage(_bankName,amountEntered));
		    
		}
		catch(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex.Message);

			await DisplayAlert("Error", "Failed to continue to sign up", "OK");
		}
		finally
		{
            ConfirmTxt.IsVisible = true;

            ConfirmIndicator.IsVisible = false;
            ConfirmIndicator.IsRunning = false;
        }
	}
}