using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
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
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class VMGroupsContainer : ReactiveObject
    {

        [Reactive] internal List<GroupSettingsVM> GroupSettingsList { get; set; } = new();
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

            GroupSettingsList.Add(GroupModeUnique = new(dataContainer, this));
            GroupSettingsList.Add(GroupLEDsControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupWireless = new(dataContainer, this));
            GroupSettingsList.Add(GroupSticksDZ = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleGeneral = new(dataContainer, this));
            GroupSettingsList.Add(GroupOutRepControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(dataContainer, this));

            GroupSticksDZ.PropertyChanged += LockSticksDeadZoneGroup;

            ChangeContextOfAllGroups(Context);
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

        private void LockSticksDeadZoneGroup(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(GroupSticksDZ.Context):
                    GroupSticksDZ.IsGroupLocked = (Context == SettingsContext.DS4W) ? true : false;
                    break;
                default:
                    break;
            }
        }

    }

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

        public static string SaveToJsonTest(BackingDataContainer container)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            string jsonString = JsonSerializer.Serialize(container, options);

            System.IO.File.WriteAllText(@"D:\ControlAppTests.json", jsonString);

            var test = new ControllersUserData();

            string profileJson = JsonSerializer.Serialize(ProfileData.DefaultProfile, options);
            System.IO.File.WriteAllText(@"D:\DefaultProfileTest.json", profileJson);

            return jsonString;
        }

    }




}
