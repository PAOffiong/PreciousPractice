using CommunityToolkit.Maui.Core.Platform;
using System.Linq;
namespace PreciousPractice;

public partial class IdInformation : ContentPage
{
    public IdInformation()
    {
        InitializeComponent();
    }

    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        PopupOverlay.IsVisible = false;
        await Navigation.SafePopAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        PopupOverlay.IsVisible = false;
    }

    private async void OnNinTextChanged(object sender, TextChangedEventArgs e)
    {
        string nin = NinEntry.Text?.Trim() ?? "";

        if (nin.Length == 11 && nin.All(char.IsDigit))
        {
            NextButton.Opacity = 1.0;
            await NinEntry.HideKeyboardAsync();
        }
        else
        {
            NextButton.Opacity = 0.5;
        }
    }

    private async void NavToPhoneNumberVerified(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;

        string nin = NinEntry.Text?.Trim() ?? string.Empty;

        if (nin.Length != 11)
            return;

        NinEntry.Unfocus();
        try
        {
            await Loading.ShowAsync("Verifying NIN...");
            await Task.Delay(500);

            var verifiedView = new PhoneNumberVerified();
            verifiedView.Configure(
                imageSource: "phone_verified.png",
                title: "NIN Verified Successfully",
                subtitle: "Click next to continue account setup",
                buttonText: "Next",
                onContinue: async () => await Loading.NavigateAsync(Navigation, () => new CreateNewAccount())
            );

            var page = new ContentPage
            {
                Content = verifiedView,
                BackgroundColor = Colors.White
            };
            NavigationPage.SetHasNavigationBar(page, false);
            Shell.SetNavBarIsVisible(page, false);
            await Navigation.PushAsync(page);
        }
        catch (Exception ex)
        {
           System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Could not verify NIN.Please try again", "OK");
        }
        finally
        {
            await Loading.HideAsync();
        }
    }

    // ================= POPUP =================

    private async Task ShowPopupAsync()
    {
        PopupOverlay.Opacity = 0;
        PopupCard.Scale = 0.8;
        PopupOverlay.IsVisible = true;

        await Task.WhenAll(
            PopupOverlay.FadeTo(1, 200),
            PopupCard.ScaleTo(1, 250, Easing.SpringOut)
        );
    }

    private async Task HidePopupAsync()
    {
        await Task.WhenAll(
            PopupOverlay.FadeTo(0, 180),
            PopupCard.ScaleTo(0.8, 180)
        );

        PopupOverlay.IsVisible = false;
        PopupOverlay.Opacity = 1;
        PopupCard.Scale = 1;
    }

    private async void clickHere(object sender, EventArgs e)
    {
        try
        {
            await ShowPopupAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failure to navigate", "ok");
        }
    }

    private async void OnEnterNINNowTapped(object sender, TappedEventArgs e)
    {
        try
        {
            EnterNINTxt.IsVisible = false;
            Indicator.IsVisible = true;
            Indicator.IsRunning = true;

            await Task.Delay(500);

            await HidePopupAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
        finally
        {
            EnterNINTxt.IsVisible = true;
            Indicator.IsRunning = false;
            Indicator.IsVisible = false;
        }
    }

    private async void OnSkipForNowTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {
            SkipTxt.IsVisible = false;
            SkipIndicator.IsVisible = true;
            SkipIndicator.IsRunning = true;

            await Task.Delay(500);

            await Loading.NavigateAsync(Navigation, () => new CreateNewAccount());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
        finally
        {
            SkipTxt.IsVisible = true;
            SkipIndicator.IsRunning = false;
            SkipIndicator.IsVisible = false;
        }
    }
    private void OnForgetNINTapped(object? sender, EventArgs e)
    {
        StaticButtons.IsVisible = true;
    }
    private void OnCancelTapped(object sender, TappedEventArgs e)
    {
        // Hide both static borders
        StaticButtons.IsVisible = false;
    }
}