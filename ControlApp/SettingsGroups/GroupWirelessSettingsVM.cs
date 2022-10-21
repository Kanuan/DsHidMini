using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupWirelessSettingsVM : GroupSettingsVM
    {
        private BackingData_Wireless _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.WirelessSettings;
        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled;
            set => this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value);
        }
        public bool IsWirelessIdleDisconnectEnabled
        {
            get => _tempBackingData.IsWirelessIdleDisconnectEnabled;
            set => this.RaiseAndSetIfChanged(ref _tempBackingData.IsWirelessIdleDisconnectEnabled, value);
        }
        public byte WirelessIdleDisconnectTime
        {
            get => _tempBackingData.WirelessIdleDisconnectTime;
            set => this.RaiseAndSetIfChanged(ref _tempBackingData.WirelessIdleDisconnectTime, value);
        }
        public bool IsQuickDisconnectComboEnabled
        {
            get => _tempBackingData.IsQuickDisconnectComboEnabled;
            set => this.RaiseAndSetIfChanged(ref _tempBackingData.IsQuickDisconnectComboEnabled, value);
        }
        public ButtonsCombo QuickDisconnectCombo
        {
            get => _tempBackingData.QuickDisconnectCombo;
            set => this.RaiseAndSetIfChanged(ref _tempBackingData.QuickDisconnectCombo, value);
        }

        public GroupWirelessSettingsVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter)
        {

        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.wirelessData);
        }

        public void SaveSettingsToBackingData(BackingData_Wireless dataSource)
        {
            BackingData_Wireless.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.wirelessData);
        }

        public void LoadSettingsFromBackingData(BackingData_Wireless dataTarget)
        {
            BackingData_Wireless.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }
    }


}
