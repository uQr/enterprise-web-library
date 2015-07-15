﻿namespace EnterpriseWebLibrary.EnterpriseWebFramework {
	/// <summary>
	/// Used to define common attributes and methods among check boxes.
	/// </summary>
	public interface CommonCheckBox {
		bool IsRadioButton { get; }

		/// <summary>
		/// Gets whether the box was created in a checked state.
		/// </summary>
		bool IsChecked { get; }

		/// <summary>
		/// Internal use only. This property is only used by CheckBoxToControlArrayDisplayLink and will be deleted.
		/// </summary>
		string GroupName { get; }

		/// <summary>
		/// Method to insert a Javascript onclick event.
		/// </summary>
		void AddOnClickJsMethod( string s );

		/// <summary>
		/// Gets whether the box is checked in the post back.
		/// </summary>
		bool IsCheckedInPostBack( PostBackValueDictionary postBackValues );

		/// <summary>
		/// Returns true if the value changed on this post back.
		/// </summary>
		bool ValueChangedOnPostBack( PostBackValueDictionary postBackValues );
	}
}