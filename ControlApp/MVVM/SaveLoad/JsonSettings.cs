﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.JsonSettings
{
    public class DSHM_Format_ContextSettings
    {
        public DSHM_HidDeviceModes? HIDDeviceMode { get; set; }// = DSHM_HidDeviceModes.DS4Windows;
        public bool? EnableDS4WLightbarTranslation { get; set; } // = false;
        public bool? PreventRemappingConflitsInDS4WMode { get; set; } // = true;
        public bool? PreventRemappingConflitsInSXSMode { get; set; } // = true;
        public DSHM_PressureModes? PressureExposureMode { get; set; }// = DSHM_PressureModes.Default;
        public DSHM_DPadExposureModes? DPadExposureMode { get; set; }// = DSHM_DPadExposureModes.Default;
        public bool? DisableWirelessIdleTimeout { get; set; }// = false;
        public double? WirelessIdleTimeoutPeriodMs { get; set; }// = 300000;

        public bool? IsQuickDisconnectComboEnabled { get; set; }// = true;
        public DSHM_QuickDisconnectCombo? QuickDisconnectCombo { get; set; }// = DSHM_QuickDisconnectCombo.PS_R1_L1;

        public bool? IsOutputRateControlEnabled { get; set; }// = true;
        public byte? OutputRateControlPeriodMs { get; set; }// = 150;
        public bool? IsOutputDeduplicatorEnabled { get; set; }// = false;

        public DeadZoneSettings DeadZoneLeft { get; set; } = new();
        public DeadZoneSettings DeadZoneRight { get; set; } = new();
        public AllRumbleSettings RumbleSettings { get; set; } = new();
        public AllLEDSettings LEDSettings { get; set; } = new();


        public class DeadZoneSettings
        {
            public bool? Apply
            {
                get;
                set;
            }
            public byte? PolarValue { get; set; }// = 10.0;

        }

        public class BMStrRescaleSettings
        {
            public bool? Enabled { get; set; }// = true;
            public byte? MinValue { get; set; }// = 64;
            public byte? MaxValue { get; set; }// = 255;
        }

        public class SMToBMConversionSettings
        {
            public bool? Enabled { get; set; }// = false;
            public byte? RescaleMinValue { get; set; }// = 1;
            public byte? RescaleMaxValue { get; set; }// = 160;
            public bool? IsSMToBMConversionToggleComboEnabled { get; set; }// = false;

        }

        public class ForcedSMSettings
        {
            public bool? BMThresholdEnabled { get; set; }// = false;
            public byte? BMThresholdValue { get; set; }// = 230;
            public bool? SMThresholdEnabled { get; set; }// = false;
            public byte? SMThresholdValue { get; set; }// = 230;
        }

        public class AllRumbleSettings
        {
            public bool? DisableBM { get; set; }// = false;
            public bool? DisableSM { get; set; }// = false;
            public BMStrRescaleSettings BMStrRescale { get; set; } = new();
            public SMToBMConversionSettings SMToBMConversion { get; set; } = new();
            public ForcedSMSettings ForcedSM { get; set; } = new();
        }

        public class SingleLEDCustoms
        {
            public byte? Duration { get; set; }// = 255;
            public byte? IntervalDuration { get; set; }// = 255;
            public byte? Enabled { get; set; }// = 0x10;
            public byte? IntervalPortionOff { get; set; }// = 0;
            public byte? IntervalPortionOn { get; set; }// = 255;
        }

        public class AllLEDSettings
        {
            public DSHM_LEDsModes? Mode { get; set; }// = DSHM_LEDsModes.BatteryIndicatorPlayerIndex;
            public SingleLEDCustoms Player1 { get; set; } = new();
            public SingleLEDCustoms Player2 { get; set; } = new();
            public SingleLEDCustoms Player3 { get; set; } = new();
            public SingleLEDCustoms Player4 { get; set; } = new();
        }
    }

    public class DSHM_Format_Settings
    {
        public DSHM_HidDeviceModes HidDeviceMode { get; set; }// = DSHM_HidDeviceModes.DS4Windows;
        public bool DisableAutoPairing { get; set; }// = false;
        public DSHM_Format_ContextSettings General { get; set; } = new();
        //public DSHM_Format_ContextSettings SDF { get; set; } = new();
        //public DSHM_Format_ContextSettings GPJ { get; set; } = new();
        //public DSHM_Format_ContextSettings SXS { get; set; } = new();
        //public DSHM_Format_ContextSettings DS4Windows { get; set; } = new();
        //public DSHM_Format_ContextSettings XInput { get; set; } = new();
    }

    public class TestNewSaveFormat
    {
        public string DefaultProfile { get; set; } = "Global";
        public List<TestSingleSettingsContainer> Profiles { get; set; } = new();
        public List<TestSingleSettingsContainer> Devices { get; set; } = new();

        public TestNewSaveFormat()
        {

        }
    }

    public class TestSingleSettingsContainer
    {
        public string? ProfileToUse { get; set; }
        public DSHM_Format_ContextSettings Settings { get; set; } = new();
    }

    public static class SaveLoadJson
    {
        //DeviceModesSettings controlAppSettings { get; set; }
        //DSHM_Format_Settings dshmSettings { get; set; }

        /*
        public bool SaveSpecificContextSettings(DeviceModesSettings controlAppSettings, DSHM_Format_Settings dshmSettings, SettingsContext context)
        
            return true;
        }
    */

        public static string SaveToDSHMFormattedJson(DSHM_Format_Settings dshmFormattedSettings)
        {

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            string jsonString = JsonSerializer.Serialize(dshmFormattedSettings, options);

            

            return jsonString;
        }

    }
}

