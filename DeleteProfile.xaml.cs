using System.Runtime.CompilerServices;

namespace PreciousPractice;

public partial class DeleteProfile : ContentPage
{
    private const int PasswordLength = 6;
	private const string PasswordKey = "Password";

    public DeleteProfile()
	{
		InitializeComponent();
		UpdateConfirmBorder();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnDeleteProfileBorderTapped(object sender, TappedEventArgs e)
	{
		try
		{
			DeleteProfileTxt.IsVisible = false;

			DeleteProfileIndicator.IsVisible = true;
			DeleteProfileIndicator.IsRunning = true;

			await Task.Delay(500);

			PasswordEntry.Text = string.Empty;
			PasswordErrorLabel.IsVisible = false;

			await ShowPopupAsync(PopupOverlay, PopupCard);
		}
		catch(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
			await DisplayAlert("Error", "Something went wrong.Please try again", "OK");

		}
		finally
		{
			DeleteProfileTxt.IsVisible = true;

			DeleteProfileIndicator.IsVisible = false;
			DeleteProfileIndicator.IsRunning = false;
		}
	}
    private  async Task ShowPopupAsync( VisualElement Overlay, VisualElement Card)
	{
		Overlay.Opacity = 0;
		Card.Opacity = 0;
		Overlay.IsVisible = true;
		Card.IsVisible = true;
		Card.Scale = 0.8;

		await Task.WhenAll(
			Overlay.FadeTo(1, 200),
			Card.FadeTo(1, 200),
			Card.ScaleTo(1, 250,  Easing.SpringOut));
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
	private async void OnPopupOverlayTapped(object sender, TappedEventArgs e)
	{
		if (Loading.IsLoading)
			return;
		await HidePopupAsync(PopupOverlay, PopupCard);
	}
	private async void OnPopupCardTapped(object sender, TappedEventArgs e)
	{
		//Takes tabs
	}
	
	private async void OnCancelBorderTapped(object sender, TappedEventArgs e)
	{
        if (Loading.IsLoading)
            return;
        await HidePopupAsync(PopupOverlay, PopupCard);
	}
    private void OnPassWordTextChanged(object sender, TextChangedEventArgs e)
    {
        PasswordErrorLabel.IsVisible = false; //hides old error while typing
        UpdateConfirmBorder();
    }
    private void UpdateConfirmBorder()
    {
        string password = PasswordEntry.Text ?? "";
        bool isValid = password.Length == PasswordLength && password.All(char.IsDigit);

        ConfirmBorder.IsEnabled = isValid;
    }
    private async void OnConfirmBorderTapped(object sender, TappedEventArgs e)
	{
		if (Loading.IsLoading)
			return;
		string entered = PasswordEntry.Text ?? "";
		
		try
		{
            ConfirmTxt.IsVisible = false;

            ConfirmIndicator.IsVisible = true;
            ConfirmIndicator.IsRunning = true;

			await Loading.ShowAsync("Deleting profile...");
            await Task.Delay(500);

			//check the password
            string? saved = await SecureStorage.Default.GetAsync("Password");

            if (string.IsNullOrEmpty(saved))
            {
                await DisplayAlert("No Password", "No password has been set yet", "OK");
            }
            if (entered != saved)
            {
                PasswordEntry.Text = string.Empty; //clears first
                PasswordErrorLabel.IsVisible = true; //...then show error message               
                return;
            }

            //removes saved profile data
            Preferences.Default.Clear();
			SecureStorage.Default.RemoveAll();

			await HidePopupAsync(PopupOverlay, PopupCard);
            {
                var verifiedView = new PhoneNumberVerified();
                verifiedView.Configure(
                    imageSource: "successful.png",
                    title: " Profile Deleted",
                    subtitle: "Your profile has been deleted successfully.",
                    buttonText: "Okay",
                    onContinue: async () =>
					{ //restart the app
						Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
						}
                );

                var page = new ContentPage { Content = verifiedView, BackgroundColor = Colors.White };
                NavigationPage.SetHasNavigationBar(page, false);
                Shell.SetNavBarIsVisible(page, false);
                await Navigation.PushAsync(page);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

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
