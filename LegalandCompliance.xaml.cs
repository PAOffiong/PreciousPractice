namespace PreciousPractice;

public partial class LegalandCompliance : ContentPage
{
    public LegalandCompliance()
    {
        InitializeComponent();
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void NavToRegistrationCertificate(object sender, EventArgs e)
    {       
        RegistrationArrow.IsVisible = false;       
        RegistrationIndicator.IsVisible = true;
        RegistrationIndicator.IsRunning = true;

        try
        {
            await Loading.NavigateAsync(Navigation, () => new RegistrationCertificate());
        }
        catch
        {
            await DisplayAlert("Error", "Failed to open Registration Certificate.", "OK");
        }
        finally
        {
            RegistrationArrow.IsVisible = true;
            RegistrationIndicator.IsRunning = false;
            RegistrationIndicator.IsVisible = false;
        }
    }
    private async void NavToIdInformation(object sender, EventArgs e)
    {
        
        SignupTxt.IsVisible = false;       
        indicator.IsVisible = true;
        indicator.IsRunning = true;

        try
        {
            await Loading.NavigateAsync(Navigation, () => new IdInformation());
        }
        catch
        {
            await DisplayAlert( "Error", "Failed to continue to sign up.", "OK");
        }
        finally
        {
            SignupTxt.IsVisible = true;
            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }
    }
}