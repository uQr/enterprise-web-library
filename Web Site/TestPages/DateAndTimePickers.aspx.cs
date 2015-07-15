using System;
using EnterpriseWebLibrary.EnterpriseWebFramework;
using EnterpriseWebLibrary.EnterpriseWebFramework.Controls;

namespace EnterpriseWebLibrary.WebSite.TestPages {
	partial class DateAndTimePickers: EwfPage {
		partial class Info {
			public override string ResourceName { get { return "Date/Time Pickers"; } }
		}

		protected override void loadData() {
			var table = FormItemBlock.CreateFormItemTable();
			table.AddFormItems(
				FormItem.Create( "Date Picker", new DatePicker( null ) ),
				FormItem.Create( "Time Picker", new TimePicker( null ) ),
				FormItem.Create( "Date/Time Picker", new DateTimePicker( null ) ),
				FormItem.Create( "Duration Picker", new DurationPicker( TimeSpan.Zero ) ) );
			ph.AddControlsReturnThis( table );
		}

		public override bool IsAutoDataUpdater { get { return true; } }
	}
}