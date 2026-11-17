
using System.Runtime.CompilerServices;

namespace PreciousPractice;

public partial class VerifyBVN : ContentPage
{
	public VerifyBVN()
	{
		InitializeComponent();
	}
    private async Task ShowPopupAsync(VisualElement Overlay, VisualElement Card)
    {
        Overlay.Opacity = 0;
        Card.Opacity = 0;
        Overlay.IsVisible = true;
        Card.IsVisible = true;
        Card.Scale = 0.8;

        await Task.WhenAll(
            Overlay.FadeTo(1,200),
            Card.FadeTo(1,200),
            Card.ScaleTo(1,250, Easing.SpringOut));
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

    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private void OnBVNTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckDetails();
    }
    private void OnAddressTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckDetails();
    }
    private void OnCityTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckDetails();
    }
    private bool IsFormComplete()
    {
        string bvn = BVNEntry.Text?.Trim() ?? "";
        string address = AddressEntry.Text?.Trim() ?? "";
        string city = CityEntry.Text?.Trim() ?? "";

        return bvn.Length == 11 && bvn.All(char.IsDigit)
        && !string.IsNullOrWhiteSpace(address)
        &&!string.IsNullOrWhiteSpace(city);
    }
    private void CheckDetails()
    {
        
        bool isValid = IsFormComplete();

        ContinueBorder.IsEnabled = isValid;
    }
    private async void OnUploadTapped(object sender, TappedEventArgs e)
    {
        if (!IsFormComplete())
        {
            await DisplayAlert("Incomplete Details", "Please enter your BVN, address and city before uploading bill", "OK");
            return;
        }
        
        await ShowPopupAsync(UploadPopupOverlay, UploadPopupCard);
    }
    private async void OnContinueBorderTapped(object sender, TappedEventArgs e)
    {
        try
        {
            ContinueTxt.IsVisible = false;
            ContinueIndicator.IsVisible = true;
            ContinueIndicator.IsRunning = true;

            await Task.Delay(500);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Something went wrong.Please try again", "OK");
        }
        finally
        {
            ContinueTxt.IsVisible = true;
            ContinueIndicator.IsVisible = false;
            ContinueIndicator.IsRunning = false;

        }
    }
    private async void OnUploadPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(UploadPopupOverlay, UploadPopupCard);
    }
    private async void OnUploadPopupCardTapped(object sender, TappedEventArgs e)
    {
        //takes taps
    }
    private async void OnScanDocumentTapped(object sender, TappedEventArgs e)
    {
        FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();
        await UseBillAsync(photo);
    }
    private async void OnImageUploadTapped(object sender, TappedEventArgs e)
    {
        string choice = await DisplayActionSheet(
        "Choose photo from", "Cancel", null,
        "Gallery", "Files & other apps");

        FileResult? photo = choice switch
        {
            "Gallery" => await MediaPicker.Default.PickPhotoAsync(),
            "Files & other apps" => await FilePicker.Default.PickAsync(
                new PickOptions { FileTypes = FilePickerFileType.Images }),
            _ => null   // Cancel
        };

        await UseBillAsync(photo);
    }
    //checks the photo and moves on if its clear
    private async Task UseBillAsync(FileResult? photo)
    {
        if (photo is null || Loading.IsLoading)
            return;

        using Stream stream = await photo.OpenReadAsync();

        // too blurry or low quality to read
        if (stream.Length < 50 * 1024)
        {
            await DisplayAlert("Image not accepted", "This image is too small to read. Please use a clearer photo.", "OK");
            return;
        }

        try
        {
            await Loading.ShowAsync("Uploading bill...");   
            await Task.Delay(500);                          

            await Navigation.PushAsync(new MembershipStatus());
            UploadPopupOverlay.IsVisible = false;
            UploadPopupCard.IsVisible = false;
        }
        finally
        {
            await Loading.HideAsync();                     
        }
    }
}