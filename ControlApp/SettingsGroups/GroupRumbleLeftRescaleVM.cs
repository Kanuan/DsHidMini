using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleLeftRescaleVM : GroupSettingsVM
    {
        private BackingData_LeftRumbleRescale _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleLeftStrRescale;

        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value);
            }
        }

        public bool IsLeftMotorStrRescalingEnabled
        {
            get => _tempBackingData.IsLeftMotorStrRescalingEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsLeftMotorStrRescalingEnabled, value);
            }
        }
        public int LeftMotorStrRescalingUpperRange
        {
            get => _tempBackingData.LeftMotorStrRescalingUpperRange;
            set
            {
                int tempInt = (value < _tempBackingData.LeftMotorStrRescalingLowerRange) ? _tempBackingData.LeftMotorStrRescalingLowerRange + 1 : value;
                this.RaiseAndSetIfChanged(ref _tempBackingData.LeftMotorStrRescalingUpperRange, tempInt);
            }
        }
        public int LeftMotorStrRescalingLowerRange
        {
            get => _tempBackingData.LeftMotorStrRescalingLowerRange;
            set
            {
                int tempInt = (value > _tempBackingData.LeftMotorStrRescalingUpperRange) ? _tempBackingData.LeftMotorStrRescalingUpperRange - 1 : value;
                this.RaiseAndSetIfChanged(ref _tempBackingData.LeftMotorStrRescalingLowerRange, tempInt);
            }
        }

        public GroupRumbleLeftRescaleVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter) { }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.leftRumbleRescaleData);
        }

        public void SaveSettingsToBackingData(BackingData_LeftRumbleRescale dataSource)
        {
            BackingData_LeftRumbleRescale.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.leftRumbleRescaleData);
        }

        public void LoadSettingsFromBackingData(BackingData_LeftRumbleRescale dataTarget)
        {
            BackingData_LeftRumbleRescale.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }
    }


}
