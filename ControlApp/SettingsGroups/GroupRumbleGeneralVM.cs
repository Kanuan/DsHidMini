using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleGeneralVM : GroupSettingsVM
    {
        private BackingData_RumbleGeneral _tempBackingData = new();
       
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleGeneral;
        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled; set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value);
            }
        }
        public bool IsVariableLightRumbleEmulationEnabled
        {
            get => _tempBackingData.IsVariableLightRumbleEmulationEnabled;
            set
            {
                if (value)
                {
                    IsLeftMotorDisabled = IsRightMotorDisabled = false;
                }
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsVariableLightRumbleEmulationEnabled, value);
            }
        }

        public bool IsVariableRightEmulToggleComboEnabled
        {
            get => _tempBackingData.IsVariableRightEmulToggleComboEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsVariableRightEmulToggleComboEnabled, value);

            }
        }

        public ButtonsCombo VariableRightEmulToggleCombo
        {
            get => _tempBackingData.VariableRightEmulToggleCombo;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.VariableRightEmulToggleCombo, value);
            }
        }

        public bool IsLeftMotorDisabled
        {
            get => _tempBackingData.IsLeftMotorDisabled;

            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsLeftMotorDisabled, value);
            }
        }
        public bool IsRightMotorDisabled
        {
            get => _tempBackingData.IsRightMotorDisabled;

            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsRightMotorDisabled, value);
            }
        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.rumbleGeneralData);
        }

        public void SaveSettingsToBackingData(BackingData_RumbleGeneral dataSource)
        {
            BackingData_RumbleGeneral.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.rumbleGeneralData);
        }

        public void LoadSettingsFromBackingData(BackingData_RumbleGeneral dataTarget)
        {
            BackingData_RumbleGeneral.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }

        public GroupRumbleGeneralVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter) { }
    }


}
