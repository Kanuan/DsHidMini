using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.JsonSettings
{
    public class DsHidMini_JsonFormatSettings
    {
        public bool? DisableWirelessIdleTimeout { get; set; } = false;
        public double? WirelessIdleTimeoutPeriodMs { get; set; } = 300000;

        public bool? IsQuickDisconnectComboEnabled { get; set; } = true;
        public DSHM_QuickDisconnectCombo? QuickDisconnectCombo { get; set; } = DSHM_QuickDisconnectCombo.PS_R1_L1;

        public bool? IsOutputRateControlEnabled { get; set; } = true;
        public byte? OutputRateControlPeriodMs { get; set; } = 150;
        public bool? IsOutputDeduplicatorEnabled { get; set; } = false;
        public DSHM_PressureModes? PressureExposureMode { get; set; } = DSHM_PressureModes.Default;
        public DSHM_DPadExposureModes? DPadExposureMode { get; set; } = DSHM_DPadExposureModes.Default;
        public DeadZoneSettings DeadZoneLeft { get; set; } = new();
        public DeadZoneSettings DeadZoneRight { get; set; } = new();
        public AllRumbleSettings RumbleSettings { get; set; } = new();
        public AllLEDSettings LEDSettings { get; set; } = new();


        public class DeadZoneSettings
        {
            public bool Apply { get; set; } = true;
            public double PolarValue { get; set; } = 10.0;

        }

        public class BMStrRescaleSettings
        {
            public bool Enabled { get; set; } = true;
            public byte MinValue { get; set; } = 64;
            public byte MaxValue { get; set; } = 255;
        }

        public class SMToBMConversionSettings
        {
            public bool Enabled { get; set; } = false;
            public byte RescaleMinValue { get; set; } = 1;
            public byte RescaleMaxValue { get; set; } = 160;
            public bool IsSMToBMConversionToggleComboEnabled { get; set; } = false;

        }

        public class ForcedSMSettings
        {
            public bool BMThresholdEnabled { get; set; } = false;
            public byte BMThresholdValue { get; set; } = 230;
            public bool SMThresholdEnabled { get; set; } = false;
            public byte SMThresholdValue { get; set; } = 230;
        }

        public class AllRumbleSettings
        {
            public bool DisableBM { get; set; } = false;
            public bool DisableSM { get; set; } = false;
            public BMStrRescaleSettings BMStrRescale { get; set; } = new();
            public SMToBMConversionSettings SMToBMConversion { get; set; } = new();
            public ForcedSMSettings ForcedSM { get; set; } = new();
        }

        public class SingleLEDCustoms
        {
            public byte? Duration { get; set; } = 255;
            public byte? IntervalDuration { get; set; } = 255;
            public byte? Enabled { get; set; } = 0x10;
            public byte? IntervalPortionOff { get; set; } = 0;
            public byte? IntervalPortionOn { get; set; } = 255;
        }

        public class AllLEDSettings
        {
            public DSHM_LEDsModes? Mode { get; set; } = DSHM_LEDsModes.BatteryIndicatorPlayerIndex;
            public SingleLEDCustoms Player1 { get; set; } = new();
            public SingleLEDCustoms Player2 { get; set; } = new();
            public SingleLEDCustoms Player3 { get; set; } = new();
            public SingleLEDCustoms Player4 { get; set; } = new();
        }
    }

    public class DSHMSettingsFormat
    {
        public DSHM_HidDeviceModes HidDeviceMode { get; set; } = DSHM_HidDeviceModes.DS4Windows;
        public bool DisableAutoPairing { get; set; } = false;
        public DsHidMini_JsonFormatSettings General { get; set; } = new();
        public DsHidMini_JsonFormatSettings SDF { get; set; }
        public DsHidMini_JsonFormatSettings GPJ { get; set; }
        public DsHidMini_JsonFormatSettings SXS { get; set; }
        public DsHidMini_JsonFormatSettings DS4Windows { get; set; }
        public DsHidMini_JsonFormatSettings XInput { get; set; }
    }

    public class SaveLoadJson
    {
        DeviceModesSettings controlAppSettings { get; set; }
        DSHMSettingsFormat dshmSettings { get; set; }

        public bool SaveSpecificContextSettings(DeviceModesSettings controlAppSettings, DSHMSettingsFormat dshmSettings, SettingsContext context)
        {
            return true;
        }

        public void SaveGroupLEDsCustoms(GroupLEDsControl group, DsHidMini_JsonFormatSettings dshmContextSettings)
        {
            DsHidMini_JsonFormatSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.LEDSettings;
            if(!group.IsGroupLEDsCustomizationEnabled)
            {
                dshm_AllLEDsSettings = null;
                return;
            }

            dshm_AllLEDsSettings.Mode = SaveLoadUtils.LEDModes_Control_DSHM_Pair[group.LEDMode];

            var dshm_singleLED = new DsHidMini_JsonFormatSettings.SingleLEDCustoms[]
            { dshm_AllLEDsSettings.Player1, dshm_AllLEDsSettings.Player2,dshm_AllLEDsSettings.Player3,dshm_AllLEDsSettings.Player4, };
            
            for(int i = 0; i < 4; i++)
            {
                switch(group.LEDMode)
                {
                    case ControlApp_LEDsModes.CustomPattern:
                        dshm_singleLED[i].Enabled = group.LEDsCustoms[i].IsLEDEnabled ? (byte)0x10 : (byte)0x00;
                        dshm_singleLED[i].Duration = group.LEDsCustoms[i].Duration;
                        dshm_singleLED[i].IntervalDuration = group.LEDsCustoms[i].IntervalDuration;
                        dshm_singleLED[i].IntervalPortionOn = group.LEDsCustoms[i].IntervalPortionON;
                        dshm_singleLED[i].IntervalPortionOff = group.LEDsCustoms[i].IntervalPortionOFF;
                        break;
                    case ControlApp_LEDsModes.CustomStatic:
                        dshm_singleLED[i].Enabled = group.LEDsCustoms[i].IsLEDEnabled ? (byte)0x10 : (byte)0x00;
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

        public void SaveGroupWireless(GroupWireless group, DsHidMini_JsonFormatSettings dshmContextSettings)
        {

            if (!group.IsGroupWirelessSettingsEnabled)
            {
                dshmContextSettings.DisableWirelessIdleTimeout = null;
                dshmContextSettings.WirelessIdleTimeoutPeriodMs = null;  
                //dshmContextSettings.QuickDisconnectCombo = null;
                return;
            }

            dshmContextSettings.DisableWirelessIdleTimeout = !group.IsWirelessIdleDisconnectEnabled;
            dshmContextSettings.WirelessIdleTimeoutPeriodMs = group.WirelessIdleDisconnectTime * 60 * 1000;
            //dshmContextSettings.QuickDisconnectCombo = dictionary combo pair;
        }
    }
}

