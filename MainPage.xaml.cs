namespace PreciousPractice;

public partial class MainPage : ContentPage
{
    public class OnboardingSlide
    {
        public string Icon { get; set; }
        public string SmallTitle { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Description { get; set; }
    }
    public MainPage()
    {
        InitializeComponent();

        carousel.ItemsSource = new List<OnboardingSlide>
        {
            new OnboardingSlide
            {
                Icon = "people_icon.png",
                SmallTitle = "Join the Movement",
                Title1 = "Your community. Your cooperative.",
                Title2 = "Your future.",
                Description = "Be part of Nigeria's fastest-growing cooperative. Together, we save smarter, invest better, and build wealth as a community."
            },

            new OnboardingSlide
            {
                Icon = "bolt_icon.png",
                SmallTitle = "Lightning Speed",
                Title1 = "Move money at the speed of",
                Title2 = "thought.",
                Description = "Send funds instantly to any account. Zero waiting. Zero fees on your first 25 monthly transfers. Just pure speed."
            },

            new OnboardingSlide
            {
                Icon = "rotation_icon.png",
                SmallTitle = "Fort Knox Level",
                Title1 = "Your money. Locked down tight.",
                Title2 = "",
                Description = "Military-grade encryption. Biometric authentication. Real-time monitoring. Sleep easy knowing your funds are protected 24/7."
            },

            new OnboardingSlide
            {
                Icon = "currency_icon.png",
                SmallTitle = "Wealth Builder",
                Title1 = "Watch your money grow on",
                Title2 = "autopilot.",
                Description = "Plant your savings today and harvest returns tomorrow. Start investing with as little as 5,000 and earn up to 12% annually."
            }
        };
    }

    private void Carousel_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (e.CurrentPosition == 3)
        {
            skipStack.IsVisible = false;
            createStack.IsVisible = true;
        }
        else
        {
            skipStack.IsVisible = true;
            createStack.IsVisible = false;
        }
    }
        private void OnSkipTapped(object sender, TappedEventArgs e)
    {
        carousel.Position = 3;
    }

    
    private void OnArrowTapped(object sender, TappedEventArgs e)
    {
        if (carousel.Position < 3)
        {
            carousel.Position++;
        }
    }
    


    private async void NavToLegalandCompliance(object sender, EventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {
            createTxt.IsVisible = false;
            indicator.IsVisible = true;
            indicator.IsRunning = true;

            await Loading.NavigateAsync(Navigation, () =>new LegalandCompliance());
        }
        catch(Exception ex) 
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Failure to navigate", "ok");
        }
        finally
        {
            createTxt.IsVisible = true;
            indicator.IsVisible = false;
            indicator.IsRunning = false;
        }
    }
    private async void NavToRegisterDeviceLogin(object sender, EventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {
            loginTxt.IsVisible = false;
            LoginIndicator.IsVisible = true;
            LoginIndicator.IsRunning= true;

            await Loading.NavigateAsync(Navigation, () => new RegisterDeviceLogin());
        }
        catch(Exception ex) 
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Failure to navigate", "ok");
        }
        finally
        {
            loginTxt.IsVisible = true;
            LoginIndicator.IsVisible = false;
            LoginIndicator.IsRunning = false;
        }
    }
    private async void NavToRegistrationPage(object sender, TappedEventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () => new RegistrationPage());
    }
}


