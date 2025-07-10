//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	VoidDisbursementVRManagerBOClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	7/26/2005 11:35:00 AM
// Description					:	Business Class for void Disbursement VR Manager
//Priya J			30/12/2009			:BT-1078
//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for VoidDisbursementVRManager.
	/// </summary>
	public sealed class VoidDisbursementVRManagerBOClass
	{
		private VoidDisbursementVRManagerBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static VoidDisbursementVRManagerBOClass GetInstance()
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

			internal static readonly VoidDisbursementVRManagerBOClass instance = new VoidDisbursementVRManagerBOClass();
		}

		/// <summary>
		/// Method to call the method of the VoidDisbursementVRManagerDAClass and is used to get the list of participants 
		/// based on the search criteria entered by the user.
		/// </summary>
		/// <param name="parameterFundIdNo"></param>
		/// <param name="parameterFName"></param>
		/// <param name="parameterLName"></param>
		/// <param name="parameterSsn"></param>
		/// <param name="parameterCheckNo"></param>
		/// <param name="parameterAnnuityOnly"></param>
		/// <returns></returns>
		public static DataSet LookUpDisbursements(string parameterFundIdNo, string parameterFName, string parameterLName, string parameterSsn, string parameterCheckNo, int parameterAnnuityOnly)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.LookUpDisbursements(parameterFundIdNo,parameterFName,parameterLName,parameterSsn,parameterCheckNo,parameterAnnuityOnly));
			}
			catch
			{
				throw;
			}

		}
		/// <summary>
		/// Method to get the details of the selected participant which calls the method of the DA Class
		/// </summary>
		/// <param name="parameterPersonId"></param>
		/// <param name="parameterAnnuityOnly"></param>
		/// <returns></returns>

		public static DataSet GetDisbursementsByPersId(string parameterPersonId,int parameterAnnuityOnly)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetDisbursementsByPersId( parameterPersonId, parameterAnnuityOnly));
			}
			catch
			{
				throw;
			}

		}
/// <summary>
/// Method to get the disbursement types based on the activity type
/// </summary>
/// <param name="parameterActivityType"></param>
/// <returns></returns>
		public static DataSet GetDisbursementStatusTypes(string parameterActivityType)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetDisbursementStatusTypes( parameterActivityType));
			}
			catch
			{
				throw;
			}

		}
		public static int IsCashOut(string parameterDisbId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.IsCashOut(parameterDisbId));
			}
			catch
			{
				throw;
			}

		}
		public static DataTable GetDeductions()
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetDeductions().Tables[0]);
			}
			catch 
			{
				throw ;
			}
		}
/// <summary>
/// Method to get the Withholding Info based on selected Disbursement Id
/// </summary>
/// <param name="parameterDisbId"></param>
/// <returns></returns>
		public static DataSet GetWithholdingInfo(string parameterDisbId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetWithholdingInfo( parameterDisbId));
			}
			catch
			{
				throw;
			}

		}
/// <summary>
/// Method to get the Related Disbursemnts based on selected Disbursement Id
/// </summary>
/// <param name="parameterDisbId"></param>
/// <returns></returns>

		public static DataSet GetRelatedDisbursement(string parameterDisbId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetRelatedDisbursement(parameterDisbId));
			}
			catch
			{
				throw;
			}

		}
/// <summary>
/// 
/// </summary>
/// <param name="parameterDisbId"></param>
/// <param name="parameterActionType"></param>
/// <param name="parameterNotes"></param>
/// <returns></returns>
		public static string  ChangeDisbursementStatus(string parameterDisbId,string parameterActionType,string parameterNotes)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.ChangeDisbursementStatus(parameterDisbId, parameterActionType, parameterNotes));
			}
			catch
			{
				throw;
			}
		}

/// <summary>
/// To do reversal in case breakdown is not there
/// </summary>
/// <param name="parameterDisbIds"></param>
				public static string DoReversal(string parameterDisbIds,string paramNewDisbursementID)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.DoReversal(parameterDisbIds,paramNewDisbursementID));
			}
			catch
			{
				throw;
			}
		}
		public static string DoLoanReversal(string parameterDisbId)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.DoLoanReversal(parameterDisbId));
			}
			catch
			{
				throw;
			}
		}
