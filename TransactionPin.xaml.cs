
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace PreciousPractice;

public partial class TransactionPin : ContentPage
{
    private const int PinLength = 4;

    // Practice only: remove when PINs come from the real API
    private const string DummyPin = "1234";

    //stores the pin and is empty now because no digit has been tapped
    private string _pin = string.Empty;

	// stops extra taps while the page is navigating after the 4th digit has been imputed
	private bool _isNavigating;
	//stores reference(a way for c# to remember and access something)to the pin dots so they can be accessed later
	private readonly Label[] _pinLabels;

    // what CompletePinAsync should show once the PIN is correct
    private readonly Func<Task>? _onPinVerified;
    public TransactionPin(Func<Task> onPinVerified = null)
	{
		InitializeComponent();
		_pinLabels = new[] {Pin1, Pin2, Pin3, Pin4};
        _onPinVerified = onPinVerified;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        //put pin afresh everytime the page opens including when navigating back
        _pin = string.Empty;
		_isNavigating = false;
		UpdatePinDisplay();
    }
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnKeyTapped(object sender,TappedEventArgs e )
	{    // if we are already navigating or pin is complete, stop
		if (_isNavigating || _pin.Length >= PinLength)
			return;
		//if the value entered is not a text(string) containing a digit,stop.
		//otherwise give the string and call the digit
		if (e.Parameter is not string digit)
			return;
		// if _pin =12 and user taps 7, _pin+=7 giving it _pin=127
		_pin += digit;
		//changes from 12 in dots to 127 in dots
		UpdatePinDisplay();
		
	}
	private void OnBackSpaceTapped(object sender, TappedEventArgs e)
	{
		//if no pin has been deleteed, do nothing
		if(_isNavigating || _pin.Length == 0) 
			return;
		
		//removes the last character as user keeps deleting while 
		_pin = _pin[..^1];
		//keeps updating the dots
		UpdatePinDisplay();
	}
	private void UpdatePinDisplay()
    { //Go through each of my four PIN labels.
	  //For every position, check whether the user has entered a digit at that position.
	  //If they have, put a ● there. If they haven't, leave it empty.
        for (int i = 0; i < _pinLabels.Length; i++)
		{
			_pinLabels[i].Text = i < _pin.Length ? "●":string.Empty;
		}
	}
	private async Task CompletePinAsync()
	{
		_isNavigating = true;
		try
		{
			await Loading.ShowAsync("Processing..");
			//the pin to compare against the saved one or dummy pin
			string savedPin = await SecureStorage.Default.GetAsync("Pin") ?? DummyPin;

            //wrong Pin:clear the dots and let the user try again
            if (_pin != savedPin)
			{
				await Loading.HideAsync();
				await DisplayAlert("Incorrect PIN", "The PIN you entered is wrong", "OK");
				
				_pin = string.Empty;
				UpdatePinDisplay() ;
				_isNavigating = false;
				return;
			}
			await Task.Delay(300);
			
			if (_onPinVerified is not null)
			{
				//opened from setting for the fingerprint
				await _onPinVerified();
			}
			else
			{
				//transfer flow
				await Navigation.PushAsync(new TransferSuccessful());
			}
        }
		catch(Exception ex)
		{
            System.Diagnostics.Debug.WriteLine($"PIN error{ex}");

            await DisplayAlert("Error", "Failed to Complete transfer", "OK");

			_isNavigating= false;		
		}
        finally
        {
            if (Loading.IsLoading)
                await Loading.HideAsync();             // ← overlay off, whatever happened
        }
    }
	private async void OnArrowVectorTapped(object sender, TappedEventArgs e)
	{
        if (_isNavigating)
            return;

        if (_pin.Length != PinLength)
        {
            await DisplayAlert("Incomplete PIN", "Please enter your 4-digit PIN.", "OK");
            return;
        }

        await CompletePinAsync();
    }
}