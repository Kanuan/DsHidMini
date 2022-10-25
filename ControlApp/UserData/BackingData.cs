using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.MVVM;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nefarius.DsHidMini.ControlApp.MVVM.GroupLEDsCustomsVM;
using static Nefarius.DsHidMini.ControlApp.MVVM.GroupSettingsVM;

namespace Nefarius.DsHidMini.ControlApp.UserData
{
    public class BackingDataContainer
    {

        public BackingData_ModesUnique modesUniqueData { get; set; } = new();
        public BackingData_LEDs ledsData { get; set; } = new();
        public BackingData_Wireless wirelessData { get; set; } = new();
        public BackingData_SticksDZ sticksDZData { get; set; } = new();
        public BackingData_RumbleGeneral rumbleGeneralData { get; set; } = new();
        public BackingData_OutRepControl outRepData { get; set; } = new();
        public BackingData_LeftRumbleRescale leftRumbleRescaleData { get; set; } = new();
        public BackingData_VariablaRightRumbleEmulAdjusts rightVariableEmulData { get; set; } = new();

        public static readonly BackingDataContainer DefaultContainer = new BackingDataContainer();

        public BackingDataContainer()
        {
                this.resetDatasToDefault();
        }

        public void PrepareForLoading()
        {
            leftRumbleRescaleData.PrepareForSettingsLoading();
            rightVariableEmulData.PrepareForSettingsLoading();
        }

        public void resetDatasToDefault()
        {
            modesUniqueData.ResetToDefault();
            ledsData.ResetToDefault();
            wirelessData.ResetToDefault();
            sticksDZData.ResetToDefault();
            rumbleGeneralData.ResetToDefault();
            outRepData.ResetToDefault();
            leftRumbleRescaleData.ResetToDefault();
            rightVariableEmulData.ResetToDefault();
        }

        public void ConvertAllToDSHM(DSHM_Format_ContextSettings dshm_data)
        {
            DSHM_Format_ContextSettings modeContext = dshm_data;
            switch (modesUniqueData.SettingsContext)
            {
                case SettingsContext.SDF:
                    modeContext = dshm_data.SDF;
                    break;
                case SettingsContext.GPJ:
                    modeContext = dshm_data.GPJ;
                    break;
                case SettingsContext.SXS:
                    modeContext = dshm_data.SXS;
                    break;
                case SettingsContext.DS4W:
                    modeContext = dshm_data.DS4Windows;
                    break;
                case SettingsContext.XInput:
                    modeContext = dshm_data.XInput;
                    break;
                default:
                    break;
            }


            modesUniqueData.SaveToDSHMSettings(dshm_data);
            ledsData.SaveToDSHMSettings(modeContext);
            wirelessData.SaveToDSHMSettings(dshm_data);
            sticksDZData.SaveToDSHMSettings(modeContext);
            rumbleGeneralData.SaveToDSHMSettings(modeContext);
            outRepData.SaveToDSHMSettings(dshm_data);
            leftRumbleRescaleData.SaveToDSHMSettings(modeContext);
            rightVariableEmulData.SaveToDSHMSettings(modeContext);

            if(modesUniqueData.SettingsContext == SettingsContext.DS4W)
            {
                dshm_data.DeadZoneLeft.Apply = false;
                dshm_data.DeadZoneRight.Apply = false;
            }
        }
    }

    public abstract class SettingsBackingData
    {
        protected bool isGroupEnabled = true;

        public bool IsGroupEnabled { get => isGroupEnabled; set => isGroupEnabled = value; }

        //public SettingsContext Context = SettingsContext.XInput;

