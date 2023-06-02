using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleRightConversionAdjustsVM : GroupSettingsVM
    {
        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP

        public BackingData_VariablaRightRumbleEmulAdjusts _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleRightConversion;

        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled;
            set
            {
                _tempBackingData.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }
        public int RightRumbleConversionUpperRange
        {
            get => _tempBackingData.RightRumbleConversionUpperRange;
            set
            {
                _tempBackingData.RightRumbleConversionUpperRange = value;
                this.RaisePropertyChanged(nameof(RightRumbleConversionUpperRange));

            }
        }
        public int RightRumbleConversionLowerRange
        {
            get => _tempBackingData.RightRumbleConversionLowerRange;
            set
            {
                _tempBackingData.RightRumbleConversionLowerRange = value;
                this.RaisePropertyChanged(nameof(RightRumbleConversionLowerRange));
            }
        }
        public bool IsForcedRightMotorLightThresholdEnabled
        {
            get => _tempBackingData.IsForcedRightMotorLightThresholdEnabled;
            set
            {
                _tempBackingData.IsForcedRightMotorLightThresholdEnabled = value;
                this.RaisePropertyChanged(nameof(IsForcedRightMotorLightThresholdEnabled));
            }
        }
        public bool IsForcedRightMotorHeavyThreasholdEnabled
        {
            get => _tempBackingData.IsForcedRightMotorHeavyThreasholdEnabled;
            set
            {
                _tempBackingData.IsForcedRightMotorHeavyThreasholdEnabled = value;
                this.RaisePropertyChanged(nameof(IsForcedRightMotorHeavyThreasholdEnabled));
            }
        }
        public int ForcedRightMotorLightThreshold
        {
            get => _tempBackingData.ForcedRightMotorLightThreshold;
            set
            {
                _tempBackingData.ForcedRightMotorLightThreshold = value;
                this.RaisePropertyChanged(nameof(ForcedRightMotorLightThreshold));
            }
        }
        public int ForcedRightMotorHeavyThreshold
        {
            get => _tempBackingData.ForcedRightMotorHeavyThreshold;
            set
            {
                _tempBackingData.ForcedRightMotorHeavyThreshold = value;
                this.RaisePropertyChanged(nameof(ForcedRightMotorHeavyThreshold));
            }
        }

        public GroupRumbleRightConversionAdjustsVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter) { }
        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.rightVariableEmulData);
        }
        public void SaveSettingsToBackingData(BackingData_VariablaRightRumbleEmulAdjusts dataSource)
        {
            BackingData_VariablaRightRumbleEmulAdjusts.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.rightVariableEmulData);
        }

        public void LoadSettingsFromBackingData(BackingData_VariablaRightRumbleEmulAdjusts dataTarget)
        {
            BackingData_VariablaRightRumbleEmulAdjusts.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }
    }


}
