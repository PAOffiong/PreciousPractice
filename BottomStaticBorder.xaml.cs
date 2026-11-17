namespace PreciousPractice;

public partial class BottomStaticBorder : ContentView
{
	public static readonly BindableProperty ActiveTabProperty =
		BindableProperty.Create(
			nameof(ActiveTab),
			typeof(string),
			typeof(BottomStaticBorder),
			defaultValue: null,
			propertyChanged: OnActiveTabChanged);
	public string ActiveTab
	{
		get => (string)GetValue(ActiveTabProperty);
		set => SetValue(ActiveTabProperty, value);
	}
	public BottomStaticBorder()
	{
		InitializeComponent();
        ResetAllToDefault();

        Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        Unloaded += (s, e) => Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
    }
    private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        ResetAllToDefault();
    }
    private static void OnActiveTabChanged(BindableObject bindable,object oldvalue,object newValue)
	{
		if (bindable is BottomStaticBorder control)
		{
			control.HighlightActiveTab(newValue as string);
		}
	}
	private  void HighlightActiveTab(string tab)
	{

		bool isDark =Application.Current.RequestedTheme ==AppTheme.Dark;
		Color selectedTextColor = Color.FromArgb("#336699");
		
		ResetAllToDefault();

		switch(tab)		
		{
		 case "Transfer":
			TransferTxt.TextColor = selectedTextColor;
			TransferImage.Source = "blue_transfer_arrow.png";
				break;

			case "Payments":
			    PaymentsTxt.TextColor = selectedTextColor;
				PaymentsImage.Source = "payments_blue.png";
				break;

		
		 case "Profile":
			    ProfileTxt.TextColor = selectedTextColor;
				ProfileImage.Source = "blue_profile.png";
				break;

		 case "More":
			MoreTxt.TextColor = selectedTextColor;
			MoreImage.Source = "blue_more.png";
				break;

		 case "Home":
		    HomeTxt.TextColor = selectedTextColor;
			HomeImage.Source = isDark? "white_home.png":"home.png";
				break;
		}

	}
	private void ResetAllToDefault()
	{
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color defaultTextColor = isDark? Colors.White :Color.FromArgb("#141414");

        TransferTxt.TextColor = defaultTextColor;
        TransferImage.Source = isDark ? "white_transfer_arrows.png" : "transfer_arrows.png";
		 
        PaymentsTxt.TextColor = defaultTextColor;
        PaymentsImage.Source = isDark ? "white_payments.png" : "payments.png";

        ProfileTxt.TextColor = defaultTextColor;
        ProfileImage.Source = isDark ? "white_profile.png" : "profile.png";

        MoreTxt.TextColor = defaultTextColor;
        MoreImage.Source = isDark ? "white_more_icon.png" : "more.png";

        HomeTxt.TextColor = defaultTextColor;
        HomeImage.Source = isDark ? "white_home.png" : "home.png";
    }
	private async void OnBottomNavigationTapped(object sender, TappedEventArgs e)
	{
		string tapped = e.Parameter as string;

		if (tapped == ActiveTab)
			return;
		switch (tapped)
		{
			case "Transfer":
				await Navigation.PushAsync(new TransferPage());
				break;

			case "Payments":
				await Navigation.PushAsync(new PaymentsPage());
				break;

			case "Profile":
				await Navigation.PushAsync(new ProfilePage());
				break;

			case "More":
				await Navigation.PushAsync(new MorePage());
				break;

			case "Home":
				await Navigation.PushAsync(new HomePage());
				break;
		}
	}

}

