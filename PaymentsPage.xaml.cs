namespace PreciousPractice;

public partial class PaymentsPage : ContentPage
{
	public PaymentsPage()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnElectricityTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new ElectricityBill());
	}
	private async void OnTvTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new TvBill());
	}
}