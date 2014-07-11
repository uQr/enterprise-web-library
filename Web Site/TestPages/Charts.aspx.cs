using System;
using System.Linq;
using RedStapler.StandardLibrary.EnterpriseWebFramework;

namespace EnterpriseWebLibrary.WebSite.TestPages {
	partial class Charts: EwfPage {
		partial class Info {
			public override string PageName { get { return "Chart"; } }
		}

		protected override void loadData() {
			ph.AddControlsReturnThis(
				new Chart(
					ChartType.Line,
					new ChartSetup( "This is the labels title", new[] { "Three", "One", "Two", "Four", "Five" }, new DataSeries( "Title", new[] { 3, 1, 2, 4, 5 } ) ),
					"The export name" ) );

			var random = new Random();
			var floatData = Enumerable.Range( 0, 20 ).Select( i => random.NextDouble() * 100 ).ToArray();

			ph.AddControlsReturnThis(
				new Chart(
					ChartType.Line,
					new ChartSetup( "Floating point numbers", floatData.Select( ( f, i ) => "" + i ), new DataSeries( "The value", floatData ) ),
					"The export name" ) );

			var floatData1 = Enumerable.Range( 0, 20 ).Select( i => random.NextDouble() * 100 ).ToArray();
			var floatData2 = Enumerable.Range( 0, 20 ).Select( i => random.NextDouble() * 100 ).ToArray();

			ph.AddControlsReturnThis(
				new Chart(
					ChartType.Line,
					new ChartSetup(
						"Two lines of Floating point numbers",
						floatData1.Select( ( f, i ) => "" + i ),
						new[] { new DataSeries( "First values ", floatData1 ), new DataSeries( "Second values ", floatData2 ) } ),
					"The export name" ) );

			var floatData3 = Enumerable.Range( 0, 10 ).Select( i => random.NextDouble() * 50 ).ToArray();

			ph.AddControlsReturnThis(
				new Chart(
					ChartType.Bar,
					new ChartSetup( "Bar graph", floatData1.Select( ( f, i ) => "" + i ), new DataSeries( "Values", floatData3 ) ),
					"The export name" ) );


			var floatData4 = Enumerable.Range( 0, 20 ).Select( i => random.NextDouble() * 100 ).ToArray();
			var floatData5 = Enumerable.Range( 0, 20 ).Select( i => random.NextDouble() * 100 ).ToArray();

			ph.AddControlsReturnThis(
				new Chart(
					ChartType.Bar,
					new ChartSetup(
						"Two bars of Floating point numbers",
						floatData4.Select( ( f, i ) => "" + i ),
						new[] { new DataSeries( "First values ", floatData4 ), new DataSeries( "Second values ", floatData5 ) } ),
					"The export name" ) );
		}
	}
}