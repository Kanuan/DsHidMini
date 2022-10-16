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
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using static Nefarius.DsHidMini.ControlApp.SettingsContainer.GroupSettings.GroupLEDsControl;

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


    public class GroupSettingsVM : ReactiveObject
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

        [Reactive] public SettingsContainer Settings { get; set; }

        [Reactive] public bool IsOverrideCheckboxVisible { get; set; }

        [Reactive] public bool IsEditingAllowed { get; set; }
        [Reactive] public SettingsModeGroups SettingsGroup { get; set; }

        [Reactive] public string Header { get; set; } = "";

        public GroupSettingsVM(SettingsModeGroups settingsGroup, SettingsContainer settings)
        {
            SettingsGroup = settingsGroup;
            Settings = settings;
            IsOverrideCheckboxVisible = (Settings.ModeContext == SettingsModeContext.General || Settings.ModeContext == SettingsModeContext.Global) ? false : true;
            if (DictGroupHeader.TryGetValue(SettingsGroup, out string groupHeader)) Header = groupHeader;
            IsEditingAllowed = settings.ContainerContext == SettingsContainerContext.Custom ? true : false;
        }
    }

    public class GroupLEDsCustomsVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupLEDsControl.IsGroupEnabled;
            set
            {
                
                Settings.GroupLEDsControl.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }

        public ControlApp_LEDsModes LEDMode
        {
            get => Settings.GroupLEDsControl.LEDMode;
            set
            {
                Settings.GroupLEDsControl.LEDMode = value;
                this.RaisePropertyChanged(nameof(LEDMode));
            }
        }

        public LEDCustoms[] LEDsCustoms
        {
            get => Settings.GroupLEDsControl.LEDsCustoms;
            set
            {
                Settings.GroupLEDsControl.LEDsCustoms = value;
                this.RaisePropertyChanged(nameof(LEDsCustoms));
            }
        }

        [Reactive] public LEDCustoms CurrentLEDCustoms { get; set; }
        public int CurrentLEDCustomsIndex
        {
            get => CurrentLEDCustoms.LEDIndex;
            set
            {
                CurrentLEDCustoms = LEDsCustoms[value];
                this.RaisePropertyChanged(nameof(CurrentLEDCustomsIndex));
            }
        }


        public GroupLEDsCustomsVM(SettingsContainer modesSettings) : base(SettingsModeGroups.LEDsControl, modesSettings)
        {
            CurrentLEDCustoms = LEDsCustoms[0];
        }
    }

    public class GroupWirelessSettingsVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupWireless.IsGroupEnabled;
            set
            {
                Settings.GroupWireless.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }

        public bool IsWirelessIdleDisconnectEnabled
        {
            get => !Settings.GroupWireless.IsWirelessIdleDisconnectDisabled;
            set
            {
                Settings.GroupWireless.IsWirelessIdleDisconnectDisabled = !value;
                this.RaisePropertyChanged(nameof(IsWirelessIdleDisconnectEnabled));
            }
        }
        public byte WirelessIdleDisconnectTime
        {
            get => (byte)(Settings.GroupWireless.WirelessIdleDisconnectTime / (60.0 * 1000));
            set
            {
                Settings.GroupWireless.WirelessIdleDisconnectTime = value * 60 * 1000;
                this.RaisePropertyChanged(nameof(WirelessIdleDisconnectTime));
            }
        }

        public GroupWirelessSettingsVM(SettingsContainer modesSettings) : base(SettingsModeGroups.WirelessSettings, modesSettings) { }
    }

    public class GroupSticksDeadzoneVM : GroupSettingsVM
    {
        [Reactive] public bool IsSettingLocked { get; set; }

        public byte LeftStickDeadZone
        {
            get => Settings.GroupSticksDZ.LeftStickDeadZone;
            set
            {
                Settings.GroupSticksDZ.LeftStickDeadZone = value;
                this.RaisePropertyChanged(nameof(LeftStickDeadZone));
                this.RaisePropertyChanged(nameof(LeftStickDeadZoneInPercent));
            }
        }
        public byte RightStickDeadZone
        {
            get => Settings.GroupSticksDZ.RightStickDeadZone;
            set
            {
                Settings.GroupSticksDZ.RightStickDeadZone = value;
                this.RaisePropertyChanged(nameof(RightStickDeadZone));
                this.RaisePropertyChanged(nameof(RightStickDeadZoneInPercent));
            }
        }

        public int LeftStickDeadZoneInPercent
        {
            get => LeftStickDeadZone * 141 / 180;
        }
        public int RightStickDeadZoneInPercent
        {
            get => RightStickDeadZone * 141 / 180;
        }

        public bool IsGroupEnabled
        {
            get => Settings.GroupSticksDZ.IsGroupEnabled;
            set
            {
                Settings.GroupSticksDZ.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }

        public GroupSticksDeadzoneVM(SettingsContainer modesSettings) : base(SettingsModeGroups.SticksDeadzone, modesSettings) 
        {
            if(Settings.ModeContext == SettingsModeContext.DS4W)
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
            get => Settings.GroupRumbleGeneral.IsGroupEnabled;
            set
            {
                Settings.GroupRumbleGeneral.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }

        public bool IsVariableLightRumbleEmulationEnabled
        {
            get => Settings.GroupRumbleGeneral.IsVariableLightRumbleEmulationEnabled;
            set
            {
                Settings.GroupRumbleGeneral.IsVariableLightRumbleEmulationEnabled = value;
                this.RaisePropertyChanged(nameof(IsVariableLightRumbleEmulationEnabled));
                this.RaisePropertyChanged(nameof(IsLeftMotorDisabled));
                this.RaisePropertyChanged(nameof(IsRightMotorDisabled));
            }
        }

        public bool IsLeftMotorDisabled
        {
            get => Settings.GroupRumbleGeneral.IsLeftMotorDisabled;
            set
            {
                Settings.GroupRumbleGeneral.IsLeftMotorDisabled = value;
                this.RaisePropertyChanged(nameof(IsVariableLightRumbleEmulationEnabled));
                this.RaisePropertyChanged(nameof(IsLeftMotorDisabled));
            }
        }
        public bool IsRightMotorDisabled
        {
            get => Settings.GroupRumbleGeneral.IsRightMotorDisabled;
            set
            {
                Settings.GroupRumbleGeneral.IsRightMotorDisabled = value;
                this.RaisePropertyChanged(nameof(IsVariableLightRumbleEmulationEnabled));
                this.RaisePropertyChanged(nameof(IsRightMotorDisabled));
            }
        }

        public GroupRumbleGeneralVM(SettingsContainer modesSettings) : base(SettingsModeGroups.RumbleGeneral, modesSettings) { }
    }

    public class GroupOutRepControlVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupOutRepControl.IsGroupEnabled;
            set
            {
                Settings.GroupOutRepControl.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }

        public bool IsOutputReportRateControlEnabled
        {
            get => Settings.GroupOutRepControl.IsOutputReportRateControlEnabled;
            set
            {
                Settings.GroupOutRepControl.IsOutputReportRateControlEnabled = value;
                this.RaisePropertyChanged(nameof(IsOutputReportRateControlEnabled));
            }
        }
        public byte MaxOutputRate
        {
            get => Settings.GroupOutRepControl.MaxOutputRate;
            set
            {
                Settings.GroupOutRepControl.MaxOutputRate = value;
                this.RaisePropertyChanged(nameof(MaxOutputRate));
            }
        }
        public bool IsOutputReportDeduplicatorEnabled
        {
            get => Settings.GroupOutRepControl.IsOutputReportDeduplicatorEnabled;
            set
            {
                Settings.GroupOutRepControl.IsOutputReportDeduplicatorEnabled = value;
                this.RaisePropertyChanged(nameof(IsOutputReportDeduplicatorEnabled));
            }
        }


        public GroupOutRepControlVM(SettingsContainer modesSettings) : base(SettingsModeGroups.OutputReportControl, modesSettings) { }
    }

    public class GroupRumbleLeftRescaleVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupRumbleLeftRescale.IsGroupEnabled;
            set
            {
                Settings.GroupRumbleLeftRescale.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }
        public bool IsLeftMotorStrRescalingEnabled
        {
            get => Settings.GroupRumbleLeftRescale.IsLeftMotorStrRescalingEnabled;
            set
            {
                Settings.GroupRumbleLeftRescale.IsLeftMotorStrRescalingEnabled = value;
                this.RaisePropertyChanged(nameof(IsLeftMotorStrRescalingEnabled));
            }
        }
        public byte LeftMotorStrRescalingUpperRange
        {
            get => Settings.GroupRumbleLeftRescale.LeftMotorStrRescalingUpperRange;
            set
            {
                Settings.GroupRumbleLeftRescale.LeftMotorStrRescalingUpperRange = value;
                this.RaisePropertyChanged(nameof(LeftMotorStrRescalingUpperRange));
            }
        }
        public byte LeftMotorStrRescalingLowerRange
        {
            get => Settings.GroupRumbleLeftRescale.LeftMotorStrRescalingLowerRange;
            set
            {
                Settings.GroupRumbleLeftRescale.LeftMotorStrRescalingLowerRange = value;
                this.RaisePropertyChanged(nameof(LeftMotorStrRescalingLowerRange));
            }
        }

        public GroupRumbleLeftRescaleVM(SettingsContainer modesSettings) : base(SettingsModeGroups.RumbleLeftStrRescale, modesSettings) { }
    }

    public class GroupRumbleRightConversionAdjustsVM : GroupSettingsVM
    {
        public bool IsGroupEnabled
        {
            get => Settings.GroupRumbleRightConversion.IsGroupEnabled;
            set
            {
                Settings.GroupRumbleRightConversion.IsGroupEnabled = value;
                this.RaisePropertyChanged(nameof(IsGroupEnabled));
            }
        }
        public byte RightRumbleConversionUpperRange
        {
            get => Settings.GroupRumbleRightConversion.RightRumbleConversionUpperRange;
            set
            {
                Settings.GroupRumbleRightConversion.RightRumbleConversionUpperRange = value;
                this.RaisePropertyChanged(nameof(RightRumbleConversionUpperRange));
            }
        }
        public byte RightRumbleConversionLowerRange
        {
            get => Settings.GroupRumbleRightConversion.RightRumbleConversionLowerRange;
            set
            {
                Settings.GroupRumbleRightConversion.RightRumbleConversionLowerRange = value;
                this.RaisePropertyChanged(nameof(RightRumbleConversionLowerRange));
            }
        }
        public bool IsForcedRightMotorLightThresholdEnabled
        {
            get => Settings.GroupRumbleRightConversion.IsForcedRightMotorLightThresholdEnabled;
            set
            {
                Settings.GroupRumbleRightConversion.IsForcedRightMotorLightThresholdEnabled = value;
                this.RaisePropertyChanged(nameof(IsForcedRightMotorLightThresholdEnabled));
            }
        }
        public bool IsForcedRightMotorHeavyThreasholdEnabled
        {
            get => Settings.GroupRumbleRightConversion.IsForcedRightMotorHeavyThreasholdEnabled;
            set
            {
                Settings.GroupRumbleRightConversion.IsForcedRightMotorHeavyThreasholdEnabled = value;
                this.RaisePropertyChanged(nameof(IsForcedRightMotorHeavyThreasholdEnabled));
            }
        }
        public byte ForcedRightMotorLightThreshold
        {
            get => Settings.GroupRumbleRightConversion.ForcedRightMotorLightThreshold;
            set
            {
                Settings.GroupRumbleRightConversion.ForcedRightMotorLightThreshold = value;
                this.RaisePropertyChanged(nameof(ForcedRightMotorLightThreshold));
            }
        }
        public byte ForcedRightMotorHeavyThreshold
        {
            get => Settings.GroupRumbleRightConversion.ForcedRightMotorHeavyThreshold;
            set
            {
                Settings.GroupRumbleRightConversion.ForcedRightMotorHeavyThreshold = value;
                this.RaisePropertyChanged(nameof(ForcedRightMotorHeavyThreshold));
            }
        }

        public GroupRumbleRightConversionAdjustsVM(SettingsContainer modesSettings) : base(SettingsModeGroups.RumbleRightConversion, modesSettings) { }
    }


}
