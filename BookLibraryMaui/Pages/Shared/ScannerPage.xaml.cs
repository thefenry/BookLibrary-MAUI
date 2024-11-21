using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace BookLibraryMaui.Pages.Shared;

public partial class ScannerPage : ContentPage
{
    private readonly TaskCompletionSource<string> _scanResultCompletionSource;

    public Task<string> ScanResult => _scanResultCompletionSource.Task;

	public ScannerPage()
	{
		InitializeComponent();
        _scanResultCompletionSource = new TaskCompletionSource<string>();

        BarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = false,
            TryHarder = true,
        };

        BarcodeReaderView.IsTorchOn = !BarcodeReaderView.IsTorchOn;
	}

    private void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // Stop detecting to avoid multiple triggers
        BarcodeReaderView.IsDetecting = false;

        // Get the detected EAN-13 barcode
        var barcodeValue = e.Results.FirstOrDefault()?.Value;
        if (barcodeValue != null && (barcodeValue.StartsWith("978") || barcodeValue.StartsWith("979")))
        {
            // Set the result and navigate back if it's a valid ISBN prefix
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                _scanResultCompletionSource.SetResult(barcodeValue);
                await Navigation.PopAsync(true);
            });
        }
        else
        {
            // Re-enable detecting if the scanned code is not a valid ISBN
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BarcodeReaderView.IsDetecting = true;
            });
        }
    }
}