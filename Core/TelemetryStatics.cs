﻿using System;
using System.IO;
using System.ServiceModel;
using System.Web;
using Humanizer;
using EnterpriseWebLibrary.Configuration;
using EnterpriseWebLibrary.Email;
using EnterpriseWebLibrary.EnterpriseWebFramework;
using EnterpriseWebLibrary.EnterpriseWebFramework.UserManagement;

namespace EnterpriseWebLibrary {
	public static class TelemetryStatics {
		/// <summary>
		/// Reports an error to the developers. The report includes the specified exception and additional information about the running program.
		/// </summary>
		public static void ReportError( Exception e ) {
			ReportError( "", e );
		}

		/// <summary>
		/// Reports an error to the developers. The report includes the specified exception and additional information about the running program. The prefix
		/// provides additional information before the standard exception and page information.
		/// </summary>
		public static void ReportError( string prefix, Exception exception ) {
			using( var sw = new StringWriter() ) {
				if( prefix.Length > 0 ) {
					sw.WriteLine( prefix );
					sw.WriteLine();
				}
				if( exception != null ) {
					sw.WriteLine( exception.ToString() );
					sw.WriteLine();
				}

				if( NetTools.IsWebApp() ) {
					// This check ensures that there is an actual request, which is not the case during application initialization.
					if( EwfApp.Instance != null && EwfApp.Instance.RequestState != null ) {
						sw.WriteLine( "URL: " + AppRequestState.Instance.Url );

						sw.WriteLine();
						foreach( string fieldName in HttpContext.Current.Request.Form )
							sw.WriteLine( "Form field " + fieldName + ": " + HttpContext.Current.Request.Form[ fieldName ] );

						sw.WriteLine();
						foreach( string cookieName in HttpContext.Current.Request.Cookies )
							sw.WriteLine( "Cookie " + cookieName + ": " + HttpContext.Current.Request.Cookies[ cookieName ].Value );

						sw.WriteLine();
						sw.WriteLine( "User agent: " + HttpContext.Current.Request.GetUserAgent() );
						sw.WriteLine( "Referrer: " + NetTools.ReferringUrl );

						User user = null;
						User impersonator = null;

						// exception-prone code
						try {
							user = AppTools.User;
							impersonator = AppRequestState.Instance.ImpersonatorExists ? AppRequestState.Instance.ImpersonatorUser : null;
						}
						catch {}

						if( user != null )
							sw.WriteLine( "User: {0}{1}".FormatWith( user.Email, impersonator != null ? " (impersonated by {0})".FormatWith( impersonator.Email ) : "" ) );
					}
				}
				else {
					sw.WriteLine( "Program: " + ConfigurationStatics.AppName );
					sw.WriteLine( "Version: " + ConfigurationStatics.AppAssembly.GetName().Version );
					sw.WriteLine( "Machine: " + EwlStatics.GetLocalHostName() );
				}

				EwlStatics.CallEveryMethod(
					delegate { EmailStatics.SendDeveloperNotificationEmail( getErrorEmailMessage( sw.ToString() ) ); },
					delegate { logError( sw.ToString() ); } );
			}
		}

		/// <summary>
		/// Reports an error to the developers. The report includes the specified message and additional information about the running program.
		/// </summary>
		public static void ReportError( string message ) {
			ReportError( message, null );
		}

		private static readonly object key = new object();

		private static void logError( string errorText ) {
			lock( key ) {
				using( var writer = new StreamWriter( File.Open( ConfigurationStatics.InstallationConfiguration.ErrorLogFilePath, FileMode.Append ) ) ) {
					writer.WriteLine( DateTime.Now + ":" );
					writer.WriteLine();
					writer.Write( errorText );
					writer.WriteLine();
					writer.WriteLine();
				}
			}
		}

		/// <summary>
		/// Reports a fault (a problem that could later cause errors) to the developers.
		/// </summary>
		public static void ReportFault( string message ) {
			EmailStatics.SendDeveloperNotificationEmail( getFaultEmailMessage( message ) );
		}

		/// <summary>
		/// Sends a notification to the developers. Do not use for anything that requires corrective action.
		/// </summary>
		public static void SendDeveloperNotification( string message ) {
			EmailStatics.SendDeveloperNotificationEmail( getNotificationEmailMessage( message ) );
		}

