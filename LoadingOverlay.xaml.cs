namespace PreciousPractice;

public partial class LoadingOverlay : ContentView
{
	public bool IsLoading { get; private set; }
	public LoadingOverlay()
	{
		InitializeComponent();
	}
	public async Task ShowAsync(string message = "please wait ...")
	{
		MessageLabel.Text = message;
		
		IsLoading = true;
		IsVisible = true;

		LoaderGif.IsAnimationPlaying = false;
		LoaderGif.IsAnimationPlaying = true;
		
		await this.FadeTo(1, 150);
	}
	public async Task HideAsync()
	{
        await this.FadeTo(0, 150);
        IsLoading = false;
		IsVisible = false;
		LoaderGif.IsAnimationPlaying = false;
	}
	public async Task NavigateAsync(INavigation navigation, Func<Page> createPage, string message = "Loading...")
	{
		if (IsLoading)
			return;

		await ShowAsync(message);

		try
		{
			await Task.Delay(300);
			Page nextPage = createPage();
			await navigation.PushAsync(nextPage);
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Navigation error{ex}");
		}
		finally
		{
			this.Opacity = 0;
			IsVisible = false;
			IsLoading= false;
			LoaderGif.IsAnimationPlaying = false;
		}
	}
}