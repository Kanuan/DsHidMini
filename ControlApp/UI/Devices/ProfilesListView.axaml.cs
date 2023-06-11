using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace ControlApp.UI.Devices
{
    internal static class ForcingThingsAround
    {
        internal static ProfileEditorViewModel vm = new ProfileEditorViewModel();
    }
    public partial class ProfilesListView : UserControl
    {
        public ProfilesListView()
        {
            this.DataContext= ForcingThingsAround.vm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
