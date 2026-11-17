using CommunityToolkit.Maui.Core.Platform;

namespace PreciousPractice;

public partial class RegisterDeviceLogin : ContentPage
{
	public RegisterDeviceLogin()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private void CheckLoginButton()
    {
        string login = RegisterLogin.Text?.Trim() ?? "";
        string password = PasswordEntry.Text?.Trim() ?? "";

        bool isValid = !string.IsNullOrWhiteSpace(login)
                    && password.Length == 6
                    && password.All(char.IsDigit);

        LoginButton.IsEnabled = isValid;
    }
    private void OnPhoneOrEmailTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckLoginButton();

        if (sender is not Entry entry)
            return;

        string text = entry.Text ?? "";

        if (string.IsNullOrEmpty(text))       
            entry.Keyboard = Keyboard.Default;
        // If the user starts typing numbers
        else if (char.IsDigit(text[0]))    
            entry.Keyboard = Keyboard.Telephone;       
        // If the user starts typing letters
        else     
            entry.Keyboard = Keyboard.Email;
    }
    private async void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
    {
       CheckLoginButton();
        string password = PasswordEntry.Text?.Trim() ?? "";

        if (password.Length == 6 && password.All(char.IsDigit))
        {
            PasswordEntry.Unfocus();
            await PasswordEntry.HideKeyboardAsync(CancellationToken.None);
            
        }
    }
    private async void NavToRegisterForgotPassword(object sender, TappedEventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () =>new RegisterForgotPassword());
    }
    private async void NavToHomePage(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        string password = PasswordEntry.Text?.Trim() ?? "";
        try
        {
            LoginTxt.IsVisible = false;

            LoginIndicator.IsVisible = true;
            LoginIndicator.IsRunning = true;

            await Loading.ShowAsync("Logging in...");
            await Task.Delay(500);

            // Check the password against the one saved at sign-up
            string? savedPassword = await SecureStorage.Default.GetAsync("Password");

            if (password != savedPassword)
            {
                await DisplayAlert("Login Failed", "Incorrect login details. Please try again.", "OK");
                PasswordEntry.Text = string.Empty;
                return;   // finally still runs: overlay and spinner are switched off
            }

            // Correct: replace the login page with Home, so back can't return here
            Application.Current!.Windows[0].Page = new NavigationPage(new HomePage());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Could not log in.Please try again", "OK");
        }
        finally
        {
            await Loading.HideAsync();
            // Stop the spinner
            LoginTxt.IsVisible = true;
            LoginIndicator.IsRunning = false;
            LoginIndicator.IsVisible = false;


        }
    }
    private async void NavToCreateNewAccount(object sender, EventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () => new CreateNewAccount());
    }

}
    
