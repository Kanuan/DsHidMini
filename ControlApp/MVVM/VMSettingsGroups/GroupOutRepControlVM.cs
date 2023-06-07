using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
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
        public bool IsOutputReportRateControlEnabled
        {
            get => _tempBackingData.IsOutputReportRateControlEnabled;
            set
            {
                _tempBackingData.IsOutputReportRateControlEnabled = value;
                this.RaisePropertyChanged(nameof(IsOutputReportRateControlEnabled));
            }
        }

        public int MaxOutputRate
        {
            get => _tempBackingData.MaxOutputRate;
            set
            {
                _tempBackingData.MaxOutputRate = value;
                this.RaisePropertyChanged(nameof(MaxOutputRate));
            }
        }

        public bool IsOutputReportDeduplicatorEnabled
        {
            get => _tempBackingData.IsOutputReportDeduplicatorEnabled;
            set
            {
                _tempBackingData.IsOutputReportDeduplicatorEnabled = value;
                this.RaisePropertyChanged(nameof(IsOutputReportDeduplicatorEnabled));
            }
        }

        public GroupOutRepControlVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer) { }

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
