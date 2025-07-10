//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.DirectoryServices;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class ADHelper
	{
		private string sDirectoryPath = "";
		private string sDomainUser = "";
		private string sDomainPassword = "";

		private DirectoryEntry oDirectoryEntry;

		public ADHelper()
		{
		}

		public ADHelper (string sPath, string sUser, string sPassword)
		{
			sDirectoryPath = sPath;
			sDomainUser = sUser;
			sDomainPassword = sPassword;
		}

		public string DirectoryPath
		{
			set
			{
				sDirectoryPath = value;
			}
		}

		public string DomainUser
		{
			set
			{
				sDomainUser = value;
			}
		}

		public string DomainPassword
		{
			set
			{
				sDomainPassword = value;
			}
		}

		public int Bind()
		{
			try
			{
				oDirectoryEntry = new DirectoryEntry(sDirectoryPath, sDomainUser, sDomainPassword);
			}
			catch (Exception ex)
			{
				throw new Exception("Error binding to the Active Directory. " + ex.Message);
			}
			return 1;
		}

		public int AddUser(string sOrganizationUnit, string sUsername, string sPassword)
		{
			try
			{
				DirectoryEntry oOrganizationUnit = null;
				DirectoryEntry oUser = null;
				oOrganizationUnit = oDirectoryEntry.Children.Find("OU=" + sOrganizationUnit);

				/*if (oOrganizationUnit != null)
				{
				}
				else
				{
					oOrganizationUnit = oDirectoryEntry.Children.Add("OU=" + sOrganizationUnit, "Organization Unit");
				}*/

				oUser = oOrganizationUnit.Children.Add("CN=" + sUsername, "User");
				oUser.Properties["SAMAccountName"].Value = sUsername;
				oUser.Properties["userPrincipalName"].Add(sUsername + "@" + sDirectoryPath);
				oUser.Properties["GivenName"].Add(sUsername);
				oUser.CommitChanges();
				oUser.Properties["userAccountControl"].Value = 66048;
				oUser.Invoke("setPassword",new object[] {sPassword});
				oUser.Properties["PwdLastSet"].Value = 0;
				oUser.CommitChanges();
			}
			catch (Exception ex)
			{
				throw new Exception("Error creating user in Active Directory. " + ex.Message);
			}
			return 1;
			
		}

		public int ChangePassword (string sOrganizationUnit, string sUsername, string sOldPassword,string sNewPassword)
		{
			try
			{
				DirectoryEntry oOrganizationUnit = null;
				DirectoryEntry oUser = null;
				oOrganizationUnit = oDirectoryEntry.Children.Find("OU=" + sOrganizationUnit);
				oUser = oOrganizationUnit.Children.Find("CN=" + sUsername);
				oUser.Invoke("ChangePassword",new object[] {sOldPassword , sNewPassword});
				oUser.CommitChanges();
			}
			catch (Exception ex)
			{
				throw new Exception("Error changing user password in Active Directory. " + ex.Message);
			}
			return 1;
		}


		public bool IsAuthenticated(string sPath , string domain, string username, string pwd)
		{
			string domainAndUsername = domain + @"\" + username;
			DirectoryEntry entry = new DirectoryEntry(sPath,domainAndUsername, pwd);

			try
			{ 
				// Bind to the native AdsObject to force authentication. 
				Object obj = entry.NativeObject;
				DirectorySearcher search = new DirectorySearcher(entry);
				search.Filter = "(SAMAccountName=" + username + ")";
				search.PropertiesToLoad.Add("cn");
				SearchResult result = search.FindOne();
				if(null == result)
				{
					return false;
				}
			}
			catch 
			{
				//throw new Exception("Error authenticating user. " + ex.Message);
				// Commented by 34231 Ragesh V.P. to by pass the error in the Server Validation. Dated 01/12/2006
				return false;
			}
			return true;
		}

		public int IsAuthenticatedADUser (string sPath , string domain, string username, string pwd)
		{
			try
			{
				bool IsAuthentic = IsAuthenticated(sPath , domain, username, pwd);
				if (IsAuthentic)
					return 1;
				else
					return 0;

			}
			catch (Exception ex)
			{
				throw new Exception("Error authenticating user. " + ex.Message);
			}
		}

		public void Close()
		{
			try
			{
				oDirectoryEntry.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error creating user in Active Directory. " + ex.Message);
			}
		}

	}
}
