
using PreciousPractice.Helpers;
namespace PreciousPractice;
public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //a file path is a text and so it makes use of string to store
        // Show the saved profile photo and hide the camera icon, if there is one
        string photoPath = Preferences.Get("ProfilePhotoPath", string.Empty);//have i saved a value in 'profilephotopath' if not return empty string(" ")
        bool hasPhoto = !string.IsNullOrEmpty(photoPath) && File.Exists(photoPath);//checks whether the string is empty or null, with the ! which reverses the statement, the expression becomes true if there is photo saved and the path is not empty

        if (hasPhoto)//if there is a photo, the hasphoto is true and  will replace the cameraIcon and the Icon becomes invisible.
            //if no photo exists, hasPhoto is false and CameraIcon is shown
            ProfileImage.Source = ImageSource.FromFile(photoPath);
            CameraIcon.IsVisible = !hasPhoto;


        string phoneNumber = Preferences.Get("RegisteredPhoneNumber", string.Empty);//retrives the number already saved, if nothing has been saved, returns empty string
		string displayPhone = FormatPhoneNumber(phoneNumber); //changes the looks of the phone number if necessary. if it was saved as 090, format produces +23490


        string fullName = Preferences.Get("FullName", "Janet Doe Kehinde");
        string gender = Preferences.Get("Gender", "Female");

        ProfileNameUnderPicture.Text = fullName.Split(' ')[0];


        string dateOfBirth = Preferences.Get("DateOfBirth", string.Empty); //retrieve DOB, if no saved date, return empty
        if (string.IsNullOrWhiteSpace(dateOfBirth))//take this as the assigned date
            dateOfBirth = "12/03/2000";

        string email = Preferences.Get("Email", string.Empty); //retrieve email, if nothing has been svd, return 
        if (string.IsNullOrWhiteSpace(email))
            email = "janetdoe@gmail.com";

        ProfileName.Text = fullName;
        GenderLabel.Text = gender;
        PhoneNumber.Text = displayPhone;
        AccountNumberLabel.Text = DefaultAccountNumber.TemporaryAccountNumber;

        DateOfBirthLabel.Text = Mask(dateOfBirth, showStart: 0, showEnd: 2, keepSeparators: true);
        EmailAddressLabel.Text = MaskEmail(email);
       
        // The number inside the "Change Phone Number" popup, on its own line
        PhoneNumberLabel.Text = string.IsNullOrEmpty(displayPhone) ? "" : $"\n{displayPhone}";// if displayPhone is empty, assign an empty string,Otherwise, assign a 
        //newline followed by the formatted phone number.
    }
    private static string FormatPhoneNumber(string phoneNumber)
    {
        string number = phoneNumber?.Trim() ?? ""; //remove surrounding whitespace from the phone number. If the input is null, use
        //an empty string instead.

        if (number.StartsWith("0"))// check whether the number  begins with 0, if true, remove the first 0 (80...)
            number = number.Substring(1);

        return string.IsNullOrEmpty(number) ? "" : $"+234{number}"; //checks whether number is empty, return empty string otherwise, add +234
    }
    private static string Mask(string text, int showStart, int showEnd, bool keepSeparators = false)//text to mask, numbers of characters to leave visible in the end and beginning
                                                                                                    //whether punctuation and other non-alphanumeric charactes should remain unchanged. Defaults to false
    {
        if (string.IsNullOrEmpty(text) || text.Length <= showStart + showEnd)//is sting null or empty or too short to have any characters left to mask btw the visible beginning and end
            return text;

        char[] chars = text.ToCharArray();//characters become example ['A','R']

        for (int i = showStart; i < chars.Length - showEnd; i++)//Suppose the input is "1234567890", showStart is 2, and showEnd is 2.
           // starts at index 2 and continues until index 7 has been processed.Indices 0, 1, 8, and 9 remain visible.
        {
            if (keepSeparators && !char.IsLetterOrDigit(chars[i]))// if "keepSeparators" is true and the current character is not a aletter
                // or number, skip it and leave it unchange (e.g / does not become *)
                continue;

            chars[i] = '*'; // replaces the current characters with *
        }

        return new string(chars);
    }
    private static string MaskEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) //if email is null or empty, return since there is no email to mask
            return email;

        int atIndex = email.IndexOf('@');
        //if the @ is missing or is at the beginning, the whole string is mask
        if (atIndex <= 0)
            return Mask(email, showStart: 2, showEnd: 2);

        string name = email.Substring(0, atIndex); //bears the name part of the email
        string domain = email.Substring(atIndex);// bears the remailing part of the email starting with @
        //masks the name, leaving 2characters visible at the beginning and 2 at the end of the name
        return Mask(name, showStart: 2, showEnd: 2) + domain;
        //leaves the domain(@gmail.com)unchanged and joins both the name an domain togther
    }
    //profile photo
    private async void OnCameraIconTapped(object sender, TappedEventArgs e)
    {
        string choice = await DisplayActionSheet("Choose an option", "Cancel", null, "Take Photo", "Choose from Gallery"); 

        try
        {
            FileResult? photo = null;//fileResult represents a selected file, such as image
            //? means FileResult may be null //= null means no file is there

            if (choice == "Take Photo")//choice is the option choosen by the user. if usser selected Take photo, condition is true
                photo = await TakePhotoAsync();//this happens
            else if (choice == "Choose from Gallery")//if user picked this option, 
                photo = await MediaPicker.Default.PickPhotoAsync(); //access media,open photos,user picks a photo(await) and stores selected file(photo)

            //cancelled or the user picks a photo from gallery
            if (photo is null) //checks whether photo contains no file result. This can happen when user cancels the action sheets which involves all the options or user cancel taking photo,
                               //or cancels gallery without choosing a photo
                return;
            await SaveProfilePhotoAsync(photo);
        }
        catch(PermissionException  )
        {
            await DisplayAlert("Permission Needed", "Please allow camera access in your phone settings to a photo", "OK");
        }
        catch (Exception ex) //catches other exceptions
        {
            System.Diagnostics.Debug.WriteLine($"Profile photo error: {ex}"); //helps developer check for problems 
            await DisplayAlert("Error", "Could not update your profile photo", "OK");//informs user of error
        }
    }
    private async Task<FileResult?> TakePhotoAsync()
    {
        if(!MediaPicker.Default.IsCaptureSupported)//checks whether photo capture is supported by the device. ! means NOT
            //if photo is not captured, show the alert
        {
            await DisplayAlert("Not Supported", "Photo capture is not supported by this device.", "OK");
            return null; // stop the method and return no photo
        }
        return await MediaPicker.Default.CapturePhotoAsync();//if the device allows, await, waits for the method to be completed
    }//return passes the reult inform of fileresult, or nothing if the user cancells halfway
    
    //copies the photo into the app's own folder and shows it
    private async Task SaveProfilePhotoAsync(FileResult photo)
    {
        //A new file name each time, so the image always shows the latest photo and not 2 photos with the same filename
        string newPath = Path.Combine(
            FileSystem.AppDataDirectory, $"profile_{DateTime.Now.Ticks}.jpg");

        using(Stream source = await photo.OpenReadAsync()) //opens a readable stream which is a program to read file's contents for selected image
        using(FileStream destination = File.Create(newPath))//creates a new file at the location specified by newPath and stores it in destination so you can write data into it
        {
            await source.CopyToAsync(destination);//copies image into destination which holds Fikestream which alows my code to read data
        }
        //Remove the previous photo so that it does not pile up
        string oldPath = Preferences.Get("ProfilePhotoPath", string.Empty);
        if(!string.IsNullOrEmpty(oldPath) && File.Exists(oldPath))//if oldphoto is not empty, check if the exist and if both are true, delete the photo
            File.Delete(oldPath);

        Preferences.Set("ProfilePhotoPath", newPath);//sets the new photo
        ProfileImage.Source = ImageSource.FromFile(newPath);//display the new photo
        CameraIcon.IsVisible = false;//camera icon becoms invisible when an image appears in the profile

    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async Task ShowPopupAsync(VisualElement Overlay, VisualElement Card)
	{
		Overlay.Opacity = 0;
		Card.Opacity = 0;
		Overlay.IsVisible = true;
		Card.IsVisible = true;
		Card.Scale = 0.8;

		await Task.WhenAll(
			Overlay.FadeTo(1, 200),
			Card.FadeTo(1, 200),
			Card.ScaleTo(1, 250, Easing.SpringOut));
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
   
    private  async void OnEditIconTapped(object sender, TappedEventArgs e)
	{
        await ShowPopupAsync(PopupOverlay, PopupCard);
    }
	private async void OnCopyAccountNumberTapped(object sender, TappedEventArgs e)
	{
        await Clipboard.Default.SetTextAsync(DefaultAccountNumber.TemporaryAccountNumber);
        await DisplayAlert("Copied", "Account number copied to clipboard.", "OK");
    }
	private async void OnDeleteProfileTapped(object sender, TappedEventArgs e)
	{
        await Loading.NavigateAsync(Navigation, () => new DeleteProfile());
	}
	private async void OnPopupOverlayTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(PopupOverlay, PopupCard);
	}
	private void OnPopupCardTapped(object sender, TappedEventArgs e)
	{
		//Takes Taps
	}
	private async void OnBackBorderTapped(object sender, TappedEventArgs e)
	{
        await HidePopupAsync(PopupOverlay, PopupCard);
    }
	private  async void OnProceedBorderTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        string phoneNumber = Preferences.Get("RegisteredPhoneNumber", string.Empty);
        try
        {
            ProceedTxt.IsVisible = false;
            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

            await Loading.ShowAsync("Sending Code...");
            await HidePopupAsync(PopupOverlay, PopupCard);
            await Navigation.PushAsync(new VerifyPhoneNumber(phoneNumber: phoneNumber,
                step: PhoneVerificationStep.OldNumber));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);

            await DisplayAlert("Error", "Could not Navigate", "OK");
        }
        finally
        {
            await Loading.HideAsync();

            ProceedTxt.IsVisible = true;
            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
    }


}