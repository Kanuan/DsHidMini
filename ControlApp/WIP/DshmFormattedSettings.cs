using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;
using static Nefarius.DsHidMini.ControlApp.DSHM_Settings.DSHM_Format_Settings;

namespace Nefarius.DsHidMini.ControlApp.DSHM_Settings
{
    public class DSHM_Format_Settings
    {
        public DSHM_HidDeviceModes? HIDDeviceMode { get; set; }// = DSHM_HidDeviceModes.DS4Windows;
        public bool? EnableDS4WLightbarTranslation { get; set; } // = false;
        public bool? PreventRemappingConflitsInDS4WMode { get; set; } // = true;
        public bool? PreventRemappingConflitsInSXSMode { get; set; } // = true;
        public bool? DisableWirelessIdleTimeout { get; set; }// = false;
        public bool? IsOutputRateControlEnabled { get; set; }// = true;
        public byte? OutputRateControlPeriodMs { get; set; }// = 150;
        public bool? IsOutputDeduplicatorEnabled { get; set; }// = false;
        public double? WirelessIdleTimeoutPeriodMs { get; set; }// = 300000;
        public bool? IsQuickDisconnectComboEnabled { get; set; }// = true;
        public DSHM_QuickDisconnectCombo? QuickDisconnectCombo { get; set; }// = DSHM_QuickDisconnectCombo.PS_R1_L1

        [JsonIgnore]
        public DSHM_Format_ContextSpecificSettings ContextSettings { get; set; } = new();
        public DSHM_Format_ContextSpecificSettings SDF { get; set; }
        public DSHM_Format_ContextSpecificSettings GPJ { get; set; }
        public DSHM_Format_ContextSpecificSettings SXS { get; set; }
        public DSHM_Format_ContextSpecificSettings DS4Windows { get; set; }
        public DSHM_Format_ContextSpecificSettings XInput { get; set; }

        public DSHM_Format_Settings()
        {

        }

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
            public byte? EnabledFlags { get; set; }// = 0x10;
            public byte? IntervalPortionOff { get; set; }// = 0;
            public byte? IntervalPortionOn { get; set; }// = 255;
        }

        public class AllLEDSettings
        {
            public DSHM_LEDsModes? Mode { get; set; }// = DSHM_LEDsModes.BatteryIndicatorPlayerIndex;
            public DSHM_LEDsAuthority? Authority { get; set; }
            public LEDsCustoms CustomPatterns { get; set; } = new();
        }

        public class LEDsCustoms
        {
            public byte? LEDFlags { get; set; } // = 0x2;
            public SingleLEDCustoms Player1 { get; set; } = new();
            public SingleLEDCustoms Player2 { get; set; } = new();
            public SingleLEDCustoms Player3 { get; set; } = new();
            public SingleLEDCustoms Player4 { get; set; } = new();
        }

        public class AxesFlipping
        {
            public bool? LeftX { get; set; }
            public bool? LeftY { get; set; }
            public bool? RightX { get; set; }
            public bool? RightY { get; set; }
        }
    }

    public class DSHM_Format_ContextSpecificSettings
    {
        public DSHM_PressureModes? PressureExposureMode { get; set; }// = DSHM_PressureModes.Default;
        public DSHM_DPadExposureModes? DPadExposureMode { get; set; }// = DSHM_DPadExposureModes.Default;
        public DeadZoneSettings DeadZoneLeft { get; set; } = new();
        public DeadZoneSettings DeadZoneRight { get; set; } = new();
        public AllRumbleSettings RumbleSettings { get; set; } = new();
        public AllLEDSettings LEDSettings { get; set; } = new();
        public AxesFlipping FlipAxis { get; set; } = new();
    }

    /// <summary>
    /// WIP: Json-serializalying an object from this class and saving it to disk results in a file with the appropriate contents to be loaded by the DsHidMini v3 driver
    /// </summary>
    public class DshmMainDataContainer
    {
        public DSHM_Format_Settings Global { get; set; } = new();
        public List<DSHMDeviceCustomSettings> Devices { get; set; } = new();
    }

    public class DSHMDeviceCustomSettings
    {
        public string DeviceAddress { get; set; }
        public DSHM_Format_Settings CustomSettings { get; set; } = new();

    }

    public class DshmCustomJsonConverter : JsonConverter<DshmMainDataContainer>
    {
        public override DshmMainDataContainer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(
        Utf8JsonWriter writer, DshmMainDataContainer instance, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
                writer.WritePropertyName(nameof(instance.Global));
                var serializedGlobal = JsonSerializer.Serialize(instance.Global, options);
                writer.WriteRawValue(serializedGlobal);

                //JsonSerializer.Serialize(writer, new { instance.Global }, options);

                writer.WritePropertyName(nameof(instance.Devices));
                writer.WriteStartObject();
                    foreach (DSHMDeviceCustomSettings device in instance.Devices)
                    {
                        if (string.IsNullOrEmpty(device.DeviceAddress?.Trim()))
                            throw new JsonException("Expected non-null, non-empty Name");
                        writer.WritePropertyName(device.DeviceAddress);

                        var serializedCustomSettings = JsonSerializer.Serialize(device.CustomSettings, options);
                        writer.WriteRawValue(serializedCustomSettings);
    }
                writer.WriteEndObject();

            writer.WriteEndObject();

}
    }
}

