using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public enum SettingsContext
    {
        Global,
        General,
        SDF,
        GPJ,
        DS4W,
        XInput,
    }

    public enum SettingsModeGroups
    {
        LEDsControl,
        /// <summary>
        ///     WirelessIdleTimeoutPeriodMs  <br/>
        ///     QuickDisconnectCombo  <br/>
        ///     QuickDisconnectHoldTime  <br/>
        /// 	DisableWirelessIdleTimeout  <br/>
        /// </summary>
        WirelessSettings,
        /// <summary>
        /// 	IsOutputRateControlEnabled  <br/>
        /// 	OutputRateControlPeriodMs  <br/>
        /// 	IsOutputDeduplicatorEnabled  <br/>
        /// </summary>
        OutputReportControl,
        /// <summary>
        /// 	ThumbSettings.DeadZoneLeft.Apply <br/>
        ///     ThumbSettings.DeadZoneLeft.PolarValue <br/>
        ///     ThumbSettings.DeadZoneRight.Apply <br/>
        ///     ThumbSettings.DeadZoneRight.PolarValue <br/>
        /// </summary>
        SticksDeadzone,
        /// <summary>
        /// 	RumbleSettings.SMToBMConversion.Enabled <br/>
        /// 	RumbleSettings.DisableBM <br/>
        ///     RumbleSettings.DisableSM <br/>
        /// </summary>
        RumbleGeneral,
        /// <summary>
        /// 	RumbleSettings.BMStrRescale.Enabled <br/>
        ///     RumbleSettings.BMStrRescale.MinValue <br/>
        ///     RumbleSettings.BMStrRescale.MaxValue <br/>
        /// </summary>
        RumbleLeftStrRescale,
        /// <summary>
        ///     RumbleSettings.SMToBMConversion.RescaleMinValue <br/>
        ///     RumbleSettings.SMToBMConversion.RescaleMaxValue <br/>
        ///     RumbleSettings.ForcedSM.BMThresholdEnabled <br/>
        ///     RumbleSettings.ForcedSM.BMThresholdValue <br/>
        ///     RumbleSettings.ForcedSM.SMThresholdEnabled <br/>
        ///     RumbleSettings.ForcedSM.SMThresholdValue <br/>
        /// </summary>
        RumbleRightConversion,
        Unique_Global,
        Unique_General,
        /// <summary>
        ///     SDF.PressureExposureMode <br/>
        ///     SDF.DPadExposureMode <br/>
        /// </summary>
        Unique_SDF,
        /// <summary>
        /// 	GPJ.PressureExposureMode <br/>
        ///     GPJ.DPadExposureMode <br/>
        /// </summary>
        Unique_GPJ,
        Unique_DS4W,
        Unique_XInput,
    }

    public enum LEDsModes
    {
        BatterySingleLED,
        BatteryFillingBar,
        CustomSimple,
        CustomComplete,
    }

    public enum QuickDisconnectCombo
    {
        Disabled,
        PS_R1_L1,
        PS_Start,
        PS_Select,
        Start_R1_L1,
        Select_R1_L1,
        Start_Select,
    }

    public enum DsPressureExposureMode
    {
        DsPressureExposureModeDigital,
        DsPressureExposureModeAnalogue,
        DsPressureExposureModeBoth,
    }

    public enum DS_DPAD_EXPOSURE_MODE
    {
        DsDPadExposureModeHAT,
        DsDPadExposureModeIndividualButtons,
        DsDPadExposureModeBoth,
    }
}
