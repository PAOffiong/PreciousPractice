using PreciousPractice.Helpers;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PreciousPractice;

public partial class TransferToPyrowo : ContentPage
{
    public class RecentTransfers
    {
        public string BankIcon { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string AccountNumber { get; set; } 

    }
    public class Beneficiaries
    {
        public string BankIcon { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string AccountNumber { get; set; } 
    }
    public class Recipients
    {
        public string BankIcon { get; set; }
        public string PersonName { get; set; }
        public string AccountNumber { get; set; }

    }
    private List<Recipients> allRecipients = new();
    private ObservableCollection<Recipients> filteredRecipients = new();
    //account that shows up in the popup
    private string _selectedAccountName = string.Empty;
    private string _selectedAccountNumber = string.Empty;
   

    public TransferToPyrowo()
	{
		InitializeComponent();
        LoadRecentTransfers();
        LoadBeneficiaries();
        LoadRecipients();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        AccountNumberLabel.Text =  DefaultAccountNumber.TemporaryAccountNumber;
        AvailableBalance.Text = AvailableBalance.Text = DefaultAccountBalance.Format(DefaultAccountBalance.SelectedBalance);

    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void OnViewTapped(object sender, TappedEventArgs e)
    {
        if (PopUpOverlay.IsVisible)
            await HidePopUpAsync();
        else
            await ShowPopUpAsync();
    }
    private void LoadRecentTransfers()
    {
        var recentTransfers = new List<RecentTransfers>
        {
            new RecentTransfers{BankIcon = "flower_green.png", PersonName = "Sarah", PersonSurName = "Micheal",AccountNumber = "8321456789"},
            new RecentTransfers{BankIcon = "flower_green.png",PersonName = "John",PersonSurName = "Kehinde",AccountNumber = "8543217892"},
            new RecentTransfers{BankIcon = "flower_green.png",PersonName = "Janet Doe",PersonSurName = "Emmanuel",AccountNumber = "8812345094"},
            new RecentTransfers{BankIcon = "flower_green.png",PersonName = "Patience",PersonSurName = "Chijioke",AccountNumber = "8332255690"},
            new RecentTransfers{BankIcon = "flower_green.png",PersonName = "Sarah",PersonSurName = "Eze",AccountNumber = "8321698655"},
            new RecentTransfers{BankIcon = "flower_green.png",PersonName = "Tom Doe",PersonSurName = "Kehinde",AccountNumber = "8658321098"},
        };
        BindableLayout.SetItemsSource(RecentTransactions, recentTransfers);
    }
    private void LoadBeneficiaries()
    {
        var beneficiaries = new List<Beneficiaries>
        {
            new Beneficiaries{ BankIcon = "flower_green.png",PersonName = "Sarah",PersonSurName = "Micheal",AccountNumber = "8213465059"},
            new Beneficiaries{BankIcon = "flower_green.png",PersonName = "John Doe",PersonSurName = "Kehinde",AccountNumber = "8135467883"},
            new Beneficiaries{BankIcon = "flower_green.png",PersonName = "Janet Doe",PersonSurName = "Emmanuel",AccountNumber = "8098765432"},
            new Beneficiaries{BankIcon = "flower_green.png",PersonName = "Patience",PersonSurName = "Chijioke",AccountNumber = "8987654321"},
            new Beneficiaries{BankIcon = "flower_green.png",PersonName = "Sarah",PersonSurName = "Eze",AccountNumber = "8789654312"},
            new Beneficiaries{BankIcon = "flower_green.png",PersonName = "Tom Doe",PersonSurName = "Kehinde",AccountNumber = "8654321789"},
        };
        BindableLayout.SetItemsSource(AllBeneficiaries, beneficiaries);
    }
    private void LoadRecipients()
    {
        allRecipients = new List<Recipients>
        {
            new Recipients{ BankIcon = "flower_green.png", PersonName = "Sarah Micheal",AccountNumber = "8213465059",},
            new Recipients{BankIcon = "flower_green.png",PersonName = "John Doe Kehinde",AccountNumber = "8135467883"},
            new Recipients{BankIcon = "flower_green.png", PersonName = "Janet Doe Emmanuel",AccountNumber = "8098765432"},
            new Recipients{BankIcon = "flower_green.png",PersonName = "Patience Chijioke",AccountNumber = "8987654321"},
            new Recipients{BankIcon = "flower_green.png",PersonName = "Sarah Eze",AccountNumber = "8789654312"},
            new Recipients{BankIcon = "flower_green.png",PersonName = "Tom Doe Kehinde",AccountNumber = "8654321789"},
            new Recipients{BankIcon = "flower_green.png",PersonName = "Sarah Okon",AccountNumber = "8658321098"},
            new Recipients{BankIcon = "flower_green.png",PersonName = "Happiness Kehinde",AccountNumber = "8321698655"},
            new Recipients{BankIcon = "flower_green.png",PersonName = "John Doe Kehinde",AccountNumber = "8332255690"},
            new Recipients{BankIcon = "flower_green.png", PersonName = "Sarah Adichie",AccountNumber = "8812345094"},
            new Recipients{BankIcon = "flower_green.png",PersonName = "Sarah Chijioke",AccountNumber = "8543217892"},
            new Recipients{BankIcon = "flower_green.png", PersonName = "John Doe Kehinde",AccountNumber = "8321456789"},
        };
        filteredRecipients = new ObservableCollection<Recipients>(allRecipients);
        TransferRecipients.ItemsSource = filteredRecipients;
    }
    private async Task ShowPopUpAsync()
    {
        PopUpOverlay.Opacity = 0;
        TransferActivityPopUp.Scale = 0.8;
        PopUpOverlay.IsVisible = true;
        TransferActivityPopUp.IsVisible = true;

        await Task.WhenAll(
            PopUpOverlay.FadeTo(1, 200),
            TransferActivityPopUp.ScaleTo(1, 250, Easing.SpringOut)
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
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? "";

        filteredRecipients.Clear();

        foreach (var recipient in allRecipients)
        {
            if (string.IsNullOrEmpty(searchText) ||
                recipient.PersonName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                recipient.AccountNumber.Contains(searchText))
            {
                filteredRecipients.Add(recipient);
            }
        }
    }
    private async void OnViewAllTapped(object sender, TappedEventArgs e)
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
    private async void OnTransferActivityPopUpTapped(object sender, TappedEventArgs e)
    {

    }
    private void OnClearSearchEntryTapped(object sender, TappedEventArgs e)
    {
        RecipientSearchEntry.Text = string.Empty;
    }
    //clear all and apply popup

    //on tapping any account under recent and beneficiaries
    private async void OnBindingTapped(object sender, TappedEventArgs e)
    {
        if (sender is not BindableObject tappedItem)
            return;

        switch(tappedItem.BindingContext)
        {
            case RecentTransfers recent:
                await OpenAccountDetailsAsync( $"{recent.PersonName} {recent.PersonSurName}", recent.AccountNumber);
             break;

            case Beneficiaries beneficiary:
                await OpenAccountDetailsAsync( $"{beneficiary.PersonName} {beneficiary.PersonSurName}", beneficiary.AccountNumber);
             break;
        }
    }
    private async void OnRecipientSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Recipients selected)
            return;
        TransferRecipients.SelectedItem = null;

        await OpenAccountDetailsAsync(selected.PersonName, selected.AccountNumber);
    }
    private async Task OpenAccountDetailsAsync(string accountName, string accountNumber)
    {
        _selectedAccountName = accountName;
        _selectedAccountNumber = accountNumber;

        DetailsAccountNameLabel.Text = accountName;
        DetailsAccountNumberLabel.Text = accountNumber;

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
        AccountDetailsOverlay.IsVisible=false;
        AccountDetailsOverlay.Opacity = 1;
        AccountDetailsCard.Scale = 1;
    }
    private void ClearSelection()
    {
        _selectedAccountName = string.Empty;
        _selectedAccountNumber = string.Empty;
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
        if(!PopUpOverlay.IsVisible)
            await ShowPopUpAsync();
    }
    private async void OnClearTapped(object sender, TappedEventArgs e)
    {
        ClearSelection();
        await HideAccountDetailsAsync();

    }
    private async void OnApplyTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading || string.IsNullOrEmpty(_selectedAccountNumber))  
            return;

        // copy details before navigating
        string accountName = _selectedAccountName;
        string accountNumber = _selectedAccountNumber;

        await Loading.NavigateAsync(Navigation,                                   
            () => new TransferSummary(accountName, accountNumber, "flower_green.png"));

        AccountDetailsOverlay.IsVisible = false;
        PopUpOverlay.IsVisible = false;
        TransferActivityPopUp.IsVisible = false;
    }

}