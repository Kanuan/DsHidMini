using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Text.Json.Serialization;
using System.Windows;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupLEDsCustomsVM : GroupSettingsVM
    {
        private BackingData_LEDs _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.LEDsControl;
        public bool IsGroupEnabled
        {
            get => _tempBackingData.IsGroupEnabled;
            set
            {
                _tempBackingData.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }
        public ControlApp_LEDsModes LEDMode
        {
            get => _tempBackingData.LEDMode;
            set
            {
                _tempBackingData.LEDMode = value;
                this.RaisePropertyChanged(nameof(LEDMode));
            }
        }

        public bool IsLedN1Enabled
        {
            get => _tempBackingData.LEDsCustoms.LED_x_Customs[0].IsLedEnabled;
            set
            {
                _tempBackingData.LEDsCustoms.LED_x_Customs[0].IsLedEnabled = value;
                this.RaisePropertyChanged(nameof(IsLedN1Enabled));
            }
        }
        public bool IsLedN2Enabled
        {
            get => _tempBackingData.LEDsCustoms.LED_x_Customs[1].IsLedEnabled;
            set
            {
                _tempBackingData.LEDsCustoms.LED_x_Customs[1].IsLedEnabled = value;
                this.RaisePropertyChanged(nameof(IsLedN1Enabled));
            }
        }
        public bool IsLedN3Enabled
        {
            get => _tempBackingData.LEDsCustoms.LED_x_Customs[2].IsLedEnabled;
            set
            {
                _tempBackingData.LEDsCustoms.LED_x_Customs[2].IsLedEnabled = value;
                this.RaisePropertyChanged(nameof(IsLedN1Enabled));
            }
        }
        public bool IsLedN4Enabled
        {
            get => _tempBackingData.LEDsCustoms.LED_x_Customs[3].IsLedEnabled;
            set
            {
                _tempBackingData.LEDsCustoms.LED_x_Customs[3].IsLedEnabled = value;
                this.RaisePropertyChanged(nameof(IsLedN1Enabled));
            }
        }

        [Reactive] public BackingData_LEDs.All4LEDsCustoms.singleLEDCustoms CurrentLEDCustoms { get; set; }

        public int CurrentLEDCustomsIndex
        {
            get => CurrentLEDCustoms.LEDIndex;
            set
            {
                CurrentLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[value];
                this.RaisePropertyChanged(nameof(CurrentLEDCustomsIndex));
            }
        }

        public GroupLEDsCustomsVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer)
        {
        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            CurrentLEDCustomsIndex = 0;
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.ledsData);
        }

        public void SaveSettingsToBackingData(BackingData_LEDs dataSource)
        {
            BackingData_LEDs.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.ledsData);
        }

        public void LoadSettingsFromBackingData(BackingData_LEDs dataTarget)
        {
            BackingData_LEDs.CopySettings(_tempBackingData, dataTarget);
            CurrentLEDCustomsIndex = 0;
            this.RaisePropertyChanged(string.Empty);
        }

    }


}
