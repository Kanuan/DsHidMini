using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI.Fody.Helpers;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupModeUniqueVM : GroupSettingsVM
    {
        public const ControlApp_DsPressureMode DEFAULT_pressureExposureMode = ControlApp_DsPressureMode.Default;
        public const ControlApp_DPADModes DEFAULT_padExposureMode = ControlApp_DPADModes.HAT;
        public const bool DEFAULT_isLEDsAsXInputSlotEnabled = false;
        public const bool DEFAULT_isDS4LightbarTranslationEnabled = false;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.Unique_All;

        [Reactive] public bool IsGroupEnabled { get; set; }
        // General
        [Reactive] public DSHM_HidDeviceModes? HIDDeviceMode { get; set; }

        // SDF and GPJ
        [Reactive] public ControlApp_DsPressureMode PressureExposureMode { get; set; }
        [Reactive] public ControlApp_DPADModes DPadExposureMode { get; set; }

        // XInput
        [Reactive] public bool IsLEDsAsXInputSlotEnabled { get; set; }

        // DS4Windows
        [Reactive] public bool IsDS4LightbarTranslationEnabled { get; set; }

        public GroupModeUniqueVM(SettingsContext context) : base(context)
        {

        }


        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {

            if ((this.Context == SettingsContext.General) && (dshmContextSettings.HIDDeviceMode != null))
                this.HIDDeviceMode = dshmContextSettings.HIDDeviceMode;
            else
                this.HIDDeviceMode = DSHM_HidDeviceModes.XInput;


            if ((this.Context == SettingsContext.General) && (dshmContextSettings.HIDDeviceMode != null))
                this.HIDDeviceMode = dshmContextSettings.HIDDeviceMode;
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
                (this.Context == SettingsContext.General)
                ? this.HIDDeviceMode : null;

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


}
