namespace PreciousPractice;

public partial class TransactionFilter : ContentPage
{
	public TransactionFilter()
	{
		InitializeComponent();

		StartDatePicker.Date = DateTime.Today;
		EndDatePicker.Date = DateTime.Today;
		ShowDate(DateTime.Today, StartDateTitleLabel, StartDateValueLabel);
		ShowDate(DateTime.Today, EndDateTitleLabel, EndDateValueLabel);

		OnTransactionDateRangeTapped(CustomPeriodBorder, null);
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private void OnTransactionDateRangeTapped(object sender, TappedEventArgs e)
	{
        bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        Color selectedTextColor = isDark ? Colors.Black : Color.FromArgb("#FFFFFF");


        CustomPeriodBorder.BackgroundColor = Colors.Transparent;
		CustomPeriodTxt.TextColor = Color.FromArgb("#336699");
		CustomPeriodBorder.Stroke = Color.FromArgb("#336699");

		CurrentWeekBorder.BackgroundColor = Colors.Transparent;
		CurrentWeekTxt.TextColor = Color.FromArgb("#336699");
		CurrentWeekBorder.Stroke = Color.FromArgb("#336699");

		LastWeekBorder.BackgroundColor = Colors.Transparent;
		LastWeekTxt.TextColor = Color.FromArgb("#336699");
		LastWeekBorder.Stroke = Color.FromArgb("#336699");

		CurrentMonthBorder.BackgroundColor = Colors.Transparent;
		CurrentMonthTxt.TextColor = Color.FromArgb("#336699");
		CurrentMonthBorder.Stroke = Color.FromArgb("#336699");
		
		CurrentYearBorder.BackgroundColor = Colors.Transparent;
		CurrentYearTxt.TextColor = Color.FromArgb("#336699"); 
		CurrentYearBorder.Stroke = Color.FromArgb("#336699");


        if (sender == CustomPeriodBorder)
		{
			CustomPeriodBorder.BackgroundColor = Color.FromArgb("#009933");
			CustomPeriodTxt.TextColor = selectedTextColor;
			CustomPeriodBorder.Stroke = Colors.Transparent;

            StartDatePicker.Date = DateTime.Today;
            EndDatePicker.Date = DateTime.Today;

            ShowDate(DateTime.Today, StartDateTitleLabel, StartDateValueLabel);
            ShowDate(DateTime.Today, EndDateTitleLabel, EndDateValueLabel);
        }
		else if(sender == CurrentWeekBorder)
		{
			CurrentWeekBorder.BackgroundColor = Color.FromArgb("#009933");
			CurrentWeekTxt.TextColor = selectedTextColor;
            CurrentWeekBorder.Stroke = Colors.Transparent;

			DateTime startOfWeek = GetStartOfWeek(DateTime.Today, DayOfWeek.Sunday);
			DateTime endOfWeek = startOfWeek.AddDays(6);

			StartDatePicker.Date = startOfWeek;
			EndDatePicker.Date = endOfWeek;

			ShowDate(startOfWeek, StartDateTitleLabel, StartDateValueLabel);
			ShowDate(endOfWeek, EndDateTitleLabel, EndDateValueLabel);
		}
		else if(sender == LastWeekBorder)
		{
			LastWeekBorder.BackgroundColor = Color.FromArgb("#009933");
			LastWeekTxt.TextColor = selectedTextColor;
            LastWeekBorder.Stroke = Colors.Transparent;

			DateTime startOfLastWeek = GetStartOfLastWeek(DateTime.Today, DayOfWeek.Sunday).AddDays(-7);
			DateTime endOfLastWeek = startOfLastWeek.AddDays(6);

			StartDatePicker.Date = startOfLastWeek;
			EndDatePicker.Date = endOfLastWeek;

			ShowDate(startOfLastWeek, StartDateTitleLabel, StartDateValueLabel);
			ShowDate(endOfLastWeek, EndDateTitleLabel, EndDateValueLabel);
		}

		else if(sender == CurrentMonthBorder)
		{
			CurrentMonthBorder.BackgroundColor = Color.FromArgb("#009933");
			CurrentMonthTxt.TextColor = selectedTextColor;
            CurrentMonthBorder.Stroke = Colors.Transparent;

			DateTime startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            DateTime endOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, daysInMonth);

            StartDatePicker.Date = startOfMonth;
            EndDatePicker.Date = endOfMonth;

            ShowDate(startOfMonth, StartDateTitleLabel, StartDateValueLabel);
            ShowDate(endOfMonth, EndDateTitleLabel, EndDateValueLabel);
        }

		else if(sender == CurrentYearBorder)
		{
			CurrentYearBorder.BackgroundColor = Color.FromArgb("#009933");
			CurrentYearTxt.TextColor = selectedTextColor;
            CurrentYearBorder.Stroke = Colors.Transparent;

			DateTime startOfYear = GetStartOfYear(DateTime.Today);
			DateTime endOfYear = GetEndOfYear(DateTime.Today);

			StartDatePicker.Date = startOfYear;
			EndDatePicker.Date = endOfYear;

			ShowDate(startOfYear, StartDateTitleLabel, StartDateValueLabel);
			ShowDate(endOfYear, EndDateTitleLabel, EndDateValueLabel);
		}
	}
    private void OnStartDateSelected(object sender, DateChangedEventArgs e)
    {   
		if(e.NewDate.HasValue)
        ShowDate(e.NewDate.Value, StartDateTitleLabel, StartDateValueLabel);
    }
    private void OnEndDateSelected(object sender, DateChangedEventArgs e)
    {
		if(e.NewDate.HasValue)
        ShowDate(e.NewDate.Value, EndDateTitleLabel, EndDateValueLabel);
    }
    private void OnStartDateFocused(object sender, FocusEventArgs e)
	{
		ShowDate(StartDatePicker.Date ?? DateTime.Today,StartDateTitleLabel, StartDateValueLabel);
	}
	
