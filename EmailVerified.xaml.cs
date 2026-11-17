namespace PreciousPractice;

public partial class EmailVerified : ContentPage
{
    private readonly bool _isLogin;
    public EmailVerified(bool isLogin = false)
	{
		InitializeComponent();

        _isLogin = isLogin;
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void NavToCreateNewAccount(object sender, EventArgs e)
    {
        if (Loading.IsLoading)
            return;
   
        try
        {
            ContinueTxt.IsVisible = false;


            ContinueIndicator.IsVisible = true;
            ContinueIndicator.IsRunning = true;


            await Task.Delay(500);
            if (_isLogin)
            {
                await Loading.NavigateAsync(Navigation, () => new RegisterDeviceLogin());

            }
            else
            {
                await Loading.NavigateAsync(Navigation, () => new CreateNewAccount());
            }
       
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);


            await DisplayAlert("Error", "Failed to open Registration Certificate.", "OK");
        }
        finally
        {

            ContinueTxt.IsVisible = true;

            // Hide spinner
            ContinueIndicator.IsRunning = false;
            ContinueIndicator.IsVisible = false;
        }
    }
}