using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.UserData;
using Nefarius.Utilities.DeviceManagement.PnP;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    internal class TestViewModel : ReactiveObject
    {
        internal static ControllersUserData UserDataManager = new ControllersUserData();
        internal static ProfileEditorViewModel vm = new ProfileEditorViewModel();
        private readonly PnPDevice _device;


        private DeviceSpecificData deviceUserData;
        private ObservableCollection<SettingTabViewModel> _settingsTabs;
        private SettingTabViewModel _currentTab;
        private string deviceAddress;

        [Reactive] private VMGroupsContainer DeviceCustomsVM { get; set; }
        [Reactive] private SettingTabViewModel DeviceCustomSettingsTab { get; set; } = new SettingTabViewModel("Custom", null, true);
        [Reactive] private SettingTabViewModel DeviceProfileSettingsTab { get; set; } = new SettingTabViewModel("Profile", null, false);

        internal string DisplayName { get; set; }

        internal string DeviceAddress => deviceAddress;

        public TestViewModel(PnPDevice device)
        {
            _device = device;
            // Hard-coded controller MAC address for testing purposes
            deviceAddress = _device.GetProperty<string>(DsHidMiniDriver.DeviceAddressProperty).ToUpper();
            DisplayName = deviceAddress;
            // Loads correspondent controller data based on controller's MAC address 
            deviceUserData = UserDataManager.GetDeviceDataByDeviceAddress(deviceAddress);

            // Loads device' specific custom settings from its BackingDataContainer into the Settings Groups VM
            DeviceCustomsVM = new(deviceUserData.DatasContainter);
            // Loads the Settings Groups VM into the tab settings VM
            DeviceCustomSettingsTab.setNewSettingsVMGroupsContainer(DeviceCustomsVM);

            // Checks if the Profile GUID the controller is set to use actually exists in the list of disk profiles and loads it if so
            if (UserDataManager.ProfilesPerGuid.ContainsKey(deviceUserData.GuidOfProfileToUse))
            {
                var DeviceProfileVM = UserDataManager.ProfilesPerGuid[deviceUserData.GuidOfProfileToUse].GetProfileVMGroupsContainer();
                DeviceProfileSettingsTab.setNewSettingsVMGroupsContainer(DeviceProfileVM);
            }

            // Selects correct tab for the settings based on if it's set to use custom, profile or global settings
            UpdateSettingsEditor();

            SaveChangesCommand = ReactiveCommand.Create(OnSaveButtonPressed);
            CancelChangesCommand = ReactiveCommand.Create(OnCancelButtonPressed);
            TabSelectedCommand = ReactiveCommand.Create<SettingTabViewModel>(OnTabSelected);

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


        // ---------------------------------------- ReactiveCommands

        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelChangesCommand { get; }
        public ReactiveCommand<SettingTabViewModel, Unit> TabSelectedCommand { get; }

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