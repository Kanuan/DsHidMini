using Avalonia.Controls;
using Avalonia.Threading;
using Nefarius.DsHidMini.ControlApp.MVVM;
using ReactiveUI;

namespace ControlApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm = new MainViewModel();
        public MainWindow()
        {
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
            this.DataContext= _vm;
            InitializeComponent();
        }
    }
}