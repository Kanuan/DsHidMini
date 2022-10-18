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
    internal class TestViewModel : ReactiveObject
    {
        [Reactive] public DeviceSettingsManager DeviceSettings { get; set; } = new();

        [Reactive] public string JsonSaveTest { get; set; }
        public void SaveSettingsToJson()
        {
            JsonSaveTest = DeviceSettingsManager.SaveToJsonTest(DevicesCustomsExample);
        }

        SettingsContainer ProfileExample = new(SettingsContext.General);
        SettingsContainer DevicesCustomsExample = new(SettingsContext.SDF);

        

        public TestViewModel()
        {


            var profileTab = new SettingTabViewModel("Profile settings", ProfileExample, false);
            var customsTab = new SettingTabViewModel("Device settings", DevicesCustomsExample, true);


            /// Creating Tabs
            /// _settingsTabs = new ObservableCollection<SettingTabViewModel>();
            /// 
            _settingsTabs = new ObservableCollection<SettingTabViewModel>
            {
                customsTab,
                profileTab,
            };

            OnTabSelected(SettingsTabs[0]);
            ButtonpressedCommand = new RelayCommand(SaveSettingsToJson);
            TabSelectedCommand = new RelayCommand<SettingTabViewModel>(OnTabSelected);
        }


        private ObservableCollection<SettingTabViewModel> _settingsTabs;
        private SettingTabViewModel _currentTab;

        public readonly List<SettingsContext> hidDeviceModesList = new List<SettingsContext>
        {
            SettingsContext.SDF,
            SettingsContext.GPJ,
            SettingsContext.SXS,
            SettingsContext.DS4W,
            SettingsContext.XInput,
        };

        public List<SettingsContext> HIDDeviceModesList
        {
            get => hidDeviceModesList;
        }

        public SettingsContext CurrentHIDMode
        {
            get => DevicesCustomsExample.Context;
            set => DevicesCustomsExample.ChangeContextOfAllGroups(value);
        }

        public ObservableCollection<SettingTabViewModel> SettingsTabs
        {
            get
            {
                return _settingsTabs;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _settingsTabs, value);
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
                this.RaiseAndSetIfChanged(ref _currentTab, value);
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