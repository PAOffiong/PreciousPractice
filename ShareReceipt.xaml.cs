using SkiaSharp;
using PreciousPractice.Helpers;

namespace PreciousPractice;

public partial class ShareReceipt : ContentPage
{
	public ShareReceipt()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

		TransferAmount.Text = DefaultAccountBalance.Format(TransferReceipt.Amount);
		DateLabel.Text = TransferReceipt.Date.ToString("MMM d, yyyy h:mm:ss tt");

		RecipientName.Text = TransferReceipt.RecipientName.ToUpper();
		RecipientAccount.Text = $"{TransferReceipt.RecipientBankName}|{TransferReceipt.RecipientAccountNumber}";

		SenderName.Text = TransferReceipt.SenderName.ToUpper();
		SenderAccount.Text = $"Pyrowo|{TransferReceipt.Mask(TransferReceipt.SenderAccountNumber)}";

		TransactionType.Text = TransferReceipt.TransactionType;
		TransactionNumber.Text = TransferReceipt.TransactionNumber;
		SessionId.Text = TransferReceipt.SessionId;
    }
	private async void OnBackIconTapped(object sender, TappedEventArgs e)
	{
		await Navigation.SafePopAsync();
	}
	private async void OnDoneTapped(object sender, TappedEventArgs e)
	{
		await Navigation.PopToRootAsync();
	}
    private async void OnShareAsImageTapped(object sender, TappedEventArgs e)
	{
		await ShareAsImageAsync();
	}
	public async Task ShareAsImageAsync()
    {
		if (Loading.IsLoading)
			return;
		try
		{
			IScreenshotResult? screenshot = await ReceiptCard.CaptureAsync();

			if (screenshot is null)
			{
				await DisplayAlert("Error", "Could not capture the receipt.", "OK");
				return;
			}
			await Loading.ShowAsync("Preparing receipt...");

			string filePath = Path.Combine(
				FileSystem.CacheDirectory,
				$"receipt_{TransferReceipt.TransactionNumber}.png");
			 //stream is a way of reading or writing data little by little
			using (Stream imageStream = await screenshot.OpenReadAsync(ScreenshotFormat.Png)) 
			using(FileStream fileStream = File.Create(filePath)) //this creates the actual .png file 
			{
				await imageStream.CopyToAsync(fileStream);

			}
			await Loading.HideAsync();
			//share gives options for the user to pick the app they want to use in sharing the receipt
			await Share.Default.RequestAsync(new ShareFileRequest
			{
				Title = "Share Receipt",
				File = new ShareFile(filePath)
			});
		}
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Share image error: {ex}");

            await DisplayAlert("Error", "Failed to share the receipt.", "OK");
        }
    }
	private async void OnShareAsPDFTapped(object sender, TappedEventArgs e)
	{
        if (Loading.IsLoading)
            return;
        try
		{
			IScreenshotResult? screenshot = await ReceiptCard.CaptureAsync();
            
			

            if (screenshot is null)
			{
				await DisplayAlert("Error", "Could not capture receipt", "OK");
				return;
			}
            await Loading.ShowAsync("Preparing receipt...");

            string imagepath = Path.Combine(
				FileSystem.CacheDirectory,
				$"receipt_{TransferReceipt.TransactionNumber}.png");
			string pdfpath = Path.Combine(FileSystem.CacheDirectory,
				$"receipt_{TransferReceipt.TransactionNumber}.pdf");
			using (Stream imageStream = await screenshot.OpenReadAsync(ScreenshotFormat.Png))
			using (FileStream fileStream = File.Create(imagepath))
			{
				await imageStream.CopyToAsync(fileStream);
			}
            //create a Real pdf containing the image
            using (SKBitmap bitmap = SKBitmap.Decode(imagepath))
            using (FileStream pdfStream = File.Create(pdfpath))
            using (SKDocument document = SKDocument.CreatePdf(pdfStream))
            {
                SKCanvas page = document.BeginPage(bitmap.Width, bitmap.Height);
                page.DrawBitmap(bitmap, 0, 0);
                document.EndPage();

                document.Close();
            }
			await Loading.HideAsync();
            // share the pdf
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Share Receipt",
                File = new ShareFile(pdfpath)
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Share PDF error: {ex}");

            await DisplayAlert("Error", "Failed to share the receipt as PDF.", "OK");
        }
    }
}