using Microsoft.Maui.ApplicationModel.Communication;
using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
namespace PreciousPractice;

public partial class AirtimeHomePage : ContentPage
{
    
    public class ServiceProvider
    {
        public string ServiceIcon { get; set; }
        public string ServiceName { get; set; }
    }
	public class AllBeneficiaries
	{
		public string ServiceIcon { get; set; }
		public string PhoneNumber { get; set; }
	}
	private ObservableCollection<AllBeneficiaries> _allBeneficiaries;
    public AirtimeHomePage()
	{
		InitializeComponent();
        ResetPage();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private void OnRefreshIconTapped(object sender, TappedEventArgs e)
    {
        ResetPage();
    }
    private void ResetPage()
    {
        // clear the entries back to their empty/placeholder state
        PhoneNumberEntry.Text = string.Empty;
        SelectServiceProviderEntry.Text = string.Empty;
        NetworkIcon.Source = null;
        DetailsPhoneNumberLabel.Text = string.Empty;

        // disable Proceed again, since nothing is filled in anymore
        ProceedBorder.IsEnabled = false;

        // reload the beneficiary/service data fresh
        LoadBeneficiaries();
        LoadServices();
    }
    private void LoadServices()
    {
        var serviceProvider = new List<ServiceProvider>
       {
           new ServiceProvider {ServiceIcon = "mtn_icon.png",ServiceName = "MTN"},
           new ServiceProvider {ServiceIcon = "airtel_icon.png",ServiceName = "Airtel"},
           new ServiceProvider {ServiceIcon = "glo_icon.png",ServiceName = "Glo"},
           new ServiceProvider {ServiceIcon = "mobile_icon.png",ServiceName = "9mobile"},
       };
        ServiceProviderList.ItemsSource =serviceProvider;
    }
	private void LoadBeneficiaries()
	{
        _allBeneficiaries = new ObservableCollection<AllBeneficiaries>
        {
            new() { ServiceIcon = "mobile_icon.png", PhoneNumber = "8092345678" },
            new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8023456789" },
            new() { ServiceIcon = "airtel_icon.png", PhoneNumber = "8034567890" },
            new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8045678901" },
            new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8056789012" },
            new() { ServiceIcon = "airtel_icon.png", PhoneNumber = "8067890123" },
            new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8078901234" },
            new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8089012345" },
			new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8103456789"},
			new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8067843245"},
			new() { ServiceIcon = "airtel_icon.png", PhoneNumber = "8026753478"},
			new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8053232768"},
			new() { ServiceIcon = "mobile_icon.png", PhoneNumber = "90927358864"},

        };
		BindableLayout.SetItemsSource(AllBeneficiariesStack, _allBeneficiaries.Take(6).ToList());
        BindableLayout.SetItemsSource(RecentReceipientStack, _allBeneficiaries.Take(6).ToList());
	}
    //popup for all Beneficiaries
    private async Task ShowPopupAsync(VisualElement overlay, VisualElement card)
    {
        overlay.Opacity = 0;
        card.Opacity = 0;
        overlay.IsVisible = true;
        card.IsVisible = true;
        card.Scale = 0.8;

        await Task.WhenAll(
            overlay.FadeTo(1, 200),
            card.FadeTo(1, 200),
            card.ScaleTo(1, 250, Easing.SpringOut)
        );
    }

    private async Task HidePopupAsync(VisualElement overlay, VisualElement card)
    {
        await Task.WhenAll(
            overlay.FadeTo(0, 200),
            card.FadeTo(0, 200),
            card.ScaleTo(0.8, 250, Easing.SpringOut)
        );

        overlay.IsVisible = false;
        card.IsVisible = false;
    }
    private async void OnViewAllTapped( object sender, TappedEventArgs e)
	{
		AirtimeBeneficiaries.ItemsSource = _allBeneficiaries;

		await ShowPopupAsync(PopupOverlay, AirtimePopupCard);
	}
    private async void OnPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(PopupOverlay, AirtimePopupCard);
    }
    
