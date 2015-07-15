﻿using System.Collections.Generic;

namespace EnterpriseWebLibrary.MailMerging.RowTree {
	/// <summary>
	/// A row in a merge row tree.
	/// </summary>
	public class MergeRow {
		/// <summary>
		/// Gets values for the merge fields at this tree depth, i.e. the fields that correspond to the data type of this row.
		/// </summary>
		public IEnumerable<MergeValue> Values { get; private set; }

		/// <summary>
		/// Gets the children of this row, each of which is the child rows for a specific data type.
		/// </summary>
		public IEnumerable<MergeRowTree> Children { get; private set; }

		internal MergeRow( IEnumerable<MergeValue> values, IEnumerable<MergeRowTree> children ) {
			Values = values;
			Children = children;
		}
	}
}