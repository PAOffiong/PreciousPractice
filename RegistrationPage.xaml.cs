using CommunityToolkit.Maui.Core.Platform;

namespace PreciousPractice;

public partial class RegistrationPage : ContentPage
{
    private string SavedPhoneNumber = "";
	public RegistrationPage()
	{
		InitializeComponent();

        SavedPhoneNumber = Preferences.Get("RegisteredPhoneNumber", "");

        CheckRegistrationDetails();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private async void OnRegistrationTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckRegistrationDetails();
       
        if (sender is not Entry entry)
            return;

        string text = entry.Text?.Trim() ?? "";

        // Hide keyboard after 10 digits in Phone Number
        if (entry == PhoneNumberEntry &&
            text.Length == 10 &&
            text.All(char.IsDigit))
        {
            await entry.HideKeyboardAsync();
            entry.Unfocus();
        }

        // Hide keyboard after 10 digits in Account Number
        if (entry == AccountNumberEntry &&
            text.Length == 10 &&
            text.All(char.IsDigit))
        {
            await entry.HideKeyboardAsync(CancellationToken.None);
            entry.Unfocus();
        }
    }

    private void CheckRegistrationDetails()
    {
        string phoneNumber = PhoneNumberEntry.Text?.Trim() ?? "";
        string accountNumber = AccountNumberEntry.Text?.Trim() ?? "";

        // Phone number must be exactly 10 digits
        bool phoneNumberValid = IsValidPhoneNumber(phoneNumber);

        // Account number must be exactly 10 digits
        bool accountNumberValid =
            accountNumber.Length == 10 &&
            accountNumber.All(char.IsDigit);

        // No number saved yet (new install, or after Delete Profile): any valid number is fine.
        // A number was saved before: it must match.
        bool phoneMatches = string.IsNullOrEmpty(SavedPhoneNumber) || phoneNumber == SavedPhoneNumber;

        bool isValid =
            phoneNumberValid &&
            accountNumberValid &&
            phoneMatches;

        RegisterButton.IsEnabled = isValid;
        RegisterButton.Opacity = isValid ? 1.0 : 0.5;
    }
    private static bool IsValidPhoneNumber(string? phoneNumber)
    {
        string digits = new string(
            (phoneNumber ?? string.Empty)
                .Where(char.IsDigit)
                .ToArray());

        return digits.Length == 10;
    }
    private async void NavToVerifyPhoneNumber(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {    //Save phone number during registration
            string phoneNumber = PhoneNumberEntry.Text?.Trim() ?? "";
   
            RegisterTxt.IsVisible = false;
            RegisterIndicator.IsVisible = true;
            RegisterIndicator.IsRunning = true;

            await Loading.ShowAsync("Verifying Phone number...");
            await Task.Delay(500);

            await Navigation.PushAsync(new VerifyPhoneNumber(isLogin:true,phoneNumber:phoneNumber));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlert("Error", "Failed to continue to sign up.", "OK");
        }
        finally
        {
            await Loading.HideAsync();
            // Show text again
            RegisterTxt.IsVisible = true;

            // Hide spinner
            RegisterIndicator.IsRunning = false;
            RegisterIndicator.IsVisible = false;
        }
    }
    private async void NavToRegisterDeviceLogin(object sender,TappedEventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () => new RegisterDeviceLogin());
    }
}
	