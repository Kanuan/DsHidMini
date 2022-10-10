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
        public SettingsContext DEFAULT_CurrentSettingContext { get; set; }

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

        public SettingsContext CurrentSettingContext { get; set; }

        private bool isGroupLEDsCustomizationEnabled;
        public bool isGroupWirelessSettingsEnabled;
        private bool isGroupSticksDeadzoneEnabled;
        private bool isGroupRumbleGeneralEnabled;
        private bool isGroupOutRepControlEnabled;
        private bool isGroupRumbleLeftRescaleEnabled;
        private bool isGroupRumbleRightConversionEnabled;

        private LEDsModes ledMode = LEDsModes.BatterySingleLED;
        public bool isWirelessIdleDisconnectDisabled;
        public byte wirelessIdleDisconnectTime;
        private QuickDisconnectCombo disconnectCombo;
        private byte disconnectComboHoldTime;
        private bool applyLeftStickDeadzone;

        private LEDCustoms currentLEDCustoms;
        private LEDCustoms[] ledsCustoms = new LEDCustoms[4]
       {
            new LEDCustoms(0), new LEDCustoms(1), new LEDCustoms(2), new LEDCustoms(3),
       };

        private bool applyRightStickDeadzone;
        private double leftStickDeadzone;
        private double rightStickDeadzone;
        private bool isVariableLightRumbleEmulationEnabled;
        private byte rightRumbleConversionUpperRange;
        private byte rightRumbleConversionLowerRange;
        private bool isForcedRightMotorLightThresholdEnabled;
        private bool isForcedRightMotorHeavyThreasholdEnabled;
        private byte forcedRightMotorLightThreshold;
        private byte forcedRightMotorHeavyThreshold;
        private bool isLeftMotorDisabled;
        private bool isRightMotorDisabled;
        private bool isOutputReportRateControlEnabled;
        private byte maxOutputRate;
        private bool isOutputReportDeduplicatorEnabled;
        private bool isLeftMotorStrRescalingEnabled;
        private byte leftMotorStrRescalingUpperRange;
        private byte leftMotorStrRescalingLowerRange;
        private DsPressureExposureMode pressureExposureMode = DsPressureExposureMode.DsPressureExposureModeBoth;
        private DS_DPAD_EXPOSURE_MODE padExposureMode = DS_DPAD_EXPOSURE_MODE.DsDPadExposureModeHAT;


        // -------------------------------------------- LEDS GROUP

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


        // -------------------------------------------- WIRELESS SETTINGS GROUP

        public bool IsGroupWirelessSettingsEnabled { get => isGroupWirelessSettingsEnabled; set => SetProperty(ref isGroupWirelessSettingsEnabled, value); }
        public bool IsWirelessIdleDisconnectDisabled { get => isWirelessIdleDisconnectDisabled; set => SetProperty(ref isWirelessIdleDisconnectDisabled, value); }
        public byte WirelessIdleDisconnectTime { get => wirelessIdleDisconnectTime; set => SetProperty(ref wirelessIdleDisconnectTime, value); }
        public QuickDisconnectCombo DisconnectCombo { get => disconnectCombo; set => SetProperty(ref disconnectCombo, value); }
        public byte DisconnectComboHoldTime { get => disconnectComboHoldTime; set => SetProperty(ref disconnectComboHoldTime, value); }

        // -------------------------------------------- STICKS DEADZONE GROUP

        public bool IsGroupSticksDeadzoneEnabled { get => isGroupSticksDeadzoneEnabled; set => SetProperty(ref isGroupSticksDeadzoneEnabled, value); }
        public bool ApplyLeftStickDeadzone { get => applyLeftStickDeadzone; set => SetProperty(ref applyLeftStickDeadzone, value); }
        public bool ApplyRightStickDeadzone { get => applyRightStickDeadzone; set => SetProperty(ref applyRightStickDeadzone, value); }
        public double LeftStickDeadzone { get => leftStickDeadzone; set => SetProperty(ref leftStickDeadzone, value); }
        public double RightStickDeadzone { get => rightStickDeadzone; set => SetProperty(ref rightStickDeadzone, value); }

        // --------------------------------------------  GENERAL RUMBLE SETTINGS GROUP

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

        // --------------------------------------------  OUTPUT REPORT CONTROL GROUP

        public bool IsGroupOutRepControlEnabled { get => isGroupOutRepControlEnabled; set => SetProperty(ref isGroupOutRepControlEnabled, value); }
        public bool IsOutputReportRateControlEnabled { get => isOutputReportRateControlEnabled; set => SetProperty(ref isOutputReportRateControlEnabled, value); }
        public byte MaxOutputRate { get => maxOutputRate; set => SetProperty(ref maxOutputRate, value); }
        public bool IsOutputReportDeduplicatorEnabled { get => isOutputReportDeduplicatorEnabled; set => SetProperty(ref isOutputReportDeduplicatorEnabled, value); }

        // -------------------------------------------- LEFT MOTOR RESCALING GROUP

        public bool IsGroupRumbleLeftRescaleEnabled { get => isGroupRumbleLeftRescaleEnabled; set => SetProperty(ref isGroupRumbleLeftRescaleEnabled, value); }
        public bool IsLeftMotorStrRescalingEnabled { get => isLeftMotorStrRescalingEnabled; set => SetProperty(ref isLeftMotorStrRescalingEnabled, value); }
        public byte LeftMotorStrRescalingUpperRange { get => leftMotorStrRescalingUpperRange; set => SetProperty(ref leftMotorStrRescalingUpperRange, value); }
        public byte LeftMotorStrRescalingLowerRange { get => leftMotorStrRescalingLowerRange; set => SetProperty(ref leftMotorStrRescalingLowerRange, value); }

        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP

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

        // -------------------------------------------- 

        public DsPressureExposureMode PressureExposureMode { get => pressureExposureMode; set => SetProperty(ref pressureExposureMode, value); }
        public DS_DPAD_EXPOSURE_MODE PadExposureMode { get => padExposureMode; set => SetProperty(ref padExposureMode, value); }

        // -------------------------------------------- 

        public DeviceModesSettings(SettingsContext settingsContext)
        {
            CurrentSettingContext = settingsContext;
            bool tempBool = (CurrentSettingContext == SettingsContext.General || CurrentSettingContext == SettingsContext.Global) ? true : false;
            IsGroupLEDsCustomizationEnabled =
                IsGroupWirelessSettingsEnabled =
                IsGroupSticksDeadzoneEnabled =
                IsGroupRumbleGeneralEnabled =
                IsGroupOutRepControlEnabled =
                IsGroupRumbleLeftRescaleEnabled =
                IsGroupRumbleRightConversionEnabled = tempBool;

            currentLEDCustoms = LEDsCustoms[0];
        }

        public void ResetToFactoryDefault()
        {
            /// Groups must be disabled by default for all modes that aren't global/default and general
            bool tempBool = (CurrentSettingContext == SettingsContext.General || CurrentSettingContext == SettingsContext.Global) ? true : false;
            IsGroupLEDsCustomizationEnabled =
                IsGroupWirelessSettingsEnabled =
                IsGroupSticksDeadzoneEnabled =
                IsGroupRumbleGeneralEnabled =
                IsGroupOutRepControlEnabled =
                IsGroupRumbleLeftRescaleEnabled =
                IsGroupRumbleRightConversionEnabled = tempBool;

            isWirelessIdleDisconnectDisabled = DEFAULT_isWirelessIdleDisconnectDisabled;
            wirelessIdleDisconnectTime = DEFAULT_wirelessIdleDisconnectTime;
            disconnectCombo = DEFAULT_disconnectCombo;
            disconnectComboHoldTime = DEFAULT_disconnectComboHoldTime;

            applyRightStickDeadzone = DEFAULT_applyRightStickDeadzone;
            applyLeftStickDeadzone = DEFAULT_applyLeftStickDeadzone;
            leftStickDeadzone = DEFAULT_leftStickDeadzone;
            rightStickDeadzone = DEFAULT_rightStickDeadzone;

            ledMode = DEFAULT_ledMode;
            foreach (LEDCustoms led in ledsCustoms)
            {
                led.Reset();
            }
            currentLEDCustoms = LEDsCustoms[0];

            isOutputReportRateControlEnabled = DEFAULT_isOutputReportRateControlEnabled;
            maxOutputRate = DEFAULT_maxOutputRate;
            isOutputReportDeduplicatorEnabled = DEFAULT_isOutputReportDeduplicatorEnabled;


            rightRumbleConversionUpperRange = DEFAULT_rightRumbleConversionUpperRange;
            rightRumbleConversionLowerRange = DEFAULT_rightRumbleConversionLowerRange;
            isForcedRightMotorLightThresholdEnabled = DEFAULT_isForcedRightMotorLightThresholdEnabled;
            isForcedRightMotorHeavyThreasholdEnabled = DEFAULT_isForcedRightMotorHeavyThreasholdEnabled;
            forcedRightMotorLightThreshold = DEFAULT_forcedRightMotorLightThreshold;
            forcedRightMotorHeavyThreshold = DEFAULT_forcedRightMotorHeavyThreshold;


            isVariableLightRumbleEmulationEnabled = DEFAULT_isVariableLightRumbleEmulationEnabled;
            isLeftMotorDisabled = DEFAULT_isLeftMotorDisabled;
            isRightMotorDisabled = DEFAULT_isRightMotorDisabled;

            isLeftMotorStrRescalingEnabled = DEFAULT_isLeftMotorStrRescalingEnabled;
            leftMotorStrRescalingUpperRange = DEFAULT_leftMotorStrRescalingUpperRange;
            leftMotorStrRescalingLowerRange = DEFAULT_leftMotorStrRescalingLowerRange;


            pressureExposureMode = DEFAULT_pressureExposureMode;
            padExposureMode = DEFAULT_padExposureMode;
    }

        public void CopyGroupSettings(SettingsModeGroups group, DeviceModesSettings fromSettings, DeviceModesSettings toSettings)
        {

        }
    }

    internal class GroupSettings : ObservableObject
    {
        public SettingsModeGroups Group { get; set; }

        public GroupSettings(SettingsModeGroups group)
        {
            Group = group;
        }
    }

    internal class GroupLEDsControl : GroupSettings
    {
        public GroupLEDsControl(SettingsModeGroups group) : base (group)
        {

        }
    }
}
