namespace PreciousPractice;

public partial class PersonalInformation : ContentPage
{
	public PersonalInformation()
	{
		InitializeComponent();

        //ContinueButton.Opacity = 0.5;
        //ContinueButton.InputTransparent=true;
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation. PopAsync();
	}
    private void OnPersonalInformationTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckPersonalInformation();
    }
    private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
    {
        string email = EmailEntry.Text?.Trim() ?? "";

        CheckPersonalInformation();
    }  
    private void CheckPersonalInformation()
    {
        // First name
        bool firstNameFilled =
            !string.IsNullOrWhiteSpace(FirstNameEntry.Text);


        // Middle name
        bool middleNameFilled =
            !string.IsNullOrWhiteSpace(MiddleNameEntry.Text);


        // Surname
        bool surnameFilled =
            !string.IsNullOrWhiteSpace(SurnameEntry.Text);


        // Email
        bool emailValid =
            !string.IsNullOrWhiteSpace(EmailEntry.Text) &&
            EmailEntry.Text.Contains("@") &&
            EmailEntry.Text.Contains(".");


        // Date of birth
        bool dateOfBirthFilled =
            !string.IsNullOrWhiteSpace(DateText.Text);


        // Gender
        bool genderSelected =
            MaleRadioButton.IsChecked ||
            FemaleRadioButton.IsChecked;


        // Account type
        bool accountTypeFilled =
            !string.IsNullOrWhiteSpace(AccountTypeEntry.Text);


        // Check EVERYTHING
        bool isComplete =
            firstNameFilled &&
            middleNameFilled &&
            surnameFilled &&
            emailValid &&
            dateOfBirthFilled &&
            genderSelected &&
            accountTypeFilled;

        ContinueButton.Opacity =
           isComplete ? 1.0 : 0.5;

        ContinueButton.InputTransparent =
           !isComplete;
        ContinueButton.IsEnabled = false;
        ContinueButton.IsEnabled = isComplete;
    }
    private void OnCalendarTapped(object sender, TappedEventArgs e)
    {
        DOBPicker.Focus();
    }
    private void DOBPicker_DateSelected(object sender, DateChangedEventArgs e)
	{    if (e.NewDate.HasValue)
		{
			DateText.Text = e.NewDate.Value.ToString("dd/MM/yyyy");

            CheckPersonalInformation();
		}
    }
    private void OnGenderChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckPersonalInformation();
    }
    private async Task ShowPopupAsync()
    {
        PopupOverlay.Opacity = 0;
        AccountTypePopup.Scale = 0.8;
        PopupOverlay.IsVisible = true;
        AccountTypePopup.IsVisible = true;

        await Task.WhenAll(
            PopupOverlay.FadeTo(1, 200),
            AccountTypePopup.ScaleTo(1, 250, Easing.SpringOut)
        );
    }
    private async Task HidePopupAsync()
    {
        await Task.WhenAll(
            PopupOverlay.FadeTo(0, 180),
            AccountTypePopup.ScaleTo(0.8, 180)
        );

        PopupOverlay.IsVisible = false;
        AccountTypePopup.IsVisible = false;
        PopupOverlay.Opacity = 1;
        AccountTypePopup.Scale = 1;
    }
    private async void ClosePopup(object sender, EventArgs e)
    {
        await HidePopupAsync();
    }
    private async void OnPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync();
    }
    private void OnAccountTypePopupTapped(object sender, TappedEventArgs e)
    {
        //takes tabs without closing
    }
    private async void OnDownArrowTapped(object sender, TappedEventArgs e)
    {
        if (PopupOverlay.IsVisible)
            await HidePopupAsync();
        else
            await ShowPopupAsync();
    }
    private async void OnSavingTapped(object sender, TappedEventArgs e)
    {
        AccountTypeEntry.Text = "Saving";
        await HidePopupAsync();
        CheckPersonalInformation();
    }
    private async void OnCurrentTapped(object sender, TappedEventArgs e)
    {
        AccountTypeEntry.Text = "Current";
        await HidePopupAsync();
        CheckPersonalInformation();
    }
    private async void NavToVerifyEmail(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading || !ContinueButton.IsEnabled)
            return;

        try
        {
            PersonalInformationTxt.IsVisible = false;
            PersonalInformationIndicator.IsRunning = true;
            PersonalInformationIndicator.IsVisible = true;

            // Save the details, so the Profile page can show them later
            string fullName = $"{FirstNameEntry.Text?.Trim()} {MiddleNameEntry.Text?.Trim()} {SurnameEntry.Text?.Trim()}";
            Preferences.Set("FullName", fullName);
            Preferences.Set("Email", EmailEntry.Text?.Trim() ?? "");
            Preferences.Set("DateOfBirth", DateText.Text ?? "");
            Preferences.Set("Gender", MaleRadioButton.IsChecked ? "Male" : "Female");
            Preferences.Set("AccountType", AccountTypeEntry.Text ?? "");


            await Loading.NavigateAsync(Navigation, () => new VerifyEmail());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Failed to continue to sign up.", "OK");
        }
        finally
        {
            PersonalInformationTxt.IsVisible = true;
            PersonalInformationIndicator.IsRunning = false;
            PersonalInformationIndicator.IsVisible = false;
        }
    }
}
