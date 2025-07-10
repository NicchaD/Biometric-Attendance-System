//******************************************************************************* 
// Changed by               Changed on      Change Description
//******************************************************************************* 
//Shashi Shekhar Singh      2010.03.03      Changes for  issue  YRS 5.0-942
//Manthan Rajguru           2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************/
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AddOfficerBOClass.
	/// </summary>
	public sealed class AddOfficerBOClass
	{
		private AddOfficerBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet SearchTitleShortDesc(string parameterSearchTitleCode)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddOfficerDAClass.SearchTitleShortDesc(parameterSearchTitleCode);
			}
			catch
			{
				throw;
			}

		}


	//----Start:Shashi Shekhar:2010-03-03: Added for  issue  YRS 5.0-942--------------------------
		/// <summary>
		/// Method to call the mathod of the data access class to fetch the list of persons
		/// </summary>
		/// <param name="parameterFundNo"></param>
		/// <param name="parameterLastName"></param>
		/// <param name="parameterFirstName"></param>
		/// <param name="parameterYMCANo"></param>
		/// <returns>Dataset</returns>
		public static DataSet LookUpOfficer(string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterYMCANo)
		{
			try
			{
				return(AddOfficerDAClass.LookUpOfficer(parameterFundNo,parameterLastName,parameterFirstName,parameterYMCANo));
			}
			catch
			{
				throw;
			}
		}

	//----End:Shashi Shekhar:2010-03-03: Added for  issue  YRS 5.0-942--------------------------


//		//----Start:Shashi Shekhar:2010-04-13: Added for  issue  Gemini-1051--------------------------
//
//		/// <summary>
//		///  To Unlink the officer record from participatns
//		/// </summary>
//		/// <param name="guiUniqueId">OfficerId</param>
//		public static void OfficerUnlinkParticipant(string guiUniqueId)
//		{
//			try
//			{
//				AddOfficerDAClass.OfficerUnlinkParticipant(guiUniqueId);
//			}
//			catch
//			{
//				throw;
//			}
//		}
//
//
//		//----End:Shashi Shekhar:2010-04-13: Added for  issue  Gemini-1051--------------------------



	}
}
