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

            var instance = 0;
            while (Devcon.Find(DsHidMiniDriver.DeviceInterfaceGuid, out var path, out var instanceId, instance++))
                _vm.Devices.Add(new TestViewModel(PnPDevice.GetDeviceByInstanceId(instanceId)));

            this.DataContext= _vm;
            InitializeComponent();
        }
    }
}