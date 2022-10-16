using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.MVVM;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.SettingsContainer.GroupSettings
{
    public abstract class GroupSettings : ReactiveObject
    {
        // DEFAULT VALUES ----------------------------------------------------- START

        public const ControlApp_LEDsModes DEFAULT_ledMode = ControlApp_LEDsModes.BatteryIndicatorPlayerIndex;
        public bool DEFAULT_isWirelessIdleDisconnectDisabled = false;
        public double DEFAULT_wirelessIdleDisconnectTime = 300000;
        public readonly ButtonsCombo DEFAULT_disconnectCombo = new()
        {
            Button1 = ControlApp_ComboButtons.PS,
            Button2 = ControlApp_ComboButtons.R1,
            Button3 = ControlApp_ComboButtons.L1,
        };
        public const byte DEFAULT_disconnectComboHoldTime = 3;

        public const bool DEFAULT_applyLeftStickDeadzone = true;
        public const bool DEFAULT_applyRightStickDeadzone = true;
        public const int DEFAULT_leftStickDeadzone = 0;
        public const int DEFAULT_rightStickDeadzone = 0;

        public const bool DEFAULT_isVariableLightRumbleEmulationEnabled = false;
        public const byte DEFAULT_rightRumbleConversionUpperRange = 140;
        public const byte DEFAULT_rightRumbleConversionLowerRange = 1;
        public const bool DEFAULT_isForcedRightMotorLightThresholdEnabled = false;
        public const bool DEFAULT_isForcedRightMotorHeavyThreasholdEnabled = false;
        public const byte DEFAULT_forcedRightMotorLightThreshold = 230;
        public const byte DEFAULT_forcedRightMotorHeavyThreshold = 230;

        public const bool DEFAULT_isLeftMotorDisabled = false;
        public const bool DEFAULT_isRightMotorDisabled = false;
        public const bool DEFAULT_isOutputReportRateControlEnabled = true;
        public const byte DEFAULT_maxOutputRate = 150;
        public const bool DEFAULT_isOutputReportDeduplicatorEnabled = false;

        public const bool DEFAULT_isLeftMotorStrRescalingEnabled = true;
        public const byte DEFAULT_leftMotorStrRescalingUpperRange = 255;
        public const byte DEFAULT_leftMotorStrRescalingLowerRange = 64;

        public const ControlApp_DsPressureMode DEFAULT_pressureExposureMode = ControlApp_DsPressureMode.Both;
        public const ControlApp_DPADModes DEFAULT_padExposureMode = ControlApp_DPADModes.HAT;
        public const bool DEFAULT_isLEDsAsXInputSlotEnabled = false;
        public const bool DEFAULT_isDS4LightbarTranslationEnabled = false;


        // DEFAULT VALUES ----------------------------------------------------- END

        abstract public SettingsModeGroups Group { get; }
        [Reactive] public SettingsModeContext Context { get; internal set; }

        public class ButtonsCombo
        {
            [Reactive] public ControlApp_ComboButtons Button1 { get; set; }
            [Reactive] public ControlApp_ComboButtons Button2 { get; set; }
            [Reactive] public ControlApp_ComboButtons Button3 { get; set; }

        }

        public GroupSettings(SettingsModeContext context)
        {
            Context = context;
            ResetGroupToOriginalDefaults();
        }

        public bool ShouldGroupBeEnabledOnReset()
        {
            return (Context == SettingsModeContext.General || Context == SettingsModeContext.Global);
        }
        public abstract void ResetGroupToOriginalDefaults();

        public abstract void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings);

        public abstract void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings);
    }


    public class GroupLEDsControl : GroupSettings
    {
        // -------------------------------------------- LEDS GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.LEDsControl;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public ControlApp_LEDsModes LEDMode { get; set; }
        [Reactive] public LEDCustoms[] LEDsCustoms { get; set; } =
            {
            new LEDCustoms(0), new LEDCustoms(1), new LEDCustoms(2), new LEDCustoms(3),
            };

        public GroupLEDsControl(SettingsModeContext context) : base(context)
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
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.LEDSettings;
            if (!this.IsGroupEnabled)
            {
                dshm_AllLEDsSettings = null;
                return;
            }

            dshm_AllLEDsSettings.Mode = SaveLoadUtils.LEDModes_Control_DSHM_Pair[this.LEDMode];

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
            throw new NotImplementedException();
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

    public class GroupWireless : GroupSettings
    {
        // -------------------------------------------- WIRELESS SETTINGS GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.WirelessSettings;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public bool IsWirelessIdleDisconnectDisabled { get; set; }
        [Reactive] public double WirelessIdleDisconnectTime { get; set; }
        [Reactive] public ButtonsCombo QuickDisconnectCombo { get; set; }


        public GroupWireless(SettingsModeContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();
            IsWirelessIdleDisconnectDisabled = DEFAULT_isWirelessIdleDisconnectDisabled;
            WirelessIdleDisconnectTime = DEFAULT_wirelessIdleDisconnectTime;
            QuickDisconnectCombo = new()
            {
                Button1 = ControlApp_ComboButtons.PS,
                Button2 = ControlApp_ComboButtons.L1,
                Button3 = ControlApp_ComboButtons.R1,
            };
            //DisconnectComboHoldTime = DEFAULT_disconnectComboHoldTime;

        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {

            if (!this.IsGroupEnabled)
            {
                dshmContextSettings.DisableWirelessIdleTimeout = null;
                dshmContextSettings.WirelessIdleTimeoutPeriodMs = null;
                //dshmContextSettings.QuickDisconnectCombo = null;
                return;
            }

            dshmContextSettings.DisableWirelessIdleTimeout = !this.IsWirelessIdleDisconnectDisabled;
            dshmContextSettings.WirelessIdleTimeoutPeriodMs = this.WirelessIdleDisconnectTime * 60 * 1000;
            //dshmContextSettings.QuickDisconnectCombo = dictionary combo pair;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupSticksDeadZone : GroupSettings
    {
        // -------------------------------------------- STICKS DEADZONE GROUP
        private bool isGroupEnabled;
        private bool applyLeftStickDeadZone;
        private bool applyRightStickDeadZone;


        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;
        public bool IsGroupEnabled
        {
            get => isGroupEnabled;
            set
            {
                isGroupEnabled = Context == SettingsModeContext.DS4W ? true : value;
                this.RaisePropertyChanged();
            }
        }
        public bool ApplyLeftStickDeadZone
        {
            get => applyLeftStickDeadZone;
            set
            {
                applyLeftStickDeadZone = Context == SettingsModeContext.DS4W ? false : value;
                this.RaisePropertyChanged();
            }
        }
        public bool ApplyRightStickDeadZone
        {
            get => applyRightStickDeadZone;
            set
            {
                applyRightStickDeadZone = Context == SettingsModeContext.DS4W ? false : value;
                this.RaisePropertyChanged();
            }
        }
        [Reactive] public byte LeftStickDeadZone { get; set; } // in %
        [Reactive] public byte RightStickDeadZone { get; set; } // in %

        public GroupSticksDeadZone(SettingsModeContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            ApplyRightStickDeadZone = DEFAULT_applyRightStickDeadzone;
            ApplyLeftStickDeadZone = DEFAULT_applyLeftStickDeadzone;
            LeftStickDeadZone = DEFAULT_leftStickDeadzone;
            RightStickDeadZone = DEFAULT_rightStickDeadzone;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
            DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;

            if (!this.IsGroupEnabled)
            {
                dshmLeftDZSettings = null;
                dshmRightDZSettings = null;
                return;
            }

            dshmLeftDZSettings.Apply = this.ApplyLeftStickDeadZone;
            dshmLeftDZSettings.PolarValue = (this.LeftStickDeadZone * 180) /141; // Necessary do conversion

            dshmRightDZSettings.Apply = this.ApplyRightStickDeadZone;
            dshmRightDZSettings.PolarValue = (this.RightStickDeadZone * 180) / 141; // Necessary do conversion
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupRumbleGeneral : GroupSettings
    {
        // --------------------------------------------  GENERAL RUMBLE SETTINGS 

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleGeneral;

        private bool isVariableLightRumbleEmulationEnabled;
        private bool isLeftMotorDisabled;
        private bool isRightMotorDisabled;
        [Reactive] public bool IsGroupEnabled { get; set; }
        public bool IsVariableLightRumbleEmulationEnabled
        {
            get => isVariableLightRumbleEmulationEnabled;
            set
            {
                if (value)
                {
                    IsLeftMotorDisabled = IsRightMotorDisabled = false;
                }
                this.RaiseAndSetIfChanged(ref isVariableLightRumbleEmulationEnabled, value);
            }
        }

        public bool IsLeftMotorDisabled
        {
            get => isLeftMotorDisabled;
            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                this.RaiseAndSetIfChanged(ref isLeftMotorDisabled, value);
            }
        }
        public bool IsRightMotorDisabled
        {
            get => isRightMotorDisabled;
            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                this.RaiseAndSetIfChanged(ref isRightMotorDisabled, value);
            }
        }
        public GroupRumbleGeneral(SettingsModeContext context) : base(context) { }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsVariableLightRumbleEmulationEnabled = DEFAULT_isVariableLightRumbleEmulationEnabled;
            IsLeftMotorDisabled = DEFAULT_isLeftMotorDisabled;
            IsRightMotorDisabled = DEFAULT_isRightMotorDisabled;

        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllRumbleSettings dshmRumbleSettings = dshmContextSettings.RumbleSettings;

            if (!this.IsGroupEnabled)
            {
                dshmRumbleSettings.SMToBMConversion.Enabled = null;
                dshmRumbleSettings.DisableBM = null;
                dshmRumbleSettings.DisableSM = null;
                return;
            }

            dshmRumbleSettings.SMToBMConversion.Enabled = this.IsVariableLightRumbleEmulationEnabled;
            dshmRumbleSettings.DisableBM = this.IsLeftMotorDisabled;
            dshmRumbleSettings.DisableSM = this.IsLeftMotorDisabled;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupOutRepControl : GroupSettings
    {
        // --------------------------------------------  OUTPUT REPORT CONTROL GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.OutputReportControl;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public bool IsOutputReportRateControlEnabled { get; set; }
        [Reactive] public byte MaxOutputRate { get; set; }
        [Reactive] public bool IsOutputReportDeduplicatorEnabled { get; set; }

        public GroupOutRepControl(SettingsModeContext context) : base(context) { }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsOutputReportRateControlEnabled = DEFAULT_isOutputReportRateControlEnabled;
            MaxOutputRate = DEFAULT_maxOutputRate;
            IsOutputReportDeduplicatorEnabled = DEFAULT_isOutputReportDeduplicatorEnabled;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            if (!this.IsGroupEnabled)
            {
                dshmContextSettings.IsOutputRateControlEnabled = null;
                dshmContextSettings.OutputRateControlPeriodMs = null;
                dshmContextSettings.IsOutputDeduplicatorEnabled = null;
                return;
            }
            dshmContextSettings.IsOutputRateControlEnabled = this.IsOutputReportRateControlEnabled;
            dshmContextSettings.OutputRateControlPeriodMs = this.MaxOutputRate;
            dshmContextSettings.IsOutputDeduplicatorEnabled = this.IsOutputReportDeduplicatorEnabled;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupRumbleLeftRescale : GroupSettings
    {
        private byte leftMotorStrRescalingUpperRange;
        private byte leftMotorStrRescalingLowerRange;

        // -------------------------------------------- LEFT MOTOR RESCALING GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleLeftStrRescale;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public bool IsLeftMotorStrRescalingEnabled { get; set; }
        public byte LeftMotorStrRescalingUpperRange
        {
            get => leftMotorStrRescalingUpperRange;
            set
            {
                byte tempByte = (value < leftMotorStrRescalingLowerRange) ? (byte)(leftMotorStrRescalingLowerRange + 1) : value;
                this.RaiseAndSetIfChanged(ref leftMotorStrRescalingUpperRange, tempByte);
            }
        }
        public byte LeftMotorStrRescalingLowerRange
        {
            get => leftMotorStrRescalingLowerRange;
            set
            {
                byte tempByte = (value > leftMotorStrRescalingUpperRange) ? (byte)(leftMotorStrRescalingUpperRange - 1) : value;
                this.RaiseAndSetIfChanged(ref leftMotorStrRescalingLowerRange, tempByte);
            }
        }

        public GroupRumbleLeftRescale(SettingsModeContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsLeftMotorStrRescalingEnabled = DEFAULT_isLeftMotorStrRescalingEnabled;
            LeftMotorStrRescalingUpperRange = DEFAULT_leftMotorStrRescalingUpperRange;
            LeftMotorStrRescalingLowerRange = DEFAULT_leftMotorStrRescalingLowerRange;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.BMStrRescaleSettings dshmLeftRumbleRescaleSettings = dshmContextSettings.RumbleSettings.BMStrRescale;

            if (!this.IsGroupEnabled)
            {
                dshmLeftRumbleRescaleSettings = null;
                return;
            }
            dshmLeftRumbleRescaleSettings.Enabled = this.IsLeftMotorStrRescalingEnabled;
            dshmLeftRumbleRescaleSettings.MinValue = this.LeftMotorStrRescalingLowerRange;
            dshmLeftRumbleRescaleSettings.MaxValue = this.LeftMotorStrRescalingUpperRange;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupRumbleRightConversion : GroupSettings
    {
        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleRightConversion;

        private byte rightRumbleConversionUpperRange;
        private byte rightRumbleConversionLowerRange;

        [Reactive] public bool IsGroupEnabled { get; set; }
        public byte RightRumbleConversionUpperRange
        {
            get => rightRumbleConversionUpperRange;
            set
            {
                byte tempByte = (value < RightRumbleConversionLowerRange) ? (byte)(RightRumbleConversionLowerRange + 1) : value;
                this.RaiseAndSetIfChanged(ref rightRumbleConversionUpperRange, tempByte);
            }
        }
        public byte RightRumbleConversionLowerRange
        {
            get => rightRumbleConversionLowerRange;
            set
            {
                byte tempByte = (value > RightRumbleConversionUpperRange) ? (byte)(RightRumbleConversionUpperRange - 1) : value;
                this.RaiseAndSetIfChanged(ref rightRumbleConversionLowerRange, tempByte);
            }
        }
        [Reactive] public bool IsForcedRightMotorLightThresholdEnabled { get; set; }
        [Reactive] public bool IsForcedRightMotorHeavyThreasholdEnabled { get; set; }
        [Reactive] public byte ForcedRightMotorLightThreshold { get; set; }
        [Reactive] public byte ForcedRightMotorHeavyThreshold { get; set; }

        public GroupRumbleRightConversion(SettingsModeContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();
            RightRumbleConversionUpperRange = DEFAULT_rightRumbleConversionUpperRange;
            RightRumbleConversionLowerRange = DEFAULT_rightRumbleConversionLowerRange;
            IsForcedRightMotorLightThresholdEnabled = DEFAULT_isForcedRightMotorLightThresholdEnabled;
            IsForcedRightMotorHeavyThreasholdEnabled = DEFAULT_isForcedRightMotorHeavyThreasholdEnabled;
            ForcedRightMotorLightThreshold = DEFAULT_forcedRightMotorLightThreshold;
            ForcedRightMotorHeavyThreshold = DEFAULT_forcedRightMotorHeavyThreshold;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.SMToBMConversionSettings dshmSMConversionSettings = dshmContextSettings.RumbleSettings.SMToBMConversion;
            DSHM_Format_ContextSettings.ForcedSMSettings dshmForcedSMSettings = dshmContextSettings.RumbleSettings.ForcedSM;

            if (!this.IsGroupEnabled)
            {
                dshmSMConversionSettings.RescaleMinValue = null;
                dshmSMConversionSettings.RescaleMaxValue = null;

                dshmForcedSMSettings = null;
                return;
            }

            // Right rumble conversion rescaling adjustment
            dshmSMConversionSettings.RescaleMinValue = this.RightRumbleConversionLowerRange;
            dshmSMConversionSettings.RescaleMaxValue = this.RightRumbleConversionUpperRange;

            // Right rumble (light) threshold
            dshmForcedSMSettings.SMThresholdEnabled = this.IsForcedRightMotorLightThresholdEnabled;
            dshmForcedSMSettings.SMThresholdValue = this.ForcedRightMotorLightThreshold;

            // Left rumble (Heavy) threshold
            dshmForcedSMSettings.BMThresholdEnabled = this.IsForcedRightMotorHeavyThreasholdEnabled;
            dshmForcedSMSettings.BMThresholdValue = this.ForcedRightMotorHeavyThreshold;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupModeUnique : GroupSettings
    {


        public override SettingsModeGroups Group { get; } = SettingsModeGroups.Unique_All;

        // General
        [Reactive] public DSHM_HidDeviceModes? HIDDeviceMode { get; set; }

        // SDF and GPJ
        [Reactive] public ControlApp_DsPressureMode PressureExposureMode { get; set; }
        [Reactive] public ControlApp_DPADModes DPadExposureMode { get; set; }

        // XInput
        [Reactive] public bool IsLEDsAsXInputSlotEnabled { get; set; }

        // DS4Windows
        [Reactive] public bool IsDS4LightbarTranslationEnabled { get; set; }

        public GroupModeUnique(SettingsModeContext context) : base(context)
        {
            this.PropertyChanged += GroupModeUnique_PropertyChanged;
        }

        private void GroupModeUnique_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetGroupToOriginalDefaults()
        {
            HIDDeviceMode = DSHM_HidDeviceModes.XInput;
            PressureExposureMode = DEFAULT_pressureExposureMode;
            DPadExposureMode = DEFAULT_padExposureMode;
            IsLEDsAsXInputSlotEnabled = DEFAULT_isLEDsAsXInputSlotEnabled;
            IsDS4LightbarTranslationEnabled = DEFAULT_isDS4LightbarTranslationEnabled;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            dshmContextSettings.HIDDeviceMode =
                (this.Context == SettingsModeContext.General)
                ? this.HIDDeviceMode : null;

            dshmContextSettings.PressureExposureMode =
                (this.Context == SettingsModeContext.SDF
                || this.Context == SettingsModeContext.GPJ)
                ? SaveLoadUtils.PressureModes_Control_DSHM_Pair[this.PressureExposureMode] : null;

            dshmContextSettings.DPadExposureMode =
                (this.Context == SettingsModeContext.SDF
                || this.Context == SettingsModeContext.GPJ)
                ? SaveLoadUtils.DPadModes_Control_DSHM_Pair[this.DPadExposureMode] : null;

        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {

            if ((this.Context == SettingsModeContext.General) && (dshmContextSettings.HIDDeviceMode != null))
                this.HIDDeviceMode = dshmContextSettings.HIDDeviceMode;
            else
                this.HIDDeviceMode = DSHM_HidDeviceModes.XInput;


            if ((this.Context == SettingsModeContext.General) && (dshmContextSettings.HIDDeviceMode != null))
                this.HIDDeviceMode = dshmContextSettings.HIDDeviceMode;
            else
                this.HIDDeviceMode = DSHM_HidDeviceModes.XInput;


            dshmContextSettings.PressureExposureMode =
                (this.Context == SettingsModeContext.SDF
                || this.Context == SettingsModeContext.GPJ)
                ? SaveLoadUtils.PressureModes_Control_DSHM_Pair[this.PressureExposureMode] : null;

            dshmContextSettings.DPadExposureMode =
                (this.Context == SettingsModeContext.SDF
                || this.Context == SettingsModeContext.GPJ)
                ? SaveLoadUtils.DPadModes_Control_DSHM_Pair[this.DPadExposureMode] : null;
        }
    }
}
