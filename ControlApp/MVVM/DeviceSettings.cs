using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
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

        public void ResetSettingsToDefault(VMGroupsContainer modeContextSettings)
        {

        }

        public static string SaveToJsonTest(VMGroupsContainer container)
        {
            var backingDataContainer = new BackingDataContainer();
            //var dshmContextSettings = dshmSettings.General;

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

            var test = new ControllersUserData();

            string profileJson = JsonSerializer.Serialize(ProfileData.DefaultProfile, options);
            System.IO.File.WriteAllText(@"D:\DefaultProfileTest.json", profileJson);

            return jsonString;
        }

    }

        public class VMGroupsContainer
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

        public VMGroupsContainer(BackingDataContainer dataContainer)
        {
            Context = dataContainer.modesUniqueData.SettingsContext;

            GroupSettingsList.Add(GroupModeUnique = new(dataContainer,this));
            GroupSettingsList.Add(GroupLEDsControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupWireless = new(dataContainer, this));
            GroupSettingsList.Add(GroupSticksDZ = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleGeneral = new(dataContainer, this));
            GroupSettingsList.Add(GroupOutRepControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(dataContainer, this));

            ChangeContextOfAllGroups(Context);

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
