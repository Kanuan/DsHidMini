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
    public class VMGroupsContainer : ReactiveObject
    {
        [Reactive] internal List<GroupSettingsVM> GroupSettingsList { get; set; } = new();
        [Reactive] public SettingsContext Context { get; set; }
        [Reactive] public GroupModeUniqueVM GroupModeUnique { get; set; }
        [Reactive] public GroupLEDsCustomsVM GroupLEDsControl { get; set; }
        [Reactive] public GroupWirelessSettingsVM GroupWireless { get; set; }
        [Reactive] public GroupSticksVM GroupSticksDZ { get; set; }
        [Reactive] public GroupRumbleGeneralVM GroupRumbleGeneral { get; set; }
        [Reactive] public GroupOutRepControlVM GroupOutRepControl { get; set; }
        [Reactive] public GroupRumbleLeftRescaleVM GroupRumbleLeftRescale { get; set; }
        [Reactive] public GroupRumbleRightConversionAdjustsVM GroupRumbleRightConversion { get; set; }

        public VMGroupsContainer(BackingDataContainer dataContainer)
        {
            Context = dataContainer.modesUniqueData.SettingsContext;

            GroupSettingsList.Add(GroupModeUnique = new(dataContainer, this));
            GroupSettingsList.Add(GroupLEDsControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupWireless = new(dataContainer, this));
            GroupSettingsList.Add(GroupSticksDZ = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleGeneral = new(dataContainer, this));
            GroupSettingsList.Add(GroupOutRepControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(dataContainer, this));
        }

        public void ChangeContextOfAllGroups(SettingsContext context)
        {
            Context = context;
            foreach (GroupSettingsVM group in GroupSettingsList)
            {
                group.ChangeContext(context);
            }
        }

        public void SaveAllChangesToBackingData(BackingDataContainer dataContainer)
        {
            foreach (GroupSettingsVM group in GroupSettingsList)
            {
                group.SaveSettingsToBackingDataContainer(dataContainer);
            }
        }

        public void LoadDatasToAllGroups(BackingDataContainer dataContainer)
        {
            foreach (GroupSettingsVM group in GroupSettingsList)
            {
                group.LoadSettingsFromBackingDataContainer(dataContainer);
            }
        }
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

        [Reactive] public virtual bool IsGroupLocked { get; set; } = false;

        [Reactive] public VMGroupsContainer SettingsContainer { get; set; }

        [Reactive] public SettingsContext Context { get; internal set; } = SettingsContext.XInput;

        [Reactive] public virtual bool IsOverrideCheckboxVisible { get; set; } = false;

        public abstract SettingsModeGroups Group { get; }

        public string Header { get; }



        // Some GroupSettings, like SticksDeadzone, require overriding this since they have logic that depends on the context
        public virtual void ChangeContext(SettingsContext context)
        {
            Context = context;
        }

        public abstract void ResetGroupToOriginalDefaults();

        public bool ShouldGroupBeEnabledOnReset()
        {
            return true;
        }

        public abstract void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource);
        public abstract void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource);

        public GroupSettingsVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter)
        {
            SettingsContainer = vmGroupsContainter;
            if (DictGroupHeader.TryGetValue(Group, out string groupHeader)) Header = groupHeader;
            LoadSettingsFromBackingDataContainer(backingDataContainer);
            ResetGroupToDefaultsCommand = ReactiveCommand.Create(ResetGroupToOriginalDefaults);
        }

        public ReactiveCommand<Unit, Unit> ResetGroupToDefaultsCommand { get; }

    }


}
