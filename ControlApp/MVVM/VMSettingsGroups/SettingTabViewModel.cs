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
    internal class SettingTabViewModel : ReactiveObject
    {
        private bool isTabSelected = false;
        private string _tabName;

        private ObservableCollection<GroupSettingsVM> _basicSettings = new();
        private ObservableCollection<GroupSettingsVM> _advancedSettings = new();
        private ObservableCollection<GroupSettingsVM> _modeUniqueSettings = new();

        public ObservableCollection<GroupSettingsVM> BasicSettingsGroupsList { get => _basicSettings; set => this.RaiseAndSetIfChanged(ref _basicSettings, value); }
        public ObservableCollection<GroupSettingsVM> AdvancedSettingsGroupsList { get => _advancedSettings; set => this.RaiseAndSetIfChanged(ref _advancedSettings, value); }
        public ObservableCollection<GroupSettingsVM> ModeUniqueSettingsGroupsList { get => _modeUniqueSettings; set => this.RaiseAndSetIfChanged(ref _modeUniqueSettings, value); }
        public string TabName { get => _tabName; set => this.RaiseAndSetIfChanged(ref _tabName, value); }
        public bool IsTabSelected { get => isTabSelected; set => this.RaiseAndSetIfChanged(ref isTabSelected, value); }
        [Reactive] public bool AllowEditing { get; set; } = false;

        public SettingTabViewModel(string tabName, VMGroupsContainer? container, bool allowEditing)
        {
            _tabName = tabName;
            AllowEditing = allowEditing;

            if(container != null)
                setNewSettingsVMGroupsContainer(container);
        }

        public void setNewSettingsVMGroupsContainer(VMGroupsContainer container)
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

            if (container.GroupModeUnique.Context != SettingsContext.General
                && container.GroupModeUnique.Context != SettingsContext.Global)
                ModeUniqueSettingsGroupsList.Add(container.GroupModeUnique);
        }
    }


}
