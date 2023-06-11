using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Nefarius.DsHidMini.ControlApp.UserData;


namespace ControlApp.UI.Devices
{
    public partial class DeviceDetailsView : UserControl
    {
        public DeviceDetailsView()
        {
            TestViewModel testViewModel = new();
            this.DataContext = testViewModel;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
