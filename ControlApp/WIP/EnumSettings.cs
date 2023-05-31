using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public enum SettingsModes
    {
        Global,
        Profile,
        Custom,
    }
    public enum SettingsContext
    {
        Global,
        General,
        SDF,
        GPJ,
        SXS,
        DS4W,
        XInput,
    }

    public enum SettingsModeGroups
    {
        LEDsControl,
        WirelessSettings,
        OutputReportControl,
        SticksDeadzone,
        RumbleGeneral,
        RumbleLeftStrRescale,
        RumbleRightConversion,
        Unique_All,
        Unique_Global,
        Unique_General,
        Unique_SDF,
        Unique_GPJ,
        Unique_SXS,
        Unique_DS4W,
        Unique_XInput,
    }

    public enum ControlApp_LEDsModes
    {
        BatteryIndicatorPlayerIndex,
        BatteryIndicatorBarGraph,
        CustomStatic,
        CustomPattern,
    }

    public enum ControlApp_QuickDisconnectCombo
    {
        PS_R1_L1,
        PS_Start,
        PS_Select,
        Start_R1_L1,
        Select_R1_L1,
        Start_Select,
    }

    public enum ControlApp_ComboButtons
    {
        None,
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

    public enum ControlApp_DsPressureMode
    {
        Digital,
        Analogue,
        Default,
    }

    public enum ControlApp_DPADModes
    {
        Default,
        HAT,
        Buttons,
    }
}
