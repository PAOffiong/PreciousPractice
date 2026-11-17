using CommunityToolkit.Maui.Core.Platform;
using PreciousPractice.Helpers;
using System.ComponentModel.Design;


namespace PreciousPractice;


public partial class SetPassword : ContentPage
{
    private readonly bool _isLogin;
    private bool _isProcessing;
    public SetPassword(bool isLogin = false)
    {
        InitializeComponent();

        _isLogin = isLogin;

        showPasswordBorder.IsEnabled = false;
        showPasswordBorder.Opacity = 0.5;

    }
    private async void OnBackIconTapped(object sender, EventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private readonly CancellationTokenSource _cts = new();
    private async void OnCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
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

                    await Code6.HideKeyboardAsync(_cts.Token);
                }


                else if (entry == Code7)
                {
                    Code8.Focus();
                }
                else if (entry == Code8)
                {
                    Code9.Focus();
                }
                else if (entry == Code9)
                {
                    Code10.Focus();
                }
                else if (entry == Code10)
                {
                    Code11.Focus();
                }
                else if (entry == Code11)
                {
                    Code12.Focus();
                }
                else if (entry == Code12)
                {

                    await Code12.HideKeyboardAsync(_cts.Token);
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
                else if (entry == Code7)
                {
                    Code6.Focus();
                }
                else if (entry == Code8)
                {
                    Code7.Focus();
                }
                else if (entry == Code9)
                {
                    Code8.Focus();
                }
                else if (entry == Code10)
                {
                    Code9.Focus();
                }
                else if (entry == Code11)
                {
                    Code10.Focus();
                }
                else if (entry == Code12)
                {
                    Code11.Focus();
                }
            }
            CheckPasswordsMatch();
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OTP entry error:{ex}");
        }
    }
    
    private void CheckPasswordsMatch()
    {
        string setPassword = $"{Code1.Text}{Code2.Text}{Code3.Text}{Code4.Text}{Code5.Text}{Code6.Text}";

        string reEnteredPassword = $"{Code7.Text}{Code8.Text}{Code9.Text}{Code10.Text}{Code11.Text}{Code12.Text}";

        if (setPassword.Length == 6 &&
            reEnteredPassword.Length == 6 &&
            setPassword == reEnteredPassword)
        {
            showPasswordBorder.IsEnabled = true;
            showPasswordBorder.Opacity = 1.0;
        }
        else
        {
            showPasswordBorder.IsEnabled = false;
            showPasswordBorder.Opacity = 0.5;
        }
    }

    private void OnCodeFocused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.CursorPosition = entry.Text?.Length ?? 0;
        }
    }
    private async void OnShowPasswordTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;

        // Make sure passwords match before continuing
        string setPassword =
            $"{Code1.Text}{Code2.Text}{Code3.Text}{Code4.Text}{Code5.Text}{Code6.Text}";

        string reEnteredPassword =
            $"{Code7.Text}{Code8.Text}{Code9.Text}{Code10.Text}{Code11.Text}{Code12.Text}";

        if (setPassword.Length != 6 ||
            reEnteredPassword.Length != 6 ||
            setPassword != reEnteredPassword)
        {
            return;
        }
           await SecureStorage.Default.SetAsync("Password", setPassword);
        try
        {
            _isProcessing = true;

            // Disable button
            showPasswordBorder.IsEnabled = false;

            // Hide "Next"
            SetPasswordTxt.IsVisible = false;

            // Show loading indicator
            SetPasswordIndicator.IsVisible = true;
            SetPasswordIndicator.IsRunning = true;

            await Loading.ShowAsync("Creating password...");
            await Task.Delay(500);

            if (_isLogin)
            {
                await Navigation.PushAsync(new SuccessPage());

                return;
            }
            // Create the verification view
            var verifiedView = new PhoneNumberVerified();
            verifiedView.Configure(
                imageSource: "password_created_icon.png",
                title: "Password Created Successfully",
                subtitle: "Click the button below to generate Account Number",
                buttonText: "Generate Acc Number",
                onContinue: async () =>
                {
                    string accountNumber = GenerateAccountNumber();
                    Preferences.Set("AccountNumber", accountNumber);


                    await Navigation.PushAsync(new StartBankingNow());
                });

            
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
        finally
        {

            await Loading. HideAsync();
           
            SetPasswordTxt.IsVisible = true;
            SetPasswordIndicator.IsRunning = false;
            SetPasswordIndicator.IsVisible = false;

            showPasswordBorder.IsEnabled = true;

            _isProcessing = false;
        }
    }
    private static string GenerateAccountNumber()
    {
        //temporary account number
        return DefaultAccountNumber.TemporaryAccountNumber;
        //10 digits starting with 8, matching the account number on xaml

       // return "8" + Random.Shared.Next(0, 1_000_000_000).ToString("D9");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        
        if (!_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }
    }
}