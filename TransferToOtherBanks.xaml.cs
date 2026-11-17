using PreciousPractice.Helpers;
using System.Collections.ObjectModel;

namespace PreciousPractice;

public partial class TransferToOtherBanks : ContentPage
{
    public class RecentTransfers
    {
        public string BankIcon { get; set; }
        public string BankName { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string AccountNumber { get; set; }
    }
    public class Recipients
    {
        public string BankIcon { get; set; }
        public string BankName { get; set; }
        public string PersonName { get; set; }
        public string AccountNumber { get; set; }
    }
    private List<Recipients> allRecipients = new();
    private ObservableCollection<Recipients> filteredRecipients = new();
    private string _selectedAccountName = string.Empty;
    private string _selectedAccountNumber = string.Empty;
    private string _selectedBankIcon = string.Empty;
    private string _selectedBankName = string.Empty;

    public TransferToOtherBanks()
    {
        InitializeComponent();
        LoadRecentTransfers();
        LoadBeneficiaries();
        LoadRecipients();
    }
    //temporary account number
    protected override void OnAppearing()
    {
        base.OnAppearing();
        AccountNumberLabel.Text = DefaultAccountNumber.TemporaryAccountNumber;
        AvailableBalance.Text = DefaultAccountBalance.Format(DefaultAccountBalance.SelectedBalance);
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private void LoadRecentTransfers()
    {
        var recentTransfers = new List<RecentTransfers>
        {

            new RecentTransfers{BankIcon = "wema_icon.png",BankName= "Wema Bank", PersonName = "Sarah", PersonSurName = "Chijioke", AccountNumber = "2718281828"},
            new RecentTransfers{BankIcon = "first_bank.png",BankName= "First Bank", PersonName = "John Doe",PersonSurName = "Kehinde", AccountNumber = "2468135790"},
            new RecentTransfers {BankIcon = "gtco_icon.png", BankName= "GTCO bank", PersonName = "John Doe",PersonSurName = "Kehinde", AccountNumber = "1234567890"},
            new RecentTransfers{BankIcon = "fidelity_bank.png",BankName = "Fidelity Bank", PersonName = "Sarah",PersonSurName = "Chijioke", AccountNumber ="1357924680"},
            new RecentTransfers{ BankIcon = "polaris_bank.png",BankName="Polaris Bank", PersonName = "Sarah",PersonSurName = "Chijioke",AccountNumber = "7890123456"},
            new RecentTransfers{BankIcon = "zenith_bank.png", BankName="Zenith Bank",PersonName = "John Doe",PersonSurName = "Kehinde", AccountNumber ="9876543210"},
        };
        BindableLayout.SetItemsSource(RecentTransactions, recentTransfers);
    }
    // Shared list: anyone saved with the "Add to beneficiaries" toggle appears here automatically
    private void LoadBeneficiaries()
    {
        
        BindableLayout.SetItemsSource(AllBeneficiaries, SavedBeneficiaries.All);
    }
    private void LoadRecipients()
    {
        allRecipients = new List<Recipients>
        {

            new Recipients{BankIcon = "alat_wema.png", BankName = "Alat by Wema Bank", PersonName= "Sarah Micheal", AccountNumber = "3526278937"},
            new Recipients{BankIcon = "first_bank.png",BankName = "First Bank", PersonName = "John Doe Kehinde",AccountNumber = "2468135790"},
            new Recipients{BankIcon = "gtco_icon.png",BankName = "GTCO Bank",PersonName = "Janet Doe Emmanuel",AccountNumber = "1234567890"},
            new Recipients{BankIcon = "wema_icon.png",BankName = "Wema Bank",PersonName = "Patience Chijioke",AccountNumber = "2718281828"},
            new Recipients{BankIcon = "zenith_bank.png",BankName = "Zenith Bank",PersonName = "Sarah Eze",AccountNumber ="9876543210"},
            new Recipients {BankIcon = "polaris_bank.png",BankName = "Polaris Bank", PersonName = "Tom Doe Kehinde", AccountNumber = "7890123456"},
            new Recipients{BankIcon = "moniepoint_bank.png",BankName = "MoniePoint Microfinance Bank",PersonName = "Sarah Okon",AccountNumber = "4512327626"},
            new Recipients{BankIcon = "fidelity_bank.png",BankName = "Fidelity Bank",PersonName = "Happiness Kehinde",AccountNumber ="1357924680"},
            new Recipients{BankIcon = "access_bank.png",BankName = "Access Bank",PersonName = "John Doe Kehinde",AccountNumber = "0123456789"},
            new Recipients{BankIcon = "wema_icon.png",BankName = "Wema Bank",PersonName = "Sarah Adichie",AccountNumber = "2718281828"},
            new Recipients{BankIcon = "first_bank.png",BankName = "First Bank", PersonName = "Sarah Chijioke",AccountNumber = "2468135790"},
            new Recipients{BankIcon = "okpe_bank.png",BankName = "Okpe Microfinance Bank",PersonName = "John Doe Kehinde",AccountNumber = "2652789298"},
        };
        filteredRecipients = new ObservableCollection<Recipients>(allRecipients);
        TransferRecipients.ItemsSource = filteredRecipients;

    }
    private async void OnRightAngleTriangleTapped(object sender, TappedEventArgs e)
    {
        if (PopUpOverlay.IsVisible)
            await HidePopUpAsync();

        else
            await ShowPopUpAsync();

    }
    private async Task ShowPopUpAsync()
    {
        PopUpOverlay.Opacity = 0; //visblity should be 0 
        TransferActivityPopUp.Scale = 0.8;
        PopUpOverlay.IsVisible = true;
        TransferActivityPopUp.IsVisible = true;

        await Task.WhenAll(
            PopUpOverlay.FadeTo(1, 200), //200 means milliseconds
            TransferActivityPopUp.ScaleTo(1, 250, Easing.SpringOut) //when the popup is showing, it should come out little by little till its normal size which is 1
             //by springout from the bottom of the page within 250 milliseconds
        );
    }
    private async Task HidePopUpAsync()
    {
        await Task.WhenAll(
            PopUpOverlay.FadeTo(0, 180),
            TransferActivityPopUp.ScaleTo(0.8, 180)
        );

        PopUpOverlay.IsVisible = false;
        TransferActivityPopUp.IsVisible = false;
        PopUpOverlay.Opacity = 1;
        TransferActivityPopUp.Scale = 1;
    }
    private async void OnSelectBankTapped(object sender, TappedEventArgs e)
    {
        await ShowPopUpAsync();
    }
    private async void OnPopUpOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopUpAsync();
    }
    // receives all the taps inside the popup and allow everything to still function
    private void OnTransferActivityPopUpTapped(object sender, TappedEventArgs e)
    {

    }
    //pop entry
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? "";

