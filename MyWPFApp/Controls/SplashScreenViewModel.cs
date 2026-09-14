using System;
using System.Collections.Generic;
using System.Text;

namespace MyWPFApp.Controls;

public partial class SplashScreenViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;
}
