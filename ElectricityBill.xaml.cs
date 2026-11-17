using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace PreciousPractice;

public partial class ElectricityBill : ContentPage
{
	public class ServiceProvider
	{
		public string CompanyLogo {  get; set; }
		public string CompanyName { get; set; }
		public string ServiceName { get; set; }
	}
	public class AllBeneficiaries
	{
		public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string ServiceName { get; set; }
        public string UserName { get; set; }
		public string MeterNumber { get; set; }
	}
	private const int MinMeterNumberLength = 11;
	private const int MaxMeterNumberLength = 12;
	private const decimal MinimumAmount = 500m;
	private List<ServiceProvider> _serviceProviders = new();
	private ObservableCollection<AllBeneficiaries> _allBeneficiaries;

	//saved beneficiary
	private AllBeneficiaries? _selectedBeneficiary;
    //logo of choosen company, sent to the next page
    private string _selectedLogo = string.Empty;
    public ElectricityBill()
	{
		InitializeComponent();

		LoadServices();
		LoadBeneficiaries();
		CheckDetails();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private void LoadServices()
	{
	      _serviceProviders = new List<ServiceProvider>
		{
			new ServiceProvider{CompanyLogo ="eko_distribution.png",CompanyName="EKO Electric Distribution", ServiceName="Postpaid"},
			new ServiceProvider{CompanyLogo = "eko_distribution.png", CompanyName="EKO Electric Distribution", ServiceName="Prepaid"},
			new ServiceProvider{CompanyLogo ="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Postpaid"},
			new ServiceProvider{CompanyLogo ="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Prepaid"},
			new ServiceProvider{CompanyLogo ="eko_distribution.png",CompanyName="EKO Electric Distribution", ServiceName="Postpaid"},
			new ServiceProvider{CompanyLogo = "eko_distribution.png", CompanyName="EKO Electric Distribution", ServiceName="Prepaid"},
			new ServiceProvider{CompanyLogo ="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Postpaid"},
			new ServiceProvider{CompanyLogo ="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Prepaid"},
			new ServiceProvider{CompanyLogo ="eko_distribution.png",CompanyName="EKO Electric Distribution", ServiceName="Postpaid"},
			new ServiceProvider{CompanyLogo = "eko_distribution.png", CompanyName="EKO Electric Distribution", ServiceName="Prepaid"},
			new ServiceProvider{CompanyLogo ="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Postpaid"},
			new ServiceProvider{CompanyLogo ="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Prepaid"},

		};
		ServiceProviderList.ItemsSource = _serviceProviders;

	}
	private void LoadBeneficiaries()
	{
	    _allBeneficiaries= new ObservableCollection<AllBeneficiaries>
		{
			new (){CompanyLogo="eko_distribution.png", CompanyName="EKO Electric Distribution", ServiceName="Postpaid", UserName="Janet Doe Kehnide", MeterNumber="123456789012"},
			new (){CompanyLogo="ikeja_distribution.png",CompanyName="Ikeja Electric Distribution",ServiceName="Postpaid", UserName="John Doe", MeterNumber="987654321987"},
			new (){CompanyLogo="eko_distribution.png", CompanyName="EKO Electric Distribution",ServiceName="Prepaid",UserName="Emmanuel Kehnide", MeterNumber="456789012345"},
			new (){CompanyLogo="ikeja_distribution.png",CompanyName="Ikeja Electric Distribution", ServiceName="Prepaid",UserName="Patience Doe Chuks", MeterNumber="112233445566"},
			new (){CompanyLogo="eko_distribution.png", CompanyName="EKO Electric Distribution",ServiceName="Postpaid", UserName="John Doe Kehnide", MeterNumber="876543210987"},
			new (){CompanyLogo="ikeja_distribution.png",CompanyName="Ikeja Electric Distribution", ServiceName="Postpaid",UserName="Janet Doe Kehnide", MeterNumber="102938475610"},
			new (){CompanyLogo="eko_distribution.png", CompanyName="EKO Electric Distribution",ServiceName="Prepaid", UserName="Jane Doe Kehnide", MeterNumber="234567890123"},
			new (){CompanyLogo="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Prepaid",UserName="Janet Doe Kehnide", MeterNumber="135792468012"},
			new (){CompanyLogo="eko_distribution.png", CompanyName="EKO Electric Distribution", ServiceName="Postpaid",UserName="Janet Doe Kehnide", MeterNumber="908172635410"},
			new (){CompanyLogo="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution",ServiceName="Postpaid",UserName="Precious Doe Kehnide", MeterNumber="654321098765"},
			new (){CompanyLogo="eko_distribution.png", CompanyName="EKO Electric Distribution",ServiceName="Prepaid",UserName="Janet Doe Kehnide", MeterNumber="345678901234"},
			new (){CompanyLogo="ikeja_distribution.png", CompanyName="Ikeja Electric Distribution", ServiceName="Prepaid",UserName="Janet Doe Kehnide", MeterNumber="998877665544"},
		};
		BindableLayout.SetItemsSource(AllBeneficiariesStack, _allBeneficiaries.Take(7).ToList());
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
			Card.ScaleTo(1, 250, Easing.SpringOut)
			);
	}
	private async Task HidePopupAsync(VisualElement Overlay, VisualElement Card)
	{
		await Task.WhenAll(
			Overlay.FadeTo(0, 200),
			Card.FadeTo(0, 200),
			Card.ScaleTo(0.8, 250, Easing.SpringOut)
			);
		Overlay.IsVisible = false;
		Card.IsVisible = false;
	}
	private void CheckDetails()
	{
		string meter = MeterNumber.Text?.Trim() ?? "";
		string amountEntry = SelectAmountEntry.Text?.Trim() ?? "";

		bool hasProvider = !string.IsNullOrWhiteSpace(SelectServiceProviderEntry.Text);
		bool hasMeter = meter.Length >= MinMeterNumberLength &&
						meter.Length <= MaxMeterNumberLength &&
						meter.All(char.IsDigit);
		bool hasAmount = decimal.TryParse(SelectAmountEntry.Text, out decimal amount) && amount >=MinimumAmount;
		bool isValid = hasProvider && hasMeter && hasAmount;

		ProceedBorder.IsEnabled = isValid;
	}
	private async void OnRightwardAngleTapped(object sender, TappedEventArgs e)
	{
		ElectricityCompanySearch.Text = string.Empty;
		ServiceProviderList.ItemsSource = _serviceProviders;

		await ShowPopupAsync(ElectricityPopupOverlay, ElectricityPopupCard);
	}
	private async void OnMeterNumberTextChanged(object sender, TextChangedEventArgs e)
	{
		if (_selectedBeneficiary is not null && MeterNumber.Text != _selectedBeneficiary.MeterNumber)
			HideSelectedBeneficiary();

		CheckDetails();

		if (MeterNumber.Text?.Length == MaxMeterNumberLength)
			MeterNumber.Unfocus();
	}
	
