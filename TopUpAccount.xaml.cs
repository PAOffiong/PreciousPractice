
using CommunityToolkit.Maui.Core.Platform;
using System.Collections.ObjectModel;
using System.Linq;
namespace PreciousPractice;

public class Bank
{
    public string BankName { get; set; }
    public string BankCode { get; set; }
    public string IconSource { get; set; }
}
public partial class TopUpAccount : ContentPage
{   
    private List<Bank> _allBanks = new();
    private ObservableCollection<Bank> _filteredBanks = new();
	public TopUpAccount()
	{
		InitializeComponent();
        LoadBanks();
	}
    private void LoadBanks()
    {
        _allBanks = new List<Bank>
        {
            new Bank {BankName = "Okpe Microfinance Bank" , BankCode = "50665" ,IconSource ="okpe_bank" },
            new Bank {BankName = "Access Bank" , BankCode = "044" ,IconSource ="access_bank" },
            new Bank {BankName = "Alat by Wema Bank" , BankCode = "035" ,IconSource ="alat_wema" },
            new Bank {BankName = "Fidelity Bank" , BankCode = "070" ,IconSource ="fidelity_bank" },
            new Bank {BankName = "First Bank" , BankCode = "011" ,IconSource ="first_bank" },
            new Bank {BankName = "Gtbank" , BankCode = "058" ,IconSource ="gtco_icon" },
            new Bank {BankName = "Moniepoint" , BankCode = "50515" ,IconSource ="moniepoint_bank" },
            new Bank {BankName = "Polaris Bank" , BankCode = "076" ,IconSource ="polaris_bank" },
            new Bank {BankName = "Zenith Bank" , BankCode = "057" ,IconSource ="zenith_bank" },
            new Bank {BankName = "Okpe Microfinance Bank" , BankCode = "50665" ,IconSource ="okpe_bank" },
            new Bank {BankName = "Access Bank" , BankCode = "044" ,IconSource ="access_bank" },
            new Bank {BankName = "Alat by Wema Bank" , BankCode = "035" ,IconSource ="alat_wema" },
            new Bank {BankName = "Fidelity Bank" , BankCode = "070" ,IconSource ="fidelity_bank" },
            new Bank {BankName = "First Bank" , BankCode = "011" ,IconSource ="first_bank" },
            new Bank {BankName = "Gtbank" , BankCode = "058" ,IconSource ="gtco_icon" },
            new Bank {BankName = "Moniepoint" , BankCode = "50515" ,IconSource ="moniepoint_bank" },
            new Bank {BankName = "Polaris Bank" , BankCode = "076" ,IconSource ="polaris_bank" },
            new Bank {BankName = "Zenith Bank" , BankCode = "057" ,IconSource ="zenith_bank" },
            
        };

        _filteredBanks = new ObservableCollection<Bank>(_allBanks);
        BankList.ItemsSource = _filteredBanks;
    }
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
    private async Task ShowPopUpAsync()
    {
        PopUpOverlay.Opacity = 0;
        BankNamePopUp.Scale = 0.8;
        PopUpOverlay.IsVisible = true;
        BankNamePopUp.IsVisible = true;

        await Task.WhenAll(
            PopUpOverlay.FadeTo(1, 200),
            BankNamePopUp.ScaleTo(1, 250, Easing.SpringOut)
        );
    }
    private async Task HidePopUpAsync()
    {
        await Task.WhenAll(
            PopUpOverlay.FadeTo(0, 180),
            BankNamePopUp.ScaleTo(0.8, 180)
        );

        PopUpOverlay.IsVisible = false;
        BankNamePopUp.IsVisible = false;
        PopUpOverlay.Opacity = 1;
        BankNamePopUp.Scale = 1;
    }
    private async void OnRightAngleTriangleTapped(object sender, TappedEventArgs e)
    {
        if (PopUpOverlay.IsVisible)
            await HidePopUpAsync();

        else
            await ShowPopUpAsync();
    }
    private async void OnPopUpOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopUpAsync();
    }
    private void OnBankNamePopUpTapped(object sender, TappedEventArgs e)
    {
        
    }
    //popup search entry
    private void OnBankSearchEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.Trim() ?? string.Empty;

        var matches = string.IsNullOrEmpty(searchText)
            ? _allBanks
            : _allBanks.Where(b => b.BankName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

        _filteredBanks.Clear();
        foreach ( var bank in matches )
            _filteredBanks.Add(bank);
    }
    private void OnClearSearchEntryTapped(object sender, TappedEventArgs e)
    {
        BankSearchEntry.Text = string.Empty;
    }
    //Entry for selected Bank
    private async void OnSelectBankEntry(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Bank selected)
            return;
        SelectBankEntry.Text = selected.BankName;

        UpdateContinueButton();

        BankList.SelectedItem = null;
        BankSearchEntry.Text = string.Empty;

        await HidePopUpAsync();
    }
    private void UpdateContinueButton()
    {
        string account = AccountNumberEntry.Text?.Trim() ?? "";

        bool hasBank = !string.IsNullOrWhiteSpace(SelectBankEntry.Text);
        bool accountValid = account.Length == 10 && account.All(char.IsDigit);

        bool isValid = hasBank && accountValid;
        ContinueBorder.IsEnabled = isValid;
        ContinueBorder.Opacity = isValid ? 1.0 : 0.5;
    }

    private async void OnEntriesTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateContinueButton();

        // Hide the keyboard once the 10-digit account number is complete
        if (sender == AccountNumberEntry && AccountNumberEntry.Text?.Length == 10)
        {
            AccountNumberEntry.Unfocus();
            await AccountNumberEntry.HideKeyboardAsync(CancellationToken.None);
        }
    }
   
    private async void NavToAddMoneytoBank(object sender, EventArgs e)
    {
        if (Loading.IsLoading)
            return;
        string bankName = SelectBankEntry.Text;
        string accountNumber = AccountNumberEntry.Text;
        try
        {
            ContinueTxt.IsVisible = false;

            ContinueIndicator.IsVisible = true;
            ContinueIndicator.IsRunning = true;

            await Loading.ShowAsync("Verifying account...");
            await Task.Delay(500);

            await Navigation.PushAsync(new AddMoneytoBank(bankName, accountNumber));
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
               
            await DisplayAlert("Error", "Could not verify account.Please try again", "OK");
        }
        finally
        {
            await Loading.HideAsync();
            ContinueTxt.IsVisible = true;

            ContinueIndicator.IsVisible = false;
            ContinueIndicator.IsRunning = false;
        }


    }
}