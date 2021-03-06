﻿using EnterpriseWebLibrary.DataAccess.CommandWriting.InlineConditionAbstraction;

namespace EnterpriseWebLibrary.DataAccess {
	/// <summary>
	/// A database command condition associated with a particular table.
	/// </summary>
	public interface TableCondition {
		/// <summary>
		/// Gets the underlying condition for this table condition.
		/// </summary>
		InlineDbCommandCondition CommandCondition { get; }
	}
}