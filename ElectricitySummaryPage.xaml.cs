
using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class ElectricitySummaryPage : ContentPage
{
    private const decimal Charges = 100m;

    private readonly String _meterNumber;
    private readonly decimal _amount;
    private readonly decimal _totalDebit;

    private bool _addToBeneficiaries = false;

    private readonly string _billType;
    public ElectricitySummaryPage(string? companyLogo = null, string providerName = "",
		string meterNumber ="", string customerName = "", decimal amount = 0, string billType = "Electricity",
        string numberLabel = "Meter Number", string planName = "")
	{
		
		InitializeComponent();
		_meterNumber = meterNumber;
		_amount = amount;
		_totalDebit = amount + Charges;
        _billType = billType;

        //The words that change between Electricity and tv
        BillTypeLabel.Text = billType;
        DetailsHeadingLabel.Text = $"{numberLabel}Details";

        // Amounts
        ElectricityAmount.Text = Format(amount);
        Amount.Text = Format(amount);
        TotalDebit.Text = Format(_totalDebit);

        // Meter details: the customer's name, or the provider if there's no name
        ServiceLogo.Source = companyLogo;
        UserName.Text = string.IsNullOrEmpty(customerName) ? providerName : customerName;
        MeterValues.Text = $"Meter Number: {meterNumber}";

        //the bundle 
        PlanNameLabel.Text = planName;
        PlanNameLabel.IsVisible = !string.IsNullOrEmpty(planName);
    }
    private static string Format(decimal value) => $"N{value:N2}";
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private async void OnChangeTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private void OnAddBeneficiaryToggleTapped(object sender, TappedEventArgs e)
    {
        _addToBeneficiaries = !_addToBeneficiaries;

        AddBeneficiaryToggle.Source = _addToBeneficiaries
            ? "dark_add_beneficiaries.png"
            :"light_add_beneficiaries.png";
    }
    private async void NavToTransactionPin(object sender, TappedEventArgs e)
    { if (Loading.IsLoading)
            return;
        try
        {
            ProceedTxt.IsVisible = false;
            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

            await Task.Delay(300);
            await Loading.NavigateAsync(Navigation, () => new TransactionPin(onPinVerified: OnPinVerifiedAsync));
        }
        finally
        {
            ProceedTxt.IsVisible = true;
            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
    }
    // Runs after the correct PIN: take the money, then show success
    private async Task OnPinVerifiedAsync()
    {
        DefaultAccountBalance.Deduct(_totalDebit);

        var successView = new PhoneNumberVerified();
        successView.Configure(
            imageSource: "successful.png",
            title: " Successful",
            subtitle: $"Congratulations! Your {_billType} payment of {Format(_amount)} was successful.",
            buttonText: "Dismiss",
            onContinue: async () =>
            {
                System.Diagnostics.Debug.WriteLine("onContinue was called");
                await Navigation.PopToRootAsync();
            });
        var page = new ContentPage
        {
            Content = successView,
            BackgroundColor = Colors.White
        };
        NavigationPage.SetHasNavigationBar(page, false);
        Shell.SetNavBarIsVisible(page, false);

        await Navigation.PushAsync(page);
    }
}