/// <summary>
/// For reissuing if breakdown is not there
/// </summary>
/// <param name="parameterDisbIds"></param>
		public static string  DoReissue(string parameterDisbIds,string paramNewDisbursementID)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.DoReissue(parameterDisbIds,paramNewDisbursementID));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Added by imran 0n 20/08/2009
		/// Get Withholdings List By DisbursementId
		/// </summary>
		/// <param name="disbursementId"></param>
		public static DataTable WithholdingsByDisbursement(string parameter_string_disbursementId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.WithholdingsByDisbursement(parameter_string_disbursementId));
			}
			catch 
			{
				throw ;
			}
		}

		/// <summary>
		/// Added on 25/08/2009 by imran
		/// To Void Withdrawal Reissue 
		/// </summary>
		/// <param name="parameterDisbIds"></param>
		public static string  DoVoidWithdrawalReissue(string parameterDisbIds)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.DoVoidWithdrawalReissue(parameterDisbIds));
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Added on 31/08/2009 by imran
		/// To Void Withdrawal Replace
		/// </summary>
		/// <param name="parameterDisbIds"></param>
		/// <param name="Dataset WithHolding"></param>
		public static string  DoVoidWithdrawalReplace(string[] parameterListDisbId,DataSet parameterdedDataSet,DataSet parameterdisbusementdtlDataset,string parameterType)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.DoVoidWithdrawalReplace(parameterListDisbId,parameterdedDataSet,parameterdisbusementdtlDataset,parameterType));
			}
			catch
			{
				throw;
			}
		}
		
		/// <summary>
		/// Added on 6/10/2009 by imran
		/// To Void Reverse
		/// 
		/// </summary>
		/// <param name="parameterListDisbId">Array of DisbId</param>
		/// <param name="parameterRefunddtlDataset">Dataset For Refund Detail</param>
		/// <param name="Type">Type</param>
		/// <param name="Status">Status</param>
		public static  string  DoVoidReverse(string[] parameterListDisbId,DataSet parameterRefunddtlDataset,string parameterType,string parastatus,int Intereststatus,string parameterFundstatus )
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.DoVoidReverse(parameterListDisbId,parameterRefunddtlDataset,parameterType,parastatus,Intereststatus,parameterFundstatus));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Added on 1/10/2009 by imran
		/// To Void Withdrawal Rissue
		/// 
		/// </summary>
		/// <param name="Array parameterDisbId"></param>
		/// <param name="Dataset Disbursement details"></param>
		public static  string  DoVoidWithdrawalRissue(string[] parameterListDisbId,DataSet parameterDataSet)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.DoVoidWithdrawalRissue(parameterListDisbId,parameterDataSet));
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
/// Method to get the Withholding Info based on selected Disbursement Id
/// </summary>
/// <param name="parameterDisbId"></param>
/// <returns></returns>
		public static DataSet GetWithholdingReverseInfo(string parameterRefRequestId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetWithholdingReverseInfo( parameterRefRequestId));
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// Method to get the GetDisbursementInfo Info based on selected Disbursement Id
		/// </summary>
		/// <param name="parameterDisbursementId"></param>
		/// <returns></returns>
		public static DataSet GetDisbursementInfo(string parameterDisbursementId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetDisbursementInfo( parameterDisbursementId));
			}
			catch
			{
				throw;
			}

		}
	

		/// <summary>
		/// Method to get the Get Disbursement WithHoldingInfo info based on the selected Disbursement Ids
		/// </summary>
		/// <param name="parameterDisbId"></param>
		/// <returns></returns>

		public static DataSet GetDisbursementWithHoldingInfo(string parameterDisbursementId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.GetDisbursementWithHoldingInfo( parameterDisbursementId));
			}
			catch
			{
				throw;
			}

		}
	
		public static DateTime getAccountingDate()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.getAccountingDate();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet GetFundStatusByPersId(string parameterPersId,string parameterRequestId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.GetFundStatusByPersId(parameterPersId,parameterRequestId);
			}
			catch
			{
				throw;
			}
		}
		public static DataTable GetDeductionsList()
		{
			DataSet l_DataSet;
			try
			{
				l_DataSet =YMCARET.YmcaDataAccessObject.VoidDisbursementVRManagerDAClass.GetDeductionsList();

				if (l_DataSet != null)
				{
					return l_DataSet.Tables [0];
				}
				else 
					return null;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Added by Priya 0n 30/12/2009 :BT-1078

		/// Get Existing Withholdings List By DisbursementId
		/// </summary>
		/// <param name="disbursementId"></param>
		public static DataTable ExistingWithholdingsByDisbursement(string strRefRequestId)
		{
			try
			{
				return (VoidDisbursementVRManagerDAClass.ExistingWithholdingsByDisbursement(strRefRequestId));
			}
			catch 
			{
				throw ;
			}
		}



	}
}
