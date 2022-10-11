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
    internal class TestViewModel : ObservableObject
    {

        public TestViewModel()
        {
            TabSelectedCommand = new RelayCommand<SettingTabViewModel>(OnTabSelected);

            /// Creating dummy device settings object for testing
            /// 
            ///
            DeviceSettingsManager deviceSettings = new();


            /// Creating Tabs
            _settingsTabs = new ObservableCollection<SettingTabViewModel>
            {
                //new SettingTabViewModel("Global/Default", GlobalSettings),
                new SettingTabViewModel("General", deviceSettings.GeneralSettings),
                new SettingTabViewModel("SDF", deviceSettings.SDFSettings),
                new SettingTabViewModel("GPJ", deviceSettings.GPJSettings),
                new SettingTabViewModel("SXS", deviceSettings.SXSSettings),
                new SettingTabViewModel("XInput", deviceSettings.XInputSettings),
                new SettingTabViewModel("DS4W", deviceSettings.DS4WSettings),
            };

            OnTabSelected(SettingsTabs[0]);  
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
            foreach (var tab in _settingsTabs)
            {
                tab.IsTabSelected = (tab == CurrentTab) ? true : false;
            }
        }

    }

}