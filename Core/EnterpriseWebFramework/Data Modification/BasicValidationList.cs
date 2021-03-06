﻿using System.Collections.Generic;

namespace EnterpriseWebLibrary.EnterpriseWebFramework {
	/// <summary>
	/// A validation list that allows its validations to be added to other validation lists. Can be useful if validations need to be added to multiple data
	/// modifications or if you want to defer the adding of validations to a data modification.
	/// </summary>
	public class BasicValidationList: ValidationList, ValidationListInternal {
		private readonly List<EwfValidation> validations = new List<EwfValidation>();

		void ValidationListInternal.AddValidation( EwfValidation validation ) {
			validations.Add( validation );
		}

		/// <summary>
		/// Adds all validations from the specified list.
		/// </summary>
		public void AddValidations( BasicValidationList validationList ) {
			validations.AddRange( validationList.validations );
		}

		internal IEnumerable<EwfValidation> Validations { get { return validations; } }
	}
}