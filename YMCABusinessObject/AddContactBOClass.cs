//******************************************************************************* 
// Changed by                   Changed on      Change Description
//******************************************************************************* 
//Shashi Shekhar Singh          2010.03.04      Changes for  issue  YRS 5.0-942
//Manthan Rajguru               2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************/
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AddContactBOClass.
	/// </summary>
	public sealed class AddContactBOClass
	{
		private AddContactBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpContactType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddContactDAClass.LookUpContactType();
			}
			catch
			{
				throw;
			}
		}

		//----Start:Shashi Shekhar:2010-03-04: Added for  issue  YRS 5.0-942--------------------------
		/// <summary>
		/// Method to call the mathod of the data access class to fetch the list of persons
		/// </summary>
		/// <param name="parameterFundNo"></param>
		/// <param name="parameterLastName"></param>
		/// <param name="parameterFirstName"></param>
		/// <param name="parameterYMCANo"></param>
		/// <returns>Dataset</returns>
		public static DataSet LookUpContact(string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterYMCANo)
		{
			try
			{
				return(AddContactDAClass.LookUpContact(parameterFundNo,parameterLastName,parameterFirstName,parameterYMCANo));
			}
			catch
			{
				throw;
			}
		
		}
		//----End:Shashi Shekhar:2010-03-04: Added for  issue  YRS 5.0-942--------------------------

		public static int CheckContact( string parameterFirstName,string parameterLastName,string parameterPhone,string parameterYMCANo)
		{
			try
			{
				return(AddContactDAClass.CheckContact(parameterFirstName,parameterLastName,parameterPhone,parameterYMCANo));
			}
			catch
			{
				throw;
			}
		
		}

//		//----Start:Shashi Shekhar:2010-04-13: Added for  issue  Gemini-1051--------------------------
//
//		/// <summary>
//		///  To Unlink the Contact record from participatns
//		/// </summary>
//		/// <param name="guiUniqueId">ContactId</param>
//		public static void ContactUnlinkParticipant(string guiUniqueId)
//		{
//			try
//			{
//				AddContactDAClass.ContactUnlinkParticipant (guiUniqueId);
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
