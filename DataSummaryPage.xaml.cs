
using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class DataSummaryPage : ContentPage
{
    private const decimal Discount = 0m;

    private readonly DataPlan? _plan;
    private readonly string _serviceName ="";
    private readonly string _phoneNumber ="";
    private readonly decimal _totalDebit;

    // Off until the user taps the toggle
    private bool _addToBeneficiaries = false;
    public DataSummaryPage(ImageSource? serviceIcon = null, string serviceName = "", string phoneNumber="", DataPlan? plan = null)
	{
		InitializeComponent();
        if (plan is null)
            return;
        // Keep what this page was given
        _plan = plan;
        _serviceName = serviceName;
        _phoneNumber = phoneNumber;
        _totalDebit = plan.Price - Discount;

        // Top: which bundle
        DataAmount.Text = plan.PlanDescription;          

        // Transfer details card
        AmountTransfered.Text = Format(plan.Price);     
        TotalDebit.Text = Format(_totalDebit);

        // Beneficiary details card
        ServiceIcon.Source = serviceIcon;
        ServiceNameLabel.Text = serviceName;
        PhoneNumberLabel.Text = phoneNumber;

    }
    private static string Format(decimal value) => $"N{value:N2}";
    private async void OnBackIconTapped(object sender,TappedEventArgs e)
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
            : "light_add_beneficiaries.png";
    } 
	private async void NavToTransactionPin(object sender,EventArgs e)
	{
        if (Loading.IsLoading)
            return;
        try
        {
            ProceedTxt.IsVisible = false;
            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

            await Loading.NavigateAsync(Navigation, () => new TransactionPin(onPinVerified: OnPinVerifiedAsync));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlert("Error", "Failed to continue with data purchase.", "OK");
        }
        finally
        {
            ProceedTxt.IsVisible = true;
            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
    }
    private async Task OnPinVerifiedAsync()
    {
        DefaultAccountBalance.Deduct(_totalDebit);
        {
            var verifiedView = new PhoneNumberVerified();
            verifiedView.Configure(
                imageSource: "successful.png",
                title: " Successful",
                subtitle: $"Congratulations! Your Data purchase of {Format(_totalDebit)} was successful.",
                buttonText: "Dismiss",
                onContinue: async () => await Navigation.PopToRootAsync()
            );

            var page = new ContentPage { Content = verifiedView, BackgroundColor = Colors.White };
            NavigationPage.SetHasNavigationBar(page, false);
            Shell.SetNavBarIsVisible(page, false);
            await Navigation.PushAsync(page);
        }
    }
}