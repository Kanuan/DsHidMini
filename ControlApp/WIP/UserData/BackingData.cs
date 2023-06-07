using JetBrains.Annotations;
using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.MVVM;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.UserData
{
    public class ButtonsCombo
    {

        private ControlApp_ComboButtons button1;
        private ControlApp_ComboButtons button2;
        private ControlApp_ComboButtons button3;

        public ControlApp_ComboButtons Button1
        {
            get => button1;
            set
            {
                if (value != button2 && value != button3)
                    button1 = value;
            }
        }
        public ControlApp_ComboButtons Button2
        {
            get => button2;
            set
            {
                if (value != button1 && value != button3)
                    button2 = value;
            }
        }
        public ControlApp_ComboButtons Button3
        {
            get => button3;
            set
            {
                if (value != button1 && value != button2)
                    button3 = value;
            }
        }

        public ButtonsCombo() { }

        public ButtonsCombo(ButtonsCombo comboToCopy)
        {
            copyCombo(comboToCopy);
        }

        public void copyCombo(ButtonsCombo comboToCopy)
        {
            Button1 = comboToCopy.Button1;
            Button2 = comboToCopy.Button2;
            Button3 = comboToCopy.Button3;
        }

    }
    
    public class BackingDataContainer
    {

        public BackingData_ModesUnique modesUniqueData { get; set; } = new();
        public BackingData_LEDs ledsData { get; set; } = new();
        public BackingData_Wireless wirelessData { get; set; } = new();
        public BackingData_Sticks sticksData { get; set; } = new();
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
            sticksData.ResetToDefault();
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
            sticksData.SaveToDSHMSettings(modeContext);
            rumbleGeneralData.SaveToDSHMSettings(modeContext);
            outRepData.SaveToDSHMSettings(dshm_data);
            leftRumbleRescaleData.SaveToDSHMSettings(modeContext);
            rightVariableEmulData.SaveToDSHMSettings(modeContext);

            if (modesUniqueData.SettingsContext == SettingsContext.DS4W)
            {
                dshm_data.DeadZoneLeft.Apply = false;
                dshm_data.DeadZoneRight.Apply = false;
            }
        }
    }

    public abstract class SettingsBackingData
    {
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
        public bool AllowAppsToOverrideLEDsInSXSMode { get; set; }

        public void ResetToDefault()
        {
            SettingsContext = SettingsContext.XInput;
            PressureExposureMode = ControlApp_DsPressureMode.Default;
            DPadExposureMode = ControlApp_DPADModes.HAT;
            IsLEDsAsXInputSlotEnabled = false;
            IsDS4LightbarTranslationEnabled = false;
            PreventRemappingConflictsInSXSMode = false;
            PreventRemappingConflictsInDS4WMode = true;
            AllowAppsToOverrideLEDsInSXSMode = false;
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
            destiny.AllowAppsToOverrideLEDsInSXSMode = source.AllowAppsToOverrideLEDsInSXSMode;
        }


        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            if (SettingsContext != SettingsContext.General)
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

            dshmContextSettings.SXS.LEDSettings.Authority = this.AllowAppsToOverrideLEDsInSXSMode ? DSHM_LEDsAuthority.Application : DSHM_LEDsAuthority.Driver;
            dshmContextSettings.DS4Windows.LEDSettings.Authority = this.IsDS4LightbarTranslationEnabled ? DSHM_LEDsAuthority.Application : DSHM_LEDsAuthority.Driver;
        }
    }

    public class BackingData_LEDs : SettingsBackingData
    {
        public ControlApp_LEDsModes LEDMode { get; set; }
        //public bool[] LEDFlags { get; set; } = new bool[4];
        public All4LEDsCustoms LEDsCustoms { get; set; } = new();

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
            destiny.LEDMode = source.LEDMode;
            destiny.LEDsCustoms.CopyLEDsCustoms(source.LEDsCustoms);
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.LEDSettings;

            dshm_AllLEDsSettings.Mode = SaveLoadUtils.Get_DSHM_LEDModes_From_ControlApp[this.LEDMode];

            var dshm_Customs = dshm_AllLEDsSettings.CustomPatterns;

            var dshm_singleLED = new DSHM_Format_ContextSettings.SingleLEDCustoms[]
            { dshm_Customs.Player1, dshm_Customs.Player2,dshm_Customs.Player3,dshm_Customs.Player4, };


            dshm_Customs.LEDFlags = 0;
            for (int i = 0; i < LEDsCustoms.LED_x_Customs.Length; i++)
            {
                All4LEDsCustoms.singleLEDCustoms singleLEDCustoms = this.LEDsCustoms.LED_x_Customs[i];

                if (singleLEDCustoms.IsLedEnabled)
                {
                    dshm_Customs.LEDFlags |= (byte)(1 << (1 + i));
                }

                switch (this.LEDMode)
                {
                    case ControlApp_LEDsModes.CustomPattern:
                        dshm_singleLED[i].EnabledFlags = singleLEDCustoms.UseLEDEffects ? (byte)0x10 : (byte)0x00;
                        dshm_singleLED[i].Duration = singleLEDCustoms.Duration;
                        dshm_singleLED[i].IntervalDuration = singleLEDCustoms.IntervalDuration;
                        dshm_singleLED[i].IntervalPortionOn = singleLEDCustoms.IntervalPortionON;
                        dshm_singleLED[i].IntervalPortionOff = singleLEDCustoms.IntervalPortionOFF;
                        break;
                    case ControlApp_LEDsModes.CustomStatic:
                        dshm_singleLED[i].EnabledFlags = 0x00; // false
                        /*
                        dshm_singleLED[i].Duration = null;
                        dshm_singleLED[i].IntervalDuration = null;
                        dshm_singleLED[i].IntervalPortionOn = null;
                        dshm_singleLED[i].IntervalPortionOff = null;
                        */
                        break;
                    case ControlApp_LEDsModes.BatteryIndicatorPlayerIndex:
                    case ControlApp_LEDsModes.BatteryIndicatorBarGraph:
                    default:
                        dshm_singleLED[i] = null;
                        break;
                }
            }
        }

        public class All4LEDsCustoms
        {
            public singleLEDCustoms[] LED_x_Customs = new singleLEDCustoms[4];
            public All4LEDsCustoms()
            {
                for (int i = 0; i < LED_x_Customs.Length; i++)
                {
                    LED_x_Customs[i] = new(i);
                }
            }

            public void CopyLEDsCustoms(All4LEDsCustoms customsToCopy)
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
                //private byte DEFAULT_intervalPortionOFF = 0x00;

                [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
                public int LEDIndex { get; }
                public bool IsLedEnabled { get; set; } = false;
                public bool UseLEDEffects { get; set; }

                public byte Duration { get; set; }
                public byte IntervalDuration { get; set; }
                public byte IntervalPortionON { get; set; }
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
                    IsLedEnabled = LEDIndex == 0 ? true : false;
                    UseLEDEffects = false;
                    Duration = DEFAULT_duration;
                    IntervalDuration = DEFAULT_intervalDuration;
                    IntervalPortionON = DEFAULT_intervalPortionON;
                    //IntervalPortionOFF = DEFAULT_intervalPortionOFF;
                }

                public void CopyCustoms(singleLEDCustoms copySource)
                {
                    this.IsLedEnabled = copySource.IsLedEnabled;
                    this.UseLEDEffects = copySource.UseLEDEffects;
                    this.Duration = copySource.Duration;
                    this.IntervalDuration = copySource.IntervalDuration;
                    this.IntervalPortionON = copySource.IntervalPortionON;
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
            destiny.IsQuickDisconnectComboEnabled = source.IsQuickDisconnectComboEnabled;
            destiny.IsWirelessIdleDisconnectEnabled = source.IsWirelessIdleDisconnectEnabled;
            destiny.QuickDisconnectCombo.copyCombo(source.QuickDisconnectCombo);
            destiny.WirelessIdleDisconnectTime = source.WirelessIdleDisconnectTime;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            dshmContextSettings.DisableWirelessIdleTimeout = !this.IsWirelessIdleDisconnectEnabled;
            dshmContextSettings.WirelessIdleTimeoutPeriodMs = this.WirelessIdleDisconnectTime * 60 * 1000;
            //dshmContextSettings.QuickDisconnectCombo = dictionary combo pair;
        }
    }

    public class BackingData_Sticks : SettingsBackingData
    {
        public StickData LeftStickData { get; set; } = new();
        public StickData RightStickData { get; set; } = new();

        public void ResetToDefault()
        {
            LeftStickData.Reset();
            RightStickData.Reset();
        }

        public static void CopySettings(BackingData_Sticks destiny, BackingData_Sticks source)
        {
            destiny.LeftStickData.CopyStickDataFromOtherStick(source.LeftStickData);
            destiny.RightStickData.CopyStickDataFromOtherStick(source.RightStickData);
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
            DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;
            DSHM_Format_ContextSettings.AxesFlipping axesFlipping = dshmContextSettings.FlipAxis;


            dshmLeftDZSettings.Apply = this.LeftStickData.IsDeadZoneEnabled;
            dshmLeftDZSettings.PolarValue = (byte)(this.LeftStickData.DeadZone * 181 / 142);
            axesFlipping.LeftX = this.LeftStickData.InvertXAxis;
            axesFlipping.LeftY = this.LeftStickData.InvertYAxis;

            dshmRightDZSettings.Apply = this.RightStickData.IsDeadZoneEnabled;
            dshmRightDZSettings.PolarValue = (byte)(this.RightStickData.DeadZone * 181 / 142);
            axesFlipping.RightX = this.RightStickData.InvertXAxis;
            axesFlipping.RightY = this.RightStickData.InvertYAxis;
        }

        public class StickData
        {
            public bool IsDeadZoneEnabled { get; set; }
            public int DeadZone { get; set; }

            public bool InvertXAxis { get; set; }
            public bool InvertYAxis { get; set; }

            public StickData()
            {
                Reset();
            }

            public void Reset()
            {
                IsDeadZoneEnabled = true;
                DeadZone = 0;
                InvertXAxis = false;
                InvertYAxis = false;
            }

            public void CopyStickDataFromOtherStick(StickData copySource)
            {
                this.IsDeadZoneEnabled = copySource.IsDeadZoneEnabled;
                this.DeadZone = copySource.DeadZone;
                this.InvertXAxis = copySource.InvertXAxis;
                this.InvertYAxis = copySource.InvertYAxis;
            }

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
            destiny.isVariableLightRumbleEmulationEnabled = source.IsVariableLightRumbleEmulationEnabled;
            destiny.isLeftMotorDisabled = source.IsLeftMotorDisabled;
            destiny.isRightMotorDisabled = source.IsRightMotorDisabled;
            destiny.isVariableRightEmulToggleComboEnabled = source.IsVariableRightEmulToggleComboEnabled;
            destiny.variableRightEmulToggleCombo.copyCombo(source.VariableRightEmulToggleCombo);
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllRumbleSettings dshmRumbleSettings = dshmContextSettings.RumbleSettings;

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
            destiny.isOutputReportDeduplicatorEnabled = source.IsOutputReportDeduplicatorEnabled;
            destiny.isOutputReportRateControlEnabled = source.IsOutputReportRateControlEnabled;
            destiny.maxOutputRate = source.MaxOutputRate;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
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
            destiny.isLeftMotorStrRescalingEnabled = source.IsLeftMotorStrRescalingEnabled;
            destiny.leftMotorStrRescalingLowerRange = source.LeftMotorStrRescalingLowerRange;
            destiny.leftMotorStrRescalingUpperRange = source.LeftMotorStrRescalingUpperRange;
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.BMStrRescaleSettings dshmLeftRumbleRescaleSettings = dshmContextSettings.RumbleSettings.BMStrRescale;

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
