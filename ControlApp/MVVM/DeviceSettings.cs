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
using Nefarius.DsHidMini.ControlApp.UserData;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

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
            var backingDataContainer = new BackingDataContainer();
            //var dshmContextSettings = dshmSettings.General;

                container.GroupModeUnique.CopySettingsFromBackingData(backingDataContainer.modesUniqueData,true);
                container.GroupLEDsControl.CopySettingsFromBackingData(backingDataContainer.ledsData,true);
                container.GroupWireless.CopySettingsFromBackingData(backingDataContainer.wirelessData, true);
                container.GroupRumbleGeneral.CopySettingsFromBackingData(backingDataContainer.rumbleGeneralData, true);
                container.GroupSticksDZ.CopySettingsFromBackingData(backingDataContainer.sticksDZData, true);
                container.GroupOutRepControl.CopySettingsFromBackingData(backingDataContainer.outRepData, true);
                container.GroupRumbleLeftRescale.CopySettingsFromBackingData(backingDataContainer.leftRumbleRescaleData, true);
                container.GroupRumbleRightConversion.CopySettingsFromBackingData(backingDataContainer.rightVariableEmulData, true);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            string jsonString = JsonSerializer.Serialize(backingDataContainer, options);

            System.IO.File.WriteAllText(@"D:\ControlAppTests.json", jsonString);

            return jsonString;
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
