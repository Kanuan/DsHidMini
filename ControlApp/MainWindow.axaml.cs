using Avalonia.Controls;
using Avalonia.Threading;
using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Nefarius.Utilities.DeviceManagement.PnP;
using ReactiveUI;

namespace ControlApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm = new MainViewModel();
        public MainWindow()
        {
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
            this.DataContext = _vm;
            var instance = 0;
            while (Devcon.FindByInterfaceGuid(DsHidMiniDriver.DeviceInterfaceGuid, out var path, out var instanceId, instance++))
                _vm.Devices.Add(new TestViewModel(PnPDevice.GetDeviceByInstanceId(instanceId)));
            InitializeComponent();
        }
    }
}