	private void OnAirtimePopupCardTapped(object sender, TappedEventArgs e)
	{
        //take taps but does not close the popup
	}
	private async void OnSelectBeneficiaryNetworkEntry(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault()is AllBeneficiaries selected)
		{
            PhoneNumberEntry.Text = selected.PhoneNumber;
            await HidePopupAsync(PopupOverlay, AirtimePopupCard);
        }
	}
    private async void OnSelectServiceProviderEntry(object sender, SelectionChangedEventArgs e)
    {
        if(e.CurrentSelection.FirstOrDefault()is ServiceProvider selected)
        {
            SelectServiceProviderEntry.Text = selected.ServiceName;
            NetworkIcon.Source = selected.ServiceIcon;

            CheckDetails();
            await HidePopupAsync(ServicePopupOverlay, ServicePopupCard);
        }
    }
    private async void OnDownwardAngleTapped(object sender, TappedEventArgs e)
    {
        await ShowPopupAsync(ServicePopupOverlay, ServicePopupCard);
    }
    private async void OnServicePopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(ServicePopupOverlay, ServicePopupCard);
    }
    private void OnServicePopupCardTapped(object sender, TappedEventArgs e)
    {
        //take taps but does not close the popup
    }
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? "";
        if(string.IsNullOrWhiteSpace(searchText))
        {
            AirtimeBeneficiaries.ItemsSource = _allBeneficiaries;
            return;
        }
        var filteredBeneficiaries = _allBeneficiaries   //create filteredbeneficiaries from _allbeneficiaries, keeeping only beneficiaries whose 
            .Where(beneficiary =>                       //phone number contains searchText(e.g 0803), and turn the result into a list.
               beneficiary.PhoneNumber.Contains(searchText)||
               beneficiary.ServiceIcon.Contains(searchText, StringComparison.OrdinalIgnoreCase)) 
            .ToList();
        AirtimeBeneficiaries.ItemsSource = filteredBeneficiaries;
    }

    private void OnClearSearchEntryTapped(object sender, TappedEventArgs e)
    {
        BeneficiaryNetworkSearchEntry.Text = string.Empty;
        AirtimeBeneficiaries.ItemsSource = _allBeneficiaries;
    }
    private void CheckDetails()
    {
        bool hasServiceProvider = !string.IsNullOrWhiteSpace(SelectServiceProviderEntry.Text);

        bool hasPhoneNumber = !string.IsNullOrWhiteSpace(PhoneNumberEntry.Text) &&
            PhoneNumberEntry.Text.Length == 10;
        ProceedBorder.IsEnabled = hasServiceProvider && hasPhoneNumber;
        
    }
    private async void OnContactCalender(object sender, TappedEventArgs e)
    {
        try//permission to read contacts
        {
            PermissionStatus status = await Permissions.RequestAsync<Permissions.ContactsRead>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Needed", "Please allow access to your contacts in your phone settings.", "OK");
                return;
            }
            //Open the phone's contact picker
            Contact? contact = await Microsoft.Maui.ApplicationModel.Communication.Contacts.Default.PickContactAsync();
            if (contact is null)
                return; //user backed out without choosing

            // each saved number into the 10-digit form your entry uses
            List<string> numbers = contact.Phones
                .Select(p => ToLocalNumber(p.PhoneNumber))
                .Where(n => n.Length == 10)
                .Distinct()
                .ToList();
            if(numbers.Count == 0)
            {
                await DisplayAlert("No Valid Number", $"{contact.DisplayName} has no Nigerian mobile number saved.", "OK");
                return;
            }
            string chosen = numbers[0];

            //if the contact has more than one number, let the user pick
            if(numbers. Count > 1)
            {
                string? pick = await DisplayActionSheet($"Choose a number for {contact.DisplayName}", "Cancel", null, numbers.Select(n => $"0{n}").ToArray());
                if (string.IsNullOrEmpty(pick) || pick == "Cancel")
                    return;

                chosen = pick.Substring(1); //remove the leading 0 again
            }
            PhoneNumberEntry.Text = chosen;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Contact pick error: {ex}");
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    // "+2348031234567", "08031234567", "2348031234567" -> "8031234567"
    private static string ToLocalNumber(string? raw)
    {
        //keep only the digits
        string digits = new string((raw ?? "").Where(char.IsDigit).ToArray());

        if(digits.StartsWith("234") && digits.Length == 13)
            digits = digits.Substring(3);  //drop the country code
       
        else if(digits.StartsWith("0")&& digits.Length == 11)
            digits = digits.Substring(1); // drops the first 0

        return digits;
    }
    private void OnPhoneNumberTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckDetails();

        if(PhoneNumberEntry.Text?.Length ==10)
        {
            PhoneNumberEntry.Unfocus();

        }
    }
    private async void OnProceedBorderTapped(object sender, TappedEventArgs e)
    {
        string phoneNumber = PhoneNumberEntry.Text?.Trim() ?? "";
        string serviceProvider = SelectServiceProviderEntry.Text?.Trim() ?? "";

        try
        {
            ProceedTxt.IsVisible = false;
            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

            await Task.Delay(500);

            await ShowPopupAsync(AirtimeAmountPopupOverlay, AirtimeAmountPopupCard);
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
           
        }
        finally
        {
            ProceedTxt.IsVisible = true;
            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
    }
    private async void OnAirtimeAmountPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(AirtimeAmountPopupOverlay, AirtimeAmountPopupCard);
    }
    private  void OnAirtimeAmountPopupCardTapped(object sender, TappedEventArgs e)
    { 
        //does not close the popup
    }
    private async void OnAirtimeBackIconTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(AirtimeAmountPopupOverlay, AirtimeAmountPopupCard);
    }
    private void OnAirtimeAmountTextChanged(object sender, TextChangedEventArgs e)
    {
        bool amountEnteredValid =!String.IsNullOrWhiteSpace(AirtimeAmountEntry.Text);

        AirtimeProceedBorder.IsEnabled = amountEnteredValid;

        if(int.TryParse(AirtimeAmountEntry.Text, out int amount))
        {
            if(amount <50)
            { 
                //invalid amount
            }
            else
            {
                //valid amount
            }
        }
    }
    private void OnAmountTapped(object sender, TappedEventArgs e)
    {
        if(sender == FiftyNaira)
        {
            AirtimeAmountEntry.Text = "50";
        }
        else if(sender == HundredNaira)
        {
            AirtimeAmountEntry.Text = "100";
        }
        else if (sender == TwoHundredNaira)
        {
            AirtimeAmountEntry.Text = "200";
        }
        else if (sender == FiveHundredNaira)
        {
            AirtimeAmountEntry.Text = "500";
        }
        else if (sender == OneThousand)
        {
            AirtimeAmountEntry.Text = "1000";
        }
        else if (sender == TwoThousand)
        {
            AirtimeAmountEntry.Text = "2000";
        }
        else if (sender == FiveThousand)
        {
            AirtimeAmountEntry.Text = "5000";
        }
        else if (sender == TenThousand)
        {
            AirtimeAmountEntry.Text = "10000";
        }
        return;
    }
    private async void OnAirtimeProceedBorderTapped(object sender, TappedEventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () => new AirtimeSummary(NetworkIcon.Source, SelectServiceProviderEntry.Text, PhoneNumberEntry.Text, AirtimeAmountEntry.Text));
    }
    private async void OnBeneficiaryTapped(object sender, TappedEventArgs e)
    {
        var element = (VisualElement)sender; //i know this is(sender) a viual element like entry, border, i want to use it 
        var selected = (AllBeneficiaries)element.BindingContext;//get the beneficiary data attached to the thing the user just tapped, and call it selected

        NetworkIcon.Source = selected.ServiceIcon;
        DetailsPhoneNumberLabel.Text = selected.PhoneNumber;

        await ShowPopupAsync(AirtimeDetailsOverlay, AirtimeDetailsCard);
    }
    private async void OnAirtimeDetailsOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(AirtimeDetailsOverlay, AirtimeDetailsCard);
    }
    private void OnAirtimeDetailsCardTapped(object sender, TappedEventArgs e)
    {
        //takes all the taps without closing popup
    }
    private async void OnChangeTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(AirtimeDetailsOverlay, AirtimeDetailsCard);
        AirtimeBeneficiaries.ItemsSource = _allBeneficiaries;
        await ShowPopupAsync(PopupOverlay, AirtimePopupCard);
    }
    private async void OnApplyTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(AirtimeDetailsOverlay, AirtimeDetailsCard);
        await ShowPopupAsync(AirtimeAmountPopupOverlay, AirtimeAmountPopupCard);
    }
    private async void OnClearAllTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(AirtimeDetailsOverlay, AirtimeDetailsCard);
        ResetPage();
    }

}