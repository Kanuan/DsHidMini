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

            if (modeSettings.CurrentSettingContext == SettingsContext.SDF
                || modeSettings.CurrentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_SDF, modeSettings));
            if (modeSettings.CurrentSettingContext == SettingsContext.GPJ
                || modeSettings.CurrentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_GPJ, modeSettings));
            if (modeSettings.CurrentSettingContext == SettingsContext.DS4W
                || modeSettings.CurrentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_DS4W, modeSettings));
            if (modeSettings.CurrentSettingContext == SettingsContext.XInput
                || modeSettings.CurrentSettingContext == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(new GroupSettingsVM(SettingsModeGroups.Unique_XInput, modeSettings));
        }
    }


}
