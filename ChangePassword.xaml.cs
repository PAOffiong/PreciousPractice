using System.Runtime.CompilerServices;

namespace PreciousPractice;

public partial class ChangePassword : ContentPage
{
	private const int PasswordLength = 6; //valid password must be 6
	private const string PasswordKey = "Password"; //used to retrieve password from storage
	public ChangePassword()
	{
		InitializeComponent();
		UpdateConfirmBorder(); 
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private void OnEyeIconTapped(object sender, TappedEventArgs e)
	{
		if (sender is not Image eyeIcon || e.Parameter is not Entry entry) //checks if wht was tapped was an imag stored in a variable named "eyeIcon" also checks if it is an entry, 
            //if true, the method runs and then stops.
			return;
		entry.IsPassword = !entry.IsPassword; //before tapping, IsPassword= true because it was hidden and after tapping, flase since text can be seen. Vice versa
		entry.CursorPosition = entry.Text?.Length ?? 0; //helps keep cursor at the end of text

		bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
		bool isShowing = !entry.IsPassword;

		eyeIcon.Source = isShowing
			? (isDark ? "eye_img.png" : "black_eye_ion.png")
			: (isDark ? "closed_eye.png" : "dark_closed_eye.png");
	}
	private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
	{
		UpdateConfirmBorder();
	}
	private static bool IsSixDigits(string? text)
	{
		string value = text ?? ""; //if text is empty, value becomes an empty string.otherwise, it uses the supplied text.
		return value.Length == PasswordLength && value.All(char.IsDigit);//confirm that the text is 6 digits, and characters are digits
	}
	private void UpdateConfirmBorder()
	{
		string oldPassword = OldPassword.Text ?? "";
		string newPassword = NewPassword.Text ?? "";
		string confirmPassword = ConfirmPassword.Text ?? "";

		string error = string.Empty;

		if (IsSixDigits(newPassword) && newPassword == oldPassword) // check whether the new password is valid and identical to the curfrent password, if true, the error message below should show
			error = "New password must be different from your current one";
		else if (IsSixDigits(confirmPassword) && confirmPassword != newPassword) //check if the confirm password is not the same with the new password,if it is not the same, the error message belo should show
			error = "Passwords do not match.";

		bool isValid =
			IsSixDigits(oldPassword) &&
			IsSixDigits(newPassword) &&
			IsSixDigits(confirmPassword) &&
			string.IsNullOrEmpty(error);

		PasswordErrorLabel.Text = error;
		PasswordErrorLabel.IsVisible = !string.IsNullOrEmpty(error);//error message shows only if there is an error

		ConfirmBorder.IsEnabled = isValid;
		ConfirmBorder.Opacity = isValid ? 1.0 : 0.5;
	}
    private async void OnConfirmBorderTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;

        string oldPassword = OldPassword.Text ?? "";
        string newPassword = NewPassword.Text ?? "";
       
        
        try
        {
            ConfirmTxt.IsVisible = false;
            ConfirmIndicator.IsVisible = true;
            ConfirmIndicator.IsRunning = true;

            await Loading.ShowAsync("Updating password...");
            await Task.Delay(500);
            
            // 1. Check the current password
            string? savedPassword = await SecureStorage.Default.GetAsync(PasswordKey);

            if (string.IsNullOrEmpty(savedPassword))
            {
                await DisplayAlert("No Password Found",
                    "No password has been set on this device yet.", "OK");
                return;
            }

            if (oldPassword != savedPassword)
            {
                OldPassword.Text = string.Empty;
                PasswordErrorLabel.Text = "Your current password is incorrect.";
                PasswordErrorLabel.IsVisible = true;               
                return;
            }

            // 2. Save the new password
            await SecureStorage.Default.SetAsync(PasswordKey, newPassword);

           
            var successView = new PhoneNumberVerified();
            successView.Configure(
                imageSource: "successful.png",
                title: "Successful",
                subtitle: "Your password has been updated successfully.",
                buttonText: "Okay",
                onContinue: async () => await Navigation.SafePopAsync());

            var successPage = new ContentPage
            {
                Content = successView,
                BackgroundColor = Colors.White
            };
            NavigationPage.SetHasNavigationBar(successPage, false);
            Shell.SetNavBarIsVisible(successPage, false);

            await Navigation.PushAsync(successPage);

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Change password error: {ex}");
            await DisplayAlert("Error", "Could not change your password. Please try again.", "OK");
        }
        finally
        {
            await Loading.HideAsync();

            ConfirmTxt.IsVisible = true;
            ConfirmIndicator.IsVisible = false;
            ConfirmIndicator.IsRunning = false;
        }
    }

}