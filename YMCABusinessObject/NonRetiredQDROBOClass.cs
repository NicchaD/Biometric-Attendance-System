//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA Yrs	
// FileName			:	NonRetiredQDROBOClass.cs
// Author Name		:	Amit Nigam
// Employee ID		:	36413
// Email			:	amit.nigam@3i-infotech.com
// Contact No		:	080-39876761
// Creation Time	:	13/6/2008 
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*********************************************************************************************************************
// Changed by			:	
// Changed on			:	
// Change Description	:	
//  Dilip Patada        :   jan 28th 2009     BT-676 - QDRO validation procedure for withdawals (refund, hardship, loan, retirement) 
//                                            of funds within the plan to be split and the withdrawal transaction date is dated 
//                                            after the QDRO
//****************************************************
//Modification History
//****************************************************
//Modified by           Date            Description
//****************************************************
//Harshala Trimukhe	    26/04/2012	    YRS 5.0-1346:Cash out plan balance <= $5,000
//Priya					22-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
//Priya					25-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
//Sanjay R.             10-Oct-2012     BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
//Manthan Rajguru       2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Pramod P. Pokale      2016.08.24      YRS-AT-2529 - YRS Enh: request to QDRO function ( Non-Retired) to allow different amt or percent (TrackIT 23098) 
//Pramod P. Pokale      2016.09.13      YRS-AT-1973 - not handling the 'Adjust' option correctly (QDRO) 
//Pramod P. Pokale      2016.09.27      YRS-AT-2529 - YRS Enh: request to QDRO function ( Non-Retired) to allow different amt or percent (TrackIT 23098) 
//Sanjay GS Rawat       2016.11.15      YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215
//Pramod P. Pokale      2016.12.29      YRS-AT-3145 - YRS enh: Fees - QDRO fee processing
//*********************************************************************************************************************

using System;
using YMCARET.YmcaDataAccessObject;
using System.Data;
using System.Collections.Generic; //PPP | 08/24/2016 | YRS-AT-2529 

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for QdroMemberActiveBusinessClass.
	/// </summary>
	public class NonRetiredQDROBOClass
	{
		public NonRetiredQDROBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
#region LookUpActiveList
//		'*******************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                    //
//		'Created By                :Amit Nigam             Modified On : 18/06/08								//
//		'Modified By               :																			//
//		'Modify Reason             :                                                                            //
//		'Constructor Description   :                                                                            //
//		'Function Description      :This function is called when the user will click  on the OK button in  the  //
//		'                          :List Tab																    //
//		'*******************************************************************************************************//
		public static DataSet LookUpActiveList(string parameterSSNo,string parameterFundNo,string parameterLastName,string parameterFirstName,string parameterCityName,string parameterStateName)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.LookUpActiveList(parameterSSNo,parameterFundNo,parameterLastName,parameterFirstName,parameterCityName,parameterStateName);
			}
			catch
			{
				throw;
			}
		}
#endregion

#region 
//		'***************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
//		'Created By                :Amit Nigam            Modified On :                                     //
//		'Modified By               :                                                                        //
//		'Modify Reason             :                                                                        //
//		'Constructor Description   :                                                                        //
//		'Class Description         :Used to Perform Split Operations.										//
//		'***************************************************************************************************//
	public static DataSet getTransactionsQDRO(string FundEventID)//,string EndDate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getTransactionsQDRO(FundEventID);//EndDate
			}
			catch
			{
				throw;
			}
		}
#endregion

 #region 
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam            Modified On :                                     //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Class Description         :Used to Perform Split Operations.										//
		//		'***************************************************************************************************//
		public static DataSet getGroupBTransactionsQDRO(string FundEventID,string EndDate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getGroupBTransactionsQDRO(FundEventID,EndDate);
			}
			catch
			{
				throw;
			}
		}
 #endregion

#region getAccountingDate
//		'***************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
//		'Modified By               :                                                                        //
//		'Modify Reason             :                                                                        //
//		'Constructor Description   :                                                                        //
//		'Event Description         :This event will excecute when the user changes the end date             //
//		'***************************************************************************************************//
	public static DateTime getAccountingDate()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getAccountingDate();
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
////		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
////		'Created By                :Amit Nigam            Modified On :                                     //
////		'Modified By               :                                                                        //
////		'Modify Reason             :                                                                        //
////		'Constructor Description   :                                                                        //
////		'Class Description         :Used to save the qdro details.                                          //
////		'***************************************************************************************************//
//    //Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
//    public static bool SaveQDROFinal(DataTable dtBenifAccountTempTable, DataTable dtRecptAccount, string OriginalFundEventId, string QDRORequestID, string Plantype, string startdate, string enddate, string participantid, string AnnuityBasisType, DataTable dtParticipantDetails, DataSet dsTransactions, DataSet l_dataset_AnnuityBasisDetail, DataSet dsAllRecipantAccountsDetail, double YMCAInterestBalance, double dblPersonalInterestBalance, DataTable dtGroupBRecipant, DataTable dtGroupBParticipant, DataTable dtGroupARecipant, DataTable dtGroupAParticipant, DataSet dsALLGroupBRecipantDetails, DataSet dsALLGroupARecipantDetails, DataSet dsALLGroupBParticipantDetails, DataSet dsALLGroupAParticipantDetails, DataTable dtALlRecords, Boolean AdjustInterest, out DataTable QdroNonRetSplitID)
//    //Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END
//    //Sanjay R. : 10-Oct-2012 -   New Paarmeter 'AdjustInterest' is added 
//    {
//        try
//        {
//            //Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
//            return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.SaveQDROFinal(dtBenifAccountTempTable, dtRecptAccount, OriginalFundEventId, QDRORequestID, Plantype, startdate, enddate, participantid, AnnuityBasisType, dtParticipantDetails, dsTransactions, l_dataset_AnnuityBasisDetail, dsAllRecipantAccountsDetail, YMCAInterestBalance, dblPersonalInterestBalance, dtGroupBRecipant, dtGroupBParticipant, dtGroupARecipant, dtGroupAParticipant, dsALLGroupBRecipantDetails, dsALLGroupARecipantDetails, dsALLGroupBParticipantDetails, dsALLGroupAParticipantDetails, dtALlRecords, AdjustInterest, out QdroNonRetSplitID);
//            //Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END
//            //DataTable dtBenifAccountTempTable,DataTable dtRecptAccount,string OriginalFundEventId,string QDRORequestID,string Plantype,string startdate,string enddate,string participantid,string AnnuityBasisType,DataTable dtParticipantDetails,DataSet dsTransactions,DataSet dsAllRecipantAccountsDetail,DataSet l_dataset_AnnuityBasisDetail)
//        }
//        catch
//        {
//            throw;
//        }
//    }
//#endregion

    // SR | 2016.11.15 | YRS-AT-2990 - Changed Method return type from boolean to object of ReturnObject class to return multiple values from called method.
    public static YMCAObjects.ReturnObject<bool> SaveRequest(DataTable requestDetails, DataTable beneficiaryTable, DataTable qdroNonRetSplitTable, DataTable qdroNonRetDetailsTable, DataSet groupBTransactions, DataSet groupATransactions)
    {
        // Changing all rows state = Added
        foreach (DataRow row in groupATransactions.Tables[0].Rows)
        {
            row.SetAdded();
        }

        foreach (DataRow row in groupBTransactions.Tables[0].Rows)
        {
            row.SetAdded();
        }
        
        return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.SaveRequest(requestDetails, beneficiaryTable, qdroNonRetSplitTable, qdroNonRetDetailsTable, groupBTransactions, groupATransactions);
    }
    //END: PPP | 08/25/2016 | YRS-AT-2529 | New function SaveRequest introduced instead of this


#region getQDRORecipient
//		'***********************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI						//
//		'Created By                :Amit Nigam             Modified On : 18/06/08									//
//		'Modified By               :																				//
//		'Modify Reason             :																				//
//		'Constructor Description   :																				//
//		'Event Description         :This event will excecute when the user gives the ssno on the beneficiaries tab  //
//		'***********************************************************************************************************//
		public static DataSet getQDRORecipient(string parameterSSNo)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getQDRORecipient(parameterSSNo);
			}
			catch
			{
				throw;
			}
		}
#endregion

#region getQDRORecipientFromBenefiary
//		'***********************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI						//
//		'Created By                :Amit Nigam             Modified On : 18/06/08									//
//		'Modified By               :																				//
//		'Modify Reason             :																				//
//		'Constructor Description   :																				//
//		'Event Description         :This event will excecute when the user gives the ssno on the beneficiaries tab  //   
//									and will fetch the details from the atsbeneficiaries table.						//
//		'***********************************************************************************************************//
		public static DataSet getQDRORecipientFromBenefiary(string parameterTypeCode,string parameterBSSNo)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getQDRORecipientFromBenefiary(parameterTypeCode,parameterBSSNo);
			}
			catch
			{
				throw;
			}
		}
#endregion

#region getQDROFundEventID
//		'*******************************************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI											//
//		'Created By                :Amit Nigam             Modified On : 18/06/08														//
//		'Modified By               :																									//
//		'Modify Reason             :																									//
//		'Constructor Description   :																									//
//		'Event Description         :This event will excecute when the user gives the recipient person id and fecth the fundeventid.     //             //
//		'*******************************************************************************************************************************//
		public static string getQDROFundEventID(string RecptPersId,out string param_string_message)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getQDROFundEventID(RecptPersId,out param_string_message);
			}
			catch
			{
				throw;
			}
		}
#endregion

#region getGUI_ID
//		'***************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
//		'Modified By               :                                                                        //
//		'Modify Reason             :                                                                        //
//		'Constructor Description   :                                                                        //
//		'Event Description         :This event will fetch the new uniqueid									//
//		'***************************************************************************************************//
		public static string getGUI_ID()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getGUI_ID();
			}
			catch
			{
				throw;
			}
		}
