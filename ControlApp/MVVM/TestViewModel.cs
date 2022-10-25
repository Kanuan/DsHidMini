using CommunityToolkit.Mvvm.Input;
using Nefarius.DsHidMini.ControlApp.UserData;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    internal class TestViewModel : ReactiveObject
    {
        private DeviceSpecificData deviceUserData;
        private ObservableCollection<SettingTabViewModel> _settingsTabs;
        private SettingTabViewModel _currentTab;
        [Reactive] public ControllersUserData UserDataManager { get; set; } = new();
        [Reactive] private VMGroupsContainer DeviceCustomsVM { get; set; }
        [Reactive] private SettingTabViewModel DeviceCustomSettingsTab { get; set; } = new SettingTabViewModel("Custom", null, true);
        [Reactive] private SettingTabViewModel DeviceProfileSettingsTab { get; set; } = new SettingTabViewModel("Profile", null, false);
        [Reactive] public string JsonSaveTest { get; set; }



        public TestViewModel()
        {
            string controllerMacTest = "123";
            deviceUserData = UserDataManager.GetDeviceSpecificData(controllerMacTest);

            DeviceCustomsVM = new(deviceUserData.DatasContainter);
            DeviceCustomSettingsTab.setNewSettingsVMGroupsContainer(DeviceCustomsVM);

            if (UserDataManager.ProfilesPerGuid.ContainsKey(deviceUserData.GuidOfProfileToUse))
            {
                var DeviceProfileVM = UserDataManager.ProfilesPerGuid[deviceUserData.GuidOfProfileToUse].GetProfileVMGroupsContainer();
                DeviceProfileSettingsTab.setNewSettingsVMGroupsContainer(DeviceProfileVM);
            }

            UpdateSettingsEditor();

            //OnTabSelected(SettingsTabs[0]);
            ButtonpressedCommand = new RelayCommand(SaveSettingsToJson);
            TabSelectedCommand = new RelayCommand<SettingTabViewModel>(OnTabSelected);
            SaveChangesCommand = new RelayCommand(OnSaveButtonPressed);
            CancelChangesCommand = new RelayCommand(OnCancelButtonPressed);

            // This is a ReactiveUI helper that checks if current SettingsMode is "Profile" when changing SettingsMode
            isDeviceInProfileSettingsMode = this
                .WhenAnyValue(x => x.CurrentDeviceSettingsMode)
                .Select(whatever => CurrentDeviceSettingsMode == SettingsModes.Profile)
                .ToProperty(this, x => x.IsDeviceInProfileSettingsMode);
        }

        public void UpdateSettingsEditor()
        {
            switch (deviceUserData.SettingsMode)
            {
                case SettingsModes.Profile:
                    CurrentTab = DeviceProfileSettingsTab;
                    break;
                case SettingsModes.Custom:
                default:
                    CurrentTab = DeviceCustomSettingsTab;
                    break;
            }
        }

        public void ChangeProfileForDevice(ProfileData profile)
        {
            deviceUserData.GuidOfProfileToUse = profile.ProfileGuid;
            DeviceProfileSettingsTab.setNewSettingsVMGroupsContainer(profile.GetProfileVMGroupsContainer());
        }

        public void SaveSettingsToJson()
        {
            JsonSaveTest = DeviceSettingsManager.SaveToJsonTest(deviceUserData.DatasContainter);
        }

        /// <summary>
        /// This should update on its own when changing SettingsMode.
        /// Initialized in the base constructor
        /// </summary>
        readonly ObservableAsPropertyHelper<bool> isDeviceInProfileSettingsMode;
        public bool IsDeviceInProfileSettingsMode { get => isDeviceInProfileSettingsMode.Value; }



        public readonly List<SettingsModes> settingsModesList = new List<SettingsModes>
        {
            SettingsModes.Global,
            SettingsModes.Profile,
            SettingsModes.Custom,
        };
        public List<SettingsModes> SettingsModesList => settingsModesList;
        public SettingsModes CurrentDeviceSettingsMode
        {
            get => deviceUserData.SettingsMode;
            set
            {
                deviceUserData.SettingsMode = value;
                UpdateSettingsEditor();
                this.RaisePropertyChanged(nameof(CurrentDeviceSettingsMode));
            }
        }

        public ProfileData? CurrentlySelectedProfile
        {
            get => UserDataManager.ProfilesPerGuid.ContainsKey(deviceUserData.GuidOfProfileToUse)
                ? UserDataManager.ProfilesPerGuid[deviceUserData.GuidOfProfileToUse] : null;
            set
            {
                ChangeProfileForDevice(value);
            }
        }

        

        public readonly List<SettingsContext> hidDeviceModesList = new List<SettingsContext>
        {
            SettingsContext.SDF,
            SettingsContext.GPJ,
            SettingsContext.SXS,
            SettingsContext.DS4W,
            SettingsContext.XInput,
        };
        public SettingsContext? CurrentHIDMode
        {
            get => DeviceCustomsVM.Context;
            set => DeviceCustomsVM.ChangeContextOfAllGroups(value.GetValueOrDefault());
        }
        public List<SettingsContext> HIDDeviceModesList => hidDeviceModesList;



        public List<ProfileData> ListOfProfiles => UserDataManager.Profiles;



        public ObservableCollection<SettingTabViewModel> SettingsTabs
        {
            get => _settingsTabs;
            set => this.RaiseAndSetIfChanged(ref _settingsTabs, value);
        }

        public SettingTabViewModel CurrentTab
        {
            get => _currentTab;
            set => this.RaiseAndSetIfChanged(ref _currentTab, value);
        }

        
        // ---------------------------------------- IRelayCommand

        public IRelayCommand ButtonpressedCommand { get; }
        public IRelayCommand<SettingTabViewModel> TabSelectedCommand { get; }

        public IRelayCommand SaveChangesCommand { get; }
        public IRelayCommand CancelChangesCommand { get; }

        // ---------------------------------------- Commands

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
            DeviceCustomsVM.SaveAllChangesToBackingData(deviceUserData.DatasContainter);
            UserDataManager.SaveDeviceSpecificDataToDisk(deviceUserData);
        }

        private void OnCancelButtonPressed()
        {
            DeviceCustomsVM.LoadDatasToAllGroups(deviceUserData.DatasContainter);
        }

    }

}