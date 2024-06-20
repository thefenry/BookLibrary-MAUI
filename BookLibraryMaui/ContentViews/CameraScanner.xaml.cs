using ZXing.Net.Maui;

namespace BookLibraryMaui.ContentViews;

public partial class CameraScanner : ContentView
{    
    public event Action<string>? BarcodeDataRetrieved;

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
        CameraBarcodeReaderView.IsDetecting = true;
    }

    public void StopScanning()
    {
        CameraBarcodeReaderView.IsDetecting = false;
    }

    private void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var result = e.Results?.FirstOrDefault();
        if (result != null)
        {            
            BarcodeDataRetrieved?.Invoke(result.Value);
        }
    }
}