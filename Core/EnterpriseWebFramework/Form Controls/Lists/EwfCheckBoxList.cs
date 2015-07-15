﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EnterpriseWebLibrary.EnterpriseWebFramework.Controls;

namespace EnterpriseWebLibrary.EnterpriseWebFramework {
	// We can't nest this inside the class below because of the type parameter.
	internal class CheckBoxListCssElementCreator: ControlCssElementCreator {
		internal const string CssClass = "ewfStandardCheckBoxList";

		CssElement[] ControlCssElementCreator.CreateCssElements() {
			return new[] { new CssElement( "CheckBoxList", "div." + CssClass ) };
		}
	}

	/// <summary>
	/// A check box list that allows multiple items to be selected.
	/// NOTE: Consider using something like the multi select feature of http://harvesthq.github.com/chosen/ to provide a space-saving mode for this control.
	/// </summary>
	public class EwfCheckBoxList<ItemIdType>: WebControl, ControlTreeDataLoader, ControlWithCustomFocusLogic {
		private readonly IEnumerable<SelectListItem<ItemIdType>> items;
		private readonly IEnumerable<ItemIdType> selectedItemIds;
		private readonly string caption;
		private readonly bool includeSelectAndDeselectAllButtons;
		private readonly byte numberOfColumns;
		private readonly PostBack postBack;

		private readonly Dictionary<SelectListItem<ItemIdType>, BlockCheckBox> checkBoxesByItem = new Dictionary<SelectListItem<ItemIdType>, BlockCheckBox>();

		/// <summary>
		/// Creates a check box list.
		/// </summary>
		public EwfCheckBoxList(
			IEnumerable<SelectListItem<ItemIdType>> items, IEnumerable<ItemIdType> selectedItemIds, string caption = "", bool includeSelectAndDeselectAllButtons = false,
			byte numberOfColumns = 1, PostBack postBack = null ) {
			this.items = items.ToArray();
			this.selectedItemIds = selectedItemIds.ToArray();
			this.caption = caption;
			this.includeSelectAndDeselectAllButtons = includeSelectAndDeselectAllButtons;
			this.numberOfColumns = numberOfColumns;
			this.postBack = postBack;
		}

		void ControlTreeDataLoader.LoadData() {
			CssClass = CssClass.ConcatenateWithSpace( CheckBoxListCssElementCreator.CssClass );

			var table = new DynamicTable { Caption = caption };
			if( includeSelectAndDeselectAllButtons ) {
				table.AddActionLink( new ActionButtonSetup( "Select All", new CustomButton( () => string.Format( @"toggleCheckBoxes( '{0}', true )", ClientID ) ) ) );
				table.AddActionLink( new ActionButtonSetup( "Deselect All", new CustomButton( () => string.Format( @"toggleCheckBoxes( '{0}', false )", ClientID ) ) ) );
			}

			var itemsPerColumn = (int)Math.Ceiling( (decimal)items.Count() / numberOfColumns );
			var cells = new List<EwfTableCell>();
			for( byte i = 0; i < numberOfColumns; i += 1 ) {
				var maxIndex = Math.Min( ( i + 1 ) * itemsPerColumn, items.Count() );
				var place = new PlaceHolder();
				for( var j = i * itemsPerColumn; j < maxIndex; j += 1 ) {
					var item = items.ElementAt( j );
					var checkBox = new BlockCheckBox( selectedItemIds.Contains( item.Id ), label: item.Label, highlightWhenChecked: true, postBack: postBack );
					place.Controls.Add( checkBox );
					checkBoxesByItem.Add( item, checkBox );
				}
				cells.Add( place );
			}
			table.AddRow( cells.ToArray() );
			Controls.Add( table );
		}

		void ControlWithCustomFocusLogic.SetFocus() {
			if( items.Any() )
				( checkBoxesByItem[ items.First() ] as ControlWithCustomFocusLogic ).SetFocus();
		}

		/// <summary>
		/// Gets the selected item IDs in the post back.
		/// </summary>
		public IEnumerable<ItemIdType> GetSelectedItemIdsInPostBack( PostBackValueDictionary postBackValues ) {
			return items.Where( i => checkBoxesByItem[ i ].IsCheckedInPostBack( postBackValues ) ).Select( i => i.Id ).ToArray();
		}

		/// <summary>
		/// Returns true if the selections changed on this post back.
		/// </summary>
		public bool SelectionsChangedOnPostBack( PostBackValueDictionary postBackValues ) {
			return items.Any( i => checkBoxesByItem[ i ].ValueChangedOnPostBack( postBackValues ) );
		}

		/// <summary>
		/// Returns the tag that represents this control in HTML.
		/// </summary>
		protected override HtmlTextWriterTag TagKey { get { return HtmlTextWriterTag.Div; } }
	}
}