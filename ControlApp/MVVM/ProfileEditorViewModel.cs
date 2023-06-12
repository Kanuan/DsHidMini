﻿using DynamicData.Binding;
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
        private ObservableCollection<SettingTabViewModel> _settingsTabs;
        private SettingTabViewModel _settingsEditor;
        [Reactive] private VMGroupsContainer DeviceCustomsVM { get; set; }
        [Reactive] private SettingTabViewModel DeviceCustomSettingsTab { get; set; } = new SettingTabViewModel("Custom", null, true);
        [Reactive] private SettingTabViewModel DeviceProfileSettingsTab { get; set; } = new SettingTabViewModel("Profile", null, false);

        private ObservableCollection<ProfileData> _profiles;
        [Reactive] public ObservableCollection<ProfileData> Profiles { get => _profiles; set => _profiles = value; }

        private ProfileData? _selectedProfile;
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


        public ProfileEditorViewModel()
        {
            SettingsEditor = new("wot", null, true);
            Profiles = new ObservableCollection<ProfileData>(TestViewModel.UserDataManager.Profiles);

            UpdateProfileList();

            CreateProfileCommand = ReactiveCommand.Create(OnAddProfileButtonPressed);
            DeleteProfileCommand = ReactiveCommand.Create<ProfileData>(OnDeleteProfileButtonPressed);
            ProfileSelectedCommand = ReactiveCommand.Create<ProfileData>(OnProfileSelected);
            SaveChangesCommand = ReactiveCommand.Create(OnSaveButtonPressed);
            CancelChangesCommand = ReactiveCommand.Create(OnCancelButtonPressed);
        }



        /// <summary>
        /// This should update on its own when changing SettingsMode.
        /// Initialized in the base constructor
        /// </summary>
        //readonly ObservableAsPropertyHelper<bool> isDeviceInProfileSettingsMode;
        //public bool IsDeviceInProfileSettingsMode { get => isDeviceInProfileSettingsMode.Value; }


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

        public void UpdateProfileList()
        {
            Profiles = new ObservableCollection<ProfileData>(TestViewModel.UserDataManager.Profiles);
        }


        // ---------------------------------------- ReactiveCommands

        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelChangesCommand { get; }
        public ReactiveCommand<ProfileData, Unit> ProfileSelectedCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateProfileCommand { get; }
        public ReactiveCommand<ProfileData, Unit> DeleteProfileCommand { get; }

        // ---------------------------------------- Commands

        private void OnProfileSelected(ProfileData? obj)
        {
            Debug.WriteLine("Yup!");
            if (obj != null)
            {
                SettingsEditor.setNewSettingsVMGroupsContainer(obj.GetProfileVMGroupsContainer());
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