namespace BookLibraryMaui;

public partial class AddBookPage : ContentPage
{
    private double _mySliderValue = 0.25;
    public double MySliderValue
    {
        get => _mySliderValue;
        set
        {
            if (_mySliderValue != value)
            {
                _mySliderValue = value;
                OnPropertyChanged(); // Notify the UI
            }
        }
    }

    public bool IsScanning { get; set; }


    public AddBookPage()
	{
		InitializeComponent();
        BindingContext = this;
        ScanView.BarcodeDataRetrieved += ScanView_OnBarcodeDataRetrieved;
    }

    private void ScanView_OnBarcodeDataRetrieved(string barcodeValue)
    {
        ScanView.StopScanning();
        IsScanning = false;
        OnPropertyChanged(nameof(IsScanning));
    }

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Snap the value to the nearest multiple of 0.25
        var newValue = Math.Round(e.NewValue / 0.25) * 0.25;
        MySliderValue = Math.Clamp(newValue, 0, 5); // Ensure it stays within the valid range       
    }   

    private void OnScan_Clicked(object sender, EventArgs env)
    {
        ScanView.StartScanning();
        IsScanning = true;
        OnPropertyChanged(nameof(IsScanning));
    }
}