using PreciousPractice.Helpers;
namespace PreciousPractice;


public partial class AddMoney : ContentPage
{
	public AddMoney()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        AccountNumberLabel.Text = DefaultAccountNumber.TemporaryAccountNumber;
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private void OnBankArrowTapped(object sender, TappedEventArgs e)
    {
        bool showDetails = !BankName.IsVisible;
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;

        BankName.IsVisible = showDetails;
        AccountNumber.IsVisible = showDetails;
        UserName.IsVisible = showDetails;
        Details.IsVisible = showDetails;

        if (showDetails)
            BankArrow.Source = isDark ? "white_upward_angle.png" : "black_triangle.png";
        else
            BankArrow.Source = isDark ? "white_downward_angle.png" : "black_downward_angle.png";
    }
    private async void OnCopyAccountNumberTapped(object sender, TappedEventArgs e)
    {
        AccountNumberLabel.Text = DefaultAccountNumber.TemporaryAccountNumber;

        if (!string.IsNullOrEmpty(DefaultAccountNumber.TemporaryAccountNumber))
        {
            await Clipboard.Default.SetTextAsync(DefaultAccountNumber.TemporaryAccountNumber);

            await DisplayAlert("Copied", "Account number copied to clipboard.", "OK");
        }
    }
	private async void OnShareDetailsTapped(object sender, TappedEventArgs e)
	{
		string bankName = BankNameLabel.Text?.Trim() ?? "";
        AccountNumberLabel.Text = DefaultAccountNumber.TemporaryAccountNumber;
        string username = UserNameLabel.Text?.Trim() ?? "";

		string message = $"Bank Name:{bankName}\nAccount Number: {DefaultAccountNumber.TemporaryAccountNumber}\nAccount Name: {UserName}";

		await Share.Default.RequestAsync(new ShareTextRequest
		{
			Text = message,
			Title = "Share Bank Details"
		});
	}
    private void OnCashArrowTapped(object sender, TappedEventArgs e)
    {
        bool showDetails = !AccountDetails.IsVisible;
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;

        AccountDetails.IsVisible = showDetails;
        Services.IsVisible = showDetails;

        if (showDetails)
            CashArrow.Source = isDark ? "white_upward_angle.png" : "black_triangle.png";
        else
            CashArrow.Source = isDark ? "white_downward_angle.png" : "black_downward_angle.png";
    }
    private async void NavToTopUpAccountIcon(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new TopUpAccount());
	}
}