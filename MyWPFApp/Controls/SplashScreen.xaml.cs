using System.Windows.Controls;

namespace MyWPFApp.Controls;

/// <summary>
/// Interaction logic for SplashScreen.xaml
/// </summary>
public partial class SplashScreen : UserControl
{
    private List<SplashScreenTask> _tasks;

    public SplashScreen(List<SplashScreenTask> tasks)
    {
        _tasks = tasks;
        ViewModel = new SplashScreenViewModel();
        DataContext = this;
        InitializeComponent();
    }

    public SplashScreenViewModel ViewModel { get; private set; }

    public async Task RunTasksAndHideAsync()
    {
        foreach (SplashScreenTask sstask in _tasks)
        {
            ViewModel.Message = sstask.Message;
            await sstask.Operation.Invoke(UpdateMessage);
        }
        Collapse();
    }

    private void UpdateMessage(string newMessage)
    {
        ViewModel.Message = newMessage;
    }

    private void Collapse()
    {
        Dispatcher?.Invoke(() => Visibility = Visibility.Collapsed);
    }
}

public record class SplashScreenTask(string Message, Func<Action<string>, Task> Operation);
