using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class AirtimeSummary : ContentPage
{
    private bool _addToBeneficiary;
    private readonly decimal _totalDebit;
    public AirtimeSummary(ImageSource? serviceIcon = null, string serviceName ="", string phoneNumber ="", string amount= "")
    {
        InitializeComponent();
        ServiceIcon.Source = serviceIcon;
        ServiceNameLabel.Text = serviceName;
        PhoneNumberLabel.Text = phoneNumber;

        decimal.TryParse(amount, out decimal parsedAmount);
        _totalDebit = parsedAmount;

        AirtimeAmount.Text = Format(parsedAmount);
        AmountTransfered.Text = Format(parsedAmount);
        TotalDebit.Text = Format(parsedAmount);
    }
    private static string Format(decimal value)
    {
        return $"N{value:N2}";
    }
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
        // Flip on ↔ off
        _addToBeneficiary = !_addToBeneficiary;

        // Show the picture that matches the new state
        AddBeneficiaryToggle.Source = _addToBeneficiary
            ? "dark_add_beneficiaries.png"
            : "light_add_beneficiaries.png";
    }
    private async void NavToTransactionPin(object sender, TappedEventArgs e)
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
            await DisplayAlert("Error", "Failed to continue with airtime purchase", "OK");

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
        
        var verifiedView = new PhoneNumberVerified();
        verifiedView.Configure(
            imageSource:"successful.png",
            title: "Successful",
            subtitle: $"Congratulations! Your airtime recharge of {AirtimeAmount.Text} was successful.",
            buttonText: "Dismiss",
            onContinue: async () => await Navigation.PopToRootAsync()
        );

        var page = new ContentPage { Content = verifiedView, BackgroundColor = Colors.White };
        NavigationPage.SetHasNavigationBar(page, false);
        Shell.SetNavBarIsVisible(page, false);
        await Navigation.PushAsync(page);
        
    }
}
