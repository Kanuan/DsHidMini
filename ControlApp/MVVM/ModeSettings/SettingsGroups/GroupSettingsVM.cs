using Avalonia.Controls.Templates;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using Avalonia.Styling;
using System.Reflection.Metadata;

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
            { SettingsModeGroups.Unique_SDF, "Template_ToDo" },
            { SettingsModeGroups.Unique_GPJ, "Template_ToDo" },
            { SettingsModeGroups.Unique_DS4W, "Template_ToDo" },
            { SettingsModeGroups.Unique_XInput, "Template_ToDo" },
        };
    }


    public class GroupSettingsVM : ObservableObject
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
            { SettingsModeGroups.Unique_DS4W, "DS4W mode specific settings" },
            { SettingsModeGroups.Unique_XInput, "GPJ mode specific settings" },

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
            IsOverrideCheckboxVisible = (Settings.Context == SettingsContext.General || Settings.Context == SettingsContext.Global) ? false : true;
            if (DictGroupHeader.TryGetValue(SettingsGroup, out string groupHeader)) Header = groupHeader;
        }
    }

    public class GroupLEDsCustomsVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupLEDsControl.IsGroupLEDsCustomizationEnabled;
            set
            {
                Settings.GroupLEDsControl.IsGroupLEDsCustomizationEnabled = value;
                OnPropertyChanged("IsGroupEnabled");
            }
        }

        public GroupLEDsCustomsVM(DeviceModesSettings modesSettings) : base(SettingsModeGroups.LEDsControl, modesSettings) { }
    }

    public class GroupWirelessSettingsVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupWireless.IsGroupWirelessSettingsEnabled;
            set
            {
                Settings.GroupWireless.IsGroupWirelessSettingsEnabled = value;
                OnPropertyChanged("IsGroupEnabled");
            }
        }

        public GroupWirelessSettingsVM(DeviceModesSettings modesSettings) : base(SettingsModeGroups.WirelessSettings, modesSettings) { }
    }

    public class GroupSticksDeadzoneVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupSticksDZ.IsGroupSticksDeadzoneEnabled;
            set
            {
                Settings.GroupSticksDZ.IsGroupSticksDeadzoneEnabled = value;
                OnPropertyChanged("IsGroupEnabled");
            }
        }

        private bool isSettingLocked = false;
        public bool IsSettingLocked
        {
            get => isSettingLocked;
            set => SetProperty(ref isSettingLocked, value);
        }

        public GroupSticksDeadzoneVM(DeviceModesSettings modesSettings) : base(SettingsModeGroups.SticksDeadzone, modesSettings) 
        {
            if(Settings.Context == SettingsContext.DS4W)
            {
                IsOverrideCheckboxVisible = false;
                IsSettingLocked = true;
            }
        }
    }

    public class GroupRumbleGeneralVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupRumbleGeneral.IsGroupRumbleGeneralEnabled;
            set
            {
                Settings.GroupRumbleGeneral.IsGroupRumbleGeneralEnabled = value;
                OnPropertyChanged("IsGroupEnabled");
            }
        }

        public GroupRumbleGeneralVM(DeviceModesSettings modesSettings) : base(SettingsModeGroups.RumbleGeneral, modesSettings) { }
    }

    public class GroupOutRepControlVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupOutRepControl.IsGroupOutRepControlEnabled;
            set
            {
                Settings.GroupOutRepControl.IsGroupOutRepControlEnabled = value;
                OnPropertyChanged("IsGroupEnabled");
            }
        }

        public GroupOutRepControlVM(DeviceModesSettings modesSettings) : base(SettingsModeGroups.OutputReportControl, modesSettings) { }
    }

    public class GroupRumbleLeftRescaleVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupRumbleLeftRescale.IsGroupRumbleLeftRescaleEnabled;
            set
            {
                Settings.GroupRumbleLeftRescale.IsGroupRumbleLeftRescaleEnabled = value;
                OnPropertyChanged("IsGroupEnabled");
            }
        }

        public GroupRumbleLeftRescaleVM(DeviceModesSettings modesSettings) : base(SettingsModeGroups.RumbleLeftStrRescale, modesSettings) { }
    }

    public class GroupRumbleRightConversionAdjustsVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupRumbleRightConversion.IsGroupRumbleRightConversionEnabled;
            set
            {
                Settings.GroupRumbleRightConversion.IsGroupRumbleRightConversionEnabled = value;
                OnPropertyChanged("IsGroupEnabled");
            }
        }

        public GroupRumbleRightConversionAdjustsVM(DeviceModesSettings modesSettings) : base(SettingsModeGroups.RumbleRightConversion, modesSettings) { }
    }


}
