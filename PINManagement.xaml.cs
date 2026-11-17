namespace PreciousPractice;

public partial class PINManagement : ContentPage
{
	public PINManagement()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnCreatePinTapped(object sender, TappedEventArgs e)
	{
        await Loading.NavigateAsync(Navigation, () => new PinToken());
    }

}