using ZXing.Net.Maui;

namespace BookLibraryMaui.ContentViews;

public partial class CameraScanner : ContentView
{    
    public event Action<string>? BarcodeDataRetrieved;
    private bool _isProcessingBarcode;

    public CameraScanner()
    {
        InitializeComponent();

        CameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = false,
        };

        CameraBarcodeReaderView.IsTorchOn = true;

        StopScanning();
    }

    public void StartScanning()
    {
        _isProcessingBarcode = false;
        CameraBarcodeReaderView.IsDetecting = true;
    }

    public void StopScanning()
    {
        CameraBarcodeReaderView.IsDetecting = false;
    }

    private void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_isProcessingBarcode)
            return;

        var result = e.Results?.FirstOrDefault();
        if (result != null)
        {
            _isProcessingBarcode = true;
            BarcodeDataRetrieved?.Invoke(result.Value);
            StopScanning();
        }
    }
}