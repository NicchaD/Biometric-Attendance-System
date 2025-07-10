//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	DataArchiveBOClass.cs
// Author Name		:	Shashi Shekhar Singh
// Employee ID		:	51426
// Email				:	shashi.singh@3i-infotech.com
// Contact No		:	
// Creation Time		:	Aug 17th 2009
// Program Specification Name	:	YMCA PS Data Archive.Doc
// Unit Test Plan Name			:	
// Description					:	Business class for DataArchive form
//*******************************************************************************
// Change history
//*******************************************************************************
// Shashi Shekhar		2010.01.26	Added RetrieveData(ArrayList alUniqueId) to update the bitIsArchived field  in table "AtsPerss"
// Shashi Shekhar		2010.04.08	Added one parameter for error message in "RetrieveData" function
// Shashi Shekhar       2010-04-12  Ref:mail-Issues identified with 7.4.2 code release - Internally identified #1 Remove ref output variable,which was used in RetrieveData method
// Manthan Rajguru      2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System.Data;
using System.Collections ;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DataArchiveBOClass.
	/// </summary>
	public class DataArchiveBOClass
	{
		public DataArchiveBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// Method to call the mathod of the data access class to fetch the list of persons
		/// </summary>
		/// <param name="parameterSSNo">SSNo.</param>
		/// <param name="parameterFundNo">Fund No.</param>
		/// <param name="parameterLastName">Last Name</param>
		/// <param name="parameterFirstName">First Name</param>
		/// <param name="parameterCity">City</param>
		/// <param name="parameterState">State</param>
		/// <returns>Dataset</returns>
		public static DataSet LookUpPersons(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterCity, string parameterState)
		{
			try
			{
				return(DataArchiveDAClass.LookUpPerson(parameterSSNo, parameterFundNo,parameterLastName,parameterFirstName,parameterCity,parameterState));
	     	}
			catch
			{
				throw;
			}
		
		}
	
		///Shashi Shekhar:2010-01-26
		/// <summary>
		/// Update bitIsArchived field  in table "AtsPerss"
		/// </summary>
		/// <param name="alUniqueId">Unique identifier</param>
		
		public static string RetrieveData(ArrayList alUniqueId)
		{
			try
			{
				return DataArchiveDAClass.RetrieveData (alUniqueId);
			}
			catch
			{
				throw;
			}
			
		}

 




	}

}