	private void OnEndDateFocused(object sender, FocusEventArgs e)
	{
		ShowDate(EndDatePicker.Date ?? DateTime.Today, EndDateTitleLabel, EndDateValueLabel);
	}
	
	private async void ShowDate(DateTime date, Label titleLabel, Label valueLabel)
	{
        titleLabel.VerticalOptions = LayoutOptions.Start; 
		titleLabel.FontSize = 10; 
		
		titleLabel.VerticalOptions = LayoutOptions.Start; 
		valueLabel.FontSize = 14; 
		valueLabel.Text = date.Date.ToString("dd MMM yyyy"); 
		
		if (!valueLabel.IsVisible) 
		{ valueLabel.IsVisible = true; 
			await titleLabel.ScaleTo(1.05, 50, Easing.CubicOut); 
			await titleLabel.ScaleTo(1, 80, Easing.CubicOut); 
		}
    }
    private void OnStartDateUnFocused(object sender, FocusEventArgs e)
	{
		if (!StartDateValueLabel.IsVisible)
			ResetDateDisplay(StartDateTitleLabel, StartDateValueLabel);
	}
	private void OnEndDateUnFocused(object sender, FocusEventArgs e)
	{
		if (!EndDateValueLabel.IsVisible)
			ResetDateDisplay(EndDateTitleLabel, EndDateValueLabel);
	}
    private void ResetDateDisplay(Label titleLabel, Label ValueLabel)
    {
        ValueLabel.IsVisible = false;
        titleLabel.VerticalOptions = LayoutOptions.Center;
        titleLabel.FontSize = 14;
    }
	private DateTime GetStartOfWeek(DateTime date, DayOfWeek startOfWeek)
	{
		int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
		return date.AddDays(-diff).Date;
	}
	private DateTime GetStartOfLastWeek(DateTime date, DayOfWeek startOfLastWeek)
	{
		int diff = (7 + (date.DayOfWeek - startOfLastWeek)) % 7;
		return date.AddDays(-diff).Date;
	}
	private  DateTime GetStartOfMonth(DateTime date)
	{
		return new DateTime(date.Year, date.Month, 1);
	}
	private DateTime GetEndOfMonth(DateTime date)
	{
		int lastDay = DateTime.DaysInMonth(date.Year, date.Month);
		return new DateTime(date.Year, date.Month,lastDay);
	}
	private DateTime GetStartOfYear(DateTime date)
	{
		return new DateTime(date.Year, 1,1);
	}
	private DateTime GetEndOfYear(DateTime date)
	{ 
		return new DateTime(date.Year, 12, 31);
	}
    private void OnClearAllTapped(object sender, TappedEventArgs e)
    {
        // Back to how the page looks when it first opens:
        // "Custom Period" selected, today's date in both boxes
        OnTransactionDateRangeTapped(CustomPeriodBorder, null!);
    }
    private async void OnApplyTapped(object sender, TappedEventArgs e)
    {
		await Loading.NavigateAsync(Navigation, () => new ShareReceipt());
    }


}