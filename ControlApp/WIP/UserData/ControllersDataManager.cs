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
