using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using DynamicData;

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
        public ObservableCollection<GroupSettingsVM> AdvancedSettingsGroupsList { get => _advancedSettings; set => SetProperty(ref _advancedSettings, value); }
        public ObservableCollection<GroupSettingsVM> ModeUniqueSettingsGroupsList { get => _modeUniqueSettings; set => SetProperty(ref _modeUniqueSettings, value); }
        public string TabName { get => _tabName; set => SetProperty(ref _tabName, value); }
        public bool IsTabSelected { get => isTabSelected; set => SetProperty(ref isTabSelected, value); }

        [Reactive] public bool AllowEditing { get; set; } = false;

        [Reactive] public GroupLEDsCustomsVM GroupLEDsControl { get; set; }
        [Reactive] GroupWirelessSettingsVM GroupWireless { get; set; }
        [Reactive] GroupSticksDeadzoneVM GroupSticksDZ { get; set; }
        [Reactive] GroupRumbleGeneralVM GroupRumbleGeneral { get; set; }
        [Reactive] GroupOutRepControlVM GroupOutRepControl { get; set;}
        [Reactive] GroupRumbleLeftRescaleVM GroupRumbleLeftRescale { get; set; }
        [Reactive] GroupRumbleRightConversionAdjustsVM GroupRumbleRightConversion { get; set; }

        public SettingTabViewModel(string tabName, SettingsContainer container, bool allowEditing)
        {
            SettingsContext context = container.Context;
            _tabName = tabName;
            AllowEditing = allowEditing;

            setNewSettingsContainer(container);

            /*
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
        */
            _basicSettings[0].PropertyChanged += SettingTabViewModel_PropertyChanged;
        }

        public void setNewSettingsContainer(SettingsContainer container)
        {
            BasicSettingsGroupsList = new ObservableCollection<GroupSettingsVM>
                    {
                        container.GroupLEDsControl,
                        container.GroupWireless,
                        container.GroupSticksDZ,
                        container.GroupRumbleGeneral,
                    };

            AdvancedSettingsGroupsList = new ObservableCollection<GroupSettingsVM>
                    {
                        container.GroupOutRepControl,
                        container.GroupRumbleLeftRescale,
                        container.GroupRumbleRightConversion,
                    };

            ModeUniqueSettingsGroupsList = new ObservableCollection<GroupSettingsVM>();

            if (container.Context == SettingsContext.SDF
                || container.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(container.GroupModeUnique);
            if (container.Context == SettingsContext.GPJ
                || container.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(container.GroupModeUnique);
            if (container.Context == SettingsContext.DS4W
                || container.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(container.GroupModeUnique);
            if (container.Context == SettingsContext.XInput
                || container.Context == SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(container.GroupModeUnique);
        }


        private void SettingTabViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(_basicSettings[2] is GroupSticksDeadzoneVM groupSticksDeadzoneVM)
            {
                //groupSticksDeadzoneVM.IsSettingLocked = !groupSticksDeadzoneVM.IsSettingLocked;
            }
        }
    }


}
