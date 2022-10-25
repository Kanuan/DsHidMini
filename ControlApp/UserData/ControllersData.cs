using Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json;
using Nefarius.DsHidMini.ControlApp.MVVM;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nefarius.DsHidMini.ControlApp.MVVM.GroupLEDsCustomsVM;
using static Nefarius.DsHidMini.ControlApp.MVVM.GroupSettingsVM;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;
using Nefarius.DsHidMini.ControlApp.UserData;

namespace Nefarius.DsHidMini.ControlApp.UserData
{


    internal class ControllersUserData
    {
        public static JsonSerializerOptions ControlAppJsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IncludeFields = true,

            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private const string DISK = @"D:\";
        private const string CONTROL_APP_FOLDER_PATH_IN_DISK = @"ControlApp\";
        private const string PROFILE_FOLDER_NAME = @"Profiles\";
        private const string DEVICES_FOLDER_NAME = @"Devices\";

        public string ProfilesFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{PROFILE_FOLDER_NAME}";
        public string DevicesFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{DEVICES_FOLDER_NAME}";

        // -----------------------------------------------------------

        public static Guid guid = new();

        public DeviceSpecificData NewControllersDefault { get; set; } = new("0123456789"); //"Global"

        public List<ProfileData> Profiles { get; set; } = new();
        public Dictionary<Guid, ProfileData> ProfilesPerGuid { get; set; } = new();

        public List<DeviceSpecificData> Devices { get; set; } = new();

        public ControllersUserData()
        {
            Profiles = CreateListOfProfilesOnDisk();
            UpdateDictionaryOfLoadedProfilesPerGuid();

            Devices = LoadDevicesFromDisk();

            /*
            ProfileData profileTest = new()
            {
                ProfileName = "Test",
                DiskFileName = "Test.json",
            };
            CheckAndFixRepeatedProfileFilePath(profileTest);
            Profiles.Add(profileTest);
            */

            SaveAllProfilesToDisk(Profiles);
        }

        private List<DeviceSpecificData> LoadDevicesFromDisk()
        {

            var devicesOnDisk = new List<DeviceSpecificData>();

            string[] devicesPaths = Directory.GetFiles($@"{DevicesFolderFullPath}", "*.json");
            foreach (string devPath in devicesPaths)
            {
                var dirName = new DirectoryInfo(devPath).Name;
                var jsonText = System.IO.File.ReadAllText(devPath);

                var data = JsonSerializer.Deserialize<DeviceSpecificData>(jsonText, ControlAppJsonSerializerOptions);
                //data.DiskFileName = dirName;
                devicesOnDisk.Add(data);
            }

            return devicesOnDisk;
        }

        public List<ProfileData> CreateListOfProfilesOnDisk()
        {
            var profilesOnDisk = new List<ProfileData>();

            string[] profilesPaths = Directory.GetFiles($@"{ProfilesFolderFullPath}", "*.json");
            foreach (string profilePath in profilesPaths)
            {
                var dirName = new DirectoryInfo(profilePath).Name;
                var jsonText = System.IO.File.ReadAllText(profilePath);

                JsonSerializerOptions ControlAppJsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    IncludeFields = true,

                    Converters =
                    {
                        new JsonStringEnumConverter()
                    }
                };

                ProfileData data = JsonSerializer.Deserialize<ProfileData>(jsonText, ControlAppJsonSerializerOptions);
                data.DiskFileName = dirName;
                profilesOnDisk.Add(data);
            }

            return profilesOnDisk;
        }

        public void UpdateDictionaryOfLoadedProfilesPerGuid()
        {
            var profilesPerGuid = new Dictionary<Guid, ProfileData>();
            foreach (ProfileData prof in Profiles)
            {
                profilesPerGuid.Add(prof.ProfileGuid, prof);
            }
            ProfilesPerGuid = profilesPerGuid;
        }

        public void SaveAllProfilesToDisk(List<ProfileData> profiles)
        {           
            foreach (ProfileData profile in profiles)
            {
                SaveProfileToDisk(profile);
            }
        }

