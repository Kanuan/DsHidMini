using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Text.Json.Serialization;
using System.Windows;

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

        public override void CopySettingsFromBackingData(SettingsBackingData ledsData, bool invertCopyDirection = false)
        {
            base.CopySettingsFromBackingData(ledsData, invertCopyDirection);
            var specific = (BackingData_LEDs)ledsData;

            if(invertCopyDirection)
            {
                specific.IsGroupEnabled = this.IsGroupEnabled;
                specific.LEDMode = this.LEDMode;
                for (int i = 0; i < LEDsCustoms.Length; i++)
                {
                    specific.LEDsCustoms[i].CopyCustoms(this.LEDsCustoms[i]);
                }
            }
            else
            {
                this.IsGroupEnabled = specific.IsGroupEnabled;
                this.LEDMode = specific.LEDMode;
                for (int i = 0; i < LEDsCustoms.Length; i++)
                {
                    this.LEDsCustoms[i].CopyCustoms(specific.LEDsCustoms[i]);
                }
                this.CurrentLEDCustoms = this.LEDsCustoms[0];
            }

        }

        public class LEDCustoms
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
                //IntervalPortionOFF = DEFAULT_intervalPortionOFF;
            }

            public void CopyCustoms(LEDCustoms copySource)
            {
                this.IsLEDEnabled = copySource.IsLEDEnabled;
                this.Duration = copySource.Duration;
                this.IntervalDuration = copySource.IntervalDuration;
                this.IntervalPortionON = copySource.IntervalPortionON;
            }
        }

        /*
        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.LEDSettings;

            if (dshm_AllLEDsSettings.Mode == null)
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
                    this.LEDsCustoms[i].IsLEDEnabled = dshm_singleLED[i].Enabled.GetValueOrDefault() == 0x10 ? true : false;
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
        */
    }


}
