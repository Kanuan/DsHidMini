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
        private ObservableCollection<GroupSettingsVM> _basicSettings;
        private ObservableCollection<GroupSettingsVM> _advancedSettings;
        private ObservableCollection<GroupSettingsVM> _modeUniqueSettings;
        private string _tabName;

        public ObservableCollection<GroupSettingsVM> BasicSettingsGroupsList
        {
            get
            {
                return _basicSettings;
            }
            set
            {
                SetProperty(ref _basicSettings, value);
            }
        }

        public ObservableCollection<GroupSettingsVM> AdvancedSettingsGroupsList
        {
            get
            {
                return _advancedSettings;
            }
            set
            {
                SetProperty(ref _advancedSettings, value);
            }
        }

        public ObservableCollection<GroupSettingsVM> ModeUniqueSettingsGroupsList
        {
            get
            {
                return _modeUniqueSettings;
            }
            set
            {
                SetProperty(ref _modeUniqueSettings, value);
            }
        }

        public string TabName
        {
            get
            {
                return _tabName;
            }
            set
            {
                SetProperty(ref _tabName, value);
            }
        }

        public SettingTabViewModel(string tabName, DeviceModesSettings modeSettings)
        {
            _tabName = tabName;
            _basicSettings = new ObservableCollection<GroupSettingsVM>
                    {
                        new GroupSettingsVM(SettingsModeGroups.LEDsControl, modeSettings),
                        new GroupSettingsVM(SettingsModeGroups.WirelessSettings, modeSettings),
                        new GroupSettingsVM(SettingsModeGroups.SticksDeadzone, modeSettings),
                        new GroupSettingsVM(SettingsModeGroups.RumbleBasicFunctions, modeSettings),
                    };

            _advancedSettings = new ObservableCollection<GroupSettingsVM>
                    {
                        new GroupSettingsVM(SettingsModeGroups.OutputReportControl, modeSettings),
                        new GroupSettingsVM(SettingsModeGroups.RumbleHeavyStrRescale, modeSettings),
                        new GroupSettingsVM(SettingsModeGroups.RumbleLightConversion, modeSettings),
                    };
            _modeUniqueSettings = new ObservableCollection<GroupSettingsVM>();


            if (modeSettings.currentSettingContext == SettingsContext.SDF
                || modeSettings.currentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_SDF, modeSettings));
            if (modeSettings.currentSettingContext == SettingsContext.GPJ
                || modeSettings.currentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_GPJ, modeSettings));
            if (modeSettings.currentSettingContext == SettingsContext.DS4W
                || modeSettings.currentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_DS4W, modeSettings));
            if (modeSettings.currentSettingContext == SettingsContext.XInput
                || modeSettings.currentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_XInput, modeSettings));
        }
    }


}
