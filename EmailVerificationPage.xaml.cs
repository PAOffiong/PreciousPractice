namespace PreciousPractice;

public partial class EmailVerificationPage : ContentPage
{
    public EmailVerificationPage()
    {
        InitializeComponent();
    }
    private async void OnBackIconTapped(object sender, TappedEventArgs e)
    {
        await Navigation.SafePopAsync();
    }
    private async void NavToRegisterForgotPassword(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterForgotPassword());
    }
    
}    