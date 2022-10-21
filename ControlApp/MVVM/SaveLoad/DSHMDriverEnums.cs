using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.DSHM_JsonData_Json
{
    public enum DSHM_HidDeviceModes
    {
        SDF,
        GPJ,
        SXS,
        DS4Windows,
        XInput,
    }

    public enum DSHM_PressureModes
    {
        Digital,
        Analogue,
        Default,
    }

    public enum DSHM_DPadExposureModes
    {
        HAT,
        IndividualButtons,
        Default,
    }

    public enum DSHM_LEDsModes
    {
        BatteryIndicatorPlayerIndex,
        BatteryIndicatorBarGraph,
        CustomPattern,
    }

    public enum DSHM_QuickDisconnectCombo
    {
        PS_R1_L1,
        PS_Start,
        PS_Select,
        Start_R1_L1,
        Select_R1_L1,
        Start_Select,
    }

    public enum DSHM_ComboButtons
    {
        PS,
        START,
        SELECT,
        R1,
        L1,
        R2,
        L2,
        R3,
        L3,
        Triangle,
        Circle,
        Cross,
        Square,
        Up,
        Right,
        Dowm,
        Left,
    }

    public class SaveLoadUtils
    {
        public static Dictionary<SettingsContext, DSHM_HidDeviceModes> Get_DSHM_HIDDeviceMode_From_ControlApp = new()
        {
            {SettingsContext.Global , DSHM_HidDeviceModes.XInput},
            {SettingsContext.General , DSHM_HidDeviceModes.XInput},
            {SettingsContext.SDF , DSHM_HidDeviceModes.SDF},
            {SettingsContext.GPJ , DSHM_HidDeviceModes.GPJ},
            {SettingsContext.SXS , DSHM_HidDeviceModes.SXS},
            {SettingsContext.DS4W , DSHM_HidDeviceModes.DS4Windows},
            {SettingsContext.XInput , DSHM_HidDeviceModes.XInput},
        };

        //---------------------------------------------------- LEDsModes

        public static Dictionary<ControlApp_LEDsModes, DSHM_LEDsModes> Get_DSHM_LEDModes_From_ControlApp = new()
        {
            { ControlApp_LEDsModes.BatteryIndicatorPlayerIndex, DSHM_LEDsModes.BatteryIndicatorPlayerIndex },
            { ControlApp_LEDsModes.BatteryIndicatorBarGraph, DSHM_LEDsModes.BatteryIndicatorBarGraph },
            { ControlApp_LEDsModes.CustomStatic, DSHM_LEDsModes.CustomPattern },
            { ControlApp_LEDsModes.CustomPattern, DSHM_LEDsModes.CustomPattern },
        };

        public static Dictionary<DSHM_LEDsModes, ControlApp_LEDsModes> Get_ControlApp_LEDModes_From_DSHM = new()
        {
            { DSHM_LEDsModes.BatteryIndicatorPlayerIndex, ControlApp_LEDsModes.BatteryIndicatorPlayerIndex },
            { DSHM_LEDsModes.BatteryIndicatorBarGraph, ControlApp_LEDsModes.BatteryIndicatorBarGraph },
            { DSHM_LEDsModes.CustomPattern, ControlApp_LEDsModes.CustomPattern },
        };

        //---------------------------------------------------- DPadModes

        public static Dictionary<ControlApp_DPADModes, DSHM_DPadExposureModes> Get_DSHM_DPadMode_From_ControlApp = new()
        {
            { ControlApp_DPADModes.Default, DSHM_DPadExposureModes.Default },
            { ControlApp_DPADModes.HAT, DSHM_DPadExposureModes.HAT },
            { ControlApp_DPADModes.IndividualButtons, DSHM_DPadExposureModes.IndividualButtons },
        };

        public static Dictionary<DSHM_DPadExposureModes, ControlApp_DPADModes> Get_ControlApp_DPadMode_From_DSHM = new()
        {
            { DSHM_DPadExposureModes.Default, ControlApp_DPADModes.Default },
            { DSHM_DPadExposureModes.HAT, ControlApp_DPADModes.HAT },
            { DSHM_DPadExposureModes.IndividualButtons, ControlApp_DPADModes.IndividualButtons },
        };

        //---------------------------------------------------- PressureModes

        public static Dictionary<ControlApp_DsPressureMode, DSHM_PressureModes> Get_DSHM_DsPressureMode_From_ControlApp = new()
        {
            { ControlApp_DsPressureMode.Default, DSHM_PressureModes.Default },
            { ControlApp_DsPressureMode.Analogue, DSHM_PressureModes.Analogue },
            { ControlApp_DsPressureMode.Digital, DSHM_PressureModes.Digital },
        };

        public static Dictionary<DSHM_PressureModes, ControlApp_DsPressureMode> Get_ControlApp_DsPressureMode_From_DSHM = new()
        {
            { DSHM_PressureModes.Default, ControlApp_DsPressureMode.Default },
            { DSHM_PressureModes.Analogue, ControlApp_DsPressureMode.Analogue },
            { DSHM_PressureModes.Digital, ControlApp_DsPressureMode.Digital },
        };



        public static Dictionary<ControlApp_QuickDisconnectCombo, DSHM_QuickDisconnectCombo> QuickDisconnectCombo_Control_DSHM_Pair = new()
        {
            { ControlApp_QuickDisconnectCombo.PS_R1_L1, DSHM_QuickDisconnectCombo.PS_R1_L1 },
            { ControlApp_QuickDisconnectCombo.PS_Start, DSHM_QuickDisconnectCombo.PS_Start },
            { ControlApp_QuickDisconnectCombo.PS_Select, DSHM_QuickDisconnectCombo.PS_Select },
            { ControlApp_QuickDisconnectCombo.Start_R1_L1, DSHM_QuickDisconnectCombo.Start_R1_L1 },
            { ControlApp_QuickDisconnectCombo.Select_R1_L1, DSHM_QuickDisconnectCombo.Select_R1_L1 },
            { ControlApp_QuickDisconnectCombo.Start_Select, DSHM_QuickDisconnectCombo.Start_Select },
        };
    }
}
