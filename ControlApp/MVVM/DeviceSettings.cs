using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class DeviceSettingsManager : ObservableObject
    {
        public DeviceModesSettings GeneralSettings = new(SettingsContext.General);
        public DeviceModesSettings SDFSettings = new(SettingsContext.SDF);
        public DeviceModesSettings GPJSettings = new(SettingsContext.GPJ);
        public DeviceModesSettings XInputSettings = new(SettingsContext.XInput);
        public DeviceModesSettings DS4WSettings = new(SettingsContext.DS4W);


        public void ResetSettingsToDefault(DeviceModesSettings modeContextSettings)
        {

        }

    }

    public class LEDCustoms : ObservableObject
    {
        private int ledIndex;

        private byte DEFAULT_duration = 0xFF;
        private byte DEFAULT_intervalDuration = 0xFF;
        private byte DEFAULT_intervalPortionON = 0xFF;
        private byte DEFAULT_intervalPortionOFF = 0x00;

        private bool isLEDEnabled;
        private byte duration;
        private byte intervalDuration;
        private byte intervalPortionON;
        private byte intervalPortionOFF;

        public bool IsLEDEnabled { get => isLEDEnabled; set => SetProperty(ref isLEDEnabled, value); }

        public int LEDIndex { get => ledIndex; private set => SetProperty(ref ledIndex, value); }
        public byte Duration { get => duration; set => SetProperty(ref duration, value); }
        public byte IntervalDuration { get => intervalDuration; set => SetProperty(ref intervalDuration, value); }
        public byte IntervalPortionON { get => intervalPortionON; set => SetProperty(ref intervalPortionON, value); }
        public byte IntervalPortionOFF { get => intervalPortionOFF; set => SetProperty(ref intervalPortionOFF, value); }
        public LEDCustoms(int ledIndex)
        {
            this.ledIndex = ledIndex;
            Reset();
        }

        internal void Reset()
        {
        isLEDEnabled = ledIndex == 0 ? true : false;
        duration = DEFAULT_duration;
        intervalDuration = DEFAULT_intervalDuration;
        intervalPortionON = DEFAULT_intervalPortionON;
        intervalPortionOFF = DEFAULT_intervalPortionOFF;
    }
    }

    public class DeviceModesSettings : ObservableObject
    {

        private SettingsContext context;
        private GroupLEDsControl groupLEDsControl;
        private GroupWireless groupWireless;
        private GroupSticksDeadzone groupSticksDZ;
        private GroupRumbleGeneral groupRumbleGeneral;
        private GroupOutRepControl groupOutRepControl;
        private GroupRumbleLeftRescale groupRumbleLeftRescale;
        private GroupRumbleRightConversion groupRumbleRightConversion;

        internal List<GroupSettings> GroupSettingsList = new();

        public SettingsContext Context { get => context; set => SetProperty(ref context, value); }
        public GroupLEDsControl GroupLEDsControl { get => groupLEDsControl; set => SetProperty(ref groupLEDsControl, value); }
        public GroupWireless GroupWireless { get => groupWireless; set => SetProperty(ref groupWireless, value); }
        public GroupSticksDeadzone GroupSticksDZ { get => groupSticksDZ; set => SetProperty(ref groupSticksDZ, value); }
        public GroupRumbleGeneral GroupRumbleGeneral { get => groupRumbleGeneral; set => SetProperty(ref groupRumbleGeneral, value); }
        public GroupOutRepControl GroupOutRepControl { get => groupOutRepControl; set => SetProperty(ref groupOutRepControl, value); }
        public GroupRumbleLeftRescale GroupRumbleLeftRescale { get => groupRumbleLeftRescale; set => SetProperty(ref groupRumbleLeftRescale, value); }
        public GroupRumbleRightConversion GroupRumbleRightConversion { get => groupRumbleRightConversion; set => SetProperty(ref groupRumbleRightConversion, value); }

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

    public abstract class GroupSettings : ObservableObject
    {
        abstract public SettingsModeGroups Group { get; }

        // DEFAULT VALUES ----------------------------------------------------- START

        public const LEDsModes DEFAULT_ledMode = LEDsModes.BatterySingleLED;
        public bool DEFAULT_isWirelessIdleDisconnectDisabled = false;
        public byte DEFAULT_wirelessIdleDisconnectTime = 5;
        public const QuickDisconnectCombo DEFAULT_disconnectCombo = QuickDisconnectCombo.PS_R1_L1;
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

        public const DsPressureExposureMode DEFAULT_pressureExposureMode = DsPressureExposureMode.DsPressureExposureModeBoth;
        public const DS_DPAD_EXPOSURE_MODE DEFAULT_padExposureMode = DS_DPAD_EXPOSURE_MODE.DsDPadExposureModeHAT;

        // DEFAULT VALUES ----------------------------------------------------- END

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

        private bool isGroupLEDsCustomizationEnabled;
        private LEDsModes ledMode = LEDsModes.BatterySingleLED;
        private LEDCustoms currentLEDCustoms;
        private LEDCustoms[] ledsCustoms = new LEDCustoms[4]
       {
            new LEDCustoms(0), new LEDCustoms(1), new LEDCustoms(2), new LEDCustoms(3),
       };

        public bool IsGroupLEDsCustomizationEnabled { get => isGroupLEDsCustomizationEnabled; set => SetProperty(ref isGroupLEDsCustomizationEnabled, value); }
        public LEDsModes LEDMode { get => ledMode; set => SetProperty(ref ledMode, value); }
        public LEDCustoms[] LEDsCustoms { get => ledsCustoms; set => SetProperty(ref ledsCustoms, value); }
        public LEDCustoms CurrentLEDCustoms { get => currentLEDCustoms; set => SetProperty(ref currentLEDCustoms, value); }
        public int CurrentLEDCustomsIndex
        {
            get => CurrentLEDCustoms.LEDIndex;
            set
            {
                CurrentLEDCustoms = LEDsCustoms[value];
                OnPropertyChanged("CurrentLEDCustomsIndex"); // Is this correct?
            }
        }

        public GroupLEDsControl(SettingsContext context) : base (context)
        {
            
        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupLEDsCustomizationEnabled = ShouldGroupBeEnabledOnReset(context);

            LEDMode = DEFAULT_ledMode;
            foreach (LEDCustoms led in ledsCustoms)
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

        public bool isGroupWirelessSettingsEnabled;
        public bool isWirelessIdleDisconnectDisabled;
        public byte wirelessIdleDisconnectTime;
        private QuickDisconnectCombo disconnectCombo;
        private byte disconnectComboHoldTime;
        public bool IsGroupWirelessSettingsEnabled { get => isGroupWirelessSettingsEnabled; set => SetProperty(ref isGroupWirelessSettingsEnabled, value); }
        public bool IsWirelessIdleDisconnectDisabled { get => isWirelessIdleDisconnectDisabled; set => SetProperty(ref isWirelessIdleDisconnectDisabled, value); }
        public byte WirelessIdleDisconnectTime { get => wirelessIdleDisconnectTime; set => SetProperty(ref wirelessIdleDisconnectTime, value); }
        public QuickDisconnectCombo DisconnectCombo { get => disconnectCombo; set => SetProperty(ref disconnectCombo, value); }
        public byte DisconnectComboHoldTime { get => disconnectComboHoldTime; set => SetProperty(ref disconnectComboHoldTime, value); }


        public GroupWireless(SettingsContext context) : base(context)
        {
            
        }

        public override void ResetGroupToOriginalDefaults(SettingsContext context)
        {
            IsGroupWirelessSettingsEnabled = ShouldGroupBeEnabledOnReset(context);
            IsWirelessIdleDisconnectDisabled = DEFAULT_isWirelessIdleDisconnectDisabled;
            WirelessIdleDisconnectTime = DEFAULT_wirelessIdleDisconnectTime;
            DisconnectCombo = DEFAULT_disconnectCombo;
            DisconnectComboHoldTime = DEFAULT_disconnectComboHoldTime;
        }
    }

    public class GroupSticksDeadzone : GroupSettings
    {
        // -------------------------------------------- STICKS DEADZONE GROUP

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;

        private bool isGroupSticksDeadzoneEnabled;
        private bool applyLeftStickDeadzone;
        private bool applyRightStickDeadzone;
        private double leftStickDeadzone;
        private double rightStickDeadzone;

        public bool IsGroupSticksDeadzoneEnabled { get => isGroupSticksDeadzoneEnabled; set => SetProperty(ref isGroupSticksDeadzoneEnabled, value); }
        public bool ApplyLeftStickDeadzone { get => applyLeftStickDeadzone; set => SetProperty(ref applyLeftStickDeadzone, value); }
        public bool ApplyRightStickDeadzone { get => applyRightStickDeadzone; set => SetProperty(ref applyRightStickDeadzone, value); }
        public double LeftStickDeadzone { get => leftStickDeadzone; set => SetProperty(ref leftStickDeadzone, value); }
        public double RightStickDeadzone { get => rightStickDeadzone; set => SetProperty(ref rightStickDeadzone, value); }

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

        private bool isGroupRumbleGeneralEnabled;
        private bool isVariableLightRumbleEmulationEnabled;
        private bool isLeftMotorDisabled;
        private bool isRightMotorDisabled;
        public bool IsGroupRumbleGeneralEnabled { get => isGroupRumbleGeneralEnabled; set => SetProperty(ref isGroupRumbleGeneralEnabled, value); }
        public bool IsVariableLightRumbleEmulationEnabled
        {
            get => isVariableLightRumbleEmulationEnabled;
            set
            {
                if (value)
                {
                    IsLeftMotorDisabled = IsRightMotorDisabled = false;
                }
                SetProperty(ref isVariableLightRumbleEmulationEnabled, value);
            }
        }

        public bool IsLeftMotorDisabled
        {
            get => isLeftMotorDisabled;
            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                SetProperty(ref isLeftMotorDisabled, value);
            }
        }
        public bool IsRightMotorDisabled
        {
            get => isRightMotorDisabled;
            set
            {
                if (value) IsVariableLightRumbleEmulationEnabled = false;
                SetProperty(ref isRightMotorDisabled, value);
            }
        }
        public GroupRumbleGeneral(SettingsContext context) : base(context)
        {

        }

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

        private bool isGroupOutRepControlEnabled;
        private bool isOutputReportRateControlEnabled;
        private byte maxOutputRate;
        private bool isOutputReportDeduplicatorEnabled;
        public bool IsGroupOutRepControlEnabled { get => isGroupOutRepControlEnabled; set => SetProperty(ref isGroupOutRepControlEnabled, value); }
        public bool IsOutputReportRateControlEnabled { get => isOutputReportRateControlEnabled; set => SetProperty(ref isOutputReportRateControlEnabled, value); }
        public byte MaxOutputRate { get => maxOutputRate; set => SetProperty(ref maxOutputRate, value); }
        public bool IsOutputReportDeduplicatorEnabled { get => isOutputReportDeduplicatorEnabled; set => SetProperty(ref isOutputReportDeduplicatorEnabled, value); }

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

        private bool isGroupRumbleLeftRescaleEnabled;
        private bool isLeftMotorStrRescalingEnabled;
        private byte leftMotorStrRescalingUpperRange;
        private byte leftMotorStrRescalingLowerRange;
        public bool IsGroupRumbleLeftRescaleEnabled { get => isGroupRumbleLeftRescaleEnabled; set => SetProperty(ref isGroupRumbleLeftRescaleEnabled, value); }
        public bool IsLeftMotorStrRescalingEnabled { get => isLeftMotorStrRescalingEnabled; set => SetProperty(ref isLeftMotorStrRescalingEnabled, value); }
        public byte LeftMotorStrRescalingUpperRange { get => leftMotorStrRescalingUpperRange; set => SetProperty(ref leftMotorStrRescalingUpperRange, value); }
        public byte LeftMotorStrRescalingLowerRange { get => leftMotorStrRescalingLowerRange; set => SetProperty(ref leftMotorStrRescalingLowerRange, value); }

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

        private bool isGroupRumbleRightConversionEnabled;
        private byte rightRumbleConversionUpperRange;
        private byte rightRumbleConversionLowerRange;
        private bool isForcedRightMotorLightThresholdEnabled;
        private bool isForcedRightMotorHeavyThreasholdEnabled;
        private byte forcedRightMotorLightThreshold;
        private byte forcedRightMotorHeavyThreshold;

        public bool IsGroupRumbleRightConversionEnabled { get => isGroupRumbleRightConversionEnabled; set => SetProperty(ref isGroupRumbleRightConversionEnabled, value); }
        public byte RightRumbleConversionUpperRange
        {
            get => rightRumbleConversionUpperRange;
            set
            {
                byte tempByte = (value < RightRumbleConversionLowerRange) ? (byte)(RightRumbleConversionLowerRange + 1) : value;
                SetProperty(ref rightRumbleConversionUpperRange, tempByte);
            }
        }
        public byte RightRumbleConversionLowerRange
        {
            get => rightRumbleConversionLowerRange;
            set
            {
                byte tempByte = (value > RightRumbleConversionUpperRange) ? (byte)(RightRumbleConversionUpperRange - 1) : value;
                SetProperty(ref rightRumbleConversionLowerRange, tempByte);
            }
        }
        public bool IsForcedRightMotorLightThresholdEnabled { get => isForcedRightMotorLightThresholdEnabled; set => SetProperty(ref isForcedRightMotorLightThresholdEnabled, value); }
        public bool IsForcedRightMotorHeavyThreasholdEnabled { get => isForcedRightMotorHeavyThreasholdEnabled; set => SetProperty(ref isForcedRightMotorHeavyThreasholdEnabled, value); }
        public byte ForcedRightMotorLightThreshold { get => forcedRightMotorLightThreshold; set => SetProperty(ref forcedRightMotorLightThreshold, value); }
        public byte ForcedRightMotorHeavyThreshold { get => forcedRightMotorHeavyThreshold; set => SetProperty(ref forcedRightMotorHeavyThreshold, value); }

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

        private DsPressureExposureMode pressureExposureMode = DsPressureExposureMode.DsPressureExposureModeBoth;
        private DS_DPAD_EXPOSURE_MODE padExposureMode = DS_DPAD_EXPOSURE_MODE.DsDPadExposureModeHAT;
        public DsPressureExposureMode PressureExposureMode { get => pressureExposureMode; set => SetProperty(ref pressureExposureMode, value); }
        public DS_DPAD_EXPOSURE_MODE PadExposureMode { get => padExposureMode; set => SetProperty(ref padExposureMode, value); }

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
