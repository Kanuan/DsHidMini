using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace ControlApp.UI.Devices
{

    public partial class ProfilesListView : UserControl
    {
        public ProfilesListView()
        {

            this.DataContext = TestViewModel.vm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
