
namespace PreciousPractice;

public partial class Transactions : ContentPage
{
    public class TransactionItem
    {
        public string BankIcon { get; set; }
        public string Preposition { get; set; }   // "from " or "to "
        public string PersonName { get; set; }
        public string DateTimeText { get; set; }
        public string AmountText { get; set; }
        public Color AmountTextColor { get; set; }
        public string StatusIcon { get; set; }
        public string StatusText { get; set; }
        public Color StatusColor { get; set; }
     
    }
    public Transactions() 
	{
		InitializeComponent();

        LoadTransactions();

        Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
    }
    private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        LoadTransactions();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
        Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
    }
    private void LoadTransactions()
    {
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color creditAmountColor = isDark ? Colors.White : Color.FromArgb("#141414");
        Color debitAmountColor = Color.FromArgb("#FF0000");
        
        var transactions = new List<TransactionItem>
        {
            new TransactionItem
            {
                BankIcon = "first_bank.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#141414")
            },
            new TransactionItem
            {
                BankIcon = "wema_icon.png",
                Preposition = "to",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "-N5,000",
                AmountTextColor = debitAmountColor,
                StatusIcon = "red_rectangle.png",
                StatusText = "Debit",
                StatusColor = Color.FromRgba("#FF0000")
            },
            new TransactionItem
            {
                BankIcon = "gtco_icon.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#0D643A")
            },
            new TransactionItem
            {
                BankIcon = "first_bank.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#0D643A")
            },
            new TransactionItem
            {
                BankIcon = "wema_icon.png",
                Preposition = "to",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "-N5,000",
                AmountTextColor = debitAmountColor,
                StatusIcon = "red_rectangle.png",
                StatusText = "Debit",
                StatusColor = Color.FromRgba("#FF0000")
            },
            new TransactionItem
            {
                BankIcon = "gtco_icon.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#0D643A")
            },
            new TransactionItem
            {
                BankIcon = "first_bank.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#0D643A")
            },
            new TransactionItem
            {
                BankIcon = "wema_icon.png",
                Preposition = "to",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "-N5,000",
                AmountTextColor = debitAmountColor,
                StatusIcon = "red_rectangle.png",
                StatusText = "Debit",
                StatusColor = Color.FromRgba("#FF0000")
            },
            new TransactionItem
            {
                BankIcon = "gtco_icon.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#0D643A")
            },
            new TransactionItem
            {
                BankIcon = "first_bank.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#0D643A")
            },
            new TransactionItem
            {
                BankIcon = "wema_icon.png",
                Preposition = "to",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "-N5,000",
                AmountTextColor = debitAmountColor,
                StatusIcon = "red_rectangle.png",
                StatusText = "Debit",
                StatusColor = Color.FromRgba("#FF0000")
            },
            new TransactionItem
            {
                BankIcon = "gtco_icon.png",
                Preposition = "to",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "-N5,000",
                AmountTextColor = debitAmountColor,
                StatusIcon = "red_rectangle.png",
                StatusText = "Debit",
                StatusColor = Color.FromRgba("#FF0000")
            },
            new TransactionItem
            {
                BankIcon = "first_bank.png",
                Preposition = "from",
                PersonName = "John Doe",
                DateTimeText ="Transfer - Feb 13th 3:38:29PM",
                AmountText = "+N10,000",
                AmountTextColor = creditAmountColor,
                StatusIcon = "green_img.png",
                StatusText = "Credit",
                StatusColor = Color.FromRgba("#0D643A")
            },

        };
        BindableLayout.SetItemsSource(TransactionsStack, transactions);

        System.Diagnostics.Debug.WriteLine($"Loaded {transactions.Count} transactions");
    }
	private async void OnBackIconTapped(object sender, EventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void NavToTransactionFilter(object sender,EventArgs e)
	{
		await Loading.NavigateAsync(Navigation, () => new TransactionFilter());
	}
}