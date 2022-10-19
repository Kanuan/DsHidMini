using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            return data is SettingsModeGroups;
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
            { SettingsModeGroups.Unique_All, "Template_Unique_All" },
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
            { SettingsModeGroups.Unique_All, "Mode specific settings" },
            { SettingsModeGroups.Unique_Global, "Default settings" },
            { SettingsModeGroups.Unique_General, "General settings" },
            { SettingsModeGroups.Unique_SDF, "SDF mode specific settings" },
            { SettingsModeGroups.Unique_GPJ, "GPJ mode specific settings" },
            { SettingsModeGroups.Unique_SXS, "SXS mode specific settings" },
            { SettingsModeGroups.Unique_DS4W, "DS4W mode specific settings" },
            { SettingsModeGroups.Unique_XInput, "GPJ mode specific settings" },

        };

        public ObservableCollection<ControlApp_ComboButtons> controlApp_ComboButtons { get; } = new ObservableCollection<ControlApp_ComboButtons> {
            ControlApp_ComboButtons.None,
            ControlApp_ComboButtons.PS,
            ControlApp_ComboButtons.START,
            ControlApp_ComboButtons.SELECT,
            ControlApp_ComboButtons.R1,
            ControlApp_ComboButtons.L1,
            ControlApp_ComboButtons.R2,
            ControlApp_ComboButtons.L2,
            ControlApp_ComboButtons.R3,
            ControlApp_ComboButtons.L3,
            ControlApp_ComboButtons.Triangle,
            ControlApp_ComboButtons.Circle,
            ControlApp_ComboButtons.Cross,
            ControlApp_ComboButtons.Square,
            ControlApp_ComboButtons.Up,
            ControlApp_ComboButtons.Right,
            ControlApp_ComboButtons.Dowm,
            ControlApp_ComboButtons.Left,
        };

        [Reactive] public virtual bool IsGroupLocked { get; set; } = false;

        [Reactive] public SettingsContainer SettingsContainer { get; set; }

        [Reactive] public SettingsContext Context { get; internal set; }

        [Reactive] public virtual bool IsOverrideCheckboxVisible { get; set; } = false;

        public abstract SettingsModeGroups Group { get; }

        public string Header { get; }

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

            public ButtonsCombo() {}
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

        // Some GroupSettings, like SticksDeadzone, require overriding this since they change behavior depending on the context
        public virtual void ChangeContext(SettingsContext context)
        {
            Context = context;
        }

        public abstract void ResetGroupToOriginalDefaults();

        public virtual void CopySettingsFromBackingData(SettingsBackingData data, bool invertCopyDirection = false)
        {
            if (invertCopyDirection)
                this.Context = data.Context;
            else
                data.Context = this.Context;
        }

        public bool ShouldGroupBeEnabledOnReset()
        {
            return true;
            //return (Context == SettingsContext.General || Context == SettingsContext.Global);
        }

        public GroupSettingsVM(SettingsContext context, SettingsContainer containter)
        {
            SettingsContainer = containter;
            //IsOverrideCheckboxVisible = (Settings.Context == SettingsContext.General || Settings.Context == SettingsContext.Global) ? false : true;
            if (DictGroupHeader.TryGetValue(Group, out string groupHeader)) Header = groupHeader;
            Context = context;
            //IsOverrideCheckboxVisible = (Context == SettingsContext.General || Context == SettingsContext.Global) ? false : true;
            ResetGroupToOriginalDefaults();

        }
    }


}
