using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupOutRepControlVM : GroupSettingsVM
    {
        private BackingData_OutRepControl _tempBackingData = new();
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.OutputReportControl;
        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value);
            }
        }

        public bool IsOutputReportRateControlEnabled
        {
            get => _tempBackingData.IsOutputReportRateControlEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsOutputReportRateControlEnabled, value);
            }
        }
        public int MaxOutputRate
        {
            get => _tempBackingData.MaxOutputRate;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.MaxOutputRate, value);
            }
        }

        public bool IsOutputReportDeduplicatorEnabled
        {
            get => _tempBackingData.IsOutputReportDeduplicatorEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.IsOutputReportDeduplicatorEnabled, value);
            }
        }

        public GroupOutRepControlVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter) { }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.outRepData);
        }

        public void SaveSettingsToBackingData(BackingData_OutRepControl dataSource)
        {
            BackingData_OutRepControl.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.outRepData);
        }

        public void LoadSettingsFromBackingData(BackingData_OutRepControl dataTarget)
        {
            BackingData_OutRepControl.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }
    }


}
