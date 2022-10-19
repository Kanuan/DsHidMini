using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
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

        public override void CopySettingsFromBackingData(SettingsBackingData wirelessData, bool invertCopyDirection = false)
        {
            base.CopySettingsFromBackingData(wirelessData, invertCopyDirection);
            var specific = (BackingData_Wireless)wirelessData;

            if (invertCopyDirection)
            {
                specific.IsGroupEnabled = this.IsGroupEnabled;

                specific.IsQuickDisconnectComboEnabled = this.IsQuickDisconnectComboEnabled;
                specific.IsWirelessIdleDisconnectEnabled = this.IsWirelessIdleDisconnectEnabled;
                specific.QuickDisconnectCombo.copyCombo(this.QuickDisconnectCombo);
                specific.WirelessIdleDisconnectTime = this.WirelessIdleDisconnectTime;
            }
            else
            {
                this.IsGroupEnabled = specific.IsGroupEnabled;

                this.IsQuickDisconnectComboEnabled = specific.IsQuickDisconnectComboEnabled;
                this.IsWirelessIdleDisconnectEnabled = specific.IsWirelessIdleDisconnectEnabled;
                this.QuickDisconnectCombo.copyCombo(specific.QuickDisconnectCombo);
                this.WirelessIdleDisconnectTime = specific.WirelessIdleDisconnectTime;
            }

        }
    }


}
