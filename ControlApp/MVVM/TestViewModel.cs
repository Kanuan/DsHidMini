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
using DynamicData;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System.Reactive;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    internal class TestViewModel : ObservableObject
    {
        [Reactive] public DeviceSettingsManager DeviceSettings { get; set; } = new();

        public void SaveSettingsToJson()
        {
            DeviceSettings.SaveToJsonTest();
        }

        public TestViewModel()
        {
            
         

            /// Creating Tabs
            _settingsTabs = new ObservableCollection<SettingTabViewModel>();


            /*{
                //new SettingTabViewModel("Global/Default", GlobalSettings),
                new SettingTabViewModel("General", deviceSettings.SettingsPerContext[SettingsContext.General]),
                new SettingTabViewModel("SDF", deviceSettings.SDFSettings),
                new SettingTabViewModel("GPJ", deviceSettings.GPJSettings),
                new SettingTabViewModel("SXS", deviceSettings.SXSSettings),
                new SettingTabViewModel("XInput", deviceSettings.XInputSettings),
                new SettingTabViewModel("DS4W", deviceSettings.DS4WSettings),
            };*/


            
            foreach(SettingsModeContext context in DeviceSettings.ActiveContexts)
            {
                SettingTabViewModel tempTab = new SettingTabViewModel(context.ToString(), DeviceSettings.SettingsPerContext[context]);
                _settingsTabs.Add(tempTab);
            }
            

            /*
            "This is the code to be collapsed"
            _settingsTabs.Add(new SettingTabViewModel("Profile: Global", DeviceSettings.SettingsPerContext[SettingsContext.SDF]));
             _settingsTabs.Add(new SettingTabViewModel("Custom", DeviceSettings.SettingsPerContext[SettingsContext.SDF]));
            */

            OnTabSelected(SettingsTabs[0]);
            ButtonpressedCommand = new RelayCommand(SaveSettingsToJson);
            TabSelectedCommand = new RelayCommand<SettingTabViewModel>(OnTabSelected);
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

        public IRelayCommand ButtonpressedCommand { get; }
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