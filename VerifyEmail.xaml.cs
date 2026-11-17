using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Extensions;
using Microsoft.Maui.ApplicationModel;
namespace PreciousPractice;

public partial class VerifyEmail : ContentPage
{
    private int countdown = 30;
    private IDispatcherTimer? countdownTimer;

    public VerifyEmail()
	{
		InitializeComponent();
	}
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
                    await Loading.ShowAsync("Verifying code...");  
                    await Task.Delay(500);

                    var verifiedView = new PhoneNumberVerified();
                    verifiedView.Configure(
                        imageSource: "email_verified_icon.png",
                        title: "Email Verified Successfully",
                        subtitle: "Click next to setup password",
                        buttonText: "Next",
                        onContinue: async () => await Navigation.PushAsync(new SetPassword())
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
            }   // If the number was deleted
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
          System.Diagnostics.Debug.WriteLine($"Otp entry error:{ex}");
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
    private void OnEmailResendTapped(object sender, EventArgs e)
    {
        if (countdown > 0)
            return;

        StartCountdown();
    }
    //Start countdown
    private void StartCountdown()
    {
        // Reset countdown
        countdown = 30;

        EmailResend.TextColor = Color.FromArgb("#666666");

        // Show 30 seconds immediately
        CountdownSpan.Text = $"({countdown}s)";

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

        // Countdown finished
        if (countdown <= 0)
        {
            countdownTimer?.Stop();

            CountdownSpan.Text = "(0s)";
            EmailResend.TextColor = Color.FromArgb("#009933");

            countdownTimer.Stop();
        }
    }


    // =========================
    // PAGE APPEARING
    // =========================
    protected override void OnAppearing()
    {
        base.OnAppearing();

        StartCountdown();
    }


    // =========================
    // STOP TIMER WHEN LEAVING PAGE
    // =========================
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        countdownTimer?.Stop();
        _cts.Cancel();
    }
}
        
    