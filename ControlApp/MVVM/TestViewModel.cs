using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.UserData;
using Nefarius.Utilities.DeviceManagement.PnP;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
        private string deviceAddress;

        [Reactive] private VMGroupsContainer DeviceCustomsVM { get; set; }
        [Reactive] private VMGroupsContainer SelectedGroupsVM { get; set; }

        internal string DisplayName { get; set; }

        internal string DeviceAddress => deviceAddress;

        public TestViewModel(PnPDevice device)
        {
            _device = device;
            // Hard-coded controller MAC address for testing purposes
            deviceAddress = _device.GetProperty<string>(DsHidMiniDriver.DeviceAddressProperty).ToUpper();
            DisplayName = deviceAddress;
            // Loads correspondent controller data based on controller's MAC address 
            deviceUserData = UserDataManager.GetDeviceData(deviceAddress);

            // Loads device' specific custom settings from its BackingDataContainer into the Settings Groups VM
            DeviceCustomsVM = new(deviceUserData.DatasContainter);

            // Checks if the Profile GUID the controller is set to use actually exists in the list of disk profiles and loads it if so
            if (UserDataManager.GetProfile(deviceUserData.GuidOfProfileToUse) == null)
            {
                deviceUserData.GuidOfProfileToUse = ProfileData.DefaultGuid;
            }
            SelectedProfile = UserDataManager.GetProfile(deviceUserData.GuidOfProfileToUse);

            CurrentDeviceSettingsMode = deviceUserData.SettingsMode;

            SaveChangesCommand = ReactiveCommand.Create(OnSaveButtonPressed);
            CancelChangesCommand = ReactiveCommand.Create(OnCancelButtonPressed);

            // This is a ReactiveUI helper that checks if current SettingsMode is "Profile" when changing SettingsMode
            isEditorLocked = this
                .WhenAnyValue(x => x.CurrentDeviceSettingsMode)
                .Select(whatever => CurrentDeviceSettingsMode == SettingsModes.Profile)
                .ToProperty(this, x => x.IsEditorLocked);

            this.WhenAnyValue(x => x.CurrentDeviceSettingsMode, x => x.SelectedProfile)
                .Subscribe(x => UpdateEditor());
        }

        public void ChangeProfileForDevice(ProfileData profile)
        {
            deviceUserData.GuidOfProfileToUse = profile.ProfileGuid;
            //ProfileCustomsVM = profile.GetProfileVMGroupsContainer();
        }

        /// <summary>
        /// This should update on its own when changing SettingsMode.
        /// Initialized in the base constructor
        /// </summary>
        readonly ObservableAsPropertyHelper<bool> isEditorLocked;
        public bool IsEditorLocked { get => isEditorLocked.Value; }

        public readonly List<SettingsModes> settingsModesList = new List<SettingsModes>
        {
            SettingsModes.Global,
            SettingsModes.Profile,
            SettingsModes.Custom,
        };
        public List<SettingsModes> SettingsModesList => settingsModesList;

        [Reactive] public SettingsModes CurrentDeviceSettingsMode { get; set; }

        public void UpdateEditor()
        {
            switch (CurrentDeviceSettingsMode)
            {
                case SettingsModes.Profile:
                    SelectedGroupsVM = SelectedProfile.GetProfileVMGroupsContainer();
                    break;
                case SettingsModes.Custom:
                    SelectedGroupsVM = DeviceCustomsVM;
                    break;
                case SettingsModes.Global:
                default:
                    SelectedGroupsVM = UserDataManager.GlobalProfile.GetProfileVMGroupsContainer();
                    break;
            }
        }

        [Reactive] public ProfileData? SelectedProfile { get; set; }

        public List<ProfileData> ListOfProfiles => UserDataManager.Profiles;

        // ---------------------------------------- ReactiveCommands

        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelChangesCommand { get; }

        // ---------------------------------------- Commands

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