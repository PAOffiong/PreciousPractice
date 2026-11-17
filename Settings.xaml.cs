
using System.Security.Cryptography;
#if ANDROID
using Android.Content;
using Android.Hardware.Biometrics;
#endif

namespace PreciousPractice;

public partial class Settings : ContentPage
{
    private const string BiometricLoginKey = "BiometricLogin";
    private const string BiometricTransAuthKey = "BiometricTransAuth";
    private const string TwoFactorKey = "TwoFactorAuth";

    // Which toggle the deactivate popup is for
    private enum DeactivateTarget { BiometricLogin, BiometricTransAuth, TwoFactor }
    private DeactivateTarget _deactivateTarget;

    private const string ToggleOnImage = "dark_add_beneficiaries.png";
    private const string ToggleOffImage = "light_add_beneficiaries.png";
   
    public Settings()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!IsBiometricSetUp())
        {
            Preferences.Set(BiometricLoginKey, false);
            Preferences.Set(BiometricTransAuthKey, false);
        }
        UpdateToggles();
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnChangePasswordTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new ChangePassword());
	}
	private async void OnPINManagementTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new PINManagement());
	}
	private async Task ShowPopupAsync(VisualElement Overlay, VisualElement Card)
	{
		Overlay.Opacity = 0;
		Card.Opacity = 0;
		Overlay.IsVisible = true;
		Card.IsVisible = true;
		Card.Scale = 0.8;

		await Task.WhenAll(
			Overlay.FadeTo(1, 200),
			Card.FadeTo(1, 200),
			Card.ScaleTo(1, 250, Easing.SpringOut));
	}
	private async Task HidePopupAsync(VisualElement Overlay, VisualElement Card)
	{
		await Task.WhenAll(
			Overlay.FadeTo(0, 200),
			Card.FadeTo(0, 200),
			Card.ScaleTo(0.8, 250, Easing.SpringOut));
		
        Overlay.IsVisible = false;
		Card.IsVisible = false;
	}

    private void UpdateToggles()
    {
        BiometricLoginToggle.Source = Preferences.Get(BiometricLoginKey, false) ? ToggleOnImage : ToggleOffImage;
        BiometricAuthToggle.Source = Preferences.Get(BiometricTransAuthKey, false) ? ToggleOnImage : ToggleOffImage;

        bool twoFactorOn = Preferences.Get(TwoFactorKey, false);
        FactorAuthenticationToggle.Source = twoFactorOn ? ToggleOnImage : ToggleOffImage;
        TwoFactorText.Opacity = twoFactorOn ? 0.4 : 1.0;
    }
    private async void OnBiometricLoginTapped(object sender, TappedEventArgs e)
	{
		if (Preferences.Get(BiometricLoginKey, false))
        {
            await ShowDeactivatePopupAsync(DeactivateTarget.BiometricLogin);
            return;
        }

		// Turning on needs a fingerprint or face set up on the phone
		if (!IsBiometricSetUp())
		{
			await DisplayAlert("Error", "You have not added any Biometric authentication to this device.", "OK");
			return;
		}

		bool confirmed = await AuthenticateWithBiometricAsync();
		if (confirmed)
		{
			Preferences.Set(BiometricLoginKey, true);
			UpdateToggles();
		}
	}
    private async void OnBiometricAuthenticationTapped(object sender, TappedEventArgs e)
    {
        if (Preferences.Get(BiometricTransAuthKey, false))
        {
            await ShowDeactivatePopupAsync(DeactivateTarget.BiometricTransAuth);
            return;
        }

        //biometric login must be turned on first
        if(!Preferences.Get(BiometricLoginKey, false))
        {
            await ShowPopupAsync(BiometricAuthenticationPopupOverlay, BiometricAuthenticationPopupCard);
            return;
        }
        //step 1: the pin page calls TransAuthPinVerified, once the pin is correct
        await Loading.NavigateAsync(Navigation, () => new TransactionPin(onPinVerified: OnTransAuthPinVerifiedAsync));
    }
    //step2: this runs after the pin has been confirmed to be correct
    private async Task OnTransAuthPinVerifiedAsync()
    {
        // close the PIN page and go back to settings
        await Navigation.SafePopAsync();
        //fingerprint sensor shows on setting
        bool confirmed = await AuthenticateWithBiometricAsync();
        if (!confirmed)
            return; // if user cancels or it failed, toggle stays off
        //when fingerprint matches, the authentication popup shows
        await ShowPopupAsync(AuthenticationPopupOverlay, AuthenticationPopupCard);
    }
    private async void OnOkayTapped(object sender, TappedEventArgs e)
    {
        Preferences.Set(BiometricTransAuthKey, true);
        UpdateToggles();

        await HidePopupAsync(AuthenticationPopupOverlay, AuthenticationPopupCard);
    }
    private void OnAuthenticationPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        //does nothing
    }
    private  void OnAuthenticationPopupCardTapped(object sender, TappedEventArgs e)
    {
        //Takes tabs without closing popup
    }
    private async Task ShowDeactivatePopupAsync(DeactivateTarget target)
    {
        _deactivateTarget = target;

        DeactivateMessageLabel.Text = target switch
        {
            DeactivateTarget.BiometricLogin =>
             "Are you sure you want to deactivate\nbiometric for login?(Face ID/Touch ID/ Fingerprint)",
            DeactivateTarget.BiometricTransAuth =>
             "Are you sure you want to deactivate\nbiometric for transactions?(Face ID/Touch ID/ Fingerprint)",
             _ =>
             "Are you sure you want to deactivate\ntwo-factor authentication?"
        };
        ResetPopupButtons();
        await ShowPopupAsync(BiometricLoginPopupOverlay, BiometricLoginPopupCard);
    }
    private async void OnDeactivateTapped(object sender, TappedEventArgs e)
    {
        DeactivateTxt.TextColor = Color.FromArgb("#009933");
        await Task.Delay(150);

        switch (_deactivateTarget)
        {
            case DeactivateTarget.BiometricLogin:
                //login and Trans goes off
                Preferences.Set(BiometricLoginKey, false);
            Preferences.Set(BiometricTransAuthKey, false);
            break;

          case DeactivateTarget.BiometricTransAuth:
            Preferences.Set(BiometricTransAuthKey, false);
            break;

        case DeactivateTarget.TwoFactor:
            Preferences.Set(TwoFactorKey, false);
            break;
        }
        
        UpdateToggles();
        await HidePopupAsync(BiometricLoginPopupOverlay, BiometricLoginPopupCard);
    }
    private async void OnLaterTapped(object sender, TappedEventArgs e)
    {
        LaterTxt.TextColor = Color.FromArgb("#009933");
        await Task.Delay(150);

        await HidePopupAsync(BiometricLoginPopupOverlay, BiometricLoginPopupCard);
    }
    private void OnBiometricLoginPopupCardTapped(object sender, TappedEventArgs e)
    {
        //takes taps without closing popup
    }
    private void ResetPopupButtons()
    {
        bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;

        DeactivateTxt.TextColor = isDark ? Color.FromArgb("#141414") : Colors.White;
        LaterTxt.TextColor = isDark ? Color.FromArgb("#141414"): Colors.White;
    }
    private async void OnGotItTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(BiometricAuthenticationPopupOverlay, BiometricAuthenticationPopupCard);
    }
    private async void OnBiometricAuthenticationPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(BiometricAuthenticationPopupOverlay, BiometricAuthenticationPopupCard);
    }
    private async void OnBiometricAuthenticationPopupCardTapped(object sender, TappedEventArgs e)
    {
        //takes taps without closin the popup
    }
    //create random secret code 
    private static string GenerateSecretCode(int length = 16)
    {// THE only characters accepted
        const string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        char[] code = new char[length];

        for (int i = 0; i < length; i++)
        {
            code[i] = allowed[RandomNumberGenerator.GetInt32(allowed.Length)];
        }

        return new string(code);
    }

    private async void OnFactorAuthenticationTapped(object sender, TappedEventArgs e)
    {
        // Already on: ask before deactivating
        if (Preferences.Get(TwoFactorKey, false))
        {
            await ShowDeactivatePopupAsync(DeactivateTarget.TwoFactor);
            return;
        }

        // Off: start the setup with a fresh secret key
        SecretCode.Text = GenerateSecretCode();
        await ShowPopupAsync(FactorAuthenticationPopupOverlay, FactorAuthenticationPopupCard);
    }
    private async void OnFactorAuthenticationPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(FactorAuthenticationPopupOverlay, FactorAuthenticationPopupCard);
    }
    private void OnFactorAuthenticationPopupCardTapped(object sender, TappedEventArgs e)
    {
        //takes taps without closing the popup
    }
    private async void OnCopyBorderTapped(object sender, TappedEventArgs e)
    {
        string secretCode = SecretCode.Text;
        if (!string.IsNullOrEmpty(secretCode))
        {
            await Clipboard.Default.SetTextAsync(secretCode);
            await DisplayAlert("Copied", "Secret code copied to clipboard", "Ok");
        }
    }
    private async void OnTextAuthenticationBorderTapped(object sender, TappedEventArgs eventArgs)
    {
        try
        {
            AuthenticationTxt.IsVisible = false;
            AuthenticationIndicator.IsVisible = true;
            AuthenticationIndicator.IsRunning = true;

            await HidePopupAsync(FactorAuthenticationPopupOverlay, FactorAuthenticationPopupCard);
            await Task.Delay(500);

            AuthenticatorCodeEntry.Text = string.Empty;
            await ShowPopupAsync(AuthenticationCodePopupOverlay, AuthenticationCodePopupCard);
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Something went wrong.Please try again", "OK");
        }
        finally
        {
            AuthenticationTxt.IsVisible = true;
            AuthenticationIndicator.IsVisible = false;
            AuthenticationIndicator.IsRunning = false;
        }
    }
    
    private async void OnAuthenticationCodePopupOverlay(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(AuthenticationCodePopupOverlay, AuthenticationCodePopupCard);
    }
    private void OnAuthenticationCodePopupCard(object sender, TappedEventArgs e)
    {
        //takes taps without closing the popup
    }
    private async void OnAuthenticatorCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        string code = AuthenticatorCodeEntry.Text?.Trim() ?? "";
        bool isValid = code.Length == 6 && code.All(char.IsDigit);

        AuthenticateBorder.IsEnabled = isValid;
    }
    private async void OnAuthenticateBorderTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {
            AuthenticateTxt.IsVisible = false;
            AuthenticateIndicator.IsVisible = true;
            AuthenticateIndicator.IsRunning = true;

            await Loading.ShowAsync("Verifying code...");
            await Task.Delay(300);

            // Switch Two-Factor on
            Preferences.Set(TwoFactorKey, true);
            UpdateToggles();

            var verifiedView = new PhoneNumberVerified();
            verifiedView.Configure(
                imageSource: "successful.png",
                title: "Authentication Successful",
                subtitle: "Your two-factor authentication setup is complete.",
                buttonText: "Return",
                onContinue: async () => await Navigation.SafePopAsync()
                );

            var page = new ContentPage
            {
                Content = verifiedView,
                BackgroundColor = Colors.White
            };

            // Hide navigation bar
            NavigationPage.SetHasNavigationBar(page, false);
            Shell.SetNavBarIsVisible(page, false);


            await Navigation.PushAsync(page);

            AuthenticationCodePopupOverlay.IsVisible = false;
            AuthenticationCodePopupCard.IsVisible = false;
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlert("Error", "Something went wrong.Please try again", "OK");
        }
        finally
        {
            await Loading.HideAsync();

            AuthenticateTxt.IsVisible = true;
            AuthenticateIndicator.IsVisible = false;
            AuthenticateIndicator.IsRunning = false;
        }
    }
    private async void OnLogOutTapped(object sender, TappedEventArgs e)
    {
        await ShowPopupAsync(LogOutPopupOverlay, LogOutPopupCard);
    }

    private async void OnLogOutPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(LogOutPopupOverlay, LogOutPopupCard);
    }
    private void OnLogOutPopupCardTapped(object sender, TappedEventArgs e)
    {
        //takes taps without closing popup
    }
    private async void OnNoBorderTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(LogOutPopupOverlay, LogOutPopupCard);
    }
    private async void OnYesBorderTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {
            YesTxt.IsVisible = false;
            YesIndicator.IsVisible = true;
            YesIndicator.IsRunning = true;

            await Loading.ShowAsync("Logging out...");
            await Task.Delay(500);
            Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Something went wrong. Try again", "OK");
        }
        finally
        {
            await Loading.HideAsync();

            YesTxt.IsVisible = true;
            YesIndicator.IsVisible = false;
            YesIndicator.IsRunning = false;
        }
    }
    private static bool IsBiometricSetUp()
    {
#if ANDROID
        if (!OperatingSystem.IsAndroidVersionAtLeast(29))
            return false;

        var manager = Platform.AppContext.GetSystemService(Context.BiometricService) as BiometricManager;
        return manager?.CanAuthenticate() == BiometricCode.Success;
#else
         return false;
#endif
    }
	//android fingerprint
    private static Task<bool> AuthenticateWithBiometricAsync()
	{
#if ANDROID
		var activity = Platform.CurrentActivity;
		if (activity is null || !OperatingSystem.IsAndroidVersionAtLeast(28))
			return Task.FromResult(false);

		var listener = new FingerprintListener();

		new BiometricPrompt.Builder(activity)
			.SetTitle("Authentication")
			.SetSubtitle("Authenticate using Biometric")
			.SetNegativeButton("CANCEL", activity.MainExecutor, listener)
			.Build()
			.Authenticate(new Android.OS.CancellationSignal(), activity.MainExecutor, listener);

		return listener.Result.Task;
#else
    return Task.FromResult(false);
#endif
	}
#if ANDROID
    // Listens for the prompt's answer: matched, error, or CANCEL
    private class FingerprintListener : BiometricPrompt.AuthenticationCallback, IDialogInterfaceOnClickListener
    {
        public TaskCompletionSource<bool> Result { get; } = new();

        // if Fingerprint matched
        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult? result)
            => Result.TrySetResult(true);

        
        public override void OnAuthenticationError(BiometricErrorCode errorCode, Java.Lang.ICharSequence? errString)
            => Result.TrySetResult(false);

        // Cancel tapped
        public void OnClick(IDialogInterface? dialog, int which)
            => Result.TrySetResult(false);

    }
#endif
	}

	