        public abstract void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings);


    }

    public class BackingData_ModesUnique : SettingsBackingData
    {
        public SettingsContext SettingsContext { get; set; }
        public ControlApp_DsPressureMode PressureExposureMode { get; set; }
        public ControlApp_DPADModes DPadExposureMode { get; set; }
        public bool IsLEDsAsXInputSlotEnabled { get; set; }
        public bool PreventRemappingConflictsInSXSMode { get; set; }
        public bool PreventRemappingConflictsInDS4WMode { get; set; }
        public bool IsDS4LightbarTranslationEnabled { get; set; }

        public void ResetToDefault()
        {
            SettingsContext = SettingsContext.XInput;
            PressureExposureMode = ControlApp_DsPressureMode.Default;
            DPadExposureMode = ControlApp_DPADModes.HAT;
            IsLEDsAsXInputSlotEnabled = false;
            IsDS4LightbarTranslationEnabled = false;
            PreventRemappingConflictsInSXSMode = false;
            PreventRemappingConflictsInDS4WMode = true;
        }

        public static void CopySettings(BackingData_ModesUnique destiny, BackingData_ModesUnique source)
        {
            destiny.SettingsContext = source.SettingsContext;
            destiny.PressureExposureMode = source.PressureExposureMode;
            destiny.DPadExposureMode = source.DPadExposureMode;
            destiny.IsLEDsAsXInputSlotEnabled = source.IsLEDsAsXInputSlotEnabled;
            destiny.IsDS4LightbarTranslationEnabled = source.IsDS4LightbarTranslationEnabled;
            destiny.PreventRemappingConflictsInSXSMode = source.PreventRemappingConflictsInSXSMode;
            destiny.PreventRemappingConflictsInDS4WMode = source.PreventRemappingConflictsInDS4WMode;
        }


        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            if(SettingsContext != SettingsContext.General)
            {
                dshmContextSettings.HIDDeviceMode = SaveLoadUtils.Get_DSHM_HIDDeviceMode_From_ControlApp[SettingsContext];
            }

            dshmContextSettings.SDF.PressureExposureMode = dshmContextSettings.GPJ.PressureExposureMode =
                (this.SettingsContext == SettingsContext.SDF
                || this.SettingsContext == SettingsContext.GPJ)
                ? SaveLoadUtils.Get_DSHM_DsPressureMode_From_ControlApp[this.PressureExposureMode] : null;

            dshmContextSettings.SDF.DPadExposureMode = dshmContextSettings.GPJ.DPadExposureMode =
                (this.SettingsContext == SettingsContext.SDF
                || this.SettingsContext == SettingsContext.GPJ)
                ? SaveLoadUtils.Get_DSHM_DPadMode_From_ControlApp[this.DPadExposureMode] : null;
        }
    }

    public class BackingData_LEDs : SettingsBackingData
    {
        public ControlApp_LEDsModes LEDMode { get; set; }
        public LEDsCustoms LEDsCustoms { get; set; } = new();

        public static readonly BackingData_LEDs defaultLEDsData = new()
        {
            LEDMode = ControlApp_LEDsModes.BatteryIndicatorPlayerIndex,
            LEDsCustoms = new(),
        };

        public void ResetToDefault()
        {
            LEDMode = ControlApp_LEDsModes.BatteryIndicatorPlayerIndex;
            LEDsCustoms.ResetLEDsCustoms();
        }

        public static void CopySettings(BackingData_LEDs destiny, BackingData_LEDs source)
        {
            destiny.IsGroupEnabled = source.IsGroupEnabled;
            destiny.LEDMode = source.LEDMode;
            destiny.LEDsCustoms.CopyLEDsCustoms(source.LEDsCustoms);
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

            var dshm_Customs = dshm_AllLEDsSettings.CustomPatterns;

            var dshm_singleLED = new DSHM_Format_ContextSettings.SingleLEDCustoms[]
            { dshm_Customs.Player1, dshm_Customs.Player2,dshm_Customs.Player3,dshm_Customs.Player4, };

            for (int i = 0; i < 4; i++)
            {
                LEDsCustoms.singleLEDCustoms ledCustoms = this.LEDsCustoms.LED_x_Customs[i];
                switch (this.LEDMode)
                {
                    case ControlApp_LEDsModes.CustomPattern:
                        dshm_singleLED[i].Enabled = ledCustoms.IsLEDEffectEnabled ? (byte)0x10 : (byte)0x00;
                        dshm_singleLED[i].Duration = ledCustoms.Duration;
                        dshm_singleLED[i].IntervalDuration = ledCustoms.IntervalDuration;
                        dshm_singleLED[i].IntervalPortionOn = ledCustoms.IntervalPortionON;
                        dshm_singleLED[i].IntervalPortionOff = ledCustoms.IntervalPortionOFF;
                        break;
                    case ControlApp_LEDsModes.CustomStatic:
                        dshm_singleLED[i].Enabled = ledCustoms.IsLEDEffectEnabled ? (byte)0x10 : (byte)0x00;
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
    }

    public class BackingData_Wireless : SettingsBackingData
    {
        public bool IsWirelessIdleDisconnectEnabled { get; set; }
        public int WirelessIdleDisconnectTime { get; set; }
        public bool IsQuickDisconnectComboEnabled { get; set; }
        public ButtonsCombo QuickDisconnectCombo { get; set; } = new();

        public void ResetToDefault()
        {
            IsWirelessIdleDisconnectEnabled = true;
            WirelessIdleDisconnectTime = 5;
            IsQuickDisconnectComboEnabled = true;
            QuickDisconnectCombo = new()
            {
                Button1 = ControlApp_ComboButtons.PS,
                Button2 = ControlApp_ComboButtons.R1,
                Button3 = ControlApp_ComboButtons.L1,
            };
        }

        public static void CopySettings(BackingData_Wireless destiny, BackingData_Wireless source)
        {
            destiny.IsGroupEnabled = source.IsGroupEnabled;
            destiny.IsQuickDisconnectComboEnabled = source.IsQuickDisconnectComboEnabled;
            destiny.IsWirelessIdleDisconnectEnabled = source.IsWirelessIdleDisconnectEnabled;
            destiny.QuickDisconnectCombo.copyCombo(source.QuickDisconnectCombo);
            destiny.WirelessIdleDisconnectTime = source.WirelessIdleDisconnectTime;
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

            dshmContextSettings.DisableWirelessIdleTimeout = !this.IsWirelessIdleDisconnectEnabled;
            dshmContextSettings.WirelessIdleTimeoutPeriodMs = this.WirelessIdleDisconnectTime * 60 * 1000;
            //dshmContextSettings.QuickDisconnectCombo = dictionary combo pair;
        }
    }

    public class BackingData_SticksDZ : SettingsBackingData
    {
        public bool ApplyLeftStickDeadZone { get; set; }
        public bool ApplyRightStickDeadZone { get; set; }
        public int LeftStickDeadZone { get; set; }
        public int RightStickDeadZone { get; set; }

        public void ResetToDefault()
        {
            ApplyRightStickDeadZone = true;
            ApplyLeftStickDeadZone = true;
            LeftStickDeadZone = 0;
            RightStickDeadZone = 0;
        }

        public static void CopySettings(BackingData_SticksDZ destiny, BackingData_SticksDZ source)
        {
            destiny.IsGroupEnabled = source.IsGroupEnabled;

            destiny.ApplyRightStickDeadZone = source.ApplyRightStickDeadZone;
            destiny.ApplyLeftStickDeadZone = source.ApplyLeftStickDeadZone;
            destiny.LeftStickDeadZone = source.LeftStickDeadZone;
            destiny.RightStickDeadZone = source.RightStickDeadZone;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
            DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;

            dshmLeftDZSettings.Apply = this.ApplyLeftStickDeadZone;
            dshmLeftDZSettings.PolarValue = (byte)this.LeftStickDeadZone;

            dshmRightDZSettings.Apply = this.ApplyRightStickDeadZone;
            dshmRightDZSettings.PolarValue = (byte)this.RightStickDeadZone;
        }

    }

    public class BackingData_RumbleGeneral : SettingsBackingData
    {
        // -------------------------------------------- DEFAULT GENERAL RUMBLE SETTINGS START

        public const bool DEFAULT_isVariableLightRumbleEmulationEnabled = false;
        public const bool DEFAULT_isLeftMotorDisabled = false;
        public const bool DEFAULT_isRightMotorDisabled = false;
        public const bool DEFAULT_isVariableRightEmulToggleComboEnabled = false;
        public static readonly ButtonsCombo DEFAULT_VariableRightEmuToggleCombo = new()
        {
            Button1 = ControlApp_ComboButtons.PS,
            Button2 = ControlApp_ComboButtons.SELECT,
            Button3 = ControlApp_ComboButtons.None,
        };

        // -------------------------------------------- DEFAULT SETTINGS END
        private bool isVariableLightRumbleEmulationEnabled;
        private bool isLeftMotorDisabled;
        private bool isRightMotorDisabled;
        private bool isVariableRightEmulToggleComboEnabled;
        private ButtonsCombo variableRightEmulToggleCombo = new();

        public bool IsVariableLightRumbleEmulationEnabled
        {
            get => isVariableLightRumbleEmulationEnabled;
            set
            {
                if (value) isLeftMotorDisabled = isRightMotorDisabled = false;
                isVariableLightRumbleEmulationEnabled = value;
            }
        }
        public bool IsLeftMotorDisabled
        {
            get => isLeftMotorDisabled;
            set
            {
                if (value) isVariableLightRumbleEmulationEnabled = false;
                isLeftMotorDisabled = value;
            }
        }
        public bool IsRightMotorDisabled
        {
            get => isRightMotorDisabled;
            set
            {
                if (value) isVariableLightRumbleEmulationEnabled = false;
                isRightMotorDisabled = value;
            }
        }
        public bool IsVariableRightEmulToggleComboEnabled { get => isVariableRightEmulToggleComboEnabled; set => isVariableRightEmulToggleComboEnabled = value; }
        public ButtonsCombo VariableRightEmulToggleCombo { get => variableRightEmulToggleCombo; set => variableRightEmulToggleCombo = value; }
        public void ResetToDefault()
        {
            isVariableLightRumbleEmulationEnabled = DEFAULT_isVariableLightRumbleEmulationEnabled;
            isLeftMotorDisabled = DEFAULT_isLeftMotorDisabled;
            isRightMotorDisabled = DEFAULT_isRightMotorDisabled;
            isVariableRightEmulToggleComboEnabled = DEFAULT_isVariableRightEmulToggleComboEnabled;
            variableRightEmulToggleCombo = new(DEFAULT_VariableRightEmuToggleCombo);
        }

        public static void CopySettings(BackingData_RumbleGeneral destiny, BackingData_RumbleGeneral source)
        {
            destiny.isGroupEnabled = source.IsGroupEnabled;
            destiny.isVariableLightRumbleEmulationEnabled = source.IsVariableLightRumbleEmulationEnabled;
            destiny.isLeftMotorDisabled = source.IsLeftMotorDisabled;
            destiny.isRightMotorDisabled = source.IsRightMotorDisabled;
            destiny.isVariableRightEmulToggleComboEnabled = source.IsVariableRightEmulToggleComboEnabled;
            destiny.variableRightEmulToggleCombo.copyCombo(source.VariableRightEmulToggleCombo);
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
    }

    public class BackingData_OutRepControl : SettingsBackingData
    {
        // -------------------------------------------- DEFAULT SETTINGS START

        public const bool DEFAULT_isOutputReportRateControlEnabled = true;
        public const int DEFAULT_maxOutputRate = 150;
        public const bool DEFAULT_isOutputReportDeduplicatorEnabled = false;

        // -------------------------------------------- DEFAULT SETTINGS END

        private bool isOutputReportRateControlEnabled;
        private int maxOutputRate;
        private bool isOutputReportDeduplicatorEnabled;

        public bool IsOutputReportRateControlEnabled { get => isOutputReportRateControlEnabled; set => isOutputReportRateControlEnabled = value; }
        public int MaxOutputRate { get => maxOutputRate; set => maxOutputRate = value; }
        public bool IsOutputReportDeduplicatorEnabled { get => isOutputReportDeduplicatorEnabled; set => isOutputReportDeduplicatorEnabled = value; }

        public void ResetToDefault()
        {
            isOutputReportRateControlEnabled = DEFAULT_isOutputReportRateControlEnabled;
            maxOutputRate = DEFAULT_maxOutputRate;
            isOutputReportDeduplicatorEnabled = DEFAULT_isOutputReportDeduplicatorEnabled;
        }

        public static void CopySettings(BackingData_OutRepControl destiny, BackingData_OutRepControl source)
        {
            destiny.isGroupEnabled = source.IsGroupEnabled;
            destiny.isOutputReportDeduplicatorEnabled = source.IsOutputReportDeduplicatorEnabled;
            destiny.isOutputReportRateControlEnabled = source.IsOutputReportRateControlEnabled;
            destiny.maxOutputRate = source.MaxOutputRate;
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
            dshmContextSettings.OutputRateControlPeriodMs = (byte)this.MaxOutputRate;
            dshmContextSettings.IsOutputDeduplicatorEnabled = this.IsOutputReportDeduplicatorEnabled;
        }
    }

    public class BackingData_LeftRumbleRescale : SettingsBackingData
    {
        // -------------------------------------------- DEFAULT LEFT MOTOR RESCALING GROUP SETTINGS START

        public const bool DEFAULT_isLeftMotorStrRescalingEnabled = true;
        public const int DEFAULT_leftMotorStrRescalingUpperRange = 255;
        public const int DEFAULT_leftMotorStrRescalingLowerRange = 64;

        // -------------------------------------------- DEFAULT LEFT MOTOR RESCALING GROUP SETTINGS END

        private bool isLeftMotorStrRescalingEnabled;
        private int leftMotorStrRescalingUpperRange;
        private int leftMotorStrRescalingLowerRange;

        public bool IsLeftMotorStrRescalingEnabled { get => isLeftMotorStrRescalingEnabled; set => isLeftMotorStrRescalingEnabled = value; }
        public int LeftMotorStrRescalingUpperRange
        {
            get => leftMotorStrRescalingUpperRange;
            set
            {
                int tempInt = (value < leftMotorStrRescalingLowerRange) ? leftMotorStrRescalingLowerRange + 1 : value;
                leftMotorStrRescalingUpperRange = tempInt;

            }
        }
        public int LeftMotorStrRescalingLowerRange
        {
            get => leftMotorStrRescalingLowerRange;
            set
            {
                int tempInt = (value > leftMotorStrRescalingUpperRange) ? leftMotorStrRescalingUpperRange - 1 : value;
                leftMotorStrRescalingLowerRange = tempInt;
            }
        }

        public void ResetToDefault()
        {
            isLeftMotorStrRescalingEnabled = DEFAULT_isLeftMotorStrRescalingEnabled;
            leftMotorStrRescalingUpperRange = DEFAULT_leftMotorStrRescalingUpperRange;
            leftMotorStrRescalingLowerRange = DEFAULT_leftMotorStrRescalingLowerRange;
        }

        public void PrepareForSettingsLoading()
        {
            leftMotorStrRescalingLowerRange = 1;
            leftMotorStrRescalingUpperRange = 255;
        }

        public static void CopySettings(BackingData_LeftRumbleRescale destiny, BackingData_LeftRumbleRescale source)
        {
            destiny.isGroupEnabled = source.IsGroupEnabled;
            destiny.isLeftMotorStrRescalingEnabled = source.IsLeftMotorStrRescalingEnabled;
            destiny.leftMotorStrRescalingLowerRange = source.LeftMotorStrRescalingLowerRange;
            destiny.leftMotorStrRescalingUpperRange = source.LeftMotorStrRescalingUpperRange;
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
            dshmLeftRumbleRescaleSettings.MinValue = (byte)this.LeftMotorStrRescalingLowerRange;
            dshmLeftRumbleRescaleSettings.MaxValue = (byte)this.LeftMotorStrRescalingUpperRange;
        }
    }

    public class BackingData_VariablaRightRumbleEmulAdjusts : SettingsBackingData
    {
        // -------------------------------------------- DEFAULT SETTINGS START

        public const byte DEFAULT_rightRumbleConversionUpperRange = 140;
        public const byte DEFAULT_rightRumbleConversionLowerRange = 1;
        public const bool DEFAULT_isForcedRightMotorLightThresholdEnabled = false;
        public const bool DEFAULT_isForcedRightMotorHeavyThreasholdEnabled = false;
        public const byte DEFAULT_forcedRightMotorLightThreshold = 230;
        public const byte DEFAULT_forcedRightMotorHeavyThreshold = 230;

        // -------------------------------------------- DEFAULT SETTINGS END


        private int rightRumbleConversionUpperRange;
        private int rightRumbleConversionLowerRange;
        private bool isForcedRightMotorLightThresholdEnabled;
        private bool isForcedRightMotorHeavyThreasholdEnabled;
        private int forcedRightMotorLightThreshold;
        private int forcedRightMotorHeavyThreshold;

        public int RightRumbleConversionUpperRange
        {
            get => rightRumbleConversionUpperRange;
            set
            {
                int tempInt = (value < rightRumbleConversionLowerRange) ? rightRumbleConversionLowerRange + 1 : value;
                rightRumbleConversionUpperRange = tempInt;
            }
        }
        public int RightRumbleConversionLowerRange
        {
            get => rightRumbleConversionLowerRange;
            set
            {
                int tempInt = (value > rightRumbleConversionUpperRange) ? (byte)(rightRumbleConversionUpperRange - 1) : value;
                rightRumbleConversionLowerRange = tempInt;
            }
        }
        public int ForcedRightMotorHeavyThreshold { get => forcedRightMotorHeavyThreshold; set => forcedRightMotorHeavyThreshold = value; }
        public int ForcedRightMotorLightThreshold { get => forcedRightMotorLightThreshold; set => forcedRightMotorLightThreshold = value; }
        public bool IsForcedRightMotorHeavyThreasholdEnabled { get => isForcedRightMotorHeavyThreasholdEnabled; set => isForcedRightMotorHeavyThreasholdEnabled = value; }
        public bool IsForcedRightMotorLightThresholdEnabled { get => isForcedRightMotorLightThresholdEnabled; set => isForcedRightMotorLightThresholdEnabled = value; }

        public void ResetToDefault()
        {
            rightRumbleConversionUpperRange = DEFAULT_rightRumbleConversionUpperRange;
            rightRumbleConversionLowerRange = DEFAULT_rightRumbleConversionLowerRange;
            isForcedRightMotorLightThresholdEnabled = DEFAULT_isForcedRightMotorLightThresholdEnabled;
            isForcedRightMotorHeavyThreasholdEnabled = DEFAULT_isForcedRightMotorHeavyThreasholdEnabled;
            forcedRightMotorLightThreshold = DEFAULT_forcedRightMotorLightThreshold;
            forcedRightMotorHeavyThreshold = DEFAULT_forcedRightMotorHeavyThreshold;
        }

        public static void CopySettings(BackingData_VariablaRightRumbleEmulAdjusts destiny, BackingData_VariablaRightRumbleEmulAdjusts source)
        {
            destiny.PrepareForSettingsLoading();
            destiny.IsGroupEnabled = source.IsGroupEnabled;
            destiny.RightRumbleConversionLowerRange = source.RightRumbleConversionLowerRange;
            destiny.RightRumbleConversionUpperRange = source.RightRumbleConversionUpperRange;
            // Right rumble (light) threshold
            destiny.IsForcedRightMotorLightThresholdEnabled = source.IsForcedRightMotorLightThresholdEnabled;
            destiny.ForcedRightMotorLightThreshold = source.ForcedRightMotorLightThreshold;
            // Left rumble (Heavy) threshold
            destiny.IsForcedRightMotorHeavyThreasholdEnabled = source.IsForcedRightMotorHeavyThreasholdEnabled;
            destiny.ForcedRightMotorHeavyThreshold = source.ForcedRightMotorHeavyThreshold;
        }

        public void PrepareForSettingsLoading()
        {
            rightRumbleConversionLowerRange = 1;
            rightRumbleConversionUpperRange = 255;
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
            dshmSMConversionSettings.RescaleMinValue = (byte)this.RightRumbleConversionLowerRange;
            dshmSMConversionSettings.RescaleMaxValue = (byte)this.RightRumbleConversionUpperRange;

            // Right rumble (light) threshold
            dshmForcedSMSettings.SMThresholdEnabled = this.IsForcedRightMotorLightThresholdEnabled;
            dshmForcedSMSettings.SMThresholdValue = (byte)this.ForcedRightMotorLightThreshold;

            // Left rumble (Heavy) threshold
            dshmForcedSMSettings.BMThresholdEnabled = this.IsForcedRightMotorHeavyThreasholdEnabled;
            dshmForcedSMSettings.BMThresholdValue = (byte)this.ForcedRightMotorHeavyThreshold;
        }
    }
}

/*
public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
{
if ((this.Context == SettingsContext.General) && (dshmContextSettings.HIDDeviceMode != null))
this.HIDDeviceMode = dshmContextSettings.HIDDeviceMode.GetValueOrDefault();
else
this.HIDDeviceMode = DSHM_HidDeviceModes.XInput;


dshmContextSettings.PressureExposureMode =
(this.Context == SettingsContext.SDF
|| this.Context == SettingsContext.GPJ)
? SaveLoadUtils.Get_DSHM_DsPressureMode_From_ControlApp[this.PressureExposureMode] : null;

dshmContextSettings.DPadExposureMode =
(this.Context == SettingsContext.SDF
|| this.Context == SettingsContext.GPJ)
? SaveLoadUtils.Get_DSHM_DPadMode_From_ControlApp[this.DPadExposureMode] : null;
}
*/

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

/*Load
public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
{
if(
dshmContextSettings.DisableWirelessIdleTimeout == null
// check for combo too
|| dshmContextSettings.WirelessIdleTimeoutPeriodMs == null)
{
this.IsGroupEnabled = false;
return;
}
this.IsGroupEnabled = true;

this.IsWirelessIdleDisconnectEnabled = !dshmContextSettings.DisableWirelessIdleTimeout.GetValueOrDefault();
this.WirelessIdleDisconnectTime =
(byte)(dshmContextSettings.WirelessIdleTimeoutPeriodMs.GetValueOrDefault() / (60.0 * 1000) );
// this.combo...
}
*/


/*Load
public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
{
DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;

// Checks if the "prevent conflicts" properties exist
// They will only exist if the settings were saved in their respective HID Device Mode
// If they exist and are true then we can skip the rest of the loading because this group will be disabled
bool preventDS4WConflicts = dshmContextSettings.PreventRemappingConflitsInDS4WMode == null ? true : dshmContextSettings.PreventRemappingConflitsInSXSMode.GetValueOrDefault();
bool preventSXSConflicts = dshmContextSettings.PreventRemappingConflitsInSXSMode == null ? false : dshmContextSettings.PreventRemappingConflitsInSXSMode.GetValueOrDefault();
if (preventDS4WConflicts
|| preventSXSConflicts)
{
return;
}


if (dshmLeftDZSettings == null
|| dshmRightDZSettings == null)
{
this.IsGroupEnabled = false;
return;
}

// left
this.ApplyLeftStickDeadZone = dshmLeftDZSettings.Apply.GetValueOrDefault();
this.LeftStickDeadZone = dshmLeftDZSettings.PolarValue.GetValueOrDefault();

// Right
this.ApplyRightStickDeadZone = dshmRightDZSettings.Apply.GetValueOrDefault();
this.RightStickDeadZone = dshmRightDZSettings.PolarValue.GetValueOrDefault();


}
*/



/* Not necessary anymore I think
if(SettingsContainer.GroupModeUnique.PreventRemappingConflictsInSXSMode)
{
    dshmRumbleSettings.SMToBMConversion.Enabled = false;
    dshmRumbleSettings.DisableBM = false;
    dshmRumbleSettings.DisableSM = false;
    return;
}
*/

/*
public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
{
    DSHM_Format_ContextSettings.AllRumbleSettings dshmRumbleSettings = dshmContextSettings.RumbleSettings;

    //Not necessary anymore I think ------------
    bool tempPreventSXSConflicts =
        dshmContextSettings.PreventRemappingConflitsInSXSMode == null ?
        false : dshmContextSettings.PreventRemappingConflitsInSXSMode.GetValueOrDefault();
    if (tempPreventSXSConflicts) return;
    // End --------------------------------------

    if (
        dshmRumbleSettings.SMToBMConversion.Enabled == null
        || dshmRumbleSettings.DisableBM == null
        || dshmRumbleSettings.DisableSM == null
       )
    {
        this.IsGroupEnabled = false;
        return;
    }
    this.IsGroupEnabled = true;

    this.IsVariableLightRumbleEmulationEnabled = dshmRumbleSettings.SMToBMConversion.Enabled.GetValueOrDefault();
    this.IsLeftMotorDisabled = dshmRumbleSettings.DisableBM.GetValueOrDefault();
    this.IsLeftMotorDisabled = dshmRumbleSettings.DisableSM.GetValueOrDefault();
}
*/

/*
public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
{
    if(dshmContextSettings.IsOutputRateControlEnabled == null
        || dshmContextSettings.OutputRateControlPeriodMs == null
        || dshmContextSettings.IsOutputDeduplicatorEnabled == null)
    {
        this.IsGroupEnabled = false;
        return;
    }
    this.IsGroupEnabled = true;

    this.IsOutputReportRateControlEnabled = dshmContextSettings.IsOutputRateControlEnabled.GetValueOrDefault();
    this.MaxOutputRate = dshmContextSettings.OutputRateControlPeriodMs.GetValueOrDefault();
    this.IsOutputReportDeduplicatorEnabled = dshmContextSettings.IsOutputDeduplicatorEnabled.GetValueOrDefault();
}
*/
/*
public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
{
    DSHM_Format_ContextSettings.BMStrRescaleSettings dshmLeftRumbleRescaleSettings = dshmContextSettings.RumbleSettings.BMStrRescale;

    if (dshmLeftRumbleRescaleSettings == null)
    {
        this.IsGroupEnabled = false;
        return;
    }
    this.IsGroupEnabled = true;

    this.IsLeftMotorStrRescalingEnabled = dshmLeftRumbleRescaleSettings.Enabled.GetValueOrDefault();
    this.LeftMotorStrRescalingLowerRange = dshmLeftRumbleRescaleSettings.MinValue.GetValueOrDefault();
    this.LeftMotorStrRescalingUpperRange = dshmLeftRumbleRescaleSettings.MaxValue.GetValueOrDefault();

}
*/
/*
public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
{
    DSHM_Format_ContextSettings.SMToBMConversionSettings dshmSMConversionSettings = dshmContextSettings.RumbleSettings.SMToBMConversion;
    DSHM_Format_ContextSettings.ForcedSMSettings dshmForcedSMSettings = dshmContextSettings.RumbleSettings.ForcedSM;

    if (dshmSMConversionSettings.RescaleMinValue == null
        || dshmSMConversionSettings.RescaleMaxValue == null
        || dshmForcedSMSettings == null
        )
    {
        this.IsGroupEnabled = false;
        return;
    }

    // Right rumble conversion rescaling adjustment
    this.RightRumbleConversionLowerRange = dshmSMConversionSettings.RescaleMinValue.GetValueOrDefault();
    this.RightRumbleConversionUpperRange = dshmSMConversionSettings.RescaleMaxValue.GetValueOrDefault();

    // Right rumble (light) threshold
    this.IsForcedRightMotorLightThresholdEnabled = dshmForcedSMSettings.SMThresholdEnabled.GetValueOrDefault();
    this.ForcedRightMotorLightThreshold = dshmForcedSMSettings.SMThresholdValue.GetValueOrDefault();


    // Left rumble (Heavy) threshold
    this.IsForcedRightMotorHeavyThreasholdEnabled = dshmForcedSMSettings.BMThresholdEnabled.GetValueOrDefault();
    this.ForcedRightMotorHeavyThreshold = dshmForcedSMSettings.BMThresholdValue.GetValueOrDefault();
}
*/
