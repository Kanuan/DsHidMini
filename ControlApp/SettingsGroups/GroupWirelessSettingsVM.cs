using Nefarius.DsHidMini.ControlApp.JsonSettings;
using ReactiveUI.Fody.Helpers;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupWirelessSettingsVM : GroupSettingsVM
    {
        // -------------------------------------------- WIRELESS SETTINGS GROUP
        public bool DEFAULT_isWirelessIdleDisconnectEnabled = true;
        public byte DEFAULT_wirelessIdleDisconnectTime = 5;
        public bool DEFAULT_isQuickDisconnectComboEnabled = true;
        public static readonly ButtonsCombo DEFAULT_disconnectCombo = new()
        {
            Button1 = ControlApp_ComboButtons.PS,
            Button2 = ControlApp_ComboButtons.R1,
            Button3 = ControlApp_ComboButtons.L1,
        };
        public const byte DEFAULT_disconnectComboHoldTime = 3;


        public override SettingsModeGroups Group { get; } = SettingsModeGroups.WirelessSettings;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public bool IsWirelessIdleDisconnectEnabled { get; set; }
        [Reactive] public byte WirelessIdleDisconnectTime { get; set; }
        [Reactive] public bool IsQuickDisconnectComboEnabled { get; set; }
        [Reactive] public ButtonsCombo QuickDisconnectCombo { get; set; } = new();

        public GroupWirelessSettingsVM(SettingsContext context, SettingsContainer containter) : base(context, containter)
        {

        }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();
            IsWirelessIdleDisconnectEnabled = DEFAULT_isWirelessIdleDisconnectEnabled;
            WirelessIdleDisconnectTime = DEFAULT_wirelessIdleDisconnectTime;
            IsQuickDisconnectComboEnabled = DEFAULT_isQuickDisconnectComboEnabled;

            QuickDisconnectCombo.copyCombo(DEFAULT_disconnectCombo);
        }

        public override void SaveToDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {

            if (!this.IsGroupEnabled)
            {
                dshmContextSettings.DisableWirelessIdleTimeout = null;
                dshmContextSettings.WirelessIdleTimeoutPeriodMs = null;
                //dshmContextSettings.QuickDisconnectCombo = null;
                return;
            }

            dshmContextSettings.DisableWirelessIdleTimeout = !this.IsWirelessIdleDisconnectEnabled;
            dshmContextSettings.WirelessIdleTimeoutPeriodMs = this.WirelessIdleDisconnectTime * 60 * 1000;
            //dshmContextSettings.QuickDisconnectCombo = dictionary combo pair;
        }

        public override void LoadFromDSHMSettings(DSHM_Format_ContextSettings dshmContextSettings)
        {
            if(
                dshmContextSettings.DisableWirelessIdleTimeout == null
                // check for combo too
                || dshmContextSettings.WirelessIdleTimeoutPeriodMs == null)
            {
                this.IsGroupEnabled = false;
                return;
            }
            this.IsGroupEnabled = true;

            this.IsWirelessIdleDisconnectEnabled = !dshmContextSettings.DisableWirelessIdleTimeout.GetValueOrDefault();
            this.WirelessIdleDisconnectTime =
                (byte)(dshmContextSettings.WirelessIdleTimeoutPeriodMs.GetValueOrDefault() / (60.0 * 1000) );
            // this.combo...
        }
    }


}
