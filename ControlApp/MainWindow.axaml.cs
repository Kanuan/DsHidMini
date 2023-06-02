using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;

namespace ControlApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
            InitializeComponent();
        }
    }
}