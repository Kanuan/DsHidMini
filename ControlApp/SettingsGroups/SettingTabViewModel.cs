using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    internal class SettingTabViewModel : ObservableObject
    {
        private bool isTabSelected = false;
        private ObservableCollection<GroupSettingsVM> _basicSettings;
        private ObservableCollection<GroupSettingsVM> _advancedSettings;
        private ObservableCollection<GroupSettingsVM> _modeUniqueSettings;
        private string _tabName;

        public ObservableCollection<GroupSettingsVM> BasicSettingsGroupsList { get => _basicSettings; set => SetProperty(ref _basicSettings, value); }
        public ObservableCollection<GroupSettingsVM> AdvancedSettingsGroupsList {  get => _advancedSettings; set => SetProperty(ref _advancedSettings, value); }
        public ObservableCollection<GroupSettingsVM> ModeUniqueSettingsGroupsList {  get => _modeUniqueSettings; set => SetProperty(ref _modeUniqueSettings, value); }
        public string TabName { get => _tabName; set => SetProperty(ref _tabName, value); }
        public bool IsTabSelected { get => isTabSelected; set => SetProperty(ref isTabSelected, value); }


        public SettingTabViewModel(string tabName, SettingsContainer settingsContainer)
        {
            _tabName = tabName;
            _basicSettings = new ObservableCollection<GroupSettingsVM>
                    {
                        new GroupLEDsCustomsVM(settingsContainer),
                        new GroupWirelessSettingsVM(settingsContainer),
                        new GroupSticksDeadzoneVM(settingsContainer),
                        new GroupRumbleGeneralVM(settingsContainer),
                    };

            _advancedSettings = new ObservableCollection<GroupSettingsVM>
                    {
                        new GroupOutRepControlVM(settingsContainer),
                        new GroupRumbleLeftRescaleVM(settingsContainer),
                        new GroupRumbleRightConversionAdjustsVM(settingsContainer),
                    };

            _modeUniqueSettings = new ObservableCollection<GroupSettingsVM>();

            if (settingsContainer.ModeContext == SettingsModeContext.SDF
                || settingsContainer.ModeContext == SettingsModeContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_SDF, settingsContainer));
            if (settingsContainer.ModeContext == SettingsModeContext.GPJ
                || settingsContainer.ModeContext == SettingsModeContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_GPJ, settingsContainer));
            if (settingsContainer.ModeContext == SettingsModeContext.DS4W
                || settingsContainer.ModeContext == SettingsModeContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_DS4W, settingsContainer));
            if (settingsContainer.ModeContext == SettingsModeContext.XInput
                || settingsContainer.ModeContext == SettingsModeContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_XInput, settingsContainer));
        }

        private void SettingTabViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(_basicSettings[2] is GroupSticksDeadzoneVM groupSticksDeadzoneVM)
            {
                groupSticksDeadzoneVM.IsSettingLocked = !groupSticksDeadzoneVM.IsSettingLocked;
            }
        }
    }


}
