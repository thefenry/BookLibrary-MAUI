using ZXing.Net.Maui;

namespace BookLibraryMaui.ContentViews;

public partial class CameraScanner : ContentView
{    
    public event Action<string> BarcodeDataRetrieved;

    public CameraScanner()
    {
        InitializeComponent();

        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false,
        };
        StopScanning();
    }

    public void StartScanning()
    {
        cameraBarcodeReaderView.IsDetecting = true;
    }

    public void StopScanning()
    {
        cameraBarcodeReaderView.IsDetecting = false;
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var result = e.Results?.FirstOrDefault();
        if (result != null)
        {            
            BarcodeDataRetrieved?.Invoke(result.Value);
        }
    }
}