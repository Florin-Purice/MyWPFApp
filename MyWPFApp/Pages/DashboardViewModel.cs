using FFMpegCore;
using Microsoft.Win32;
using MyWPFApp.Models;

namespace MyWPFApp.Pages;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    public partial int NumberA { get; set; } = 0;
    [ObservableProperty]
    public partial int NumberB { get; set; } = 0;
    [ObservableProperty]
    public partial int Sum { get; set; } = 0;

    [RelayCommand]
    private void CalculateSum()
    {
        Sum = Calculator.Sum(NumberA, NumberB);
    }

    [RelayCommand]
    private async Task AnalyzeMedia()
    {
        OpenFileDialog ofd = new();
        if(ofd.ShowDialog() == true)
        {
            try
            {
                IMediaAnalysis ma = await FFProbe.AnalyseAsync(ofd.FileName);
                MessageBox.Show("ffprobe test ok");
            }
            catch
            { }
        }
    }
}
