// Change history
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date		Description
//*******************************************************************************
// NP		                2007.09.03	Coded to handle exit code properly from the Check_If_Settled procedure.
// NP		                2007.09.05	Coded to allow searches by Fund No
// NP		                2008.04.11	Changes related to Phase IV - Part1
//						                Added code to allow forcing of compuations for non-retired fundevents as either DA/DI
// NP		                2008.07.09	Adding call to stored procedure to identify basic account balance.
// NP		                2008.08.12	IVP2 - Adding code to recompute options if we have settled and unsetted beneficiaries and funds have been split for the beneficiaries
// SR                       2010.01.06  Added function to determine the Market Based Amount.
// SR                       2012.11.26  YRS 5.0-1707:Adding function to find no. of Payroll month since death.
// SR                       2012.11.26  YRS 5.0-1707:New Death Benefit Application form 
// AA                       2012.12.04  YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up
// SR                       2012.12.14  YRS 5.0-1707:To get the JS annuity details
// SR                       2013.08.20  YRS 5.0-2185: YRS 5.0-2185 : Option C annuity Amount not on Death bene form 
// AA                       2014.08.12  BT:2460:YRS 5.0-2331 YRS 5.0-2331 - Death Benefit Application Form 
// SP                       2014.12.02  BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
// Manthan Rajguru          2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep Adusumilli        2015.10.12  YRS-AT-2478 - Death Benefit Application to show taxable and non-taxable amounts(TrackIT 21695)
//Sanjay Rawat       		2015.12.04  YRS-AT-2718 - YRS enh: Annuity Estimate - change for Retired Death Benefit, use new effective date 1/1/2019 for calculations 
//Chandra Sekar             2016.08.05  YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
//*******************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
//Change By:Preeti Pre-Retirement Death . Changed procedure name.
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DeathBenefitsCalculatorDAClass.
	/// </summary>
	public sealed class DeathBenefitsCalculatorDAClass
	{
		public DeathBenefitsCalculatorDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		//NP:PS:2007.09.05 - Added code to allow searches by Fund Number
		public static DataSet LookUp_DeathCalc_MemberListForDeath(string parameterSSN, string parameterLName ,string parameterFName, string parameterFundNo)
		{
			DataSet l_dataset_DeathCalc_LookUp_MemberListForDeath = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeathCalc_LookUp_MemberListForDeath");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_SSN",DbType.String,parameterSSN);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_LName",DbType.String,parameterLName);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_FName",DbType.String,parameterFName);				
				db.AddInParameter(LookUpCommandWrapper, "@varchar_FundNo",DbType.String,parameterFundNo);	//NP:PS:2007.09.05 - Adding FundNo parameter to procedure to allow searches on it
			
				l_dataset_DeathCalc_LookUp_MemberListForDeath = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_DeathCalc_LookUp_MemberListForDeath,"r_MemberListForDeath");
				return l_dataset_DeathCalc_LookUp_MemberListForDeath;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		private static DataSet DeathCalc_Check_If_Settled(string parameterPersId, string parameterFundEventID, DateTime parameterDeathDate, string parameterFundStatus, out int l_int_returnStatus )
		{
			//Vipul 18thSept06: Gemini # YREN-2629. The Death Options for participant already settled also need to show up in new post 01July06 Format
			//Vipul 18thSept06: Added the parameter parameterDeathDate

			//We need to check whether the participant chosen for Death Calculation is already "SETTLED" or not
			//IF Settled, Give a appropriate message to the user & display the details ... There is a bug in Foxpro Related to this
			DataSet l_dataset_DeathCalc_SettledData = null;
			Database db = null;
			DbCommand commandDeathCalculator = null;
			l_int_returnStatus = 1 ;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;

				commandDeathCalculator = db.GetStoredProcCommand("yrs_usp_DeathCalc_Check_If_Settled");
				
				if (commandDeathCalculator == null) return null;
                
				db.AddInParameter(commandDeathCalculator, "@varchar_lcPersID",DbType.String,parameterPersId);
                db.AddInParameter(commandDeathCalculator, "@Varchar_FundEventID", DbType.String, parameterFundEventID);
				//NP:PS:2007.09.03 - Adding parameter to force calculations for one fund status in case of RA/RT/DD persons
                db.AddInParameter(commandDeathCalculator, "@varchar_FundStatus", DbType.String, parameterFundStatus);
				//Vipul 18thSept06: Gemini # YREN-2629. The Death Options for participant already settled also need to show up in new post 01July06 Format
                db.AddInParameter(commandDeathCalculator, "@datetime_DeathDate", DbType.DateTime, parameterDeathDate);
				//Vipul 18thSept06: Gemini # YREN-2629. The Death Options for participant already settled also need to show up in new post 01July06 Format

                db.AddOutParameter(commandDeathCalculator, "@int_ReturnStatus", DbType.Int32, 9);
				commandDeathCalculator.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 


				l_dataset_DeathCalc_SettledData = new DataSet() ;
				db.LoadDataSet(commandDeathCalculator, l_dataset_DeathCalc_SettledData,"r_SettledData[]");

				//NP:PS:2007.09.03 - Removing check to obtain the actual return code from the procedure
				//				if (l_dataset_DeathCalc_SettledData.Tables[0].Rows.Count == 0)
				//				{
				//					l_int_returnStatus = -2 ;
				//				}
				
				if (l_int_returnStatus >= 0)
				{
                    //TODOMIGRATION - Check if convert to Int32 would work here
					//l_int_returnStatus = Convert.ToInt32(commandDeathCalculator.GetParameterValue("@int_ReturnStatus"));;
                    l_int_returnStatus = Convert.ToInt32(db.GetParameterValue(commandDeathCalculator, "@int_ReturnStatus"));
				}
				
				return l_dataset_DeathCalc_SettledData;
		
			}
			catch (Exception ex)
			{
				throw ex;
			}
	
		}
		//NP:IVP1:2008.04.11 - Adding parameter ForceCalculationsAs to the list to enable calculations as either DA or DI for non-retired fundevent statuses
		//	Valid values for parameterForceCalculationsAs are:
		//		DA: To consider as Active Death - (applied $10k death benefit)
		//		DI:	To consider as Inaction Death - (does not give $10k death benefit)
		//		Null or Empty: To use the default logic for computations
		public static DataSet Calculate_Death_Benefits(string parameterPersId, string parameterFundEventID , DateTime parameterDeathDate, string parameterFundStatus, string parameterForceCalculationsAs, out int l_int_returnStatus)
		{
			Database db=null;
			DbCommand commandDeathCalculationProcessing = null;
			DataSet l_dataset_DeathCalc_ProcessedData= null;
			DataSet l_dataset_DeathCalc_SettledData = null;
			l_int_returnStatus = 0;
		
			try 
			{
				//Vipul 18thSept06 Gemini # YREN-2629. The Death Options for participant already settled also need to show up in new post 01July06 Format
				//l_dataset_DeathCalc_SettledData = DeathCalc_Check_If_Settled( parameterPersId, parameterFundEventID, out l_int_returnStatus );
				l_dataset_DeathCalc_SettledData = DeathCalc_Check_If_Settled( parameterPersId, parameterFundEventID, parameterDeathDate, parameterFundStatus, out l_int_returnStatus );
				//Vipul 18thSept06 Gemini # YREN-2629. The Death Options for participant already settled also need to show up in new post 01July06 Format
				//NP:IVP2:2008.08.12 - Not returning existing computations if return status is -10. We need to recompute values in this case as -10 represents a scenario where some beneficiaries are setted and some are 
				//	unsettled and the funds for the unsettled beenficiaries are split into beneficiary accounts.
				if (l_int_returnStatus < 0 && l_int_returnStatus != -10)
				{
					return l_dataset_DeathCalc_SettledData; 
				}
	
				db=DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				//Change By:Preeti Pre-Retirement Death . Changed procedure name.
				//Changes have been  incorporated in the original procedure naming "yrs_usp_DeathCalc_CalcOptions" after approval from the client.
				// Check with the death date, if death date is >= 07/01/2006 then call new proc else called old proc
				//parameterDeathDate=null;
				commandDeathCalculationProcessing= db.GetStoredProcCommand("yrs_usp_DeathCalc_CalcOptions");
				commandDeathCalculationProcessing.CommandTimeout=1200;
				if(commandDeathCalculationProcessing==null) return null;
				db.AddInParameter(commandDeathCalculationProcessing, "@varchar_lcPersID",DbType.String,parameterPersId);
                db.AddInParameter(commandDeathCalculationProcessing, "@Varchar_FundEventID", DbType.String, parameterFundEventID);
                db.AddInParameter(commandDeathCalculationProcessing, "@datetime_ldDeathDate", DbType.DateTime, parameterDeathDate);
				//NP:PS:2007.09.03 - Adding parameter to force calculations for one fund status in case of RA/RT/DD persons
                db.AddInParameter(commandDeathCalculationProcessing, "@varchar_FundStatus", DbType.String, parameterFundStatus);
				//NP:IVP1:2008.04.11 - Adding parameter to force calculations as DA or DI for non-retired fundevents
                db.AddInParameter(commandDeathCalculationProcessing, "@varchar_ForceCalculationsAs", DbType.String, parameterForceCalculationsAs);
                db.AddOutParameter(commandDeathCalculationProcessing, "@int_ReturnStatus", DbType.Int16, 4); 
						
				l_dataset_DeathCalc_ProcessedData=new DataSet();
				db.LoadDataSet(commandDeathCalculationProcessing,l_dataset_DeathCalc_ProcessedData,"r_DeathCalc_ProcessedData[]");
					
				return l_dataset_DeathCalc_ProcessedData;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		

		public static string GetAnnuities(string parameterFundEventId) 
		{

			if (parameterFundEventId == null) return string.Empty;
			if (parameterFundEventId == string.Empty) return string.Empty;

			DataSet l_dataset_DeathCalc_Annuities = null;
			Database db = null;
			DbCommand commandDeathCalculator = null;
			string l_string_returnValue = string.Empty;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;

				commandDeathCalculator = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetAnnuitiesByFundId");
				
				if (commandDeathCalculator == null) return null;
                
				db.AddInParameter(commandDeathCalculator, "@varchar_FundEventID", DbType.String, parameterFundEventId);
				commandDeathCalculator.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 


				l_dataset_DeathCalc_Annuities = new DataSet() ;
				db.LoadDataSet(commandDeathCalculator, l_dataset_DeathCalc_Annuities, "AnnuityTypes");
				if (l_dataset_DeathCalc_Annuities.Tables[0].Rows.Count == 0)
				{
					return string.Empty;
				}
				
				foreach (DataRow dr in l_dataset_DeathCalc_Annuities.Tables[0].Rows )
				{
					l_string_returnValue += (l_string_returnValue == string.Empty? string.Empty : ", ") 
												+ dr[0].ToString().Trim() ;
				}
				return l_string_returnValue;
			
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	
		public static decimal GetBasicAccountBalance(string parameterFundEventId) 
		{
			if (parameterFundEventId == null) return 0;
			if (parameterFundEventId == string.Empty) return 0;

			Database db = null;
			DbCommand commandDeathCalculator = null;
			string l_string_ErrorString = string.Empty;
			decimal l_decimal_BasicAccountBalance = 0.0M;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) throw new Exception("Unable to create Database connection");

				commandDeathCalculator = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetBasicAccountBalances");
				
				if (commandDeathCalculator == null) throw new Exception("Unable to access Stored Procedure");
                
				commandDeathCalculator.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Add input parameters
				db.AddInParameter(commandDeathCalculator, "@guiFundEventID", DbType.String, parameterFundEventId);
				//Add output parameters
				db.AddOutParameter(commandDeathCalculator, "@BasicAccountBalance", DbType.Decimal, 20);
				db.AddOutParameter(commandDeathCalculator, "@coutput", DbType.String, 1000);
				//Execute the command
				db.ExecuteNonQuery(commandDeathCalculator);
				//Get the output values
				//l_decimal_BasicAccountBalance  = Convert.ToDecimal(commandDeathCalculator.GetParameterValue("@BasicAccountBalance"));
				//l_string_ErrorString = Convert.ToString(commandDeathCalculator.GetParameterValue("@coutput"));
                //TODOMIGRATION - Check if convert to decimal works properly like this
                //Start:Added by Anudeep:08.12.2012 to sets initial value if it null and throws error
                if (db.GetParameterValue(commandDeathCalculator, "@BasicAccountBalance") == DBNull.Value)
                {
                    l_decimal_BasicAccountBalance = 0.0M;
                }
                //End:Added by Anudeep:08.12.2012 to sets initial value if it null and throws error
                else
                {
                    l_decimal_BasicAccountBalance = Convert.ToDecimal(db.GetParameterValue(commandDeathCalculator, "@BasicAccountBalance"));
                }
                
                l_string_ErrorString = db.GetParameterValue(commandDeathCalculator, "@coutput").ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			//If error string contains a value then we faced some sort of error. Throw an exception. Else return the basic account balance.
			if (l_string_ErrorString != string.Empty) {
				throw new Exception(l_string_ErrorString);
			} else {
				return l_decimal_BasicAccountBalance;
			}
		}

		//SR:2010.01.06 - Adding function to determine the Market Based Amount.
		public static decimal GetMarketBasedAmount(string parameterFundEventId,string ParameterFundStatus) 
		{
			if (parameterFundEventId == null) return 0;
			if (parameterFundEventId == string.Empty) return 0;

			Database db = null;
			DbCommand commandDeathCalculator = null;
			string l_string_ErrorString = string.Empty;
			decimal l_decimal_MarketBasedAmount = 0.0M;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) throw new Exception("Unable to create Database connection");

				commandDeathCalculator = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetMarketBasedAmount");
				
				if (commandDeathCalculator == null) throw new Exception("Unable to access Stored Procedure");
                
				commandDeathCalculator.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Add input parameters
				db.AddInParameter(commandDeathCalculator, "@guiFundEventID", DbType.String, parameterFundEventId);
				db.AddInParameter(commandDeathCalculator, "@chrStatusType", DbType.String, ParameterFundStatus);
				//Add output parameters
				db.AddOutParameter(commandDeathCalculator, "@MarketBasedAmount", DbType.Decimal, 20);
				db.AddOutParameter(commandDeathCalculator, "@coutput", DbType.String, 1000);
				//Execute the command
				db.ExecuteNonQuery(commandDeathCalculator);
				//Get the output values
				//l_decimal_MarketBasedAmount  = Convert.ToDecimal(commandDeathCalculator.GetParameterValue("@MarketBasedAmount"));
				//l_string_ErrorString = Convert.ToString(commandDeathCalculator.GetParameterValue("@coutput"));
                //TODOMIGRATION - Check if convert to decimal works or not
                l_decimal_MarketBasedAmount = Convert.ToDecimal(db.GetParameterValue(commandDeathCalculator, "@MarketBasedAmount"));
                l_string_ErrorString = db.GetParameterValue(commandDeathCalculator, "@coutput").ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			//If error string contains a value then we faced some sort of error. Throw an exception. Else return the basic account balance.
			if (l_string_ErrorString != string.Empty) 
			{
				throw new Exception(l_string_ErrorString);
			} 
			else 
			{
				return l_decimal_MarketBasedAmount;
			}
		}

        // SR:2012.11.26  YRS 5.0-1707:Adding function to find no. of Payroll month since death.
        public static string GetPayrollMonthsSinceDeath(DateTime parameterDeathDate)
        {
            DbCommand commandGetPayrollMonths = null;
            Database db = null;
            string strMonths = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandGetPayrollMonths = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetPayrollMonthSinceDeath");
                if (commandGetPayrollMonths == null) return null;
                db.AddInParameter(commandGetPayrollMonths, "@datetime_idjsdeathdate", DbType.DateTime, parameterDeathDate);
                db.AddOutParameter(commandGetPayrollMonths, "@numeric_Months", DbType.Int32, 32);
                db.ExecuteNonQuery(commandGetPayrollMonths);
                strMonths = Convert.ToString(db.GetParameterValue(commandGetPayrollMonths, "@numeric_Months"));
                return strMonths;
            }
            catch
            {
                throw;
            }
            finally
            {
                commandGetPayrollMonths = null;
                db = null;
                strMonths = string.Empty;
            }
        }


        // SR:2012.11.26 - YRS 5.0-1707:New Death Benefit Application form 
        //SP 2014.12.04 BT-2310\YRS 5.0-2255: Added above parameters into below method "strRepFirstName, strRepLastName, strRepSalutation &strRepTelephone"
        public static string SaveDeathBenefitCalculatorFormDetails(string strdecsdPersID,
                                                                   string strdecsdFundeventID,                                                                 
                                                                    string StrBeneficiaryName,                                                                
                                                                    decimal decJSAnnuityAmount,
                                                                    decimal decRetPlan,
                                                                    decimal decPrincipalGuaranteeAnnuity_RP,
                                                                    decimal decSavPlan,
                                                                    decimal decPrincipalGuaranteeAnnuity_SP,
                                                                    decimal decRetiredDeathBenefit,
                                                                    decimal decAnnuityMFromRP,
                                                                    decimal decFirstMAnnuityFromRP,
                                                                    decimal decAnnuityCFromRP,
                                                                    decimal decFirstCAnnuityFromRP,
                                                                    decimal decLumpSumFromNonHumanBen,
                                                                    decimal decAnnuityMFromSP,
                                                                    decimal decFirstMAnnuityFromSP,
                                                                    decimal decAnnuityCFromSP,
                                                                    decimal decFirstCAnnuityFromSP,
                                                                    decimal decAnnuityMFromRDB,
                                                                    decimal decFirstMAnnuityFromRDB,
                                                                    decimal decAnnuityFromJSAndRDB,
                                                                    decimal decFirstAnnuityFromJSAndRDB,
                                                                    decimal decAnnuityMFromResRemainingOfRP,
                                                                    decimal decFirstMAnnuityFromResRemainingOfRP,
                                                                    decimal decAnnuityCFromResRemainingOfRP,
                                                                    decimal decFirstCAnnuityFromResRemainingOfRP,
                                                                    decimal decAnnuityMFromResRemainingOfSP,
                                                                    decimal decFirstMAnnuityFromResRemainingOfSP,
                                                                    decimal decAnnuityCFromResRemainingOfSP,
                                                                    decimal decFirstCAnnuityFromResRemainingOfSP,
                                                                    int intMonths,
                                                                    bool blnActiveDeathBenfit,
                                                                    bool blnSection1Visible,
                                                                    bool blnSection2Visible,
                                                                    bool blnSection3Visible,
                                                                    bool blnSection4Visible,
                                                                    bool blnSection5Visible,
                                                                    bool blnSection6Visible,
                                                                    bool blnSection7Visible,
                                                                    bool blnSection8Visible,
                                                                    bool blnSection9Visible,
                                                                    bool blnSection10Visible,
                                                                    bool blnSection11Visible,
                                                                    bool blnSection12Visible,
                                                                    bool blnCopyIDM,
                                                                    bool blnFollowUp,
                                                                    string strAddressID,
                                                                    string StrBeneficiaryFirstName,
                                                                    string StrBeneficiaryLastName,
                                                                    string StrBeneficiarySSNo
                                                                    , string strRepFirstName
                                                                    , string strRepLastName
                                                                    , string strRepSalutation
                                                                    , string strRepTelephone,
                                                                    decimal decRetTaxable,
                                                                    decimal decRetNonTaxable,
                                                                    decimal decSavTaxable,
                                                                    decimal decSavNonTaxable,
                                                                     //START - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                                                                    decimal decPrincipalGuaranteeAnnuityRetTaxable,
                                                                    decimal decPrincipalGuaranteeAnnuityRetNonTaxable,
                                                                    decimal decPrincipalGuaranteeAnnuitySavTaxable,
                                                                    decimal decPrincipalGuaranteeAnnuitySavNonTaxable
                                                                    //End - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                                                                    )
        {
            //AA 2015.12.10 YRS-AT-2478 Added to store taxable and non taxable money

            DbCommand commandDeatBenfitForm = null;
            Database db = null;
            string strReturnStatus = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandDeatBenfitForm = db.GetStoredProcCommand("yrs_usp_DeathCalc_DeathBenefitformOptions");
                if (commandDeatBenfitForm == null) return null;
                db.AddInParameter(commandDeatBenfitForm, "@guiDecsdPersID", DbType.String, strdecsdPersID);
                db.AddInParameter(commandDeatBenfitForm, "@guiDecsdFundEventID", DbType.String, strdecsdFundeventID);  
                db.AddInParameter(commandDeatBenfitForm, "@chvBeneficiaryName", DbType.String, StrBeneficiaryName);      
                db.AddInParameter(commandDeatBenfitForm, "@mnyJSAnnuities", DbType.Decimal, decJSAnnuityAmount);
                db.AddInParameter(commandDeatBenfitForm, "@mnyRetPlan", DbType.Decimal, decRetPlan);
                db.AddInParameter(commandDeatBenfitForm, "@mnyPrincipalGuaranteeAnnuity_RP", DbType.Decimal, decPrincipalGuaranteeAnnuity_RP);
                db.AddInParameter(commandDeatBenfitForm, "@mnySavPlan", DbType.Decimal, decSavPlan);
                db.AddInParameter(commandDeatBenfitForm, "@mnyPrincipalGuaranteeAnnuity_SP", DbType.Decimal, decPrincipalGuaranteeAnnuity_SP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyRetiredDeathBenefit", DbType.Decimal, decRetiredDeathBenefit);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityMFromRP", DbType.Decimal, decAnnuityMFromRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstMAnnuityFromRP", DbType.Decimal, decFirstMAnnuityFromRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityCFromRP", DbType.Decimal, decAnnuityCFromRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstCAnnuityFromRP", DbType.Decimal, decFirstCAnnuityFromRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyLumpSumFromNonHumanBen", DbType.Decimal, decLumpSumFromNonHumanBen);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityMFromSP", DbType.Decimal, decAnnuityMFromSP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstMAnnuityFromSP", DbType.Decimal, decFirstMAnnuityFromSP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityCFromSP", DbType.Decimal, decAnnuityCFromSP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstCAnnuityFromSP", DbType.Decimal, decFirstCAnnuityFromSP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityMFromRDB", DbType.Decimal, decAnnuityMFromRDB);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstMAnnuityFromRDB", DbType.Decimal, decFirstMAnnuityFromRDB);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityFromJSAndRDB", DbType.Decimal, decAnnuityFromJSAndRDB);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstAnnuityFromJSAndRDB", DbType.Decimal, decFirstAnnuityFromJSAndRDB);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityMFromResRemainingOfRP", DbType.Decimal, decAnnuityMFromResRemainingOfRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstMAnnuityFromResRemainingOfRP", DbType.Decimal, decFirstMAnnuityFromResRemainingOfRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityCFromResRemainingOfRP", DbType.Decimal, decAnnuityCFromResRemainingOfRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstCAnnuityFromResRemainingOfRP", DbType.Decimal, decFirstCAnnuityFromResRemainingOfRP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityMFromResRemainingOfSP", DbType.Decimal, decAnnuityMFromResRemainingOfSP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstMAnnuityFromResRemainingOfSP", DbType.Decimal, decFirstMAnnuityFromResRemainingOfSP);
                db.AddInParameter(commandDeatBenfitForm, "@mnyAnnuityCFromResRemainingOfSP", DbType.Decimal, decAnnuityCFromResRemainingOfSP); // SR:2013.08.20 - BT 2185/YRS 5.0-2185 - Parameter value changed from First C annuity value to  Annuity C value.
                db.AddInParameter(commandDeatBenfitForm, "@mnyFirstCAnnuityFromResRemainingOfSP", DbType.Decimal, decFirstCAnnuityFromResRemainingOfSP);         
                db.AddInParameter(commandDeatBenfitForm, "@intMonths", DbType.Int16 , intMonths);
                db.AddInParameter(commandDeatBenfitForm, "@bitActiveDeathBenefit", DbType.Boolean, blnActiveDeathBenfit);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection1Visible", DbType.Boolean, blnSection1Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection2Visible", DbType.Boolean, blnSection2Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection3Visible", DbType.Boolean, blnSection3Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection4Visible", DbType.Boolean, blnSection4Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection5Visible", DbType.Boolean, blnSection5Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection6Visible", DbType.Boolean, blnSection6Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection7Visible", DbType.Boolean, blnSection7Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection8Visible", DbType.Boolean, blnSection8Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection9Visible", DbType.Boolean, blnSection9Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection10Visible", DbType.Boolean, blnSection10Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection11Visible", DbType.Boolean, blnSection11Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitSection12Visible", DbType.Boolean, blnSection12Visible);
                db.AddInParameter(commandDeatBenfitForm, "@bitCopyIDM", DbType.Boolean, blnCopyIDM);
                db.AddInParameter(commandDeatBenfitForm, "@bitFollowUp", DbType.Boolean, blnFollowUp);
                db.AddInParameter(commandDeatBenfitForm, "@guiAddressID", DbType.String, strAddressID);
                db.AddInParameter(commandDeatBenfitForm, "@chvBeneficiaryFirstName", DbType.String, StrBeneficiaryFirstName);
                db.AddInParameter(commandDeatBenfitForm, "@chvBeneficiaryLastName", DbType.String, StrBeneficiaryLastName);
                db.AddInParameter(commandDeatBenfitForm, "@chrBeneficiaryTaxNumber", DbType.String, StrBeneficiarySSNo);
                //SP 2014.12.04 BT-2310\YRS 5.0-2255:- Start
                db.AddInParameter(commandDeatBenfitForm, "@chvRepFirstName", DbType.String, strRepFirstName);
                db.AddInParameter(commandDeatBenfitForm, "@chvRepLastName", DbType.String, strRepLastName);
                db.AddInParameter(commandDeatBenfitForm, "@chvRepSalutation", DbType.String, strRepSalutation);
                db.AddInParameter(commandDeatBenfitForm, "@chvRepTelephone", DbType.String, strRepTelephone);
                //SP 2014.12.04 BT-2310\YRS 5.0-2255:- End 
                
                //AA 2015.12.10 YRS-AT-2478 Added to store taxable and non taxable money-Start
                db.AddInParameter(commandDeatBenfitForm, "@mnyRetTaxable", DbType.Decimal, decRetTaxable);
                db.AddInParameter(commandDeatBenfitForm, "@mnyRetNonTaxable", DbType.Decimal, decRetNonTaxable);
                db.AddInParameter(commandDeatBenfitForm, "@mnySavTaxable", DbType.Decimal, decSavTaxable);
                db.AddInParameter(commandDeatBenfitForm, "@mnySavNonTaxable", DbType.Decimal, decSavNonTaxable);
                //AA 2015.12.10 YRS-AT-2478 Added to store taxable and non taxable money-end
                //START-Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                db.AddInParameter(commandDeatBenfitForm, "@mnyPrincipalGuaranteeAnnuityRetTaxable", DbType.Decimal, decPrincipalGuaranteeAnnuityRetTaxable);
                db.AddInParameter(commandDeatBenfitForm, "@mnyPrincipalGuaranteeAnnuityRetNonTaxable", DbType.Decimal, decPrincipalGuaranteeAnnuityRetNonTaxable);
                db.AddInParameter(commandDeatBenfitForm, "@mnyPrincipalGuaranteeAnnuitySavTaxable", DbType.Decimal, decPrincipalGuaranteeAnnuitySavTaxable);
                db.AddInParameter(commandDeatBenfitForm, "@mnyPrincipalGuaranteeAnnuitySavNonTaxable", DbType.Decimal, decPrincipalGuaranteeAnnuitySavNonTaxable);
                //END-Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                db.AddOutParameter(commandDeatBenfitForm, "@int_ReturnStatus", DbType.Int32, 32);                
              
                db.ExecuteNonQuery(commandDeatBenfitForm);
                strReturnStatus = Convert.ToString(db.GetParameterValue(commandDeatBenfitForm, "@int_ReturnStatus"));
                return strReturnStatus; 
            }
            catch
            {
                throw;
            }
            finally
            {
                commandDeatBenfitForm = null;
                db = null;
                strReturnStatus = string.Empty;
            }



        }

        //Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up
        //get the Forms list from database
        public static DataSet GetMetaAdditionalForms()
        {
         
                DbCommand commandGetAdditionalForms = null;
                Database db = null;
                DataSet l_dataset_DeathCalc_Annuities = null;
                try
                {
                    db = DatabaseFactory.CreateDatabase("YRS");

                    if (db == null) return null;

                    commandGetAdditionalForms = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetDeathBenefitAdditionalForms");
                    if (commandGetAdditionalForms == null) return null;

                    commandGetAdditionalForms.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);


                    l_dataset_DeathCalc_Annuities = new DataSet();
                    db.LoadDataSet(commandGetAdditionalForms, l_dataset_DeathCalc_Annuities, "Additional_Forms");
                    return l_dataset_DeathCalc_Annuities;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    commandGetAdditionalForms = null;
                    db = null;
                    
                }
            
        }
        //Inserts the form details into database
        public static void SaveDeathFormDetails(DataTable dtDeathBenefitFormReqdDocs)
        {
            DbCommand commandSaveDeathForms = null;
            Database db = null;
            DbTransaction tran = null;
            DbConnection cn = null;
            int intDBAppFormID;
            int intMetaDBAdditionalFormID;
            string chvAdditionalText;
            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;
                
                cn = db.CreateConnection();
                cn.Open();
                //Get a Transaction from the Database
                tran = cn.BeginTransaction(IsolationLevel.Serializable);
                if (tran == null)
                {
                    commandSaveDeathForms = null;
                    db = null;
                }
                
                for (int i = 0; i < dtDeathBenefitFormReqdDocs.Rows.Count; i++)
                {
                    commandSaveDeathForms = db.GetStoredProcCommand("yrs_usp_DeathCalc_InsertFormsAndDocs");
                    if (commandSaveDeathForms == null) return;
                    intDBAppFormID = Convert.ToInt32(dtDeathBenefitFormReqdDocs.Rows[i].ItemArray[0]);
                    intMetaDBAdditionalFormID = Convert.ToInt32(dtDeathBenefitFormReqdDocs.Rows[i].ItemArray[1]);
                    chvAdditionalText = Convert.ToString(dtDeathBenefitFormReqdDocs.Rows[i].ItemArray[2]);
                    db.AddInParameter(commandSaveDeathForms, "@intDBAppFormID", DbType.Int32, intDBAppFormID);
                    db.AddInParameter(commandSaveDeathForms, "@intMetaDBAdditionalFormID", DbType.Int32, intMetaDBAdditionalFormID);
                    db.AddInParameter(commandSaveDeathForms, "@chvAdditionalText", DbType.String, chvAdditionalText);
                    db.ExecuteNonQuery(commandSaveDeathForms);
                }
                tran.Commit();
                
            }
            catch
            {
                if (tran != null) tran.Rollback();
                if (cn != null) cn.Close();
                throw;
            }
            finally
            {
                commandSaveDeathForms = null;
                db = null;
                cn.Close();
                cn.Dispose();
                tran.Dispose();
                tran = null;
                cn = null;

            }
        }
        //Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up
        //Anudeep A :2012.12.04-YRS 5.0-1707:New Death Benefit Application form 
        //To get the beneficiary details before caluculations
        public static DataSet PopulateBeneficiaries(string parameterPersId, string parameterFundEventID,DateTime parameterDeathDate,string parameterFundStatus)
        {
            Database db = null;
            DbCommand commandDeathCalculationProcessing = null;
            DataSet l_dataset_DeathCalc_BeneficiariesData = null;
            try
            {


                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                commandDeathCalculationProcessing = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetBeneficiaries");
                commandDeathCalculationProcessing.CommandTimeout = 1200;
                if (commandDeathCalculationProcessing == null) return null;
                db.AddInParameter(commandDeathCalculationProcessing, "@varchar_lcPersID", DbType.String, parameterPersId);
                db.AddInParameter(commandDeathCalculationProcessing, "@Varchar_FundEventID", DbType.String, parameterFundEventID);
                db.AddInParameter(commandDeathCalculationProcessing, "@datetime_ldDeathDate", DbType.String, parameterDeathDate);
                db.AddInParameter(commandDeathCalculationProcessing, "@varchar_FundStatus", DbType.String, parameterFundStatus);
                l_dataset_DeathCalc_BeneficiariesData = new DataSet();
                db.LoadDataSet(commandDeathCalculationProcessing, l_dataset_DeathCalc_BeneficiariesData, "r_DeathCalc_Beneficiaries[]");

                return l_dataset_DeathCalc_BeneficiariesData;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db = null;
                commandDeathCalculationProcessing = null;
                
            }
        }
        
        //Sanjay R.:2012.12.14-YRS 5.0-1707:New Death Benefit Application form 
        //To get the JS annuities options for beneficiary
        public static DataSet PopulateJSAnnuities(string parameterFundEventID)
        {
            Database db = null;
            DbCommand commandDeathCalculationProcessing = null;
            DataSet l_dataset_DeathCalc_JSAnnuities = null;
            try
            {


                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                commandDeathCalculationProcessing = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetJSAnnuities");
                commandDeathCalculationProcessing.CommandTimeout = 1200;
                if (commandDeathCalculationProcessing == null) return null;            
                db.AddInParameter(commandDeathCalculationProcessing, "@varchar_FundEventID", DbType.String, parameterFundEventID);
                l_dataset_DeathCalc_JSAnnuities = new DataSet();
                db.LoadDataSet(commandDeathCalculationProcessing, l_dataset_DeathCalc_JSAnnuities, "r_DeathCalc_JSAnnuities[]");
                return l_dataset_DeathCalc_JSAnnuities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db = null;
                commandDeathCalculationProcessing = null;
            }
        }

        //SR:2012.12.15 -YRS 5.0-1707 : For DD,RA,RT,RE,RDNP,RPT persons calculate separately 
        public static DataSet Calculate_Death_Benefits_For_FundstatusDD(string parameterPersId, string parameterFundEventID, DateTime parameterDeathDate, string parameterFundStatus, string parameterForceCalculationsAs, out int l_int_returnStatus)
        { 
            DataSet l_dataset_DeathCalc_BeneficiaryData = null;
            DataSet l_dataset_DeathCalc_DRData = null;
            DataSet l_dataset_DeathCalc_DAData = null;
            DataSet l_dataset_DeathCalc_FinalData = new DataSet();
            int l_int_returnStatus_DR;
            int l_int_returnStatus_DA;


            try
            {
                parameterFundStatus = "DD";
                l_dataset_DeathCalc_BeneficiaryData = PopulateBeneficiaries(parameterPersId, parameterFundEventID, parameterDeathDate, parameterFundStatus);
                parameterFundStatus = "DR";
                l_dataset_DeathCalc_DRData = Calculate_Death_Benefits(parameterPersId, parameterFundEventID, parameterDeathDate, parameterFundStatus, parameterForceCalculationsAs, out l_int_returnStatus);
                l_int_returnStatus_DR = l_int_returnStatus;
                parameterFundStatus = "DA";
                l_dataset_DeathCalc_DAData = Calculate_Death_Benefits(parameterPersId, parameterFundEventID, parameterDeathDate, parameterFundStatus, parameterForceCalculationsAs, out l_int_returnStatus);
                l_int_returnStatus_DA = l_int_returnStatus;
               

                if (l_dataset_DeathCalc_DAData.Tables.Count == 0 && l_dataset_DeathCalc_DRData.Tables.Count == 0 )
                    {
                        l_int_returnStatus = -9;
                   }
                else if (l_dataset_DeathCalc_DAData.Tables.Count == 0)
                   {
                            l_dataset_DeathCalc_FinalData = l_dataset_DeathCalc_DRData.Copy();
                            l_int_returnStatus =l_int_returnStatus_DA;
                    }
                else if (l_dataset_DeathCalc_DRData.Tables.Count == 0)
                    {
                        l_dataset_DeathCalc_FinalData = l_dataset_DeathCalc_DAData.Copy();
                        l_int_returnStatus = l_int_returnStatus_DR;
                    }
                else
                    {
                        l_dataset_DeathCalc_FinalData.Tables.Add(l_dataset_DeathCalc_BeneficiaryData.Tables[0].Copy());
                        l_dataset_DeathCalc_FinalData.Tables.Add(l_dataset_DeathCalc_DRData.Tables[1].Copy());
                        l_dataset_DeathCalc_FinalData.Tables.Add(l_dataset_DeathCalc_DAData.Tables[2].Copy());
                        l_dataset_DeathCalc_FinalData.Tables.Add(l_dataset_DeathCalc_DRData.Tables[3].Copy());
                        l_dataset_DeathCalc_FinalData.Tables.Add(l_dataset_DeathCalc_DRData.Tables[4].Copy());
                        l_dataset_DeathCalc_FinalData.Tables.Add(l_dataset_DeathCalc_DAData.Tables[5].Copy());
                        l_dataset_DeathCalc_FinalData.Tables.Add(l_dataset_DeathCalc_DRData.Tables[6].Copy()); // SR | 2015.12.09 | YRS-AT-2718 | Added new table to get list of SSNo for whom death Benefit not Allowed
                    }
                return l_dataset_DeathCalc_FinalData;              
               
        
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        //SR:2012.12.15 -YRS 5.0-1707 : For DD,RA,RT,RE,RDNP,RPT persons calculate separately 
        public static DataSet GetDeathCalulation(string parameterPersId, string parameterFundEventID, DateTime parameterDeathDate, string parameterFundStatus, string parameterForceCalculationsAs, out int l_int_returnStatus)
        {
           try
           {              
               if ((parameterFundStatus =="DD") || (parameterFundStatus == "RA") || (parameterFundStatus=="RT") ||(parameterFundStatus== "RE") || (parameterFundStatus== "RDNP") || (parameterFundStatus== "RPT"))
              {
                 return  Calculate_Death_Benefits_For_FundstatusDD( parameterPersId,  parameterFundEventID,  parameterDeathDate,  parameterFundStatus,  parameterForceCalculationsAs, out l_int_returnStatus);
              }
              else
              {
                 return  Calculate_Death_Benefits( parameterPersId,  parameterFundEventID ,  parameterDeathDate,  parameterFundStatus,  parameterForceCalculationsAs, out  l_int_returnStatus);
               }
           }
          catch
          {
             throw;
          }
        }

        //SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
        public static DataSet GetBeneficiaryAdress(string parameterSSN, string parameterPersId, string paramFirstName,string paramLastName)
        {
            DataSet dsBendtls = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;           

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_GetBeneficiaryAddress");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@paramSSNO", DbType.String, parameterSSN);
                db.AddInParameter(LookUpCommandWrapper, "@paramPersId", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@paramFirstName", DbType.String, paramFirstName);
                db.AddInParameter(LookUpCommandWrapper, "@paramLastName", DbType.String, paramLastName);   
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsBendtls = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsBendtls,"BenAddress");
                return dsBendtls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //End, SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.

        //Start:Anudeep:12.08.2013:Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
        public static DataSet GetFollowupDays()
        {
            DataSet dsFollowupdetails = new DataSet();
            Database db = null;
            DbCommand LookUpCommandWrapper = null; 
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeathCalc_GetDeathBenfitFollowupStatus");

                if (LookUpCommandWrapper == null) return null;
                db.ExecuteNonQuery(LookUpCommandWrapper);
                db.LoadDataSet(LookUpCommandWrapper, dsFollowupdetails, "FollowupDetails");
                return dsFollowupdetails;   
            }
            catch
            {
                throw;
            }
        }

        public static void SaveResponse(List<Dictionary<string, string>> LTerminationIds)
        {
            string strReturnValue = string.Empty;
            string strUniqueId = string.Empty;
            Boolean blnResponse;
            Database db = null;
            DbCommand updateCommandWrapper = null;
            DbTransaction tran = null;
            DbConnection cn = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;

                //Get a connection and Open it
                cn = db.CreateConnection();
                cn.Open();

                //Get a Transaction from the Database
                tran = cn.BeginTransaction(IsolationLevel.Serializable);
                if (tran == null) return;

                foreach (Dictionary<string, string>  item in LTerminationIds)
                {
                    strUniqueId = item["Id"];
                    blnResponse = Convert.ToBoolean(item["boolvalue"]);

                    updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeathCalc_SaveFollowupResponse");

                    db.AddInParameter(updateCommandWrapper, "@intDeathBenefitFollowupId", DbType.String, strUniqueId);

                    db.AddInParameter(updateCommandWrapper, "@bitResponseReceived", DbType.Boolean, blnResponse);

                    db.AddOutParameter(updateCommandWrapper, "@outIntReturnValue", DbType.Int32, 10);


                    db.ExecuteNonQuery(updateCommandWrapper);

                    strReturnValue = Convert.ToString(db.GetParameterValue(updateCommandWrapper, "@outIntReturnValue"));

                    if (strReturnValue != "1")
                    {
                        tran.Rollback();
                        return;


                    }

                }

                tran.Commit();
                return;
            }

            catch (Exception ex)
            {
                if (tran != null)tran.Rollback();
                if (cn != null) cn.Close();
                throw ex;
            }
            finally
            {
                db = null;
                cn.Close();
                cn.Dispose();
                tran.Dispose();
                tran = null;
                cn = null;
            }
        }


        public static void UpdateFollowupStatus(List<Dictionary<string, string>> LTerminationIds)
        {
            string strReturnValue = string.Empty;
            string strUniqueId = string.Empty;
            string strResponse;
            Database db = null;
            DbCommand updateCommandWrapper = null;
            DbTransaction tran = null;
            DbConnection cn = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;

                //Get a connection and Open it
                cn = db.CreateConnection();
                cn.Open();

                //Get a Transaction from the Database
                tran = cn.BeginTransaction(IsolationLevel.Serializable);
                if (tran == null) return;

                foreach (Dictionary<string, string>  item in LTerminationIds)
                {
                    strUniqueId = item["Id"];
                    strResponse = item["value"];

                    updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeathCalc_UpdateFollowupStatus");

                    db.AddInParameter(updateCommandWrapper, "@intDeathBenefitFollowupId", DbType.String, strUniqueId);

                    db.AddInParameter(updateCommandWrapper, "@chvValue", DbType.String, strResponse);


                    db.AddOutParameter(updateCommandWrapper, "@outIntReturnValue", DbType.Int32, 10);


                    db.ExecuteNonQuery(updateCommandWrapper);

                    strReturnValue = Convert.ToString(db.GetParameterValue(updateCommandWrapper, "@outIntReturnValue"));

                    if (strReturnValue != "1")
                    {
                        tran.Rollback();
                        return;


                    }

                }

                tran.Commit();
                return;
            }

            catch (Exception ex)
            {
                if (tran != null)tran.Rollback();
                if (cn != null) cn.Close();
                throw ex;
            }
            finally
            {
                db = null;
                cn.Close();
                cn.Dispose();
                tran.Dispose();
                tran = null;
                cn = null;
            }
        }
        //End:Anudeep:12.08.2013:Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.

        // Start: Anudeep BT:2460: YRS 5.0-2331 - Added to get the uniqueid of joint survivor address 
        public static DataSet GetJointSurviourAdress(string paramSSN, string paramPersId)
        {
            DataSet dsBendtls = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_GetAnnuityJointSurvivorAddress");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@paramSSNO", DbType.String, paramSSN);
                db.AddInParameter(LookUpCommandWrapper, "@paramPersId", DbType.String, paramPersId);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsBendtls = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsBendtls, "BenAddress");
                return dsBendtls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // End: Anudeep BT:2460: YRS 5.0-2331 - Added to get the uniqueid of joint survivor address 

        // Start: 2014.12.02 Shashank BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
        public static DataSet GetFollowupBeneficiariesDetails( string paramPersId)
        {
            DataSet dsBenFollowup = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dth_GetFollowupBeneficiariesDetails");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@guiPersID", DbType.String, paramPersId);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsBenFollowup = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsBenFollowup, "Benefollowup");
                return dsBenFollowup;
            }
            catch 
            {
                throw;
            }
        }

        public static DataSet GetFollowUpRepresentativeDetails(string paramPersId, string paramGuiBeneficiaryId, string paramBeneSSN)
        {
            DataSet dsRepDetails ;
            Database db = null;
            DbCommand dbCommand = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) throw new Exception("database object db is null");

                dbCommand = db.GetStoredProcCommand("yrs_usp_Dth_GetFollowUpRepresentativeDetails");
                if (dbCommand == null) throw new Exception("dbCommand object is null");

                db.AddInParameter(dbCommand, "@guiPersID", DbType.String, paramPersId);
                db.AddInParameter(dbCommand, "@chvBeneficiaryTaxNumber", DbType.String, paramBeneSSN);
                db.AddInParameter(dbCommand, "@guiBeneficiaryId", DbType.String, paramGuiBeneficiaryId);
                             
                db.ExecuteNonQuery(dbCommand);

                dsRepDetails = new DataSet();
                db.LoadDataSet(dbCommand, dsRepDetails, "RepDetails");


                return dsRepDetails;
            }
            catch
            {
                throw;
            }
        }

        // End: 2014.12.02  Shashank BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
	}
}
