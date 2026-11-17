using CommunityToolkit.Maui.Core.Platform;

namespace PreciousPractice;

public partial class PinToken : ContentPage
{
    private readonly string _newPassword;

    public PinToken(string newPassword = "")
    {
        InitializeComponent();
        _newPassword = newPassword;
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnCodeTextChanged(object sender, TextChangedEventArgs e)
	{
		if (sender is not Entry entry)
			return;
		if (e.NewTextValue?.Length > 1)
			return;
		try
		{
			if (!string.IsNullOrEmpty(e.NewTextValue))
			{
				if (entry == Code1)
				{
					Code2.Focus();
				}
				else if (entry == Code2)
				{
					Code3.Focus();
				}
				else if (entry == Code3)
				{
					Code4.Focus();
				}
				else if (entry == Code4)
				{
					Code5.Focus();
				}
				else if (entry == Code5)
				{
					Code6.Focus();
				}
				else if (entry == Code6)
				{
					await Code6.HideKeyboardAsync(CancellationToken.None);
				}
			}
			else if (string.IsNullOrEmpty(e.NewTextValue))
			{
				if (entry == Code2)
				{
					Code1.Focus();
				}
				else if (entry == Code3)
				{
					Code2.Focus();
				}
				else if (entry == Code4)
				{
					Code3.Focus();
				}
				else if (entry == Code5)
				{
					Code4.Focus();
				}
				else if (entry == Code6)
				{
					Code5.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Otp entry error:{ex}");
		}
        string code = $"{Code1.Text}{Code2.Text}{Code3.Text}{Code4.Text}{Code5.Text}{Code6.Text}";

		ProceedBorder.IsEnabled = code.Length == 6;
    }
	private void OnCodeFocused(object sender, FocusEventArgs e)
	{
		if (sender is Entry entry)
		{
			entry.CursorPosition = entry.Text?.Length ?? 0; 
		}
	}
	private async void OnProceedBorderTapped(object sender, TappedEventArgs e)
	{
		if (Loading.IsLoading)
			return;

		//put the 6 boxes into one code like '123456'
		string code = $"{Code1.Text}{Code2.Text}{Code3.Text}{Code4.Text}{Code5.Text}{Code6.Text}";
		// Stop if any box is empty
		if (code.Length != 6)
		{
			await DisplayAlert("Incomplete Code", "Please enter the 6-digit code.", "OK");
			return;
		}
		try
		{
			ProceedTxt.IsVisible = false;
			ProceedIndicator.IsVisible = true;
			ProceedIndicator.IsRunning = true;

			await Loading.ShowAsync("Verifying code...");
			await Task.Delay(500);

			// Save the new password now that the code has been entered
			await SecureStorage.Default.SetAsync("Password", _newPassword);

			await Navigation.PushAsync(new SetTransactionPin());
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);

			await DisplayAlert("Error", "Something went wrong.Please try again", "OK");
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