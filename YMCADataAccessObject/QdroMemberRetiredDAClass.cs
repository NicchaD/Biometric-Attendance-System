// Project Name		:		
// FileName			:	QdroMemberRetiredDAClass.cs
// Author Name		:	Nidhin Raj	
// Employee ID		:	37232
// Email			:	nidhin.raj@3i-infotech.com
// Contact No		:	080-39876746
// Creation Time	:	13/6/2008 
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>

//
// Changed by			:	Dilip 
// Changed on			:	18/12/2008
// Change Description	:	The retirement type in that atsRetirees record for the 
//							Alternate Payee should be the same retirement type as 
//		exists in the original retirees record
//*****************************************************************************************************************************
//Modification History
//*****************************************************************************************************************************
//Modified by           Date                Description
//*****************************************************************************************************************************
//Harshala Trumukhe     23 Mar 2012         BT ID : 1002 - Error in Retired QDRO Settlement.
//Anudeeep A            18 jul 2013         BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
//Shashank Patel	    03-Oct-2013		    BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Chandra sekar.c       2016.07.05          YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
//Manthan Rajguru       2016.08.16          YRS-AT-2482: Add gender and marital status fields to QDRO beneficiary add screen (TrackIT 22117)
//Chandra sekar         2016.08.22          YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
//Pramod P. Pokale      2017.01.24          YRS-AT-3299 - YRS enh:improve usability of QDRO split screens(Retired) (TrackIT 28050) 
//*****************************************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for QdroMemberRetiredDAClass.
	/// </summary>
	public class QdroMemberRetiredDAClass
	{
		//int  parameterNewRecpient =0;
		public QdroMemberRetiredDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpRetiredList(string parameterSSNo,string parameterFundNo,string parameterLastName,string parameterFirstName,string parameterState,string parameterCity)
		{
			DataSet l_dataset_dsLookUpRetiredList = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_RetiredList");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_SSNo",DbType.String,parameterSSNo);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundIdNo",DbType.String,parameterFundNo);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FirstName",DbType.String,parameterFirstName);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_LastName",DbType.String,parameterLastName);
				             
				db.AddInParameter(LookUpCommandWrapper,"@varchar_City",DbType.String,parameterCity);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_State",DbType.String,parameterState);
				
			
				l_dataset_dsLookUpRetiredList = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpRetiredList,"RetiredList");
				//System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
				return l_dataset_dsLookUpRetiredList;
			}
			catch 
			{
				throw;
			}
		}

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
				//System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
				return l_dataset_dsLookUpQDRORecipient;
			}
			catch 
			{
				throw;
			}
		}
		
		public static DataSet getParticipantAccountDetail(string parameterSSNo,string parameterFundEventID,string parameterPlanType)
		{
			DataSet l_dataset_dsLookUpPartAccountDetail = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_AccountBalancesRetired");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,parameterFundEventID);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_SSNO",DbType.String,parameterSSNo);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_PlanType",DbType.String,parameterPlanType);
				
				l_dataset_dsLookUpPartAccountDetail = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpPartAccountDetail,"PartAccountDetail");
				//System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
				return l_dataset_dsLookUpPartAccountDetail;
			}
			catch 
			{
				throw;
			}
		}

		private static void InsertQDRORefunds(DataRow beneficiarydatarow  ,string OriginalFundEventId, DbTransaction DBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertFundEventsRetired");
				db.AddInParameter(insertCommandWrapper,"@varchar_RecptFundEventId",DbType.String,beneficiarydatarow["FundEventID"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_RecptPersId",DbType.String,beneficiarydatarow["id"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_OriginalFundEventId",DbType.String,OriginalFundEventId);
				db.ExecuteNonQuery(insertCommandWrapper,DBTransaction);
			}
			catch 
			{
				throw;
			}
		}

		private static void InsertRetireeQDRO(DataRow beneficiarydatarow,string OriginalFundEventId,DbTransaction DBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertRetireeQDRO");
				db.AddInParameter(insertCommandWrapper,"@varchar_RetireeId",DbType.String,beneficiarydatarow["RecptRetireeID"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_RecptFundEventId",DbType.String,beneficiarydatarow["FundEventID"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_RecptPersId",DbType.String,beneficiarydatarow["id"]);
				db.AddInParameter(insertCommandWrapper,"@varchar_OriginalFundEventId",DbType.String,OriginalFundEventId); // Added by dilip on 18/12/2008 (OriginalFundEventId)
				db.ExecuteNonQuery(insertCommandWrapper,DBTransaction);
			}
			catch 
			{
				throw;
			}
		}

		public static void insertPersDetails(DataRow beneficiarydatarow, DbTransaction DBTransaction, Database db)
		{

            DataRow drAddress;
            DataTable dtAddress = new DataTable();
            DataSet dsAddress = new DataSet();
			DbCommand l_DBCommandWrapper = null;
			//Database db = null;
			
			try
			{
				//for (int datarow=0;datarow<dtBenifAccountTempTable.Rows.Count;datarow++)
				//{
					//db= DatabaseFactory.CreateDatabase("YRS");				
																												
					l_DBCommandWrapper = db.GetStoredProcCommand("yrs_usp_QDRO_InsertPersDtls");

					db.AddInParameter(l_DBCommandWrapper,"@varchar_UniqueId",DbType.String,beneficiarydatarow["id"]);		 
					db.AddInParameter(l_DBCommandWrapper,"@varchar_SSNO",DbType.String,beneficiarydatarow["SSNo"]);		 		 
					db.AddInParameter(l_DBCommandWrapper,"@varchar_LastName",DbType.String,beneficiarydatarow["LastName"]);		        
					db.AddInParameter(l_DBCommandWrapper,"@varchar_FirstName",DbType.String,beneficiarydatarow["FirstName"]);		 	     
					db.AddInParameter(l_DBCommandWrapper,"@varchar_MiddleName",DbType.String,beneficiarydatarow["MiddleName"]);		 	     
					db.AddInParameter(l_DBCommandWrapper,"@varchar_SalutationCode",DbType.String,beneficiarydatarow["SalutationCode"]);		      
					db.AddInParameter(l_DBCommandWrapper,"@varchar_SuffixTitle",DbType.String,beneficiarydatarow["SuffixTitle"]);		 	     
					db.AddInParameter(l_DBCommandWrapper,"@varchar_BirthDate", DbType.DateTime,beneficiarydatarow["BirthDate"]);		 	 
					db.AddInParameter(l_DBCommandWrapper,"@chvMaritalCode",DbType.String,beneficiarydatarow["MaritalCode"]);
                    db.AddInParameter(l_DBCommandWrapper, "@chvGenderCode", DbType.String, beneficiarydatarow["GenderCode"]);	//Manthan Rajguru | 2016.08.16 | YRS-AT-2482: Adding gender code.
                    db.AddInParameter(l_DBCommandWrapper, "@EMail", DbType.String, beneficiarydatarow["EmailAddress"]); //PPP | 01/23/2017 | YRS-AT-3299 | Changed "EMail" to "EmailAddress"
                    db.AddInParameter(l_DBCommandWrapper, "@PhoneNo", DbType.String, beneficiarydatarow["PhoneNumber"]); //PPP | 01/23/2017 | YRS-AT-3299 | Changed "PhoneNo" to "PhoneNumber"
                    //Commented by Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                    //db.AddInParameter(l_DBCommandWrapper,"@Add1",DbType.String,beneficiarydatarow["Add1"]);		 	 	
                    //db.AddInParameter(l_DBCommandWrapper,"@Add2",DbType.String,beneficiarydatarow["Add2"]);		 	 
                    //db.AddInParameter(l_DBCommandWrapper,"@Add3",DbType.String,beneficiarydatarow["Add3"]);		 	 	
                    //db.AddInParameter(l_DBCommandWrapper,"@City",DbType.String,beneficiarydatarow["City"]);		 		 	
                    //db.AddInParameter(l_DBCommandWrapper,"@State",DbType.String,beneficiarydatarow["State"]);		 	         	
                    //db.AddInParameter(l_DBCommandWrapper,"@zip",DbType.String,beneficiarydatarow["zip"]);		 		 	
                    //db.AddInParameter(l_DBCommandWrapper,"@Country",DbType.String,beneficiarydatarow["Country"]);
                                       							
					//l_DBCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationSettings.AppSettings ["SmallConnectionTimeOut"]) ;
					db.ExecuteNonQuery(l_DBCommandWrapper,DBTransaction);
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
                    drAddress["addr1"] = beneficiarydatarow["Address1"]; //PPP | 01/23/2017 | YRS-AT-3299 | Changed "Add1" to "Address1"
                    drAddress["addr2"] = beneficiarydatarow["Address2"]; //PPP | 01/23/2017 | YRS-AT-3299 | Changed "Add2" to "Address2"
                    drAddress["addr3"] = beneficiarydatarow["Address3"]; //PPP | 01/23/2017 | YRS-AT-3299 | Changed "Add3" to "Address3"
                    drAddress["city"] = beneficiarydatarow["City"];
                    drAddress["state"] = beneficiarydatarow["State"];
                    drAddress["zipCode"] = beneficiarydatarow["zip"];
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
                    AddressDAClass.SaveAddress(dsAddress, DBTransaction, db);
				//}

			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetNewGuid()
		{
			DataSet dsNewGuid = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_QDRO_GetNewGuid");
				
				if (getCommandWrapper == null) return null;
						
				dsNewGuid = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsNewGuid,"NewGuid");
				
				return dsNewGuid;
			}
			catch 
			{
				throw;
			}
		}

        public static void SaveRetiredSplit(DataTable dtBenifAccountTempTable, DataTable dtRecptAccount, string RecptFundEventId, string OriginalFundEventId, string QDRORequestID, DataTable dtPartAccount, decimal Totalsplitpercentage, DataTable dtBenifAccount) //PPP | 01/24/2017 | YRS-AT-3299 | Changed the datatype of Totalsplitpercentage from Double to Decimal
        {
            bool bool_TransactionStarted = false;
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();

                DBTransaction = DBconnectYRS.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                bool_TransactionStarted = true;

                foreach (DataRow beneficiarydatarow in dtBenifAccountTempTable.Rows)
                {
                    if (Convert.ToBoolean((beneficiarydatarow["FlagNewBenf"].ToString())))
                    {
                        insertPersDetails(beneficiarydatarow, DBTransaction, db);
                    }
                    //03-Oct-2013: SP: BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee
                    InsertEstateBeneficiary(beneficiarydatarow, DBTransaction, db);
                }

                foreach (DataRow benificiaryFundEventDataRow in dtBenifAccountTempTable.Rows)
                {
                    InsertQDRORefunds(benificiaryFundEventDataRow, OriginalFundEventId, DBTransaction, db);
                    InsertRetireeQDRO(benificiaryFundEventDataRow, OriginalFundEventId, DBTransaction, db);  // Added by dilip on 18/12/2008 (OriginalFundEventId)
                }

                SaveRetireesData(Totalsplitpercentage, dtRecptAccount, RecptFundEventId, OriginalFundEventId, QDRORequestID, dtPartAccount, dtBenifAccount, DBTransaction, db);

                ClearRecipientStaging(QDRORequestID, DBTransaction, db); //PPP | 01/24/2017 | YRS-AT-3299 | On successful save, deletes archieved recipients from staging table

                DBTransaction.Commit();
                bool_TransactionStarted = false;
                DBconnectYRS.Close();
            }
            catch
            {
                if (bool_TransactionStarted)
                {
                    DBTransaction.Rollback();
                    DBconnectYRS.Close();
                }
                throw;
            }
        }


        // Retiree QDRO Details will be inserted into the database 
        // #1.FOR Looping is for Beneficiary
        // #2.FOR Loop based on SplitType
        // #3.FOR Loop based on SplitType Person and Annuities and adding Split Amounts
        // #4.FOR Looping is based on  recipient Annuities
		public static void SaveRetireesData(decimal Totalsplitpercentage, DataTable dtRecptAccount, string RecptFundEventId, string OriginalFundEventId, string QDRORequestID, DataTable dtPartAccount, DataTable dtBenifAccount, DbTransaction DBTransaction, Database db) //PPP | 01/24/2017 | YRS-AT-3299 | Changed the datatype of Totalsplitpercentage from Double to Decimal
        {

            //Database db = null;
            DbCommand CommandSaveRetiredSplit = null;
            DbCommand CommandSaveRetiredSplitPartAccount = null;
            DbCommand CommandSaveRetiredSplitParticipant = null;

            string QdroRetiredSplitID = "";
            //DataRow[] dataRowBenificiaryArray = dtRecptAccount.Select("distinct RecptRetireeIDlike '%%'", "");
            //START - Chandra sekar- 2016.08.22- YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            for (int datarowBenf = 0; datarowBenf < dtBenifAccount.Rows.Count; datarowBenf++)
            {
                //db=DatabaseFactory.CreateDatabase("YRS");
                string BeneficiaryPersonID = "";
                string receipientFundEventID = "";
                string splitType = "";
                double mnySplitAmount = 0.0;
                string mnySplitPercent = "";
                bool bitShareSpecialDiv = false;
                bool l_IsSplitFound = false;
                string[] targetColumns = { "splitType" };
                string[] compareColumns = { "splitType" };
                string[] clubbedColumns = null;
                DataTable dtSplitType;
                // Get distinct basis type clubbed amount
                dtSplitType = YMCARET.YmcaDataAccessObject.YMCACommonDAClass.SelectDistinct(dtRecptAccount, targetColumns, compareColumns, clubbedColumns);

                // 1#
                for (int datarows = 0; datarows < dtSplitType.Rows.Count; datarows++)
                {
                    mnySplitAmount = 0.00;
                    for (int datarow = 0; datarow < dtRecptAccount.Rows.Count; datarow++)
                    {
                        if ((dtBenifAccount.Rows[datarowBenf]["id"].ToString().Trim() == dtRecptAccount.Rows[datarow]["RecipientPersonID"].ToString().Trim()) && (dtSplitType.Rows[datarows]["splitType"].ToString().Trim() == dtRecptAccount.Rows[datarow]["splitType"].ToString().Trim()))
                        {
                            BeneficiaryPersonID = dtRecptAccount.Rows[datarow]["RecipientPersonID"].ToString();
                            receipientFundEventID = dtRecptAccount.Rows[datarow]["RecipientFundEventID"].ToString();
                            splitType = dtRecptAccount.Rows[datarow]["splitType"].ToString();
                            mnySplitAmount = mnySplitAmount + Convert.ToDouble(dtRecptAccount.Rows[datarow]["CurrentPayment"]);
                            mnySplitPercent = Convert.ToBoolean(dtRecptAccount.Rows[datarow]["IsSplitPercentage"]) == true ? Convert.ToString(dtRecptAccount.Rows[datarow]["RecipientSplitPercent"]) : "0.00"; // Chandra sekar - 2016.07.02 - YRS-AT-2481
                            // mnySplitPercent = dtRecptAccount.Rows[datarow]["RecipientSplitPercent"].ToString(); //Commented by Chandra sekar - 2016.07.02 - YRS-AT-2481
                            bitShareSpecialDiv = Convert.ToBoolean(dtRecptAccount.Rows[datarow]["IncludeSpecialdev"]);
                            l_IsSplitFound = true;
                        }
                    }
                    if (l_IsSplitFound == true)
                    {
                        CommandSaveRetiredSplitParticipant = db.GetStoredProcCommand("yrs_usp_QDRO_SaveRetiredSplit");

                        db.AddInParameter(CommandSaveRetiredSplitParticipant, "@unique_QdroRequestID", DbType.String, QDRORequestID);
                        db.AddInParameter(CommandSaveRetiredSplitParticipant, "@unique_BeneficiaryPersonID", DbType.String, BeneficiaryPersonID);
                        db.AddInParameter(CommandSaveRetiredSplitParticipant, "@receipientFundEventID", DbType.String, receipientFundEventID);
                        db.AddInParameter(CommandSaveRetiredSplitParticipant, "@splitType", DbType.String, splitType);
                        db.AddInParameter(CommandSaveRetiredSplitParticipant, "@mnySplitAmount", DbType.Double, mnySplitAmount);
                        db.AddInParameter(CommandSaveRetiredSplitParticipant, "@numRecipientSplitPercent", DbType.String, mnySplitPercent);
                        db.AddInParameter(CommandSaveRetiredSplitParticipant, "@bitShareSpecialDiv", DbType.Boolean, bitShareSpecialDiv);
                        db.AddOutParameter(CommandSaveRetiredSplitParticipant, "@varchar_QdroRetiredSplitID", DbType.String, 1000);
                        db.ExecuteNonQuery(CommandSaveRetiredSplitParticipant, DBTransaction);
                        QdroRetiredSplitID = db.GetParameterValue(CommandSaveRetiredSplitParticipant, "@varchar_QdroRetiredSplitID").ToString().Trim();
                        // 2#
                        l_IsSplitFound = false;
                        for (int datarow = 0; datarow < dtRecptAccount.Rows.Count; datarow++)
                        {

                            if ((dtBenifAccount.Rows[datarowBenf]["id"].ToString().Trim() == dtRecptAccount.Rows[datarow]["RecipientPersonID"].ToString().Trim()) && (dtSplitType.Rows[datarows]["splitType"].ToString().Trim() == dtRecptAccount.Rows[datarow]["splitType"].ToString().Trim()))
                            {
                                //db=DatabaseFactory.CreateDatabase("YRS");
                                string strRecipientID = dtRecptAccount.Rows[datarow]["RecipientPersonID"].ToString().Trim();

                                CommandSaveRetiredSplit = db.GetStoredProcCommand("yrs_usp_QDRO_SaveRetiredSplitBeneficiary");

                                db.AddInParameter(CommandSaveRetiredSplit, "@unique_QdroRequestID", DbType.String, QDRORequestID);
                                db.AddInParameter(CommandSaveRetiredSplit, "@chrAnnuityType", DbType.String, dtRecptAccount.Rows[datarow]["AnnuityType"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@unique_BeneficiaryPersonID", DbType.String, dtRecptAccount.Rows[datarow]["RecipientPersonID"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@chvAnnuitySourceCode", DbType.String, dtRecptAccount.Rows[datarow]["AnnuitySourceCode"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@chvPlanType", DbType.String, dtRecptAccount.Rows[datarow]["PlanType"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@PurchaseDate", DbType.DateTime, dtRecptAccount.Rows[datarow]["PurchaseDate"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnySSLevelingAmount", DbType.String, dtRecptAccount.Rows[datarow]["SSLevelingAmt"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnySSReductionAmount", DbType.String, dtRecptAccount.Rows[datarow]["SSReductionAmt"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@dtmSSReductionEffDate", DbType.DateTime, dtRecptAccount.Rows[datarow]["SSReductionEftDate"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyCurrentPayment", DbType.String, dtRecptAccount.Rows[datarow]["CurrentPayment"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyPersonalPreTaxCurrentPayment", DbType.String, dtRecptAccount.Rows[datarow]["EmpPreTaxCurrentPayment"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyPersonalPostTaxCurrentPayment", DbType.String, dtRecptAccount.Rows[datarow]["EmpPostTaxCurrentPayment"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyYmcaPreTaxCurrentPayment", DbType.String, dtRecptAccount.Rows[datarow]["YmcaPreTaxCurrentPayment"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyPersonalPreTaxReserveRemaining", DbType.String, dtRecptAccount.Rows[datarow]["EmpPreTaxRemainingReserves"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyPersonalPostTaxReserveRemaining", DbType.String, dtRecptAccount.Rows[datarow]["EmpPostTaxRemainingReserves"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyYmcaPreTaxReserveRemaining", DbType.String, dtRecptAccount.Rows[datarow]["YmcapreTaxRemainingReserves"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@numRecipientSplitPercent", DbType.String, dtRecptAccount.Rows[datarow]["RecipientSplitPercent"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@unique_guiAnnuityID", DbType.String, dtRecptAccount.Rows[datarow]["guiAnnuityID"]);
                                //CommandSaveRetiredSplit.AddInParameter("@unique_RecipientPersID",DbType.String,RecipientPersonID);   

                                double splitamount = (Convert.ToDouble(dtRecptAccount.Rows[datarow]["AnnuityTotal"]) * Convert.ToDouble(dtRecptAccount.Rows[datarow]["RecipientSplitPercent"]) * (.01));
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnySplitAmount", DbType.Double, splitamount);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnySplitPercent", DbType.String, dtRecptAccount.Rows[datarow]["RecipientSplitPercent"]);
                                double totalbalance = Convert.ToDouble(dtRecptAccount.Rows[datarow]["AnnuityTotal"]) - Convert.ToDouble(splitamount);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyTotalBalance", DbType.Double, totalbalance);

                                db.AddInParameter(CommandSaveRetiredSplit, "@mnySelectedAmount", DbType.String, dtRecptAccount.Rows[datarow]["AnnuityTotal"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@mnyBenefitAmount", DbType.String, splitamount);

                                if (Convert.ToDouble(dtRecptAccount.Rows[datarow]["SSLevelingAmt"]) > 0)
                                {
                                    db.AddInParameter(CommandSaveRetiredSplit, "@bitSSLeveling ", DbType.Boolean, 1);
                                }
                                else
                                {
                                    db.AddInParameter(CommandSaveRetiredSplit, "@bitSSLeveling ", DbType.Boolean, 0);
                                }
                                db.AddInParameter(CommandSaveRetiredSplit, "@retireesFundEventID", DbType.String, OriginalFundEventId);
                                db.AddInParameter(CommandSaveRetiredSplit, "@receipientFundEventID", DbType.String, dtRecptAccount.Rows[datarow]["RecipientFundEventID"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@yrsJointSurvivorID", DbType.String, dtRecptAccount.Rows[datarow]["guiAnnuityJointSurvivorsID"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@splitType", DbType.String, dtRecptAccount.Rows[datarow]["splitType"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@recipientRetireeID", DbType.String, dtRecptAccount.Rows[datarow]["RecptRetireeID"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@AdjustmentBasisCode", DbType.String, dtRecptAccount.Rows[datarow]["AdjustmentBasisCode"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@bitShareSpecialDiv", DbType.Boolean, dtRecptAccount.Rows[datarow]["IncludeSpecialdev"]);
                                db.AddInParameter(CommandSaveRetiredSplit, "@chvMaritalCode", DbType.String, dtBenifAccount.Select("id = '" + strRecipientID + "'")[0]["MaritalCode"]); //Manthan Rajguru | 2016.08.16 | YRS-AT-2482: Adding the marital status.
                                db.AddInParameter(CommandSaveRetiredSplit, "@chvGenderCode", DbType.String, dtBenifAccount.Select("id = '" + strRecipientID + "'")[0]["GenderCode"]); //Manthan Rajguru | 2016.08.16 | YRS-AT-2482: Adding the marital status.
                                db.AddInParameter(CommandSaveRetiredSplit, "@varchar_THIS_QdroRetiredSplitID", DbType.String, QdroRetiredSplitID);
                                db.ExecuteNonQuery(CommandSaveRetiredSplit, DBTransaction);
                            }
                        }
                    }
                }
            }

            try
            {

                //foreach(DataRow DataRowBenificiary in dataRowBenificiaryArray)

                //Added for participant annuity
                for (int datarow = 0; datarow < dtPartAccount.Rows.Count; datarow++)
                {
                    //db=DatabaseFactory.CreateDatabase("YRS");
                    //Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
                    if (Convert.ToString(dtPartAccount.Rows[datarow]["Selected"]).Trim().ToLower() == "true")
                    {
                        //'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
                        CommandSaveRetiredSplitPartAccount = db.GetStoredProcCommand("yrs_usp_QDRO_SaveRetiredSplitRetirees");

                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@unique_QdroRequestID", DbType.String, QDRORequestID);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnySSLevelingAmount", DbType.String, dtPartAccount.Rows[datarow]["SSLevelingAmt"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnySSReductionAmount", DbType.String, dtPartAccount.Rows[datarow]["SSReductionAmt"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@dtmSSReductionEffDate", DbType.DateTime, dtPartAccount.Rows[datarow]["SSReductionEftDate"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnyCurrentPayment", DbType.String, dtPartAccount.Rows[datarow]["CurrentPayment"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnyPersonalPreTaxCurrentPayment", DbType.String, dtPartAccount.Rows[datarow]["EmpPreTaxCurrentPayment"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnyPersonalPostTaxCurrentPayment", DbType.String, dtPartAccount.Rows[datarow]["EmpPostTaxCurrentPayment"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnyYmcaPreTaxCurrentPayment", DbType.String, dtPartAccount.Rows[datarow]["YmcaPreTaxCurrentPayment"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnyPersonalPreTaxReserveRemaining", DbType.String, dtPartAccount.Rows[datarow]["EmpPreTaxRemainingReserves"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnyPersonalPostTaxReserveRemaining", DbType.String, dtPartAccount.Rows[datarow]["EmpPostTaxRemainingReserves"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@mnyYmcaPreTaxReserveRemaining", DbType.String, dtPartAccount.Rows[datarow]["YmcapreTaxRemainingReserves"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@unique_guiAnnuityID", DbType.String, dtPartAccount.Rows[datarow]["guiAnnuityID"]);
                        db.AddInParameter(CommandSaveRetiredSplitPartAccount, "@totalSplitercentage", DbType.Decimal, Totalsplitpercentage); //PPP | 01/24/2017 | YRS-AT-3299 | Changed the datatype of Totalsplitpercentage from Double to Decimal

                        db.ExecuteNonQuery(CommandSaveRetiredSplitPartAccount, DBTransaction);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

		//03-Oct-2013 :SP: BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  - Start 
		private static void InsertEstateBeneficiary(DataRow parameterDataRowBeneficiary, DbTransaction parameterDBTransaction, Database parameterDataBase)
		{

			DbCommand l_DBCommandWrapper = null;

			try
			{


				l_DBCommandWrapper = parameterDataBase.GetStoredProcCommand("YRS_USP_QDRO_CreateEstateBeneficiary");

				parameterDataBase.AddInParameter(l_DBCommandWrapper, "@guiPersID", DbType.String, parameterDataRowBeneficiary["id"]);
				parameterDataBase.AddInParameter(l_DBCommandWrapper, "@beneficiaryType", DbType.String, "RETIRE");

				parameterDataBase.ExecuteNonQuery(l_DBCommandWrapper, parameterDBTransaction);
			}
			catch
			{
				throw;
			}
		}
		// -- 03-Oct-2013 :SP: BT:2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  - End 

        //START: PPP | 01/24/2017 | YRS-AT-3299
        // Helps to clear recipient from staging table
        public static void ClearRecipientStaging(string requestID, DbTransaction pDBTransaction, Database db)
        {
            DbCommand cmd;
            try
            {
                cmd = db.GetStoredProcCommand("yrs_usp_QDRO_ClearRecipientStaging");
                db.AddInParameter(cmd, "@VARCHAR_RequestID", DbType.String, requestID);
                db.ExecuteNonQuery(cmd, pDBTransaction);
            }
            catch
            {
                throw;
            }
        }
        //END: PPP | 01/24/2017 | YRS-AT-3299
    }
}
