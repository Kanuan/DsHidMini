using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;
using FontAwesome5;
using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.Util;
using Nefarius.DsHidMini.ControlApp.Util.Web;
using Nefarius.Utilities.DeviceManagement.PnP;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlApp.UI.Devices;
using System.Collections.ObjectModel;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    internal class TestViewModel
        : ObservableObject
    {

        public TestViewModel()
        {
            TabSelectedCommand = new RelayCommand<SettingTabViewModel>(OnTabSelected);

            /// Creating dummy device settings object for testing
            /// 
            ///
            DeviceModesSettings GlobalSettings = new();
            GlobalSettings.currentSettingContext = SettingsContext.Global;

            DeviceModesSettings GeneralSettings = new();
            GeneralSettings.currentSettingContext = SettingsContext.General;

            DeviceModesSettings SDFSettings = new();
            DeviceModesSettings GPJSettings = new();
            DeviceModesSettings XInputSettings = new();
            DeviceModesSettings DS4WSettings = new();

            /// Creating Tabs
            _settingsTabs = new ObservableCollection<SettingTabViewModel>
            {
                new SettingTabViewModel("Global/Default", GlobalSettings),
                new SettingTabViewModel("General", GeneralSettings),
                new SettingTabViewModel("SDF", SDFSettings),
                new SettingTabViewModel("GPJ", GPJSettings),
                new SettingTabViewModel("XInput", XInputSettings),
                new SettingTabViewModel("DS4W", DS4WSettings),
            };

            CurrentTab = SettingsTabs[1];   
        }


        private ObservableCollection<SettingTabViewModel> _settingsTabs;
        private SettingTabViewModel _currentTab;



        public ObservableCollection<SettingTabViewModel> SettingsTabs
        {
            get
            {
                return _settingsTabs;
            }
            set
            {
                SetProperty(ref _settingsTabs, value);
            }
        }

        public SettingTabViewModel CurrentTab
        {
            get
            {
                return _currentTab;
            }
            set
            {
                SetProperty(ref _currentTab, value);
            }
        }

        public IRelayCommand<SettingTabViewModel> TabSelectedCommand { get; }
        private void OnTabSelected(SettingTabViewModel? obj)
        {
            CurrentTab = obj;
        }

    }

}