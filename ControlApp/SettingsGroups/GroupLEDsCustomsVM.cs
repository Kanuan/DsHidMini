using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
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
        public bool IsGroupEnabled { get => _tempBackingData.IsGroupEnabled; set => this.RaiseAndSetIfChanged(ref _tempBackingData.IsGroupEnabled, value); }
        public ControlApp_LEDsModes LEDMode { get => _tempBackingData.LEDMode; set => this.RaiseAndSetIfChanged(ref _tempBackingData.LEDMode, value); }
        public LEDsCustoms.singleLEDCustoms[] AllLEDsCustoms
        {
            get => _tempBackingData.LEDsCustoms.LED_x_Customs;
            set
            {
                this.RaiseAndSetIfChanged(ref _tempBackingData.LEDsCustoms.LED_x_Customs, value);
            }
        }

        [Reactive] public LEDsCustoms.singleLEDCustoms CurrentLEDCustoms { get; set; }
        public int CurrentLEDCustomsIndex
        {
            get => CurrentLEDCustoms.LEDIndex;
            set
            {
                CurrentLEDCustoms = AllLEDsCustoms[value];
                this.RaisePropertyChanged("CurrentLEDCustomsIndex");
            }
        }

        public GroupLEDsCustomsVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer, vmGroupsContainter)
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

        public class LEDsCustoms
        {
            public singleLEDCustoms[] LED_x_Customs = new singleLEDCustoms[4];

            public LEDsCustoms()
            {
                for(int i = 0; i < LED_x_Customs.Length; i++)
                {
                    LED_x_Customs[i] = new(i);
                }
            }

            public void CopyLEDsCustoms(LEDsCustoms customsToCopy)
            {
                for (int i = 0; i < LED_x_Customs.Length; i++)
                {
                    LED_x_Customs[i].CopyCustoms(customsToCopy.LED_x_Customs[i]);
                }
            }

            public void ResetLEDsCustoms()
            {
                for (int i = 0; i < LED_x_Customs.Length; i++)
                {
                    LED_x_Customs[i].Reset();
                }
            }



            public class singleLEDCustoms
            {
                private byte DEFAULT_duration = 0xFF;
                private byte DEFAULT_intervalDuration = 0xFF;
                private byte DEFAULT_intervalPortionON = 0xFF;
                private byte DEFAULT_intervalPortionOFF = 0x00;

                [Reactive] public bool IsLEDEnabled { get; set; }

                [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
                public int LEDIndex { get; }
                [Reactive] public byte Duration { get; set; }
                [Reactive] public byte IntervalDuration { get; set; }
                [Reactive] public byte IntervalPortionON { get; set; }
                public byte IntervalPortionOFF
                {
                    get => (byte)(256 - IntervalPortionON);
                }
                public singleLEDCustoms(int ledIndex)
                {
                    this.LEDIndex = ledIndex;
                    Reset();
                }

                internal void Reset()
                {
                    IsLEDEnabled = LEDIndex == 0 ? true : false;
                    Duration = DEFAULT_duration;
                    IntervalDuration = DEFAULT_intervalDuration;
                    IntervalPortionON = DEFAULT_intervalPortionON;
                    //IntervalPortionOFF = DEFAULT_intervalPortionOFF;
                }

                public void CopyCustoms(singleLEDCustoms copySource)
                {
                    this.IsLEDEnabled = copySource.IsLEDEnabled;
                    this.Duration = copySource.Duration;
                    this.IntervalDuration = copySource.IntervalDuration;
                    this.IntervalPortionON = copySource.IntervalPortionON;
                }
            }
        }

    }


}
