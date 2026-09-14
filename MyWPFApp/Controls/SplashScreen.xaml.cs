using System.Windows.Controls;

namespace MyWPFApp.Controls;

/// <summary>
/// Interaction logic for SplashScreen.xaml
/// </summary>
public partial class SplashScreen : UserControl
{
    private readonly List<Func<Action<string>, Task>> _tasks;

    public SplashScreen(List<Func<Action<string>, Task>> taskWithMessageUpdateCallbackList)
    {
        _tasks = taskWithMessageUpdateCallbackList;
        ViewModel = new SplashScreenViewModel();
        DataContext = this;
        InitializeComponent();
    }

    public SplashScreenViewModel ViewModel { get; private set; }

    public async Task RunTasksAndHideAsync()
    {
        foreach (Func<Action<string>, Task> task in _tasks)
            await task.Invoke(UpdateMessage);
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
