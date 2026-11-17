using CommunityToolkit.Maui.Core.Platform;

namespace PreciousPractice;

public partial class MorePage : ContentPage
{
	private const string UssdEnabledKey = "UssdEnabled";
	private const string UssdPinKey = "UssdPin";

	private const string ToggleOffImage = "light_add_beneficiaries.png";
	private const string ToggleOnImage = "dark_add_beneficiaries.png";

	public MorePage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		string fullName = Preferences.Get( "FullName", "Janet Doe Kehnide");
		string firstName = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";

		UserName.Text = string.IsNullOrEmpty(firstName)? "Hi" : $"Hi, {firstName}";

		UpdateToggleImage();
    }
	private async void OnSettingsTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new Settings());
	}
	private async void OnTransactionHistoryTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new Transactions());
	}
	private async void OnBVNTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () =>new VerifyBVN());
	}
	private async void OnCustomerServiceTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new CustomerService());
	}
	//is ussd actually switched on? the true value
	private static bool IsUssdEnabled => Preferences.Get(UssdEnabledKey, false);
    //draws the toggle
    private void UpdateToggleImage()
    {
        UssdToggle.Source = IsUssdEnabled
            ? "dark_add_beneficiaries.png"    // on
            : "light_add_beneficiaries.png";  // off
    }
    private async void OnToggleImageTapped(object sender, TappedEventArgs e)
	{
		//Already on:offer to switch it off
		if (IsUssdEnabled)
		{
			bool turnOff = await DisplayAlert("Disable USSD", "Do you want to turn off banking?", "Turn Off", "Cancel");

			if (turnOff)
			{
				Preferences.Set(UssdEnabledKey, false);
				SecureStorage.Default.Remove(UssdPinKey);
				UpdateToggleImage();
			}
			return;
		}
		//off: show it as "on" while the user sets a pin
		UssdToggle.Source = ToggleOnImage;

		
		await ShowPopupAsync(UssdPopupOverlay, UssdPopupCard);
		Code1.Focus();
	}
    // No repeated digits (1123) and no consecutive digits (1234 or 4321)
    private static bool IsAllowedPin(string pin)
    {
        bool hasRepeats = pin.Distinct().Count() != pin.Length;
        bool isConsecutive = "0123456789".Contains(pin) || "9876543210".Contains(pin);

        return !hasRepeats && !isConsecutive;
    }
    //if user closes popup without completing all the requirements
    private async Task CancelUssdPopupAsync()
	{
		await HidePopupAsync(UssdPopupOverlay, UssdPopupCard);
		UpdateToggleImage();
	}
	private async void OnUssdPopupOverlayTapped(object sender, TappedEventArgs e)
	{
		await CancelUssdPopupAsync();
	}
	private void OnUssdPopupCardTapped(object sender, TappedEventArgs e)
	{
		//rakes taps without closing popup
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
			Card.FadeTo (1, 200),
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
	private void CheckPINMatch()
	{
		string setPIN = $"{Code1.Text}{Code2.Text}{Code3.Text}{Code4.Text}";
		string reEnterPIN = $"{Code5.Text}{Code6.Text}{Code7.Text}{Code8.Text}";

		bool isMatch = setPIN.Length == 4 && reEnterPIN == setPIN;

		ProceedBorder.IsEnabled = isMatch;
	}
    private async void OnCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;
        if (e.NewTextValue?.Length > 1)
			return;
		try
		{
			if (!string.IsNullOrEmpty(e.NewTextValue))
			{
				if (sender == Code1)
				{
					Code2.Focus();
				}
				else if (sender == Code2)
				{
					Code3.Focus();
				}
				else if (sender == Code3)
				{
					Code4.Focus();
				}
				else if (sender == Code4)
				{
					Code5.Focus();
				}
				else if (sender == Code5)
				{
					Code6.Focus();
				}
				else if (sender == Code6)
				{
					Code7.Focus();
				}
				else if (sender == Code7)
				{
					Code8.Focus();
				}
				else if (sender == Code8)
				{
					await Code8.HideKeyboardAsync(CancellationToken.None);
				}
			}
			else if (string.IsNullOrEmpty(e.NewTextValue))
			{
				if (sender == Code2)
				{
					Code1.Focus();
				}
				else if (sender == Code3)
				{
					Code2.Focus();
				}
				else if (sender == Code4)
				{
					Code3.Focus();
				}
				else if (sender == Code5)
				{
					Code4.Focus();
				}
				else if (sender == Code6)
				{
					Code5.Focus();
				}
				else if (sender == Code7)
				{
					Code6.Focus();
				}
				else if (sender == Code8)
				{
					Code7.Focus();
				}
			}
			CheckPINMatch();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Otp entrt erroe:{ex}");
		}
	}
	private void OnCodeFocused(object sender, FocusEventArgs e)
	{
		if (sender is Entry entry)
		{
			entry.CursorPosition = entry.Text?.Length ?? 0;
        }
	}
	private async void OnClearTapped(object sender, TappedEventArgs e)
	{
		await CancelUssdPopupAsync();
	}
    protected override bool OnBackButtonPressed() //phone back button
    {
		if (Loading.IsLoading)
			return true; //saving:IGNORE THE BACK BUTTON
        if (UssdPopupOverlay.IsVisible)
        {
            _ = CancelUssdPopupAsync();
            return true;   // don't leave the page
        }

        return base.OnBackButtonPressed();
    }
    private async void OnProceedBorderTapped(object sender, TappedEventArgs e)
    {
		if (Loading.IsLoading)
			return;
        string setPIN = $"{Code1.Text}{Code2.Text}{Code3.Text}{Code4.Text}";
        string reEnteredPIN = $"{Code5.Text}{Code6.Text}{Code7.Text}{Code8.Text}";

        if (setPIN.Length != 4 || setPIN != reEnteredPIN)
        {
            await DisplayAlert("PINs Don't Match", "Please make sure both PINs are the same.", "OK");
            return;
        }
        if (!IsAllowedPin(setPIN))
        {
            await DisplayAlert("Invalid PIN", "Your PIN can't be consecutive numbers (like 1234 or 4321).", "OK");
            Code1.Text = Code2.Text = Code3.Text = Code4.Text = string.Empty;
            Code5.Text = Code6.Text = Code7.Text = Code8.Text = string.Empty;
            CheckPINMatch();
            Code1.Focus();
            return;
        }
        try
        {
            ProceedTxt.IsVisible = false;
            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

			await Loading.ShowAsync("Creating USSD Profile...");
            await Task.Delay(500);
            // 1. Save the USSD PIN and switch USSD on for real
            await SecureStorage.Default.SetAsync(UssdPinKey, setPIN);
            Preferences.Set(UssdEnabledKey, true);

            // 2. Close the popup; the toggle now stays on
            await HidePopupAsync(UssdPopupOverlay, UssdPopupCard);
            UpdateToggleImage();

            var verifiedView = new PhoneNumberVerified();
            verifiedView.Configure(
                imageSource: "successful.png",
                title: "USSD profile created successfully",
                subtitle: "",
                buttonText: "Okay",
                onContinue: async () =>  await Navigation.SafePopAsync()
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
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Something went wrong. Please try again", "OK");
        }
        finally
        {
			await Loading.HideAsync();
            
			ProceedTxt.IsVisible = true;
            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
    }
    private async void OnThemeTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () =>new ThemePage());
	}
	private async void OnAboutTapped(object sender, TappedEventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new AboutPage());
	}	
}