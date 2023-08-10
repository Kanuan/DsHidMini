using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.UserData
{
    public class ProfileData
    {
        private VMGroupsContainer vmGroupsContainer;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public static Guid DefaultGuid = new Guid("00000000000000000000000000000000");
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
            ProfileGuid = DefaultGuid,
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

        public void SaveToDSHM(DSHM_Format_Settings dshmContextData)
        {
            DatasContainter.modesUniqueData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.ledsData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.wirelessData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.sticksData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.rumbleGeneralData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.outRepData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.leftRumbleRescaleData.SaveToDSHMSettings(dshmContextData);
            DatasContainter.rightVariableEmulData.SaveToDSHMSettings(dshmContextData);
        }
    }
    internal class ControllersUserData
    {
        // ----------------------------------------------------------- FIELDS

        public static JsonSerializerOptions ControlAppJsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IncludeFields = true,

            Converters =
            {
                new JsonStringEnumConverter(),
                new DshmCustomJsonConverter(),

            }
        };

        private const string DISK = @"C:\";
        private const string CONTROL_APP_FOLDER_PATH_IN_DISK = @"ProgramData\DsHidMini\ControlApp\";
        private const string CONTROL_APP_SETTINGS_FILE_NAME = @"ControlAppSettings.json";
        private const string DSHM_FOLDER_PATH_IN_DISK = @"ProgramData\DsHidMini\";
        private const string PROFILE_FOLDER_NAME = @"Profiles\";
        private const string DEVICES_FOLDER_NAME = @"Devices\";

        public string DshmFolderFullPath { get; } = $@"{DISK}{DSHM_FOLDER_PATH_IN_DISK}";
        public string ProfilesFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{PROFILE_FOLDER_NAME}";
        public string DevicesFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{DEVICES_FOLDER_NAME}";
        public string ControlAppFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}";
        public string ControlAppSettingsFileFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{CONTROL_APP_SETTINGS_FILE_NAME}";

        public static Guid guid = new();
        private List<ProfileData> profiles = new();

        // ----------------------------------------------------------- AUTO-PROPERTIES

        private ControlAppGeneralSettings controlAppSettings { get; set; } = new();
        public DeviceSpecificData NewControllersDefault { get; set; } = new("0123456789"); //"Global"
        public List<DeviceSpecificData> Devices { get; set; } = new();

        // ----------------------------------------------------------- PROPERTIES

        public ProfileData GlobalProfile
        {
            get
            {
                ProfileData gp = GetProfile(controlAppSettings.GlobalProfileGuid);
                if(gp == null)
                {
                    controlAppSettings.GlobalProfileGuid = ProfileData.DefaultGuid;
                    gp = ProfileData.DefaultProfile;
                }
                return gp;
            }
            set
            {
                controlAppSettings.GlobalProfileGuid = value.ProfileGuid;
                SaveControlAppSettingsToDisk();
            }
        }
        public List<ProfileData> Profiles
        {
            get => profiles;
            set => profiles = value;
        }


        // ----------------------------------------------------------- CONSTRUCTOR

        public ControllersUserData()
        {
            Profiles = LoadProfilesFromDisk();
            Devices = LoadDevicesFromDisk();
            LoadControlAppSettingsFromDisk();
            SaveAllProfilesToDisk(Profiles);
        }

        private class ControlAppGeneralSettings
        {
            public Guid GlobalProfileGuid { get; set; } = ProfileData.DefaultGuid;
        }

        // ----------------------------------------------------------- METHODS

        private List<DeviceSpecificData> LoadDevicesFromDisk()
        {

            var devicesOnDisk = new List<DeviceSpecificData>();

            if(Directory.Exists(DevicesFolderFullPath))
            {
                string[] devicesPaths = Directory.GetFiles($@"{DevicesFolderFullPath}", "*.json");
                foreach (string devPath in devicesPaths)
                {
                    var dirName = new DirectoryInfo(devPath).Name;
                    var jsonText = System.IO.File.ReadAllText(devPath);

                    var data = JsonSerializer.Deserialize<DeviceSpecificData>(jsonText, ControlAppJsonSerializerOptions);
                    //data.DiskFileName = dirName;
                    devicesOnDisk.Add(data);
                }
            }
            return devicesOnDisk;
        }

        public List<ProfileData> LoadProfilesFromDisk()
        {
            var profilesOnDisk = new List<ProfileData>();
            profilesOnDisk.Add(ProfileData.DefaultProfile); // Include Default Profile on profile list
            if(Directory.Exists(ProfilesFolderFullPath))
            {
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
            }

            return profilesOnDisk;
        }

        public ProfileData GetProfile(Guid profileGuid)
        {
            ProfileData profile = null;

            foreach(ProfileData p in Profiles)
            {
                if(p.ProfileGuid == profileGuid)
                {
                    profile = p;
                    break;
                }
            }
            return profile;
        }
        
        public void LoadControlAppSettingsFromDisk()
        {
            if(File.Exists(ControlAppSettingsFileFullPath))
            {
                var jsonText = File.ReadAllText(ControlAppSettingsFileFullPath);
                controlAppSettings = JsonSerializer.Deserialize<ControlAppGeneralSettings>(jsonText, ControlAppJsonSerializerOptions);
            }
        }

        public void SaveControlAppSettingsToDisk()
        {
            string settingsJson = JsonSerializer.Serialize(controlAppSettings, ControlAppJsonSerializerOptions);

            System.IO.Directory.CreateDirectory(ControlAppFolderFullPath);
            System.IO.File.WriteAllText($@"{ControlAppSettingsFileFullPath}", settingsJson);
        }

        public void SaveAllProfilesToDisk(List<ProfileData> profiles)
        {
            foreach (ProfileData profile in profiles)
            {
                if(profile == ProfileData.DefaultProfile)
                {
                    continue;
                }
                SaveProfileToDisk(profile);
            }
        }

        public void CheckAndFixRepeatedProfileFilePath(ProfileData newProfile)
        {
            var existingProfilePaths = new List<string>();
            foreach (ProfileData profile in Profiles)
            {
                existingProfilePaths.Add(profile.DiskFileName);
            }

            string newNameForProfile = newProfile.DiskFileName;
            for (int i = 1; existingProfilePaths.Contains(newNameForProfile); i++)
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

            System.IO.Directory.CreateDirectory(ProfilesFolderFullPath);
            System.IO.File.WriteAllText($@"{ProfilesFolderFullPath}{profile.DiskFileName}", profileJson);
        }
        public void SaveDeviceSpecificDataToDisk(DeviceSpecificData device)
        {
            // Save profile to disk
            string profileJson = JsonSerializer.Serialize(device, ControlAppJsonSerializerOptions);

            System.IO.Directory.CreateDirectory(DevicesFolderFullPath);
            System.IO.File.WriteAllText($@"{DevicesFolderFullPath}{device.DeviceMac}.json", profileJson);

            TestFunctionSaveToDSHM();
        }

        public void TestFunctionSaveToDSHM()
        {
            var dshm_data = new DshmMainDataContainer();
            
            GlobalProfile.DataContainer.ConvertAllToDSHM(dshm_data.Global);
           
            foreach(DeviceSpecificData dev in Devices)
            {
                var temp = new DSHMDeviceCustomSettings();
                temp.DeviceAddress = dev.DeviceMac;

                switch (dev.SettingsMode)
                {
                    case SettingsModes.Custom:
                dev.DatasContainter.ConvertAllToDSHM(temp.CustomSettings);
                        break;

                    case SettingsModes.Profile:
                        ProfileData devprof = GetProfile(dev.GuidOfProfileToUse);
                        if(devprof == null)
                        {
                            continue; // If profile set for the controller does not exist anymore then leave settings blank so controller loads Global Profile
                        }
                        else
                        {
                            devprof.DataContainer.ConvertAllToDSHM(temp.CustomSettings);
                        }
                        break;

                    case SettingsModes.Global:
                    default:
                        continue;
                }
                dshm_data.Devices.Add(temp);
            }
            
            string profileJson = JsonSerializer.Serialize(dshm_data, ControlAppJsonSerializerOptions);

            System.IO.Directory.CreateDirectory(DshmFolderFullPath);
            System.IO.File.WriteAllText($@"{DshmFolderFullPath}DsHidMini.json", profileJson);
        }

        public void CreateNewProfile(string profileName)
        {
            ProfileData newProfile = new();
            newProfile.ProfileName = profileName;
            newProfile.DiskFileName = profileName + ".json";
            CheckAndFixRepeatedProfileFilePath(newProfile);
            SaveProfileToDisk(newProfile);
            Profiles.Add(newProfile);
        }

        public void DeleteProfile(ProfileData profile)
        {
            if (profile == ProfileData.DefaultProfile) // Must not save Default Profile to disk
            {
                return;
            }
            Profiles.Remove(profile);
            if(File.Exists($@"{ProfilesFolderFullPath}{profile.DiskFileName}"))
            {
                System.IO.File.Delete($@"{ProfilesFolderFullPath}{profile.DiskFileName}");
            }
        }


        public DeviceSpecificData GetDeviceData(string deviceMac)
        {
            foreach (DeviceSpecificData dev in Devices)
            {
                if (dev.DeviceMac == deviceMac)
                {
                    return dev;
                }
            }
            var newDevice = new DeviceSpecificData(deviceMac);
            newDevice.DeviceMac = deviceMac;
            Devices.Add(newDevice);
            return newDevice;
        }

    }
}