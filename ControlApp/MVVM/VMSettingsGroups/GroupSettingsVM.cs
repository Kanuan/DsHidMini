using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Windows;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
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

    }


}
