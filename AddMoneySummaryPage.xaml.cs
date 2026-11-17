namespace PreciousPractice;

public partial class AddMoneySummaryPage : ContentPage
{
    public AddMoneySummaryPage(string bankName = "", string amountEntered = "")
    {
        InitializeComponent();

        BankLabel.Text = bankName;
        AmountEntered.Text = amountEntered;
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void OnAuthorizedBorderTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {
            AuthorizedTxt.IsVisible = false;

            AuthorizedIndicator.IsVisible = true;
            AuthorizedIndicator.IsRunning = true;
            //overlay on
            await Loading.ShowAsync("Processing transfer...");

            await Task.Delay(1000);
                
            var verifiedView = new PhoneNumberVerified();
            verifiedView.Configure(
                imageSource: "transfer_successful.png",
                title: "",
                subtitle:"",
                buttonText: "Dismiss",
                onContinue: async () =>
                    await Navigation.PopToRootAsync());

            var page = new ContentPage
            {
                Content = verifiedView,
                BackgroundColor = Colors.White
            };

            // Hide navigation bar
            NavigationPage.SetHasNavigationBar(page, false);
            Shell.SetNavBarIsVisible(page, false);


            await Navigation.PushAsync(page);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Failed to Dismiss", "OK");
        }
        finally
        {

            await Loading.HideAsync();

            AuthorizedTxt.IsVisible = true;

            AuthorizedIndicator.IsVisible = false;
            AuthorizedIndicator.IsRunning = false;
        }


    }
}