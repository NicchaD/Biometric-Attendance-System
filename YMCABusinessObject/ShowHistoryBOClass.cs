//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	ShowHistoryBOClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	8/4/2005 9:57:10 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	BO Class for show history popup of the Void Disbursement VR Manager
//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System.Data;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for ShowHistoryBOClass.
	/// </summary>
	public class ShowHistoryBOClass
	{
		private ShowHistoryBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ShowHistoryBOClass GetInstance()
		{
			
			return Nested.instance;
		}
    
		class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly ShowHistoryBOClass instance = new ShowHistoryBOClass();
		}


		public static DataSet GetDisbursementHistory(string parameterDisbId)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.ShowHistoryDAClass.GetDisbursementHistory(parameterDisbId));
			}
			catch
			{
				throw;
			}
			
		}
	}
}
