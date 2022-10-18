using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupLEDsCustomsVM : GroupSettingsVM
    {
        public const ControlApp_LEDsModes DEFAULT_ledMode = ControlApp_LEDsModes.BatteryIndicatorPlayerIndex;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.LEDsControl;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public ControlApp_LEDsModes LEDMode { get; set; }
        [Reactive]
        public LEDCustoms[] LEDsCustoms { get; set; } =
            {
            new LEDCustoms(0), new LEDCustoms(1), new LEDCustoms(2), new LEDCustoms(3),
            };
        [Reactive] public LEDCustoms CurrentLEDCustoms { get; set; }
        public int CurrentLEDCustomsIndex
        {
            get => CurrentLEDCustoms.LEDIndex;
            set
            {
                CurrentLEDCustoms = LEDsCustoms[value];
                this.RaisePropertyChanged("CurrentLEDCustomsIndex");
            }

        }

        public GroupLEDsCustomsVM(SettingsContext context, SettingsContainer containter) : base(context, containter)
        {

        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            LEDMode = DEFAULT_ledMode;
            foreach (LEDCustoms led in LEDsCustoms)
            {
                led.Reset();
            }
            CurrentLEDCustoms = LEDsCustoms[0];
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.LEDSettings;
            if (!this.IsGroupEnabled)
            {
                dshm_AllLEDsSettings = null;
                return;
            }

            dshm_AllLEDsSettings.Mode = SaveLoadUtils.Get_DSHM_LEDModes_From_ControlApp[this.LEDMode];

            var dshm_singleLED = new DSHM_Format_ContextSettings.SingleLEDCustoms[]
            { dshm_AllLEDsSettings.Player1, dshm_AllLEDsSettings.Player2,dshm_AllLEDsSettings.Player3,dshm_AllLEDsSettings.Player4, };

            for (int i = 0; i < 4; i++)
            {
                switch (this.LEDMode)
                {
                    case ControlApp_LEDsModes.CustomPattern:
                        dshm_singleLED[i].Enabled = this.LEDsCustoms[i].IsLEDEnabled ? (byte)0x10 : (byte)0x00;
                        dshm_singleLED[i].Duration = this.LEDsCustoms[i].Duration;
                        dshm_singleLED[i].IntervalDuration = this.LEDsCustoms[i].IntervalDuration;
                        dshm_singleLED[i].IntervalPortionOn = this.LEDsCustoms[i].IntervalPortionON;
                        dshm_singleLED[i].IntervalPortionOff = this.LEDsCustoms[i].IntervalPortionOFF;
                        break;
                    case ControlApp_LEDsModes.CustomStatic:
                        dshm_singleLED[i].Enabled = this.LEDsCustoms[i].IsLEDEnabled ? (byte)0x10 : (byte)0x00;
                        dshm_singleLED[i].Duration = null;
                        dshm_singleLED[i].IntervalDuration = null;
                        dshm_singleLED[i].IntervalPortionOn = null;
                        dshm_singleLED[i].IntervalPortionOff = null;
                        break;
                    case ControlApp_LEDsModes.BatteryIndicatorPlayerIndex:
                    case ControlApp_LEDsModes.BatteryIndicatorBarGraph:
                    default:
                        dshm_singleLED[i] = null;
                        break;
                }
            }
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.LEDSettings;

            if(dshm_AllLEDsSettings.Mode == null)
            {
                this.IsGroupEnabled = false;
                return;
            }
            this.IsGroupEnabled = true;

            this.LEDMode = SaveLoadUtils.Get_ControlApp_LEDModes_From_DSHM[dshm_AllLEDsSettings.Mode.GetValueOrDefault()];
            var IsItActuallyStaticMode = true;
            var dshm_singleLED = new DSHM_Format_ContextSettings.SingleLEDCustoms[]
            { dshm_AllLEDsSettings.Player1, dshm_AllLEDsSettings.Player2,dshm_AllLEDsSettings.Player3,dshm_AllLEDsSettings.Player4, };

            for (int i = 0; i < 4; i++)
            {
                if (this.LEDMode == ControlApp_LEDsModes.CustomPattern)
                {
                    this.LEDsCustoms[i].IsLEDEnabled = dshm_singleLED[i].Enabled.GetValueOrDefault() == 0x10 ? true : false ;
                    this.LEDsCustoms[i].Duration = dshm_singleLED[i].Duration.GetValueOrDefault();
                    this.LEDsCustoms[i].IntervalDuration = dshm_singleLED[i].IntervalDuration.GetValueOrDefault();
                    this.LEDsCustoms[i].IntervalPortionON = dshm_singleLED[i].IntervalPortionOn.GetValueOrDefault();
                    this.LEDsCustoms[i].IntervalPortionOFF = dshm_singleLED[i].IntervalPortionOff.GetValueOrDefault();
                }

                // Attempts to check differentiate between custom and static mode
                if (dshm_singleLED[i].Duration != 255) IsItActuallyStaticMode = false;
                if (dshm_singleLED[i].IntervalDuration != 255) IsItActuallyStaticMode = false;
                if (dshm_singleLED[i].IntervalPortionOn != 255) IsItActuallyStaticMode = false;
                if (dshm_singleLED[i].IntervalPortionOff != 0) IsItActuallyStaticMode = false;
            }
            if (IsItActuallyStaticMode) this.LEDMode = ControlApp_LEDsModes.CustomStatic;
        }

        public class LEDCustoms
        {
            private byte DEFAULT_duration = 0xFF;
            private byte DEFAULT_intervalDuration = 0xFF;
            private byte DEFAULT_intervalPortionON = 0xFF;
            private byte DEFAULT_intervalPortionOFF = 0x00;

            [Reactive] public bool IsLEDEnabled { get; set; }

            [Reactive] public int LEDIndex { get; set; }
            [Reactive] public byte Duration { get; set; }
            [Reactive] public byte IntervalDuration { get; set; }
            [Reactive] public byte IntervalPortionON { get; set; }
            [Reactive] public byte IntervalPortionOFF { get; set; }
            public LEDCustoms(int ledIndex)
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
                IntervalPortionOFF = DEFAULT_intervalPortionOFF;
            }
        }


    }


}
