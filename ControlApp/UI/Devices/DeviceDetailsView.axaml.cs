using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;



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
