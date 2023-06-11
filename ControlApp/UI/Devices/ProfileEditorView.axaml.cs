using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Nefarius.DsHidMini.ControlApp.UserData;


namespace ControlApp.UI.Devices
{
    public partial class ProfileEditorView : UserControl
    {
        public ProfileEditorView()
        {
            this.DataContext = ForcingThingsAround.vm;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
