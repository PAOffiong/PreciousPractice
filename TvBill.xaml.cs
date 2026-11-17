
using CommunityToolkit.Maui.Converters;
using PreciousPractice.Helpers;
using System.Collections.ObjectModel;
namespace PreciousPractice;

public partial class TvBill : ContentPage
{
	public class ServiceProvider
	{
		public string Logo {  get; set; }
		public string TVProviders {  get; set; }
		public string TVSubscriptions {  get; set; }
	}
	public class AllBeneficiaries
	{
		public string Logo { get; set; }
		public string TVProviders {  get; set; }
		public string TVSubscriptions { get; set; }
		public string UserName { get; set; }
		public string SmartCardNumber {  get; set; }
	}
	private const int MinSmartCardNumberLength = 10;
	private const int MaxSmartCardNumberLength = 11;
	private string _selectedLogo = string.Empty;
    private SubscriptionPlan? _selectedPlan;
    private List<ServiceProvider> _serviceProvider = new();
	private ObservableCollection<AllBeneficiaries> _allBeneficiaries;

	private AllBeneficiaries? _selectedBeneficiary;
	public TvBill()
	{
		InitializeComponent();

		LoadService();
        SubscriptionPlanList.ItemsSource = SubscriptionPlans.All;
        LoadBeneficiaries();
		CheckDetails();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private void LoadService()
	{
		_serviceProvider = new List<ServiceProvider>
		{
			new ServiceProvider{Logo="dstv.png",TVProviders="DSTV Subscription", TVSubscriptions="DSTV"},
			new ServiceProvider{Logo="gotv.png",TVProviders="GOTV Subscription", TVSubscriptions="GOTV"},
			new ServiceProvider{Logo="startimes.png",TVProviders="Startimes Subscription", TVSubscriptions="StarTimes"},		
		};
		TVSubscriptionList.ItemsSource = _serviceProvider;
	}
    
	private void LoadBeneficiaries()
	{
		_allBeneficiaries = new ObservableCollection<AllBeneficiaries>
		{
            new() { Logo = "dstv.png", TVProviders = "DSTV Subscription", TVSubscriptions = "DSTV", UserName = "Janet Doe Kehnide", SmartCardNumber = "1234567890"},
			new() { Logo = "gotv.png", TVProviders = "GOTV Subscription", TVSubscriptions = "GOTV", UserName = "John Doe Kehnide", SmartCardNumber = "1234567689"},
			new() { Logo = "startimes.png", TVProviders = "StarTimes Subscription", TVSubscriptions = "StarTimes", UserName = "Jane Bush Richard", SmartCardNumber ="12345678901"},
			new() { Logo = "dstv.png", TVProviders = "DSTV Subscription", TVSubscriptions = "DSTV", UserName = "Janet Ezekiel Kehnide", SmartCardNumber="2345678901" },
			new() { Logo = "gotv.png", TVProviders = "GOTV Subscription", TVSubscriptions = "GOTV", UserName = "Princess Doe Kehnide", SmartCardNumber= "5678901243" },
			new() { Logo = "startimes.png", TVProviders = "StarTimes Subscription", TVSubscriptions = "StarTimes", UserName = "Blessing Okon Kehnide", SmartCardNumber="34567890122"},
			new() { Logo = "dstv.png", TVProviders = "DSTV Subscription", TVSubscriptions = "DSTV", UserName = "Wisdom Doe John", SmartCardNumber="3456789012"},
			new() { Logo = "gotv.png", TVProviders = "GOTV Subscription", TVSubscriptions = "GOTV", UserName = "Doe David Kehnide", SmartCardNumber="4567890123" },
			new() { Logo = "startimes.png", TVProviders = "StarTimes Subscription", TVSubscriptions = "StarTimes",UserName = "Dawn John Kehnide", SmartCardNumber="56789012345"},
			new() { Logo = "dstv.png", TVProviders = "DSTV Subscription", TVSubscriptions = "DSTV",UserName = "Joseph Doe Micheal", SmartCardNumber="4567890234"},
			new() { Logo = "gotv.png", TVProviders = "GOTV Subscription", TVSubscriptions = "GOTV",UserName = "Janet Doe Kehnide", SmartCardNumber="2457907532"},
			new() { Logo = "startimes.png", TVProviders = "StarTimes Subscription", TVSubscriptions = "StarTimes", UserName = "Doe David Kehnide", SmartCardNumber="08654312384"},
			new() { Logo = "dstv.png", TVProviders = "DSTV Subscription", TVSubscriptions = "DSTV", UserName = "Princess Doe Kehnide" , SmartCardNumber="6543278965"},
			new() { Logo = "gotv.png", TVProviders = "GOTV Subscription", TVSubscriptions = "GOTV",UserName = "Wisdom Doe John" , SmartCardNumber="2576598157"},
			new() { Logo = "startimes.png", TVProviders = "StarTimes Subscription", TVSubscriptions = "StarTimes" , UserName = "Janet Ezekiel Kehnide", SmartCardNumber="12375488565"},
			
		}
        ;
		BindableLayout.SetItemsSource( AllBeneficiariesStack, _allBeneficiaries.Take(7).ToList());
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
			Card.ScaleTo(1,250,Easing.SpringOut));
	}
	private async Task HidePopupAsync(VisualElement Overlay, VisualElement Card)
	{
        await Task.WhenAll(
            Overlay.FadeTo(0, 200),
            Card.FadeTo(0, 200),
            Card.ScaleTo(0.8, 250, Easing.SpringOut));

		Overlay.IsVisible=false;
		Card.IsVisible=false;
    }
	private void CheckDetails()
	{
        string smartCard = SmartCardNumber.Text?.Trim() ?? "";

		bool hasProvider = !string.IsNullOrWhiteSpace(SelectTVSubscriptionEntry.Text);
		bool hasSmartCard = smartCard.Length >= MinSmartCardNumberLength && smartCard.Length <= MaxSmartCardNumberLength
							 && smartCard.All(Char.IsDigit);

		bool hasPlan = _selectedPlan is not null;

		bool isValid = hasProvider && hasSmartCard && hasPlan;

		ProceedBorder.IsEnabled = isValid;
    }
	private async void OnRightwardAngleTapped(object sender, TappedEventArgs e)
	{
		TVSubscriptionSearch.Text = string.Empty;
		TVSubscriptionList.ItemsSource = _serviceProvider;

		await ShowPopupAsync(TVPopupOverlay, TVPopupCard);
    }
	private async void OnTVPopupOverlayTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(TVPopupOverlay, TVPopupCard);
	}
	private async void OnTVPopupCardTapped(object sender, TappedEventArgs e)
	{
		//takes tabs without closing the popup
	}
    private async void OnPopupBackIconTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(TVPopupOverlay, TVPopupCard);
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
		string search = e.NewTextValue?.Trim() ?? "";

		if (string.IsNullOrWhiteSpace(search))
		{
			TVSubscriptionList.ItemsSource = _serviceProvider;
		}
		else
		{
            TVSubscriptionList.ItemsSource = _serviceProvider
				.Where(p => p.TVProviders.Contains(search, StringComparison.OrdinalIgnoreCase)
				||p.TVSubscriptions.Contains(search, StringComparison.OrdinalIgnoreCase))
				.ToList();
		}
    }
    private void OnSubscriptionPlanSelected(object sender, SelectionChangedEventArgs e)
    {
        _selectedPlan = e.CurrentSelection.FirstOrDefault() as SubscriptionPlan;
		//bundle picked before a provider: fill the provider in from the bundle
		if(_selectedPlan is not null && string.IsNullOrWhiteSpace(SelectTVSubscriptionEntry.Text))
		{
			var provider =  _serviceProvider.FirstOrDefault(p =>p.TVSubscriptions == _selectedPlan.Provider);
			if (provider is not null)
			{
				SelectTVSubscriptionEntry.Text = provider.TVProviders;
				_selectedLogo = provider.Logo;
			}
		}
        CheckDetails();
    }
    private async void OnSelectTVSubscriptionEntry(object sender, SelectionChangedEventArgs e)
    {
		if (e.CurrentSelection.FirstOrDefault() is not ServiceProvider provider)
			return;
        TVSubscriptionList.SelectedItem = null;
		SelectTVSubscriptionEntry.Text = $"{provider.TVProviders}({provider.TVSubscriptions})";
		_selectedLogo = provider.Logo;

		ShowPlansFor(provider.TVSubscriptions);

        await HidePopupAsync(TVPopupOverlay, TVPopupCard);
    }
	private void ShowPlansFor(string provider)
	{
		_selectedPlan = null;
		SubscriptionPlanList.SelectedItem = null;
		SubscriptionPlanList.ItemsSource = SubscriptionPlans.All
            .Where(p => p.Provider == provider)
            .ToList();

        CheckDetails();
    }
	private void OnSmartCardNumberTextChanged(object sender, TextChangedEventArgs e)
	{
		CheckDetails();

		if (SmartCardNumber.Text?.Length == MaxSmartCardNumberLength)
			SmartCardNumber.Unfocus();
	}
	private void ShowSelectedBeneficiaryCard(AllBeneficiaries beneficiary)
	{
		_selectedBeneficiary = beneficiary;
		
		SelectedBeneficiaryLogo.Source = beneficiary.Logo;
		SelectedBeneficiaryName.Text = beneficiary.UserName;
        SelectedBeneficiarySmartCardNumber.Text = $"SmartCard Number: {beneficiary.SmartCardNumber}";
        SelectedBeneficiaryCard.IsVisible = true;

        SelectTVSubscriptionEntry.Text = beneficiary.TVProviders;  
        _selectedLogo = beneficiary.Logo;
		SmartCardNumber.Text = beneficiary.SmartCardNumber;

		ShowPlansFor(beneficiary.TVSubscriptions);
    }
	private void HideSelectedBeneficiaryCard()
	{
		_selectedBeneficiary = null;
		SelectedBeneficiaryCard.IsVisible = false;
	}
    private async void OnClearSelectedBeneficiaryTapped(object sender, TappedEventArgs e)
    {
		HideSelectedBeneficiaryCard();
		SmartCardNumber.Text = string.Empty;
		SelectTVSubscriptionEntry.Text = string.Empty;
		_selectedLogo = string.Empty;

        _selectedPlan = null;
        SubscriptionPlanList.SelectedItem = null;
        SubscriptionPlanList.ItemsSource = SubscriptionPlans.All;

        CheckDetails();
    }
    //beneficiaries
    private async void OnViewAllTapped(object sender, TappedEventArgs e)
    {
        BeneficiarySearchEntry.Text = string.Empty;
        SubscriptionBeneficiaries.ItemsSource = _allBeneficiaries;

        await ShowPopupAsync(BeneficiaryPopupOverlay, BeneficiaryPopupCard);
    }
    //beneficiarypopup
    private async void OnBeneficiaryTapped(object sender, TappedEventArgs e)
    {
        if(sender is BindableObject item && item.BindingContext is AllBeneficiaries beneficiary)
			ShowSelectedBeneficiaryCard(beneficiary);
    }
    private async void OnBeneficiaryPopupOverlayTapped(object sender, TappedEventArgs e)
    {
		await HidePopupAsync(BeneficiaryPopupOverlay, BeneficiaryPopupCard);
    }
    private async void OnBeneficiaryPopupCardTapped(object sender, TappedEventArgs e)
    {
		//takes tabs
    }
    private void OnBeneficiarySearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string search = e.NewTextValue?.Trim() ?? "";
		
            SubscriptionBeneficiaries.ItemsSource = string.IsNullOrEmpty(search)
            ? _allBeneficiaries
            : _allBeneficiaries
                .Where(b => b.UserName.Contains(search, StringComparison.OrdinalIgnoreCase)
                         || b.SmartCardNumber.Contains(search))
                .ToList();
        
    }
	private void OnClearSearchEntryTapped(object sender, TappedEventArgs e)
	{
		BeneficiarySearchEntry.Text = string.Empty;
    }
	private async void OnSelectBeneficiaryEntry(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is not AllBeneficiaries beneficiary)
			return;
        SubscriptionBeneficiaries.SelectedItem = null;
		ShowSelectedBeneficiaryCard (beneficiary);

		await HidePopupAsync(BeneficiaryPopupOverlay, BeneficiaryPopupCard);

	}
	private async void OnProceedBorderTapped(object sender, TappedEventArgs e)
	{
		if (Loading.IsLoading)
			return;
		try
		{
			ProceedTxt.IsVisible = false;
			ProceedIndicator.IsVisible = true;
			ProceedIndicator.IsRunning = true;
			await Loading.ShowAsync("oading...");
			await Task.Delay(500);

			string smartCard = SmartCardNumber.Text?.Trim() ?? "";

			string customerName = _selectedBeneficiary?.UserName
				?? _allBeneficiaries.FirstOrDefault(b => b.SmartCardNumber == smartCard)?.UserName
				?? "";
			await Navigation.PushAsync(new ElectricitySummaryPage(
				companyLogo: _selectedLogo,
				providerName:SelectTVSubscriptionEntry.Text,
				meterNumber:smartCard,
				customerName:customerName,
				amount:_selectedPlan!.Price,
				billType:"TV",
				numberLabel:"SmartCard Number",
				planName:$"{_selectedPlan.PlanDescription}({_selectedPlan.Validity})"));
		}
		catch(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
			await DisplayAlert("Error", "Something went wrong. Please try again", "OK");
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