using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.JsonSettings
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
        public static Dictionary<ControlApp_LEDsModes, DSHM_LEDsModes> LEDModes_Control_DSHM_Pair = new()
        {
            { ControlApp_LEDsModes.BatteryIndicatorPlayerIndex, DSHM_LEDsModes.BatteryIndicatorPlayerIndex },
            { ControlApp_LEDsModes.BatteryIndicatorBarGraph, DSHM_LEDsModes.BatteryIndicatorBarGraph },
            { ControlApp_LEDsModes.CustomStatic, DSHM_LEDsModes.CustomPattern },
            { ControlApp_LEDsModes.CustomPattern, DSHM_LEDsModes.CustomPattern },
        };

        public static Dictionary<ControlApp_DPADModes, DSHM_DPadExposureModes> DPadModes_Control_DSHM_Pair = new()
        {
            { ControlApp_DPADModes.Default, DSHM_DPadExposureModes.Default },
            { ControlApp_DPADModes.HAT, DSHM_DPadExposureModes.HAT },
            { ControlApp_DPADModes.IndividualButtons, DSHM_DPadExposureModes.IndividualButtons },
        };

        public static Dictionary<ControlApp_DsPressureMode, DSHM_PressureModes> PressureModes_Control_DSHM_Pair = new()
        {
            { ControlApp_DsPressureMode.Both, DSHM_PressureModes.Default },
            { ControlApp_DsPressureMode.DsPressureExposureModeAnalogue, DSHM_PressureModes.Analogue },
            { ControlApp_DsPressureMode.DsPressureExposureModeDigital, DSHM_PressureModes.Digital },
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
