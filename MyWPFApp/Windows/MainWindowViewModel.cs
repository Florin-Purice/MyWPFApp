using MyWPFApp.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MyWPFApp.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "WPF UI - MyWPFApp";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems =
    [
        new NavigationViewItem()
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = typeof(DashboardPage)
        },
        new NavigationViewItem()
        {
            Content = "Data",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            TargetPageType = typeof(DataPage)
        }
    ];

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems =
    [
        new NavigationViewItem()
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = typeof(SettingsPage)
        }
    ];

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems =
    [
        new MenuItem { Header = "Home", Tag = "tray_home" }
    ];
}
