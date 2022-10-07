using Avalonia.Controls.Templates;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    internal class GroupSettingsVM : ObservableObject
    {


        private static Dictionary<SettingsModeGroups, string> DictGroupHeader = new()
        {
            { SettingsModeGroups.LEDsControl, "LEDs control" },
            { SettingsModeGroups.WirelessSettings, "Wireless settings" },
            { SettingsModeGroups.SticksDeadzone, "Sticks DeadZone (DZ)" },
            { SettingsModeGroups.RumbleBasicFunctions, "Rumble settings" },
            { SettingsModeGroups.OutputReportControl, "Output report control" },
            { SettingsModeGroups.RumbleHeavyStrRescale, "Left motor (heavy) rescale" },
            { SettingsModeGroups.RumbleLightConversion, "Variable light rumble emulation adjuster" },
        };

        private DeviceModesSettings _settings; // This object already contains all the necessary logic
        public DeviceModesSettings Settings
        {
            get { return _settings; }
            set { SetProperty(ref _settings, value); }
        }

        private bool _isOverrideCheckboxVisible;
        public bool IsOverrideCheckboxVisible
        {
            get { return _isOverrideCheckboxVisible; }
            set { SetProperty(ref _isOverrideCheckboxVisible, value); }
        }

        private SettingsModeGroups _settingsGroups;
        public SettingsModeGroups SettingsGroup
        {
            get { return _settingsGroups; }
            set { SetProperty(ref _settingsGroups, value); }
        }

        private string _header = "";
        public string Header
        {
            get => _header;
            set
            {
                SetProperty(ref _header, value);
            }
        }

        public GroupSettingsVM(SettingsModeGroups settingsGroup, DeviceModesSettings settings)
        {
            SettingsGroup = settingsGroup;
            Settings = settings;
            IsOverrideCheckboxVisible = ( Settings.currentSettingContext == SettingsContext.General || Settings.currentSettingContext == SettingsContext.Global) ? false : true;
            if (DictGroupHeader.TryGetValue(SettingsGroup, out string groupHeader)) Header = groupHeader;
        }
    }

    public class TemplateSelector : IDataTemplate
    {
        public IControl Build(object param)
        {
            string templateName = SettingsGroupToTemplateDict[(SettingsModeGroups)param];
            var  resultingCtrl = ((IDataTemplate)Avalonia.Application.Current.Resources[templateName]).Build(0);
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
            { SettingsModeGroups.RumbleBasicFunctions, "Template_RumbleBasicFunctions" },
            { SettingsModeGroups.OutputReportControl, "Template_OutputRateControl" },
            { SettingsModeGroups.RumbleHeavyStrRescale, "Template_RumbleHeavyStrRescale" },
            { SettingsModeGroups.RumbleLightConversion, "Template_RumbleVariableLightEmuTuning" },
            { SettingsModeGroups.Unique_SDF, "Template_ToDo" },
            { SettingsModeGroups.Unique_GPJ, "Template_ToDo" },
            { SettingsModeGroups.Unique_DS4W, "Template_ToDo" },
            { SettingsModeGroups.Unique_XInput, "Template_ToDo" },
        };
    }
}
