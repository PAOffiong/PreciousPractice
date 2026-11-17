

namespace PreciousPractice;

public partial class RegisterForgotPassword : ContentPage
{
	public RegisterForgotPassword()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, EventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private async void OnEmailResetTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckEmail();
    }

    private bool CheckEmail()
    {
        string email = EmailEntry.Text?.Trim() ?? "";

        bool emailIsValid =
            !string.IsNullOrWhiteSpace(email) &&
            email.Contains("@") &&
            email.Contains(".") &&
            email.IndexOf("@") < email.LastIndexOf(".");

        VerificationCodeButton.IsEnabled = emailIsValid;

        return emailIsValid;
    }
    private async void NavToEmailVerified(object sender, EventArgs e)
    {
        if (Loading.IsLoading)
            return;

        if (!CheckEmail())
            return;
        EmailEntry.Unfocus();
        try
        {
            VerificationCodeTxt.IsVisible = false;


            VerificationCodeIndicator.IsVisible = true;
            VerificationCodeIndicator.IsRunning = true;

            await Loading.ShowAsync("Verifying mail...");
            await Task.Delay(500);

            await Navigation.PushAsync(new EmailVerified(true));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);


            await DisplayAlert("Error", "Failed to open Registration Certificate.", "OK");
        }
        finally
        {
            await Loading.HideAsync();

            VerificationCodeTxt.IsVisible = true;

            // Hide spinner
            VerificationCodeIndicator.IsRunning = false;
            VerificationCodeIndicator.IsVisible = false;
        }
    }
    private async void NavToCreateNewAccount(object sender, EventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () => new CreateNewAccount());
    }
}
	
