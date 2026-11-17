using PreciousPractice.Helpers;
using System.Collections.ObjectModel;

namespace PreciousPractice;

public partial class DataHomePage : ContentPage
{
    public class ServiceProvider
    {
        public string ServiceIcon { get; set; }
        public string ServiceName { get; set; }
    }
    public class DataBeneficiaries
    {
        public string ServiceIcon { get; set; }
        public string PhoneNumber { get; set; }
		public string Network {  get; set; }
    }
	private List<ServiceProvider> _serviceProviders = new();
	private ObservableCollection<DataBeneficiaries> _dataBeneficiaries = new();

	//data bundle popup
	private DataPlanCategory _dataCategory = DataPlanCategory.All;
	private DataPlan? _selectedPlan; //stores the plan user has selected. the ?allows the field to be null
									 //meaning that no plan has been selected
	private readonly ObservableCollection<DataPlan> _visibleDataPlans = new();//stores plans currently displayed in the data-plan list
    public DataHomePage()
	{
		InitializeComponent();

		LoadServices();
		LoadBeneficiaries();
		DataPlanList.ItemsSource = _visibleDataPlans;//tells the data-plan list to display items from _visibleDataPlans.


        ResetPage();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private void ResetPage()
	{
        SelectServiceProviderEntry.Text = string.Empty;
        PhoneNumberEntry.Text = string.Empty;
        BeneficiaryNetworkSearchEntry.Text = string.Empty;
		NetworkIcon.Source = null;
		DetailsPhoneNumberLabel.Text = string.Empty;

		CheckDetail();

    }
	private void LoadServices()
	{
		_serviceProviders = new List<ServiceProvider>
		{
			new ServiceProvider {ServiceIcon = "mtn_icon.png",ServiceName = "MTN"},
		   new ServiceProvider {ServiceIcon = "airtel_icon.png",ServiceName = "Airtel"},
		   new ServiceProvider {ServiceIcon = "glo_icon.png",ServiceName = "Glo"},
		   new ServiceProvider {ServiceIcon = "mobile_icon.png",ServiceName = "9mobile"},
		};
		ServiceProviderList.ItemsSource = _serviceProviders; //connects the list to the network data
	}
	private void LoadBeneficiaries()
	{
		_dataBeneficiaries = new ObservableCollection<DataBeneficiaries>
		{
			new() { ServiceIcon = "mobile_icon.png", PhoneNumber = "8092345678", Network="9mobile" },
			new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8023456789", Network="Glo"},
			new() { ServiceIcon = "airtel_icon.png", PhoneNumber = "8034567890", Network="Airtel" },
			new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8045678901", Network="MTN" },
			new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8056789012", Network="Glo" },
			new() { ServiceIcon = "airtel_icon.png", PhoneNumber = "8067890123", Network="Airtel" },
			new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8078901234", Network="MTN" },
			new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8089012345", Network="Glo" },
			new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8103456789", Network="MTN"},
			new() { ServiceIcon = "mtn_icon.png", PhoneNumber = "8067843245", Network="MTN"},
			new() { ServiceIcon = "airtel_icon.png", PhoneNumber = "8026753478", Network="Airtel"},
			new() { ServiceIcon = "glo_icon.png", PhoneNumber = "8053232768", Network="Glo"},
			new() { ServiceIcon = "mobile_icon.png", PhoneNumber = "9092735886", Network="9mobile"}
		};
		BindableLayout.SetItemsSource(RecentReceipientStack, _dataBeneficiaries.Take(8).ToList());
		BindableLayout.SetItemsSource(AllBeneficiariesStack, _dataBeneficiaries.Take(8).ToList());
	}
	private void OnRefreshIconTapped(Object sender, TappedEventArgs e)
	{
		ResetPage();
	}
	private async Task ShowPopupAsync(VisualElement overlay, VisualElement card)
	{
		overlay.Opacity=0;
		card.Opacity=0;
		overlay.IsVisible =true;
		card.IsVisible =true;
		card.Scale = 0.8;

		await Task.WhenAll(
			overlay.FadeTo(1, 200),
			card.FadeTo(1, 200),
			card.ScaleTo(1, 250, Easing.SpringOut));
	}
	private async Task HidePopupAsync(VisualElement overlay, VisualElement card)
	{
		await Task.WhenAll(
			overlay.FadeTo(0, 200),
			card.FadeTo(0, 200),
			card.ScaleTo(0.8 ,250, Easing.SpringOut));

		overlay.IsVisible = false;
		card.IsVisible = false;
	}
	private void CheckDetail()
	{
		string phone = PhoneNumberEntry.Text?.Trim() ?? ""; //get the text written, remove spaces but if nothing is written, return empty string

		bool hasServiceProvider = !string.IsNullOrWhiteSpace(SelectServiceProviderEntry.Text); //return true if there is no value, ! means reveses the result
		//hasServiceProvider is true if the network field has a text
		bool hasPhoneNumber = phone.Length == 10 && phone.All(Char.IsDigit); //must be 10 characters and these characters must be digits

		bool isValid = hasServiceProvider && hasPhoneNumber;

        ProceedBorder.IsEnabled = isValid;
        ProceedBorder.Opacity = isValid ? 1.0 : 0.5;

    }
	private async void OnViewAllTapped(object sender, TappedEventArgs e)
	{
        BeneficiaryNetworkSearchEntry.Text = string.Empty; // clears any previous search
        DataBeneficiary.ItemsSource = _dataBeneficiaries; //the beneficiaies list
		
		await ShowPopupAsync(PopupOverlay, DataPopupCard);
	}
    private async void OnBeneficiaryTapped(object sender, TappedEventArgs e)
    {
        if (sender is not BindableObject item || item.BindingContext is not DataBeneficiaries selected)
            return;

        SelectServiceProviderEntry.Text = selected.Network;
        PhoneNumberEntry.Text = selected.PhoneNumber;
        NetworkIcon.Source = selected.ServiceIcon;
        DetailsPhoneNumberLabel.Text = selected.PhoneNumber;
        CheckDetail();

        await ShowPopupAsync(DataDetailsPopupOverlay, DataDetailsPopupCard);
    }
    private void OnPhoneNumberTextChanged(object sender, TextChangedEventArgs e)
	{
		CheckDetail();
			if(PhoneNumberEntry.Text?.Length ==10)		
			PhoneNumberEntry.Unfocus();	
	}
	private async void OnProceedBorderTapped(object sender, TappedEventArgs e)
	{
		string phoneNumber = PhoneNumberEntry.Text?.Trim() ?? ""; //get the phonenumber and trim spaces, retuen empty string if niull
		string serviceProvider = SelectServiceProviderEntry.Text?.Trim() ?? "";

		try
		{
			ProceedTxt.IsVisible = false;
			ProceedIndicator.IsVisible = true;
			ProceedIndicator.IsRunning = true;

			await Task.Delay(500);
			await OpenDataBundlePopupAsync(); 
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
    //networkpopup
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
		//Takes taps without closing the popup
	}
	private async void OnSelectServiceProviderEntry(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is ServiceProvider selected)// retrieves the first item in the current selection or
																			// default value if nothing has been selected
		{
			SelectServiceProviderEntry.Text = selected.ServiceName; //copies selected network
			NetworkIcon.Source = selected.ServiceIcon;// updates the network icon
            ServiceProviderList.SelectedItem = null;
        }
		CheckDetail();
		await HidePopupAsync(ServicePopupOverlay,ServicePopupCard);
	}
	//Beneficiaries viewall popup
	private async void OnPopupOverlayTapped(Object sender, TappedEventArgs e)
	{
		await HidePopupAsync(PopupOverlay, DataPopupCard);
	}
	private void OnDataPopupCardTapped(Object sender, TappedEventArgs e)
	{
		//Takes taps without closing the popup
	}
	private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
	{
		string searchText = e.NewTextValue?.Trim() ?? "";//takes new value, removes space, if value is null, returns empty string
		if (!string.IsNullOrEmpty(searchText)) //is true if user has entered a search item
		{
			DataBeneficiary.ItemsSource = _dataBeneficiaries;
			return;
		}
		DataBeneficiary.ItemsSource = _dataBeneficiaries  //this does the filtering, when the user starts typing, the phone numbers begin to filter
			.Where(b =>b.PhoneNumber.Contains(searchText) ||
			           b.Network.Contains(searchText, StringComparison.OrdinalIgnoreCase)) //searches the network and ignores whether user tprd in upper or lower case 
			.ToList(); //brings the list containing only matching beneficiaries with what the user typed
	}
	private void OnClearSearchEntryTapped(object sender, TappedEventArgs e)
	{
		BeneficiaryNetworkSearchEntry.Text = string.Empty;
		DataBeneficiary.ItemsSource = _dataBeneficiaries;
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
    //DataBeneficiaries from view allpopup
    private async void OnSelectBeneficiaryNetworkEntry(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not DataBeneficiaries selected) //retrieves the first currently selected and stores it
            return;

        DataBeneficiary.SelectedItem = null; 
        SelectServiceProviderEntry.Text = selected.Network;
        PhoneNumberEntry.Text = selected.PhoneNumber; //copies  the selected phone number and stores it in the phone number entry
        NetworkIcon.Source = selected.ServiceIcon;
        DetailsPhoneNumberLabel.Text = selected.PhoneNumber;
        CheckDetail();

        await HidePopupAsync(PopupOverlay, DataPopupCard);
    }
    //Data bundle Popup
    private async Task OpenDataBundlePopupAsync()
    {
        _dataCategory = DataPlanCategory.All;
        _selectedPlan = null;
        DataPlanList.SelectedItem = null;

        ShowDataPlans();
        UpdateTabStyles();
        UpdateDataProceed();

        await ShowPopupAsync(DataBundlePopupOverlay, DataBundlePopupCard);
    }

    // Shows only the plans for the chosen network and tab
    private void ShowDataPlans() //its network matches the selected network, category matches its cataegory except
		//the category is all
    {
        string network = SelectServiceProviderEntry.Text?.Trim() ?? "";

        _visibleDataPlans.Clear();

        foreach (var plan in DataPlans.All)
        {
            bool rightNetwork = plan.Network == network;
            bool rightCategory = _dataCategory == DataPlanCategory.All || plan.Category == _dataCategory;

            if (rightNetwork && rightCategory)
                _visibleDataPlans.Add(plan); //is an observeable collection, the list can show changes as the collection is clered and repopulated
        }
    }

    private void OnDataCategoryTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not string tab) //verifies that the "if line" below is a string
            return;

