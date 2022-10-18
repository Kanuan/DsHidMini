using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupModeUniqueVM : GroupSettingsVM
    {
        public const ControlApp_DsPressureMode DEFAULT_pressureExposureMode = ControlApp_DsPressureMode.Default;
        public const ControlApp_DPADModes DEFAULT_padExposureMode = ControlApp_DPADModes.HAT;
        public const bool DEFAULT_isLEDsAsXInputSlotEnabled = false;
        public const bool DEFAULT_isDS4LightbarTranslationEnabled = false;
        public const bool DEFAULT_areDSHMRumbleSettingsDisabled = true;
        private bool DEFAULT_areRumbleSettingsDisabledAndLockedInSXSMode = true;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.Unique_All;

        [Reactive] public bool IsGroupEnabled { get; set; }
        // General
        [Reactive] public DSHM_HidDeviceModes HIDDeviceMode{ get; set; }

        // SDF and GPJ
        readonly ObservableAsPropertyHelper<bool> arePressureaNDPadOptionsVisible;
        public bool ArePressureaNDPadOptionsVisible => arePressureaNDPadOptionsVisible.Value;
        /*
        public bool ArePressureaNDPadOptionsVisible
        {
            get => ((Context == SettingsContext.SDF) || (Context == SettingsContext.GPJ)) ? true : false;
        }
        */
        [Reactive] public ControlApp_DsPressureMode PressureExposureMode { get; set; }
        [Reactive] public ControlApp_DPADModes DPadExposureMode { get; set; }

        // SXS 
        public bool AreSXSRelatedOptionsVisible
        {
            get => Context == SettingsContext.SXS ? true : false;
        }
        public bool PreventRemappingConflictsInSXSMode
        {
            get
            {
                if(Context == SettingsContext.SXS) return DEFAULT_areRumbleSettingsDisabledAndLockedInSXSMode;
                return false;
            }

            set => DEFAULT_areRumbleSettingsDisabledAndLockedInSXSMode = value;
        }

        // XInput
        [Reactive] public bool IsLEDsAsXInputSlotEnabled { get; set; }

        // DS4Windows
        [Reactive] public bool IsDS4LightbarTranslationEnabled { get; set; }

        public GroupModeUniqueVM(SettingsContext context, SettingsContainer containter) : base(context, containter)
        {
            if(context != SettingsContext.General)
            {
                IsGroupEnabled = true;
                IsOverrideCheckboxVisible = false;
            }

            arePressureaNDPadOptionsVisible = this
                .WhenAnyValue(x => x.Context)
                .Select(ArePressureaNDPadOptionsVisible => (( Context == SettingsContext.SDF ) || (Context == SettingsContext.GPJ)) ? true : false)
                .ToProperty(this, x => x.ArePressureaNDPadOptionsVisible);


    }

        public override void ChangeContext(SettingsContext context)
        {
            base.ChangeContext(context);

        }

        public override void ResetGroupToOriginalDefaults()
        {
            HIDDeviceMode = DSHM_HidDeviceModes.XInput;
            PressureExposureMode = DEFAULT_pressureExposureMode;
            DPadExposureMode = DEFAULT_padExposureMode;
            IsLEDsAsXInputSlotEnabled = DEFAULT_isLEDsAsXInputSlotEnabled;
            IsDS4LightbarTranslationEnabled = DEFAULT_isDS4LightbarTranslationEnabled;
            PreventRemappingConflictsInSXSMode = DEFAULT_areDSHMRumbleSettingsDisabled;
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
    }


}
