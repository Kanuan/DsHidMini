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

        [Reactive] public Dictionary<SettingsContext, DeviceModesSettings> SettingsPerContext { get; set; } = new();

        [Reactive] public List<SettingsContext> ActiveContexts { get; set; } = new List<SettingsContext>
        {
            //SettingsContext.General,
            SettingsContext.SDF,
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
            foreach(SettingsContext context in ActiveContexts)
            {
                SettingsPerContext.Add(context, new DeviceModesSettings(context));
            }
            SaveToJsonTest();
        }

        public void ResetSettingsToDefault(DeviceModesSettings modeContextSettings)
        {

        }

        public void SaveToJsonTest()
        {
            var dshmSettings = new DSHM_Format_Settings();
            var dshmContextSettings = dshmSettings.General;

            DeviceModesSettings Controltest = this.SettingsPerContext[SettingsContext.SDF];

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

        public class DeviceModesSettings
    {

        internal List<GroupSettings> GroupSettingsList = new();


        [Reactive] public SettingsContext Context { get; set; }
        [Reactive] public GroupModeUnique GroupModeUnique { get; set; }
        [Reactive] public GroupLEDsControl GroupLEDsControl { get; set; }
        [Reactive] public GroupWireless GroupWireless { get; set; }
        [Reactive] public GroupSticksDeadZone GroupSticksDZ { get; set; }
        [Reactive] public GroupRumbleGeneral GroupRumbleGeneral { get; set; }
        [Reactive] public GroupOutRepControl GroupOutRepControl { get; set; }
        [Reactive] public GroupRumbleLeftRescale GroupRumbleLeftRescale { get; set; }
        [Reactive] public GroupRumbleRightConversion GroupRumbleRightConversion { get; set; }

        public DeviceModesSettings(SettingsContext settingsContext)
        {
            Context = settingsContext;

            GroupSettingsList.Add(GroupModeUnique = new(Context));
            GroupSettingsList.Add(GroupLEDsControl = new(Context));
            GroupSettingsList.Add(GroupWireless = new(Context));
            GroupSettingsList.Add(GroupSticksDZ = new(Context));
            GroupSettingsList.Add(GroupRumbleGeneral = new(Context));
            GroupSettingsList.Add(GroupOutRepControl = new(Context));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(Context));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(Context));

            foreach(GroupSettings group in GroupSettingsList)
            {
                group.ResetGroupToOriginalDefaults();
            }
        }

        public void CopyGroupSettings(SettingsModeGroups group, DeviceModesSettings fromSettings, DeviceModesSettings toSettings)
        {

        }
    }


}
