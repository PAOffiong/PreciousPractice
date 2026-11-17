using PreciousPractice.Helpers;
namespace PreciousPractice;

public partial class StartBankingNow : ContentPage
{
	public StartBankingNow()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        AccountNumberLabel.Text = DefaultAccountNumber.TemporaryAccountNumber;
    }

    private async void OnCopyAccountNumberTapped(object sender, TappedEventArgs e)
    {
        string accountNumber = DefaultAccountNumber.TemporaryAccountNumber;

        if (!string.IsNullOrEmpty(accountNumber))
        {
            await Clipboard.Default.SetTextAsync(accountNumber);

            await DisplayAlert("Copied", "Account number copied to clipboard.", "OK");
        }
    }
    private async void NavToRegistrationPage(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        { 

            startTxt.IsVisible = false;


            StartIndicator.IsVisible = true;
            StartIndicator.IsRunning = true;

            await Task.Delay(500);

            await Loading.NavigateAsync(Navigation, () => new RegistrationPage());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Failed to continue to sign up.", "OK");
        }
        finally
        {
            // Show text again
            startTxt.IsVisible = true;

            // Hide spinner
            StartIndicator.IsRunning = false;
            StartIndicator.IsVisible = false;
        }
    }
}