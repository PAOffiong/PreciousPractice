namespace PreciousPractice;

public partial class TransferPage : ContentPage
{	
	public TransferPage()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void NavToTransferToPyrowo(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new TransferToPyrowo());
	}
	private async void NavToTransferToOtherBanks(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new TransferToOtherBanks());
	}
}