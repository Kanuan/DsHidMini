using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class TemplateSelector : IDataTemplate
    {
        public IControl Build(object param)
        {
            string templateName = SettingsGroupToTemplateDict[(SettingsModeGroups)param];
            var resultingCtrl = ((IDataTemplate)Avalonia.Application.Current.Resources[templateName]).Build(0);
            // resultingCtrl.DataContext = param.
            return resultingCtrl;
        }

        public bool Match(object data)
        {
            // Check if we can accept the provided data
            return data is Nefarius.DsHidMini.ControlApp.MVVM.SettingsModeGroups;
        }

        private static Dictionary<SettingsModeGroups, string> SettingsGroupToTemplateDict = new()
        {
            { SettingsModeGroups.LEDsControl, "Template_LEDsSettings" },
            { SettingsModeGroups.WirelessSettings, "Template_WirelessSettings" },
            { SettingsModeGroups.SticksDeadzone, "Template_SticksDeadZone" },
            { SettingsModeGroups.RumbleGeneral, "Template_RumbleBasicFunctions" },
            { SettingsModeGroups.OutputReportControl, "Template_OutputRateControl" },
            { SettingsModeGroups.RumbleLeftStrRescale, "Template_RumbleHeavyStrRescale" },
            { SettingsModeGroups.RumbleRightConversion, "Template_RumbleVariableLightEmuTuning" },
            { SettingsModeGroups.Unique_Global, "Template_ToDo" },
            { SettingsModeGroups.Unique_General, "Template_ToDo" },
            { SettingsModeGroups.Unique_SDF, "Template_SDF_GPJ_PressureButtons" },
            { SettingsModeGroups.Unique_GPJ, "Template_SDF_GPJ_PressureButtons" },
            { SettingsModeGroups.Unique_SXS, "Template_ToDo" },
            { SettingsModeGroups.Unique_DS4W, "Template_ToDo" },
            { SettingsModeGroups.Unique_XInput, "Template_ToDo" },
        };
    }


    public abstract class GroupSettingsVM : ReactiveObject
    {

        /// Replace with LexLoc
        private static Dictionary<SettingsModeGroups, string> DictGroupHeader = new()
        {
            { SettingsModeGroups.LEDsControl, "LEDs control" },
            { SettingsModeGroups.WirelessSettings, "Wireless settings" },
            { SettingsModeGroups.SticksDeadzone, "Sticks DeadZone (DZ)" },
            { SettingsModeGroups.RumbleGeneral, "Rumble settings" },
            { SettingsModeGroups.OutputReportControl, "Output report control" },
            { SettingsModeGroups.RumbleLeftStrRescale, "Left motor (heavy) rescale" },
            { SettingsModeGroups.RumbleRightConversion, "Variable light rumble emulation adjuster" },
            { SettingsModeGroups.Unique_Global, "Default settings" },
            { SettingsModeGroups.Unique_General, "General settings" },
            { SettingsModeGroups.Unique_SDF, "SDF mode specific settings" },
            { SettingsModeGroups.Unique_GPJ, "GPJ mode specific settings" },
            { SettingsModeGroups.Unique_SXS, "SXS mode specific settings" },
            { SettingsModeGroups.Unique_DS4W, "DS4W mode specific settings" },
            { SettingsModeGroups.Unique_XInput, "GPJ mode specific settings" },

        };

        [Reactive] public DeviceModesSettings Settings { get; set; }

        [Reactive] public SettingsContext Context { get; internal set; }

        [Reactive] public bool IsOverrideCheckboxVisible { get; set; }

        public abstract SettingsModeGroups Group { get; }

        [Reactive] public string Header { get; set; }

        public class ButtonsCombo
        {
            [Reactive] public ControlApp_ComboButtons Button1 { get; set; }
            [Reactive] public ControlApp_ComboButtons Button2 { get; set; }
            [Reactive] public ControlApp_ComboButtons Button3 { get; set; }

        }
        
        // Some GroupSettings, like SticksDeadzone, require overriding this since they change behavior depending on the context
        public virtual void ChangeContext(SettingsContext context)
        {
            Context = context;
        }

        public abstract void ResetGroupToOriginalDefaults();

        public abstract void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings);

        public abstract void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings);


        public bool ShouldGroupBeEnabledOnReset()
        {
            return (Context == SettingsContext.General || Context == SettingsContext.Global);
        }

        public GroupSettingsVM(SettingsContext context)
        {
            //IsOverrideCheckboxVisible = (Settings.Context == SettingsContext.General || Settings.Context == SettingsContext.Global) ? false : true;
            if (DictGroupHeader.TryGetValue(Group, out string groupHeader)) Header = groupHeader;
            Context = context;
            IsOverrideCheckboxVisible = (Context == SettingsContext.General || Context == SettingsContext.Global) ? false : true;
            ResetGroupToOriginalDefaults();

        }
    }


}