        public void CheckAndFixRepeatedProfileFilePath(ProfileData newProfile)
        {
            var existingProfilePaths = new List<string>();
            foreach(ProfileData profile in Profiles)
            {
                existingProfilePaths.Add(profile.DiskFileName);
            }

            string newNameForProfile = newProfile.DiskFileName;
            for(int i = 1; existingProfilePaths.Contains(newNameForProfile); i++)
            {
                newNameForProfile = $"New{i}_{newProfile.DiskFileName}";
            }

            newProfile.DiskFileName = newNameForProfile;
        }

        public void SaveProfileToDisk(ProfileData profile)
        {
            var defaultGuide = new Guid("00000000000000000000000000000000");
            // Ignore the defaullt profile
            if (profile.ProfileGuid == defaultGuide) return;

            // Save profile to disk
            string profileJson = JsonSerializer.Serialize(profile, ControlAppJsonSerializerOptions);
            System.IO.File.WriteAllText($@"{ProfilesFolderFullPath}{profile.DiskFileName}", profileJson);
        }
        public void SaveDeviceSpecificDataToDisk(DeviceSpecificData device)
        {
            // Save profile to disk
            string profileJson = JsonSerializer.Serialize(device, ControlAppJsonSerializerOptions);
            System.IO.File.WriteAllText($@"{DevicesFolderFullPath}{device.DeviceMac}.json", profileJson);

            TestFunctionSaveToDSHM(device.DatasContainter);
        }

        public void TestFunctionSaveToDSHM(BackingDataContainer dataContainer)
        {
            var dshm_data = new TestNewSaveFormat();
            dataContainer.ConvertAllToDSHM(dshm_data.Global);
            string profileJson = JsonSerializer.Serialize(dshm_data, ControlAppJsonSerializerOptions);
            System.IO.File.WriteAllText($@"C:\ProgramData\DsHidMini\DsHidMini.json", profileJson);
        }


        public DeviceSpecificData GetDeviceSpecificData(string deviceMac)
        {
            foreach(DeviceSpecificData dev in Devices)
            {
                if(dev.DeviceMac == deviceMac)
                {
                    return dev;
                }
            }
            var newDevice = new DeviceSpecificData(deviceMac);
            newDevice.DeviceMac = deviceMac;
            Devices.Add(newDevice);
            return newDevice;
        }

        public ProfileData? GetProfileData(Guid profileGuid)
        {
            foreach (ProfileData prof in Profiles)
            {
                if (prof.ProfileGuid == profileGuid)
                {
                    return prof;
                }
            }
            return null;
        }
    }

    public class ProfileData
    {
        private VMGroupsContainer vmGroupsContainer;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        private const string DefaultGuid = "00000000000000000000000000000000";
        public string ProfileName { get; set; }
        public Guid ProfileGuid { get; set; } = Guid.NewGuid();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string DiskFileName { get; set; }


        public BackingDataContainer DataContainer { get; set; } = new();

        public ProfileData()
        {
            /*
            ProfileGuid = Guid.NewGuid();
            DataContainer = BackingDataContainer.GetDefaultDatas();
            */
        }



        public static readonly ProfileData DefaultProfile = new()
        {
            ProfileName = "DSHM XInput",
            DiskFileName = "Default_DSHM_XInput",
            ProfileGuid = new Guid(DefaultGuid),
            DataContainer = new(),
        };

        public override string ToString()
        {
            return ProfileName;
        }

        public VMGroupsContainer GetProfileVMGroupsContainer()
        {
            if (vmGroupsContainer == null)
                vmGroupsContainer = new VMGroupsContainer(DataContainer);
            return vmGroupsContainer;
        }
    }

    public class DeviceSpecificData
    {
        public string DeviceMac { get; set; } = "0000000000";
        public string DeviceCustomName { get; set; } = "DualShock 3";
        public Guid GuidOfProfileToUse { get; set; } = new Guid();
        public SettingsModes SettingsMode { get; set; } = SettingsModes.Global;

        public BackingDataContainer DatasContainter { get; set; } = new();

        public DeviceSpecificData(string deviceMac)
        {    
            DeviceMac = deviceMac;
        }      

        public void SaveToDSHM(DSHM_Format_ContextSettings dshmContextData)
        {
            DatasContainter.modesUniqueData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.ledsData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.wirelessData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.sticksDZData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.rumbleGeneralData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.outRepData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.leftRumbleRescaleData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.rightVariableEmulData.SaveToDSHMSettings(dshmContextData);
        }
    }


}
