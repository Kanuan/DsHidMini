using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
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
            set
            {
                _tempBackingData.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }
        public bool IsWirelessIdleDisconnectEnabled
        {
            get => _tempBackingData.IsWirelessIdleDisconnectEnabled;
            set
            {
                _tempBackingData.IsWirelessIdleDisconnectEnabled = value;
                this.RaisePropertyChanged(nameof(IsWirelessIdleDisconnectEnabled));
            }
        }
        public int WirelessIdleDisconnectTime
        {
            get => _tempBackingData.WirelessIdleDisconnectTime;
            set
            {
                _tempBackingData.WirelessIdleDisconnectTime = value;
                this.RaisePropertyChanged(nameof(WirelessIdleDisconnectTime));
            }
        }
        public bool IsQuickDisconnectComboEnabled
        {
            get => _tempBackingData.IsQuickDisconnectComboEnabled;
            set
            {
                _tempBackingData.IsQuickDisconnectComboEnabled = value;
                this.RaisePropertyChanged(nameof(IsQuickDisconnectComboEnabled));
            }
        }

        public ControlApp_ComboButtons QuickDisconnectComboButton1
        {
            get => _tempBackingData.QuickDisconnectCombo.Button1;
            set
            {
                _tempBackingData.QuickDisconnectCombo.Button1 = value;
                this.RaisePropertyChanged(nameof(QuickDisconnectComboButton1));
            }
        }

        public ControlApp_ComboButtons QuickDisconnectComboButton2
        {
            get => _tempBackingData.QuickDisconnectCombo.Button2;
            set
            {
                _tempBackingData.QuickDisconnectCombo.Button2 = value;
                this.RaisePropertyChanged(nameof(QuickDisconnectComboButton2));
            }
        }

        public ControlApp_ComboButtons QuickDisconnectComboButton3
        {
            get => _tempBackingData.QuickDisconnectCombo.Button3;
            set
            {
                _tempBackingData.QuickDisconnectCombo.Button3 = value;
                this.RaisePropertyChanged(nameof(QuickDisconnectComboButton3));
            }
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

        public void SaveSettingsToBackingData(BackingData_Wireless dataTarget)
        {
            BackingData_Wireless.CopySettings(dataTarget, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.wirelessData);
        }

        public void LoadSettingsFromBackingData(BackingData_Wireless dataSource)
        {
            BackingData_Wireless.CopySettings(_tempBackingData, dataSource);
            this.RaisePropertyChanged(string.Empty);
        }
    }


}
