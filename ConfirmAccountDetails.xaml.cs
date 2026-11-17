using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class ConfirmAccountDetails : ContentPage
{   
	
	public ConfirmAccountDetails()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, EventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private async void OnCopyAccountNumberTapped(object sender, TappedEventArgs e)
    {
        string accountNumber = AccountNumberLabel.Text?.Trim() ?? "";
       
        if (!string.IsNullOrEmpty(accountNumber))
        {
            await Clipboard.Default.SetTextAsync(accountNumber);

            await DisplayAlert("Copied", "Account number copied to clipboard.", "OK");
        }
    }

	private async void NavToSetPassword(object sender, EventArgs e)
	{  if (Loading.IsLoading)
            return;
        try
        {
            continueTxt.IsVisible = false;


            ContinueIndicator.IsVisible = true;
            ContinueIndicator.IsRunning = true;

        
            await Task.Delay(500);
           
            await Loading.NavigateAsync(Navigation, () => new SetPassword(true));
	    }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            
                await DisplayAlert("Error", "Failed to open Registration Certificate.", "OK");
        }
        finally
        {
            
            continueTxt.IsVisible = true;

            // Hide spinner
            ContinueIndicator.IsRunning = false;
            ContinueIndicator.IsVisible = false;
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        AccountNumberLabel.Text = Preferences.Get("AccountNumber", DefaultAccountNumber.TemporaryAccountNumber);
    }
}