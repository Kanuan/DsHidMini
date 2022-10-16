using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.SettingsContainer.GroupSettings;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class DeviceSettingsManager : ReactiveObject
    {
        [Reactive] public string DeviceName { get; set; }

        [Reactive] public string TestSaveJson { get; set; } = "Binding working";

        [Reactive] public Dictionary<SettingsModeContext, SettingsContainer> SettingsPerContext { get; set; } = new();

        [Reactive] public List<SettingsModeContext> ActiveContexts { get; set; } = new List<SettingsModeContext>
        {
            SettingsModeContext.General,
            SettingsModeContext.SDF,
            //SettingsContext.GPJ,
            //SettingsContext.DS4W,
            //SettingsContext.XInput,
        };

        /* delete later
        [Reactive] public DeviceModesSettings GeneralSettings { get; set; } = new(SettingsContext.General);
        [Reactive] public DeviceModesSettings SDFSettings { get; set; } = new(SettingsContext.SDF);
        [Reactive] public DeviceModesSettings GPJSettings { get; set; } = new(SettingsContext.GPJ);
        [Reactive] public DeviceModesSettings SXSSettings { get; set; } = new(SettingsContext.SXS);
        [Reactive] public DeviceModesSettings XInputSettings { get; set; } = new(SettingsContext.XInput);
        [Reactive] public DeviceModesSettings DS4WSettings { get; set; } = new(SettingsContext.DS4W);
        */

        public DeviceSettingsManager()
        {
            /*
            foreach(SettingsModeContext context in ActiveContexts)
            {
                SettingsPerContext.Add(context, new SettingsContainer(context));
            }
            */
            SettingsPerContext.Add(SettingsModeContext.General, new SettingsContainer(SettingsModeContext.General, SettingsContainerContext.Profile));
            SettingsPerContext.Add(SettingsModeContext.SDF, new SettingsContainer(SettingsModeContext.SDF, SettingsContainerContext.Custom));
            SaveToJsonTest();
        }

        public void ResetSettingsToDefault(SettingsContainer modeContextSettings)
        {

        }

        public void SaveToJsonTest()
        {
            var dshmSettings = new DSHM_Format_Settings();
            var dshmContextSettings = dshmSettings.General;

            SettingsContainer Controltest = this.SettingsPerContext[SettingsModeContext.SDF];

            //foreach (DeviceModesSettings contextSettings in SettingsPerContext.Values)
            //{
                Controltest.GroupModeUnique.SaveToDSHMSettings(dshmContextSettings);
                Controltest.GroupLEDsControl.SaveToDSHMSettings(dshmContextSettings);
                Controltest.GroupWireless.SaveToDSHMSettings(dshmContextSettings);
                Controltest.GroupSticksDZ.SaveToDSHMSettings(dshmContextSettings);
                Controltest.GroupRumbleGeneral.SaveToDSHMSettings(dshmContextSettings);
                Controltest.GroupOutRepControl.SaveToDSHMSettings(dshmContextSettings);
                Controltest.GroupRumbleLeftRescale.SaveToDSHMSettings(dshmContextSettings);
                Controltest.GroupRumbleRightConversion.SaveToDSHMSettings(dshmContextSettings);
            //}

            TestSaveJson = SaveLoadJson.SaveToDSHMFormattedJson(dshmSettings);
        }

    }

        public class SettingsContainer
        {


        internal List<GroupSettings> GroupSettingsList = new();

        public SettingsContainerContext ContainerContext { get; set; }

        [Reactive] public SettingsModeContext ModeContext { get; set; }
        [Reactive] public GroupModeUnique GroupModeUnique { get; set; }
        [Reactive] public GroupLEDsControl GroupLEDsControl { get; set; }
        [Reactive] public GroupWireless GroupWireless { get; set; }
        [Reactive] public GroupSticksDeadZone GroupSticksDZ { get; set; }
        [Reactive] public GroupRumbleGeneral GroupRumbleGeneral { get; set; }
        [Reactive] public GroupOutRepControl GroupOutRepControl { get; set; }
        [Reactive] public GroupRumbleLeftRescale GroupRumbleLeftRescale { get; set; }
        [Reactive] public GroupRumbleRightConversion GroupRumbleRightConversion { get; set; }

        public SettingsContainer(SettingsModeContext settingsContext, SettingsContainerContext containerContext)
        {
            ContainerContext = containerContext;
            ModeContext = settingsContext;

            GroupSettingsList.Add(GroupModeUnique = new(ModeContext));
            GroupSettingsList.Add(GroupLEDsControl = new(ModeContext));
            GroupSettingsList.Add(GroupWireless = new(ModeContext));
            GroupSettingsList.Add(GroupSticksDZ = new(ModeContext));
            GroupSettingsList.Add(GroupRumbleGeneral = new(ModeContext));
            GroupSettingsList.Add(GroupOutRepControl = new(ModeContext));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(ModeContext));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(ModeContext));

            /*
            foreach(GroupSettings group in GroupSettingsList)
            {
                group.ResetGroupToOriginalDefaults();
            }
            */
        }

        public void CopyGroupSettings(SettingsModeGroups group, SettingsContainer fromSettings, SettingsContainer toSettings)
        {

        }
    }


}
