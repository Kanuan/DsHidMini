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


        public SettingTabViewModel(string tabName, DeviceModesSettings modeSettings)
        {
            _tabName = tabName;
            _basicSettings = new ObservableCollection<GroupSettingsVM>
                    {
                        new GroupLEDsCustomsVM(modeSettings),
                        new GroupWirelessSettingsVM(modeSettings),
                        new GroupSticksDeadzoneVM(modeSettings),
                        new GroupRumbleGeneralVM(modeSettings),
                    };

            _advancedSettings = new ObservableCollection<GroupSettingsVM>
                    {
                        new GroupOutRepControlVM(modeSettings),
                        new GroupRumbleLeftRescaleVM(modeSettings),
                        new GroupRumbleRightConversionAdjustsVM(modeSettings),
                    };

            _modeUniqueSettings = new ObservableCollection<GroupSettingsVM>();

            if (modeSettings.Context == SettingsContext.SDF
                || modeSettings.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_SDF, modeSettings));
            if (modeSettings.Context == SettingsContext.GPJ
                || modeSettings.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_GPJ, modeSettings));
            if (modeSettings.Context == SettingsContext.DS4W
                || modeSettings.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_DS4W, modeSettings));
            if (modeSettings.Context == SettingsContext.XInput
                || modeSettings.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_XInput, modeSettings));
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