		/// <summary>
		/// Reports an administrator-correctable error to the administrators.
		/// </summary>
		public static void ReportAdministratorCorrectableError( Exception e ) {
			var emailMessage = getErrorEmailMessage( e.Message );
			emailMessage.ToAddresses.AddRange( EmailStatics.GetAdministratorEmailAddresses() );
			EmailStatics.SendEmailWithDefaultFromAddress( emailMessage );
		}

		/// <summary>
		/// Reports an administrator-correctable error to the administrators.
		/// </summary>
		public static void ReportAdministratorCorrectableError( string message ) {
			var emailMessage = getErrorEmailMessage( message );
			emailMessage.ToAddresses.AddRange( EmailStatics.GetAdministratorEmailAddresses() );
			EmailStatics.SendEmailWithDefaultFromAddress( emailMessage );
		}

		/// <summary>
		/// Reports an administrator-correctable fault (a problem that could later cause errors) to the administrators.
		/// </summary>
		public static void ReportAdministratorCorrectableFault( string message ) {
			var emailMessage = getFaultEmailMessage( message );
			emailMessage.ToAddresses.AddRange( EmailStatics.GetAdministratorEmailAddresses() );
			EmailStatics.SendEmailWithDefaultFromAddress( emailMessage );
		}

		/// <summary>
		/// Sends a notification to the administrators. Do not use for anything that requires corrective action.
		/// </summary>
		public static void SendAdministratorNotification( string message ) {
			var emailMessage = getNotificationEmailMessage( message );
			emailMessage.ToAddresses.AddRange( EmailStatics.GetAdministratorEmailAddresses() );
			EmailStatics.SendEmailWithDefaultFromAddress( emailMessage );
		}

		private static EmailMessage getErrorEmailMessage( string body ) {
			return new EmailMessage
				{
					Subject =
						"Error in {0}".FormatWith( ConfigurationStatics.InstallationConfiguration.SystemName ) +
						( ConfigurationStatics.IsClientSideProgram ? " on {0}".FormatWith( EwlStatics.GetLocalHostName() ) : "" ),
					BodyHtml = body.GetTextAsEncodedHtml()
				};
		}

		private static EmailMessage getFaultEmailMessage( string message ) {
			return new EmailMessage
				{
					Subject = "Fault in {0}".FormatWith( ConfigurationStatics.InstallationConfiguration.SystemName ),
					BodyHtml = message.GetTextAsEncodedHtml()
				};
		}

		private static EmailMessage getNotificationEmailMessage( string message ) {
			return new EmailMessage
				{
					Subject = "Notification from {0}".FormatWith( ConfigurationStatics.InstallationConfiguration.SystemName ),
					BodyHtml = message.GetTextAsEncodedHtml()
				};
		}

		/// <summary>
		/// Executes the specified method. Returns true if it is successful. If an exception occurs, this method returns false and details about the exception are emailed
		/// to the developers and logged. This should not be necessary in web applications, as they have their own error handling.
		/// Prior to calling this, you should be sure that system logic initialization has succeeded.  This almost always means that ExecuteAppWithStandardExceptionHandling
		/// has been called somewhere up the stack.
		/// This method has no side effects other than those of the given method and the email/logging that occurs in the event of an error.  This can be used
		/// repeatedly inside any application as an alternative to a try catch block.
		/// </summary>
		public static bool ExecuteBlockWithStandardExceptionHandling( Action method ) {
			try {
				method();
				return true;
			}
			catch( Exception e ) {
				ReportError( e );
				return false;
			}
		}

		/// <summary>
		/// Use this to email errors from web service methods and turn normal exceptions into FaultExceptions.
		/// </summary>
		public static void ExecuteWebServiceWithStandardExceptionHandling( Action method ) {
			// NOTE: Do we need to check whether the system logic was initialized properly or will EwfApp_BeginRequest take care of it?
			try {
				method();
			}
			catch( Exception e ) {
				throw createWebServiceException( e );
			}
		}

		/// <summary>
		/// Use this to email errors from web service methods and turn normal exceptions into FaultExceptions.
		/// </summary>
		public static T ExecuteWebServiceWithStandardExceptionHandling<T>( Func<T> method ) {
			// NOTE: Do we need to check whether the system logic was initialized properly or will EwfApp_BeginRequest take care of it?
			try {
				return method();
			}
			catch( Exception e ) {
				throw createWebServiceException( e );
			}
		}

		private static Exception createWebServiceException( Exception e ) {
			if( !( e is Wcf.AccessDeniedException ) )
				ReportError( e );
			return new FaultException( e.ToString() );
		}
	}
}