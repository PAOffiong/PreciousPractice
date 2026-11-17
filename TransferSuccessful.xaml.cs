using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class TransferSuccessful : ContentPage
{
	public TransferSuccessful()
	{
		InitializeComponent();
	}

	private async void OnShareTapped(object sender, EventArgs e)
	{
		try
		{
			var receiptPage = new ShareReceipt();

			await Navigation.PushAsync(new ShareReceipt());
			
			await Task.Delay(300);
			//Capture the receipt

			await receiptPage.ShareAsImageAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Share receipt error: {ex}");

            await DisplayAlert("Error","Failed to share the transaction receipt.", "OK");
        }
	}
	private async void OnViewReceiptTapped(object sender, TappedEventArgs e)
	{
		await Navigation.PushAsync(new ShareReceipt());
	}
	private async void OnDismissBorderTapped(object sender, TappedEventArgs e)
	{
		try
		{
			DismissTxt.IsVisible = true;

			DismissIndicator.IsVisible = false;
			DismissIndicator.IsRunning = false;

			await Task.Delay(500);
			await Navigation.PushAsync(new TransferPage());
	       
		}
		catch(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex.Message);

			await DisplayAlert("Error", "Failed to Continue", "OK");
		}
		finally
		{
			DismissTxt.IsVisible = false;

			DismissIndicator.IsVisible = true;
			DismissIndicator.IsRunning = true;
		}

	}
}