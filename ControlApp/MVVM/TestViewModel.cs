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
using Nefarius.DsHidMini.ControlApp.UserData;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    internal class TestViewModel : ReactiveObject
    {
        [Reactive] public ControllersUserData UserDataManager { get; set; } = new();

        [Reactive] public string JsonSaveTest { get; set; }
        public void SaveSettingsToJson()
        {
            JsonSaveTest = DeviceSettingsManager.SaveToJsonTest(DevicesCustomsExample);
        }

        

        DeviceSpecificData deviceDatas;
        [Reactive] private VMGroupsContainer DevicesCustomsExample { get; set; }

        public TestViewModel()
        {
            //VMGroupsContainer ProfileExample = new(SettingsContext.DS4W);
            //var profileTab = new SettingTabViewModel("Profile settings", ProfileExample, false);

            string controllerMacTest = "123";

            deviceDatas = UserDataManager.GetDeviceSpecificData(controllerMacTest);
            DevicesCustomsExample = new(deviceDatas.DatasContainter);

            var customsTab = new SettingTabViewModel("Device settings", DevicesCustomsExample, true);

            /// Creating Tabs
            /// _settingsTabs = new ObservableCollection<SettingTabViewModel>();
            /// 
            _settingsTabs = new ObservableCollection<SettingTabViewModel>
            {
                customsTab,
                //profileTab,
            };

            OnTabSelected(SettingsTabs[0]);
            ButtonpressedCommand = new RelayCommand(SaveSettingsToJson);
            TabSelectedCommand = new RelayCommand<SettingTabViewModel>(OnTabSelected);
            SaveChangesCommand = new RelayCommand(OnSaveButtonPressed);
            CancelChangesCommand = new RelayCommand(OnCancelButtonPressed);
            
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

        public SettingsContext? CurrentHIDMode
        {
            get => DevicesCustomsExample.Context;
            set => DevicesCustomsExample.ChangeContextOfAllGroups(value.GetValueOrDefault());
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

        public IRelayCommand SaveChangesCommand { get; }
        public IRelayCommand CancelChangesCommand { get; }

        private void OnTabSelected(SettingTabViewModel? obj)
        {
            CurrentTab = obj;
            foreach (var tab in _settingsTabs)
            {
                tab.IsTabSelected = (tab == CurrentTab) ? true : false;
            }
        }

        private void OnSaveButtonPressed()
        {
            DevicesCustomsExample.SaveAllChangesToBackingData(deviceDatas.DatasContainter);
            UserDataManager.SaveDeviceSpecificDataToDisk(deviceDatas);
        }

        private void OnCancelButtonPressed()
        {
            DevicesCustomsExample.LoadDatasToAllGroups(deviceDatas.DatasContainter);
        }

    }

}