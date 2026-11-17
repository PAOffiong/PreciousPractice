
namespace PreciousPractice;

public partial class AddNewAccount : ContentPage
{
	public AddNewAccount()
	{
		InitializeComponent();
	}
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	
	private async Task ShowPopupAsync()
	{
		PopupOverlay.Opacity = 0;
		AccountPopupBorder.Scale = 0.8;
		PopupOverlay.IsVisible = true;
		AccountPopupBorder.IsVisible = true;

		await Task.WhenAll(
			PopupOverlay.FadeTo(1, 200),
			AccountPopupBorder.ScaleTo(1, 180)
			);

	}
	private async Task HidePopupAsync()
	{
		await Task.WhenAll(
			PopupOverlay.FadeTo(0, 200),
			AccountPopupBorder.ScaleTo(0.8, 180)
			);

		PopupOverlay.Opacity = 1;
		AccountPopupBorder.Scale = 1;
		PopupOverlay.IsVisible= false;
		AccountPopupBorder.IsVisible = false;
	}
    private async void OnDownwardVectorTapped(object sender, TappedEventArgs e)
    {
        if (PopupOverlay.IsVisible)
            await HidePopupAsync();
        else
            await ShowPopupAsync();
    }
	private async void OnAccountBorderTapped(object sender, TappedEventArgs e)
	{
		SavingsAccountBorder.BackgroundColor = Colors.Transparent;
		SavingsAccountTxt.TextColor = Color.FromArgb("#336699");
		SavingsAccountBorder.Stroke = Color.FromArgb("#336699");
		
		CurrentAccountBorder.BackgroundColor = Colors.Transparent;
		CurrentAccountTxt.TextColor = Color.FromArgb("#336699");
		CurrentAccountBorder.Stroke = Color.FromArgb("#336699");

		string selectedType = "";

		if(sender == SavingsAccountBorder)
		{
            SavingsAccountBorder.BackgroundColor = Color.FromArgb("#009933");
            SavingsAccountTxt.TextColor = Color.FromArgb("#FFFFFF");
			SavingsAccountBorder.Stroke = Colors.Transparent;

			selectedType = "Savings";
        }
		else if(sender == CurrentAccountBorder)
		{
            CurrentAccountBorder.BackgroundColor = Color.FromArgb("#009933");
            CurrentAccountTxt.TextColor = Color.FromArgb("#FFFFFF");
            CurrentAccountBorder.Stroke = Colors.Transparent;

			selectedType = "Current";
        }

		AccountTypeEntry.Text = selectedType;
        

    }
	private async void OnPopupOverlayTapped(object sender, TappedEventArgs e)
	{
		await HidePopupAsync();
	}
	private void OnAccountPopupTapped(object sender, TappedEventArgs e)
	{

	}
	
	private void OnEntriesTextChanged(object sender, TextChangedEventArgs e)
	{
        bool selectedAccount = !String.IsNullOrWhiteSpace(AccountTypeEntry.Text);
        bool accountSuffix = !String.IsNullOrWhiteSpace(AccountSuffixEntry.Text);

		ContinueBorder.IsEnabled = selectedAccount && accountSuffix;

    }

}