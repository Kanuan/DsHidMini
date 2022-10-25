using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using System.Collections.Generic;

namespace ControlApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance; // Interesting line of code that fixes the issue!
            InitializeComponent();
        }
    }
}