	private void OnAmountTextChanged(object sender, TextChangedEventArgs e)
	{
		CheckDetails();
	}
	private void OnAmountTapped(object sender, TappedEventArgs e)
	{
		if(e.Parameter is string amount)
			SelectAmountEntry.Text = amount;
	}
	//ElectricityPopup
	private async void OnElectricityPopupOverlayTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(ElectricityPopupOverlay, ElectricityPopupCard);
	}
	private async void OnElectricityPopupCardTapped(object sender, TappedEventArgs e)
	{
		//takes taps without closing the popup
	}
	private async void OnPopupBackIconTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync(ElectricityPopupOverlay,ElectricityPopupCard);
	}
	private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
	{
		string search = e.NewTextValue?.Trim() ?? "";
       
		if (string.IsNullOrWhiteSpace(search))
        {
            ServiceProviderList.ItemsSource = _serviceProviders;
        }
        else
        {
            ServiceProviderList.ItemsSource = _serviceProviders
                .Where(p => p.CompanyName.Contains(search, StringComparison.OrdinalIgnoreCase)
                || p.ServiceName.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

    }
	private async void OnSelectServiceProviderEntry(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is not ServiceProvider provider)
			return;
		ServiceProviderList.SelectedItem = null;
		SelectServiceProviderEntry.Text = $"{provider.CompanyName}({provider.ServiceName})";
		_selectedLogo = provider.CompanyLogo;
		CheckDetails();

		await HidePopupAsync(ElectricityPopupOverlay, ElectricityPopupCard);
	}
    //selected Beneficiary
    private void ShowSelectedBeneficiary(AllBeneficiaries beneficiary)
    {
        _selectedBeneficiary = beneficiary;

        SelectedBeneficiaryCompanyLogo.Source = beneficiary.CompanyLogo;
        SelectedBeneficiaryName.Text = beneficiary.UserName;
        SelectedBeneficiaryMeterNumber.Text = $"Meter Number: {beneficiary.MeterNumber}";
        SelectedBeneficiaryCard.IsVisible = true;

        // Fill the form from the beneficiary
        SelectServiceProviderEntry.Text = $"{beneficiary.CompanyName} ({beneficiary.ServiceName})";
		_selectedLogo = beneficiary.CompanyLogo;
		MeterNumber.Text = beneficiary.MeterNumber;

        CheckDetails();
    }

    private void HideSelectedBeneficiary()
    {
        _selectedBeneficiary = null;
        SelectedBeneficiaryCard.IsVisible = false;
    }
    private void OnClearSelectedBeneficiaryTapped(object sender, TappedEventArgs e)
    {
        HideSelectedBeneficiary();
        MeterNumber.Text = string.Empty;
        SelectServiceProviderEntry.Text = string.Empty;
		_selectedLogo = string.Empty;
        CheckDetails();
    }

	//view all beneficiary pop
    private async void OnBeneficiaryTapped(object sender, TappedEventArgs e)
    {
        if (sender is BindableObject item && item.BindingContext is AllBeneficiaries beneficiary)
            ShowSelectedBeneficiary(beneficiary);
    }
    private async void OnViewAllTapped(object sender, TappedEventArgs e)
    {
		BeneficiarySearchEntry.Text = string.Empty ;
        MeterBeneficiaries.ItemsSource = _allBeneficiaries;
        
		await ShowPopupAsync(BeneficiaryPopupOverlay, BeneficiaryPopupCard);
    }
    private async void OnBeneficiaryPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopupAsync(BeneficiaryPopupOverlay, BeneficiaryPopupCard);
    }
    private async void OnBeneficiaryPopupCardTapped(object sender, TappedEventArgs e)
    {
        //takes taps without closing the popup
    }
    private async void OnBeneficiarySearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string search = e.NewTextValue?.Trim() ?? "";

        MeterBeneficiaries.ItemsSource = string.IsNullOrEmpty(search)
            ? _allBeneficiaries
            : _allBeneficiaries
                .Where(b => b.UserName.Contains(search, StringComparison.OrdinalIgnoreCase)
                         || b.MeterNumber.Contains(search))
                .ToList();
    }
    private async void OnSelectBeneficiaryEntry(object sender, SelectionChangedEventArgs e)
	{
        if (e.CurrentSelection.FirstOrDefault() is not AllBeneficiaries beneficiary)
            return;

        MeterBeneficiaries.SelectedItem = null;
        ShowSelectedBeneficiary(beneficiary);

        await HidePopupAsync(BeneficiaryPopupOverlay, BeneficiaryPopupCard);
    }
    private async void OnClearSearchEntryTapped(object sender, TappedEventArgs e)
    {
        BeneficiarySearchEntry.Text = string.Empty;
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
            
			string meter = MeterNumber.Text?.Trim() ?? "";

            // The picked beneficiary's name, or look the meter number up in the saved list
            string customerName = _selectedBeneficiary?.UserName
                ?? _allBeneficiaries.FirstOrDefault(b => b.MeterNumber == meter)?.UserName
                ?? "";

            decimal amount = decimal.Parse(SelectAmountEntry.Text);

            await Loading.NavigateAsync(Navigation, () => new ElectricitySummaryPage(
                companyLogo: _selectedLogo,
                providerName: SelectServiceProviderEntry.Text,
                meterNumber: meter,
                customerName: customerName,
                amount: amount));
        }
		catch(Exception ex)
		{
			System.Diagnostics.Debug.WriteLine(ex);
			await DisplayAlert("Error", "Something went wrong.Please try again", "OK");
		}
		finally
		{
            ProceedTxt.IsVisible = true;
            ProceedIndicator.IsVisible = false;
            ProceedIndicator.IsRunning = false;
        }
	}
}