using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlApp.UI.Devices
{
    public partial class DeviceDetailsView : UserControl
    {
        public DeviceDetailsView()
        {
            InitializeComponent();
            Nefarius.DsHidMini.ControlApp.MVVM.TestViewModel testViewModel = new();
            this.DataContext = testViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
