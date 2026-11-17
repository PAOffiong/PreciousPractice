namespace PreciousPractice;

public partial class ThemePage : ContentPage
{
	private const string ThemeKey = "AppTheme"; // "light, dark or default
	private static readonly Color SelectedStroke = Color.FromArgb("#009933");
	public ThemePage()
	{
		InitializeComponent();

		SelectTheme(Preferences.Get(ThemeKey, "Default"));
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private void OnThemeBorderTapped(object sender, TappedEventArgs e)
	{
		if (e.Parameter is string theme)
			SelectTheme(theme);

	}
	private void OnButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		if (!e.Value)
			return; //ignore the button that is unticked

		if (sender == LightModeButton) SelectTheme("Light");
		else if (sender == DarkModeButton) SelectTheme("Dark");
		else if (sender == DefaultButton) SelectTheme("Default");
	}
	private void SelectTheme(string theme)
	{
		LightModeBorder.Stroke = theme == "Light"? SelectedStroke:Colors.Transparent;
		DarkModeBorder.Stroke = theme == "Dark"? SelectedStroke:Colors.Transparent;
		DefaultBorder.Stroke = theme == "Default"? SelectedStroke:Colors.Transparent;

		LightModeButton.IsChecked = theme == "Light";
		DarkModeButton.IsChecked = theme == "Dark";
		DefaultButton.IsChecked = theme == "Default";

		if (Application.Current is not null)
		{
			Application.Current.UserAppTheme = theme switch
			{
				"Light" => AppTheme.Light,
				"Dark" => AppTheme.Dark,
				_=> AppTheme.Unspecified //follow phone setting
			};
		}
		Preferences.Set(ThemeKey, theme);
	}
}