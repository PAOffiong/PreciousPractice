using CommunityToolkit.Maui.Core.Platform;

namespace PreciousPractice;

public partial class CreateNewAccount : ContentPage
{
	public CreateNewAccount()
	{
		InitializeComponent();
	}
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void OnPhoneNumberTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateCreateAccountState();
        if (IsValidPhoneNumber(PhoneNumberEntry.Text))
        {
            await PhoneNumberEntry.HideKeyboardAsync();
        }
    }
    private void OnInvitationCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateCreateAccountState();
    }

    private void OnAgreementCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        UpdateCreateAccountState();
    }
    private void UpdateCreateAccountState()
    {
        bool phoneNumberIsValid = IsValidPhoneNumber(PhoneNumberEntry.Text);
        
        bool canAcceptAgreement =
            phoneNumberIsValid;
            

        AgreementRadioButton.IsEnabled = canAcceptAgreement;

       
        bool canCreateAccount =
            canAcceptAgreement &&
            AgreementRadioButton.IsChecked;

        CreateNewAccountButton.Opacity = canCreateAccount ? 1.0 : 0.5;
        CreateNewAccountButton.InputTransparent = !canCreateAccount;
    }

    private static bool IsValidPhoneNumber(string? phoneNumber)
    {
        string digits = new string(
            (phoneNumber ?? string.Empty)
                .Where(char.IsDigit)
                .ToArray());

        
        return digits.Length == 10;
    }
    
    private void OnInvitationCodeArrowTapped(object sender, TappedEventArgs e)
    {
        InvitationCodeField.IsVisible = !InvitationCodeField.IsVisible;
    }
    private async void NavToVerifyPhoneNumber(object sender, TappedEventArgs e)
    {
        if (CreateNewAccountButton.InputTransparent)
            return;
        try
        {
            NewAccountTxt.IsVisible = false;


            NewAccountIndicator.IsVisible = true;
            NewAccountIndicator.IsRunning = true;

            await Task.Delay(500);

            await Loading.NavigateAsync(Navigation, () => new VerifyPhoneNumber(isLogin: false));


        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Failed to continue to sign up.", "OK");
        }
        finally
        {
            // Show text again
            NewAccountTxt.IsVisible = true;

            // Hide spinner
            NewAccountIndicator.IsRunning = false;
            NewAccountIndicator.IsVisible = false;
        }
    }
    private async void NavToRegisterDeviceLogin(object sender, TappedEventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () => new RegisterDeviceLogin());
    }

}