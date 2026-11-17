namespace PreciousPractice;

public partial class PhoneNumberVerified : ContentView
{
    //public event EventHandler? ContinueClicked;
    private Func<Task>? _onContinue;
    private bool _isProcessing;
    public PhoneNumberVerified()
    {
        InitializeComponent();
    }
    public void Configure(string? imageSource =null, string title="", string subtitle ="", Func<Task>? onContinue = null,string buttonText ="Continue")
    {
        ResultImage.Source = imageSource;
        TitleLabel.Text = title;
        SubtitleLabel.Text = subtitle;
        PhoneNumberVerifiedTxt.Text = buttonText;
        _onContinue = onContinue;
    }

    private async void OnContinueTapped(object sender, TappedEventArgs e)
    {
        if (_onContinue == null)
            return;
        if (_isProcessing)
            return;
        try
        {
            _isProcessing = true;
            continueBorder.IsEnabled = false;
            PhoneNumberVerifiedTxt.IsVisible = false;
            PhoneNumberVerifiedIndicator.IsVisible = true;
            PhoneNumberVerifiedIndicator.IsRunning = true;

            await Loading.ShowAsync();
            await Task.Delay(400);

            await _onContinue();
        }
        finally
        {
            await Loading.HideAsync();

            PhoneNumberVerifiedTxt.IsVisible = true;
            PhoneNumberVerifiedIndicator.IsRunning = false;
            PhoneNumberVerifiedIndicator.IsVisible = false;
            continueBorder.IsEnabled = true;
            _isProcessing = false;
        }
    }
}