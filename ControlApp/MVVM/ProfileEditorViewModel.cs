using DynamicData.Binding;
using Nefarius.DsHidMini.ControlApp.UserData;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{

    internal class ProfileEditorViewModel : ReactiveObject
    {
        // ----------------------------------------------------------- FIELDS

        private ObservableCollection<ProfileData> _profiles;
        private ProfileData? _selectedProfile;

        // ----------------------------------------------------------- PROPERTIES

        [Reactive] public ObservableCollection<ProfileData> Profiles { get => _profiles; set => _profiles = value; }

        public ProfileData? SelectedProfile
        {
            get => _selectedProfile;
            set
            { 
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);
                OnProfileSelected(value);
            }
        }
        public SettingTabViewModel SettingsEditor
        {
            get => _settingsEditor;
            set => this.RaiseAndSetIfChanged(ref _settingsEditor, value);
        }

        public readonly List<SettingsModes> settingsModesList = new List<SettingsModes>
        {
            SettingsModes.Global,
            SettingsModes.Profile,
            SettingsModes.Custom,
        };
        public List<SettingsModes> SettingsModesList => settingsModesList;

        public List<ProfileData> ListOfProfiles => TestViewModel.UserDataManager.Profiles;

        public ObservableCollection<SettingTabViewModel> SettingsTabs
        {
            get => _settingsTabs;
            set => this.RaiseAndSetIfChanged(ref _settingsTabs, value);
        }

        // ----------------------------------------------------------- CONSTRUCTOR

        public ProfileEditorViewModel()
        {
            SettingsEditor = new("wot", null, true);
            Profiles = new ObservableCollection<ProfileData>(TestViewModel.UserDataManager.Profiles);

            CreateProfileCommand = ReactiveCommand.Create(OnAddProfileButtonPressed);
            DeleteProfileCommand = ReactiveCommand.Create<ProfileData>(OnDeleteProfileButtonPressed);
            ProfileSelectedCommand = ReactiveCommand.Create<ProfileData>(OnProfileSelected);
            SetProfileAsGlobalCommand = ReactiveCommand.Create<ProfileData>(OnSetAsGlobalButtonPressed);
            SaveChangesCommand = ReactiveCommand.Create(OnSaveButtonPressed);
            CancelChangesCommand = ReactiveCommand.Create(OnCancelButtonPressed);
        }

        public void UpdateProfileList()
        {
            Profiles = new ObservableCollection<ProfileData>(TestViewModel.UserDataManager.Profiles);
        }


        // ---------------------------------------- ReactiveCommands

        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelChangesCommand { get; }
        public ReactiveCommand<ProfileData, Unit> ProfileSelectedCommand { get; }
        public ReactiveCommand<ProfileData, Unit> SetProfileAsGlobalCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateProfileCommand { get; }
        public ReactiveCommand<ProfileData, Unit> DeleteProfileCommand { get; }

        // ---------------------------------------- Commands

        private void OnProfileSelected(ProfileData? obj)
        {
            Debug.WriteLine("Yup!");
            if (obj != null)
            {
                SettingsEditor.setNewSettingsVMGroupsContainer(obj.GetProfileVMGroupsContainer());
                SettingsEditor.AllowEditing = (obj == ProfileData.DefaultProfile) ? false : true;
            }
        }

        private void OnSetAsGlobalButtonPressed(ProfileData? obj)
        {
            if (obj != null)
            {
                TestViewModel.UserDataManager.GlobalProfile = obj;
            }
        }

        private void OnAddProfileButtonPressed()
        {
            TestViewModel.UserDataManager.CreateNewProfile("New profile");
            UpdateProfileList();
        }

        private void OnDeleteProfileButtonPressed(ProfileData? obj)
        {
            if (obj == null) return;
            TestViewModel.UserDataManager.DeleteProfile(obj);
            UpdateProfileList();
        }

        private void OnSaveButtonPressed()
        {
            SelectedProfile.GetProfileVMGroupsContainer().SaveAllChangesToBackingData(SelectedProfile.DataContainer);
            TestViewModel.UserDataManager.SaveProfileToDisk(SelectedProfile);
        }

        private void OnCancelButtonPressed()
        {
            SelectedProfile.GetProfileVMGroupsContainer().LoadDatasToAllGroups(SelectedProfile.DataContainer);
        }

    }

}