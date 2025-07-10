//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	DisbursementBreakdownBOClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	8/12/2005 11:05:42 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DisbursementBreakdownBOClass.
	/// </summary>
	public sealed class DisbursementBreakdownBOClass
	{
		public DisbursementBreakdownBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DisbursementBreakdownBOClass GetInstance()
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

			internal static readonly DisbursementBreakdownBOClass instance = new DisbursementBreakdownBOClass();
		}

		public static DataSet LookUpTransacts(string parameterPersId, string parameterFundEventId)
		{
			try
			{
				return(DisbursementBreakdownDAClass.LookUpTransacts(parameterPersId, parameterFundEventId));
			}
			catch
			{
				throw;
			}

		}

		public static DataSet LookUpDisbursementDetails(string parameterPersId, string parameterDisbId,string parameterFundEventId,string parameterDate)
		{
			try
			{
				return(DisbursementBreakdownDAClass.LookUpDisbursementDetails(parameterPersId,parameterDisbId,parameterFundEventId,parameterDate));
			}
			catch
			{
				throw;
			}

		}

		public static string UpdateDisbursement(string parameterXml)
		{
			
			try
			{
				return (DisbursementBreakdownDAClass.UpdateDisbursement(parameterXml));
				
			}
			catch
			{
				throw;
			}


		}
		public static string AddTransactsForReissue(string parameterXml,string parameterDisbId,string parameterPersId)
		{
			try
			{
				return (DisbursementBreakdownDAClass.AddTransactsForReissue(parameterXml, parameterDisbId, parameterPersId));

			}
			catch
			{
				throw;
			}
		}
		public static string  ChangeDisbursementStatus(string parameterDisbId,string parameterActionType,string parameterNotes)
		{
			try
			{
				return(DisbursementBreakdownDAClass.ChangeDisbursementStatus(parameterDisbId, parameterActionType, parameterNotes));
			}
			catch
			{
				throw;
			}
		}

		public static string AddTransactsForReversal(string parameterXml,string parameterDisbIds,string parameterPersId,string parameterStatus)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DisbursementBreakdownDAClass.AddTransactsForReversal(parameterXml, parameterDisbIds, parameterPersId,  parameterStatus);
			}
			catch
			{
				throw;
				
			}
		}

			public static string UpdateDisbursementsForReversal(string parameterDisbIds)
			{
				try
				{
					return YMCARET.YmcaDataAccessObject.DisbursementBreakdownDAClass.UpdateDisbursementsForReversal(parameterDisbIds);
				}
				catch
				{
					throw;
				}
			}


	
	}
}
