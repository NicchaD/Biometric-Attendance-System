//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA - YRS
// FileName			:	RefundRequestBOClass.cs
// Author Name		:	SrimuruganG
// Employee ID		:	32365
// Email			:	srimurugan.ag@icici-infotech.com
// Contact No		:	8744
// Creation Time	:	8/15/2005 3:27:04 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for ReissueRefund.
	/// </summary>
	public sealed class ReissueRefund
	{
		public ReissueRefund()
		{		
		}

		public static DataTable GetRefundReissuesDetails ()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.ReissueRefundDAClass.GetRefundReissuesDetails ());
			}
			catch 
			{
				throw;
			}
		}

		//public static DataSet GetMemberReissueDetails (string parameterPersonID, string parameterDisbursementID, string parameterFundID)
		public static DataSet GetMemberReissueDetails (string parameterPersonID, string parameterDisbursementID, string parameterRefRequestID)
		{
			DataTable	l_DataTable = null;
			DataSet		l_DataSet = null;

			try
			{

				l_DataSet  = YMCARET.YmcaDataAccessObject.ReissueRefundDAClass.GetMemberReissueDetails (parameterPersonID, parameterDisbursementID, parameterRefRequestID);

				if (l_DataSet == null) return null;

//				l_DataTable = l_DataSet.Tables ["Current"];
//
//				if (l_DataTable == null) return l_DataSet;
//
//				foreach (DataRow l_DataRow in l_DataTable.Rows)
//				{
//					l_DataRow ["PersonalTotal"] = Convert.ToDecimal (l_DataRow ["PersonalPreTax"]) + Convert.ToDecimal (l_DataRow ["PersonalPostTax"]) + Convert.ToDecimal (l_DataRow ["PersonalInterest"]);
//					l_DataRow ["YmcaTotal"] = Convert.ToDecimal (l_DataRow ["YmcaPreTax"]) + Convert.ToDecimal (l_DataRow ["YMCAInterest"]);
// 
//					l_DataRow ["TotalTotal"] = Convert.ToDecimal (l_DataRow ["PersonalTotal"]) + Convert.ToDecimal (l_DataRow ["YmcaTotal"]);
//
//				}
//			
				return l_DataSet;

			}
			catch 
			{
				throw;
			}
		}


//		public static DataTable GetReissueTransactions (string parameterFundEventID)
//		{
//			try
//			{
//				return (YMCARET.YmcaDataAccessObject.ReissueRefundDAClass.GetReissueTransactions (parameterFundEventID));
//			}
//			catch 
//			{
//				throw;
//			}
//		}

		public static bool SaveRefundReIssue (DataSet parameterDataSet)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.ReissueRefundDAClass.SaveRefundReIssue (parameterDataSet));
			}
			catch 
			{
				throw;
			}
		}
//Priya 07-Sep-09
		public static DataSet GetReissueDisbursementDetails(string RefRequestsID)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ReissueRefundDAClass.GetReissueDisbursementDetails(RefRequestsID);
			}
			catch
			{
				throw;
			}

		}
		public static DataSet GetRefundSchemas()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ReissueRefundDAClass.GetRefundSchemas();
			}
			catch
			{
				throw;
			}
		}
		//end 07-Sep-09
	}
}