#endregion

#region getYMCAId
//		'***************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
//		'Created By                :Amit Nigam             Modified On : 18/06/08							//
//		'Modified By               :                                                                        //
//		'Modify Reason             :                                                                        //
//		'Constructor Description   :                                                                        //
//		'Event Description         :This event will fetch the YMCAId for the participant                    //
//		'***************************************************************************************************//
		public static string getYMCAId(string PersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getYMCAId(PersId);
					
			}
			catch 
			{
				throw;
			}
		}
#endregion

#region getPartAccountDetailbyPlan
//		'***************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
//		'Modified By               :                                                                        //
//		'Modify Reason             :                                                                        //
//		'Constructor Description   :                                                                        //
//		'Event Description         :This event will fetch the account detail as per the plan type.          //
//		'***************************************************************************************************//
		public static DataSet getPartAccountDetailbyPlan(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getPartAccountDetailbyPlan(FundEventID,StartDate,EndDate,PlanType);
			}
			catch
			{
				throw;
			}
		}
#endregion 

		#region getGroupBPartAccountDetailbyPlan
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getGroupBPartAccountDetailbyPlan(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getGroupBPartAccountDetailbyPlan(FundEventID,StartDate,EndDate,PlanType);
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region getFundedUnfundedTransactionsDetail
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getFundedUnfundedTransactionsDetail(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getFundedUnfundedTransactionsDetail(FundEventID,StartDate,EndDate,PlanType);
			}
			catch
			{
				throw;
			}
		}
		#endregion


		#region getGroupAPartAccountDetailbyPlan
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getGroupAPartAccountDetailbyPlan(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getGroupAPartAccountDetailbyPlan(FundEventID,StartDate,EndDate,PlanType);
			}
			catch
			{
				throw;
			}
		}
		#endregion


#region getAnnuityBasisDetail
		//		'***************************************************************************************************//
		//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                //
		//		'Created By                :Amit Nigam             Modified On : 18/06/08                           //
		//		'Modified By               :                                                                        //
		//		'Modify Reason             :                                                                        //
		//		'Constructor Description   :                                                                        //
		//		'Event Description         :This event will fetch the account detail as per the plan type.          //
		//		'***************************************************************************************************//
		public static DataSet getAnnuityBasisDetail(string FundEventID,string StartDate,string EndDate,string PlanType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getAnnuityBasisDetail(FundEventID,StartDate,EndDate,PlanType);
			}
			catch
			{
				throw;
			}
		}
		#endregion 

