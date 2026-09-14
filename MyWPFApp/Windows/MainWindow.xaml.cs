using FFMpegCore;
using FFMpegCore.Exceptions;
using FFMpegCore.Extensions.Downloader;
using FFMpegCore.Helpers;
using MyWPFApp.Controls;
using System.IO;
using Velopack;
using Velopack.Sources;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxResult = System.Windows.MessageBoxResult;
using SplashScreen = MyWPFApp.Controls.SplashScreen;

namespace MyWPFApp.Windows;

public partial class MainWindow : INavigationWindow
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(
        MainWindowViewModel viewModel,
        INavigationViewPageProvider navigationViewPageProvider,
        INavigationService navigationService
    )
    {
        ViewModel = viewModel;
        DataContext = this;

        SystemThemeWatcher.Watch(this);

        InitializeComponent();
        SetPageService(navigationViewPageProvider);

        navigationService.SetNavigationControl(RootNavigation);
        GlobalFFOptions.Current.BinaryFolder = @"..\ffbin";
        InitializeSplashScreen();
    }

    private void InitializeSplashScreen()
    {
        List<SplashScreenTask> tasks =
        [
            new SplashScreenTask(
                "Checking for updates...",
                UpdateMyApp
                ),
            new SplashScreenTask(
                "Verifying ffmpeg install...",
                CheckFFMpegInstall
                )
        ];
        SplashScreen splashScreen = new(tasks);
        SplashScreenHost.Content = splashScreen;
        Task.Run(() => splashScreen.RunTasksAndHideAsync());
    }

    private static async Task CheckFFMpegInstall()
    {
        string binaryPath = GlobalFFOptions.GetFFMpegBinaryPath();

        bool exists = File.Exists(binaryPath);
        MessageBox.Show($"binary path = {binaryPath}, exists {exists}");
        try
        {
            FFMpegHelper.VerifyFFMpegExists(GlobalFFOptions.Current);
        }
        catch
        {
            // ffmpeg was not found
            // ask for download confirmation
            MessageBoxResult mbResult = MessageBox.Show("FFMpeg not found. Install ffmpeg?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (mbResult == MessageBoxResult.Yes)
            {
                try
                {
                    // create bin folder
                    string dirPath = GlobalFFOptions.Current.BinaryFolder;
                    Directory.CreateDirectory(dirPath);
                    // download ffmpeg binaries
                    List<string> downloaded = await FFMpegDownloader.DownloadBinaries();
                }
                catch
                {
                    //App.Current.Shutdown();
                }
            }
            else;
            //App.Current.Shutdown();
        }
    }

    private static async Task UpdateMyApp()
    {
#if !DEBUG
        IUpdateSource updateSource = new GithubSource("https://github.com/Florin-Purice/MyWPFApp", accessToken: null, prerelease: false);
        UpdateManager mgr = new(updateSource);

        // check for new version
        UpdateInfo? newVersion = await mgr.CheckForUpdatesAsync();
        if (newVersion == null)
            return; // no update available

        // ask for update confirmation
        MessageBoxResult mbResult = MessageBox.Show("New version found. Update now?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
        if (mbResult == MessageBoxResult.Yes)
        {
            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
#endif
    }

    #region INavigationWindow methods

    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) => RootNavigation.SetPageProviderService(navigationViewPageProvider);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    #endregion INavigationWindow methods

    /// <summary>
    /// Raises the closed event.
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        // Make sure that closing this window will begin the process of closing the application.
        Application.Current.Shutdown();
    }

    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}
