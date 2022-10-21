using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
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
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value);
            }
        }
        public int RightRumbleConversionUpperRange
        {
            get => _tempBackingData.RightRumbleConversionUpperRange;
            set
            {
                int tempInt = (value < _tempBackingData.RightRumbleConversionLowerRange) ? _tempBackingData.RightRumbleConversionLowerRange + 1 : value;
                this.RaiseAndSetIfChanged(ref _tempBackingData.RightRumbleConversionUpperRange, tempInt);

            }
        }
        public int RightRumbleConversionLowerRange
        {
            get => _tempBackingData.RightRumbleConversionLowerRange;
            set
            {
                int tempInt = (value > _tempBackingData.RightRumbleConversionUpperRange) ? (byte)(_tempBackingData.RightRumbleConversionUpperRange - 1) : value;
                this.RaiseAndSetIfChanged(ref _tempBackingData.RightRumbleConversionLowerRange, tempInt);
            }
        }
        public bool IsForcedRightMotorLightThresholdEnabled
        {
            get => _tempBackingData.IsForcedRightMotorLightThresholdEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsForcedRightMotorLightThresholdEnabled, value);
            }
        }
        public bool IsForcedRightMotorHeavyThreasholdEnabled
        {
            get => _tempBackingData.IsForcedRightMotorHeavyThreasholdEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsForcedRightMotorHeavyThreasholdEnabled, value);
            }
        }
        public int ForcedRightMotorLightThreshold
        {
            get => _tempBackingData.ForcedRightMotorLightThreshold;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.ForcedRightMotorLightThreshold, value);
            }
        }
        public int ForcedRightMotorHeavyThreshold
        {
            get => _tempBackingData.ForcedRightMotorHeavyThreshold;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.ForcedRightMotorHeavyThreshold, value);
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
