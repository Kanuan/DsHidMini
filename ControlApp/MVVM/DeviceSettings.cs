using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class DeviceSettingsManager
    {
        [Reactive] public DeviceModesSettings GeneralSettings { get; set; } = new(SettingsContext.General);
        [Reactive] public DeviceModesSettings SDFSettings { get; set; } = new(SettingsContext.SDF);
        [Reactive] public DeviceModesSettings GPJSettings { get; set; } = new(SettingsContext.GPJ);
        [Reactive] public DeviceModesSettings SXSSettings { get; set; } = new(SettingsContext.SXS);
        [Reactive] public DeviceModesSettings XInputSettings { get; set; } = new(SettingsContext.XInput);
        [Reactive] public DeviceModesSettings DS4WSettings { get; set; } = new(SettingsContext.DS4W);

        public void ResetSettingsToDefault(DeviceModesSettings modeContextSettings)
        {

        }

    }

    public class LEDCustoms
    {
        private int ledIndex;

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
            this.ledIndex = ledIndex;
            Reset();
        }

        internal void Reset()
        {
        IsLEDEnabled = ledIndex == 0 ? true : false;
        Duration = DEFAULT_duration;
        IntervalDuration = DEFAULT_intervalDuration;
        IntervalPortionON = DEFAULT_intervalPortionON;
        IntervalPortionOFF = DEFAULT_intervalPortionOFF;
    }
    }

    public class DeviceModesSettings : ObservableObject
    {

        internal List<GroupSettings> GroupSettingsList = new();

        [Reactive] public SettingsContext Context { get; set; }
        [Reactive] public GroupLEDsControl GroupLEDsControl { get; set; }
        [Reactive] public GroupWireless GroupWireless { get; set; }
        [Reactive] public GroupSticksDeadzone GroupSticksDZ { get; set; }
        [Reactive] public GroupRumbleGeneral GroupRumbleGeneral { get; set; }
        [Reactive] public GroupOutRepControl GroupOutRepControl { get; set; }
        [Reactive] public GroupRumbleLeftRescale GroupRumbleLeftRescale { get; set; }
        [Reactive] public GroupRumbleRightConversion GroupRumbleRightConversion { get; set; }

        public DeviceModesSettings(SettingsContext settingsContext)
        {
            Context = settingsContext;

            GroupSettingsList.Add(GroupLEDsControl = new(Context));
            GroupSettingsList.Add(GroupWireless = new(Context));
            GroupSettingsList.Add(GroupSticksDZ = new(Context));
            GroupSettingsList.Add(GroupRumbleGeneral = new(Context));
            GroupSettingsList.Add(GroupOutRepControl = new(Context));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(Context));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(Context));

            foreach(GroupSettings group in GroupSettingsList)
            {
                group.ResetGroupToOriginalDefaults(Context);
            }
        }


        public void CopyGroupSettings(SettingsModeGroups group, DeviceModesSettings fromSettings, DeviceModesSettings toSettings)
        {

        }
    }

    public abstract class GroupSettings : ReactiveObject
    {
        abstract public SettingsModeGroups Group { get; }

        // DEFAULT VALUES ----------------------------------------------------- START

        public const ControlApp_LEDsModes DEFAULT_ledMode = ControlApp_LEDsModes.BatteryIndicatorPlayerIndex;
        public bool DEFAULT_isWirelessIdleDisconnectEnabled = true;
        public byte DEFAULT_wirelessIdleDisconnectTime = 5;
        public readonly ButtonsCombo DEFAULT_disconnectCombo = new()
        {
            Button1 =  ControlApp_ComboButtons.PS,
            Button2 = ControlApp_ComboButtons.R1,
            Button3 = ControlApp_ComboButtons.L1,
            };
        public const byte DEFAULT_disconnectComboHoldTime = 3;

        public const bool DEFAULT_applyLeftStickDeadzone = true;
        public const bool DEFAULT_applyRightStickDeadzone = true;
        public const double DEFAULT_leftStickDeadzone = 0;
        public const double DEFAULT_rightStickDeadzone = 0;

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


        // DEFAULT VALUES ----------------------------------------------------- END

        public class ButtonsCombo
        {
            [Reactive] public ControlApp_ComboButtons Button1 { get; set; }
            [Reactive] public ControlApp_ComboButtons Button2 { get; set; }
            [Reactive] public ControlApp_ComboButtons Button3 { get; set; }

        }

        public GroupSettings(SettingsContext context)
        {
            ResetGroupToOriginalDefaults(context);
        }

        public bool ShouldGroupBeEnabledOnReset(SettingsContext context)
        {
            return (context == SettingsContext.General || context == SettingsContext.Global);
        }
        public abstract void ResetGroupToOriginalDefaults(SettingsContext context);

        
    }


    public class GroupLEDsControl : GroupSettings
    {
        // -------------------------------------------- LEDS GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.LEDsControl;
        [Reactive] public bool IsGroupLEDsCustomizationEnabled { get; set; }
        [Reactive] public ControlApp_LEDsModes LEDMode { get; set; }
        [Reactive] public LEDCustoms[] LEDsCustoms { get; set; } =
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
                this.RaisePropertyChanged();
            }
        }

        public GroupLEDsControl(SettingsContext context) : base (context)
        {
            
        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupLEDsCustomizationEnabled = ShouldGroupBeEnabledOnReset(context);

            LEDMode = DEFAULT_ledMode;
            foreach (LEDCustoms led in LEDsCustoms)
            {
                led.Reset();
            }
            CurrentLEDCustoms = LEDsCustoms[0];
        }
    }

    public class GroupWireless : GroupSettings
    {
        // -------------------------------------------- WIRELESS SETTINGS GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.WirelessSettings;
        [Reactive] public bool IsGroupWirelessSettingsEnabled { get; set; }
        [Reactive] public bool IsWirelessIdleDisconnectEnabled { get; set; }
        [Reactive] public byte WirelessIdleDisconnectTime { get; set; }
        [Reactive] public ButtonsCombo QuickDisconnectCombo { get; set; }


        public GroupWireless(SettingsContext context) : base(context)
        {
            
        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupWirelessSettingsEnabled = ShouldGroupBeEnabledOnReset(context);
            IsWirelessIdleDisconnectEnabled = DEFAULT_isWirelessIdleDisconnectEnabled;
            WirelessIdleDisconnectTime = DEFAULT_wirelessIdleDisconnectTime;
            QuickDisconnectCombo = new()
            {
                Button1 = ControlApp_ComboButtons.PS,
                Button2 = ControlApp_ComboButtons.L1,
                Button3 = ControlApp_ComboButtons.R1,
            };
            //DisconnectComboHoldTime = DEFAULT_disconnectComboHoldTime;

        }
    }

    public class GroupSticksDeadzone : GroupSettings
    {
        // -------------------------------------------- STICKS DEADZONE GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;
        [Reactive] public bool IsGroupSticksDeadzoneEnabled { get; set; }
        [Reactive] public bool ApplyLeftStickDeadzone { get; set; }
        [Reactive] public bool ApplyRightStickDeadzone { get; set; }
        [Reactive] public double LeftStickDeadzone { get; set; }
        [Reactive] public double RightStickDeadzone { get; set; }

        public GroupSticksDeadzone(SettingsContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupSticksDeadzoneEnabled = ShouldGroupBeEnabledOnReset(context);

            ApplyRightStickDeadzone = DEFAULT_applyRightStickDeadzone;
            ApplyLeftStickDeadzone = DEFAULT_applyLeftStickDeadzone;
            LeftStickDeadzone = DEFAULT_leftStickDeadzone;
            RightStickDeadzone = DEFAULT_rightStickDeadzone;
        }
    }

    public class GroupRumbleGeneral : GroupSettings
    {
        // --------------------------------------------  GENERAL RUMBLE SETTINGS 

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleGeneral;

        private bool isVariableLightRumbleEmulationEnabled;
        private bool isLeftMotorDisabled;
        private bool isRightMotorDisabled;
        [Reactive] public bool IsGroupRumbleGeneralEnabled { get; set; }
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
        public GroupRumbleGeneral(SettingsContext context) : base(context) { }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupRumbleGeneralEnabled = ShouldGroupBeEnabledOnReset(context);

            IsVariableLightRumbleEmulationEnabled = DEFAULT_isVariableLightRumbleEmulationEnabled;
            IsLeftMotorDisabled = DEFAULT_isLeftMotorDisabled;
            IsRightMotorDisabled = DEFAULT_isRightMotorDisabled;

        }
    }

    public class GroupOutRepControl : GroupSettings
    {
        // --------------------------------------------  OUTPUT REPORT CONTROL GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.OutputReportControl;
        [Reactive] public bool IsGroupOutRepControlEnabled { get; set; }
        [Reactive] public bool IsOutputReportRateControlEnabled { get; set; }
        [Reactive] public byte MaxOutputRate { get; set; }
        [Reactive] public bool IsOutputReportDeduplicatorEnabled { get; set; }

        public GroupOutRepControl(SettingsContext context) : base(context) { }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupOutRepControlEnabled = ShouldGroupBeEnabledOnReset(context);

            IsOutputReportRateControlEnabled = DEFAULT_isOutputReportRateControlEnabled;
            MaxOutputRate = DEFAULT_maxOutputRate;
            IsOutputReportDeduplicatorEnabled = DEFAULT_isOutputReportDeduplicatorEnabled;
        }
    }

    public class GroupRumbleLeftRescale : GroupSettings
    {
        // -------------------------------------------- LEFT MOTOR RESCALING GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleLeftStrRescale;
        [Reactive] public bool IsGroupRumbleLeftRescaleEnabled { get; set; }
        [Reactive] public bool IsLeftMotorStrRescalingEnabled { get; set; }
        [Reactive] public byte LeftMotorStrRescalingUpperRange { get; set; }
        [Reactive] public byte LeftMotorStrRescalingLowerRange { get; set; }

        public GroupRumbleLeftRescale(SettingsContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupRumbleLeftRescaleEnabled = ShouldGroupBeEnabledOnReset(context);

            IsLeftMotorStrRescalingEnabled = DEFAULT_isLeftMotorStrRescalingEnabled;
            LeftMotorStrRescalingUpperRange = DEFAULT_leftMotorStrRescalingUpperRange;
            LeftMotorStrRescalingLowerRange = DEFAULT_leftMotorStrRescalingLowerRange;
        }
    }

    public class GroupRumbleRightConversion : GroupSettings
    {
        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleRightConversion;

        private byte rightRumbleConversionUpperRange;
        private byte rightRumbleConversionLowerRange;

        [Reactive] public bool IsGroupRumbleRightConversionEnabled { get; set; }
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

        public GroupRumbleRightConversion(SettingsContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupRumbleRightConversionEnabled = ShouldGroupBeEnabledOnReset(context);
            RightRumbleConversionUpperRange = DEFAULT_rightRumbleConversionUpperRange;
            RightRumbleConversionLowerRange = DEFAULT_rightRumbleConversionLowerRange;
            IsForcedRightMotorLightThresholdEnabled = DEFAULT_isForcedRightMotorLightThresholdEnabled;
            IsForcedRightMotorHeavyThreasholdEnabled = DEFAULT_isForcedRightMotorHeavyThreasholdEnabled;
            ForcedRightMotorLightThreshold = DEFAULT_forcedRightMotorLightThreshold;
            ForcedRightMotorHeavyThreshold = DEFAULT_forcedRightMotorHeavyThreshold;
        }
    }

    public class GroupModeUnique : GroupSettings
    {
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.Unique_All;

        [Reactive] public ControlApp_DsPressureMode PressureExposureMode { get; set; }
        [Reactive] public ControlApp_DPADModes PadExposureMode { get; set; }

        public GroupModeUnique(SettingsContext context) : base(context)
        {

        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            PressureExposureMode = DEFAULT_pressureExposureMode;
            PadExposureMode = DEFAULT_padExposureMode;
        }
    }
}