#region GetParticipantDetail
//		'**********************************************************************************************************//
//		'Class Name                :NonRetiredQDROBOClass               Used In     : YMCAUI                       //
//		'Created By                :Amit Nigam             Modified On : 18/06/08								   //
//		'Modified By               :                                                                               //
//		'Modify Reason             :                                                                               //
//		'Constructor Description   :                                                                               //
//		'Event Description         :This event will excecute when the user gives the ssno on the beneficiaries tab //
//		'**********************************************************************************************************//
		public static DataSet GetParticipantDetail(string PersSSID)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.GetParticipantDetail(PersSSID);
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
	//		'Created By                :Dilip Patada			            created On  : 28-01-2009   				   //
	//		'Modified By               :                                                                               //
	//		'Modify Reason             :                                                                               //
	//		'Constructor Description   :                                                                               //
	//		'Event Description         :This function is used for validating the refund, hardship, loan, retirement	   //
	//									Before spliting.															   //
	//		'**********************************************************************************************************//
	public static bool ValidateDisbursements(string parameterpersid,string QDROEndDate ,string PlanType)
	{
		try
		{
			return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.ValidateDisbursements(parameterpersid,QDROEndDate,PlanType);
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
		try
		{
			return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.ValidateEndOfMonth( strQDROEndDate);
	}
		catch
		{
			throw;
}
	}
		//END Priya 22-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created

	//Priya 25-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
	public static String ValidateQDROEDBalCuurentBalances(string strFundEventID, string strQDROStartDate, string strQDROEndDate, string strPlantype)
	{
		try
		{
			return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.ValidateQDROEDBalCuurentBalances( strFundEventID,  strQDROStartDate,  strQDROEndDate,  strPlantype);
	}
		catch
		{
			throw;
		}
	}
		//END Priya 25-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created

    //Added By SG: 2012.12.03: BT-1436:
    //START: PPP | 09/12/2016 | YRS-AT-2529 | Removed QDRONonRetSplitID and added planType parameter, as well as renamed existing parameters 
    //public static void UpdateQDRONonRetSplitTable(string QDRORequestID, string QDRONonRetSplitID, string RefRequestID, string FundEventID)
    public static void UpdateQDRONonRetSplitTable(string qdroRequestID, string refRequestID, string fundEventID, string planType)
    //END: PPP | 09/12/2016 | YRS-AT-2529 | Removed QDRONonRetSplitID and added planType parameter, as well as renamed existing parameters 
    {
        try
        {
            //START: PPP | 09/12/2016 | YRS-AT-2529 | Removed QDRONonRetSplitID and added planType parameter, as well as renamed existing parameters 
            //NonRetiredQDRODAClass.UpdateQDRONonRetSplitTable(QDRORequestID, QDRONonRetSplitID, RefRequestID, FundEventID);
            NonRetiredQDRODAClass.UpdateQDRONonRetSplitTable(qdroRequestID, refRequestID, fundEventID, planType);
            //END: PPP | 09/12/2016 | YRS-AT-2529 | Removed QDRONonRetSplitID and added planType parameter, as well as renamed existing parameters 
        }
        catch
        {
            throw;
        }
    }
    //END: Added By SG: 2012.12.03: BT-1436:

    //START: PPP | 08/30/2016 | YRS-AT-2529
    //preparing data for AtsQdroNonRetSplit table
    public static DataTable PrepareQdroNonRetSplitTable(string requestID, DateTime startDate, DateTime endDate, DataTable beneficiaryDetails, DataTable splitConfiguration, DataTable participantAccountData, DataTable recipientAccountData)
    {
        DataTable qdroNonRetSplitTable;
        DataRow[] splitConfigurationRows;
        DataRow qdroNonRetSplitRow;
        string persID, fundEventID, planType, ssn;
        decimal participantOriginalBalance, beneficiaryAmount;
        bool isSpouse;

        decimal? feeOnRetirementPlan, feeOnSavingsPlan; //PPP | 11/29/2016 | YRS-AT-3145 
        try
        {
            qdroNonRetSplitTable = CreateQdroNonRetSplitTable();

            foreach (DataRow beneficiaryRow in beneficiaryDetails.Rows)
            {
                persID = Convert.ToString(beneficiaryRow["id"]);
                fundEventID = Convert.ToString(beneficiaryRow["RecpFundEventId"]);
                ssn = Convert.ToString(beneficiaryRow["SSNo"]);
                isSpouse = Convert.ToBoolean(beneficiaryRow["bitRecipientSpouse"]);
               
                //START: PPP | 11/29/2016 | YRS-AT-3145 | Fees value storing in variable
                feeOnRetirementPlan = Convert.IsDBNull(beneficiaryRow["FeeOnRetirementPlan"]) ? null : (decimal?)Convert.ToDecimal(beneficiaryRow["FeeOnRetirementPlan"]);
                feeOnSavingsPlan = Convert.IsDBNull(beneficiaryRow["FeeOnSavingsPlan"]) ? null : (decimal?)Convert.ToDecimal(beneficiaryRow["FeeOnSavingsPlan"]);
                //END: PPP | 11/29/2016 | YRS-AT-3145 | Fees value storing in variable

                splitConfigurationRows = splitConfiguration.Select(string.Format("PersId='{0}'", persID));
                if (splitConfigurationRows != null && splitConfigurationRows.Length > 0)
                {
                    foreach (DataRow configurationRow in splitConfigurationRows)
                    {
                        planType = Convert.ToString(configurationRow["PlanType"]);
                        participantOriginalBalance = GetParticipantOriginalBalance(planType, participantAccountData);
                        beneficiaryAmount = GetRecipientSplitBalance(fundEventID, planType, recipientAccountData);

                        qdroNonRetSplitRow = qdroNonRetSplitTable.NewRow();

                        qdroNonRetSplitRow["UniqueID"] = Guid.NewGuid().ToString();
                        qdroNonRetSplitRow["QdroRequestID"] = requestID;
                        qdroNonRetSplitRow["RecipientPersID"] = persID;
                        qdroNonRetSplitRow["RecipientFundEventID"] = fundEventID;
                        qdroNonRetSplitRow["SSN"] = ssn;
                        qdroNonRetSplitRow["StartDate"] = startDate;
                        qdroNonRetSplitRow["EndDate"] = endDate;
                        qdroNonRetSplitRow["SplitType"] = planType;
                        qdroNonRetSplitRow["SplitAmount"] = Convert.ToDecimal(configurationRow["Amount"]);
                        qdroNonRetSplitRow["SplitPercent"] = Convert.ToDecimal(configurationRow["Percentage"]);
                        qdroNonRetSplitRow["TotalBalance"] = participantOriginalBalance - beneficiaryAmount;
                        qdroNonRetSplitRow["SelectedAmount"] = participantOriginalBalance;
                        qdroNonRetSplitRow["BenefitAmount"] = beneficiaryAmount;
                        qdroNonRetSplitRow["RecipientSpouse"] = isSpouse;

                        //START: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record recipient fees in AtsQdroNonRetSplit
                        qdroNonRetSplitRow["FeeOnRetirementPlan"] = DBNull.Value;
                        qdroNonRetSplitRow["FeeOnSavingsPlan"] = DBNull.Value;
                        switch (planType.ToUpper())
                        {
                            case "BOTH":
                                if (feeOnRetirementPlan.HasValue)
                                    qdroNonRetSplitRow["FeeOnRetirementPlan"] = feeOnRetirementPlan;

                                if (feeOnSavingsPlan.HasValue)
                                    qdroNonRetSplitRow["FeeOnSavingsPlan"] = feeOnSavingsPlan;

                                break;
                            case "RETIREMENT":
                                if (feeOnRetirementPlan.HasValue)
                                    qdroNonRetSplitRow["FeeOnRetirementPlan"] = feeOnRetirementPlan;
                                break;
                            case "SAVINGS":
                                if (feeOnSavingsPlan.HasValue)
                                    qdroNonRetSplitRow["FeeOnSavingsPlan"] = feeOnSavingsPlan;
                                break;
                        }
                        //END: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record recipient fees in AtsQdroNonRetSplit
            
                        qdroNonRetSplitTable.Rows.Add(qdroNonRetSplitRow);
                    }
                }
            }

            return qdroNonRetSplitTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            ssn = null;
            planType = null;
            fundEventID = null;
            persID = null;
            qdroNonRetSplitRow = null;
            splitConfigurationRows = null;
            qdroNonRetSplitTable = null;
        }
    }

    private static DataTable CreateQdroNonRetSplitTable()
    {
        DataTable qdroNonRetSplitTable;
        try
        {
            qdroNonRetSplitTable = new DataTable();
            qdroNonRetSplitTable.Columns.Add(new DataColumn("UniqueID", Type.GetType("System.String")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("QdroRequestID", Type.GetType("System.String")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("RecipientPersID", Type.GetType("System.String")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("RecipientFundEventID", Type.GetType("System.String")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("SSN", Type.GetType("System.String")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("StartDate", Type.GetType("System.DateTime")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("EndDate", Type.GetType("System.DateTime")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("SplitType", Type.GetType("System.String")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("SplitAmount", Type.GetType("System.Decimal")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("SplitPercent", Type.GetType("System.Decimal")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("TotalBalance", Type.GetType("System.Decimal")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("SelectedAmount", Type.GetType("System.Decimal")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("BenefitAmount", Type.GetType("System.Decimal")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("RecipientSpouse", Type.GetType("System.Boolean")));
            //START: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record recipient fees in AtsQdroNonRetSplit
            qdroNonRetSplitTable.Columns.Add(new DataColumn("FeeOnRetirementPlan", Type.GetType("System.Decimal")));
            qdroNonRetSplitTable.Columns.Add(new DataColumn("FeeOnSavingsPlan", Type.GetType("System.Decimal")));
            //END: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record recipient fees in AtsQdroNonRetSplit
            qdroNonRetSplitTable.TableName = "QdroNonRetSplit";
            return qdroNonRetSplitTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            qdroNonRetSplitTable = null;
        }
    }

    private static decimal GetParticipantOriginalBalance(string planType, DataTable participantAccountData)
    {
        DataRow[] balanceRows;
        decimal total;
        try
        {
            total = 0;
            if (planType.ToLower() == "both")
            {
                balanceRows = participantAccountData.Select(""); // Select all rows
            }
            else
            {
                balanceRows = participantAccountData.Select(string.Format("PlanType='{0}'", planType));
            }

            foreach (DataRow balance in balanceRows)
            {
                total += Convert.ToDecimal(balance["mnyBalance"]);
            }
            return total;
        }
        catch
        {
            throw;
        }
        finally
        {
            balanceRows = null;
        }
    }

    private static decimal GetRecipientSplitBalance(string fundEventID, string planType, DataTable recipientAccountData)
    {
        DataRow[] balanceRows;
        string transactType;
        decimal personPreTax, personPostTax, personInterest;
        decimal ymcaPreTax, ymcaInterest;
        try
        {
            personPreTax = 0;
            personPostTax = 0;
            personInterest = 0;
            ymcaPreTax = 0;
            ymcaInterest = 0;
            if (planType.ToLower() == "both")
            {
                balanceRows = recipientAccountData.Select(String.Format("FundEventID='{0}'", fundEventID)); // Select all rows
            }
            else
            {
                balanceRows = recipientAccountData.Select(string.Format("FundEventID='{0}' and PlanType='{1}'", fundEventID, planType));
            }

            foreach (DataRow balance in balanceRows)
            {
                transactType = Convert.ToString(balance["TransactType"]);
                if (transactType == "QSPR")
                {
                    personPreTax += Convert.ToDecimal(balance["PersonalPreTax"]);
                    personPostTax += Convert.ToDecimal(balance["PersonalPostTax"]);
                    ymcaPreTax += Convert.ToDecimal(balance["YmcaPreTax"]);
                }
                else if (transactType == "QSIN")
                {
                    personInterest += Convert.ToDecimal(balance["PersonalPreTax"]);
                    ymcaInterest += Convert.ToDecimal(balance["YmcaPreTax"]);
                }
            }
            return personPreTax + personPostTax + ymcaPreTax + personInterest + ymcaInterest;
        }
        catch
        {
            throw;
        }
        finally
        {
            transactType = null;
            balanceRows = null;
        }
    }

    //preparing data for AtsQdroNonRetDetails table
    public static DataTable PrepareQdroNonRetDetailsTable(DataTable qdroNonRetSplitTable, DataTable recipientAccountData)
    {
        DataTable qdroNonRetDetailsTable;
        DataRow qdroNonRetDetailsRow;

        DataRow[] recipientAcountRows;
        string splitID, persID, fundEventID, planType;

        List<string> accountTypes, annuityBasisTypes, transactTypes;

        decimal personPreTax, personPostTax, ymcaPreTax;
        try
        {
            qdroNonRetDetailsTable = CreateQdroNonRetDetailsTable();
            foreach (DataRow splitRow in qdroNonRetSplitTable.Rows)
            {
                splitID = Convert.ToString(splitRow["UniqueID"]);
                persID = Convert.ToString(splitRow["RecipientPersID"]);
                fundEventID = Convert.ToString(splitRow["RecipientFundEventID"]);
                planType = Convert.ToString(splitRow["SplitType"]);

                if (planType.ToLower() == "both")
                {
                    recipientAcountRows = recipientAccountData.Select(String.Format("FundEventID='{0}'", fundEventID));
                }
                else
                {
                    recipientAcountRows = recipientAccountData.Select(string.Format("FundEventID='{0}' and PlanType='{1}'", fundEventID, planType));
                }

                accountTypes = GetDistinctAccountTypes(recipientAcountRows);
                annuityBasisTypes = GetDistinctAnnuityBasisTypes(recipientAcountRows);
                transactTypes = GetDistinctTransactTypes(recipientAcountRows);

                if (accountTypes.Count > 0 && annuityBasisTypes.Count > 0 && transactTypes.Count > 0)
                {
                    foreach (string accountType in accountTypes)
                    {
                        foreach (string annuityBasisType in annuityBasisTypes)
                        {
                            foreach (string transactType in transactTypes)
                            {
                                personPreTax = 0;
                                personPostTax = 0;
                                ymcaPreTax = 0;

                                // Create record
                                foreach (DataRow transaction in recipientAcountRows)
                                {
                                    if (Convert.ToString(transaction["AcctType"]) == accountType && Convert.ToString(transaction["AnnuityBasisType"]) == annuityBasisType && Convert.ToString(transaction["TransactType"]) == transactType)
                                    {
                                        personPreTax += Convert.ToDecimal(transaction["PersonalPreTax"]);
                                        personPostTax += Convert.ToDecimal(transaction["PersonalPostTax"]);
                                        ymcaPreTax += Convert.ToDecimal(transaction["YmcaPreTax"]);
                                    }
                                }

                                if (personPreTax > 0 || personPostTax > 0 || ymcaPreTax > 0)
                                {
                                    qdroNonRetDetailsRow = qdroNonRetDetailsTable.NewRow();

                                    qdroNonRetDetailsRow["NonRetSplitID"] = splitID;
                                    qdroNonRetDetailsRow["AccountType"] = Convert.ToString(accountType);
                                    qdroNonRetDetailsRow["AnnuityBasisType"] = Convert.ToString(annuityBasisType);
                                    qdroNonRetDetailsRow["PersonalPreTax"] = personPreTax;
                                    qdroNonRetDetailsRow["PersonalPostTax"] = personPostTax;
                                    qdroNonRetDetailsRow["YmcaPreTax"] = ymcaPreTax;

                                    qdroNonRetDetailsTable.Rows.Add(qdroNonRetDetailsRow);
                                }
                            }
                        }
                    }
                }
            }

            return qdroNonRetDetailsTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            qdroNonRetDetailsTable = null;
        }
    }

    private static DataTable CreateQdroNonRetDetailsTable()
    {
        DataTable qdroNonRetDetailsTable;
        try
        {
            qdroNonRetDetailsTable = new DataTable();
            qdroNonRetDetailsTable.Columns.Add(new DataColumn("NonRetSplitID", Type.GetType("System.Guid")));
            qdroNonRetDetailsTable.Columns.Add(new DataColumn("AccountType", Type.GetType("System.String")));
            qdroNonRetDetailsTable.Columns.Add(new DataColumn("AnnuityBasisType", Type.GetType("System.String")));
            qdroNonRetDetailsTable.Columns.Add(new DataColumn("PersonalPreTax", Type.GetType("System.Decimal")));
            qdroNonRetDetailsTable.Columns.Add(new DataColumn("PersonalPostTax", Type.GetType("System.Decimal")));
            qdroNonRetDetailsTable.Columns.Add(new DataColumn("YmcaPreTax", Type.GetType("System.Decimal")));
            qdroNonRetDetailsTable.TableName = "RecipientDatatoAtsQdroDetails";
            return qdroNonRetDetailsTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            qdroNonRetDetailsTable = null;
        }
    }

    private static List<string> GetDistinctAccountTypes(DataRow[] transactionRows)
    {
        List<string> distinctAccountTypes;
        try
        {
            distinctAccountTypes = new List<string>();
            foreach (DataRow transaction in transactionRows)
            {
                if (!distinctAccountTypes.Contains(Convert.ToString(transaction["AcctType"])))
                {
                    distinctAccountTypes.Add(Convert.ToString(transaction["AcctType"]));
                }
            }
            return distinctAccountTypes;
        }
        catch
        {
            throw;
        }
        finally
        {
            distinctAccountTypes = null;
        }
    }

    private static List<string> GetDistinctAnnuityBasisTypes(DataRow[] transactionRows)
    {
        List<string> distinctAnnuityBasisTypes;
        try
        {
            distinctAnnuityBasisTypes = new List<string>();
            foreach (DataRow transaction in transactionRows)
            {
                if (!distinctAnnuityBasisTypes.Contains(Convert.ToString(transaction["AnnuityBasisType"])))
                {
                    distinctAnnuityBasisTypes.Add(Convert.ToString(transaction["AnnuityBasisType"]));
                }
            }
            return distinctAnnuityBasisTypes;
        }
        catch
        {
            throw;
        }
        finally
        {
            distinctAnnuityBasisTypes = null;
        }
    }

    private static List<string> GetDistinctTransactTypes(DataRow[] transactionRows)
    {
        List<string> distinctTransactTypes;
        try
        {
            distinctTransactTypes = new List<string>();
            foreach (DataRow transaction in transactionRows)
            {
                if (!distinctTransactTypes.Contains(Convert.ToString(transaction["TransactType"])))
                {
                    distinctTransactTypes.Add(Convert.ToString(transaction["TransactType"]));
                }
            }
            return distinctTransactTypes;
        }
        catch
        {
            throw;
        }
        finally
        {
            distinctTransactTypes = null;
        }
    }

    // PrepareRequestDetailsTable is creating a table which holds general information of Split
    public static DataTable PrepareRequestDetailsTable(string participantPersID, string participantFundEventID, string requestID, bool isAdjustInterest, string startDate, string endDate
        , decimal? feeOnRetirementPlan, decimal? feeOnSavingsPlan //PPP | 11/29/2016 | YRS-AT-3145 | Accepting fees charged to participant
        )
    {
        DataTable requestDetails;
        DataRow requestDetailsRow;
        try
        {
            requestDetails = new DataTable();
            requestDetails.Columns.Add(new DataColumn("PersID", Type.GetType("System.String")));
            requestDetails.Columns.Add(new DataColumn("FundEventID", Type.GetType("System.String")));
            requestDetails.Columns.Add(new DataColumn("RequestID", Type.GetType("System.String")));
            requestDetails.Columns.Add(new DataColumn("IsAdjustInterest", Type.GetType("System.Boolean")));
            requestDetails.Columns.Add(new DataColumn("StartDate", Type.GetType("System.String")));
            requestDetails.Columns.Add(new DataColumn("EndDate", Type.GetType("System.String")));
            //START: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record participant fees in atsQdroRequests
            requestDetails.Columns.Add(new DataColumn("FeeOnRetirementPlan", Type.GetType("System.Decimal")));
            requestDetails.Columns.Add(new DataColumn("FeeOnSavingsPlan", Type.GetType("System.Decimal")));
            //END: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record participant fees in atsQdroRequests

            requestDetailsRow = requestDetails.NewRow();
            requestDetailsRow["PersID"] = participantPersID;
            requestDetailsRow["FundEventID"] = participantFundEventID;
            requestDetailsRow["RequestID"] = requestID;
            requestDetailsRow["IsAdjustInterest"] = isAdjustInterest;
            requestDetailsRow["StartDate"] = startDate;
            requestDetailsRow["EndDate"] = endDate;
            //START: PPP | 11/29/2016 | YRS-AT-3145 | Recording fees charged to participant 
            if (feeOnRetirementPlan == null)
            {
                requestDetailsRow["FeeOnRetirementPlan"] = DBNull.Value;
            }
            else
            {
                requestDetailsRow["FeeOnRetirementPlan"] = feeOnRetirementPlan;
            }

            if (feeOnSavingsPlan == null)
            {
                requestDetailsRow["FeeOnSavingsPlan"] = DBNull.Value;
            }
            else
            {
                requestDetailsRow["FeeOnSavingsPlan"] = feeOnSavingsPlan;
            }
            //END: PPP | 11/29/2016 | YRS-AT-3145 | Recording fees charged to participant 
            requestDetails.Rows.Add(requestDetailsRow);

            return requestDetails;
        }
        catch
        {
            throw;
        }
        finally
        {
            requestDetailsRow = null;
            requestDetails = null;
        }
    }

    // Following methods belongs to "show balance"
    // CreateAccountBalanceTable is creating empty table set required to be displayed on "Show Balance"
    private static DataTable CreateAccountBalanceTable()
    {
        DataTable accountTbale = new DataTable();
        accountTbale.Columns.Add("AcctType", typeof(System.String));
        accountTbale.Columns.Add("PersonalPreTax", typeof(System.Decimal));
        accountTbale.Columns.Add("PersonalPostTax", typeof(System.Decimal));
        accountTbale.Columns.Add("PersonalInterestBalance", typeof(System.Decimal));
        accountTbale.Columns.Add("PersonalTotal", typeof(System.Decimal));
        accountTbale.Columns.Add("YMCAPreTax", typeof(System.Decimal));
        accountTbale.Columns.Add("YMCAInterestBalance", typeof(System.Decimal));
        accountTbale.Columns.Add("YMCATotal", typeof(System.Decimal));
        accountTbale.Columns.Add("TotalTotal", typeof(System.Decimal));
        accountTbale.Columns.Add("Selected", typeof(System.Boolean));
        accountTbale.Columns.Add("PersId", typeof(System.String));
        accountTbale.Columns.Add("PlanType", typeof(System.String)); //MMR | 2016.11.30 | YRS-AT-3145 | Added column to get plan type
        return accountTbale;
    }

    private static List<string> GetDistinctAccountTypes(DataTable table)
    {
        List<string> distinctAccountTypes;
        try
        {
            distinctAccountTypes = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                if (!distinctAccountTypes.Contains(Convert.ToString(row["AcctType"])))
                {
                    distinctAccountTypes.Add(Convert.ToString(row["AcctType"]));
                }
            }
            return distinctAccountTypes;
        }
        catch
        {
            throw;
        }
        finally
        {
            distinctAccountTypes = null;
        }
    }

    // PrepareBeneficiaryAfterSplitAccountTable is creating table which hold Recipient/Beneficiary account balances
    public static DataTable PrepareBeneficiaryAfterSplitAccountTable(string fundEventID, string planType, DataSet recepientAfterSplitAccountsDetail, DataSet participantOriginalBalance)
    {
        DataSet participantAccountDetails;
        DataTable recipientAfterSplitValuesTable;
        DataRow[] rows;
        DataRow[] allAccountRows;
        DataRow finalRow;
        DataRow originalBalanceRow; //MMR | 2016.11.30 | YRS-AT-3145 | Declared datarow to store participant original balance row
        decimal personPreTax, personPostTax, personInterest, ymcaPreTax, ymcaInterest;

        List<string> distinctAccountTypes;
        try
        {
            participantAccountDetails = participantOriginalBalance.Copy();
            // Original balance
            if (planType.ToLower() != "both")
            {
                rows = participantAccountDetails.Tables[0].Select(string.Format("PlanType <> '{0}'", planType.ToUpper()));
                if (rows.Length > 0)
                {
                    foreach (DataRow data in rows)
                    {
                        data.Delete();
                    }
                    participantAccountDetails.AcceptChanges();
                }
            }

            recipientAfterSplitValuesTable = CreateAccountBalanceTable();

            distinctAccountTypes = GetDistinctAccountTypes(participantAccountDetails.Tables[0]);

            // Prepare recipient table
            foreach (string accountType in distinctAccountTypes)
            {
                if (recepientAfterSplitAccountsDetail.Tables[0].Select(string.Format("FundEventID='{0}' and AcctType='{1}'", fundEventID, accountType)).Length > 0)
                {
                    personPreTax = 0;
                    personPostTax = 0;
                    ymcaPreTax = 0;
                    personInterest = 0;
                    ymcaInterest = 0;

                    // Principal
                    allAccountRows = recepientAfterSplitAccountsDetail.Tables[0].Select(string.Format("FundEventID='{0}' and AcctType='{1}' and TransactType='QSPR'", fundEventID, accountType));
                    if (allAccountRows.Length > 0)
                    {
                        foreach (DataRow accountRows in allAccountRows)
                        {
                            personPreTax += Convert.ToDecimal((Convert.IsDBNull(accountRows["PersonalPreTax"]) ? 0 : accountRows["PersonalPreTax"]));
                            personPostTax += Convert.ToDecimal((Convert.IsDBNull(accountRows["PersonalPostTax"]) ? 0 : accountRows["PersonalPostTax"]));
                            ymcaPreTax += Convert.ToDecimal((Convert.IsDBNull(accountRows["YmcaPreTax"]) ? 0 : accountRows["YmcaPreTax"]));
                        }
                    }

                    // Interest
                    allAccountRows = recepientAfterSplitAccountsDetail.Tables[0].Select(string.Format("FundEventID='{0}' and AcctType='{1}' and TransactType='QSIN'", fundEventID, accountType));
                    if (allAccountRows.Length > 0)
                    {
                        foreach (DataRow accountRows in allAccountRows)
                        {
                            personInterest += Convert.ToDecimal((Convert.IsDBNull(accountRows["PersonalPreTax"]) ? 0 : accountRows["PersonalPreTax"]));
                            ymcaInterest += Convert.ToDecimal((Convert.IsDBNull(accountRows["YmcaPreTax"]) ? 0 : accountRows["YmcaPreTax"]));
                        }
                    }

                    //START: MMR | 2016.11.30 | YRS-AT-3145 | Added to get plan type
                    originalBalanceRow = participantAccountDetails.Tables[0].NewRow();
                    // Get original balance row
                    foreach (DataRow row in participantAccountDetails.Tables[0].Rows)
                    {
                        if (Convert.ToString(row["AcctType"]) == accountType)
                        {
                            originalBalanceRow = row;
                            break;
                        }
                    }
                    //END: MMR | 2016.11.30 | YRS-AT-3145 | Added to get plan type

                    finalRow = recipientAfterSplitValuesTable.NewRow();
                    finalRow["AcctType"] = accountType;
                    finalRow["PersonalPreTax"] = Math.Round(personPreTax, 2, MidpointRounding.AwayFromZero);
                    finalRow["PersonalPostTax"] = Math.Round(personPostTax, 2, MidpointRounding.AwayFromZero);
                    finalRow["PersonalInterestBalance"] = Math.Round(personInterest, 2, MidpointRounding.AwayFromZero);
                    finalRow["PersonalTotal"] = Math.Round(personPreTax + personPostTax + personInterest, 2, MidpointRounding.AwayFromZero);
                    finalRow["YMCAPreTax"] = Math.Round(ymcaPreTax, 2, MidpointRounding.AwayFromZero);
                    finalRow["YMCAInterestBalance"] = Math.Round(ymcaInterest, 2, MidpointRounding.AwayFromZero);
                    finalRow["YMCATotal"] = Math.Round(ymcaPreTax + ymcaInterest, 2, MidpointRounding.AwayFromZero);
                    finalRow["TotalTotal"] = Math.Round(personPreTax + personPostTax + personInterest + ymcaPreTax + ymcaInterest, 2, MidpointRounding.AwayFromZero);
                    finalRow["PlanType"] = originalBalanceRow["PlanType"]; //MMR | 2016.11.30 | YRS-AT-3145 | Assigning plan type to recipient split table
                    recipientAfterSplitValuesTable.Rows.Add(finalRow);
                }
            }

            return recipientAfterSplitValuesTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            finalRow = null;
            allAccountRows = null;
            rows = null;
            recipientAfterSplitValuesTable = null;
            participantAccountDetails = null;
        }
    }

    // PrepareParticipantAfterSplitAccountTable is creating table which holds remaining balance of participant
    public static DataTable PrepareParticipantAfterSplitAccountTable(string planType, DataSet participantAfterSplitAccountsDetail, DataSet participantOriginalBalance)
    {
        DataSet participantAccountDetails;
        DataTable participantAfterSplitValuesTable;
        DataRow[] allAccountRows, rows;
        DataRow finalRow;
        DataRow originalBalanceRow;
        decimal personPreTax, personPostTax, personInterest, ymcaPreTax, ymcaInterest;

        List<string> distinctAccountTypes;
        try
        {
            participantAfterSplitValuesTable = new DataTable(); // = null //PPP | 09/27/2016 | YRS-AT-2529 | Initiating it with a blank table instead of null value which could result "object reference null" error
            if (participantAfterSplitAccountsDetail != null)
            {
                participantAccountDetails = participantOriginalBalance.Copy();
                // Original balance
                if (planType.ToLower() != "both")
                {
                    rows = participantAccountDetails.Tables[0].Select(string.Format("PlanType <> '{0}'", planType.ToUpper()));
                    if (rows.Length > 0)
                    {
                        foreach (DataRow data in rows)
                        {
                            data.Delete();
                        }
                        participantAccountDetails.AcceptChanges();
                    }
                }

                participantAfterSplitValuesTable = CreateAccountBalanceTable();

                distinctAccountTypes = GetDistinctAccountTypes(participantAccountDetails.Tables[0]);
                // Prepare participant table
                foreach (string accountType in distinctAccountTypes)
                {
                    if (participantAfterSplitAccountsDetail.Tables[0].Select(string.Format("AcctType='{0}'", accountType)).Length > 0)
                    {
                        personPreTax = 0;
                        personPostTax = 0;
                        ymcaPreTax = 0;
                        personInterest = 0;
                        ymcaInterest = 0;

                        // All values in dsAllPartAccountsDetail are negative
                        // Principal
                        allAccountRows = participantAfterSplitAccountsDetail.Tables[0].Select(string.Format("AcctType='{0}' and TransactType='QWPR'", accountType));
                        if (allAccountRows.Length > 0)
                        {
                            foreach (DataRow accountRows in allAccountRows)
                            {
                                personPreTax += Convert.ToDecimal((Convert.IsDBNull(accountRows["PersonalPreTax"]) ? 0 : accountRows["PersonalPreTax"]));
                                personPostTax += Convert.ToDecimal((Convert.IsDBNull(accountRows["PersonalPostTax"]) ? 0 : accountRows["PersonalPostTax"]));
                                ymcaPreTax += Convert.ToDecimal((Convert.IsDBNull(accountRows["YmcaPreTax"]) ? 0 : accountRows["YmcaPreTax"]));
                            }
                        }

                        // Interest
                        allAccountRows = participantAfterSplitAccountsDetail.Tables[0].Select(string.Format("AcctType='{0}' and TransactType='QWIN'", accountType));
                        if (allAccountRows.Length > 0)
                        {
                            foreach (DataRow accountRows in allAccountRows)
                            {
                                personInterest += Convert.ToDecimal((Convert.IsDBNull(accountRows["PersonalPreTax"]) ? 0 : accountRows["PersonalPreTax"]));
                                ymcaInterest += Convert.ToDecimal((Convert.IsDBNull(accountRows["YmcaPreTax"]) ? 0 : accountRows["YmcaPreTax"]));
                            }
                        }

                        originalBalanceRow = participantAccountDetails.Tables[0].NewRow();
                        // Get original balance row
                        foreach (DataRow row in participantAccountDetails.Tables[0].Rows)
                        {
                            if (Convert.ToString(row["AcctType"]) == accountType)
                            {
                                originalBalanceRow = row;
                                break;
                            }
                        }

                        finalRow = participantAfterSplitValuesTable.NewRow();
                        finalRow["AcctType"] = accountType;
                        finalRow["PersonalPreTax"] = Math.Round(Convert.ToDecimal(originalBalanceRow["PersonalPreTax"]) + personPreTax, 2, MidpointRounding.AwayFromZero);
                        finalRow["PersonalPostTax"] = Math.Round(Convert.ToDecimal(originalBalanceRow["PersonalPostTax"]) + personPostTax, 2, MidpointRounding.AwayFromZero);
                        finalRow["PersonalInterestBalance"] = Math.Round(Convert.ToDecimal(originalBalanceRow["PersonalInterestBalance"]) + personInterest, 2, MidpointRounding.AwayFromZero);
                        //finalRow("PersonalTotal") = originalBalanceRow("PersonalAmt") + (personPreTax + personPostTax + personInterest)
                        finalRow["YMCAPreTax"] = Math.Round(Convert.ToDecimal(originalBalanceRow["YmcaPreTax"]) + ymcaPreTax, 2, MidpointRounding.AwayFromZero);
                        finalRow["YMCAInterestBalance"] = Math.Round(Convert.ToDecimal(originalBalanceRow["YmcaInterestBalance"]) + ymcaInterest, 2, MidpointRounding.AwayFromZero);
                        //finalRow("YMCATotal") = originalBalanceRow("YmcaAmt") + (ymcaPreTax + ymcaInterest)
                        finalRow["TotalTotal"] = Math.Round(Convert.ToDecimal(originalBalanceRow["mnyBalance"]) + (personPreTax + personPostTax + personInterest + ymcaPreTax + ymcaInterest), 2, MidpointRounding.AwayFromZero);
                        finalRow["PlanType"] = originalBalanceRow["PlanType"]; //MMR | 2016.11.30 | YRS-AT-3145 | Assigning plan type to participant split table
                        participantAfterSplitValuesTable.Rows.Add(finalRow);
                    }
                    //START: PPP | 09/27/2016 | YRS-AT-2529 | Adding non splited account types also
                    else
                    {
                        originalBalanceRow = participantAccountDetails.Tables[0].NewRow();
                        // Get original balance row
                        foreach (DataRow row in participantAccountDetails.Tables[0].Rows)
                        {
                            if (Convert.ToString(row["AcctType"]) == accountType)
                            {
                                originalBalanceRow = row;
                                break;
                            }
                        }

                        finalRow = participantAfterSplitValuesTable.NewRow();
                        finalRow["AcctType"] = accountType;
                        finalRow["PersonalPreTax"] = originalBalanceRow["PersonalPreTax"];
                        finalRow["PersonalPostTax"] = originalBalanceRow["PersonalPostTax"];
                        finalRow["PersonalInterestBalance"] = originalBalanceRow["PersonalInterestBalance"];
                        finalRow["YMCAPreTax"] = originalBalanceRow["YmcaPreTax"];
                        finalRow["YMCAInterestBalance"] = originalBalanceRow["YmcaInterestBalance"];
                        finalRow["TotalTotal"] = originalBalanceRow["mnyBalance"];
                        finalRow["PlanType"] = originalBalanceRow["PlanType"]; //MMR | 2016.11.30 | YRS-AT-3145 | Assigning plan type to participant split table
                        participantAfterSplitValuesTable.Rows.Add(finalRow);
                    }
                    //END: PPP | 09/27/2016 | YRS-AT-2529 | Adding non splited account types also
                }
            }
            return participantAfterSplitValuesTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            originalBalanceRow = null;
            finalRow = null;
            rows = null;
            allAccountRows = null;
            participantAfterSplitValuesTable = null;
            participantAccountDetails = null;
        }
    }

    public static void RoundOffAmount(DataSet source)
    {
        foreach (DataTable table in source.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                row["PersonalPreTax"] = Math.Round(Convert.ToDecimal(row["PersonalPreTax"]), 2, MidpointRounding.AwayFromZero);
                row["PersonalPostTax"] = Math.Round(Convert.ToDecimal(row["PersonalPostTax"]), 2, MidpointRounding.AwayFromZero);
                row["YmcaPreTax"] = Math.Round(Convert.ToDecimal(row["YmcaPreTax"]), 2, MidpointRounding.AwayFromZero);
            }
        }
        source.AcceptChanges();
    }

    public static DataTable PrepareProposedSplitTable(DataSet originalBalance, List<string> selectedAccountTypes, decimal splitPercentage, decimal splitAmount)
    {
        DataTable proposedSplitTable;
        DataRow[] originalBalanceRows;
        DataRow proposedSplitRow;
        decimal totalOfProposedAccountBalance;
        decimal personPreTax, personPostTax, personInterest, ymcaPreTax, ymcaInterest, total;
        decimal differenceAmount;
        try
        {
            proposedSplitTable = originalBalance.Tables[0].Clone();
            totalOfProposedAccountBalance = 0;
            foreach (string selectedAccountType in selectedAccountTypes)
            {
                personPreTax = 0;
                personPostTax = 0;
                personInterest = 0;
                ymcaPreTax = 0;
                ymcaInterest = 0;
                total = 0;
                originalBalanceRows = originalBalance.Tables[0].Select(string.Format("AcctType='{0}'", selectedAccountType));
                if (originalBalanceRows.Length > 0)
                {
                    personPreTax = Math.Round(Convert.ToDecimal(originalBalanceRows[0]["PersonalPreTax"]) * splitPercentage, 2, MidpointRounding.AwayFromZero);
                    personPostTax = Math.Round(Convert.ToDecimal(originalBalanceRows[0]["PersonalPostTax"]) * splitPercentage, 2, MidpointRounding.AwayFromZero);
                    personInterest = Math.Round(Convert.ToDecimal(originalBalanceRows[0]["PersonalInterestBalance"]) * splitPercentage, 2, MidpointRounding.AwayFromZero);
                    ymcaPreTax = Math.Round(Convert.ToDecimal(originalBalanceRows[0]["YmcaPreTax"]) * splitPercentage, 2, MidpointRounding.AwayFromZero);
                    ymcaInterest = Math.Round(Convert.ToDecimal(originalBalanceRows[0]["YmcaInterestBalance"]) * splitPercentage, 2, MidpointRounding.AwayFromZero);
                    total = Math.Round(Convert.ToDecimal(originalBalanceRows[0]["mnyBalance"]) * splitPercentage, 2, MidpointRounding.AwayFromZero);
                    totalOfProposedAccountBalance += total;

                    if (total != (personPreTax + personPostTax + personInterest + ymcaPreTax + ymcaInterest))
                    {
                        differenceAmount = total - (personPreTax + personPostTax + personInterest + ymcaPreTax + ymcaInterest);

                        if (personPreTax > 0 && (personPreTax + differenceAmount) > 0)
                        {
                            personPreTax += differenceAmount;
                        }
                        else if (personPostTax > 0 && (personPostTax + differenceAmount) > 0)
                        {
                            personPostTax += differenceAmount;
                        }
                        else if (personInterest > 0 && (personInterest + differenceAmount) > 0)
                        {
                            personInterest += differenceAmount;
                        }
                        else if (ymcaPreTax > 0 && (ymcaPreTax + differenceAmount) > 0)
                        {
                            ymcaPreTax += differenceAmount;
                        }
                        else if (ymcaInterest > 0 && (ymcaInterest + differenceAmount) > 0)
                        {
                            ymcaInterest += differenceAmount;
                        }
                    }
                }
                proposedSplitRow = proposedSplitTable.NewRow();
                proposedSplitRow["AcctType"] = selectedAccountType;
                proposedSplitRow["PersonalPreTax"] = personPreTax;
                proposedSplitRow["PersonalPostTax"] = personPostTax;
                proposedSplitRow["PersonalInterestBalance"] = personInterest;
                proposedSplitRow["YmcaPreTax"] = ymcaPreTax;
                proposedSplitRow["YmcaInterestBalance"] = ymcaInterest;
                proposedSplitRow["mnyBalance"] = total;
                proposedSplitTable.Rows.Add(proposedSplitRow);
            }

            if (splitAmount != totalOfProposedAccountBalance)
            {
                differenceAmount = splitAmount - totalOfProposedAccountBalance;
                foreach (DataRow row in proposedSplitTable.Rows)
                {
                    personPreTax = Convert.ToDecimal(row["PersonalPreTax"]);
                    personPostTax = Convert.ToDecimal(row["PersonalPostTax"]);
                    personInterest = Convert.ToDecimal(row["PersonalInterestBalance"]);
                    ymcaPreTax = Convert.ToDecimal(row["YmcaPreTax"]);
                    ymcaInterest = Convert.ToDecimal(row["YmcaInterestBalance"]);

                    if (personPreTax > 0 && (personPreTax + differenceAmount) > 0)
                    {
                        personPreTax += differenceAmount;
                        row["PersonalPreTax"] = personPreTax;
                        break;
                    }
                    else if (personPostTax > 0 && (personPostTax + differenceAmount) > 0)
                    {
                        personPostTax += differenceAmount;
                        row["PersonalPostTax"] = personPostTax;
                        break;
                    }
                    else if (personInterest > 0 && (personInterest + differenceAmount) > 0)
                    {
                        personInterest += differenceAmount;
                        row["PersonalInterestBalance"] = personInterest;
                        break;
                    }
                    else if (ymcaPreTax > 0 && (ymcaPreTax + differenceAmount) > 0)
                    {
                        ymcaPreTax += differenceAmount;
                        row["YmcaPreTax"] = ymcaPreTax;
                        break;
                    }
                    else if (ymcaInterest > 0 && (ymcaInterest + differenceAmount) > 0)
                    {
                        ymcaInterest += differenceAmount;
                        row["YmcaInterestBalance"] = ymcaInterest;
                        break;
                    }
                }
            }

            return proposedSplitTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            proposedSplitRow = null;
            originalBalanceRows = null;
            proposedSplitTable = null;
        }
    }
    //END: PPP | 08/30/2016 | YRS-AT-2529

    //START: PPP | 09/13/2016 | YRS-AT-1973 
    public static bool IsAdjustmentValid(DataTable recipientSplitedValuesTable, DataTable recipientAdjustedValuesTable)
    {
        DataRow totalRow;
        decimal personPreTaxTotal, personPostTaxTotal, personInterestTotal, ymcaPreTaxTotal, ymcaInterestTotal, total;
        decimal personPreTaxAdjustedTotal, personPostTaxAdjustedTotal, personInterestAdjustedTotal, ymcaPreTaxAdjustedTotal, ymcaInterestAdjustedTotal, adjustedTotal;
        bool isAdjustmentValid;
        try
        {
            totalRow = GetAccountTotal(recipientSplitedValuesTable);
            personPreTaxTotal = Convert.ToDecimal(totalRow["PersonalPreTax"]);
            personPostTaxTotal = Convert.ToDecimal(totalRow["PersonalPostTax"]);
            personInterestTotal = Convert.ToDecimal(totalRow["PersonalInterestBalance"]);
            ymcaPreTaxTotal = Convert.ToDecimal(totalRow["YMCAPreTax"]);
            ymcaInterestTotal = Convert.ToDecimal(totalRow["YMCAInterestBalance"]);

            totalRow = GetAccountTotal(recipientAdjustedValuesTable);
            personPreTaxAdjustedTotal = Convert.ToDecimal(totalRow["PersonalPreTax"]);
            personPostTaxAdjustedTotal = Convert.ToDecimal(totalRow["PersonalPostTax"]);
            personInterestAdjustedTotal = Convert.ToDecimal(totalRow["PersonalInterestBalance"]);
            ymcaPreTaxAdjustedTotal = Convert.ToDecimal(totalRow["YMCAPreTax"]);
            ymcaInterestAdjustedTotal = Convert.ToDecimal(totalRow["YMCAInterestBalance"]);

            isAdjustmentValid = true;
            if (personPreTaxTotal != personPreTaxAdjustedTotal)
            {
                isAdjustmentValid = false;
            }
            else if (personPostTaxTotal != personPostTaxAdjustedTotal)
            {
                isAdjustmentValid = false;
            }
            else if (personInterestTotal != personInterestAdjustedTotal)
            {
                isAdjustmentValid = false;
            }
            else if (ymcaPreTaxTotal != ymcaPreTaxAdjustedTotal)
            {
                isAdjustmentValid = false;
            }
            else if (ymcaInterestTotal != ymcaInterestAdjustedTotal)
            {
                isAdjustmentValid = false;
            }
            else
            {
                for (int rowCounter = 0; rowCounter <= recipientSplitedValuesTable.Rows.Count - 1; rowCounter++)
                {
                    total = Convert.ToDecimal(recipientSplitedValuesTable.Rows[rowCounter]["TotalTotal"]);
                    adjustedTotal = Convert.ToDecimal(recipientAdjustedValuesTable.Rows[rowCounter]["TotalTotal"]);
                    if (total != adjustedTotal)
                    {
                        isAdjustmentValid = false;
                        break;
                    }
                }
            }

            return isAdjustmentValid;
        }
        catch
        {
            throw;
        }
        finally
        {
            totalRow = null;
        }
    }

    public static bool IsAdjustmentDoneAgainstPositiveValue(DataSet originalBalance, DataTable recipientAdjustedValuesTable)
    {
        DataRow[] recipientAdjustedRows;
        string accountType;
        decimal personPreTax, personPostTax, personInterest, ymcaPreTax, ymcaInterest;
        decimal personPreTaxAdjusted, personPostTaxAdjusted, personInterestAdjusted, ymcaPreTaxAdjusted, ymcaInterestAdjusted;
        bool isAdjustmentValid;
        try
        {
            isAdjustmentValid = false;

            if (originalBalance != null && originalBalance.Tables != null && originalBalance.Tables[0].Rows.Count > 0)
            {
                if (recipientAdjustedValuesTable != null && recipientAdjustedValuesTable.Rows.Count > 0)
                {
                    foreach (DataRow originalBalanceRow in originalBalance.Tables[0].Rows)
                    {
                        accountType = Convert.ToString(originalBalanceRow["AcctType"]);
                        recipientAdjustedRows = recipientAdjustedValuesTable.Select(string.Format("AcctType='{0}'", accountType));
                        if (recipientAdjustedRows.Length > 0)
                        {
                            personPreTax = Convert.ToDecimal(originalBalanceRow["PersonalPreTax"]);
                            personPostTax = Convert.ToDecimal(originalBalanceRow["PersonalPostTax"]);
                            personInterest = Convert.ToDecimal(originalBalanceRow["PersonalInterestBalance"]);
                            ymcaPreTax = Convert.ToDecimal(originalBalanceRow["YMCAPreTax"]);
                            ymcaInterest = Convert.ToDecimal(originalBalanceRow["YMCAInterestBalance"]);

                            personPreTaxAdjusted = Convert.ToDecimal(recipientAdjustedRows[0]["PersonalPreTax"]);
                            personPostTaxAdjusted = Convert.ToDecimal(recipientAdjustedRows[0]["PersonalPostTax"]);
                            personInterestAdjusted = Convert.ToDecimal(recipientAdjustedRows[0]["PersonalInterestBalance"]);
                            ymcaPreTaxAdjusted = Convert.ToDecimal(recipientAdjustedRows[0]["YMCAPreTax"]);
                            ymcaInterestAdjusted = Convert.ToDecimal(recipientAdjustedRows[0]["YMCAInterestBalance"]);

                            isAdjustmentValid = true;
                            if (personPreTax < personPreTaxAdjusted || personPreTaxAdjusted < 0)
                            {
                                isAdjustmentValid = false;
                            }
                            else if (personPostTax < personPostTaxAdjusted || personPostTaxAdjusted < 0)
                            {
                                isAdjustmentValid = false;
                            }
                            else if (personInterest < personInterestAdjusted || personInterestAdjusted < 0)
                            {
                                isAdjustmentValid = false;
                            }
                            else if (ymcaPreTax < ymcaPreTaxAdjusted || ymcaPreTaxAdjusted < 0)
                            {
                                isAdjustmentValid = false;
                            }
                            else if (ymcaInterest < ymcaInterestAdjusted || ymcaInterestAdjusted < 0)
                            {
                                isAdjustmentValid = false;
                            }

                            if (!isAdjustmentValid)
                                break;
                        }
                    }
                }
            }

            return isAdjustmentValid;
        }
        catch
        {
            throw;
        }
        finally
        {
            accountType = null;
            recipientAdjustedRows = null;
        }
    }

    public static bool IsAdjustmentLeavingPositiveValueInParticipantAccount(DataTable participantAfterSplitValuesTable, DataTable recipientAdjustedValuesTable)
    {
        DataRow[] participantRows;
        decimal personPreTaxTotal, personPostTaxTotal, personInterestTotal, ymcaPreTaxTotal, ymcaInterestTotal; //PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Removed ", total" which was not used by function
        decimal personPreTaxAdjustedTotal, personPostTaxAdjustedTotal, personInterestAdjustedTotal, ymcaPreTaxAdjustedTotal, ymcaInterestAdjustedTotal;
        bool isAdjustmentValid;

        string accountType;
        try
        {
            isAdjustmentValid = true;

            foreach (DataRow row in recipientAdjustedValuesTable.Rows)
            {
                accountType = Convert.ToString(row["AcctType"]);

                personPreTaxAdjustedTotal = Convert.ToDecimal(row["PersonalPreTax"]);
                personPostTaxAdjustedTotal = Convert.ToDecimal(row["PersonalPostTax"]);
                personInterestAdjustedTotal = Convert.ToDecimal(row["PersonalInterestBalance"]);
                ymcaPreTaxAdjustedTotal = Convert.ToDecimal(row["YMCAPreTax"]);
                ymcaInterestAdjustedTotal = Convert.ToDecimal(row["YMCAInterestBalance"]);

                if (personPreTaxAdjustedTotal != 0 || personPostTaxAdjustedTotal != 0 || personInterestAdjustedTotal != 0 || ymcaPreTaxAdjustedTotal != 0 || ymcaInterestAdjustedTotal != 0)
                {
                    participantRows = participantAfterSplitValuesTable.Select(string.Format("AcctType='{0}'", accountType));
                    if (participantRows.Length > 0)
                    {
                        personPreTaxTotal = Convert.ToDecimal(participantRows[0]["PersonalPreTax"]);
                        personPostTaxTotal = Convert.ToDecimal(participantRows[0]["PersonalPostTax"]);
                        personInterestTotal = Convert.ToDecimal(participantRows[0]["PersonalInterestBalance"]);
                        ymcaPreTaxTotal = Convert.ToDecimal(participantRows[0]["YMCAPreTax"]);
                        ymcaInterestTotal = Convert.ToDecimal(participantRows[0]["YMCAInterestBalance"]);

                        if ((personPreTaxTotal + personPreTaxAdjustedTotal) < 0)
                        {
                            isAdjustmentValid = false;
                            break;
                        }
                        else if ((personPostTaxTotal + personPostTaxAdjustedTotal) < 0)
                        {
                            isAdjustmentValid = false;
                            break;
                        }
                        else if ((personInterestTotal + personInterestAdjustedTotal) < 0)
                        {
                            isAdjustmentValid = false;
                            break;
                        }
                        else if ((ymcaPreTaxTotal + ymcaPreTaxAdjustedTotal) < 0)
                        {
                            isAdjustmentValid = false;
                            break;
                        }
                        else if ((ymcaInterestTotal + ymcaInterestAdjustedTotal) < 0)
                        {
                            isAdjustmentValid = false;
                            break;
                        }
                    }
                }
            }

            return isAdjustmentValid;
        }
        catch
        {
            throw;
        }
        finally
        {
            accountType = null;
            participantRows = null;
        }
    }

    private static DataRow GetAccountTotal(DataTable accountsTable)
    {
        DataRow totalRow;
        decimal personPreTaxTotal, personPostTaxTotal, ymcaPreTaxTotal;
        decimal personInterestTotal, ymcaInterestTotal;

        personPreTaxTotal = 0;
        personPostTaxTotal = 0;
        ymcaPreTaxTotal = 0;
        personInterestTotal = 0;
        ymcaInterestTotal = 0;
        foreach (DataRow accountRows in accountsTable.Rows)
        {
            personPreTaxTotal += Convert.ToDecimal(Convert.IsDBNull(accountRows["PersonalPreTax"]) ? 0 : accountRows["PersonalPreTax"]);
            personPostTaxTotal += Convert.ToDecimal(Convert.IsDBNull(accountRows["PersonalPostTax"]) ? 0 : accountRows["PersonalPostTax"]);
            personInterestTotal += Convert.ToDecimal(Convert.IsDBNull(accountRows["PersonalInterestBalance"]) ? 0 : accountRows["PersonalInterestBalance"]);

            ymcaPreTaxTotal += Convert.ToDecimal(Convert.IsDBNull(accountRows["YmcaPreTax"]) ? 0 : accountRows["YmcaPreTax"]);
            ymcaInterestTotal += Convert.ToDecimal(Convert.IsDBNull(accountRows["YMCAInterestBalance"]) ? 0 : accountRows["YMCAInterestBalance"]);
        }

        totalRow = accountsTable.NewRow();
        totalRow["AcctType"] = "Total";
        totalRow["PersonalPreTax"] = Math.Round(personPreTaxTotal, 2, MidpointRounding.AwayFromZero);
        totalRow["PersonalPostTax"] = Math.Round(personPostTaxTotal, 2, MidpointRounding.AwayFromZero);
        totalRow["PersonalInterestBalance"] = Math.Round(personInterestTotal, 2, MidpointRounding.AwayFromZero);
        totalRow["PersonalTotal"] = Math.Round(personPreTaxTotal + personPostTaxTotal + personInterestTotal, 2, MidpointRounding.AwayFromZero);
        totalRow["YMCAPreTax"] = Math.Round(ymcaPreTaxTotal, 2, MidpointRounding.AwayFromZero);
        totalRow["YMCAInterestBalance"] = Math.Round(ymcaInterestTotal, 2, MidpointRounding.AwayFromZero);
        totalRow["YMCATotal"] = Math.Round(ymcaPreTaxTotal + ymcaInterestTotal, 2, MidpointRounding.AwayFromZero);
        totalRow["TotalTotal"] = Math.Round(personPreTaxTotal + personPostTaxTotal + personInterestTotal + ymcaPreTaxTotal + ymcaInterestTotal, 2, MidpointRounding.AwayFromZero);

        return totalRow;
    }

    public static DataTable GetAdjustedAmountTable(DataTable recipientSplitedValuesTable, DataTable recipientAdjustedValuesTable)
    {
        DataTable differenceTable;
        DataRow splitedRow, adjustedRow, differenceRow;
        try
        {
            differenceTable = recipientSplitedValuesTable.Clone();
            for (int rowCounter = 0; rowCounter < recipientSplitedValuesTable.Rows.Count; rowCounter++)
            {
                splitedRow = recipientSplitedValuesTable.Rows[rowCounter];
                adjustedRow = recipientAdjustedValuesTable.Rows[rowCounter];

                differenceRow = differenceTable.NewRow();
                differenceRow["AcctType"] = Convert.ToString(splitedRow["AcctType"]);
                differenceRow["PersonalPreTax"] = Math.Round(Convert.ToDecimal(splitedRow["PersonalPreTax"]) - Convert.ToDecimal(adjustedRow["PersonalPreTax"]), 2, MidpointRounding.AwayFromZero);
                differenceRow["PersonalPostTax"] = Math.Round(Convert.ToDecimal(splitedRow["PersonalPostTax"]) - Convert.ToDecimal(adjustedRow["PersonalPostTax"]), 2, MidpointRounding.AwayFromZero);
                differenceRow["PersonalInterestBalance"] = Math.Round(Convert.ToDecimal(splitedRow["PersonalInterestBalance"]) - Convert.ToDecimal(adjustedRow["PersonalInterestBalance"]), 2, MidpointRounding.AwayFromZero);
                differenceRow["YMCAPreTax"] = Math.Round(Convert.ToDecimal(splitedRow["YMCAPreTax"]) - Convert.ToDecimal(adjustedRow["YMCAPreTax"]), 2, MidpointRounding.AwayFromZero);
                differenceRow["YMCAInterestBalance"] = Math.Round(Convert.ToDecimal(splitedRow["YMCAInterestBalance"]) - Convert.ToDecimal(adjustedRow["YMCAInterestBalance"]), 2, MidpointRounding.AwayFromZero);
                differenceTable.Rows.Add(differenceRow);
            }

            return differenceTable;
        }
        catch
        {
            throw;
        }
        finally
        {
            splitedRow = null;
            adjustedRow = null;
            differenceRow = null;
            differenceTable = null;
        }
    }

    public static void AdjustAccountBalances(bool isParticipantDetails, DataTable sourceTable, DataTable adjustedTable, string recipientPersID, string principalTransactionType, string interestTransactionType)
    {
        string accountType;
        decimal personPreTax, personPostTax, personInterest;
        decimal ymcaPreTax, ymcaInterest;
        try
        {
            for (int rowCounter = 0; rowCounter < adjustedTable.Rows.Count; rowCounter++)
            {
                accountType = Convert.ToString(adjustedTable.Rows[rowCounter]["AcctType"]);
                personPreTax = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["PersonalPreTax"]);
                personPostTax = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["PersonalPostTax"]);
                personInterest = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["PersonalInterestBalance"]);
                ymcaPreTax = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["YMCAPreTax"]);
                ymcaInterest = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["YMCAInterestBalance"]);

                if (personPreTax != 0 || personPostTax != 0 || personInterest != 0 || ymcaPreTax != 0 || ymcaInterest != 0)
                {
                    AdjustAmount(isParticipantDetails, sourceTable, recipientPersID, accountType, principalTransactionType, personPreTax, true, false, false);
                    AdjustAmount(isParticipantDetails, sourceTable, recipientPersID, accountType, principalTransactionType, personPostTax, false, true, false);
                    AdjustAmount(isParticipantDetails, sourceTable, recipientPersID, accountType, principalTransactionType, ymcaPreTax, false, false, true);

                    AdjustAmount(isParticipantDetails, sourceTable, recipientPersID, accountType, interestTransactionType, personInterest, true, false, false);
                    AdjustAmount(isParticipantDetails, sourceTable, recipientPersID, accountType, interestTransactionType, ymcaInterest, false, false, true);
                }
            }

            DeleteZeroAmountRows(sourceTable);
        }
        catch
        {
            throw;
        }
        finally
        {
            accountType = null;
        }
    }

    // For A and B transaction adjustments
    public static void AdjustAccountBalances(bool isParticipantDetails, DataTable sourceATable, DataTable sourceBTable, DataTable adjustedTable, string recipientPersID, string principalTransactionType, string interestTransactionType)
    {
        string accountType;
        decimal personPreTax, personPostTax, personInterest;
        decimal ymcaPreTax, ymcaInterest;
        try
        {
            for (int rowCounter = 0; rowCounter < adjustedTable.Rows.Count; rowCounter++)
            {
                accountType = Convert.ToString(adjustedTable.Rows[rowCounter]["AcctType"]);
                personPreTax = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["PersonalPreTax"]);
                personPostTax = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["PersonalPostTax"]);
                personInterest = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["PersonalInterestBalance"]);
                ymcaPreTax = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["YMCAPreTax"]);
                ymcaInterest = Convert.ToDecimal(adjustedTable.Rows[rowCounter]["YMCAInterestBalance"]);

                if (personPreTax != 0 || personPostTax != 0 || personInterest != 0 || ymcaPreTax != 0 || ymcaInterest != 0)
                {
                    AdjustAccountBalances(isParticipantDetails, sourceATable, sourceBTable, recipientPersID, accountType, principalTransactionType, personPreTax, true, false, false);
                    AdjustAccountBalances(isParticipantDetails, sourceATable, sourceBTable, recipientPersID, accountType, principalTransactionType, personPostTax, false, true, false);
                    AdjustAccountBalances(isParticipantDetails, sourceATable, sourceBTable, recipientPersID, accountType, principalTransactionType, ymcaPreTax, false, false, true);

                    AdjustAccountBalances(isParticipantDetails, sourceATable, sourceBTable, recipientPersID, accountType, interestTransactionType, personInterest, true, false, false);
                    AdjustAccountBalances(isParticipantDetails, sourceATable, sourceBTable, recipientPersID, accountType, interestTransactionType, ymcaInterest, false, false, true);
                }
            }
            DeleteZeroAmountRows(sourceATable);
            DeleteZeroAmountRows(sourceBTable);
        }
        catch
        {
            throw;
        }
        finally
        {
            accountType = null;
        }
    }

    private static void AdjustAccountBalances(bool isParticipantDetails, DataTable sourceATable, DataTable sourceBTable, string recipientPersID, string accountType, string transactionType, decimal adjustmentAmount, bool isPersonalPreTax, bool isPersonalPostTax, bool isYmcaPreTax)
    {
        if (adjustmentAmount != 0)
        {
            adjustmentAmount = AdjustAmount(isParticipantDetails, sourceATable, recipientPersID, accountType, transactionType, adjustmentAmount, isPersonalPreTax, isPersonalPostTax, isYmcaPreTax);
        }

        if (adjustmentAmount != 0)
        {
            AdjustAmount(isParticipantDetails, sourceBTable, recipientPersID, accountType, transactionType, adjustmentAmount, isPersonalPreTax, isPersonalPostTax, isYmcaPreTax);
        }
    }

    private static decimal AdjustAmount(bool isParticipantDetails, DataTable sourceTable, string recipientPersID, string accountType, string transactionType, decimal adjustmentAmount, bool isPersonalPreTax, bool isPersonalPostTax, bool isYmcaPreTax)
    {
        DataRow[] accountRows;
        Decimal originalAmount;

        if (adjustmentAmount != 0)
        {
            originalAmount = 0;
            if (isParticipantDetails)
            {
                accountRows = sourceTable.Select(string.Format("BenfitPersId='{0}' and AcctType='{1}' and TransactType='{2}'", recipientPersID, accountType, transactionType));
            }
            else
            {
                accountRows = sourceTable.Select(string.Format("PersID='{0}' and AcctType='{1}' and TransactType='{2}'", recipientPersID, accountType, transactionType));
            }
            foreach (DataRow row in accountRows)
            {
                if (isPersonalPreTax)
                    originalAmount = Convert.ToDecimal(row["PersonalPreTax"]);
                else if (isPersonalPostTax)
                    originalAmount = Convert.ToDecimal(row["PersonalPostTax"]);
                else if (isYmcaPreTax)
                    originalAmount = Convert.ToDecimal(row["YmcaPreTax"]);

                if (originalAmount != 0)
                {
                    if (isParticipantDetails)
                    {
                        originalAmount = System.Math.Abs(originalAmount); // convert negative value to positive
                    }

                    if (adjustmentAmount > originalAmount)
                    {
                        adjustmentAmount -= originalAmount;
                        originalAmount = 0;
                    }
                    else
                    {
                        originalAmount -= adjustmentAmount;
                        adjustmentAmount = 0;
                    }

                    if (isParticipantDetails)
                        originalAmount = Math.Round(originalAmount * -1, 2, MidpointRounding.AwayFromZero); // convert back positive value to negative
                    else
                        originalAmount = Math.Round(originalAmount, 2, MidpointRounding.AwayFromZero);

                    if (isPersonalPreTax)
                        row["PersonalPreTax"] = originalAmount;
                    else if (isPersonalPostTax)
                        row["PersonalPostTax"] = originalAmount;
                    else if (isYmcaPreTax)
                        row["YmcaPreTax"] = originalAmount;

                    if (adjustmentAmount == 0)
                        break;
                }
            }
            sourceTable.AcceptChanges();
        }
        return adjustmentAmount;
    }

    private static void DeleteZeroAmountRows(DataTable sourceTable)
    {
        DataRow[] accountRows;
        bool areRowsDeleted;
        try
        {
            areRowsDeleted = false;
            accountRows = sourceTable.Select("PersonalPreTax=0 and PersonalPostTax=0 and YmcaPreTax=0");
            foreach (DataRow row in accountRows)
            {
                row.Delete();
                areRowsDeleted = true;
            }
            if (areRowsDeleted)
                sourceTable.AcceptChanges();
        }
        catch
        {
            throw;
        }
        finally
        {
            accountRows = null;
        }
    }
    //END: PPP | 09/13/2016 | YRS-AT-1973 

    //START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 
    // Provides all recipient details saved for given request id
    public static DataSet GetRecipientDetails(string requestID)
    {
        return NonRetiredQDRODAClass.GetRecipientDetails(requestID);
    }

    // Performs create, update and delete operation on recipient details
    public static void MaintainRecipientDetails(string recipientDetails, string requestID, string action)
    {
        NonRetiredQDRODAClass.MaintainRecipientDetails(recipientDetails,requestID,action );
    }
    //END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 
    }
	
}
