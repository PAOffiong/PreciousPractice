

namespace PreciousPractice;

public partial class NewPhoneNumberPage : ContentPage
{
	public NewPhoneNumberPage()
	{
		InitializeComponent();
        UpdateContinueBorder();
    }
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private void OnPhoneNumberTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateContinueBorder();
    }
    private void UpdateContinueBorder()
	{
        
        string phoneNumber = PhoneNumberEntry.Text?.Trim() ?? "";
        // Phone number must be exactly 10 digits
        bool isValid = IsValidPhoneNumber(phoneNumber);


        ContinueBorder.IsEnabled = isValid;
        ContinueBorder.Opacity = isValid ? 1.0 : 0.5;
    }
    private static bool IsValidPhoneNumber(string? phoneNumber)
    {
        string number = phoneNumber?.Trim() ?? "";
        return number.Length == 10 && number.All(char.IsDigit);
    }
    private async void OnContinueBorderTapped(object sender, TappedEventArgs e)
	{
        if (Loading.IsLoading)
            return;
        string newNumber = Normalize(PhoneNumberEntry.Text);
        string currentNumber = Normalize(Preferences.Get("RegisteredPhoneNumber", string.Empty));

        if (newNumber.Length != 10 || !newNumber.All(char.IsDigit))
        {
            await DisplayAlert("Invalid Number", "Please enter a valid 10-digits", "OK");
            return;
        }
        if (newNumber == currentNumber)
        {
            await DisplayAlert("Same Number", "This is already your phone number. Please enter a new one.", "OK");
            return;
        }

        try
        {
            ContinueTxt.IsVisible = false;
            ContinueIndicator.IsVisible = true;
            ContinueIndicator.IsRunning = true;

            await Loading.ShowAsync("Please wait...");
            
            await Loading.NavigateAsync(Navigation, () => new VerifyPhoneNumber(phoneNumber: newNumber,
                  step: PhoneVerificationStep.NewNumber));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Not Valid", "OK");

        }
        finally
        {
            await Loading.HideAsync();

            ContinueTxt.IsVisible = true;
            ContinueIndicator.IsVisible = false;
            ContinueIndicator.IsRunning = false;
        }
    }
    // "08012345678" and "8012345678" are the same number: compare them the same way
    private static string Normalize(string? number)
    {
        string digits = number?.Trim() ?? "";
        return digits.StartsWith("0") ? digits.Substring(1) : digits;
    }
}