        if (Enum.TryParse(tab,ignoreCase:true, out DataPlanCategory category))//This attempts to convert the string into a valid DataPlanCategory enum value(mtn, airtel,etc)
        {
            _dataCategory = category;

            // A plan picked under another tab may no longer be visible
            _selectedPlan = null;
            DataPlanList.SelectedItem = null;

            ShowDataPlans();
            UpdateTabStyles();
            UpdateDataProceed();
        }
    }
    private void UpdateTabStyles()
    //Checks whether the app is using dark mode.Applies a green background to the selected tab.
    //makes selected tab border transparent, Changes the text colour to a theme-appropriate colour for an unselected tab.
    {
        StyleTab(AllTab, _dataCategory == DataPlanCategory.All);
        StyleTab(DailyTab, _dataCategory == DataPlanCategory.Daily);
        StyleTab(WeeklyTab, _dataCategory == DataPlanCategory.Weekly);
        StyleTab(MonthlyTab, _dataCategory == DataPlanCategory.Monthly);
    }

    // One place that decides how a tab looks when selected or not
    private void StyleTab(Border tab, bool isSelected)
    {
        bool isDark = Application.Current?.RequestedTheme == AppTheme.Dark;

        tab.BackgroundColor = isSelected ? Color.FromArgb("#009933") : Colors.Transparent;
        tab.Stroke = isSelected ? Colors.Transparent : Color.FromArgb("#009933");

        // Make the text readable on green
        if (tab.Content is Label label) // text styling works only when rgw tab's direct
			//content is a label. if label is in another layout, it won't work.
        {
            label.TextColor = isSelected
                ? Colors.White
                : (isDark ? Colors.White : Color.FromArgb("#141414"));
        }
    }
    // Keeps the selection, so the chosen card stays highlighted (#CCFFE5)
    private void OnDataPlanSelected(object sender, SelectionChangedEventArgs e)
    {
        _selectedPlan = e.CurrentSelection.FirstOrDefault() as DataPlan;
        UpdateDataProceed();
    }

    // The popup's Proceed is enabled only once a plan is picked
    private void UpdateDataProceed()//checks if a plan has been selected, then the proceed border is enabled
    {
        bool hasPlan = _selectedPlan is not null; 

        DataProceedBorder.IsEnabled = hasPlan;
        DataProceedBorder.Opacity = hasPlan ? 1.0 : 0.5;
    }

    private async void OnDataBundlePopupOverlayTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(DataBundlePopupOverlay, DataBundlePopupCard);
	}
	private void OnDataBundlePopupCardTapped(object sender, TappedEventArgs e)
	{
		//Takes taps without closing the popup
	}
	private async void OnDataBackIconTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(DataBundlePopupOverlay, DataBundlePopupCard);
	}
	private async void OnDataProceedBorderTapped(object sender, TappedEventArgs e)
	{
		if (_selectedPlan is null)
			return;

		await HidePopupAsync(DataBundlePopupOverlay, DataBundlePopupCard);
		await Loading.NavigateAsync(Navigation, () => new DataSummaryPage(NetworkIcon.Source, SelectServiceProviderEntry.Text, PhoneNumberEntry.Text, _selectedPlan));
	}
	//DataDetailpopup
	private async void OnDataDetailsPopupOverlayTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(DataDetailsPopupOverlay, DataDetailsPopupCard);
	}
	private async void OnDataDetailsPopupCardTapped(object sender, TappedEventArgs e)
	{
		//takes all the taps
	}
	private async void OnChangeTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(DataDetailsPopupOverlay, DataDetailsPopupCard);

		await ShowPopupAsync(PopupOverlay, DataPopupCard);
		DataBeneficiary.ItemsSource = _dataBeneficiaries;		
	}
	private async void OnClearAllTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(DataDetailsPopupOverlay, DataDetailsPopupCard);
		ResetPage();
	}
	private async void OnApplyTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(DataDetailsPopupOverlay, DataDetailsPopupCard);
		await OpenDataBundlePopupAsync();
	}
	
}