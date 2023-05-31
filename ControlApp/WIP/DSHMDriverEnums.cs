using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.DSHM_Settings
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

    public enum DSHM_LEDsAuthority
    {
        Automatic,
        Driver,
        Application,
    }
}