        filteredRecipients.Clear();

        foreach (var recipient in allRecipients)
        {
            if (string.IsNullOrEmpty(searchText) ||
                recipient.BankName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                recipient.PersonName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                filteredRecipients.Add(recipient);
            }
        }
    }
    private void OnClearSearchEntryTapped(object sender, TappedEventArgs e)
    {
        RecipientSearchEntry.Text = string.Empty;
    }
    //Entry for selected Bank
    private async void OnSelectBankEntry(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Recipients selected)
            return;
        SelectBankEntry.Text = selected.BankName;
        CheckTransferDetails();

        TransferRecipients.SelectedItem = null;
        RecipientSearchEntry.Text = string.Empty;

        await HidePopUpAsync();
    }
    private async void OnViewTapped(object sender, TappedEventArgs e)
    {
        if (PopUpOverlay.IsVisible)
            await HidePopUpAsync();
        else
            await ShowPopUpAsync();
    }
    private async void OnViewAllTapped(object sender, TappedEventArgs e)
    {
        if (PopUpOverlay.IsVisible)
            await HidePopUpAsync();

        else
            await ShowPopUpAsync();
    }
    private async void OnPopupOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopUpAsync();
    }
    private void OnEntriesTextChanged(object sender, TextChangedEventArgs e)
    {
        CheckTransferDetails();
    }

    private void CheckTransferDetails()
    {
        string accountNumber = BeneficiaryAccountEntry.Text?.Trim() ?? "";
        string bankName = SelectBankEntry.Text?.Trim() ?? "";

        bool accountNumberValid =
            accountNumber.Length == 10 &&
            accountNumber.All(char.IsDigit);

        bool bankSelected = !string.IsNullOrEmpty(bankName);

        bool isValid = accountNumberValid && bankSelected;

        ProceedBorder.IsEnabled = isValid;
        ProceedBorder.Opacity = isValid ? 1.0 : 0.5;

    }
    private async void OnBindingTapped(object sender, TappedEventArgs e)
    {
        if (sender is not BindableObject tappedItem)
            return;

        switch (tappedItem.BindingContext)
        {
            case RecentTransfers recent:
                await OpenAccountDetailsAsync($"{recent.PersonName} {recent.PersonSurName}", recent.AccountNumber, recent.BankIcon, recent.BankName);
                break;

            case SavedBeneficiary beneficiary:
                await OpenAccountDetailsAsync($"{beneficiary.PersonName} {beneficiary.PersonSurName}", beneficiary.AccountNumber, beneficiary.BankIcon, beneficiary.BankName);
                break;
        }

    }
    private async void OnRecipientSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Recipients selected)
            return;
        TransferRecipients.SelectedItem = null;

        await OpenAccountDetailsAsync(selected.PersonName, selected.AccountNumber, selected.BankIcon, selected.BankName);
    }
    private async Task OpenAccountDetailsAsync(string accountName, string accountNumber, string bankIcon, string bankName)
    {
        _selectedAccountName = accountName;
        _selectedAccountNumber = accountNumber;
        _selectedBankIcon = bankIcon;
        _selectedBankName = bankName;

        DetailsAccountNameLabel.Text = accountName;
        DetailsAccountNumberLabel.Text = accountNumber;
        DetailsBankIcon.Source = bankIcon;

        AccountDetailsOverlay.Opacity = 0;
        AccountDetailsCard.Scale = 0.8;
        AccountDetailsOverlay.IsVisible = true;

        await Task.WhenAll(
            AccountDetailsOverlay.FadeTo(1, 200),
            AccountDetailsCard.ScaleTo(1, 250, Easing.SpringOut)
        );
    }
    private async Task HideAccountDetailsAsync()
    {
        await Task.WhenAll(
            AccountDetailsOverlay.FadeTo(0, 180),
            AccountDetailsCard.ScaleTo(0.8, 180)
            );
        AccountDetailsOverlay.IsVisible = false;
        AccountDetailsOverlay.Opacity = 1;
        AccountDetailsCard.Scale = 1;
    }
    private void ClearSelection()
    {
        _selectedAccountName = string.Empty;
        _selectedAccountNumber = string.Empty;
        _selectedBankIcon = string.Empty;
        _selectedBankName = string.Empty;
    }
    private async void OnAccountDetailsOverlayTapped(object sender, EventArgs e)
    {
        ClearSelection();
        await HideAccountDetailsAsync();
    }
    //stops taps on the card from closing the popup
    private void OnAccountDetailsCardTapped(object sender, EventArgs e)
    {
    }
    private async void OnChangeTapped(object sender, TappedEventArgs e)
    {
        ClearSelection();
        await HideAccountDetailsAsync();

        //show list so user can pick another beneficiary
        if (!PopUpOverlay.IsVisible)
            await ShowPopUpAsync();
    }
    private async void OnClearTapped(object sender, TappedEventArgs e)
    {
        ClearSelection();
        await HideAccountDetailsAsync();
    }
    private async void OnApplyTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading||string.IsNullOrEmpty(_selectedAccountNumber))
            return;
        //copy details before navigating
        string accountName = _selectedAccountName;
        string accountNumber = _selectedAccountNumber;
        string bankIcon = _selectedBankIcon;
        string bankName = _selectedBankName;
        ClearSelection();
        
        await Loading.NavigateAsync(Navigation, () => new TransferSummary(accountName, accountNumber, bankIcon, bankName));

        //close popup so thatt it is still not open when user returns
        await HideAccountDetailsAsync();
    }
    private async void OnProceedBorderTapped(object sender, TappedEventArgs e)
    { if (Loading.IsLoading)
            return;
        string accountNumber = BeneficiaryAccountEntry.Text?.Trim() ?? "";
        string bankName = SelectBankEntry.Text?.Trim() ?? "";

        // Find the logo that belongs to the chosen bank
        string bankIcon = allRecipients
            .FirstOrDefault(r => r.BankName == bankName)?.BankIcon ?? "";
        try
        {
            ProceedTxt.IsVisible = false;
            ProceedIndicator.IsVisible = true;
            ProceedIndicator.IsRunning = true;

            await Loading.ShowAsync("Verifying account...");
            await Task.Delay(500);

            // No real name lookup in this practice app, so use a placeholder
            string accountName = "New Beneficiary";

            await Navigation.PushAsync(new TransferSummary(accountName, accountNumber, bankIcon, bankName));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Failed to continue with transfer.", "OK");
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


