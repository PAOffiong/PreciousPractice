//using Bumptech.Glide.Signature;


using PreciousPractice.Helpers;
using System.Runtime.CompilerServices;

namespace PreciousPractice;

public partial class HomePage : ContentPage
{
    private IDispatcherTimer? carouselTimer;

    private bool isBalanceVisible = true;

    public List<string> Updates { get; set; } =
        new List<string>
        {
            "ours_product.png",
            "ours_airtime_discount.png"
        };

    public HomePage()
    {
        InitializeComponent();

        BindingContext = this;

        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        ResetFinancialServiceBorders();
        ResetPaymentsAndTransfersDefaults();


        StartCarousel();

        Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
    }
    private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
    
        bool isDark = e.RequestedTheme == AppTheme.Dark;
        ResetFinancialServiceBorders();
         ResetPaymentsAndTransfersDefaults();
        UpdateEyeIcon();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Application.Current.RequestedThemeChanged-= OnRequestedThemeChanged;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        string accountNumber = DefaultAccountNumber.TemporaryAccountNumber;

        AccountNumberLabel.Text = $"Janet Doe - {accountNumber}";
        SavingsBankAccountNumberTxt.Text = accountNumber;
        CurrentBankAccountNumberTxt.Text = accountNumber;

        SavingsBankAmountTxt.Text = DefaultAccountBalance.Format(DefaultAccountBalance.Savings);
        CurrentBankAmountTxt.Text = DefaultAccountBalance.Format(DefaultAccountBalance.Current);

