using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Nefarius.DsHidMini.ControlApp.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.WIP.UserData
{
    internal class ControllersDataManager
    {
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
                DatasContainter.sticksData.SaveToDSHMSettings(dshmContextData);
                DatasContainter.rumbleGeneralData.SaveToDSHMSettings(dshmContextData);
                DatasContainter.outRepData.SaveToDSHMSettings(dshmContextData);
                DatasContainter.leftRumbleRescaleData.SaveToDSHMSettings(dshmContextData);
                DatasContainter.rightVariableEmulData.SaveToDSHMSettings(dshmContextData);
            }
        }
    }
}
