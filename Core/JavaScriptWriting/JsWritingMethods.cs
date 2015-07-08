using System;
using System.Web.UI;
using Humanizer;

namespace RedStapler.StandardLibrary.JavaScriptWriting {
	/// <summary>
	/// A collection of methods that create JavaScript and/or add it to web controls.
	/// </summary>
	public static class JsWritingMethods {
		//Form Events

		/// <summary>
		/// JavaScript onblur event.
		/// </summary>
		public const string onblur = "onblur";

		/// <summary>
		/// JavaScript onchange event.
		/// </summary>
		public const string onchange = "onchange";

		/// <summary>
		/// JavaScript onfocus event.
		/// </summary>
		public const string onfocus = "onfocus";

		/// <summary>
		/// JavaScript onkeydown event.
		/// </summary>
		public const string onkeydown = "onkeydown";

		/// <summary>
		/// JavaScript onkeypress event.
		/// </summary>
		public const string onkeypress = "onkeypress";

		/// <summary>
		/// JavaScript onkeyup event.
		/// </summary>
		public const string onkeyup = "onkeyup";

		/// <summary>
		/// JavaScript onselect event.
		/// </summary>
		public const string onselect = "onselect";

		/// <summary>
		/// JavaScript onreset event.
		/// </summary>
		public const string onreset = "onreset";

		//Mouse Events

		/// <summary>
		/// /// JavaScript onclick event.
		/// </summary>
		public const string onclick = "onclick";

		/// <summary>
		/// /// JavaScript onmousedown event.
		/// </summary>
		public const string onmousedown = "onmousedown";

		/// <summary>
		/// /// JavaScript onmousemove event.
		/// </summary>
		public const string onmousemove = "onmousemove";

		/// <summary>
		/// /// JavaScript onmouseout event.
		/// </summary>
		public const string onmouseout = "onmouseout";

		/// <summary>
		/// /// JavaScript onmouseover event.
		/// </summary>
		public const string onmouseover = "onmouseover";

		/// <summary>
		/// /// JavaScript onmouseup event.
		/// </summary>
		public const string onmouseup = "onmouseup";

		/// <summary>
		/// /// JavaScript ondblclick event.
		/// </summary>
		public const string ondblclick = "ondblclick";

		/// <summary>
		/// /// JavaScript onerror event.
		/// </summary>
		public const string onerror = "onerror";

		/// <summary>
		/// Returns a script that can be used to open a pop up window with the specified url and settings.
		/// </summary>
		public static string GetPopUpWindowScript( string url, Control urlResolver, PopUpWindowSettings settings ) {
			Func<bool, string> boolToYesNo = b => b ? "yes" : "no";
			return
				"var popUpWindow = window.open( '{0}', '{1}', 'width={2},height={3},toolbar={4},location={5},status=no,resizable={6},scrollbars={7}' ); popUpWindow.focus();"
					.FormatWith(
						urlResolver.GetClientUrl( url ),
						settings.Name,
						settings.Width,
						settings.Height,
						boolToYesNo( settings.ShowsNavigationToolbar ),
						boolToYesNo( settings.ShowsLocationBar ),
						boolToYesNo( settings.IsResizable ),
						boolToYesNo( settings.ShowsScrollBarsWhenNecessary ) );
		}
	}
}