using CommunityToolkit.Maui.Core.Platform;

namespace PreciousPractice;

public enum PhoneVerificationStep
{
    Normal, // create account/registration flows
    OldNumber, //Changing number:verify the current one
    NewNumber //changing number: verify the new one
}
public partial class VerifyPhoneNumber : ContentPage
{
    private int countdown = 30;

    private readonly bool _isLogin;
    private readonly PhoneVerificationStep _step;
    private readonly string _phoneNumber;
    private IDispatcherTimer? countdownTimer;

    private bool _hasShownPopup = false;
    public VerifyPhoneNumber(bool isLogin = false, string phoneNumber = "", PhoneVerificationStep step = PhoneVerificationStep.Normal)
    {
        InitializeComponent();
            
        _isLogin = isLogin;
        _step = step;

        TitleLabel.Text = step switch
        {
            PhoneVerificationStep.OldNumber => "Verify Old Phone Number",
            PhoneVerificationStep.NewNumber => "Verify New Phone Number",
            _ => "Verify Phone Number"
        };
        

        if (string.IsNullOrEmpty(phoneNumber))
            phoneNumber = Preferences.Get("RegisteredPhoneNumber", string.Empty);
    else if (step == PhoneVerificationStep.Normal)
            Preferences.Set("RegisteredPhoneNumber", phoneNumber);

        _phoneNumber = phoneNumber;

        PhoneNumber.Text = FormatPhoneNumber(phoneNumber);

    }
    private static string FormatPhoneNumber(string phoneNumber)
    {
        string number = phoneNumber?.Trim() ?? "";
    // +234 is static infront
        if (number.StartsWith("0"))
        number = number.Substring(1);

        return string.IsNullOrEmpty(number) ? "," : $"+234{number},";
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasShownPopup)
        return;

        _hasShownPopup = true;

        // Show the popup immediately
        VerificationPopup.IsVisible = true;

        StartCountdown();


        // Keep it visible for 1.5 seconds
        await Task.Delay(3500);

        // Hide the popup
        VerificationPopup.IsVisible = false;
    }

    //Back Button
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    //OTP input
    private readonly CancellationTokenSource _cts = new();
    private async void OnCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        if (e.NewTextValue?.Length > 1)
            return;

        try
        {

            // If a number was entered
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                if (entry == Code1)
                {
                    Code2.Focus();
                }
                else if (entry == Code2)
                {
                    Code3.Focus();
                }
                else if (entry == Code3)
                {
                    Code4.Focus();
                }
                else if (entry == Code4)
                {
                    Code5.Focus();
                }
                else if (entry == Code5)
                {
                    Code6.Focus();
                }

                else if (entry == Code6)
                {
                    // Hide keyboard
                    await Code6.HideKeyboardAsync(CancellationToken.None);

                    if (Loading.IsLoading)
                        return;
                    await Loading.ShowAsync("verifying code...");

                    await Task.Delay(500);
                    
                    //Changing number, step 1:old number verified - enter the new number
                    if(_step == PhoneVerificationStep.OldNumber)
                    {
                        await Navigation.PushAsync(new NewPhoneNumberPage());
                        return;
                    }
                    //Changing number, step 2: new number verified, save it and show success page
                    if (_step == PhoneVerificationStep.NewNumber)
                    {
                        Preferences.Set("RegisteredPhoneNumber", _phoneNumber);

                        var successView = new PhoneNumberVerified();
                        successView.Configure(
                            imageSource: "successful.png",
                            title: "Successful",
                            subtitle: "You have successfully updated your phone number.",
                            buttonText: "Okay",
                            onContinue: async () => await Navigation.PopToRootAsync());

                        var successPage = new ContentPage 
                        { 
                            Content = successView,
                            BackgroundColor = Colors.White };

                        NavigationPage.SetHasNavigationBar(successPage, false);
                        Shell.SetNavBarIsVisible(successPage, false);
                        await Navigation.PushAsync(successPage);
                        return;

                    }
                    if (_isLogin)
                    {
                        await Navigation.PushAsync(new ConfirmAccountDetails());

                        return;
                    }
                    VerifyPhoneNumberPage.IsVisible = false;

                    var verifiedView = new PhoneNumberVerified();
                    verifiedView.Configure(
                        imageSource: "phone_verified.png",
                        title: "Phone Number Verified\nSuccessfully",
                        subtitle: "Click next to continue account setup",
                        buttonText: "Next",
                        onContinue: async () => await Navigation.PushAsync(new PersonalInformation())
                    );

                    var page = new ContentPage
                    {
                        Content = verifiedView,
                        BackgroundColor = Colors.White
                    };
                    NavigationPage.SetHasNavigationBar(page, false);
                    Shell.SetNavBarIsVisible(page, false);
                    await Navigation.PushAsync(page);
                }
            }

            // If the number was deleted
            else if (string.IsNullOrEmpty(e.NewTextValue))
            {
                if (entry == Code2)
                {
                    Code1.Focus();
                }
                else if (entry == Code3)
                {
                    Code2.Focus();
                }
                else if (entry == Code4)
                {
                    Code3.Focus();
                }
                else if (entry == Code5)
                {
                    Code4.Focus();
                }
                else if (entry == Code6)
                {
                    Code5.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OTP entry error: {ex}");
        }
        finally                                        
        {
            if (Loading.IsLoading)
                await Loading.HideAsync();
        }
    }

    // Keeps the cursor in the correct position
    private void OnCodeFocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.CursorPosition = entry.Text?.Length ?? 0;
        }
    }
    private async void OnResendOtpTapped(object sender, TappedEventArgs e)
    {
        // Only allow Resend when countdown has reached 0
        if (countdown > 0 || Loading.IsLoading)
            return;

        await Loading.ShowAsync("Sending a new code...");
        await Task.Delay(500);
        await Loading.HideAsync();
        // Start the countdown again
        StartCountdown();
    }
    //Start countdown
    private void StartCountdown()
    {
        // Reset countdown
        countdown = 30;
        // Reset to the original color in case this is a re-run after a resend
        ResendOtp.TextColor = Color.FromArgb("#666666");
        // Show 30 seconds immediately
        CountdownSpan.Text = $"({countdown}s)";
            
        countdownTimer?.Stop();

        // Create timer
        countdownTimer = Dispatcher.CreateTimer();
          

        // Run every 1 second
        countdownTimer.Interval = TimeSpan.FromSeconds(1);

        countdownTimer.Tick -= CountdownTimer_Tick;
        countdownTimer.Tick += CountdownTimer_Tick;

        countdownTimer.Start();
    }


    // =========================
    // COUNTDOWN TIMER
    // =========================
    private void CountdownTimer_Tick(object? sender, EventArgs e)
    {
        countdown--;

        CountdownSpan.Text = $"({countdown}s)";

        if (countdown <= 0)
        {
            countdown = 0;

            CountdownSpan.Text = "(0s)";

            // Change Resend to green
            ResendOtp.TextColor = Color.FromArgb("#009933");

            // Stop countdown
            countdownTimer?.Stop();

        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        countdownTimer?.Stop();
        _cts.Cancel();
    }
    private void OnCallIconTapped(object? sender, EventArgs e)
    {
            StaticButtons.IsVisible = true;
    }
    private void OnCancelTapped(object sender, TappedEventArgs e)
    {
        // Hide both static borders
        StaticButtons.IsVisible = false;
    }
    
}