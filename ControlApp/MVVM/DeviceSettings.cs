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

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class DeviceSettingsManager : ReactiveObject
    {
        [Reactive] public string DeviceName { get; set; }

        [Reactive] public static string TestSaveJson { get; set; } = "Binding working";


        public DeviceSettingsManager()
        {

        }

        public void ResetSettingsToDefault(SettingsContainer modeContextSettings)
        {

        }

        public static string SaveToJsonTest(SettingsContainer container)
        {
            var dshmSettings = new DSHM_Format_Settings();
            var dshmContextSettings = dshmSettings.General;

                container.GroupModeUnique.SaveToDSHMSettings(dshmContextSettings);
                container.GroupLEDsControl.SaveToDSHMSettings(dshmContextSettings);
                container.GroupWireless.SaveToDSHMSettings(dshmContextSettings);
                container.GroupSticksDZ.SaveToDSHMSettings(dshmContextSettings);
                container.GroupRumbleGeneral.SaveToDSHMSettings(dshmContextSettings);
                container.GroupOutRepControl.SaveToDSHMSettings(dshmContextSettings);
                container.GroupRumbleLeftRescale.SaveToDSHMSettings(dshmContextSettings);
                container.GroupRumbleRightConversion.SaveToDSHMSettings(dshmContextSettings);

            return SaveLoadJson.SaveToDSHMFormattedJson(dshmSettings);
        }

    }

        public class SettingsContainer
    {

        internal List<GroupSettingsVM> GroupSettingsList = new();


        [Reactive] public SettingsContext Context { get; set; }
        [Reactive] public GroupModeUniqueVM GroupModeUnique { get; set; }
        [Reactive] public GroupLEDsCustomsVM GroupLEDsControl { get; set; }
        [Reactive] public GroupWirelessSettingsVM GroupWireless { get; set; }
        [Reactive] public GroupSticksDeadzoneVM GroupSticksDZ { get; set; }
        [Reactive] public GroupRumbleGeneralVM GroupRumbleGeneral { get; set; }
        [Reactive] public GroupOutRepControlVM GroupOutRepControl { get; set; }
        [Reactive] public GroupRumbleLeftRescaleVM GroupRumbleLeftRescale { get; set; }
        [Reactive] public GroupRumbleRightConversionAdjustsVM GroupRumbleRightConversion { get; set; }

        public SettingsContainer(SettingsContext settingsContext)
        {
            Context = settingsContext;

            GroupSettingsList.Add(GroupModeUnique = new(Context,this));
            GroupSettingsList.Add(GroupLEDsControl = new(Context, this));
            GroupSettingsList.Add(GroupWireless = new(Context, this));
            GroupSettingsList.Add(GroupSticksDZ = new(Context, this));
            GroupSettingsList.Add(GroupRumbleGeneral = new(Context, this));
            GroupSettingsList.Add(GroupOutRepControl = new(Context, this));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(Context, this));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(Context, this));

            GroupModeUnique.PropertyChanged += GroupModeUnique_PropertyChanged;
        }

        public void ChangeContextOfAllGroups(SettingsContext context)
        {
            Context = context;
            foreach (GroupSettingsVM group in GroupSettingsList)
            {
                group.ChangeContext(context);
            }
        }

        private void GroupModeUnique_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            /*
            switch (e.PropertyName)
            {
                case nameof(GroupModeUnique.HIDDeviceMode):
                    Context = GroupModeUnique.Context;
                    foreach(GroupSettingsVM group in GroupSettingsList)
                    {
                        group.ChangeContext(Context);
                    }
                    break;
                default:
                    break;
            }
            */
        }
    }


}
