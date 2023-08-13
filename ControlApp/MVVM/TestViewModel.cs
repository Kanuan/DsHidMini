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
        // ------------------------------------------------------ FIELDS

        internal static ControllersUserData UserDataManager = new ControllersUserData();
        internal static ProfileEditorViewModel vm = new ProfileEditorViewModel();
        private readonly PnPDevice _device;
        private DeviceSpecificData deviceUserData;
        public readonly List<SettingsModes> settingsModesList = new List<SettingsModes>
        {
            SettingsModes.Global,
            SettingsModes.Profile,
            SettingsModes.Custom,
        };

        // ------------------------------------------------------ PROPERTIES

        internal string DeviceAddress { get; set; }

        [Reactive] private VMGroupsContainer DeviceCustomsVM { get; set; }
        [Reactive] private VMGroupsContainer SelectedGroupsVM { get; set; }

        internal string DisplayName { get; set; }
        [Reactive] public bool IsEditorEnabled { get; set; }
        [Reactive] public bool IsProfileSelectorVisible { get; set; }
        public List<SettingsModes> SettingsModesList => settingsModesList;

        [Reactive] public SettingsModes CurrentDeviceSettingsMode { get; set; }

        // ------------------------------------------------------ CONSTRUCTOR

        public TestViewModel(PnPDevice device)
        {
            _device = device;
            // Hard-coded controller MAC address for testing purposes
            DeviceAddress = _device.GetProperty<string>(DsHidMiniDriver.DeviceAddressProperty).ToUpper();
            // Loads correspondent controller data based on controller's MAC address 
            deviceUserData = UserDataManager.GetDeviceData(DeviceAddress);
            DisplayName = DeviceAddress;
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

            this.WhenAnyValue(x => x.CurrentDeviceSettingsMode, x => x.SelectedProfile)
                .Subscribe(x => UpdateEditor());
        }

        // ---------------------------------------- ReactiveCommands

        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelChangesCommand { get; }

        // ------------------------------------------------------ METHODS

        public void ChangeProfileForDevice(ProfileData profile)
        {
            deviceUserData.GuidOfProfileToUse = profile.ProfileGuid;
            //ProfileCustomsVM = profile.GetProfileVMGroupsContainer();
        }

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
            SelectedGroupsVM.AllowEditing = CurrentDeviceSettingsMode == SettingsModes.Custom;
            IsProfileSelectorVisible = CurrentDeviceSettingsMode == SettingsModes.Profile;
        }

        [Reactive] public ProfileData? SelectedProfile { get; set; }

        public List<ProfileData> ListOfProfiles => UserDataManager.Profiles;

        // ---------------------------------------- Commands

        private void OnSaveButtonPressed()
        {
            deviceUserData.SettingsMode = CurrentDeviceSettingsMode;

            if(CurrentDeviceSettingsMode != SettingsModes.Global)
            {
                SelectedGroupsVM.SaveAllChangesToBackingData(deviceUserData.DatasContainter);
            }

            if (CurrentDeviceSettingsMode == SettingsModes.Profile)
            {
                deviceUserData.GuidOfProfileToUse = SelectedProfile.ProfileGuid;
            }

            UserDataManager.ConvertAndSaveSettingsToDshmSettingsFile();
        }

        private void OnCancelButtonPressed()
        {
            DeviceCustomsVM.LoadDatasToAllGroups(deviceUserData.DatasContainter);
            SelectedProfile = UserDataManager.GetProfile(deviceUserData.GuidOfProfileToUse);
            CurrentDeviceSettingsMode = deviceUserData.SettingsMode;
        }

    }

}