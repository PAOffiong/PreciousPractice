using CommunityToolkit.Maui.Core.Platform;
namespace PreciousPractice;

public partial class SetTransactionPin : ContentPage
{
	public SetTransactionPin()
	{
		InitializeComponent();
		CheckPINMatch();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnCodeTextChanged(object sender, TextChangedEventArgs e)
	{
		if (sender is not Entry entry)
			return;
		if (e.NewTextValue?.Length>1)
			return;
		try
		{
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
					Code7.Focus();
				}
				else if (entry == Code7)
				{
					Code8.Focus();
				}
				else if (entry == Code8)
				{
					await Code8.HideKeyboardAsync(CancellationToken.None);
				}
			}
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
			}
			CheckPINMatch();

		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Otp entry error:{ex}");
		}
	}
	private void CheckPINMatch()
	{
		string setPIN = $"{Code1.Text}{Code2.Text}{Code3.Text}{Code4.Text}";
		string reEnteredPIN = $"{Code5.Text}{Code6.Text}{Code7.Text}{Code8.Text}";

        bool isMatch = setPIN.Length == 4 && setPIN == reEnteredPIN;

		ProceedBorder.IsEnabled = isMatch;
    }
    // No repeated digits (1123) and no consecutive digits (1234 or 4321)
    private static bool IsAllowedPin(string pin)
    {
        bool hasRepeats = pin.Distinct().Count() != pin.Length;
        bool isConsecutive = "0123456789".Contains(pin) || "9876543210".Contains(pin);

        return !hasRepeats && !isConsecutive;
    }

    private void OnCodeFocused(object sender, FocusEventArgs e)
	{
		if (sender is Entry entry)
		{
			entry.CursorPosition = entry.Text?.Length ?? 0;
		}
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
            await DisplayAlert("Invalid PIN",
                "Your PIN can't have repeated digits (like 1123) or consecutive numbers (like 1234 or 4321).", "OK");
            Code1.Text = Code2.Text = Code3.Text = Code4.Text = string.Empty;
            Code5.Text = Code6.Text = Code7.Text = Code8.Text = string.Empty;
            Code1.Focus();
            return;
        }
        try
        {
			ProceedTxt.IsVisible = false;
			ProceedIndicator.IsVisible = true;
			ProceedIndicator.IsRunning = true;

			await Loading.ShowAsync("Setting pin...");
			await Task.Delay(500);
            await SecureStorage.Default.SetAsync("Pin", setPIN);

            var verifiedView = new PhoneNumberVerified();
			verifiedView.Configure(
				imageSource: "successful.png",
				title: "PIN Changed",
				subtitle: "",
				buttonText: "Return",
                onContinue: async () =>
                {
                    // Remove this PIN page from underneath the success page...
                    Navigation.RemovePage(this);
                    // ...then close the success page: back to PIN Management
                    await Navigation.SafePopAsync();
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
}