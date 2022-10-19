using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.MVVM;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nefarius.DsHidMini.ControlApp.MVVM.GroupLEDsCustomsVM;
using static Nefarius.DsHidMini.ControlApp.MVVM.GroupSettingsVM;

namespace Nefarius.DsHidMini.ControlApp.UserData
{
    internal class ControllersUserData
    {
        public static Guid guid = new();

        public DeviceSpecificData NewControllersDefault { get; set; } = new(); //"Global"

        public List<ProfileData> Profiles { get; set; } = new();

        public List<DeviceSpecificData> Devices { get; set; } = new();

        public DeviceSpecificData GetDeviceSpecificData(string deviceMac)
        {
            DeviceSpecificData deviceData = new();
            foreach(DeviceSpecificData dev in Devices)
            {
                if(dev.DeviceMac == deviceMac)
                {
                    deviceData = dev;
                    break;
                }
            }
            return deviceData;
        }

        public ProfileData GetProfileData(Guid profileGuid)
        {
            ProfileData profileData = new();
            foreach (ProfileData prof in Profiles)
            {
                if (prof.ProfileGuid == profileGuid)
                {
                    profileData = prof;
                    break;
                }
            }
            return profileData;
        }

    }

    public class ProfileData
    {
        public string ProfileName { get; set; }
        public Guid ProfileGuid { get; set; } = new();
        public SettingsContainer DeviceSettings { get; set; }
    }

    public class DeviceSpecificData
    {
        public string DeviceMac { get; set; } = "0000000000";
        public string DeviceCustomName { get; set; } = "Test";
        public Guid SelectedProfile { get; set; }
        public bool DoNotUseprofile { get; set; } = false;

        public BackingDataContainer DatasContainter { get; set; } = new();

        public void SaveToDSHM(DSHM_Format_ContextSettings dshmContextData)
        {
            DatasContainter.modesUniqueData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.ledsData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.wirelessData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.sticksDZData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.rumbleGeneralData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.outRepData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.leftRumbleRescaleData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.rightVariableEmulData.SaveToDSHMSettings(dshmContextData);
        }
    }

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
    }

    public abstract class SettingsBackingData
    {
        public bool IsGroupEnabled { get; set; }
        public SettingsContext Context { get; internal set; }

        public abstract void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings);

    }

    public class BackingData_ModesUnique : SettingsBackingData
    {
        public ControlApp_DsPressureMode PressureExposureMode { get; set; }
        public ControlApp_DPADModes DPadExposureMode { get; set; }
        public bool IsLEDsAsXInputSlotEnabled { get; set; }
        public bool PreventRemappingConflictsInSXSMode { get; set; }
        public bool PreventRemappingConflictsInDS4WMode { get; set; }
        public bool IsDS4LightbarTranslationEnabled { get; set; }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            dshmContextSettings.PressureExposureMode =
                (this.Context == SettingsContext.SDF
                || this.Context == SettingsContext.GPJ)
                ? SaveLoadUtils.Get_DSHM_DsPressureMode_From_ControlApp[this.PressureExposureMode] : null;

            dshmContextSettings.DPadExposureMode =
                (this.Context == SettingsContext.SDF
                || this.Context == SettingsContext.GPJ)
                ? SaveLoadUtils.Get_DSHM_DPadMode_From_ControlApp[this.DPadExposureMode] : null;
        }
    }

    public class BackingData_LEDs : SettingsBackingData
    {
        public ControlApp_LEDsModes LEDMode;
        public LEDCustoms[] LEDsCustoms { get; set; } = { new LEDCustoms(0), new LEDCustoms(1), new LEDCustoms(2), new LEDCustoms(3), };

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.LEDSettings;
            if (!this.IsGroupEnabled)
            {
                dshm_AllLEDsSettings = null;
                return;
            }

            dshm_AllLEDsSettings.Mode = SaveLoadUtils.Get_DSHM_LEDModes_From_ControlApp[this.LEDMode];

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
    }

    public class BackingData_Wireless : SettingsBackingData
    {
        public bool IsWirelessIdleDisconnectEnabled { get; set; }
        public byte WirelessIdleDisconnectTime { get; set; }
        public bool IsQuickDisconnectComboEnabled { get; set; }
        public ButtonsCombo QuickDisconnectCombo { get; set; } = new();

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
    }

    public class BackingData_SticksDZ : SettingsBackingData
    {
        public bool ApplyLeftStickDeadZone { get; set; }
        public bool ApplyRightStickDeadZone { get; set; }
        public byte LeftStickDeadZone { get; set; }
        public byte RightStickDeadZone { get; set; }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.DeadZoneLeft;
            DSHM_Format_ContextSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.DeadZoneRight;

            /*
            if (this.Context == SettingsContext.DS4W
                || SettingsContainer.GroupModeUnique.PreventRemappingConflictsInSXSMode)
            {
                dshmLeftDZSettings.Apply = dshmRightDZSettings.Apply = false;
                return;
            }

            if (!this.IsGroupEnabled)
            {
                dshmLeftDZSettings = null;
                dshmRightDZSettings = null;
                return;
            }
            */

            dshmLeftDZSettings.Apply = this.ApplyLeftStickDeadZone;
            dshmLeftDZSettings.PolarValue = this.LeftStickDeadZone;

            dshmRightDZSettings.Apply = this.ApplyRightStickDeadZone;
            dshmRightDZSettings.PolarValue = this.RightStickDeadZone;
        }

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
    }

    public class BackingData_RumbleGeneral : SettingsBackingData
    {
        public bool IsVariableLightRumbleEmulationEnabled { get; set; }
        public bool IsLeftMotorDisabled { get; set; }
        public bool IsRightMotorDisabled { get; set; }
        public bool IsVariableRightEmulToggleComboEnabled { get; set; }
        public ButtonsCombo VariableRightEmulToggleCombo { get; set; } = new();

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            DSHM_Format_ContextSettings.AllRumbleSettings dshmRumbleSettings = dshmContextSettings.RumbleSettings;

            /* Not necessary anymore I think
            if(SettingsContainer.GroupModeUnique.PreventRemappingConflictsInSXSMode)
            {
                dshmRumbleSettings.SMToBMConversion.Enabled = false;
                dshmRumbleSettings.DisableBM = false;
                dshmRumbleSettings.DisableSM = false;
                return;
            }
            */

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
    }

    public class BackingData_OutRepControl : SettingsBackingData
    {
        public bool IsOutputReportRateControlEnabled { get; set; }
        public byte MaxOutputRate { get; set; }
        public bool IsOutputReportDeduplicatorEnabled { get; set; }

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
    }

    public class BackingData_LeftRumbleRescale : SettingsBackingData
    {
        public bool IsLeftMotorStrRescalingEnabled { get; set; }
        public byte LeftMotorStrRescalingUpperRange { get; set; }
        public byte LeftMotorStrRescalingLowerRange { get; set; }

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
    }

    public class BackingData_VariablaRightRumbleEmulAdjusts : SettingsBackingData
    {
        public byte RightRumbleConversionUpperRange { get; set; }
        public byte RightRumbleConversionLowerRange { get; set; }
        public bool IsForcedRightMotorLightThresholdEnabled { get; set; }
        public bool IsForcedRightMotorHeavyThreasholdEnabled { get; set; }
        public byte ForcedRightMotorLightThreshold { get; set; }
        public byte ForcedRightMotorHeavyThreshold { get; set; }

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
    }

}
