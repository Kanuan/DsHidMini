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

        private readonly ObservableAsPropertyHelper<VMGroupsContainer> selectedProfileVMGroups;

        // ----------------------------------------------------------- PROPERTIES

        [Reactive] public List<ProfileData> Profiles { get; set; }
        [Reactive] public ProfileData? SelectedProfile { get; set; } = null;

        public VMGroupsContainer? SelectedProfileVMGroups => selectedProfileVMGroups.Value;

        public readonly List<SettingsModes> settingsModesList = new List<SettingsModes>
        {
            SettingsModes.Global,
            SettingsModes.Profile,
            SettingsModes.Custom,
        };
        public List<SettingsModes> SettingsModesList => settingsModesList;

        public List<ProfileData> ListOfProfiles => TestViewModel.UserDataManager.Profiles;


        // ----------------------------------------------------------- CONSTRUCTOR

        public ProfileEditorViewModel()
        {
            Profiles = new List<ProfileData>(TestViewModel.UserDataManager.Profiles);

            selectedProfileVMGroups = this.
                WhenAnyValue(x => x.SelectedProfile)
                .Select(VMGroups => SelectedProfile != null ? SelectedProfile.GetProfileVMGroupsContainer() : null)
                .ToProperty(this, nameof(SelectedProfileVMGroups));

            this.
                WhenAnyValue(x => x.SelectedProfile)
                .Where(x => SelectedProfile != null)
                .Subscribe(x => LockProfileIfDefault());

            CreateProfileCommand = ReactiveCommand.Create(OnAddProfileButtonPressed);
            DeleteProfileCommand = ReactiveCommand.Create<ProfileData>(OnDeleteProfileButtonPressed);
            SetProfileAsGlobalCommand = ReactiveCommand.Create<ProfileData>(OnSetAsGlobalButtonPressed);
            SaveChangesCommand = ReactiveCommand.Create(OnSaveButtonPressed);
            CancelChangesCommand = ReactiveCommand.Create(OnCancelButtonPressed);
        }

        public void UpdateProfileList()
        {
            Profiles = new List<ProfileData>(TestViewModel.UserDataManager.Profiles);
        }

        public void LockProfileIfDefault()
        {
            SelectedProfileVMGroups.AllowEditing = (SelectedProfile == ProfileData.DefaultProfile) ? false : true;
        }


        // ---------------------------------------- ReactiveCommands

        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelChangesCommand { get; }
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
            SelectedProfileVMGroups.SaveAllChangesToBackingData(SelectedProfile.DataContainer);
            TestViewModel.UserDataManager.SaveProfileToDisk(SelectedProfile);
        }

        private void OnCancelButtonPressed()
        {
            SelectedProfileVMGroups.LoadDatasToAllGroups(SelectedProfile.DataContainer);
        }

    }

}