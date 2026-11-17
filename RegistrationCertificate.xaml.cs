namespace PreciousPractice;

public partial class RegistrationCertificate : ContentPage
{
	public RegistrationCertificate()
	{
		InitializeComponent();
	}
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void NavToLegalandCompliance(object sender, EventArgs e)
    {
        await Navigation.SafePopAsync();
    }
}