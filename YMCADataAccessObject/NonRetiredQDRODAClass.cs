//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA Yrs	
// FileName			:	NonRetiredQDRODAClass.cs
// Author Name		:	Amit Nigam
// Employee ID		:	36413
// Email			:	amit.nigam@3i-infotech.com
// Contact No		:	080-39876761
// Creation Time	:	13/6/2008 
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>

//
// Changed by			:	
// Changed on			:	
// Change Description	:	
//*****************************************************************************************************************************
//Chnaged by			Date			  Description
//*****************************************************************************************************************************
//Priya				    11.06.08		  Added if else statement to avoid null values
//Dilip Patada			jan 28th 2009     BT-676 - QDRO validation procedure for withdawals (refund, hardship, loan, retirement) 
//                                        of funds within the plan to be split and the withdrawal transaction date is dated 
//                                        after the QDRO
//Neeraj Singh			06/jun/2010       Enhancement for .net 4.0
//Neeraj Singh          07/jun/2010       review changes done
//Harshala Trimukhe	    26/04/2012	      YRS 5.0-1346:Cash out plan balance <= $5,000
//Priya				    22-June-2012	  BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
//Priya				    25-June-2012	  BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
//Sanjay R.             10-Oct-2012       BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
//Anudeep               18-jul-2013       BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
//Shashank Patel		03-Oct-2013		  BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee
//Manthan Rajguru       2015.09.16        YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru       2016.08.16        YRS-AT-2482: Add gender and marital status fields to QDRO beneficiary add screen (TrackIT 22117)
//Manthan Rajguru       2016.08.26        YRS-AT-2488 -  YRS enh: PART 1 of 4:RMD's for alternate payees (QDRO recipients) (TrackIT 22284)   
//Pramod P. Pokale      2016.08.24        YRS-AT-2529 - YRS Enh: request to QDRO function ( Non-Retired) to allow different amt or percent (TrackIT 23098) 
//Sanjay GS Rawat       2016.11.15        YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215
//Pramod P. Pokale      2016.12.09        YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215
//Pramod P. Pokale      2016.11.29        YRS-AT-3145 -  YRS enh: Fees - QDRO fee processing 
//*********************************************************************************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for QDROMemberActiveDACLass.
	/// </summary>
	public class NonRetiredQDRODAClass
	{
		public NonRetiredQDRODAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region LookUpActiveList		
		//		'*******************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                    //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08								//
		//		'Modified By               :																			//
		//		'Modify Reason             :																			//
		//		'Constructor Description   :																			//
		//		'Function Description      :This function is called when the user will click  on the OK button in  the	//
		//		'                          :List Tab																	//
		//		'*******************************************************************************************************//
		public static DataSet LookUpActiveList(string parameterSSNo,string parameterFundNo,string parameterLastName,string parameterFirstName,string parameterCityName,string parameterStateName)
		{
			DataSet l_dataset_dsLookUpActiveList = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_GetActiveMemberList");
				
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_SSNo",DbType.String,parameterSSNo);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundIdNo",DbType.String,parameterFundNo);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FirstName",DbType.String,parameterFirstName);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_LastName",DbType.String,parameterLastName);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_City",DbType.String,parameterCityName);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_State",DbType.String,parameterStateName);
				l_dataset_dsLookUpActiveList = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpActiveList,"ActiveList");
				return l_dataset_dsLookUpActiveList;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getTransactionsQDRO
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to Perform Split Operations.										//
		//		'***************************************************************************************************//
		public static DataSet getTransactionsQDRO(string FundEventID)//,string EndDate
		{
			DataSet l_dataset_dsLookUpTransQDRO = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getTransactions");
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,FundEventID);
				l_dataset_dsLookUpTransQDRO = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpTransQDRO,"Transactions");
				return l_dataset_dsLookUpTransQDRO;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getTransactionsQDRO
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to Perform Split Operations.										//
		//		'***************************************************************************************************//
		public static DataSet getGroupBTransactionsQDRO(string FundEventID,string EndDate)
		{
			DataSet l_dataset_dsLookUpTransQDRO = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getGroupBTransactions");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,FundEventID);
				db.AddInParameter(LookUpCommandWrapper,"@datetime_EndDate",DbType.DateTime,EndDate);
			
				l_dataset_dsLookUpTransQDRO = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpTransQDRO,"Transactions");
				return l_dataset_dsLookUpTransQDRO;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getAccountingDate
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will excecute when the user changes the end date             //
		//		'***************************************************************************************************//
		public static DateTime getAccountingDate()
		{
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			DateTime AccountingDate;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
			
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getAcctDate");
				
					
				AccountingDate=Convert.ToDateTime(db.ExecuteScalar(LookUpCommandWrapper));
				return AccountingDate;
			}
			catch(Exception ex) 
			{
				throw ex;
			}
		}
		#endregion

        #region UpdateQDRORequests
        //		'***************************************************************************************************//
        //		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
        //		'Created By                :Amit Nigam            Modified On :                                     //
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Class Description         :Used to save the qdro details in qdrorequest table.                     //
        //		'***************************************************************************************************//
        private static void UpdateQDRORequests(string originalFundEventId, decimal? feeOnRetirementPlan, decimal? feeOnSavingsPlan, DbTransaction pDBTransaction, Database db)
        {
            DbCommand updateCommandWrapper = null;
            try
            {
                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_UpdateQDRORequests");
	            //START: PPP | 11/29/2016 | YRS-AT-3145 | Passing fees applied to participant to record in atsQdroRequests
                //db.AddInParameter(updateCommandWrapper, "@varchar_FundEventID", DbType.String, OriginalFundEventId);
                db.AddInParameter(updateCommandWrapper, "@varchar_FundEventID", DbType.String, originalFundEventId);
                db.AddInParameter(updateCommandWrapper, "@NUMERIC_FeeOnRetirementPlan", DbType.Decimal, feeOnRetirementPlan);
                db.AddInParameter(updateCommandWrapper, "@NUMERIC_FeeOnSavingsPlan", DbType.Decimal, feeOnSavingsPlan);
	            //START: PPP | 11/29/2016 | YRS-AT-3145 | Passing fees applied to participant to record in atsQdroRequests
                db.ExecuteNonQuery(updateCommandWrapper, pDBTransaction);

            }
            catch
            {
                throw;
            }
        }
        #endregion
		#region UpdateQDRORequests
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to save the qdro details in qdrorequest table.                     //
		//		'***************************************************************************************************//
		private static void UpdateGroupBDetails(DataSet dsALLGroupBRecipantDetails)
		{


			Database db = null;
			DbCommand DeleteCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
						
				foreach (DataRow GroupBRecipant in dsALLGroupBRecipantDetails.Tables[0].Rows)
				{
					DeleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_QDRO_InsertGroupBParticipant");
                    db.AddInParameter(DeleteCommandWrapper, "@varchar_FundEventID", DbType.String, GroupBRecipant["FundEventID"]);
					db.ExecuteNonQuery(DeleteCommandWrapper);
				}


			}
			catch 
			{
				throw;
			}

		}	
		#endregion

        //START: PPP | 08/25/2016 | YRS-AT-2529 | SaveQDROData is not in use
        //#region SaveQDROData
        ////		'***************************************************************************************************//
        ////		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
        ////		'Created By                :Amit Nigam            Modified On :                                     //
        ////		'Modified By               :                                                                        //
        ////		'Modify Reason             :                                                                        //
        ////		'Constructor Description   :                                                                        //
        ////		'Class Description         :Used to save the qdro details.                                          //
        ////		'***************************************************************************************************//
        //private static void SaveQDROData(DataTable dtRecptAccount, string OriginalFundEventId, string participantid, string QDRORequestID, string AnnuityBasisType, DataTable dtParticipantDetails, string startdate, string enddate, string Plantype, DataSet dsAllPartAccountsDetail, DataSet l_dataset_AnnuityBasisDetail, DataTable dtBenifAccountTempTable, DataSet dsAllRecipantAccountsDetail, double YMCAInterestBalance, double dblPersonalInterestBalance, DataTable dtGroupBRecipant, DataTable dtGroupBParticipant, DataTable dtGroupARecipant, DataTable dtGroupAParticipant, DataSet dsALLGroupBRecipantDetails, DataSet dsALLGroupARecipantDetails, DataSet dsALLGroupBParticipantDetails, DataSet dsALLGroupAParticipantDetails, DataTable dtALlRecords, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
        //{
        //    try
        //    {
        //        InsertQDRORecipientDetails(enddate, dsALLGroupBRecipantDetails, dsALLGroupARecipantDetails, OriginalFundEventId, dtALlRecords, dtRecptAccount, QDRORequestID, pDBTransaction, db, AdjustInterest);
        //        InsertQDRORecipientDatatoAtsQdroDetails(dtRecptAccount, OriginalFundEventId, participantid, QDRORequestID, AnnuityBasisType, Plantype, dtParticipantDetails, dsAllRecipantAccountsDetail, pDBTransaction, db);
        //        InsertQDROPArtAccountDetails(enddate, dsALLGroupBParticipantDetails, dsALLGroupAParticipantDetails, OriginalFundEventId, QDRORequestID, pDBTransaction, db, AdjustInterest);
        //        UpdateQDRORequests(OriginalFundEventId, pDBTransaction, db);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //#endregion
        //END: PPP | 08/25/2016 | YRS-AT-2529 | SaveQDROData is not in use

		#region InsertQDRORecipientDatatoAtsQdroDetails
        //START: PPP | 08/25/2016 | YRS-AT-2529 | Function signature as well as data storage part changed
        ////		'***************************************************************************************************//
        ////		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
        ////		'Created By                :Amit Nigam            Modified On :                                     //
        ////		'Modified By               :                                                                        //
        ////		'Modify Reason             :                                                                        //
        ////		'Constructor Description   :                                                                        //
        ////		'Class Description         :Used to save the recipient data to  atsQdroNonRetDetails table.         //
        ////		'***************************************************************************************************//
        //private static void InsertQDRORecipientDatatoAtsQdroDetails(DataTable dtRecptAccount, string OriginalFundEventId, string participantid, string QDRORequestID, string AnnuityBasisType, string Plantype, DataTable dtParticipantDetails, DataSet dsAllRecipantAccountsDetail, DbTransaction pDBTransaction, Database db)
        //{
        //    DbCommand insertCommandWrapper = null;
        //    try
        //    {
        //        for (int datarow = 0; datarow < dsAllRecipantAccountsDetail.Tables[0].Rows.Count; datarow++)
        //        {
        //            insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertRecipientDatatoAtsQdroDetails");
        //            db.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.Guid, dsAllRecipantAccountsDetail.Tables[0].Rows[datarow]["PersId"]);
        //            db.AddInParameter(insertCommandWrapper, "@varchar_AcctType", DbType.String, dsAllRecipantAccountsDetail.Tables[0].Rows[datarow]["AcctType"]);
        //            db.AddInParameter(insertCommandWrapper, "@varchar_AnnuityBasisType", DbType.String, dsAllRecipantAccountsDetail.Tables[0].Rows[datarow]["AnnuityBasisType"]);
        //            db.AddInParameter(insertCommandWrapper, "@Numeric_PersonalPreTax", DbType.Double, dsAllRecipantAccountsDetail.Tables[0].Rows[datarow]["PersonalPreTax"]);
        //            db.AddInParameter(insertCommandWrapper, "@Numeric_PersonalPostTax", DbType.Double, dsAllRecipantAccountsDetail.Tables[0].Rows[datarow]["PersonalPostTax"]);
        //            db.AddInParameter(insertCommandWrapper, "@Numeric_YmcaPreTax", DbType.Double, dsAllRecipantAccountsDetail.Tables[0].Rows[datarow]["YMCAPreTax"]);
        //            db.ExecuteNonQuery(insertCommandWrapper, pDBTransaction);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        private static void InsertQDRORecipientDatatoAtsQdroDetails(DataTable qdroNonRetDetailsTable, DbTransaction dbTransaction, Database db)
        {
            DbCommand insertCommandWrapper;
            DataSet qdroNonRetSplit;
            try
            {
                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertRecipientDatatoAtsQdroDetails");
                db.AddInParameter(insertCommandWrapper, "@UNIQUEIDENTIFIER_NonRetSplitID", DbType.Guid, "NonRetSplitID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@VARCHAR_AcctType", DbType.String, "AccountType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@VARCHAR_AnnuityBasisType", DbType.String, "AnnuityBasisType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_PersonalPreTax", DbType.Decimal, "PersonalPreTax", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_PersonalPostTax", DbType.Decimal, "PersonalPostTax", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_YmcaPreTax", DbType.Decimal, "YmcaPreTax", DataRowVersion.Current);
                insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                qdroNonRetSplit = new DataSet();
                qdroNonRetSplit.Tables.Add(qdroNonRetDetailsTable);
                db.UpdateDataSet(qdroNonRetSplit, "RecipientDatatoAtsQdroDetails", insertCommandWrapper, null, null, dbTransaction);
            }
            catch
            {
                throw;
            }
            finally
            {
                qdroNonRetSplit = null;
                insertCommandWrapper = null;
            }
        }
        //END: PPP | 08/25/2016 | YRS-AT-2529 | Function signature as well as data storage part changed
        #endregion

        //#region InsertQDRORecipientDetails
        ////		'***************************************************************************************************//
        ////		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
        ////		'Created By                :Amit Nigam            Modified On :                                     //
        ////		'Modified By               :                                                                        //
        ////		'Modify Reason             :                                                                        //
        ////		'Constructor Description   :                                                                        //
        ////		'Class Description         :Used to save the recipient details into atstransacts.                   //
        ////		'***************************************************************************************************//
        //private static void InsertQDRORecipientDetails(string enddate, DataSet dsALLGroupBRecipantDetails, DataSet dsALLGroupARecipantDetails, string OriginalFundEventId, DataTable dtALlRecords, DataTable dtRecptAccount, string QDRORequestID, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
        //{
        //    try
        //    {
        //        InsertQDROGroupARecipant(enddate, dsALLGroupARecipantDetails, dtRecptAccount, dtALlRecords, QDRORequestID, pDBTransaction, db, AdjustInterest);
        //        InsertQDROGroupBRecipant(enddate, dsALLGroupBRecipantDetails, QDRORequestID, pDBTransaction, db, AdjustInterest);
        //    }

        //    catch
        //    {
        //        throw;
        //    }
        //}
        //#endregion

        private static void InsertQDROGroupBParticipants(string enddate, DataSet dsALLGroupBParticipantDetails, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
		{
			DbCommand insertCommandWrapper = null;
			try
			{
				foreach (DataRow GroupBRecipant in dsALLGroupBParticipantDetails.Tables[0].Rows)
				{
							
					insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertGroupBTransactionDetails");
                    db.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.String, GroupBRecipant["Persid"]);
                    db.AddInParameter(insertCommandWrapper, "@varchar_FundEventID", DbType.String, GroupBRecipant["FundEventID"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_AcctType",DbType.String,GroupBRecipant["AcctType"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_TransactType",DbType.String,GroupBRecipant["TransactType"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_AnnuityBasisType",DbType.String,GroupBRecipant["AnnuityBasisType"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_TransactionRefID",DbType.String,GroupBRecipant["TransactionRefID"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_TransactDate",DbType.String,enddate);
					db.AddInParameter(insertCommandWrapper,"@Numeric_MonthlyComp",DbType.String,GroupBRecipant["MonthlyComp"]);
					db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPreTax",DbType.String,GroupBRecipant["PersonalPreTax"]);
					db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPostTax",DbType.String,GroupBRecipant["PersonalPostTax"]);
					db.AddInParameter(insertCommandWrapper,"@Numeric_YmcaPreTax",DbType.String,GroupBRecipant["YmcaPreTax"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_FundedDate",DbType.String,GroupBRecipant["FundedDate"]);
                    db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.String, GroupBRecipant["UniqueId"]);
                    db.AddInParameter(insertCommandWrapper, "@varchar_RequestId", DbType.String, "UniqueId", DataRowVersion.Current);
                    //SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
                    db.AddInParameter(insertCommandWrapper, "@Bool_AdjustInterest", DbType.Boolean, AdjustInterest);
                    //Ends,SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
					db.ExecuteNonQuery(insertCommandWrapper,pDBTransaction);
				}
			}
			catch 
			{
				throw;
			}
		}

		#region InsertQDRORecipientDetails
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Ganeswar Sahoo                         Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to save the recipient details into atstransacts.                   //
		//		'***************************************************************************************************//
        //START: PPP | 08/26/2016 | YRS-AT-2529 | Removed dtRecptAccount and dtALlRecords parameters because they are not required
        //private static void InsertQDROGroupARecipant(string enddate, DataSet dsALLGroupARecipantDetails, DataTable dtRecptAccount, DataTable dtALlRecords, string QDRORequestID, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
        private static void InsertQDROGroupARecipant(string endDate, DataSet groupARecipantDetails, string qdroRequestID, DbTransaction dbTransaction, Database db, Boolean isAdjustInterest)
        //END: PPP | 08/26/2016 | YRS-AT-2529 | Removed dtRecptAccount and dtALlRecords parameters because they are not required
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
            try
            {
                insertCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_QDRO_InsertGroupATransactionDetails");
                db.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.Guid, "Persid", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_FundEventID", DbType.Guid, "FundEventID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_AcctType", DbType.String, "AcctType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_TransactType", DbType.String, "TransactType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_AnnuityBasisType", DbType.String, "AnnuityBasisType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_TransactionRefID", DbType.Guid, "TransactionRefID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_TransactDate", DbType.String, endDate); // PPP | 08/26/2016 | YRS-AT-2529 | enddate renamed to endDate
                db.AddInParameter(insertCommandWrapper, "@Numeric_MonthlyComp", DbType.String, "MonthlyComp", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Numeric_PersonalPreTax", DbType.String, "PersonalPreTax", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Numeric_PersonalPostTax", DbType.String, "PersonalPostTax", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Numeric_YmcaPreTax", DbType.String, "YmcaPreTax", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_FundedDate", DbType.String, "FundedDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "UniqueId", DataRowVersion.Current);
                //Added By Ganeswar for Funded date Update
                db.AddInParameter(insertCommandWrapper, "@varchar_RequestId", DbType.String, qdroRequestID); // PPP | 08/26/2016 | YRS-AT-2529 | QDRORequestID renamed to qdroRequestID
                //Added By Ganeswar for Funded date Update
                //SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
                db.AddInParameter(insertCommandWrapper, "@Bool_AdjustInterest", DbType.Boolean, isAdjustInterest); // PPP | 08/26/2016 | YRS-AT-2529 | AdjustInterest renamed to isAdjustInterest
                //Ends,SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 


                if (groupARecipantDetails != null)
                {
                    db.UpdateDataSet(groupARecipantDetails, "Transactions", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, dbTransaction); // PPP | 08/26/2016 | YRS-AT-2529 | pDBTransaction renamed to dbTransaction
                }
            }
            catch
            {
                throw;
            }
		}
		#endregion

		#region InsertQDRORecipientDetails
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to save the recipient details into atstransacts.                   //
		//		'***************************************************************************************************//
        private static void InsertQDROGroupBRecipant(string enddate, DataSet dsALLGroupBRecipantDetails, string QDRORequestID, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_QDRO_InsertGroupBTransactionDetails");
                db.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.Guid, "Persid", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_FundEventID", DbType.Guid, "FundEventID", DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_AcctType",DbType.String,"AcctType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactType",DbType.String,"TransactType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_AnnuityBasisType",DbType.String ,"AnnuityBasisType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactionRefID",DbType.Guid,"TransactionRefID",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactDate",DbType.String,enddate);
				db.AddInParameter(insertCommandWrapper,"@Numeric_MonthlyComp",DbType.String,"MonthlyComp",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPreTax",DbType.String,"PersonalPreTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPostTax",DbType.String,"PersonalPostTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_YmcaPreTax",DbType.String,"YmcaPreTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_FundedDate",DbType.String,"FundedDate",DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.String, "UniqueId", DataRowVersion.Current);
                //SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
                db.AddInParameter(insertCommandWrapper, "@Bool_AdjustInterest", DbType.Boolean, AdjustInterest);
                //Ends,SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date
				//Added By Ganeswar for Funded date Update
				db.AddInParameter(insertCommandWrapper,"@varchar_RequestId",DbType.String,QDRORequestID);	
				//Added By Ganeswar for Funded date Update
			if (dsALLGroupBRecipantDetails != null)
				{
					db.UpdateDataSet(dsALLGroupBRecipantDetails,"Transactions" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);
				}
				}
				catch 
				{
					throw;
				}
			
		}
		#endregion

        //START: PPP | 08/25/2016 | YRS-AT-2529 | Function not in use
        //#region InsertQDROPArtAccountDetails
        ////		'***************************************************************************************************//
        ////		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
        ////		'Created By                :Amit Nigam            Modified On :                                     //
        ////		'Modified By               :                                                                        //
        ////		'Modify Reason             :                                                                        //
        ////		'Constructor Description   :                                                                        //
        ////		'Class Description         :Used to save the Participant details into atstransacts.                 //
        ////		'***************************************************************************************************//

        //private static void InsertQDROPArtAccountDetails(string enddate, DataSet dsALLGroupBParticipantDetails, DataSet dsALLGroupAParticipantDetails, string OriginalFundEventId, string QDRORequestID, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
        //{
        //    try
        //    {
        //        InsertQDROGroupBParticipant(enddate,dsALLGroupBParticipantDetails,QDRORequestID,pDBTransaction,db, AdjustInterest);
        //        InsertQDROGroupAParticipant(enddate,dsALLGroupAParticipantDetails,QDRORequestID,pDBTransaction,db, AdjustInterest);
        //    }
        //catch 
        //    {
        //        throw;
        //    }
        //}
        //#endregion
        //END: PPP | 08/25/2016 | YRS-AT-2529 | Function not in use

		#region InsertQDROGroupB Participant
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Ganeswar Sahoo                      Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to save the recipient details into atstransacts.                   //
		//		'***************************************************************************************************//
        private static void InsertQDROGroupBParticipant(string enddate, DataSet dsALLGroupBParticipantDetails, string QDRORequestID, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_QDRO_InsertGroupBTransactionDetails");
                db.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.Guid, "Persid", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_FundEventID", DbType.Guid, "FundEventID", DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_AcctType",DbType.String,"AcctType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactType",DbType.String,"TransactType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_AnnuityBasisType",DbType.String ,"AnnuityBasisType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactionRefID",DbType.Guid,"TransactionRefID",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactDate",DbType.String,enddate);
				db.AddInParameter(insertCommandWrapper,"@Numeric_MonthlyComp",DbType.String,"MonthlyComp",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPreTax",DbType.String,"PersonalPreTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPostTax",DbType.String,"PersonalPostTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_YmcaPreTax",DbType.String,"YmcaPreTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_FundedDate",DbType.String,"FundedDate",DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "UniqueId", DataRowVersion.Current);
				//Added By Ganeswar for Funded date Update
				db.AddInParameter(insertCommandWrapper,"@varchar_RequestId",DbType.String,QDRORequestID);
				//Added By Ganeswar for Funded date Update
                //SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
                db.AddInParameter(insertCommandWrapper, "@Bool_AdjustInterest", DbType.Boolean, AdjustInterest);
                //Ends,SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 

				if (dsALLGroupBParticipantDetails != null)
				{
					db.UpdateDataSet(dsALLGroupBParticipantDetails,"Transactions" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);
				}
			}
			catch 
			{
				throw;
			}

		}
		#endregion

		#region InsertQDROGroupAParticipant
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Ganeswar Sahoo                      Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to save the Participant details into atstransacts.                   //
		//		'***************************************************************************************************//
        private static void InsertQDROGroupAParticipant(string enddate, DataSet dsALLGroupAParticipantDetails, string QDRORequestID, DbTransaction pDBTransaction, Database db, Boolean AdjustInterest)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_QDRO_InsertGroupATransactionDetails");
                db.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.Guid, "Persid", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_FundEventID", DbType.Guid, "FundEventID", DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_AcctType",DbType.String,"AcctType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactType",DbType.String,"TransactType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_AnnuityBasisType",DbType.String ,"AnnuityBasisType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactionRefID",DbType.Guid,"TransactionRefID",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_TransactDate",DbType.String,enddate);
				db.AddInParameter(insertCommandWrapper,"@Numeric_MonthlyComp",DbType.String,"MonthlyComp",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPreTax",DbType.String,"PersonalPreTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_PersonalPostTax",DbType.String,"PersonalPostTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Numeric_YmcaPreTax",DbType.String,"YmcaPreTax",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_FundedDate",DbType.String,"FundedDate",DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "UniqueId", DataRowVersion.Current);
				//Added By Ganeswar for Funded date Update
  				db.AddInParameter(insertCommandWrapper,"@varchar_RequestId",DbType.String,QDRORequestID);	
				//Added By Ganeswar for Funded date Update
                //SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
                db.AddInParameter(insertCommandWrapper, "@Bool_AdjustInterest", DbType.Boolean, AdjustInterest);
                //Ends,SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 

				if (dsALLGroupAParticipantDetails != null)
				{
					db.UpdateDataSet(dsALLGroupAParticipantDetails,"Transactions" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);
				}
			}
			catch 
			{
				throw;
			}

		}
		#endregion

        //START: PPP | 08/25/2016 | YRS-AT-2529 | New function SaveRequest introduced instead of this
        //#region SaveQDROFinal
        ////		'***************************************************************************************************//
        ////		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
        ////		'Created By                :Amit Nigam            Modified On :                                     //
        ////		'Modified By               :                                                                        //
        ////		'Modify Reason             :                                                                        //
        ////		'Constructor Description   :                                                                        //
        ////		'Class Description         :Used to save the qdro details.                                          //
        ////		'***************************************************************************************************//
        ////SR:2012.10.10-BT:1046/YRS 5.0-1603: New Parameter 'AdjustInterest' is added SaveQDROFinal,SaveQDROData Function
        ////Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
        //public static bool SaveQDROFinal(DataTable dtBenifAccountTempTable, DataTable dtRecptAccount, string OriginalFundEventId, string QDRORequestID, string Plantype, string startdate, string enddate, string participantid, string AnnuityBasisType, DataTable dtParticipantDetails, DataSet dsAllPartAccountsDetail, DataSet l_dataset_AnnuityBasisDetail, DataSet dsAllRecipantAccountsDetail, double YMCAInterestBalance, double dblPersonalInterestBalance, DataTable dtGroupBRecipant, DataTable dtGroupBParticipant, DataTable dtGroupARecipant, DataTable dtGroupAParticipant, DataSet dsALLGroupBRecipantDetails, DataSet dsALLGroupARecipantDetails, DataSet dsALLGroupBParticipantDetails, DataSet dsALLGroupAParticipantDetails, DataTable dtALlRecords, Boolean AdjustInterest, out DataTable QdroNonRetSplitID)
        ////Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END										
        //{
        //    bool bool_TransactionStarted = false;
        //    Database db = null;
        //    DbTransaction pDBTransaction = null;
        //    DbConnection DBconnectYRS = null;

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");
        //        DBconnectYRS = db.CreateConnection();
        //        DBconnectYRS.Open();
        //        pDBTransaction = DBconnectYRS.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        //        bool_TransactionStarted = true;
        //        foreach (DataRow beneficiarydatarow in dtBenifAccountTempTable.Rows)
        //        {
        //            if (Convert.ToBoolean((beneficiarydatarow["FlagNewBenf"].ToString())))
        //            {
        //                InsertPersDtls(beneficiarydatarow, pDBTransaction, db);
        //            }

        //            //03-Oct-2013: SP: BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee
        //            InsertEstateBeneficiary(beneficiarydatarow, pDBTransaction, db);
        //        }
        //        foreach (DataRow benificiaryFundEventDataRow in dtBenifAccountTempTable.Rows)
        //        {
        //            InsertQDRORefunds(benificiaryFundEventDataRow, OriginalFundEventId, pDBTransaction, db);
        //        }

        //        UpdateParticipantMaritalStatus(participantid, pDBTransaction, db);


        //        //Added By SG: 2012.11.29: BT-1436
        //        //InsertQdroNonRetSplit(dtRecptAccount, dtBenifAccountTempTable, dtParticipantDetails, startdate, enddate, Plantype, QDRORequestID, pDBTransaction, db);
        //        QdroNonRetSplitID = InsertQdroNonRetSplit(dtRecptAccount, dtBenifAccountTempTable, dtParticipantDetails, startdate, enddate, Plantype, QDRORequestID, pDBTransaction, db);

        //        SaveQDROData(dtRecptAccount, OriginalFundEventId, participantid, QDRORequestID, AnnuityBasisType, dtParticipantDetails, startdate, enddate, Plantype, dsAllPartAccountsDetail, l_dataset_AnnuityBasisDetail, dtBenifAccountTempTable, dsAllRecipantAccountsDetail, YMCAInterestBalance, dblPersonalInterestBalance, dtGroupBRecipant, dtGroupBParticipant, dtGroupARecipant, dtGroupAParticipant, dsALLGroupBRecipantDetails, dsALLGroupARecipantDetails, dsALLGroupBParticipantDetails, dsALLGroupAParticipantDetails, dtALlRecords, pDBTransaction, db, AdjustInterest);
        //        if (AdjustInterest == false)
        //        {
        //            getInterestAdjustments(QDRORequestID, pDBTransaction, db);
        //        }


        //        pDBTransaction.Commit();
        //        bool_TransactionStarted = false;
        //        DBconnectYRS.Close();
        //        //Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
        //        return bool_TransactionStarted;
        //        //Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END
        //    }
        //    catch
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        if (bool_TransactionStarted)
        //        {
        //            pDBTransaction.Rollback();
        //        }
        //        if (DBconnectYRS.State == ConnectionState.Open)
        //            DBconnectYRS.Close();
        //    }
        //}
		//#endregion
        // SR | 2016.11.15 | YRS-AT-2990 - Changed Method return type from boolean to object of ReturnObject class to return multiple values from called method.
        public static  YMCAObjects.ReturnObject<bool> SaveRequest(DataTable requestDetails, DataTable beneficiaryTable, DataTable qdroNonRetSplitTable, DataTable qdroNonRetDetailsTable, DataSet groupBTransactions, DataSet groupATransactions)
		{
			Database db = null;
            DbTransaction dbTransaction = null;
            DbConnection dbConnection = null;
            bool isTransactionStarted = false;
            string participantPersID, participantFundEventID, requestID;
            bool isAdjustInterest;
            string endDate;
            // START | SR | 2016.11.15 | YRS-AT-2990 | define return type
            YMCAObjects.ReturnObject<bool> requestResult;
            //START: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Negative balance function now returns bool
            //short negativeBalanceType = 0; 
            bool isNegativeBalanceExists;
            //END: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Negative balance function now returns bool
            // END | SR | 2016.11.15 | YRS-AT-2990 | define return type
            decimal? participantFeeOnRetirementPlan, participantFeeOnSavingsPlan; //PPP | 11/29/2016 | YRS-AT-3145 
            string feeChargingResult; //PPP | 01/12/2017 | YRS-AT-3145 | While charging fee, it validates balance also, this variable holds the error if any is there.
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                dbConnection = db.CreateConnection();
                dbConnection.Open();
                dbTransaction = dbConnection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                isTransactionStarted = true;
                // START | SR | 2016.11.15 | YRS-AT-2990 | Initialise return type
                requestResult = new YMCAObjects.ReturnObject<bool>();
                requestResult.Value = true;
                // END | SR | 2016.11.15 | YRS-AT-2990 | Initialise return type
                participantPersID = string.Empty;
                participantFundEventID = string.Empty;
                requestID = string.Empty;
                isAdjustInterest = false;
                endDate = string.Empty;
                //START: PPP | 11/29/2016 | YRS-AT-3145 | Fee charged to participant storing in variable
                participantFeeOnRetirementPlan = null;
                participantFeeOnSavingsPlan = null;
                //END: PPP | 11/29/2016 | YRS-AT-3145 | Fee charged to participant storing in variable
                if (requestDetails != null && requestDetails.Rows != null && requestDetails.Rows.Count > 0)
                {
                    participantPersID = Convert.ToString(requestDetails.Rows[0]["PersID"]);
                    participantFundEventID = Convert.ToString(requestDetails.Rows[0]["FundEventID"]);
                    requestID = Convert.ToString(requestDetails.Rows[0]["RequestID"]);
                    isAdjustInterest = Convert.ToBoolean(requestDetails.Rows[0]["IsAdjustInterest"]);
                    endDate = Convert.ToString(requestDetails.Rows[0]["EndDate"]);
                    //START: PPP | 11/29/2016 | YRS-AT-3145 | Fee charged to participant storing in variable
                    participantFeeOnRetirementPlan = Convert.IsDBNull(requestDetails.Rows[0]["FeeOnRetirementPlan"]) ? null : (decimal?)Convert.ToDecimal(requestDetails.Rows[0]["FeeOnRetirementPlan"]);
                    participantFeeOnSavingsPlan = Convert.IsDBNull(requestDetails.Rows[0]["FeeOnSavingsPlan"]) ? null : (decimal?)Convert.ToDecimal(requestDetails.Rows[0]["FeeOnSavingsPlan"]);
                    //END: PPP | 11/29/2016 | YRS-AT-3145 | Fee charged to participant storing in variable
                }

                // Create beneficiary AtsPerss record
                foreach (DataRow beneficiarydatarow in beneficiaryTable.Rows)
                {
                    if (Convert.ToBoolean((beneficiarydatarow["FlagNewBenf"].ToString())))
                    {
                        InsertPersDtls(beneficiarydatarow, dbTransaction, db);
                    }

					//03-Oct-2013: SP: BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee
                    InsertEstateBeneficiary(beneficiarydatarow, dbTransaction, db);
                }

                // Create beneficiary AtsFundEvents records
                foreach (DataRow benificiaryFundEventDataRow in beneficiaryTable.Rows)
                {
                    InsertQDRORefunds(benificiaryFundEventDataRow, participantFundEventID, dbTransaction, db);
                }
               
                UpdateParticipantMaritalStatus(participantPersID, dbTransaction, db);

                //AtsQdroNonRetSplit
                InsertQdroNonRetSplit(qdroNonRetSplitTable, dbTransaction, db);

                //AtsQdroNonRetDetails
                InsertQDRORecipientDatatoAtsQdroDetails(qdroNonRetDetailsTable, dbTransaction, db);

                // AtsTransacts 
                InsertQDROGroupARecipant(endDate, groupATransactions, requestID, dbTransaction, db, isAdjustInterest);
                InsertQDROGroupBRecipant(endDate, groupBTransactions, requestID, dbTransaction, db, isAdjustInterest);

                //START: PPP | 11/29/2016 | YRS-AT-3145 | Passing fee charged to participant to yrs_usp_QDRO_UpdateQDRORequests
                //UpdateQDRORequests(participantFundEventID, dbTransaction, db);
                UpdateQDRORequests(participantFundEventID, participantFeeOnRetirementPlan, participantFeeOnSavingsPlan, dbTransaction, db);
                //END: PPP | 11/29/2016 | YRS-AT-3145 | Passing fee charged to participant to yrs_usp_QDRO_UpdateQDRORequests

                if (!isAdjustInterest)
                {
                    getInterestAdjustments(requestID, dbTransaction, db);

                    //START: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Checking negative balance irrespective interest
                    //negativeBalanceType = LookUpForParticipantNegativeBalance(requestID, dbTransaction, db); // SR | 2016.11.16 | YRS-AT-2990 - verify negative balances after QDRO split (TrackIT 26215)
                    //END: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Checking negative balance irrespective interest
                }

                //START: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Negative balance function now returns bool
                isNegativeBalanceExists = LookUpForParticipantNegativeBalance(requestID, dbTransaction, db);
                //END: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Negative balance function now returns bool

                // START | SR | 2016.11.16 | YRS-AT-2990 - Validation to prevent negative balances after QDRO split (TrackIT 26215)
                if (isNegativeBalanceExists) //if (negativeBalanceType != 0) //PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Negative balance function now returns bool
                {
                    //START: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | No need to check plan, if negative balance then just return the message code
                    //if (negativeBalanceType == 1) // Retirement Plan balance is negative
                    //{
                    //    requestResult.MessageList.Add("RETIREMENT");
                    //}
                    //else if (negativeBalanceType == 2) // Savings Plan balance is negative // PPP | 12/09/2016 | YRS-AT-2990 | Converted "if" into "else if"
                    //{
                    //    requestResult.MessageList.Add("SAVINGS");
                    //}
                    //else if (negativeBalanceType == 3) // Both Plan balance is negative // PPP | 12/09/2016 | YRS-AT-2990 | Converted "if" into "else if"
                    //{
                    //    requestResult.MessageList.Add("BOTH");
                    //}    
                    requestResult.MessageList.Add("MESSAGE_QDRO_REQUEST_NEGATIVEBALANCE");
                    //END: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | No need to check plan, if negative balance then just return the message code
                }
                else
                {
                    feeChargingResult = ChargeFee(requestID, YMCAObjects.Module.QDRO, dbTransaction, db); //PPP | 01/12/2017 | YRS-AT-3145 | Charge fee to participant and it's recipient(s)
                    if (string.IsNullOrEmpty(feeChargingResult)) //PPP | 11/30/2016 | YRS-AT-3145 | If failed to charge fee then display error
                    {
                        //START: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | After applying fees check for negative balance
                        isNegativeBalanceExists = LookUpForParticipantNegativeBalance(requestID, dbTransaction, db);
                        if (isNegativeBalanceExists)
                        {
                            requestResult.MessageList.Add("MESSAGE_QDRO_AFTER_FEE_NEGATIVEBALANCE");
                        }
                        else
                        {
                            // If request is not affecting negative balance in account of participant and recipient then 
                            // delete recipients data saved in staging table
                            ClearRecipientStaging(requestID, dbTransaction, db);
                            //END: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | After applying fees check for negative balance

                            requestResult.Value = false;
                            dbTransaction.Commit();
                            isTransactionStarted = false;
                        }
                    }
                    else
                        requestResult.MessageList.Add(feeChargingResult);
                }
                // END | SR | 2016.11.16 | YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215)

                //dbConnection.Close();
				//Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
                //return isTransactionStarted;
				//Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END
                return requestResult;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (isTransactionStarted)
                {
                    dbTransaction.Rollback();
                }
                if (dbConnection.State == ConnectionState.Open)
                    dbConnection.Close();

                requestResult = null; // PPP | 12/09/2016 | YRS-AT-2990
                participantPersID = null;
                participantFundEventID = null;
                requestID = null;
                dbConnection = null;
                dbTransaction = null;
                db = null;
            }
		}
        //END: PPP | 08/25/2016 | YRS-AT-2529 | New function SaveRequest introduced instead of this
	
		#region getInterestAdjustments
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to get Interest Adjustments.										//
		//		'***************************************************************************************************//
		public static void getInterestAdjustments(string QdroRequestid,DbTransaction pDBTransaction,Database db)
		{
					
			DbCommand LookUpCommandWrapper = null;
		
			try
			{
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_NONRetiredQDRO_InterestAdjustments");
				db.AddInParameter(LookUpCommandWrapper,"@guiQdroRequestid",DbType.String,QdroRequestid);
				db.AddOutParameter(LookUpCommandWrapper, "@cOutput", DbType.String, 1000);
				db.ExecuteNonQuery(LookUpCommandWrapper,pDBTransaction);
				//Priya 22-June-2012 Bt-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
				string stroutput = string.Empty;
				stroutput = db.GetParameterValue(LookUpCommandWrapper, "@cOutput").ToString();
					
				if( stroutput != string.Empty)
				{
					throw new Exception(stroutput);
			}
				//ENd Priya 22-June-2012 Bt-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
			}
			catch 
			{
				throw;
			}
		}
		#endregion
	
		#region UpdateParticipantMaritalStatus
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to update the marital status of participant						//
		//		'***************************************************************************************************//
	
		private static void UpdateParticipantMaritalStatus(string participantid, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_UpdateParticipantMStatus");
				db.AddInParameter(insertCommandWrapper,"@varchar_ParticipantId",DbType.String,participantid);
				db.ExecuteNonQuery(insertCommandWrapper,pDBTransaction);
			}
			catch 
			{
				throw;
			}
		}
		#endregion

        #region InsertQdroNonRetSplit
        //START: PPP | 08/25/2016 | YRS-AT-2529 | Function signature as well as data storage part changed
        ////		'***************************************************************************************************//
        ////		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
        ////		'Created By                :Amit Nigam            Modified On :                                     //
        ////		'Modified By               :                                                                        //
        ////		'Modify Reason             :                                                                        //
        ////		'Constructor Description   :                                                                        //
        ////		'Class Description         :Used to Insert the Details in the atsQdroNonRetSplit table              //
        ////		'***************************************************************************************************//
        ////Added By SG: 212.11.29: BT-1436
        ////private static void InsertQdroNonRetSplit(DataTable dtRecptAccount, DataTable dtBenifAccountTempTable, DataTable dtParticipantDetails, string startdate, string enddate, string Plantype, string QDRORequestID, DbTransaction pDBTransaction, Database db)
        //private static DataTable InsertQdroNonRetSplit(DataTable dtRecptAccount, DataTable dtBenifAccountTempTable, DataTable dtParticipantDetails, string startdate, string enddate, string Plantype, string QDRORequestID, DbTransaction pDBTransaction, Database db)
        //{
        //    DbCommand insertCommandWrapper = null;
        //    string RecpPersonID = string.Empty;
        //    try
        //    {

        //        //Added By SG: 212.11.29: BT-1436
        //        DataTable dtSplitID = new DataTable();
        //        dtSplitID.Columns.Add("FundEventID", typeof(string));
        //        dtSplitID.Columns.Add("QDRONonRetSplitID", typeof(string));

        //        for (int datarow = 0; datarow < dtBenifAccountTempTable.Rows.Count; datarow++)
        //        {
        //            double SplitAmount = 0.0;
        //            double splitpercent = 0.0;
        //            double BenefitAmount = 0.0;
        //            double SelectedAmount = 0.0;
        //            double TotalBalance = 0.0;
        //            insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_Qdro_InsertQdroNonRetSplit");
        //            db.AddInParameter(insertCommandWrapper, "@varchar_RecpPersonID", DbType.String, dtBenifAccountTempTable.Rows[datarow]["id"]);
        //            db.AddInParameter(insertCommandWrapper, "@EndDate", DbType.DateTime, enddate);
        //            db.AddInParameter(insertCommandWrapper, "@StartDate", DbType.DateTime, startdate);
        //            db.AddInParameter(insertCommandWrapper, "@varchar_RecpFundEventId", DbType.String, dtBenifAccountTempTable.Rows[datarow]["RecpFundEventId"]);
        //            db.AddInParameter(insertCommandWrapper, "@QDRORequestId", DbType.String, QDRORequestID);
        //            db.AddInParameter(insertCommandWrapper, "@SplitType", DbType.String, Plantype);
        //            for (int recpDatarow = 0; recpDatarow < dtRecptAccount.Rows.Count; recpDatarow++)
        //            {
        //                if (dtBenifAccountTempTable.Rows[datarow]["id"].ToString() == dtRecptAccount.Rows[recpDatarow]["PersId"].ToString())
        //                {
        //                    //Priya 11.06.08 Added if else statement to avoid null values
        //                    if (dtRecptAccount.Rows[recpDatarow]["Amount"] == System.DBNull.Value)
        //                    {
        //                        SplitAmount = 0.0;
        //                    }
        //                    else
        //                    {
        //                        SplitAmount = Convert.ToDouble(dtRecptAccount.Rows[recpDatarow]["Amount"]);
        //                    }
        //                    //SplitAmount=Convert.ToDouble(dtRecptAccount.Rows[recpDatarow]["Amount"]);	//  @SplitAmount 

        //                    if (Convert.ToDouble(SplitAmount) == 0.0)
        //                    {
        //                        if (dtRecptAccount.Rows[recpDatarow]["Percentage"] == System.DBNull.Value)
        //                        {
        //                            splitpercent = 0.0;
        //                        }
        //                        else
        //                        {
        //                            splitpercent = Convert.ToDouble(dtRecptAccount.Rows[recpDatarow]["Percentage"]);//  @SplitPercent
        //                        }
        //                        //End 11.06.08
        //                        //splitpercent=Convert.ToDouble(dtRecptAccount.Rows[recpDatarow]["Percentage"]);//  @SplitPercent
        //                    }
        //                }
        //            }
        //            db.AddInParameter(insertCommandWrapper, "@SplitPercent", DbType.Double, splitpercent);//  @SplitPercent
        //            db.AddInParameter(insertCommandWrapper, "@SplitAmount", DbType.Double, SplitAmount);//  @SplitAmount 
        //            for (int recpdatarow = 0; recpdatarow < dtRecptAccount.Rows.Count; recpdatarow++)
        //            {
        //                if (dtBenifAccountTempTable.Rows[datarow]["id"].ToString() == dtRecptAccount.Rows[recpdatarow]["PersId"].ToString())
        //                {
        //                    //Priya 11.06.08 Added if else statement to avoid null values
        //                    if (dtRecptAccount.Rows[recpdatarow]["TotalTotal"] == System.DBNull.Value)
        //                    {
        //                        BenefitAmount += 0.0;
        //                    }
        //                    else
        //                    {
        //                        BenefitAmount += Convert.ToDouble(dtRecptAccount.Rows[recpdatarow]["TotalTotal"]);
        //                    }
        //                    //BenefitAmount+=Convert.ToDouble(dtRecptAccount.Rows[recpdatarow]["TotalTotal"]);	
        //                    //End 11.06.08
        //                }
        //            }
        //            db.AddInParameter(insertCommandWrapper, "@BenefitAmount", DbType.Double, BenefitAmount);//  @BenefitAmount
        //            for (int datarowSelectedAmount = 0; datarowSelectedAmount < dtParticipantDetails.Rows.Count; datarowSelectedAmount++)
        //            {
        //                SelectedAmount += Convert.ToDouble(dtParticipantDetails.Rows[datarowSelectedAmount]["TotalTotal"]);
        //            }
        //            db.AddInParameter(insertCommandWrapper, "@SelectedAmount", DbType.Double, SelectedAmount);
        //            TotalBalance = SelectedAmount - BenefitAmount;
        //            db.AddInParameter(insertCommandWrapper, "@TotalBalance", DbType.Double, TotalBalance);

        //            //Added By SG: 212.11.29: BT-1436
        //            db.AddOutParameter(insertCommandWrapper, "@guiQDRONonRetSplitID", DbType.String, 50);

        //            db.ExecuteNonQuery(insertCommandWrapper, pDBTransaction);

        //            //Added By SG: 212.11.29: BT-1436
        //            DataRow dr = dtSplitID.NewRow();
        //            dr[0] = dtBenifAccountTempTable.Rows[datarow]["RecpFundEventId"].ToString();
        //            dr[1] = db.GetParameterValue(insertCommandWrapper, "@guiQDRONonRetSplitID").ToString();
        //            dtSplitID.Rows.Add(dr);
        //        }
        //        //Added By SG: 212.11.29: BT-1436
        //        return dtSplitID;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        private static void InsertQdroNonRetSplit(DataTable qdroNonRetSplitTable, DbTransaction dbTransaction, Database db)
        {
            DbCommand insertCommandWrapper;
            DataSet qdroNonRetSplit;
            try
            {
                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_Qdro_InsertQdroNonRetSplit");
                db.AddInParameter(insertCommandWrapper, "@UNIQUEIDENTIFIER_QDRONonRetSplitID", DbType.String, "UniqueID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@VARHAR_BeneficiaryPersonID", DbType.String, "RecipientPersID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@DATETIME_EndDate", DbType.DateTime, "EndDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@DATETIME_StartDate", DbType.DateTime, "StartDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@VARCHAR_BeneficiaryFundEventID", DbType.String, "RecipientFundEventID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@VARCHAR_QDRORequestID", DbType.String, "QdroRequestID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@VARCHAR_SplitType", DbType.String, "SplitType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_SplitPercent", DbType.Decimal, "SplitPercent", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_SplitAmount", DbType.Decimal, "SplitAmount", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_TotalBalance", DbType.Decimal, "TotalBalance", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_SelectedAmount", DbType.Decimal, "SelectedAmount", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_BenefitAmount", DbType.Decimal, "BenefitAmount", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@BIT_RecipientSpouse", DbType.Boolean, "RecipientSpouse", DataRowVersion.Current);
                //START: PPP | 11/29/2016 | YRS-AT-3145 | Passing fees columns to record recipient fees in AtsQdroNonRetSplit
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_FeeOnRetirementPlan", DbType.Decimal, "FeeOnRetirementPlan", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@NUMERIC_FeeOnSavingsPlan", DbType.Decimal, "FeeOnSavingsPlan", DataRowVersion.Current);
                //END: PPP | 11/29/2016 | YRS-AT-3145 | Passing fees columns to record recipient fees in AtsQdroNonRetSplit
                insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                qdroNonRetSplit = new DataSet();
                qdroNonRetSplit.Tables.Add(qdroNonRetSplitTable);
                db.UpdateDataSet(qdroNonRetSplit, "QdroNonRetSplit", insertCommandWrapper, null, null, dbTransaction);
            }
            catch
            {
                throw;
            }
            finally
            {
                qdroNonRetSplit = null;
                insertCommandWrapper = null;
            }
        }
        //END: PPP | 08/25/2016 | YRS-AT-2529 | Function signature as well as data storage part changed
        # endregion		
		
		#region InsertQDRORefunds
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to Insert fund event status.										//
		//		'***************************************************************************************************//

		private static void InsertQDRORefunds(DataRow beneficiarydatarow  ,string OriginalFundEventId, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertFundEventsUpdMStatus");
				db.AddInParameter(insertCommandWrapper,"@varchar_RecptFundEventId",DbType.String,beneficiarydatarow["RecpFundEventId"]);
                db.AddInParameter(insertCommandWrapper, "@varchar_RecptPersId", DbType.String, beneficiarydatarow["id"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_OriginalFundEventId",DbType.String,OriginalFundEventId);
                db.AddInParameter(insertCommandWrapper, "@varchar_MaritalCode", DbType.String, beneficiarydatarow["MaritalCode"]); //Manthan Rajguru | 2016.08.16 | YRS-AT-2482 -  Adding the marital code dynamically for beneficiary.
                db.AddInParameter(insertCommandWrapper, "@varchar_GenderCode", DbType.String, beneficiarydatarow["GenderCode"]); //Manthan Rajguru | 2016.08.16 | YRS-AT-2482 -  Adding the marital code dynamically for beneficiary.				
                db.ExecuteNonQuery(insertCommandWrapper,pDBTransaction);
			}
			catch 
			{
				throw;
			}
		}

		#endregion

		#region getQDRORecipient
		//		'***********************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI						//
		//		'Created By                :Amit Nigam             Modified On : 18/06/08									//
		//		'Modified By               :																				//
		//		'Modify Reason             :																				//
		//		'Constructor Description   :																				//
		//		'Event Description         :This event will excecute when the user gives the ssno on the beneficiaries tab  //
		//		'***********************************************************************************************************//
		public static DataSet getQDRORecipient(string parameterSSNo)
		{
			DataSet l_dataset_dsLookUpQDRORecipient = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getRecipient");
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_SSNO",DbType.String,parameterSSNo);
				l_dataset_dsLookUpQDRORecipient = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpQDRORecipient,"QDRORecipient");
				return l_dataset_dsLookUpQDRORecipient;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getQDRORecipientFromBenefiary
		//		'***********************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI					    //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08									//
		//		'Modified By               :																				//
		//		'Modify Reason             :																				//
		//		'Constructor Description   :																				//
		//		'Event Description         :This event will excecute when the user gives the ssno on the beneficiaries tab  //   
		//									and will fetch the details from the atsbeneficiaries table.						//
		//		'***********************************************************************************************************//
		public static DataSet getQDRORecipientFromBenefiary(string parameterTypeCode,string parameterBSSNo)
		{
			DataSet l_dataset_dsLookUpQDRORecipientfromBeneficiary = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getRecipientfromBeneficiary");
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@TypeCode",DbType.String,parameterTypeCode);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_BSSNO",DbType.String,parameterBSSNo);
				l_dataset_dsLookUpQDRORecipientfromBeneficiary = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpQDRORecipientfromBeneficiary ,"QDRORecipient");
				return l_dataset_dsLookUpQDRORecipientfromBeneficiary ;
			}
			catch 
			{
				throw;
			}
		}

		#endregion

		#region getQDROFundEventID
		//		'***************************************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI										//
		//		'Created By                :Amit Nigam             Modified On : 18/06/08													//
		//		'Modified By               :																								//
		//		'Modify Reason             :																								//
		//		'Constructor Description   :																								//
		//		'Event Description         :This event will excecute when the user gives the recipient person id and fecth the fundeventid. //
		//		'***************************************************************************************************************************//
		//   
		public static string getQDROFundEventID(string RecptPersId,out string param_string_message)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			string l_string_return = "";
			param_string_message = "";
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_geFundEventId");
				db.AddInParameter(insertCommandWrapper,"@varchar_PersID",DbType.String,RecptPersId);
				DataSet l_dataset_FundEvent = new DataSet();
				db.LoadDataSet(insertCommandWrapper, l_dataset_FundEvent,"FundEvent");
				if (l_dataset_FundEvent!=null)
				{
					if (l_dataset_FundEvent.Tables.Count>0)
					{
						if (l_dataset_FundEvent.Tables[0].Rows.Count>0)
						{
							l_string_return = l_dataset_FundEvent.Tables[0].Rows[0][0].ToString();
						}
					}					
				}
				if (l_string_return == "")
				{
					param_string_message = "Unable to locate fund event/employment event.";
				}

				return l_string_return;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getGUI_ID
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08							//
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the new uniqueid					                //
		//		'***************************************************************************************************//
		public static string getGUI_ID()
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getNewID");
				return db.ExecuteScalar(insertCommandWrapper).ToString();
				
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region InsertPersDtls
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08							//
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will insert the beneficiries details in the atsperss table   //
		//		'***************************************************************************************************//
		private static void InsertPersDtls(DataRow beneficiarydatarow,DbTransaction pDBTransaction,Database db)
		{
			DbCommand insertCommandWrapper = null;
            DataRow drAddress;
            DataTable dtAddress = new DataTable();
            DataSet dsAddress = new DataSet();
            try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertPersDtls");
                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueId", DbType.String, beneficiarydatarow["id"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_SSNO",DbType.String,beneficiarydatarow["SSNo"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_LastName",DbType.String,beneficiarydatarow["LastName"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_FirstName",DbType.String,beneficiarydatarow["FirstName"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_MiddleName",DbType.String,beneficiarydatarow["MiddleName"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_SalutationCode",DbType.String,beneficiarydatarow["SalutationCode"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_SuffixTitle",DbType.String,beneficiarydatarow["SuffixTitle"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_BirthDate",DbType.DateTime,beneficiarydatarow["BirthDate"]);
				db.AddInParameter(insertCommandWrapper,"@chvMaritalCode",DbType.String,beneficiarydatarow["MaritalCode"]);
                db.AddInParameter(insertCommandWrapper, "@chvGenderCode", DbType.String, beneficiarydatarow["GenderCode"]); //Manthan Rajguru | 2016.08.16 | YRS-AT-2482: Adding GenderCode.
				db.AddInParameter(insertCommandWrapper,"@EMail",DbType.String,beneficiarydatarow["EmailAddress"]);
				db.AddInParameter(insertCommandWrapper,"@PhoneNo",DbType.String,beneficiarydatarow["PhoneNumber"]);
                //Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                //db.AddInParameter(insertCommandWrapper,"@Add1",DbType.String,beneficiarydatarow["Address1"]);
                //db.AddInParameter(insertCommandWrapper,"@Add2",DbType.String,beneficiarydatarow["Address2"]);
                //db.AddInParameter(insertCommandWrapper,"@Add3",DbType.String,beneficiarydatarow["Address3"]);
                //db.AddInParameter(insertCommandWrapper,"@City",DbType.String,beneficiarydatarow["City"]);
                //db.AddInParameter(insertCommandWrapper,"@State",DbType.String,beneficiarydatarow["State"]);
                //db.AddInParameter(insertCommandWrapper,"@zip",DbType.String,beneficiarydatarow["Zip"]);
                //db.AddInParameter(insertCommandWrapper,"@Country",DbType.String,beneficiarydatarow["Country"]);
                db.ExecuteNonQuery(insertCommandWrapper,pDBTransaction);
                
                //Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                dtAddress.Columns.Add("UniqueId");
                dtAddress.Columns.Add("guiEntityId");
                dtAddress.Columns.Add("addr1");
                dtAddress.Columns.Add("addr2");
                dtAddress.Columns.Add("addr3");
                dtAddress.Columns.Add("city");
                dtAddress.Columns.Add("state");
                dtAddress.Columns.Add("zipCode");
                dtAddress.Columns.Add("country");
                dtAddress.Columns.Add("isActive");
                dtAddress.Columns.Add("isPrimary");
                dtAddress.Columns.Add("effectiveDate");
                dtAddress.Columns.Add("isBadAddress");
                dtAddress.Columns.Add("addrCode");
                dtAddress.Columns.Add("entityCode");
                dtAddress.Columns.Add("Note");
                dtAddress.Columns.Add("bitImportant");
                drAddress = dtAddress.NewRow();

                drAddress["guiEntityId"] = beneficiarydatarow["id"];
                drAddress["addr1"] = beneficiarydatarow["Address1"];
                drAddress["addr2"] = beneficiarydatarow["Address2"];
                drAddress["addr3"] = beneficiarydatarow["Address3"];
                drAddress["city"] = beneficiarydatarow["City"];
                drAddress["state"] = beneficiarydatarow["State"];
                drAddress["zipCode"] = beneficiarydatarow["Zip"];
                drAddress["country"] = beneficiarydatarow["Country"];
                drAddress["isActive"] = true;
                drAddress["isPrimary"] = true;
                drAddress["effectiveDate"] = beneficiarydatarow["Address_effectiveDate"];
                drAddress["isBadAddress"] = beneficiarydatarow["BadAddress"];
                drAddress["addrCode"] = "HOME";
                drAddress["entityCode"] = "PERSON";
                drAddress["Note"] = beneficiarydatarow["AddressNote"].ToString();
                drAddress["bitImportant"] = beneficiarydatarow["AddressNote_bitImportant"];


                dtAddress.Rows.Add(drAddress);
                dtAddress.TableName = "Address";
                dsAddress.Tables.Add(dtAddress);
                AddressDAClass.SaveAddress(dsAddress, pDBTransaction, db);

			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getPartAccountDetailbyPlan
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08							//
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getPartAccountDetailbyPlan(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			DataSet l_dataset_dsLookUpPartAccountDetailbyPlan = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_AccountBalancesByPlan");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@FundEventID",DbType.String,FundEventID);
				db.AddInParameter(LookUpCommandWrapper,"@StartDate",DbType.String,StartDate);
				db.AddInParameter(LookUpCommandWrapper,"@EndDate",DbType.String,EndDate);
				db.AddInParameter(LookUpCommandWrapper,"@Plantype",DbType.String,PlanType);
			
				l_dataset_dsLookUpPartAccountDetailbyPlan = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpPartAccountDetailbyPlan,"PartAccountDetailbyPlan");
				return l_dataset_dsLookUpPartAccountDetailbyPlan;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getPartAccountDetailbyPlan
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08							//
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getGroupAPartAccountDetailbyPlan(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			DataSet l_dataset_dsLookUpPartAccountDetailbyPlan = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_GroupATransactionsByPlanType");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,FundEventID);
				db.AddInParameter(LookUpCommandWrapper,"@datetime_StartDate",DbType.String,StartDate);
				db.AddInParameter(LookUpCommandWrapper,"@datetime_EndDate",DbType.String,EndDate);
				db.AddInParameter(LookUpCommandWrapper,"@Plantype",DbType.String,PlanType);
			
				l_dataset_dsLookUpPartAccountDetailbyPlan = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpPartAccountDetailbyPlan,"PartAccountDetailbyPlan");
				return l_dataset_dsLookUpPartAccountDetailbyPlan;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08							//
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//




		public static DataSet getGroupBPartAccountDetailbyPlan(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			DataSet l_dataset_dsLookUpPartAccountDetailbyPlan = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_GroupBTransactionsByPlanType");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,FundEventID);
				db.AddInParameter(LookUpCommandWrapper,"@datetime_StartDate",DbType.String,StartDate);
				db.AddInParameter(LookUpCommandWrapper,"@datetime_EndDate",DbType.String,EndDate);
				db.AddInParameter(LookUpCommandWrapper,"@Plantype",DbType.String,PlanType);
			
				l_dataset_dsLookUpPartAccountDetailbyPlan = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpPartAccountDetailbyPlan,"PartAccountDetailbyPlan");
				return l_dataset_dsLookUpPartAccountDetailbyPlan;
			}
			catch 
			{
				throw;
			}
		}
		

		#region getAnnuityBasisDetail
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08							//
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getAnnuityBasisDetail(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			DataSet l_dataset_dsLookUpAnnuityBasisDetail = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_AnnuityBasisDetail");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@FundEventID",DbType.String,FundEventID);
				db.AddInParameter(LookUpCommandWrapper,"@StartDate",DbType.String,StartDate);
				db.AddInParameter(LookUpCommandWrapper,"@EndDate",DbType.String,EndDate);
				db.AddInParameter(LookUpCommandWrapper,"@Plantype",DbType.String,PlanType);
			
				l_dataset_dsLookUpAnnuityBasisDetail = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAnnuityBasisDetail,"AnnuityBasisDetail");
				return l_dataset_dsLookUpAnnuityBasisDetail;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region GetParticipantDetail
		//		'***************************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI							//
		//		'Created By                :Amit Nigam             Modified On : 18/06/08										//
		//		'Modified By               :																					//
		//		'Modify Reason             :																					//
		//		'Constructor Description   :																					//
		//		'Event Description         :This event will excecute when the user gives the ssno on the beneficiaries tab      //
		//		'***************************************************************************************************************//
		public static DataSet GetParticipantDetail(string PersSSID)
		{
			DataSet l_dataset_dsGetParticipantDetail = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_GetParticipantDetail");
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_SSId",DbType.String,PersSSID);
				l_dataset_dsGetParticipantDetail = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetParticipantDetail,"ParticipantDetail");
				return l_dataset_dsGetParticipantDetail;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region getYMCAId
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the YMCAId for the participant                    //
		//		'***************************************************************************************************//
		public static string getYMCAId(string PersId)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			string l_string_return = "";
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getYMCAId");
				db.AddInParameter(insertCommandWrapper,"@varchar_PersID",DbType.String,PersId);
				DataSet l_dataset_YMCAID = new DataSet();
				db.LoadDataSet(insertCommandWrapper, l_dataset_YMCAID,"YMCAID");
				if (l_dataset_YMCAID!=null)
				{
					if (l_dataset_YMCAID.Tables.Count>0)
					{
						if (l_dataset_YMCAID.Tables[0].Rows.Count>0)
						{
							l_string_return = l_dataset_YMCAID.Tables[0].Rows[0][0].ToString();
						}
					}					
				}
				return l_string_return;
			}
			catch 
			{
				throw;
			}
		}
		#endregion

		#region ValidateDisbursements
		//		'**********************************************************************************************************//
		//		'Class Name                :ValidateDisbursements               Used In     : YMCAUI                       //
		//		'Created By                :Dilip Patada			            created On  :28-01-2009    				   //
		//		'Modified By               :                                                                               //
		//		'Modify Reason             :                                                                               //
		//		'Constructor Description   :                                                                               //
		//		'Event Description         :This function is used for validating the refund, hardship, loan, retirement	   //
		//									Before spliting.															   //
		//		'**********************************************************************************************************//
		public static bool ValidateDisbursements(string parameterpersid,string QDROEndDate ,string PlanType)
		{
			Database db= null;
			DbCommand SelectCommandWrapper =null;
			bool intreturnStatus=false ;
            
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				SelectCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_PerformValidation");
				db.AddInParameter(SelectCommandWrapper,"@PersId", DbType.String,parameterpersid);
				db.AddInParameter(SelectCommandWrapper,"@QDROEndDate", DbType.String,QDROEndDate);
				db.AddInParameter(SelectCommandWrapper,"@PlanType", DbType.String,PlanType);
				db.AddOutParameter(SelectCommandWrapper,"@returnStatus",DbType.Boolean,0);
				db.AddOutParameter(SelectCommandWrapper,"@cOutput",DbType.String,1000);
				db.ExecuteNonQuery(SelectCommandWrapper);
				intreturnStatus=Convert.ToBoolean(db.GetParameterValue(SelectCommandWrapper,"@returnStatus"));
				return intreturnStatus;
			}
			catch
			{
				throw;
			}
		}
		#endregion 

		#region getFundedUnfundedTransactionsDetail
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDRODAClass               Used In     : YMCAUI                //
		//		'Created By                :Ganeswar Sahoo                      Modified On : 25/01/09							//
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getFundedUnfundedTransactionsDetail(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			DataSet l_dataset_FundedUnfundedTransactions = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_getFundedUnfundedTransactions");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,FundEventID);
				db.AddInParameter(LookUpCommandWrapper,"@datetime_StartDate",DbType.String,StartDate);
				db.AddInParameter(LookUpCommandWrapper,"@datetime_EndDate",DbType.String,EndDate);
				db.AddInParameter(LookUpCommandWrapper,"@Plantype",DbType.String,PlanType);
			
				l_dataset_FundedUnfundedTransactions = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_FundedUnfundedTransactions,"AnnuityBasisDetail");
				return l_dataset_FundedUnfundedTransactions;
			}
			catch 
			{
				throw;
			}
		}
		#endregion
		//Priya 22-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
		public static String ValidateEndOfMonth(string strQDROEndDate)
		{
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			String strMessage = string.Empty ;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_ValidateEndOfMonth");
				db.AddInParameter(LookUpCommandWrapper, "@varchar_QDROEndDate", DbType.String, strQDROEndDate);
				db.AddOutParameter(LookUpCommandWrapper, "@cOutput", DbType.String, 1000);
				db.ExecuteScalar(LookUpCommandWrapper);
				strMessage = db.GetParameterValue(LookUpCommandWrapper, "@cOutput").ToString();
				return strMessage;
	}
			catch (Exception ex)
			{
				throw ex;
}
		}
		//END Priya 22-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
		
		//Priya 25-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
		public static String ValidateQDROEDBalCuurentBalances(string strFundEventID, string strQDROStartDate, string strQDROEndDate, string strPlantype)
		{
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			String strMessage = string.Empty;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_ValidateQDROEDBalCuurentBalances");
				db.AddInParameter(LookUpCommandWrapper, "@varchar_FundEventID", DbType.String, strFundEventID);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_QDROStartDate", DbType.String, strQDROStartDate);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_QDROEndDate", DbType.String, strQDROEndDate);
				db.AddInParameter(LookUpCommandWrapper, "@Plantype", DbType.String, strPlantype);
				db.AddOutParameter(LookUpCommandWrapper, "@cOutput", DbType.String, 1000);
				db.ExecuteScalar(LookUpCommandWrapper);
				strMessage = db.GetParameterValue(LookUpCommandWrapper, "@cOutput").ToString();
				return strMessage;
	}
			catch (System.Data.SqlClient.SqlException ex)
			{
				throw ex;
}
		}
		//END Priya 22-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created

        //Added By SG: 2012.12.03: BT-1436:
        //START: PPP | 09/12/2016 | YRS-AT-2529 | Removed QDRONonRetSplitID and added planType parameter, as well as renamed existing parameters 
        //public static void UpdateQDRONonRetSplitTable(string QDRORequestID, string QDRONonRetSplitID, string RefRequestID, string FundEventID)
        public static void UpdateQDRONonRetSplitTable(string qdroRequestID, string refRequestID, string fundEventID, string planType)
        //END: PPP | 09/12/2016 | YRS-AT-2529 | Removed QDRONonRetSplitID and added planType parameter, as well as renamed existing parameters 
        {
            DbCommand insertCommandWrapper = null;
            Database db=null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_RefRequestInSplitNonRet");
                //START: PPP | 09/12/2016 | YRS-AT-2529 | Removed @QDRONonRetSplitID and added @VARCHAR_PlanType parameter, as well as renamed existing parameters 
                //db.AddInParameter(insertCommandWrapper, "@QDRORequestID", DbType.String, QDRORequestID);
                //db.AddInParameter(insertCommandWrapper, "@RefRequestID", DbType.String, RefRequestID);
                //db.AddInParameter(insertCommandWrapper, "@QDRONonRetSplitID", DbType.String, QDRONonRetSplitID);
                //db.AddInParameter(insertCommandWrapper, "@RecipientFundEventID", DbType.String, FundEventID);
                db.AddInParameter(insertCommandWrapper, "@UNIQUEIDENTIFIER_QDRORequestID", DbType.String, qdroRequestID);
                db.AddInParameter(insertCommandWrapper, "@UNIQUEIDENTIFIER_RefRequestID", DbType.String, refRequestID);
                db.AddInParameter(insertCommandWrapper, "@UNIQUEIDENTIFIER_RecipientFundEventID", DbType.String, fundEventID);
                db.AddInParameter(insertCommandWrapper, "@VARCHAR_PlanType", DbType.String, planType);
                //END: PPP | 09/12/2016 | YRS-AT-2529 | Removed @QDRONonRetSplitID and added @VARCHAR_PlanType parameter, as well as renamed existing parameters 

                db.ExecuteNonQuery(insertCommandWrapper);
            }
            catch
            {
                throw;
            }
        }
        //END: Added By SG: 2012.12.03: BT-1436:

		//03-Oct-2013 :SP: BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  - Start 
		private static void InsertEstateBeneficiary(DataRow parameterDataRowBeneficiary, DbTransaction parameterDbTransaction, Database parameterDbDatabase)
		{

			DbCommand dbCommandWrapper = null;

			try
			{


				dbCommandWrapper = parameterDbDatabase.GetStoredProcCommand("YRS_USP_QDRO_CreateEstateBeneficiary");

				parameterDbDatabase.AddInParameter(dbCommandWrapper, "@guiPersID", DbType.String, parameterDataRowBeneficiary["id"]);
				parameterDbDatabase.AddInParameter(dbCommandWrapper, "@beneficiaryType", DbType.String, "MEMBER");

				parameterDbDatabase.ExecuteNonQuery(dbCommandWrapper, parameterDbTransaction);
			}
			catch
			{
				throw;
			}
		}
		// -- 03-Oct-2013 :SP: BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  - End

        //START | SR | 2016.11.15 | YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215
        public static bool LookUpForParticipantNegativeBalance(string QdroRequestid, DbTransaction pDBTransaction, Database db) //PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Changed return type from short to bool
        {                       
            DbCommand LookUpCommandWrapper = null;
            try
            {
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_LookUpForParticipantNegativeBalance");
                if (LookUpCommandWrapper == null) return false;
                db.AddInParameter(LookUpCommandWrapper, "@UNIQUEIDENTIFIER_QDRORequestid", DbType.String, QdroRequestid);
                //START: PPP | 01/03/2017 | YRS-AT-3145 | Changed the output parameter, int was required to determine the affected plantype which is not used anymore, just negative balance flag is requiredto raise error
                //db.AddOutParameter(LookUpCommandWrapper, "@INT_Result", DbType.Int16, 0);
                db.AddOutParameter(LookUpCommandWrapper, "@BIT_IsNegativeBalance", DbType.Boolean, 0);
                //END: PPP | 01/03/2017 | YRS-AT-3145 | Changed the output parameter, int was required to determine the affected plantype which is not used anymore, just negative balance flag is requiredto raise error
                db.ExecuteNonQuery(LookUpCommandWrapper, pDBTransaction);
                //START: PPP | 01/03/2017 | YRS-AT-3145 | Returning bool
                //return Convert.ToInt16(db.GetParameterValue(LookUpCommandWrapper, "@INT_Result").ToString());
                return Convert.ToBoolean(db.GetParameterValue(LookUpCommandWrapper, "@BIT_IsNegativeBalance"));
                //END: PPP | 01/03/2017 | YRS-AT-3145 | Returning bool
            }
            catch
            {
                throw;
            }
        }
        //END | SR | 2016.11.15 | YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215

        //START: PPP | 11/30/2016 | YRS-AT-3145 | Initiate charging fee to participant and its recipient(s)
        private static string ChargeFee(string requestID, string module, DbTransaction transaction, Database db)
        {
            DbCommand command;
            bool isSuccessfullyCompleted, isRTBalanceValid;
            string result;
            try
            {
                command = db.GetStoredProcCommand("yrs_usp_QDRO_ChargeFee");
                db.AddInParameter(command, "@VARCHAR_QDRORequestID", DbType.String, requestID);
                db.AddInParameter(command, "@VARCHAR_Module", DbType.String, module);
                db.AddOutParameter(command, "@BIT_IsSuccessfullyCompleted", DbType.Boolean, 0);
                db.AddOutParameter(command, "@BIT_IsRTBalanceValid", DbType.Boolean, 0);
                db.ExecuteNonQuery(command, transaction);
                isSuccessfullyCompleted = Convert.ToBoolean(db.GetParameterValue(command, "@BIT_IsSuccessfullyCompleted"));
                isRTBalanceValid = Convert.ToBoolean(db.GetParameterValue(command, "@BIT_IsRTBalanceValid"));

                result = string.Empty;
                if (!isRTBalanceValid)
                {
                    result = "MESSAGE_QRDO_NO_RT_FOR_FEE_ROPR";
                }
                else if (!isSuccessfullyCompleted)
                {
                    result = "MESSAGE_QDRO_AFTER_FEE_NEGATIVEBALANCE";
                }
                return result;
            }
            catch
            {
                throw;
            }
            finally 
            {
                result = null;
                command = null;
            }
        }
        //END: PPP | 11/30/2016 | YRS-AT-3145 | Initiate charging fee to participant and its recipient(s)

        //START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 
        // Provides all recipient details saved for given request id
        public static DataSet GetRecipientDetails(string requestID)
        {
            DataSet ds = null;
            Database db = null;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                cmd = db.GetStoredProcCommand("yrs_usp_QDRO_GetRecipientStaging");
                if (cmd == null) return null;
                db.AddInParameter(cmd, "@VARCHAR_RequestID", DbType.String, requestID);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "RecipientDetail");
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd = null;
                db = null;
                ds = null;
            }
        }

        // Performs create, update and delete operation on recipient details
        public static void MaintainRecipientDetails(string recipientDetails, string requestID, string action)
        {
            Database db;
            DbCommand cmd;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                cmd = db.GetStoredProcCommand("yrs_usp_QDRO_MaintainRecipientStaging");
                db.AddInParameter(cmd, "@XML_RecipientDetails", DbType.Xml, recipientDetails);
                db.AddInParameter(cmd, "@VARCHAR_RequestID", DbType.String, requestID);
                db.AddInParameter(cmd, "@VARCHAR_Action", DbType.String, action);
                db.ExecuteNonQuery(cmd);
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd = null;
                db = null;
            }
        }

        // Helps to clear recipient from staging table
        public static void ClearRecipientStaging(string requestID, DbTransaction pDBTransaction, Database db) //PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Changed return type from short to bool
        {
            DbCommand LookUpCommandWrapper;
            try
            {
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_ClearRecipientStaging");
                db.AddInParameter(LookUpCommandWrapper, "@VARCHAR_RequestID", DbType.String, requestID);
                db.ExecuteNonQuery(LookUpCommandWrapper, pDBTransaction);
            }
            catch
            {
                throw;
            }
        }
        //END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 
    }
}
