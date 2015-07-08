﻿namespace RedStapler.StandardLibrary.InstallationSupportUtility.InstallationModel {
	public interface RecognizedInstallation: ExistingInstallation, KnownInstallation {
		RecognizedInstallationLogic RecognizedInstallationLogic { get; }
	}
}