        AccountTypeLabel.Text = DefaultAccountBalance.SelectedAccount;
        UpdateBalanceDisplay();
    }
    // The ONE place that decides what the balance label shows
    private void UpdateBalanceDisplay()
    {
        BalanceLabel.Text = isBalanceVisible
            ? DefaultAccountBalance.Format(DefaultAccountBalance.SelectedBalance)
            : "******";

        UpdateEyeIcon();
    }
    private async void OnCopyAccountNumberTapped(object sender, TappedEventArgs e)
    {
        string accountNumber = Preferences.Get("AccountNumber", DefaultAccountNumber.TemporaryAccountNumber);

        if (!string.IsNullOrEmpty(accountNumber))
        {
            await Clipboard.Default.SetTextAsync(accountNumber);

            await DisplayAlert("Copied","Account number copied to clipboard.","OK");
        }
    }
    private void ResetPaymentsAndTransfersDefaults()
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color defaultColor = isDark ? Colors.Black : Color.FromArgb("#FFFFFF");
        string sendMoneyDefaultIcon = isDark ? "send_money_black.png" : "send_money_icon.png";
        string addMoneyDefaultIcon = isDark ? "black_add_money_icon.png" : "add_money_icon.png";

        SendMoneyBorder.BackgroundColor = Colors.Transparent;
        SendMoneyTxt.TextColor = defaultColor;
        SendMoneyBorder.Stroke = defaultColor;
        SendMoneyImage.Source = sendMoneyDefaultIcon;

        AddMoneyBorder.BackgroundColor = Colors.Transparent;
        AddMoneyTxt.TextColor = defaultColor;
        AddMoneyBorder.Stroke = defaultColor;
        AddMoneyImage.Source = addMoneyDefaultIcon;
    }

    private async void OnPaymentsandTransfersTapped(object sender, TappedEventArgs e)
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;

        ResetPaymentsAndTransfersDefaults();

        if (sender == SendMoneyBorder)
        {
            SendMoneyBorder.Stroke = Color.FromArgb("#336699");
            SendMoneyImage.Source = isDark ? "dark_mode_send_money.png" : "blue_send_money.png";
            SendMoneyTxt.TextColor = Color.FromArgb("#336699");

            await Navigation.PushAsync(new TransferPage());
        }
        else if (sender == AddMoneyBorder)
        {
            AddMoneyBorder.Stroke = Color.FromArgb("#009933");
            AddMoneyImage.Source = isDark ? "black_add_money" : "green_add_money";
            AddMoneyTxt.TextColor = Color.FromArgb("#009933");

            await Navigation.PushAsync(new AddMoney());
        }
    }

    private void OnFinancialServiceTapped(object sender, TappedEventArgs e)
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color selectedTextColor = isDark ? Colors.Black : Color.FromArgb("#FFFFFF");

        ResetFinancialServiceBorders();

        if (sender == SavingsBorder)
        {
            SavingsBorder.BackgroundColor = Color.FromArgb("#009933");
            SavingsTxt.TextColor = selectedTextColor;
            SavingsBorder.Stroke = Colors.Transparent;
            SavingsIcon.Source = "savings_white.png";
            SavingsIcon.IsVisible = !isDark;
        }
        else if (sender == LoanBorder)
        {
            LoanBorder.BackgroundColor = Color.FromArgb("#009933");
            LoanTxt.TextColor = selectedTextColor;
            LoanBorder.Stroke = Colors.Transparent;
            LoanIcon.Source = "loan_white.png";
            LoanIcon.IsVisible = !isDark;
        }
        else if (sender == InvestmentBorder)
        {
            InvestmentBorder.BackgroundColor = Color.FromArgb("#009933");
            InvestmentTxt.TextColor = selectedTextColor;
            InvestmentBorder.Stroke = Colors.Transparent;
            InvestmentIcon.Source = "investment_white.png";
            InvestmentIcon.IsVisible = !isDark;
        }
    }


    private void ResetFinancialServiceBorders()
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color defaultTextColor = isDark ? Color.FromArgb("#336699") : Color.FromArgb("#333333");

        SavingsBorder.BackgroundColor = Colors.Transparent;
        SavingsTxt.TextColor = defaultTextColor;
        SavingsBorder.Stroke = Color.FromArgb("#336699");
        SavingsIcon.Source = "savings_icon.png";
        SavingsIcon.IsVisible = !isDark;

        LoanBorder.BackgroundColor = Colors.Transparent;
        LoanTxt.TextColor = defaultTextColor;
        LoanBorder.Stroke = Color.FromArgb("#336699");
        LoanIcon.Source = "loan_icon.png";
        LoanIcon.IsVisible = !isDark;

        InvestmentBorder.BackgroundColor = Colors.Transparent;
        InvestmentTxt.TextColor = defaultTextColor;
        InvestmentBorder.Stroke = Color.FromArgb("#336699");
        InvestmentIcon.Source = "investment_icon.png";
        InvestmentIcon.IsVisible = !isDark;
    }

    private void OnEyeImageTapped(object sender,TappedEventArgs e)
    {
        isBalanceVisible = !isBalanceVisible;
        UpdateBalanceDisplay();
    }
    private void UpdateEyeIcon()
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;

        if (isBalanceVisible)
            EyeImage.Source = isDark ? "black_eye_icon.png" : "eye_img.png";
        else
            EyeImage.Source = isDark ? "dark_closed_eye.png" : "closed_eye.png";
    }
    private async Task ShowPopUpAsync()
    {
        PopUpOverlay.Opacity = 0;
        SavingsPopUp.Scale = 0.8;
        PopUpOverlay.IsVisible = true;
        SavingsPopUp.IsVisible = true;

        await Task.WhenAll(
            PopUpOverlay.FadeTo(1, 200),
            SavingsPopUp.ScaleTo(1, 250, Easing.SpringOut)
        );
    }
    private async Task HidePopUpAsync()
    {
        await Task.WhenAll(
            PopUpOverlay.FadeTo(0, 180),
            SavingsPopUp.ScaleTo(0.8, 180)
        );

        PopUpOverlay.IsVisible = false;
        SavingsPopUp.IsVisible = false;
        PopUpOverlay.Opacity = 1;
        SavingsPopUp.Scale = 1;
    }
    private async void OnSavingsVectorTapped(object sender, TappedEventArgs e)
    {
        if (PopUpOverlay.IsVisible)
            await HidePopUpAsync();

        else
            await ShowPopUpAsync();
    }
    private async void OnAccountTypeBorderTapped(object sender, TappedEventArgs e)
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color defaultTextColor = isDark ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#141414");
        Color selectedTextColor = isDark ? Color.FromArgb("#141414") : Color.FromArgb("#FFFFFF");

        SavingsBankBorder.BackgroundColor = Colors.Transparent;
        SavingsBankTxt.TextColor = defaultTextColor;
        SavingsBankAccountNumberTxt.TextColor = defaultTextColor;
        SavingsBankAmountTxt.TextColor = defaultTextColor;
        SavingsBankBorder.Stroke = Color.FromArgb("#336699");
        SavingsBankImage.Source = isDark? "blue_black_img.png":"blue_white_naira.png";

        CurrentBankBorder.BackgroundColor = Colors.Transparent;
        CurrentBankTxt.TextColor = defaultTextColor;
        CurrentBankAccountNumberTxt.TextColor = defaultTextColor;
        CurrentBankAmountTxt.TextColor = defaultTextColor;
        CurrentBankBorder.Stroke = Color.FromArgb("#336699");
        CurrentBankImage.Source = isDark ? "blue_black_img.png" : "blue_white_naira.png";

        string selectedType = "";

        if (sender == SavingsBankBorder)
        {
            SavingsBankBorder.BackgroundColor = Color.FromArgb("#009933");
            SavingsBankTxt.TextColor = selectedTextColor;
            SavingsBankAccountNumberTxt.TextColor = selectedTextColor;
            SavingsBankAmountTxt.TextColor = selectedTextColor;
            SavingsBankBorder.Stroke = Colors.Transparent;
            SavingsBankImage.Source = isDark? "black_green_img":"white_green_naira.png";

            selectedType = "Savings";
        }
        else if (sender == CurrentBankBorder)
        {
            CurrentBankBorder.BackgroundColor = Color.FromArgb("#009933");
            CurrentBankTxt.TextColor = selectedTextColor;
            CurrentBankAccountNumberTxt.TextColor = selectedTextColor;
            CurrentBankAmountTxt.TextColor = selectedTextColor;
            CurrentBankBorder.Stroke = Colors.Transparent;
            CurrentBankImage.Source = isDark ? "black_green_img" : "white_green_naira.png";

            selectedType = "Current";
        }
        DefaultAccountBalance.SelectedAccount = selectedType;
        AccountTypeLabel.Text = selectedType;
        UpdateBalanceDisplay();

        await HidePopUpAsync();
    }
    private async void  OnPopUpOverlayTapped(object sender, TappedEventArgs e)
    {
        await HidePopUpAsync();
    }
    private void OnSavingsPopUpTapped(object sender, TappedEventArgs e)
    {

    }
    
    private async void NavToAddNewAccount(object sender, EventArgs e)
    {
        await Loading.NavigateAsync(Navigation, () => new AddNewAccount());
    }
    private  async void OnQuickActionTapped(object sender, TappedEventArgs e)
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color selectedTextColor = isDark ? Colors.Black : Color.FromArgb("#FFFFFF");
       

        SpendBorder.BackgroundColor = Colors.Transparent;
            SpendTxt.TextColor = Color.FromArgb("#336699");
            SpendBorder.Stroke = Color.FromArgb("#336699");
            SpendIcon.Source = "spend_money_icon.png";
            
            AirtimeBorder.BackgroundColor = Colors.Transparent;
            AirtimeTxt.TextColor = Color.FromArgb("#336699");
            AirtimeBorder.Stroke = Color.FromArgb("#336699");
            AirtimeIcon.Source = "signal_cellular_alt.png";
            
            DataBorder.BackgroundColor = Colors.Transparent;
            DataTxt.TextColor = Color.FromArgb("#336699");
            DataBorder.Stroke = Color.FromArgb("#336699");
            DataIcon.Source = "wifi.png";
            
            UtilitiesBorder.BackgroundColor = Colors.Transparent;
            UtilitiesTxt.TextColor = Color.FromArgb("#336699");
            UtilitiesBorder.Stroke = Color.FromArgb("#336699"); 
            UtilitiesIcon.Source = "utilities_icon.png";
            
        

        if (sender ==SpendBorder)
        {
            SpendBorder.BackgroundColor = Color.FromArgb("#009933");
            SpendTxt.TextColor = selectedTextColor;
            SpendBorder.Stroke = Colors.Transparent;    // ← only the tapped one loses its outline
            SpendIcon.Source = isDark? "black_spend_money.png": "spend_white.png";
        }
        else if (sender == AirtimeBorder)
        {
            AirtimeBorder.BackgroundColor = Color.FromArgb("#009933");
            AirtimeTxt.TextColor = selectedTextColor;
            AirtimeBorder.Stroke = Colors.Transparent;    // ← only the tapped one loses its outline
            AirtimeIcon.Source = isDark? "black_signal_cellular_alt.png":"signal_cellular_white.png";

            await Loading.NavigateAsync(Navigation, () => new AirtimeHomePage());
        }
        else if (sender == DataBorder)
        {
            DataBorder.BackgroundColor = Color.FromArgb("#009933");
            DataTxt.TextColor = selectedTextColor;
            DataBorder.Stroke = Colors.Transparent;    // ← only the tapped one loses its outline
            DataIcon.Source = isDark? "black_wifi.png" :"wifi_white.png";

            await Loading.NavigateAsync(Navigation, () => new DataHomePage());
        }
        else if (sender ==UtilitiesBorder)
        {
            UtilitiesBorder.BackgroundColor = Color.FromArgb("#009933");
            UtilitiesTxt.TextColor = selectedTextColor;
            UtilitiesBorder.Stroke = Colors.Transparent;    // ← only the tapped one loses its outline
            UtilitiesIcon.Source = isDark? "black_utilities.png":"utilities_white.png";

            await Loading.NavigateAsync(Navigation, () => new PaymentsPage());
        }
    }
    private async void NavToTransactions(object sender, EventArgs e)
    {if (Loading.IsLoading)
            return;
        try
        {
            TransactionsTxt.IsVisible = false;

            TransactionsIndicator.IsVisible = true;
            TransactionsIndicator.IsRunning = true;

            await Task.Delay(500);

            await Loading.NavigateAsync(Navigation, () => new Transactions());
        }
        catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);

            await DisplayAlert("Error", "Failed to Open Transaction History", "OK");
        }
        finally
        {
            TransactionsTxt.IsVisible = true;

            TransactionsIndicator.IsRunning = false;
            TransactionsIndicator.IsVisible = false;
        }
    }
    private void StartCarousel()
    {
        carouselTimer = Dispatcher.CreateTimer();
   
        carouselTimer.Interval = TimeSpan.FromSeconds(3);

        carouselTimer.Tick += (sender, e) =>
        {
            if (UpdatesCarousel.ItemsSource is IList<string> images &&
                images.Count > 0)
            {
                int nextPosition = UpdatesCarousel.Position + 1;

                if (nextPosition >= images.Count)
                    nextPosition = 0;

                UpdatesCarousel.Position = nextPosition;
            }
        };

        carouselTimer.Start();
    }
}