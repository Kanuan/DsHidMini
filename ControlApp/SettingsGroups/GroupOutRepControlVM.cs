using Nefarius.DsHidMini.ControlApp.JsonSettings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupOutRepControlVM : GroupSettingsVM
    {
        // --------------------------------------------  OUTPUT REPORT CONTROL GROUP
        public const bool DEFAULT_isOutputReportRateControlEnabled = true;
        public const byte DEFAULT_maxOutputRate = 150;
        public const bool DEFAULT_isOutputReportDeduplicatorEnabled = false;
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.OutputReportControl;
        [Reactive] public bool IsGroupEnabled { get; set; }
        [Reactive] public bool IsOutputReportRateControlEnabled { get; set; }
        [Reactive] public byte MaxOutputRate { get; set; }
        [Reactive] public bool IsOutputReportDeduplicatorEnabled { get; set; }

        public GroupOutRepControlVM(SettingsContext context, SettingsContainer containter) : base(context, containter) { }

        public override void ResetGroupToOriginalDefaults()
        {
            IsGroupEnabled = ShouldGroupBeEnabledOnReset();

            IsOutputReportRateControlEnabled = DEFAULT_isOutputReportRateControlEnabled;
            MaxOutputRate = DEFAULT_maxOutputRate;
            IsOutputReportDeduplicatorEnabled = DEFAULT_isOutputReportDeduplicatorEnabled;
        }

        public override void CopySettingsFromBackingData(SettingsBackingData outRepBackingData, bool invertCopyDirection = false)
        {
            base.CopySettingsFromBackingData(outRepBackingData, invertCopyDirection);

            var specific = (BackingData_OutRepControl)outRepBackingData;

            if (invertCopyDirection)
            {
                specific.IsGroupEnabled = this.IsGroupEnabled;
                specific.IsOutputReportDeduplicatorEnabled = this.IsOutputReportDeduplicatorEnabled;
                specific.IsOutputReportRateControlEnabled = this.IsOutputReportRateControlEnabled;
                specific.MaxOutputRate = this.MaxOutputRate;
            }
            else
            {
                this.IsGroupEnabled = specific.IsGroupEnabled;
                this.IsOutputReportDeduplicatorEnabled = specific.IsOutputReportDeduplicatorEnabled;
                this.IsOutputReportRateControlEnabled = specific.IsOutputReportRateControlEnabled;
                this.MaxOutputRate = specific.MaxOutputRate;
            }
        }
    }


}
