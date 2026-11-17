
namespace PreciousPractice;

public partial class CustomerService : ContentPage
{
	//contact details in one place,so they're easy to change
	private const string SupportPhone = "+2348130545390";
	private const string SupportWhatsApp = "2348130545390";
	private const string SupportEmail = "customercare@pyrowoonline.com";
	public CustomerService()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnWhatsAppTapped(object sender, TappedEventArgs e)
	{
		try
		{
			await Launcher.Default.OpenAsync($"https://wa.me/{SupportWhatsApp}");
		}
		catch(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
			await DisplayAlert("Error", "Could not open WhatsApp.", "OK");
		}
	}
	private async void OnCallTapped(object sender, TappedEventArgs e)
	{
		const string PhoneOption = "Phone";
		const string WhatsAppOption = "WhatsApp";
		const string CopyOption = "Copy Number";

		//build list from what this phone can use
		var options = new List<string>();

		if (PhoneDialer.Default.IsSupported)
			options.Add(PhoneOption);
		if (await Launcher.Default.CanOpenAsync("whatsapp://send"))
			options.Add(WhatsAppOption);
		options.Add(CopyOption);

		string? choice = await DisplayActionSheet($"Call{SupportPhone} with", "Cancel", null, options.ToArray()); 

		try
		{
			switch(choice)
			{
				case PhoneOption: PhoneDialer.Default.Open(SupportPhone);
					break;
				case WhatsAppOption: await Launcher.Default.OpenAsync($"https://wa.me/{SupportWhatsApp}");
					break;
				case CopyOption: await Clipboard.Default.SetTextAsync(SupportPhone);
					await DisplayAlert("Copied", "Phone number copied to clipboard", "OK");
					break;
					//Cancel, or tapped outside: do nothing
			}
		}
		catch(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
			await DisplayAlert("Error", "Could not open that app.", "OK");
		}
	}
	private async void OnEmailTapped(object sender, TappedEventArgs e)
	{
		try
		{
			if(Email.Default.IsComposeSupported)
			{
				var message = new EmailMessage
				{
					Subject = "Customer Support",
					To = new List<string>{ SupportEmail }
				};
				await Email.Default.ComposeAsync(message);
			}
			else
			{
				await DisplayAlert("No Email App", $"Please email us at {SupportEmail}", "OK");
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
			await DisplayAlert("Error", "Could not open your email app.", "OK");
		}
	}
}