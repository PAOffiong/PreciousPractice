namespace PreciousPractice;

public partial class MembershipStatus : ContentPage
{
	public MembershipStatus()
	{
		InitializeComponent();
	}
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void OnBackToHomeTapped(object sender, TappedEventArgs e)
    {
        if (Loading.IsLoading)
            return;
        try
        {
            BackToHomeTxt.IsVisible = false;
            BackToHomeIndicator.IsVisible = true;
            BackToHomeIndicator.IsRunning = true;

            await Loading.ShowAsync("Return to HomePage...");

            await Task.Delay(500);

            await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await DisplayAlert("Error", "Something went wrong. Please try again.", "OK");
        }
        finally
        {
            await Loading.HideAsync();

            BackToHomeTxt.IsVisible = true;
            BackToHomeIndicator.IsVisible = false;
            BackToHomeIndicator.IsRunning = false;
        }
    }
}