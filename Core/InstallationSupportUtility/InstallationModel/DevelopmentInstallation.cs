﻿namespace EnterpriseWebLibrary.InstallationSupportUtility.InstallationModel {
	public interface DevelopmentInstallation: ExistingInstallation {
		DevelopmentInstallationLogic DevelopmentInstallationLogic { get; }
		int CurrentMajorVersion { get; }
		int NextBuildNumber { get; }
	}
}