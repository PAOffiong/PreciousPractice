namespace PreciousPractice;

public partial class SuccessPage : ContentPage
{
	public SuccessPage()
	{
		InitializeComponent();
	}
    private async void NavToRegisterDeviceLogin(object sender, TappedEventArgs e)
    { if (Loading.IsLoading)
            return;
        try
        {
            startTxt.IsVisible = false;


            StartIndicator.IsVisible = true;
            StartIndicator.IsRunning = true;


            await Task.Delay(500);

            await Loading.NavigateAsync(Navigation, () => new RegisterDeviceLogin());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Failed to continue to sign up.", "OK");
        }
        finally
        {
            // Stop the spinner
            startTxt.IsVisible = true;
            StartIndicator.IsRunning = false;
            StartIndicator.IsVisible = false;


        }
    }
}
