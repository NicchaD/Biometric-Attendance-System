//*******************************************************************************
// Project Name		:	YMCA PS
// FileName			:	RetirementDAClass.cs
// Author Name		:	Mohammed hafiz
// Employee ID		:	33284
// Email			:	hafiz.rehman@3i-infotech.com
// Contact No		:	8641
// Creation Time	:	07-Dec-2006
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	
//************************************************************************************
//Modficiation History
//************************************************************************************
//Date			Modified By			Description
//************************************************************************************
//8-Apr-2008	Mohammed Hafiz      Phase IV Changes
//15-Apr-2009	Ashish Srivastava	Phase V changes
//31-July-2009	Ashish Srivastava	Phase V part III changes
//2009.11.17	Ashish Srivastava	Change parameter persID with fundEventID for calculate average salary
//2010.06.21	Ashish Srivastava	YRS 5.0-1115
//  2010.10.11  Ashish Srivastava   Resolved Issue  YRS 5.0-855 BT-624
//2010.11.16    Ashish Srivastava   Resolved Issue  YRS 5.0-1215 BT-666
//2010.11.17    Priya Jawale        Added validation method for exact age annuity
//2011-02-01    Ashish Srivastava   BT-665 Disability Retirement
//2011.02.21    Ashish Srivsatava   replace perid with fundeventid for getting Ymca resolution
//2011.03.24    Sanket Vaidya       For YRS 5.0-1294,BT 794  : For disability requirement 
//2011.06.01    Sanket vaidya       YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
//2012.06.28	Shashank Patel		BT-712/YRS 5.0-1246 : Handling DLIN records
//2012.08.07    Sanjay R.           BT-957/YRS 5.0-1484 : Termination Watcher 
//2012-10-18	Priya P				YRS 5.0-1484 : Termination Watcher: commented SR code of termiantion watcher to undo release from 12.7.0 patch
//2012-10-30    Anudeep             YRS 5.0-1484 : Termination Watcher: reverted Changes as per Observations 
//2013-07-10    Anudeep             BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
//20/08/2013    Dinesh.k            BT-2161: YRS 5.0-2191 - Trying to do a second retirement
//2013-08-22    Anudeep             YRS 5.0-1862:Add notes record when user enters address in any module.
//2014.11.20    Shashank Patel      BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
//2015.09.16    Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2015.12.17    Sanjay Singh        YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
//2016.04.21    Chandra sekar.c     YRS-AT-2612 - YRS enh: Annuity Estimate Calculator should include 15+ yrs of service catchup
//2016.04.21    Chandra sekar.c     YRS-AT-2891 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
//2016.05.05    Pramod P. Pokale    YRS-AT-2683 - YRS and website enhancement-iFor PRA calculator use full YTD and full projected contributions
//2016.07.27    Manthan Rajguru     YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field. 
//2016.08.02    Sanjay Singh        YRS-AT-2382 - Capture 'Reason' for change of beneficiary SSN and Annty bene SSN(TrackIT 19856)
//2017.03.01    Pramod P. Pokale    YRS-AT-3317 - YRS bug - Annuity Estimate calculator reported lowering annual amt for the same retirement dates. (TrackIT 28688) 
//2017.03.03    Manthan Rajguru     YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012)
//2017.03.06    Santosh Bura        YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012) 
//2017.12.28    Pramod P. Pokale    YRS-AT-3328 -  YRS bug-annuity estimates not allowing exclusion of certain accounts (TrackIT 28917) 
//2018.10.31    Benhan David        YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497) 
//2019.10.24    Megha Lad           YRS-AT-4597 - YRS enh: State Withholding Project - First Annuity Payments (UI design)
//****************************************************************************************************
using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// This class fethces all the Retirement related information from the database for 
	/// RetirementEstimate and RetirementProcessing. 
	/// </summary>
	public class RetirementDAClass
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public RetirementDAClass()
		{
			
		}


		#region Retirement Estimate
		public static DataSet getRetireeInformation(string Param_RetireeSSNo)
		{
			DataSet dsSearchRetiree = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_getRetireeInfo");
				
				if (SearchCommandWrapper == null) return null;
				db.AddInParameter(SearchCommandWrapper,"@SSNo",DbType.String,Param_RetireeSSNo);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				dsSearchRetiree = new DataSet();
				dsSearchRetiree.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchRetiree,"RetireeInformation");
				
				return dsSearchRetiree;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
				
		public static DataSet getParticipantBeneficiaries(string Param_guiUniqueID)
		{
			DataSet dsSearchRetEst = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				//SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_getRetireeEstInfo"); //Phase IV Changes
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_GetParticipantBeneficiaries");
				
				if (SearchCommandWrapper == null) return null;
				db.AddInParameter(SearchCommandWrapper,"@guiUniqueID",DbType.String,Param_guiUniqueID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				dsSearchRetEst = new DataSet();
				dsSearchRetEst.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchRetEst,"RetireeEstInformation");
				
				return dsSearchRetEst;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}


        /// <summary>
        /// This Method Return Employement records and average salary
        /// </summary>
        /// <param name="Param_guiFundEventID"></param>
        /// <returns></returns>
        //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
		//public static DataSet getRetEmpInformation(string Param_guiPersID)               
        public static DataSet getRetEmpInformation(string param_guiFundEventID, string retireType, DateTime retirementDate) //MMR | 2017.03.09 | YRS-AT-2625 | Added parameter for retire type and retirement date
		{
			DataSet dsSearchRetEmpInfo = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_getRetireeEmpInfo");
				
				if (SearchCommandWrapper == null) return null;
                //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
				//db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,Param_guiPersID);
                db.AddInParameter(SearchCommandWrapper, "@guiFundEventID", DbType.String, param_guiFundEventID);
                db.AddInParameter(SearchCommandWrapper, "@VARCHAR_RetireType", DbType.String, retireType); //MMR | 2017.03.09 | YRS-AT-2625 | Addeed parameter for retire type
                db.AddInParameter(SearchCommandWrapper, "@DATETIME_RetirementDate", DbType.DateTime, retirementDate); //MMR | 2017.03.09 | YRS-AT-2625 | Addeed parameter for retirement date
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				dsSearchRetEmpInfo = new DataSet();
				dsSearchRetEmpInfo.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchRetEmpInfo,"RetireeEmployeeInformation");
				
				return dsSearchRetEmpInfo;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getRetEmpSalInformation(string Param_guiFundEventID,string Param_guiYmcaID,string Param_StartDate,string Param_EndDate)
		{
			DataSet dsSearchRetEmpSalInfo = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");			
				if (db == null) return null;

				if (Param_EndDate == string.Empty)
				{
					SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_salary_NoEndDate");
					if (SearchCommandWrapper == null) return null;
					//Ashish:2009.11.17 Added fundEventID as parameter
					//SearchCommandWrapper.AddInParameter("@guiPersID",DbType.String,Param_guiPersID);
					db.AddInParameter(SearchCommandWrapper,"@guiFundEventID",DbType.String,Param_guiFundEventID);
					db.AddInParameter(SearchCommandWrapper,"@guiYmcaID",DbType.String,Param_guiYmcaID);
					db.AddInParameter(SearchCommandWrapper,"@StartDate",DbType.String,Param_StartDate);
					SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
					dsSearchRetEmpSalInfo = new DataSet();
					dsSearchRetEmpSalInfo.Locale = CultureInfo.InvariantCulture;
					db.LoadDataSet(SearchCommandWrapper, dsSearchRetEmpSalInfo,"RetireeSalaryInformation");
				}
				else
				{
					SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_salary_WithEndDate");
					if (SearchCommandWrapper == null) return null;
					//Ashish:2009.11.17 Added fundEventID as parameter
					//SearchCommandWrapper.AddInParameter("@guiPersID",DbType.String,Param_guiPersID);
					db.AddInParameter(SearchCommandWrapper,"@guiFundEventID",DbType.String,Param_guiFundEventID);
					db.AddInParameter(SearchCommandWrapper,"@guiYmcaID",DbType.String,Param_guiYmcaID);
					db.AddInParameter(SearchCommandWrapper,"@StartDate",DbType.String,Param_StartDate);
					db.AddInParameter(SearchCommandWrapper,"@EndDate",DbType.String,Param_EndDate);
                    SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
					dsSearchRetEmpSalInfo = new DataSet();
					dsSearchRetEmpSalInfo.Locale = CultureInfo.InvariantCulture;
					db.LoadDataSet(SearchCommandWrapper, dsSearchRetEmpSalInfo,"RetireeSalaryInformation");
				}
				
				return dsSearchRetEmpSalInfo;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getRetElectiveAccounts(string Param_guiPersID,string Param_guiYmcaID)
		{
			DataSet dsSearchElectiveAccounts = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getElectiveAccounts");
				
				if (SearchCommandWrapper == null) return null;
			
				db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,Param_guiPersID);
				db.AddInParameter(SearchCommandWrapper,"@YmcaID",DbType.String,Param_guiYmcaID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 								
				dsSearchElectiveAccounts = new DataSet();
				dsSearchElectiveAccounts.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchElectiveAccounts,"RetireeElectiveAccountsInformation");
				
				return dsSearchElectiveAccounts;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getRetElectiveAccounts(string Param_guiPersID)
		{
			DataSet dsSearchElectiveAccounts = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getElectiveAccounts");
				
				if (SearchCommandWrapper == null) return null;
			
				db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,Param_guiPersID);
				db.AddInParameter(SearchCommandWrapper,"@YmcaID",DbType.String,"");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 								
				dsSearchElectiveAccounts = new DataSet();
				dsSearchElectiveAccounts.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchElectiveAccounts,"RetireeElectiveAccountsInformation");
				
				return dsSearchElectiveAccounts;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getRetWorkDates(string Param_guiPersID)
		{
			DataSet dsSearchWorkDates = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_getRetiree_Workdates");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,Param_guiPersID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				dsSearchWorkDates = new DataSet();
				dsSearchWorkDates.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchWorkDates,"RetireeStartedWorkDateInformation");

				return dsSearchWorkDates;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getRetAccountsBalanceData(bool _ByBasis,string Param_StartDate, string Param_EndDate, string Param_guiPersID, string Param_guiFundEventID, bool Param_AllocateDummyAssnMoney,bool Param_CondenseMoneyByAcctbyBasis, string planType)
		{
			DataSet dsSearchAccountsBalance = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");			
				if (db == null) return null;

				if (_ByBasis)	
				{
					//Commented by Ashish for pahase V part III changes
					//SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_get_TransSumByAccountByBasis");
					SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Ret_Est_GetAccountBalSumByAccountByBasis");
				}
				else
				{
					SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_get_TransSumByAccount");
				}
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper,"@startdate",DbType.String,Param_StartDate);
				db.AddInParameter(SearchCommandWrapper,"@enddate",DbType.String,Param_EndDate);
				db.AddInParameter(SearchCommandWrapper,"@guiFundEventId",DbType.String, Param_guiFundEventID);
				db.AddInParameter(SearchCommandWrapper,"@planType", DbType.String, planType);
				//added by Ashish for pahase V part III changes
				if(!_ByBasis)
				{
					db.AddInParameter(SearchCommandWrapper,"@llAllocateDummyAssnMoney",DbType.Boolean,Param_AllocateDummyAssnMoney);
					db.AddInParameter(SearchCommandWrapper,"@llCondenseMoneyByAcctByBasis",DbType.Boolean,Param_CondenseMoneyByAcctbyBasis);
				}
				
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				
				dsSearchAccountsBalance = new DataSet();
				dsSearchAccountsBalance.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchAccountsBalance,"RetireeAccountsBalanceInformation");
				
				return dsSearchAccountsBalance;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getEmploymentDetails(string Param_guiFundEventID, string Param_RetireType,string Param_ProjectedRetirementDate)
		{
			DataSet dsSearchRetEstimateEmployment = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				//SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_get_CurRetEstimates"); //Phase IV Changes
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_GetEmploymentDetails");				
				
				if (SearchCommandWrapper == null) return null;
				db.AddInParameter(SearchCommandWrapper,"@guiFundEventId", DbType.String, Param_guiFundEventID);
				db.AddInParameter(SearchCommandWrapper,"@RetireType",DbType.String,Param_RetireType);
				db.AddInParameter(SearchCommandWrapper,"@ProjectedRetirementDate",DbType.String,Param_ProjectedRetirementDate);
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				dsSearchRetEstimateEmployment = new DataSet();
				dsSearchRetEstimateEmployment.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchRetEstimateEmployment,"RetEstimateEmployment");
				
				return dsSearchRetEstimateEmployment;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getContributionLimits()
		{
			DataSet dsSearchContributionLimits = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
			
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_ContributionLimits");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
							
				dsSearchContributionLimits = new DataSet();
				dsSearchContributionLimits.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchContributionLimits,"ContributionLimitsInformation");
				
				return dsSearchContributionLimits;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getAnnuityFactorPre96()
		{
			DataSet dsSearchAnnuityFactor = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getAnnuityFactorPre96");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
								
				dsSearchAnnuityFactor = new DataSet();
				dsSearchAnnuityFactor.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchAnnuityFactor,"AnnuityFactorPre96");
				
				return dsSearchAnnuityFactor;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		public static DataSet getMetaInterestRates()
		{
			DataSet dsSearchMetaInterestRates = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getMetaInterestRates");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
								
				dsSearchMetaInterestRates = new DataSet();
				dsSearchMetaInterestRates.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchMetaInterestRates,"MetaInterestRates");
				
				return dsSearchMetaInterestRates;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getMetaAccountTypes()
		{
			DataSet dsSearchMetaAccountTypes = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
			
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getMetaAccountTypes");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
							
				dsSearchMetaAccountTypes = new DataSet();
				dsSearchMetaAccountTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchMetaAccountTypes,"MetaAccountTypes");
				
				return dsSearchMetaAccountTypes;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		
		public static DataSet getKnownInterestRate(string Param_guiAcctType,string Param_Year,string Param_Month)
		{
			DataSet dsSearchKnownInterestRate = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getKnownInterestRate");

				db.AddInParameter(SearchCommandWrapper,"@guiAccountType",DbType.String,Param_guiAcctType);
				db.AddInParameter(SearchCommandWrapper,"@chrYear",DbType.String,Param_Year);
				db.AddInParameter(SearchCommandWrapper,"@chrMonth",DbType.String,Param_Month);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
								
				dsSearchKnownInterestRate = new DataSet();
				dsSearchKnownInterestRate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchKnownInterestRate,"GetKnownInterestRate");
				
				return dsSearchKnownInterestRate;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getActiveEmploymentEvents(string Param_guiPersID)
		{
			DataSet dsSearchActiveEmploymentEvents = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getActiveEmploymentEvents");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,Param_guiPersID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsSearchActiveEmploymentEvents = new DataSet();
				dsSearchActiveEmploymentEvents.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchActiveEmploymentEvents,"ActiveEmploymentEvents");
				
				return dsSearchActiveEmploymentEvents;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
        //ASHISH:2011.02.21 Replace persid with fundeventID 
        // START : SB | 03/06/2017 | YRS-AT-2625 | Existing function is being replaced by additional parameters for finding out resolution in calculating retire Estimates 
		//public static DataSet getYmcaResolutions(string Param_guiPersID)
 		//public static DataSet getYmcaResolutions(string Param_guiFundEventID)
        public static DataSet getYmcaResolutions(string Param_guiFundEventID, DateTime Param_RetirementDate, string Param_RetirementType)
        // END : SB | 03/06/2017 | YRS-AT-2625 | Existing function is being replaced by additional parameters for finding out resolution in calculating retire Estimates 
		{
			DataSet dsSearchYmcaResolutions = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			string [] l_TableNames;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getYmcaResolutions");
				
				if (SearchCommandWrapper == null) return null;
				
				//db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,Param_guiPersID);
                db.AddInParameter(SearchCommandWrapper, "@guiFundEventId", DbType.String, Param_guiFundEventID);
                //START: SB | 03/06/2017 | YRS-AT-2625 | In case of Disability Retirement, we need to fetch highest resolution. So passing retirement date which will help to decide how many resolutions we can consider and retirement type
                db.AddInParameter(SearchCommandWrapper, "@DATETIME_RetirementDate", DbType.Date, Param_RetirementDate);  
                db.AddInParameter(SearchCommandWrapper, "@VARCHAR_RetirementType", DbType.String, Param_RetirementType);
                //END: SB | 03/06/2017 | YRS-AT-2625 | In case of Disability Retirement, we need to fetch highest resolution. So passing retirement date which will help to decide how many resolutions we can consider and retirement type
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				dsSearchYmcaResolutions = new DataSet();
				dsSearchYmcaResolutions.Locale = CultureInfo.InvariantCulture;

				l_TableNames = new string [] {"YmcaResolutions","YmcaResolutionsDistinct"};
				
				db.LoadDataSet(SearchCommandWrapper, dsSearchYmcaResolutions, l_TableNames);
				
				return dsSearchYmcaResolutions;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

        //ASHISH:2011.02.21 This method is not in use.
        ///// <summary>
        ///// Get YMCA resolutions as per plan type.
        ///// </summary>
        ///// <param name="Param_guiPersID"></param>
        ///// <param name="planType"></param>
        ///// <returns></returns>
        //public static DataSet getYmcaResolutions(string Param_guiPersID, string planType)
        //{
        //    DataSet dsSearchYmcaResolutions = null;
        //    Database db = null;
        //    DbCommand SearchCommandWrapper = null;
        //    string [] l_TableNames;
        //    try
        //    {
        //        db= DatabaseFactory.CreateDatabase("YRS");
			
        //        if (db == null) return null;
				
        //        SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getYmcaResolutions");
				
        //        if (SearchCommandWrapper == null) return null;
				
        //        db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String, Param_guiPersID);
        //        db.AddInParameter(SearchCommandWrapper,"@planType", DbType.String, planType);
        //        SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
        //        dsSearchYmcaResolutions = new DataSet();
        //        dsSearchYmcaResolutions.Locale = CultureInfo.InvariantCulture;

        //        l_TableNames = new string [] {"YmcaResolutions","YmcaResolutionsDistinct"};
				
        //        db.LoadDataSet(SearchCommandWrapper, dsSearchYmcaResolutions, l_TableNames);
				
        //        return dsSearchYmcaResolutions;
        //    }
        //    catch 
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        SearchCommandWrapper.Dispose();
        //        SearchCommandWrapper=null;
        //        db = null;
        //    }
        //}

		public static DataSet getTerminationDate(string Param_guiPersID)
		{
			DataSet dsSearchTerminationDate = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getTerminationDate");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,Param_guiPersID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsSearchTerminationDate = new DataSet();
				dsSearchTerminationDate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchTerminationDate,"TerminationDate");
				
				return dsSearchTerminationDate;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getAnnuitize(string tdRetireeBirthDate,string tdRetireDate ,string tcAnnuityType,string tcRetireType, string tcBasisType, string tdBeneficiaryBirthDate , decimal tnBalance )
		
		{
			DataSet dsSearchAnnuitize = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_RetAnnuitize");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@dRetireeBirthday",DbType.String,tdRetireeBirthDate);
				db.AddInParameter(SearchCommandWrapper,"@dRetirementDate",DbType.String,tdRetireDate);
				db.AddInParameter(SearchCommandWrapper,"@cAnnuityType",DbType.String,tcAnnuityType);
				db.AddInParameter(SearchCommandWrapper,"@cRetirementType",DbType.String,tcRetireType);
				db.AddInParameter(SearchCommandWrapper,"@cBasisType",DbType.String,tcBasisType);
				db.AddInParameter(SearchCommandWrapper,"@dBeneficiaryBirthday",DbType.String,tdBeneficiaryBirthDate);
				db.AddInParameter(SearchCommandWrapper,"@nBalance",DbType.String,tnBalance);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsSearchAnnuitize = new DataSet();
				dsSearchAnnuitize.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchAnnuitize,"GetAnnuity");
				
				return dsSearchAnnuitize;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
        //////ASHISH:2010.11.16:Added new method for YRS 5.0-1215
        //public static DataSet getAnnuitizeWithExactAge(string tdRetireeBirthDate, string tdRetireDate, string tcAnnuityType, string tcRetireType, string tcBasisType, string tdBeneficiaryBirthDate, decimal tnBalance)
        //{
        //    DataSet dsSearchAnnuitize = null;
        //    Database db = null;
        //    DbCommand SearchCommandWrapper = null;

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");

        //        if (db == null) return null;

        //        SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Ret_RetAnnuitizeWithExactAge");

        //        if (SearchCommandWrapper == null) return null;

        //        db.AddInParameter(SearchCommandWrapper, "@dRetireeBirthday", DbType.String, tdRetireeBirthDate);
        //        db.AddInParameter(SearchCommandWrapper, "@dRetirementDate", DbType.String, tdRetireDate);
        //        db.AddInParameter(SearchCommandWrapper, "@cAnnuityType", DbType.String, tcAnnuityType);
        //        db.AddInParameter(SearchCommandWrapper, "@cRetirementType", DbType.String, tcRetireType);
        //        db.AddInParameter(SearchCommandWrapper, "@cBasisType", DbType.String, tcBasisType);
        //        db.AddInParameter(SearchCommandWrapper, "@dBeneficiaryBirthday", DbType.String, tdBeneficiaryBirthDate);
        //        db.AddInParameter(SearchCommandWrapper, "@nBalance", DbType.String, tnBalance);
        //        SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
        //        dsSearchAnnuitize = new DataSet();
        //        dsSearchAnnuitize.Locale = CultureInfo.InvariantCulture;
        //        db.LoadDataSet(SearchCommandWrapper, dsSearchAnnuitize, "GetAnnuity");

        //        return dsSearchAnnuitize;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        SearchCommandWrapper.Dispose();
        //        SearchCommandWrapper = null;
        //        db = null;
        //    }
        //}

		public static DataSet getMetaAnnuityFactors(string tcRetireType, string tdRetireDate,string tdRetireeBirthDate,string tdBeneficiaryBirthDate)
		{
			DataSet dsMetaAnnuityFactors = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getMetaAnnuityFactors");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@cRetirementType",DbType.String,tcRetireType);
				db.AddInParameter(SearchCommandWrapper,"@dRetirementDate",DbType.String,tdRetireDate);
				db.AddInParameter(SearchCommandWrapper,"@dRetireeBirthday",DbType.String,tdRetireeBirthDate);
				if (tdBeneficiaryBirthDate == "")
				{
				}
				else
				{
					db.AddInParameter(SearchCommandWrapper,"@dBeneficiaryBirthday",DbType.String,tdBeneficiaryBirthDate);
				}
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsMetaAnnuityFactors = new DataSet();
				dsMetaAnnuityFactors.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsMetaAnnuityFactors,"GetMetaAnnuityFactors");
				
				return dsMetaAnnuityFactors;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
        ///ASHISH:2010.11.16:Added new method for YRS 5.0-1215
        public static DataSet getMetaAnnuityFactorsBeforeExactAgeEffDate(string tcRetireType, string tdRetireDate, string tdRetireeBirthDate, string tdBeneficiaryBirthDate)
        {
            DataSet dsMetaAnnuityFactors = null;
            Database db = null;
            DbCommand SearchCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getMetaAnnuityFactorsBeforeExactAgeEffDate");

                if (SearchCommandWrapper == null) return null;

                db.AddInParameter(SearchCommandWrapper, "@cRetirementType", DbType.String, tcRetireType);
                db.AddInParameter(SearchCommandWrapper, "@dRetirementDate", DbType.String, tdRetireDate);
                db.AddInParameter(SearchCommandWrapper, "@dRetireeBirthday", DbType.String, tdRetireeBirthDate);
                if (tdBeneficiaryBirthDate == "")
                {
                }
                else
                {
                    db.AddInParameter(SearchCommandWrapper, "@dBeneficiaryBirthday", DbType.String, tdBeneficiaryBirthDate);
                }
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                dsMetaAnnuityFactors = new DataSet();
                dsMetaAnnuityFactors.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(SearchCommandWrapper, dsMetaAnnuityFactors, "GetMetaAnnuityFactors");

                return dsMetaAnnuityFactors;
            }
            catch
            {
                throw;
            }
            finally
            {
                SearchCommandWrapper.Dispose();
                SearchCommandWrapper = null;
                db = null;
            }
        }
        //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
        public static DataSet getMetaAnnuityFactorsForDisability(string tcRetireType, string tdRetireDate, string tdRetireeBirthDate, string tdBeneficiaryBirthDate)
        {
            DataSet dsMetaAnnuityFactors = null;
            Database db = null;
            DbCommand SearchCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getMetaAnnuityFactorsExactAgeEffDate_Disability");

                if (SearchCommandWrapper == null) return null;

                db.AddInParameter(SearchCommandWrapper, "@cRetirementType", DbType.String, tcRetireType);
                db.AddInParameter(SearchCommandWrapper, "@dRetirementDate", DbType.String, tdRetireDate);
                db.AddInParameter(SearchCommandWrapper, "@dRetireeBirthday", DbType.String, tdRetireeBirthDate);
                if (tdBeneficiaryBirthDate == "")
                {
                }
                else
                {
                    db.AddInParameter(SearchCommandWrapper, "@dBeneficiaryBirthday", DbType.String, tdBeneficiaryBirthDate);
                }
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                dsMetaAnnuityFactors = new DataSet();
                dsMetaAnnuityFactors.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(SearchCommandWrapper, dsMetaAnnuityFactors, "GetMetaAnnuityFactorsDisability");

                return dsMetaAnnuityFactors;
            }
            catch
            {
                throw;
            }
            finally
            {
                SearchCommandWrapper.Dispose();
                SearchCommandWrapper = null;
                db = null;
            }
        }
		public static DataSet getSurvivorsInfo(string guiPersID)
		{
			DataSet dsSurvivorsInfo = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getSurvivorsInfo");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@guiPersID",DbType.String,guiPersID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsSurvivorsInfo = new DataSet();
				dsSurvivorsInfo.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSurvivorsInfo,"GetSurvivorsInfo");
				
				return dsSurvivorsInfo;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		public static DataSet getBeneficiaryMinMaxAge()
		{
			DataSet dsBeneficiaryMinMaxAge = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
			
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getBeneficiaryMinMaxAge");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
			
				dsBeneficiaryMinMaxAge = new DataSet();
				dsBeneficiaryMinMaxAge.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsBeneficiaryMinMaxAge,"BeneficiaryMinMaxAge");
				
				return dsBeneficiaryMinMaxAge;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		public static DataSet getDisabledBeneficiaryMinMaxAge()
		{
			DataSet dsDisabledBeneficiaryMinMaxAge = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getDisabled_BeneficiaryMinMaxAge");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
				
				dsDisabledBeneficiaryMinMaxAge = new DataSet();
				dsDisabledBeneficiaryMinMaxAge.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsDisabledBeneficiaryMinMaxAge,"DisabledBeneficiaryMinMaxAge");
				
				return dsDisabledBeneficiaryMinMaxAge;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		public static DataSet getBenAnnuityFactors(string tcBasis,string tcBeneficiary,string tcRetireType,int tnRetireeRetirementFactorAge,int tnBeneficiaryRetirementFactorAge,DateTime p_datetime_RetirementDate)
		{
			DataSet dsBenAnnuityFactors = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getBen_AnnuityFactors");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@PrePost",DbType.String,tcBasis);
				db.AddInParameter(SearchCommandWrapper,"@Beneficiary",DbType.String,tcBeneficiary);
				db.AddInParameter(SearchCommandWrapper,"@RetireType",DbType.String,tcRetireType);
				db.AddInParameter(SearchCommandWrapper,"@RetireeRetirementFactorAge",DbType.Int16,tnRetireeRetirementFactorAge);
				db.AddInParameter(SearchCommandWrapper,"@BeneficiaryRetirementFactorAge",DbType.Int16,tnBeneficiaryRetirementFactorAge);
				db.AddInParameter(SearchCommandWrapper,"@RetirementDate",DbType.Date,p_datetime_RetirementDate);
				
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsBenAnnuityFactors = new DataSet();
				dsBenAnnuityFactors.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsBenAnnuityFactors,"BenAnnuityFactors");
				
				return dsBenAnnuityFactors;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		
		public static DataSet getDBAnnuityOptions(DateTime p_datetime_RetirementDate)
		{
			DataSet dsDBAnnuityOptions = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_DBAnnuityOptions");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.AddInParameter(SearchCommandWrapper,"@RetirementDate",DbType.Date,p_datetime_RetirementDate);
				if (SearchCommandWrapper == null) return null;
				
				dsDBAnnuityOptions = new DataSet();
				dsDBAnnuityOptions.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsDBAnnuityOptions,"DBAnnuityOptions");
				return dsDBAnnuityOptions;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getVestedInfo(string SSNo,string RetirementDate)
		{
			DataSet dsVestedInfo = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_IsVested");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@SSNo",DbType.String,SSNo);
				db.AddInParameter(SearchCommandWrapper,"@RetirementDate",DbType.String,RetirementDate);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsVestedInfo = new DataSet();
				dsVestedInfo.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsVestedInfo,"IsVested");
				return dsVestedInfo;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getSSMetaConfigDetails()
		{
			DataSet dsSSMetaConfigDetails = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
			
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getSSMetaConfigDetails");
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;

				dsSSMetaConfigDetails = new DataSet();
				dsSSMetaConfigDetails.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSSMetaConfigDetails,"SSMetaConfigDetails");
			
				return dsSSMetaConfigDetails;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getSSEstBenefit(int Retiree_RetireYear,int Retiree_BirthYear, int Retiree_RetirementProjSalary)
		{
			DataSet dsSSEstBenefit = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getSSEstBenefit");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@Retiree_RetireYear",DbType.Int32,Retiree_RetireYear);
				db.AddInParameter(SearchCommandWrapper,"@Retiree_BirthYear",DbType.Int32,Retiree_BirthYear);
				db.AddInParameter(SearchCommandWrapper,"@Retiree_RetirementProjSalary",DbType.Int32,Retiree_RetirementProjSalary);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsSSEstBenefit = new DataSet();
				dsSSEstBenefit.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSSEstBenefit,"SSEstBenefit");
				return dsSSEstBenefit;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		public static DataSet getSSReductionFactor(string dtmbirthdate,string dtmProjectedRetDate)
		{
			DataSet dsSSReductionFactor = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_SSReduction");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@dtmbirthdate",DbType.String,dtmbirthdate);
				db.AddInParameter(SearchCommandWrapper,"@dtmProjectedRetDate",DbType.String,dtmProjectedRetDate);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				dsSSReductionFactor = new DataSet();
				dsSSReductionFactor.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSSReductionFactor,"SSReductionFactor");
				return dsSSReductionFactor;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet getMetaAnnuityTypes(DateTime p_datetime_RetirementDate)
		{
			DataSet dsMetaAnnuityTypes = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
			
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getMetaAnnuityTypes");				
				db.AddInParameter(SearchCommandWrapper,"@RetirementDate",DbType.Date,p_datetime_RetirementDate);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (SearchCommandWrapper == null) return null;
			
				dsMetaAnnuityTypes = new DataSet();
				dsMetaAnnuityTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsMetaAnnuityTypes,"getMetaAnnuityTypes");

				return dsMetaAnnuityTypes;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet LookUpMetaAnnuityTypes(DateTime p_datetime_RetirementDate)
		{
			DataSet dsLookUpMetaAnnuityType = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_MetaAnnuityType");
				db.AddInParameter(LookUpCommandWrapper,"@RetirementDate",DbType.Date,p_datetime_RetirementDate);
				LookUpCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (LookUpCommandWrapper == null) return null;

				dsLookUpMetaAnnuityType = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper,dsLookUpMetaAnnuityType,"dsMetaAnnuityTypes");

				return dsLookUpMetaAnnuityType;
			}
			catch
			{
				throw;
			}
			finally
			{
				LookUpCommandWrapper.Dispose();
				LookUpCommandWrapper=null;
				db = null;
			}
		}
		public static int getSafeHarborFactor(string RetireeBirthDate, string Benebirthdate, string RetireeRetireDate)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			int intSafeHarborFactor;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_Calc_GetSafeHarborFactor");
				
				if (SearchCommandWrapper == null) return 0;
				
				db.AddInParameter(SearchCommandWrapper,"@dRetireeBirthdate",DbType.String,RetireeBirthDate);
				db.AddInParameter(SearchCommandWrapper,"@dRetireDate",DbType.String,RetireeRetireDate);
				db.AddInParameter(SearchCommandWrapper,"@dBeneficiaryBirthdate",DbType.String,Benebirthdate);
				db.AddOutParameter(SearchCommandWrapper,"@nSafeHarborFactor",DbType.Int16,6);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				intSafeHarborFactor = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper,"@nSafeHarborFactor"));
				return intSafeHarborFactor;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet EligNoRefundsPending(string guiPersID)
		{
			DataSet dsEligNoRefundsPending = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_EligNoRefundsPending");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@varchar_PersId",DbType.String,guiPersID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 				
				dsEligNoRefundsPending = new DataSet();
				dsEligNoRefundsPending.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsEligNoRefundsPending,"EligNoRefundsPending");
				return dsEligNoRefundsPending;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet EligibleUnFundedTransactionExist(string guiFundEventID)
		{
			DataSet dsEligibleUnFundedTransactionExist = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_EligUnfundedTransExist");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@varchar_FundEventID",DbType.String,guiFundEventID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 					
				dsEligibleUnFundedTransactionExist = new DataSet();
				dsEligibleUnFundedTransactionExist.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsEligibleUnFundedTransactionExist,"EligibleUnFundedTransactionExist");
				return dsEligibleUnFundedTransactionExist;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet EligibleFundEvent(string EligibilityType, string Retiretype,string guiFundEventID)
		{
			DataSet dsEligibleFundEvent = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_eligibleFundEvent");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@varchar_EligibilityType",DbType.String,EligibilityType);
				db.AddInParameter(SearchCommandWrapper,"@varchar_RetireType",DbType.String,Retiretype);
				db.AddInParameter(SearchCommandWrapper,"@varchar_FundEventID",DbType.String,guiFundEventID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 				
				dsEligibleFundEvent = new DataSet();
				dsEligibleFundEvent.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsEligibleFundEvent,"EligibleFundEvent");
				return dsEligibleFundEvent;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static DataSet EligibleAlreadyRetired(string guiPersID)
		{
			DataSet dsEligibleAlreadyRetired = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_EligibleAlreadyRetired");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@varchar_PersId",DbType.String,guiPersID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 								
				dsEligibleAlreadyRetired = new DataSet();
				dsEligibleAlreadyRetired.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsEligibleAlreadyRetired,"EligibleAlreadyRetired");
				return dsEligibleAlreadyRetired;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static int getEligiblePIA(string FundEventID, bool llVestedOnRetireDate)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			int intEligiblePIA;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_EligiblePIAVestedPassed");
				
				if (SearchCommandWrapper == null) return 0;
	
				db.AddInParameter(SearchCommandWrapper,"@varchar_FundEventId",DbType.String,FundEventID);
				db.AddInParameter(SearchCommandWrapper,"@bit_Vested",DbType.Boolean,llVestedOnRetireDate);
				db.AddOutParameter(SearchCommandWrapper,"@int_Output",DbType.Int16,6);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				intEligiblePIA = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper,"@int_Output"));
				return intEligiblePIA;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static int EligibleIsQDRO(string FundEventID)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			int intEligibleIsQDRO;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_EligibleIsQDRO");
				
				if (SearchCommandWrapper == null) return 0;
	
				db.AddInParameter(SearchCommandWrapper,"@varchar_FundEventId",DbType.String,FundEventID);
				db.AddOutParameter(SearchCommandWrapper,"@int_Output",DbType.Int16,6);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				intEligibleIsQDRO = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper,"@int_Output"));
				return intEligibleIsQDRO;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static string EligibleAge(string PersId,string dtm_Date,bool bit_QDRO,string RetireType,bool bit_EligibleServiceMinMonths,bool bit_SSPlanPre1974,string FundEventId)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			string strEligibleAge;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_EligibleAge");
				
				if (SearchCommandWrapper == null) return null;
	
				db.AddInParameter(SearchCommandWrapper,"@varchar_PersId",DbType.String,PersId);
				db.AddInParameter(SearchCommandWrapper,"@dtm_Date",DbType.String,dtm_Date);
				db.AddInParameter(SearchCommandWrapper,"@bit_QDRO",DbType.Boolean,bit_QDRO);
				db.AddInParameter(SearchCommandWrapper,"@varchar_RetireType",DbType.String,RetireType);
				db.AddInParameter(SearchCommandWrapper,"@bit_EligibleServiceMinMonths",DbType.Boolean,bit_EligibleServiceMinMonths);
				db.AddInParameter(SearchCommandWrapper,"@bit_SSPlanPre1974",DbType.Boolean,bit_SSPlanPre1974);
				db.AddInParameter(SearchCommandWrapper,"@varchar_FundEventId",DbType.String,FundEventId);
				db.AddOutParameter(SearchCommandWrapper,"@char_Output",DbType.String,2);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				strEligibleAge = Convert.ToString(db.GetParameterValue(SearchCommandWrapper,"@char_Output"));
				return strEligibleAge;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
        //YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
        // START : SB | 03/08/2017 | YRS-AT-2625 | Adding additional parameter required to get the paid services 
        // public static int getWithdrawntransactionCheckForDisability(string FundEventID)
       
        public static DataSet getWithdrawntransactionCheckForDisability(string FundEventID, DateTime RetriementDate)
        {
            Database db = null;
            DbCommand SearchCommandWrapper = null;
            //START |  SR | 2017.03.20 | commented & rewritten below line of code to return table instead of output parameter
            //Boolean IsWithdrawalExist=false;
            DataSet withdrawalDetails = null;
            //END |  SR | 2017.03.20 | commented & rewritten below line of code to return table instead of output parameter
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                // SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_ValidateBasicActWithdrawnForDisability");  //SB | 03/08/2017 | YRS-AT-2625 | Old Procedure is replaced by new procedure to check paid serivces exist if withdrawal is taken
                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Ret_CheckPersonalSideWithdrawalExist");  //SB | 03/08/2017 | YRS-AT-2625 | New Procedure 
                if (SearchCommandWrapper == null) return null;

                //db.AddInParameter(SearchCommandWrapper, "@Varchar_tcFundEventID", DbType.String, FundEventID);   // Old Parameter names has been renamed in new procedure  --SB YRS-AT-2625
                db.AddInParameter(SearchCommandWrapper, "@VARCHAR_FundEventId", DbType.String, FundEventID);      // Old Parameter names has been renamed in new procedure  --SB YRS-AT-2625 
                db.AddInParameter(SearchCommandWrapper, "@DATETIME_RetirementDate", DbType.DateTime, RetriementDate);  // New Parameter added in new procedure  --SB YRS-AT-2625

                // db.AddOutParameter(SearchCommandWrapper, "@int_Output", DbType.Int16, 6);  // Old Parameter names has been renamed in new procedure  --SB YRS-AT-2625
                //db.AddOutParameter(SearchCommandWrapper, "@BIT_IsWithdrawalExists", DbType.Int16, 6);  // Old Parameter names has been renamed in new procedure  --SB YRS-AT-2625

                //START |  SR | 2017.03.20 | commented & rewritten below line of code to return table instead of output parameter
                //SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                //db.ExecuteNonQuery(SearchCommandWrapper);
                //// intTransactionCount = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper, "@int_Output"));   // Old Parameter names has been renamed in new procedure  --SB YRS-AT-2625
                //IsWithdrawalExist = Convert.ToBoolean(db.GetParameterValue(SearchCommandWrapper, "@BIT_IsWithdrawalExists"));   // Old Parameter names has been renamed in new procedure  --SB YRS-AT-2625
                //// END : SB | 03/08/2017 | YRS-AT-2625 | Adding additional parameter required to get the paid services 
                //return IsWithdrawalExist;              
                withdrawalDetails = new DataSet();
                withdrawalDetails.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(SearchCommandWrapper, withdrawalDetails, "WithdrawalDetails");
                return withdrawalDetails;
                //END |  SR | 2017.03.20 | commented & rewritten below line of code to return table instead of output parameter
            }
            catch
            {
                throw;
            }
            finally
            {
                SearchCommandWrapper.Dispose();
                SearchCommandWrapper = null;
                db = null;
            }
        }

		public static int getEligibleServiceMinMonths(string FundEventID, bool Bit_tlpaidOnly,string EligibilyType,string ProjectedRetDate)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			int intEligibleServiceMinMonths;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
	
				if (db == null) return 0;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_YCEligibleServiceMinMonths");

				if (SearchCommandWrapper == null) return 0;

				db.AddInParameter(SearchCommandWrapper,"@Varchar_tcFundEventID",DbType.String,FundEventID);
				db.AddInParameter(SearchCommandWrapper,"@Binary_tlpaidOnly",DbType.Boolean,Bit_tlpaidOnly);
				db.AddInParameter(SearchCommandWrapper,"@Varchar_tcEligibilyType",DbType.String,EligibilyType);
				db.AddInParameter(SearchCommandWrapper,"@DateTime_dtmProjectedRetDate",DbType.String,ProjectedRetDate);
				db.AddOutParameter(SearchCommandWrapper,"@int_Output",DbType.Int16,6);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				intEligibleServiceMinMonths = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper,"@int_Output"));
				return intEligibleServiceMinMonths;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}	
		public static DataSet EligibleSSPlanPre1974(string guiPersID)
		{
			DataSet dsEligibleSSPlanPre1974 = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_YCEligibleSSPlanPre1974");
				
				if (SearchCommandWrapper == null) return null;
				
				db.AddInParameter(SearchCommandWrapper,"@varchar_lcPersID",DbType.String,guiPersID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 												
				dsEligibleSSPlanPre1974 = new DataSet();
				dsEligibleSSPlanPre1974.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsEligibleSSPlanPre1974,"EligibleSSPlanPre1974");
				return dsEligibleSSPlanPre1974;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static int EstimateAge(string fundeventID,string RetirementDate)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			int intEstimateAge;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_YCEstimateAge");
				
				if (SearchCommandWrapper == null) return 0;
	
				db.AddInParameter(SearchCommandWrapper,"@varchar_fundeventID",DbType.String,fundeventID);
				db.AddInParameter(SearchCommandWrapper,"@lnRetirementDate",DbType.String,RetirementDate);
				db.AddOutParameter(SearchCommandWrapper,"@int_Output",DbType.Int16,2);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				intEstimateAge = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper,"@int_Output"));
				return intEstimateAge;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		public static int EligibleNotTerminatedWithinMonths(string FundEventID,string RetirementDate,string TerminationDate)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			int intEligibleNotTerminatedWithinMonths;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_YCEligibleNotTerminatedWithinMonthsofRetirmentDate");
				
				if (SearchCommandWrapper == null) return 0;
	
				db.AddInParameter(SearchCommandWrapper,"@DateTime_tdRetirementDate",DbType.String,RetirementDate);
				db.AddInParameter(SearchCommandWrapper,"@DateTime_tdTerminationDate",DbType.String,TerminationDate);
				db.AddInParameter(SearchCommandWrapper,"@guiFundEventID",DbType.String,FundEventID);
				db.AddOutParameter(SearchCommandWrapper,"@int_Output",DbType.Int16,2);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				intEligibleNotTerminatedWithinMonths = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper,"@int_Output"));
				return intEligibleNotTerminatedWithinMonths;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
        //2011.03.24    Sanket Vaidya        For YRS 5.0-1294,BT 794  : For disability requirement 
        public static DateTime GetLastTerminationDate(string FundEventID)
        {
            DateTime lastTermDate;
            Database db = null;
            DbCommand SearchCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return Convert.ToDateTime("1900-01-01");

                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetLastTerminationDate");

                if (SearchCommandWrapper == null) return Convert.ToDateTime("1900-01-01");

                db.AddInParameter(SearchCommandWrapper, "@FundEventID", DbType.String, FundEventID);
                db.AddOutParameter(SearchCommandWrapper, "@TerminationDate", DbType.Date, 10);
                //db.AddOutParameter(SearchCommandWrapper, "@TerminationDate", DbType.Date,Convert.ToDateTime("1900-01-01"));
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.ExecuteNonQuery(SearchCommandWrapper);
                lastTermDate = Convert.ToDateTime(db.GetParameterValue(SearchCommandWrapper, "@TerminationDate"));

            }
            catch 
            {                
                throw;
            }
            return lastTermDate;
        }

		/// <summary>
		/// Get data that checks if the retirement and termination of a participant are within 6 monhts of each other
		/// </summary>
		/// <param name="FundEventID"></param>
		/// <param name="RetirementDate"></param>
		/// <returns></returns>
		public static int EligibleNotTerminatedWithinMonths(string FundEventID, string RetirementDate)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			int eligibleNotTerminatedWithinMonths;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_EligibleNotTerminatedWithinMonthsofRetirmentDate");
				
				if (SearchCommandWrapper == null) return 0;
	
				db.AddInParameter(SearchCommandWrapper,"@RetirementDate", DbType.String, RetirementDate);
				db.AddInParameter(SearchCommandWrapper,"@FundEventID", DbType.String, FundEventID);
				db.AddOutParameter(SearchCommandWrapper,"@int_Output", DbType.Int16, 2);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				db.ExecuteNonQuery(SearchCommandWrapper);
				eligibleNotTerminatedWithinMonths = Convert.ToInt16(db.GetParameterValue(SearchCommandWrapper,"@int_Output"));
				return eligibleNotTerminatedWithinMonths;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		/// <summary>
		/// Gets the last date on which the daily interest was generated during the current month.
		/// </summary>
		///<returns>DateTime</returns>		
		public static DateTime getDailyInterestLog()
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			DateTime dt = Convert.ToDateTime("1/1/1900");
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return dt;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_GetDlyInterestLog");
				if (SearchCommandWrapper == null) return dt;

				DataSet dsLookupDailyIntLog = new DataSet();
				db.LoadDataSet(SearchCommandWrapper,dsLookupDailyIntLog,"DailyIntLog");
				
				DateTime LastRunDate = DateTime.Now.AddMonths(-1);
				
				string stringLastRunDate = LastRunDate.Month.ToString() + "/" + DateTime.DaysInMonth(LastRunDate.Year,LastRunDate.Month).ToString() + "/" + LastRunDate.Year.ToString(); 

				if (dsLookupDailyIntLog.Tables.Count > 0)
				{
					if (dsLookupDailyIntLog.Tables["DailyIntLog"].Rows.Count > 0)
					{
						if (dsLookupDailyIntLog.Tables["DailyIntLog"].Rows[0]["dtmLastRunDate"].ToString() != "")
						{
							stringLastRunDate = dsLookupDailyIntLog.Tables["DailyIntLog"].Rows[0]["dtmLastRunDate"].ToString(); 
						}						
					}
				}

				return Convert.ToDateTime(stringLastRunDate).Date;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper = null;
				db = null;
			}


			//yrs_usp_Est_GetDlyInterestLog
		}

		//Added by Ashish For Phase V part III changes ,start
		
		public static DataSet GetAnnuityBasisTypeList()
		{
			DataSet dsAnnuityBasisTypeList = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_GetAnnuityBasisTypeList");
				
				if (SearchCommandWrapper == null) return null;
				
				
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 												
				dsAnnuityBasisTypeList = new DataSet();
				
				db.LoadDataSet(SearchCommandWrapper, dsAnnuityBasisTypeList,"AnnuityBasisTypeList");
				return dsAnnuityBasisTypeList;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}

		//Added by Ashish For Phase V part III changes ,End

        //START: PPP | 05/06/2016 | YRS-AT-2683 | Fetching unfunded and not received amount
        public static DataSet GetPendingBalances(string strFundEventID, DateTime dtRetirementDate, DateTime? endWorkDate) // PPP | 03/01/2017 | YRS-AT-3317 | Added EndWork date parameter
        {
            DataSet ds = null;
            Database db = null;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_Ret_Est_GetPendingBalances");
                if (cmd == null) return null;

                db.AddInParameter(cmd, "@guiFundEventID", DbType.String, strFundEventID);
                db.AddInParameter(cmd, "@dtRetirementDate", DbType.DateTime, dtRetirementDate);

                //START: PPP | 03/01/2017 | YRS-AT-3317 | endWorkDate will represent termination date or user selected EndWorkDate in case of active employment
                if (endWorkDate != null)
                    db.AddInParameter(cmd, "@DATETIME_EndWorkDate", DbType.DateTime, endWorkDate.Value);
                //END: PPP | 03/01/2017 | YRS-AT-3317 | endWorkDate will represent termination date or user selected EndWorkDate in case of active employment

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                ds = new DataSet();
                ds.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(cmd, ds, new string[] { "RetireeNotReceivedAccountsBalanceInformation", "RetireeUnfundedAccountsBalanceInformation" });

                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                db = null;
            }
        }
        //END: PPP | 05/06/2016 | YRS-AT-2683 | Fetching unfunded and not received amount
        #endregion

		#region Retirement Purchase

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dsFedWithDrawal"></param>
		/// <param name="dsGenWithDrawal"></param>
		/// <param name="dsRetired"></param>
		/// <param name="dsNotes"></param>
		/// <param name="dsAnnuityJointSurvivors"></param>
		/// <param name="dtSelectedAnnuity"></param>
		/// <param name="fundEventId"></param>
		/// <param name="fundEventStatus"></param>
		/// <param name="PersId"></param>
		/// <param name="RetDate"></param>
		/// <param name="RetireeBirthDate"></param>
		/// <param name="RetTypeCode"></param>
		/// <param name="UserId"></param>
		/// <param name="yrsDeathBenefit"></param>
		/// <param name="yrsDeathBenefitUsed"></param>
		/// <param name="lnTaxable"></param>
		/// <param name="lnnonTaxable"></param>
		/// <param name="yrsSSDate"></param>
		/// <param name="yrsOption"></param>
		/// <param name="yrsOptionSav"></param>
		/// <param name="finalFundStatus"></param>
		/// <returns></returns>
		public bool Purchase(DataSet dsFedWithDrawal, DataSet dsGenWithDrawal, DataSet dsRetired,DataSet dsBeneficiaryAddress,
			DataSet dsNotes, DataSet dsAnnuityJointSurvivors, DataTable dtSelectedAnnuity, 
			string fundEventId, string fundEventStatus, string PersId, string RetDate, string RetireeBirthDate, 
			string RetTypeCode, string UserId, decimal yrsDeathBenefit, decimal yrsDeathBenefitUsed, decimal lnTaxable,
            decimal lnnonTaxable, decimal yrsSSReductionAmount, string yrsSSDate, string yrsSSDateSav, string yrsOption, string yrsOptionSav, string finalFundStatus, bool isPrePlanSplitRetirement,
            DataSet dsBeneficiariesSSN = null  // SR | 2016.08.02 | YRS-AT-2382 | Added a new parameter for Beneficiary SSN change auditing
            , YMCAObjects.StateWithholdingDetails objSTWPersonDetail = null // ML | 2019.11.26 | Yrs-AT- 4597| Added new paramater for StateTax Saving
            )
		{
			Database db = null;
			DbTransaction DBTransaction = null;          
           	DbConnection DBconnectYRS = null;
			bool l_bool_Fed_Valid = false;
            string planType = "";

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return false;
     
				
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction(IsolationLevel.ReadUncommitted);

				//Saving Retiree Beneficiary
				if (dsRetired != null)
				{
					if(dsRetired.Tables.Count > 0)
					{
                        if (dsRetired.Tables[0].Rows.Count > 0)
                        {
                          YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GeneratePhonySSN(dsRetired); //Manthan Rajguru | YRS-AT-2560 | Getting system generated phony SSN
                          InsertRetiredBeneficiaries(dsRetired, DBTransaction, db);
                        }
                    }                    
				}

                //Start:Anudeep:28.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                //Start:Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
                if (dsBeneficiaryAddress != null)
                {
                    if (dsBeneficiaryAddress.Tables.Count > 0 && dsRetired.Tables.Count > 0)
                    {
                        foreach (DataRow drAddress in dsBeneficiaryAddress.Tables[0].Rows)
                        {
                            if (!(String.IsNullOrEmpty(drAddress["NewId"].ToString())) && dsRetired.Tables[0].Rows.Count > 0)
                            {
                                DataRow[] drBeneRow;
                                drBeneRow = dsRetired.Tables[0].Select("NewId = '" + drAddress["NewId"].ToString() + "'");
                                if (drBeneRow.Length > 0)
                                {
                                    drAddress["BeneID"] = drBeneRow[0]["UniqueId"];
                                }
                            }
                        }
                        //Code Added by Dinesh Kanojia 20/08/2013
                        //Start - Manthan Rajguru | YRS-AT-2560 | Getting system genearted phony SSN
                        foreach (DataRow drAddress in dsBeneficiaryAddress.Tables[0].Rows)
                        {
                            if (!(String.IsNullOrEmpty(drAddress["BeneID"].ToString())) && dsRetired.Tables[0].Rows.Count > 0)
                            {
                                DataRow[] drBeneRow;
                                drBeneRow = dsRetired.Tables[0].Select("UniqueId = '" + drAddress["BeneID"].ToString() + "'");
                                if (drBeneRow.Length > 0)
                                {
                                    drAddress["BenSSNo"] = drBeneRow[0]["TaxID"];
                                }
                            }
                        }
                        //End - Manthan Rajguru | YRS-AT-2560 | Getting system genearted phony SSN
                        //Start Dinesh.k  20/08/2013    BT-2161: YRS 5.0-2191 - Trying to do a second retirement
                        AddressDAClass.SaveAddressOfBeneficiariesByPerson(dsBeneficiaryAddress, DBTransaction, db);
                    }
                    //AddressDAClass.SaveAddressOfBeneficiariesByPerson(dsBeneficiaryAddress, DBTransaction, db);
                    //End Dinesh.k  20/08/2013    BT-2161: YRS 5.0-2191 - Trying to do a second retirement
                }
                //End:Anudeep:28.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
                //End:Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
				//Saving Notes
				if (dsNotes != null)
				{
					if (dsNotes.Tables.Count>0)
					{
						if (dsNotes.Tables[0].Rows.Count>0)
						{
							InsertNotes(dsNotes,DBTransaction,db);
						}
					}
				}

				// Create retirement process id
				string retirementProcessID = Guid.NewGuid().ToString(); 
				//Saving General WithHolding
				// Save into Actual table only if fund status is not RT, RD or RA
				DataSet dsGenWithDrawalTemp;
				dsGenWithDrawalTemp = dsGenWithDrawal.Copy();
				//if (fundEventStatus == "DF" || "TM" || "XL" || "QD")
				//as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
				//if (fundEventStatus != "RT" && fundEventStatus != "RD" && fundEventStatus != "RP" && fundEventStatus != "RA")
				if (fundEventStatus != "RT" && fundEventStatus != "RPT" && fundEventStatus != "RD" && fundEventStatus != "RP" && fundEventStatus != "RA")
				{
					if (dsGenWithDrawal != null)
					{
						if (dsGenWithDrawal.Tables.Count>0)
						{
							if (dsGenWithDrawal.Tables[0].Rows.Count>0)
							{
								InsertRetireesGenWithdrawals(dsGenWithDrawal,DBTransaction,db);
							}
						}
					}
				}
				// Always insert records into GenWithTemporary table
				if ((dsGenWithDrawal != null) && (dsGenWithDrawal.Tables.Count>0) && (dsGenWithDrawal.Tables[0].Rows.Count>0))
				{
					// Update the new guid in the table
					dsGenWithDrawalTemp.Tables[0].Columns.Add("RetirementProcessID");
					foreach(DataRow dr in  dsGenWithDrawalTemp.Tables[0].Rows)
					{
						dr["RetirementProcessID"] = retirementProcessID;
					}

					InsertRetireesGenWithdrawalsTemp(dsGenWithDrawalTemp, DBTransaction, db);
				}

				

				//Saving Federal WithHolding
				// Save into Actual table only if fund status is not RT, RD or RA
				DataSet dsFedWithDrawalTemp;
				dsFedWithDrawalTemp = dsFedWithDrawal.Copy();
				//if (fundEventStatus == "DF" || "TM" || "XL" || "QD")
				//as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
				//if (fundEventStatus != "RT" && fundEventStatus != "RD" && fundEventStatus != "RP" && fundEventStatus != "RA")
				if (fundEventStatus != "RT" && fundEventStatus != "RPT" && fundEventStatus != "RD" && fundEventStatus != "RP" && fundEventStatus != "RA")
				{
					l_bool_Fed_Valid = false;
					if (dsFedWithDrawal != null)
					{
						if (dsFedWithDrawal.Tables.Count>0)
						{
							if(dsFedWithDrawal.Tables[0].Rows.Count>0)
							{
								l_bool_Fed_Valid = true;
								InsertRetireesFedWithdrawals(dsFedWithDrawal,DBTransaction,db);
							}
						}
					}
					//Since Federal WithHolding is compulsory for the purchase to complete it is being checked for.
					if(l_bool_Fed_Valid == false)
					{
						if (DBTransaction != null)
						{
							DBTransaction.Rollback();
						}
						return false;
					}
				}

				// Always insert records into FedWithTemporary table
				if ((dsFedWithDrawal != null) && (dsFedWithDrawal.Tables.Count>0) && (dsFedWithDrawal.Tables[0].Rows.Count>0))
				{
					// Update the new guid in the table
					dsFedWithDrawalTemp.Tables[0].Columns.Add("RetirementProcessID");
					foreach(DataRow dr in  dsFedWithDrawalTemp.Tables[0].Rows)
					{
						dr["RetirementProcessID"] = retirementProcessID;
					}

					InsertRetireesFedWithdrawalsTemp(dsFedWithDrawalTemp, DBTransaction, db);
				}

                //START :ML |YRS-AT-4597 |Save State Tax data
                if (objSTWPersonDetail != null )
                {
                    YMCARET.YmcaDataAccessObject.StateWithholdingDAClass.SavePersStateTaxdetails(objSTWPersonDetail, DBTransaction);
                }
                //END :ML |YRS-AT-4597 |Save State Tax data

				// Done 1 Get an unique id for the new retiree record.  
				// Done    If doesn't then get the existing one else get a new one. // Use a stored procedure to do it.
				// Done 2 For each row in dtAnnuity
				// Done 1. Save the benefeciary record 
				// Done 2. Save the annuity record get the annuityID and AnnuityAdjustmentID   
				//      3 Save Disbursement and Retiree record. // Pass the 3 annuityAdjId and 
				//      Test the complete routine 
				// Overtime : 4 hrs to find the bug in the system

				// Step 1 Get the retiree id
				string retireeID = getRetireeID(fundEventId,RetTypeCode);

				// Step 2.1 //Saving Annuity Joint Survivors
				dtSelectedAnnuity.Columns.Add("JointSurvivorId"); 
				if ((dsAnnuityJointSurvivors != null) && (dsAnnuityJointSurvivors.Tables.Count > 0)
					&& (dsAnnuityJointSurvivors.Tables[0].Rows.Count > 0))
				{
					if ((dtSelectedAnnuity.Select("planType = 'R'")).Length > 0)
					{
						saveAnnuityBenefeciaries(dtSelectedAnnuity, dsAnnuityJointSurvivors, "R", DBTransaction, db);
						
						if ((dtSelectedAnnuity.Select("planType = 'DB'")).Length > 0)
							saveAnnuityBenefeciaries(dtSelectedAnnuity, dsAnnuityJointSurvivors, "DB", DBTransaction, db);
					}
					
					if ((dtSelectedAnnuity.Select("planType = 'S'")).Length > 0)
						saveAnnuityBenefeciaries(dtSelectedAnnuity, dsAnnuityJointSurvivors, "S", DBTransaction, db);
				}
				//Step 2.2
				dtSelectedAnnuity.Columns.Add("AnnuityId"); 
				dtSelectedAnnuity.Columns.Add("AnnuityAdjustmentId"); 
				if ((dtSelectedAnnuity.Select("planType = 'R'")).Length > 0)					
				{
					saveAnnuity(dtSelectedAnnuity, "R", PersId, RetDate, RetTypeCode,
						UserId, yrsDeathBenefit, yrsDeathBenefitUsed, lnTaxable, 
						lnnonTaxable, yrsSSReductionAmount, yrsSSDate, yrsOption, fundEventId, retireeID, DBTransaction, db);
						
					if ((dtSelectedAnnuity.Select("planType = 'DB'")).Length > 0)
						saveAnnuity(dtSelectedAnnuity, "DB", PersId, RetDate, RetTypeCode,
							UserId, yrsDeathBenefit, yrsDeathBenefitUsed, lnTaxable, 
							lnnonTaxable, 0, yrsSSDate, yrsOption, fundEventId, retireeID, DBTransaction, db);
				}
					
				if ((dtSelectedAnnuity.Select("planType = 'S'")).Length > 0)
					saveAnnuity(dtSelectedAnnuity, "S", PersId, RetDate, RetTypeCode,
						UserId, yrsDeathBenefit, yrsDeathBenefitUsed, lnTaxable, 
						lnnonTaxable, yrsSSReductionAmount, yrsSSDateSav, yrsOptionSav, fundEventId, retireeID, DBTransaction, db);

				// Step 3
				string annAdjIDRet = string.Empty;
				string annAdjIDDB = string.Empty; 
				string annAdjIDSav = string.Empty;
				foreach(DataRow dr in dtSelectedAnnuity.Rows)
				{
					if (dr["planType"].ToString() == "R")
						annAdjIDRet  = dr["AnnuityAdjustmentId"].ToString();
					if (dr["planType"].ToString() == "DB")
						annAdjIDDB  = dr["AnnuityAdjustmentId"].ToString();
					if (dr["planType"].ToString() == "S")
						annAdjIDSav  = dr["AnnuityAdjustmentId"].ToString();
				}
				
				// Set the proper retirement type.
				string selectedPlanType = string.Empty;
				if (annAdjIDRet != string.Empty && annAdjIDSav != string.Empty)
					selectedPlanType = "B";
				else if (annAdjIDSav == string.Empty)
					selectedPlanType = "R";
				else
					selectedPlanType = "S";
				
				if (isPrePlanSplitRetirement)
					selectedPlanType = "B";

				RetirementDAClass.createDisbursementRecords(PersId, RetDate, RetireeBirthDate, RetTypeCode
					, lnTaxable, lnnonTaxable, fundEventId, retireeID, annAdjIDRet, annAdjIDDB, annAdjIDSav
					, retirementProcessID, finalFundStatus, selectedPlanType, DBTransaction, db);
				//2012-10-18	Priya P				YRS 5.0-1484 : Termination Watcher: commented SR code of termiantion watcher to undo release from 12.7.0 patch
                //SR:2012.08.06 :  BT-957/YRS 5.0-1484 : Termination Watcher
				if (selectedPlanType == "R")
				{
					planType = "RETIREMENT";
				}
				else if (selectedPlanType == "S")
                {//Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
					planType = "SAVINGS";
				}
				else
				{
					planType = "BOTH";
				}
                //Anudeep:30-10-2012 reverted Changes as per Observations 
				TerminationWatcherDA.UpdateTerminationWatcher(PersId, fundEventId, "Retirement", planType, DBTransaction, db);
                //End, SR:2012.08.06 :  BT-957/YRS 5.0-1484 : Termination Watcher
				//END 2012-10-18	Priya P				YRS 5.0-1484 : Termination Watcher: commented SR code of termiantion watcher to undo release from 12.7.0 patch

                //START: SR | 2016.08.02 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table
                if ((dsBeneficiariesSSN != null) && (dsBeneficiariesSSN.Tables.Count > 0) && (dsBeneficiariesSSN.Tables[0].Rows.Count > 0))
                {
                    RetirementDAClass.InsertBeneficiariesSSNChangeAuditRecord(dsBeneficiariesSSN, DBTransaction, db);
                }
                //END: SR | 2016.08.02 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table
                
				DBTransaction.Commit();



				return true;
			}
			catch
			{				
				if (DBTransaction != null)
					DBTransaction.Rollback();

				throw;
			}
			finally
			{
				if (DBconnectYRS != null)
					DBconnectYRS.Close();

				DBTransaction=null;
				DBconnectYRS=null;
				db = null;
			}
		}

		/// <summary>
		/// Get Retiree ID for the given FundEventID
		/// If exists, return existing one
		/// Else, return a new one.
		/// </summary>
		/// <param name="personID"></param>
        /// <param name="retireeType"></param>
		/// <returns>Retire ID</returns>
		private static string getRetireeID(string fundEventID,string retireeType)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			string retireeId;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return string.Empty;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetRetireeID");
				
				if (SearchCommandWrapper == null) return string.Empty;
				
				db.AddInParameter(SearchCommandWrapper,"@FundEventID", DbType.String, fundEventID);
                db.AddInParameter(SearchCommandWrapper, "@RetireeType", DbType.String, retireeType);
				db.AddOutParameter(SearchCommandWrapper,"@RetireeID", DbType.String, 36);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(SearchCommandWrapper);
				retireeId = Convert.ToString(db.GetParameterValue(SearchCommandWrapper,"@RetireeID"));
				return retireeId;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}


		public static void InsertRetireesGenWithdrawalsTemp(DataSet dsGenWithDrawals, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			try
			{
				if (dsGenWithDrawals != null)
				{
					foreach(DataRow dr in dsGenWithDrawals.Tables[0].Rows)
					{
						insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_InsertGeneralWithDrawalsTemp");
				
						db.AddInParameter(insertCommandWrapper,"@Persid", DbType.String, dr["PersID"].ToString()); 
						db.AddInParameter(insertCommandWrapper,"@Type", DbType.String, dr["Type"].ToString());
						db.AddInParameter(insertCommandWrapper,"@StartDate", DbType.String, dr["Start Date"].ToString());
						db.AddInParameter(insertCommandWrapper,"@Amount", DbType.Double, dr["Add'l Amount"].ToString());
						db.AddInParameter(insertCommandWrapper,"@EndDate", DbType.String, dr["End Date"].ToString());
						db.AddInParameter(insertCommandWrapper,"@RetirementProcessID", DbType.String, dr["RetirementProcessID"].ToString());
						
						insertCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
						db.ExecuteNonQuery(insertCommandWrapper, pDBTransaction);
					}
				}
			}
			catch 
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper=null;
			}
		}

		public static void InsertRetireesFedWithdrawalsTemp(DataSet dsFedWithDrawals, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			try
			{
				if (dsFedWithDrawals != null)
				{
					foreach(DataRow dr in dsFedWithDrawals.Tables[0].Rows)
					{
						insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_InsertFedWithdrawalsTemp");
				
						db.AddInParameter(insertCommandWrapper,"@Persid",DbType.String, dr["Persid"].ToString());
						db.AddInParameter(insertCommandWrapper,"@MaritalStatusCode", DbType.String, dr["Marital Status"].ToString());
						db.AddInParameter(insertCommandWrapper,"@Exemptions",DbType.Int32, dr["Exemptions"].ToString());
						db.AddInParameter(insertCommandWrapper,"@Amount",DbType.Double, dr["Add'l Amount"].ToString());
						db.AddInParameter(insertCommandWrapper,"@WithHoldingType",DbType.String, dr["Type"].ToString());
						db.AddInParameter(insertCommandWrapper,"@TaxEntityType",DbType.String, dr["Tax Entity"].ToString());
						db.AddInParameter(insertCommandWrapper,"@RetirementProcessID", DbType.String, dr["RetirementProcessID"].ToString());

						insertCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
						db.ExecuteNonQuery(insertCommandWrapper, pDBTransaction);
					}
				}
				
			}
			catch 
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper = null;
			}
		}

		/// <summary>
		/// This wrapper method is used to control the sequence in which the benefeciaries are saved.
		/// 1. Retirement
		/// 2. Death Benefit -- Update with Retirement one
		/// 3. Savings
		/// </summary>
		/// <param name="dtSelectedAnnuity"></param>
		/// <param name="planType"></param>
		private  void saveAnnuityBenefeciaries(DataTable dtSelectedAnnuity, DataSet dsAnnuityJointSurvivors, string planType, DbTransaction DBTransaction, Database db)
		{			
			DataRow drSelectedAnnuity = dtSelectedAnnuity.Select("planType = '" + planType + "'")[0];

			if(planType.Trim() != "DB")
			{
				DataRow[] drs = dsAnnuityJointSurvivors.Tables[0].Select("planType = '" + planType + "'");
				if (drs.Length > 0)
				{
					DataRow dr = drs[0];

					DataRow[] dRows = dsAnnuityJointSurvivors.Tables[0].Select("planType = '" + planType + "'");
					if (dRows.Length > 0)
					{
						DataSet dsBen =  dsAnnuityJointSurvivors.Copy();
						dsBen.Tables[0].Rows.Clear();
						dsBen.Tables[0].ImportRow(dRows[0]);
					
						string guiJointSurvivorId = string.Empty;
						InsertAnnuityJointSurvivors(dsBen, out guiJointSurvivorId, DBTransaction, db);
						drSelectedAnnuity["JointSurvivorId"] = guiJointSurvivorId;
					}
				}
			}
			else // For disability
			{
				DataRow drRet = dtSelectedAnnuity.Select("planType = 'R'")[0];
				drSelectedAnnuity["JointSurvivorId"] = drRet["JointSurvivorId"];
			}
			
			dtSelectedAnnuity.AcceptChanges();
		}

		
		private void saveAnnuity(DataTable dtSelectedAnnuity, string planType, string PersId, string RetDate, string RetTypeCode,
			string UserId, decimal yrsDeathBenefit, decimal yrsDeathBenefitUsed, decimal lnTaxable, 
			decimal lnnonTaxable, decimal yrsSSReductionAmount, string yrsSSDate, string yrsOption, string fundEventId, string retireeID, DbTransaction DBTransaction, Database db)
		{
			string annuityId = string.Empty;
			string annuityAdjustmentId = string.Empty;
			
			DataRow dr = dtSelectedAnnuity.Select("planType = '" + planType + "'")[0];
			// From the row fetch all the values					
			decimal mnyCurrentPayment = 0;
			decimal mnyPersonalPreTaxCurrentPayment = 0;
			decimal mnyPersonalPostTaxCurrentPayment = 0;
			decimal mnyYMCAPreTaxCurrentPayment = 0;
			decimal mnyYMCAPostTaxCurrentPayment = 0;
			decimal mnySSIncrease = 0;
			decimal mnySSDecrease = 0;
			//decimal yrsSSReductionAmount = 0;
			decimal mnySurvivorRetiree = 0;
			bool bitSSLeveling = false;
			string yrsOptionDB = string.Empty;
			string plantType = string.Empty;
			string jointSurvivorId = string.Empty;
			bool deathBenefitFlag = false;

			mnyCurrentPayment = Math.Round(Convert.ToDecimal(dr["mnyCurrentPayment"]), 2);
			mnyPersonalPreTaxCurrentPayment = Math.Round(Convert.ToDecimal(dr["mnyPersonalPreTaxCurrentPayment"]), 2);
			mnyPersonalPostTaxCurrentPayment = Math.Round(Convert.ToDecimal(dr["mnyPersonalPostTaxCurrentPayment"]), 2);
			mnyYMCAPreTaxCurrentPayment = Math.Round(Convert.ToDecimal(dr["mnyYmcaPreTaxCurrentPayment"]), 2);
			mnyYMCAPostTaxCurrentPayment = Math.Round(Convert.ToDecimal(dr["mnyYmcaPostTaxCurrentPayment"]), 2);
			mnySSIncrease = Math.Round(Convert.ToDecimal(dr["mnySSIncrease"]), 2);
			mnySSDecrease = Math.Round(Convert.ToDecimal(dr["mnySSDecrease"]), 2);
			mnySurvivorRetiree = Math.Round(Convert.ToDecimal(dr["mnySurvivorRetiree"]), 2);
			bitSSLeveling = (dr["bitSSLeveling"].ToString() == "True" ? true : false);
			yrsOptionDB = dr["chrDBAnnuityType"].ToString();
			plantType = dr["planType"].ToString();
			jointSurvivorId = dr["JointSurvivorId"].ToString();
			deathBenefitFlag = (plantType  == "DB");

            decimal reserveReductionPercent = Math.Round(Convert.ToDecimal(dr["mnyReserveReductionPercent"]), 10); //PPP | 03/09/2017 | YRS-AT-2625 | Fetching mnyReserveReductionPercent value

			// If it is a death benefit then get the Retirement AnnuityID
			if(deathBenefitFlag)
				annuityId = (dtSelectedAnnuity.Select("planType = 'R'")[0])["AnnuityId"].ToString();
					
			// Call the SP and save annuity
			RetirementDAClass.createAnnuityRecords(PersId, RetDate, RetTypeCode, UserId, mnyCurrentPayment, 
				mnyPersonalPreTaxCurrentPayment, mnyPersonalPostTaxCurrentPayment, mnyYMCAPreTaxCurrentPayment, 
				mnyYMCAPostTaxCurrentPayment, mnySSIncrease, yrsDeathBenefit, yrsDeathBenefitUsed, lnTaxable, 
				lnnonTaxable, yrsSSReductionAmount, yrsSSDate, bitSSLeveling, yrsOption, yrsOptionDB,
                mnySurvivorRetiree, jointSurvivorId, fundEventId, retireeID, annuityId, plantType, reserveReductionPercent, //PPP | 03/09/2017 | YRS-AT-2625 | Passing reserveReductionPercent value
				out annuityId, out annuityAdjustmentId, deathBenefitFlag, DBTransaction, db);
					
			// Save the Annuity and AnnuityAdjustmentID
			dr["AnnuityId"] = annuityId;
			dr["AnnuityAdjustmentId"] = annuityAdjustmentId;
		}


		//Saving Annuity Joint Survivors
		private static void InsertAnnuityJointSurvivors(DataSet p_dataset_AnnuityJointSurvivors,out string p_string_guiJointSurvivorId, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_RP_InsertAnnuityJointSurvivors");

				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@chrSSNO",DbType.String,"chrSSNO",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@chvLastName",DbType.String,"chvLastName",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@chvFirstName",DbType.String,"chvFirstName",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@chvMiddleName",DbType.String,"chvMiddleName",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@dtmBirthDate",DbType.DateTime,"dtmBirthDate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@dtmDeathDate",DbType.DateTime,"dtmDeathDate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bitSpouse",DbType.Boolean,"bitSpouse",DataRowVersion.Current);

				db.AddOutParameter(insertCommandWrapper,"@out_guiJointSurvivorID",DbType.String,50);
				
				p_string_guiJointSurvivorId="";
				
				if (p_dataset_AnnuityJointSurvivors != null)
				{
					db.UpdateDataSet(p_dataset_AnnuityJointSurvivors,"AnnuityJointSurvivors",insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);
					p_string_guiJointSurvivorId = db.GetParameterValue(insertCommandWrapper,"@out_guiJointSurvivorID").ToString();
				}
			}
			catch
			{
				throw;
			}		
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper = null;

				updateCommandWrapper = null;
				deleteCommandWrapper = null;
			}
		}

		public static void InsertNotes(DataSet p_dataset_Notes, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYMCANotes");

				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@varchar_guiUniqueID",DbType.Guid, "UniqueId",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_PersonID",DbType.Guid, "PersonId",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_NoteTypeCode",DbType.String,1);
				db.AddInParameter(insertCommandWrapper,"@text_Note",DbType.String,"Note",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Important",DbType.Boolean,"bitImportant",DataRowVersion.Current);

				if (p_dataset_Notes != null)
				{
					db.UpdateDataSet(p_dataset_Notes,"Member Notes",insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper=null;

				updateCommandWrapper=null;
				deleteCommandWrapper=null;
			}
		}


		private static bool createAnnuityRecords(string PersId,string RetDate,string RetTypeCode,string UserId,
			decimal mnyCurrentPayment, decimal mnyPersonalPreTaxCurrentPayment,decimal mnyPersonalPostTaxCurrentPayment,
			decimal mnyYMCAPreTaxCurrentPayment,decimal mnyYMCAPostTaxCurrentPayment, decimal mnySSIncrease, 
			decimal yrsDeathBenefit,decimal yrsDeathBenefitUsed,decimal lnTaxable,
			decimal lnnonTaxable,decimal yrsSSReductionAmount,string yrsSSDate,
			bool bitSSLeveling,string yrsOption,string yrsOptionDB,
			decimal mnySurvivorRetiree, string p_string_guiJointSurvivorId,string l_string_FundEventId,
            string retireeID, string annuityIDPrev, string plantType, decimal reserveReductionPercent, out string annuityId, out string annuityAdjustmentId, bool deathBenefitFlag,
            DbTransaction pDBTransaction, Database db) //PPP | 03/09/2017 | YRS-AT-2625 | Added reserveReductionPercent parameter
		{
			DbCommand InsertCommandWrapper = null;
			try
			{
				InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_CreateAnnuity");
				
				db.AddInParameter(InsertCommandWrapper,"@PersId",DbType.String,PersId);
				db.AddInParameter(InsertCommandWrapper,"@RetDate",DbType.String,RetDate);
				db.AddInParameter(InsertCommandWrapper,"@RetTypeCode",DbType.String,RetTypeCode);
				db.AddInParameter(InsertCommandWrapper,"@DeathBenefit", DbType.Boolean, deathBenefitFlag);
				db.AddInParameter(InsertCommandWrapper,"@mnyCurrentPayment",DbType.Decimal,mnyCurrentPayment);
				db.AddInParameter(InsertCommandWrapper,"@mnyPersonalPreTaxCurrentPayment",DbType.Decimal,mnyPersonalPreTaxCurrentPayment);
				db.AddInParameter(InsertCommandWrapper,"@mnyPersonalPostTaxCurrentPayment",DbType.Decimal,mnyPersonalPostTaxCurrentPayment);
				db.AddInParameter(InsertCommandWrapper,"@mnyYMCAPreTaxCurrentPayment",DbType.Decimal,mnyYMCAPreTaxCurrentPayment);
				db.AddInParameter(InsertCommandWrapper,"@mnyYMCAPostTaxCurrentPayment",DbType.Decimal,mnyYMCAPostTaxCurrentPayment);
				db.AddInParameter(InsertCommandWrapper,"@mnySSIncrease",DbType.Decimal,mnySSIncrease);
				db.AddInParameter(InsertCommandWrapper,"@mnySurvivorRetiree",DbType.Decimal,mnySurvivorRetiree);
				
				db.AddInParameter(InsertCommandWrapper,"@yrsDeathBenefit",DbType.Decimal,yrsDeathBenefit);
				db.AddInParameter(InsertCommandWrapper,"@yrsDeathBenefitUsed",DbType.Decimal,yrsDeathBenefitUsed);
				db.AddInParameter(InsertCommandWrapper,"@yrsSSReductionAmount",DbType.Decimal,yrsSSReductionAmount);
				db.AddInParameter(InsertCommandWrapper,"@yrsSSDate",DbType.String,yrsSSDate);

				db.AddInParameter(InsertCommandWrapper,"@bitSSLeveling",DbType.Boolean,bitSSLeveling);
				db.AddInParameter(InsertCommandWrapper,"@yrsOption",DbType.String,yrsOption);
				db.AddInParameter(InsertCommandWrapper,"@yrsOptionDB",DbType.String,yrsOptionDB);
				
				db.AddInParameter(InsertCommandWrapper,"@FundEventId",DbType.String,l_string_FundEventId);
				db.AddInParameter(InsertCommandWrapper,"@yrsJointSurvivorId",DbType.String,p_string_guiJointSurvivorId);
				
				db.AddInParameter(InsertCommandWrapper,"@RetireeIDMain", DbType.String, retireeID);
				db.AddInParameter(InsertCommandWrapper,"@RetireAnnuityID", DbType.String, annuityIDPrev);
				db.AddInParameter(InsertCommandWrapper,"@planType", DbType.String, plantType );

                //START: PPP | 03/09/2017 | YRS-AT-2625 | @ReserveReductionPercent will be 1 for all Normal and Disability annuities, except "C" annuity calculated for Disability, So there is no change in behaviour of code output for them
                db.AddInParameter(InsertCommandWrapper, "@ReserveReductionPercent", DbType.Decimal, reserveReductionPercent);
                //END: PPP | 03/09/2017 | YRS-AT-2625 | @ReserveReductionPercent will be 1 for all Normal and Disability annuities, except "C" annuity calculated for Disability, So there is no change in behaviour of code output for them

				db.AddOutParameter(InsertCommandWrapper,"@OutRetireAnnuityID", DbType.String, 50);
				db.AddOutParameter(InsertCommandWrapper,"@OutAnnuityAdjId", DbType.String, 50);

				InsertCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(InsertCommandWrapper,pDBTransaction);

				annuityId = db.GetParameterValue(InsertCommandWrapper,"@OutRetireAnnuityID").ToString();
				annuityAdjustmentId = db.GetParameterValue(InsertCommandWrapper,"@OutAnnuityAdjId").ToString();

				return true;
			}
			catch
			{
				throw;
			}
			finally
			{
				InsertCommandWrapper.Dispose();
				InsertCommandWrapper = null;
			}
		}


		private static bool createDisbursementRecords(string PersId,string RetDate, string RetireeBirthDate, string RetTypeCode, 
			decimal lnTaxable, decimal lnnonTaxable, string fundEventId, string retireeID, string AnnAdjIDRet, 
			string AnnAdjIDDB, string AnnAdjIDSav, string retirementProcessID, string finalFundStatus, 
			string selectedPlanType, DbTransaction pDBTransaction, Database db)
		{
			DbCommand InsertCommandWrapper = null;
			try
			{
				InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_CreateDisbursementAndRetire");
				
				db.AddInParameter(InsertCommandWrapper,"@PersId", DbType.String, PersId);
				db.AddInParameter(InsertCommandWrapper,"@RetDate", DbType.String, RetDate);
				db.AddInParameter(InsertCommandWrapper,"@RetireeBirthDate", DbType.String, RetireeBirthDate);
				db.AddInParameter(InsertCommandWrapper,"@RetTypeCode", DbType.String, RetTypeCode);				
				db.AddInParameter(InsertCommandWrapper,"@lnTaxable", DbType.Decimal, lnTaxable);
				db.AddInParameter(InsertCommandWrapper,"@lnnonTaxable", DbType.Decimal,lnnonTaxable);			
				db.AddInParameter(InsertCommandWrapper,"@FundEventId", DbType.String, fundEventId);
				db.AddInParameter(InsertCommandWrapper,"@RetireeId", DbType.String, retireeID);
				db.AddInParameter(InsertCommandWrapper,"@AnnuityAdjIdRetP", DbType.String, AnnAdjIDRet);
				db.AddInParameter(InsertCommandWrapper,"@AnnuityAdjIdDBP", DbType.String, AnnAdjIDDB);
				db.AddInParameter(InsertCommandWrapper,"@AnnuityAdjIdSavP", DbType.String, AnnAdjIDSav);
				db.AddInParameter(InsertCommandWrapper,"@RetirementProcessID", DbType.String, retirementProcessID);
				db.AddInParameter(InsertCommandWrapper,"@FinalFundStatus", DbType.String, finalFundStatus);
				db.AddInParameter(InsertCommandWrapper,"@PlanType", DbType.String, selectedPlanType);
				
				InsertCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.ExecuteNonQuery(InsertCommandWrapper, pDBTransaction);				

				return true;
			}
			catch
			{
				throw;
			}
			finally
			{
				InsertCommandWrapper.Dispose();
				InsertCommandWrapper = null;
			}
		}

		

		public static void InsertRetiredBeneficiaries(DataSet dsBeneficiaries, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
            string strNewUniqueId;

			try
			{
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_InsertRetiredBeneficiaries");

                foreach (DataRow dr in dsBeneficiaries.Tables[0].Rows)
                {
                    //Anudeep:12.07.2013 : Bt-1501:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (dr.RowState == DataRowState.Added || string.IsNullOrEmpty(dr["UniqueID"].ToString()))
                        {
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryName1", DbType.String, "Name", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryName2", DbType.String, "Name2", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@RelationShipCode", DbType.String, "Rel", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryGroupCode", DbType.String, "Groups", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, "TaxID", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryLevelCode", DbType.String, "Lvl", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BenefitPCTG", DbType.Double, "Pct", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BirthDate", DbType.String, "Birthdate", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, "PersId", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryTypeCode", DbType.String, "BeneficiaryTypeCode", DataRowVersion.Current);
                            //insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                            insertCommandWrapper.Parameters.Clear();
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryName1", DbType.String, dr["Name"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryName2", DbType.String, dr["Name2"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@RelationShipCode", DbType.String, dr["Rel"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryGroupCode", DbType.String, dr["Groups"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, dr["TaxID"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryLevelCode", DbType.String, dr["Lvl"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@BenefitPCTG", DbType.Double, Convert.ToDouble(dr["Pct"].ToString().Trim()));
                            db.AddInParameter(insertCommandWrapper, "@BirthDate", DbType.String, dr["Birthdate"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, dr["PersId"].ToString().Trim());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryTypeCode", DbType.String, dr["BeneficiaryTypeCode"].ToString().Trim());

                            //SP 2014.12.01 BT-2310\YRS 5.0-2255 - Start
                            db.AddInParameter(insertCommandWrapper, "@RepFirstName", DbType.String, dr["RepFirstName"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepLastName", DbType.String, dr["RepLastName"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepSalutationCode", DbType.String, dr["RepSalutation"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepTelephoneNo", DbType.String, dr["RepTelephone"].ToString());
                            //SP 2014.12.01 BT-2310\YRS 5.0-2255 - End

                            db.AddOutParameter(insertCommandWrapper, "@stringUniqueID", DbType.String, 1000);
                            insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                            db.ExecuteNonQuery(insertCommandWrapper, pDBTransaction);
                            strNewUniqueId = db.GetParameterValue(insertCommandWrapper, "@stringUniqueID").ToString();
                            dr["UniqueID"] = strNewUniqueId;
                            dr.AcceptChanges();
                        }
                    }
                }

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMP_UpdateRetiredBeneficiaries");

                db.AddInParameter(updateCommandWrapper, "@BeneficiaryName1", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryName2", DbType.String, "Name2", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RelationShipCode", DbType.String, "Rel", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryGroupCode", DbType.String, "Groups", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, "TaxID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryLevelCode", DbType.String, "Lvl", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BenefitPCTG", DbType.Double, "Pct", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BirthDate", DbType.String, "Birthdate", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@stringPersid", DbType.String , "PersId", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@stringUniqueID", DbType.String , "UniqueID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryType", DbType.String, "BeneficiaryTypeCode", DataRowVersion.Current);

                //SP 2014.12.01 BT-2310\YRS 5.0-2255 - Start
                db.AddInParameter(updateCommandWrapper, "@RepFirstName", DbType.String, "RepFirstName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepLastName", DbType.String, "RepLastName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepSalutationCode", DbType.String, "RepSalutation", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepTelephoneNo", DbType.String, "RepTelephone", DataRowVersion.Current);
                //SP 2014.12.01 BT-2310\YRS 5.0-2255 - End

                //deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeleteBeneficiaries");
                deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addrs_DeleteBeneficiary");
                db.AddInParameter(deleteCommandWrapper, "@guiBeneficiaryId", DbType.String, "UniqueID", DataRowVersion.Original);

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (dsBeneficiaries != null)
				{
                    db.UpdateDataSet(dsBeneficiaries, "RetiredBeneficiaries", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, pDBTransaction);
                    dsBeneficiaries.AcceptChanges();
				}				
			}
			catch 
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper = null;

				updateCommandWrapper.Dispose();
				updateCommandWrapper = null;

				deleteCommandWrapper.Dispose();
				deleteCommandWrapper = null;
			}
		}
		public static void InsertBeneficiaries(DataSet dsBeneficiaries, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_InsertActiveBeneficiaries");
				
				db.AddInParameter(insertCommandWrapper,"@BeneficiaryName1",DbType.String,"Name",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@BeneficiaryName2",DbType.String,"Name2",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@RelationShipCode",DbType.String,"Rel",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@BeneficiaryGroupCode",DbType.String,"Groups",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@BeneficiaryTaxNumber",DbType.String,"TaxID",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@BeneficiaryLevelCode",DbType.String,"Lvl",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@BenefitPCTG",DbType.Double,"Pct",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@BirthDate",DbType.String,"Birthdate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Persid",DbType.Guid,"PersId",DataRowVersion.Current);

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMP_UpdateActiveBeneficiaries");
				
				db.AddInParameter(updateCommandWrapper,"@BeneficiaryName1",DbType.String,"Name",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@BeneficiaryName2",DbType.String,"Name2",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@RelationShipCode",DbType.String,"Rel",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@BeneficiaryGroupCode",DbType.String,"Groups",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@BeneficiaryTaxNumber",DbType.String,"TaxID",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@BeneficiaryLevelCode",DbType.String,"Lvl",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@BenefitPCTG",DbType.Double,"Pct",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@BirthDate",DbType.String,"Birthdate",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@stringPersid",DbType.String ,"PersId",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@stringUniqueID",DbType.Guid,"UniqueID",DataRowVersion.Current);
				
				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_DeleteBeneficiaries");
				db.AddInParameter(deleteCommandWrapper,"@UniqueID",DbType.String,"UniqueID",DataRowVersion.Original);
				
				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (dsBeneficiaries != null)
				{
					db.UpdateDataSet(dsBeneficiaries,"ActiveBeneficiaries" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);					
				}				
			}
			catch 
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper = null;
				
				updateCommandWrapper.Dispose();
				updateCommandWrapper = null;

				deleteCommandWrapper.Dispose();
				deleteCommandWrapper = null;			
			}
		}

		public static void InsertRetireesFedWithdrawals(DataSet dsFedWithDrawals, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_FP_InsertFedWithdrawals");
				
				db.AddInParameter(insertCommandWrapper,"@Persid",DbType.Guid,"Persid",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@MaritalStatusCode",DbType.String,"Marital Status",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Exemptions",DbType.Int32,"Exemptions",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Amount",DbType.Double,"Add'l Amount",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@WithHoldingType",DbType.String ,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@TaxEntityType",DbType.String,"Tax Entity",DataRowVersion.Current);

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_UpdateFedtaxWithholding");
				
				db.AddInParameter(updateCommandWrapper,"@MaritalStatusCode",DbType.String,"Marital Status",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@Exemptions",DbType.Int32,"Exemptions",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@Amount",DbType.Decimal,"Add'l Amount",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@WithHoldingType",DbType.String ,"Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@TaxEntityType",DbType.String,"Tax Entity",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@UniqueID",DbType.Guid,"FedWithdrawalID",DataRowVersion.Current);
				
				if (dsFedWithDrawals != null)
				{
					db.UpdateDataSet(dsFedWithDrawals,"FedWithDrawals" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);					
				}				
			}
			catch 
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper=null;

				updateCommandWrapper.Dispose();
				updateCommandWrapper = null;
			}
		}

		public static void InsertRetireesGenWithdrawals(DataSet dsGenWithDrawals, DbTransaction pDBTransaction, Database db)
		{
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_FP_InsertGeneralWithDrawals");
				
				db.AddInParameter(insertCommandWrapper,"@Persid",DbType.Guid,"PersID",DataRowVersion.Current); 
				db.AddInParameter(insertCommandWrapper,"@Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@StartDate",DbType.String,"Start Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@Amount",DbType.Double,"Add'l Amount",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@EndDate",DbType.String,"End Date",DataRowVersion.Current);
				
				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_FP_UpdateGeneralWithDrawals");
				
				db.AddInParameter(updateCommandWrapper,"@Uniqueid",DbType.Guid ,"GenWithdrawalID",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@Persid",DbType.Guid,"PersID",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@Amount",DbType.Double,"Add'l Amount",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@StartDate",DbType.String,"Start Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@EndDate",DbType.String,"End Date",DataRowVersion.Current);
				
				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (dsGenWithDrawals != null)
				{
					db.UpdateDataSet(dsGenWithDrawals,"GeneralWithDrawals" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,pDBTransaction);					
				}				
			}
			catch 
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper = null;

				updateCommandWrapper.Dispose();
				updateCommandWrapper = null;

				deleteCommandWrapper = null;		
			}
		}

		public static DataSet GetActiveBeneficiaries(string persId)
		{
			DataSet dsActiveBeneficiaries = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");			
				if (db == null) return null;
				
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetActiveBeneficiaries");
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@PersId", DbType.String, persId);
				
				dsActiveBeneficiaries = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, dsActiveBeneficiaries, "ActiveBeneficiaries");
				
				return dsActiveBeneficiaries;
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet GetJointAnnuitySurvivors(string persId)
		{
			DataSet dsJointSurvivors = null;
			Database db = null;
			DbCommand commandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");			
				if (db == null) return null;
				
				commandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetJointAnnuitySurvivors");
				if (commandWrapper == null) return null;
				db.AddInParameter(commandWrapper,"@PersId", DbType.String, persId);
				
				dsJointSurvivors = new DataSet();
				db.LoadDataSet(commandWrapper, dsJointSurvivors, "JointSurvivors");
				
				return dsJointSurvivors;
			}
			catch 
			{
				throw;
			}
		}

		#endregion

		#region InsertPRA_ReportValues
		public static string InsertPRA_ReportValues(string PersID,int AgeRetired,

			string BenBirthDate,
			string BeneficiaryName,

			string PRAAssumption,
			string RetBirthDate,

			string RetiredDate,
			decimal RetiredDeathBen,

			decimal C_Insured,
			decimal C_Monthly,
			decimal C_Reduction,

			decimal C62_Insured,
			decimal C62_Monthly,
			decimal C62_Reduction,
			decimal CS_Insured,
			decimal CS_Monthly,
			decimal CS_Reduction,


			decimal J1_Retiree,
			decimal J1_Survivor,
			decimal J162_Retiree,
			decimal J162_Survivor,
			decimal J1I_Retiree,
			decimal J1I_Survivor,
				
				
			decimal J1P_Retiree,
			decimal J1P_Survivor,
			decimal J1P62_Retiree,
			decimal J1P62_Survivor,
			decimal J1PS_Retiree,
				
			decimal J1PS_Survivor,
			decimal J1S_Retiree,
			decimal J1S_Survivor,
			decimal J5_Retiree,
			decimal J5_Survivor,

			decimal J562_Retiree,
			decimal J562_Survivor,
			decimal J5I_Retiree,
			decimal J5I_Survivor,
			decimal J5L_Retiree,
				
			decimal J5L_Survivor,
			decimal J5L62_Retiree,
			decimal J5L62_Survivor,
			decimal J5LS_Retiree,
			decimal J5LS_Survivor,				


			decimal J5P_Retiree,
			decimal J5P_Survivor,
			decimal J5P62_Retiree,
			decimal J5P62_Survivor,
			decimal J5PS_Retiree,				

			decimal J5PS_Survivor,
			decimal J5S_Retiree,
			decimal J5S_Survivor,
			decimal J7_Retiree,
			decimal J7_Survivor,
				
			decimal J762_Retiree,
			decimal J762_Survivor,
			decimal J7I_Retiree,
			decimal J7I_Survivor,
			decimal J7L_Retiree,	
			

			decimal J7L_Survivor,
			decimal J7L62_Retiree,
			decimal J7L62_Survivor,
			decimal J7LS_Retiree,
			decimal J7LS_Survivor,				

			decimal J7P_Retiree,
			decimal J7P_Survivor,
			decimal J7P62_Retiree,
			decimal J7P62_Survivor,
			decimal J7PS_Retiree,				

			decimal J7PS_Survivor,
			decimal J7S_Retiree,
			decimal J7S_Survivor,
			decimal M_Retiree,
			decimal M62_Retiree,				
			decimal MI_Retiree,
			decimal MS_Retiree,


			decimal ZC_Annually,
			decimal ZC62_Annually,
			decimal ZCS_Annually,
			decimal ZJ1_Retiree,
			decimal ZJ1_Survivor,				
			decimal ZJ162_Retiree,
			decimal ZJ162_Survivor,

			decimal ZJ1I_Retiree,
			decimal ZJ1I_Survivor,
			decimal ZJ1P_Retiree,
			decimal ZJ1P_Survivor,
			decimal ZJ1P62_Retiree,				
			decimal ZJ1P62_Survivor,
			decimal ZJ1PS_Retiree,


			decimal ZJ1PS_Survivor,
			decimal ZJ1S_Retiree,
			decimal ZJ1S_Survivor,
			decimal ZJ5_Retiree,
			decimal ZJ5_Survivor,				
			decimal ZJ562_Retiree,
			decimal ZJ562_Survivor,


			decimal ZJ5I_Retiree,
			decimal ZJ5I_Survivor,
			decimal ZJ5L_Retiree,
			decimal ZJ5L_Survivor,

			decimal ZJ5L62_Retiree,				
			decimal ZJ5L62_Survivor,
			decimal ZJ5LS_Retiree,

			decimal ZJ5LS_Survivor,				
			decimal ZJ5P_Retiree,
			decimal ZJ5P_Survivor,

			decimal ZJ5P62_Retiree,				
			decimal zJ5P62_Survivor,
			decimal ZJ5PS_Retiree,


			decimal ZJ5PS_Survivor,				
			decimal ZJ5S_Retiree,
			decimal ZJ5S_Survivor,

			decimal ZJ7_Retiree,				
			decimal ZJ7_Survivor,
			decimal ZJ762_Retiree,

			decimal ZJ762_Survivor,				
			decimal ZJ7I_Retiree,
			decimal ZJ7I_Survivor,

			decimal ZJ7L_Retiree,				
			decimal ZJ7L_Survivor,
			decimal zJ7L62_Retiree,

			decimal ZJ7L62_Survivor,				
			decimal ZJ7LS_Retiree,
			decimal ZJ7LS_Survivor,

			decimal ZJ7P_Retiree,				
			decimal zJ7P_Survivor,
			decimal ZJ7P62_Retiree,

			decimal zJ7P62_Survivor,				
			decimal ZJ7PS_Retiree,
			decimal zJ7PS_Survivor,

			decimal ZJ7S_Retiree,				
			decimal ZJ7S_Survivor,
			decimal ZM_Retiree,

			decimal ZM62_Retiree,				
			decimal ZMI_Retiree,
			decimal ZMS_Retiree,
			decimal SSBenefit,
			decimal SSIncrease,
			decimal DeathBenAmtUsed,
			decimal DeathBenPercUsed,string paraFundNo
			, decimal projFinalYearSal //2012.07.13 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page (adding new parameter projFinalYearSal)
			)//DataSet parameterPRAReportValues)
		{
			string strGUID = null;
			Database db= null;
			DbCommand insertCommandWrapper = null;
			
			db = DatabaseFactory.CreateDatabase("YRS");

			try
			{
						
				if (db == null) return null;
				// Defining The Insert Command Wrapper With parameters
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_Rpt_PRAReport");
				
				db.AddOutParameter(insertCommandWrapper,"@guid",DbType.String,255);
				db.AddInParameter(insertCommandWrapper,"@guiUniqueID",DbType.String, PersID);
				db.AddInParameter(insertCommandWrapper,"@AgeRetired",DbType.Int32,AgeRetired);

				db.AddInParameter(insertCommandWrapper,"@BenBirthDate",DbType.String,BenBirthDate);
				db.AddInParameter(insertCommandWrapper,"@BeneficiaryName",DbType.String,BeneficiaryName);

				db.AddInParameter(insertCommandWrapper,"@PRAAssumption",DbType.String,PRAAssumption);
				db.AddInParameter(insertCommandWrapper,"@RetBirthDate",DbType.String,RetBirthDate);

				db.AddInParameter(insertCommandWrapper,"@RetiredDate",DbType.String,RetiredDate);
				db.AddInParameter(insertCommandWrapper,"@RetiredDeathBen",DbType.Decimal,RetiredDeathBen);

				db.AddInParameter(insertCommandWrapper,"@C_Insured",DbType.Decimal,C_Insured);
				db.AddInParameter(insertCommandWrapper,"@C_Monthly",DbType.Decimal,C_Monthly);
				db.AddInParameter(insertCommandWrapper,"@C_Reduction",DbType.Decimal,C_Reduction);

				db.AddInParameter(insertCommandWrapper,"@C62_Insured",DbType.Decimal,C62_Insured);
				db.AddInParameter(insertCommandWrapper,"@C62_Monthly",DbType.Decimal,C62_Monthly);
				db.AddInParameter(insertCommandWrapper,"@C62_Reduction",DbType.Decimal,C62_Reduction);
				db.AddInParameter(insertCommandWrapper,"@CS_Insured",DbType.Decimal,CS_Insured);
				db.AddInParameter(insertCommandWrapper,"@CS_Monthly",DbType.Decimal,CS_Monthly);
				db.AddInParameter(insertCommandWrapper,"@CS_Reduction",DbType.Decimal,CS_Reduction);


				db.AddInParameter(insertCommandWrapper,"@J1_Retiree",DbType.Decimal,J1_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J1_Survivor",DbType.Decimal,J1_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J162_Retiree",DbType.Decimal,J162_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J162_Survivor",DbType.Decimal,J162_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J1I_Retiree",DbType.Decimal,J1I_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J1I_Survivor",DbType.Decimal,J1I_Survivor);
				
				
				db.AddInParameter(insertCommandWrapper,"@J1P_Retiree",DbType.Decimal,J1P_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J1P_Survivor",DbType.Decimal,J1P_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J1P62_Retiree",DbType.Decimal,J1P62_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J1P62_Survivor",DbType.Decimal,J1P62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J1PS_Retiree",DbType.Decimal,J1PS_Retiree);
				
				db.AddInParameter(insertCommandWrapper,"@J1PS_Survivor",DbType.Decimal,J1PS_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J1S_Retiree",DbType.Decimal,J1S_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J1S_Survivor",DbType.Decimal,J1S_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5_Retiree",DbType.Decimal,J5_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J5_Survivor ",DbType.Decimal,J5_Survivor);

				db.AddInParameter(insertCommandWrapper,"@J562_Retiree",DbType.Decimal,J562_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J562_Survivor",DbType.Decimal,J562_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5I_Retiree",DbType.Decimal,J5I_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J5I_Survivor",DbType.Decimal,J5I_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5L_Retiree ",DbType.Decimal,J5L_Retiree);
				
				db.AddInParameter(insertCommandWrapper,"@J5L_Survivor",DbType.Decimal,J5L_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5L62_Retiree",DbType.Decimal,J5L62_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J5L62_Survivor",DbType.Decimal,J5L62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5LS_Retiree",DbType.Decimal,J5LS_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J5LS_Survivor",DbType.Decimal,J5LS_Survivor);				


				db.AddInParameter(insertCommandWrapper,"@J5P_Retiree",DbType.Decimal,J5P_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J5P_Survivor",DbType.Decimal,J5P_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5P62_Retiree",DbType.Decimal,J5P62_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J5P62_Survivor",DbType.Decimal,J5P62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5PS_Retiree",DbType.Decimal,J5PS_Retiree);				

				db.AddInParameter(insertCommandWrapper,"@J5PS_Survivor",DbType.Decimal,J5PS_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J5S_Retiree",DbType.Decimal,J5S_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J5S_Survivor",DbType.Decimal,J5S_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7_Retiree",DbType.Decimal,J7_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J7_Survivor",DbType.Decimal,J7_Survivor);
				
				db.AddInParameter(insertCommandWrapper,"@J762_Retiree",DbType.Decimal,J762_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J762_Survivor",DbType.Decimal,J762_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7I_Retiree",DbType.Decimal,J7I_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J7I_Survivor",DbType.Decimal,J7I_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7L_Retiree",DbType.Decimal,J7L_Retiree);	
			

				db.AddInParameter(insertCommandWrapper,"@J7L_Survivor",DbType.Decimal,J7L_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7L62_Retiree",DbType.Decimal,J7L62_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J7L62_Survivor",DbType.Decimal,J7L62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7LS_Retiree",DbType.Decimal,J7LS_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J7LS_Survivor",DbType.Decimal,J7LS_Survivor);				

				db.AddInParameter(insertCommandWrapper,"@J7P_Retiree",DbType.Decimal,J7P_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J7P_Survivor",DbType.Decimal,J7P_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7P62_Retiree",DbType.Decimal,J7P62_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J7P62_Survivor",DbType.Decimal,J7P62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7PS_Retiree",DbType.Decimal,J7PS_Retiree);				

				db.AddInParameter(insertCommandWrapper,"@J7PS_Survivor",DbType.Decimal,J7PS_Survivor);
				db.AddInParameter(insertCommandWrapper,"@J7S_Retiree",DbType.Decimal,J7S_Retiree);
				db.AddInParameter(insertCommandWrapper,"@J7S_Survivor",DbType.Decimal,J7S_Survivor);
				db.AddInParameter(insertCommandWrapper,"@M_Retiree",DbType.Decimal,M_Retiree);
				db.AddInParameter(insertCommandWrapper,"@M62_Retiree",DbType.Decimal,M62_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@MI_Retiree",DbType.Decimal,MI_Retiree);
				db.AddInParameter(insertCommandWrapper,"@MS_Retiree",DbType.Decimal,MS_Retiree);


				db.AddInParameter(insertCommandWrapper,"@ZC_Annually",DbType.Decimal,ZC_Annually);
				db.AddInParameter(insertCommandWrapper,"@ZC62_Annually",DbType.Decimal,ZC62_Annually);
				db.AddInParameter(insertCommandWrapper,"@ZCS_Annually",DbType.Decimal,ZCS_Annually);
				db.AddInParameter(insertCommandWrapper,"@ZJ1_Retiree",DbType.Decimal,ZJ1_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ1_Survivor",DbType.Decimal,ZJ1_Survivor);				
				db.AddInParameter(insertCommandWrapper,"@ZJ162_Retiree",DbType.Decimal,ZJ162_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ162_Survivor",DbType.Decimal,ZJ162_Survivor);

				db.AddInParameter(insertCommandWrapper,"@ZJ1I_Retiree",DbType.Decimal,ZJ1I_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ1I_Survivor",DbType.Decimal,ZJ1I_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ1P_Retiree",DbType.Decimal,ZJ1P_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ1P_Survivor",DbType.Decimal,ZJ1P_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ1P62_Retiree",DbType.Decimal,ZJ1P62_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@ZJ1P62_Survivor",DbType.Decimal,ZJ1P62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ1PS_Retiree",DbType.Decimal,ZJ1PS_Retiree);


				db.AddInParameter(insertCommandWrapper,"@ZJ1PS_Survivor",DbType.Decimal,ZJ1PS_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ1S_Retiree",DbType.Decimal,ZJ1S_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ1S_Survivor",DbType.Decimal,ZJ1S_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ5_Retiree",DbType.Decimal,ZJ5_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ5_Survivor",DbType.Decimal,ZJ5_Survivor);				
				db.AddInParameter(insertCommandWrapper,"@ZJ562_Retiree",DbType.Decimal,ZJ562_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ562_Survivor",DbType.Decimal,ZJ562_Survivor);


				db.AddInParameter(insertCommandWrapper,"@ZJ5I_Retiree",DbType.Decimal,ZJ5I_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ5I_Survivor",DbType.Decimal,ZJ5I_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ5L_Retiree",DbType.Decimal,ZJ5L_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ5L_Survivor",DbType.Decimal,ZJ5L_Survivor);

				db.AddInParameter(insertCommandWrapper,"@ZJ5L62_Retiree",DbType.Decimal,ZJ5L62_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@ZJ5L62_Survivor",DbType.Decimal,ZJ5L62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ5LS_Retiree",DbType.Decimal,ZJ5LS_Retiree);

				db.AddInParameter(insertCommandWrapper,"@ZJ5LS_Survivor",DbType.Decimal,ZJ5LS_Survivor);				
				db.AddInParameter(insertCommandWrapper,"@ZJ5P_Retiree",DbType.Decimal,ZJ5P_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ5P_Survivor",DbType.Decimal,ZJ5P_Survivor);

				db.AddInParameter(insertCommandWrapper,"@ZJ5P62_Retiree",DbType.Decimal,ZJ5P62_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@zJ5P62_Survivor",DbType.Decimal,zJ5P62_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ5PS_Retiree",DbType.Decimal,ZJ5PS_Retiree);


				db.AddInParameter(insertCommandWrapper,"@ZJ5PS_Survivor",DbType.Decimal,ZJ5PS_Survivor);				
				db.AddInParameter(insertCommandWrapper,"@ZJ5S_Retiree",DbType.Decimal,ZJ5S_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ5S_Survivor",DbType.Decimal,ZJ5S_Survivor);

				db.AddInParameter(insertCommandWrapper,"@ZJ7_Retiree",DbType.Decimal,ZJ7_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@ZJ7_Survivor",DbType.Decimal,ZJ7_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ762_Retiree",DbType.Decimal,ZJ762_Retiree);

				db.AddInParameter(insertCommandWrapper,"@ZJ762_Survivor",DbType.Decimal,ZJ762_Survivor);				
				db.AddInParameter(insertCommandWrapper,"@ZJ7I_Retiree",DbType.Decimal,ZJ7I_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ7I_Survivor",DbType.Decimal,ZJ7I_Survivor);

				db.AddInParameter(insertCommandWrapper,"@ZJ7L_Retiree",DbType.Decimal,ZJ7L_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@ZJ7L_Survivor",DbType.Decimal,ZJ7L_Survivor);
				db.AddInParameter(insertCommandWrapper,"@zJ7L62_Retiree",DbType.Decimal,zJ7L62_Retiree);

				db.AddInParameter(insertCommandWrapper,"@ZJ7L62_Survivor",DbType.Decimal,ZJ7L62_Survivor);				
				db.AddInParameter(insertCommandWrapper,"@ZJ7LS_Retiree",DbType.Decimal,ZJ7LS_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZJ7LS_Survivor",DbType.Decimal,ZJ7LS_Survivor);

				db.AddInParameter(insertCommandWrapper,"@ZJ7P_Retiree",DbType.Decimal,ZJ7P_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@zJ7P_Survivor",DbType.Decimal,zJ7P_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZJ7P62_Retiree",DbType.Decimal,ZJ7P62_Retiree);

				db.AddInParameter(insertCommandWrapper,"@zJ7P62_Survivor",DbType.Decimal,zJ7P62_Survivor);				
				db.AddInParameter(insertCommandWrapper,"@ZJ7PS_Retiree",DbType.Decimal,ZJ7PS_Retiree);
				db.AddInParameter(insertCommandWrapper,"@zJ7PS_Survivor",DbType.Decimal,zJ7PS_Survivor);

				db.AddInParameter(insertCommandWrapper,"@ZJ7S_Retiree",DbType.Decimal,ZJ7S_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@ZJ7S_Survivor",DbType.Decimal,ZJ7S_Survivor);
				db.AddInParameter(insertCommandWrapper,"@ZM_Retiree",DbType.Decimal,ZM_Retiree);

				db.AddInParameter(insertCommandWrapper,"@ZM62_Retiree",DbType.Decimal,ZM62_Retiree);				
				db.AddInParameter(insertCommandWrapper,"@ZMI_Retiree",DbType.Decimal,ZMI_Retiree);
				db.AddInParameter(insertCommandWrapper,"@ZMS_Retiree",DbType.Decimal,ZMS_Retiree);
				db.AddInParameter(insertCommandWrapper,"@SSBenefit", DbType.Decimal, SSBenefit);
				db.AddInParameter(insertCommandWrapper,"@SSIncrease", DbType.Decimal, SSIncrease);				
				db.AddInParameter(insertCommandWrapper,"@DeathBenAmtUsed", DbType.Decimal, DeathBenAmtUsed);
				db.AddInParameter(insertCommandWrapper,"@DeathBenPercUsed", DbType.Decimal, DeathBenPercUsed);
				//Ashish:2010.06.21 YRS 5.0-1115
				db.AddInParameter(insertCommandWrapper,"@FundIdNo", DbType.String, paraFundNo);
				//2012.07.13 SP :  BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page
				db.AddInParameter(insertCommandWrapper, "@ProjFinalYearSal", DbType.Decimal, projFinalYearSal);
			{
				db.ExecuteNonQuery(insertCommandWrapper);
				strGUID = db.GetParameterValue(insertCommandWrapper,"@guid").ToString();
			}
				return strGUID;
			}
			catch
			{
				throw;
			}
			finally
			{
				insertCommandWrapper.Dispose();
				insertCommandWrapper = null;				
			}
		}

		#endregion 
		
		#region Other Methods
		public static DataSet getLastPayrollDate()
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			DataSet dsLastPayrollDate = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetLastPayrollDate");
				
				if (SearchCommandWrapper == null) return null;
				
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 								
				dsLastPayrollDate = new DataSet();
				dsLastPayrollDate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsLastPayrollDate,"LastPayrollDate");
				return dsLastPayrollDate;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		
		public static DataSet getWithHoldings(decimal tnTotalTaxable, string tcPersID,int tlForce)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			DataSet dsWithHoldings = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Ret_Proc_GetWithHoldings");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper,"@cPersID",DbType.String,tcPersID);
				db.AddInParameter(SearchCommandWrapper,"@nTaxableAmount",DbType.String,tnTotalTaxable);
				db.AddInParameter(SearchCommandWrapper,"@bForce",DbType.String,tlForce);
				
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				
				dsWithHoldings = new DataSet();
				dsWithHoldings.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsWithHoldings,"WithHoldings");
				return dsWithHoldings;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		
		public static DataSet GetPersonDetails(string pstrFundEventId)
		{

			Database db = null;
			DbCommand SearchCommandWrapper = null;
			DataSet dsPerson = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetPersonDetails");
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper,"@pguiFundEventId", DbType.String, pstrFundEventId);
				
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				
				dsPerson = new DataSet();
				dsPerson.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsPerson, "Person");
				return dsPerson;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper=null;
				db = null;
			}
		}
		public static DataSet TaxFactors()
		{
			DataSet l_dataset_dsTaxFactors = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_TaxFactors");
				
				if (LookUpCommandWrapper == null) return null;						
			
				l_dataset_dsTaxFactors = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsTaxFactors,"TaxFactors");
				return l_dataset_dsTaxFactors;
			}
			catch 
			{
				throw;
			}
			finally
			{
				LookUpCommandWrapper.Dispose();
				LookUpCommandWrapper=null;
				db = null;
			}
		}

		public static string LastPayrollDate(string pstrPersId)
		{
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			string l_string_LastPayrollDate;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_LastPayrollDate");
				
				if (LookUpCommandWrapper == null) return null;						
				
				db.AddInParameter(LookUpCommandWrapper,"@PersId",DbType.String,pstrPersId);
				db.AddOutParameter(LookUpCommandWrapper,"@pLastPayrollDate",DbType.String,15);
				
				db.ExecuteNonQuery(LookUpCommandWrapper);
				
				l_string_LastPayrollDate = Convert.ToString(db.GetParameterValue(LookUpCommandWrapper,"@pLastPayrollDate"));

				return l_string_LastPayrollDate;
			}
			catch 
			{
				throw;
			}
			finally
			{
				LookUpCommandWrapper.Dispose();
				LookUpCommandWrapper=null;
				db = null;
			}
		}
		
		//public static DataSet getElectiveAccountsByPlan(string personID, string planType)
		//Commnetd by Ashish 15-Apr-2009 and added retirement date as parameter
//		public static DataSet getElectiveAccountsByPlan(string fundEventID, string planType)
			public static DataSet getElectiveAccountsByPlan(string fundEventID, string planType,string retirementDate)
		{
			DataSet dsElectiveAccounts = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_getElectiveAccountsByPlan");
				
				if (SearchCommandWrapper == null) return null;
			
				//SearchCommandWrapper.AddInParameter("@guiPersID", DbType.String, personID);
				db.AddInParameter(SearchCommandWrapper,"@guiFundEventID", DbType.String, fundEventID);
				db.AddInParameter(SearchCommandWrapper,"@YmcaID", DbType.String, string.Empty);
				db.AddInParameter(SearchCommandWrapper,"@planType", DbType.String, planType);
				db.AddInParameter(SearchCommandWrapper,"@retirementDate", DbType.String, retirementDate);

				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				dsElectiveAccounts = new DataSet();
				dsElectiveAccounts.Locale = CultureInfo.InvariantCulture;
				//Added by Ashish 15-Apr-2009				
				string [] l_TableNames={"RetireeElectiveAccountsInformation","RetireeGroupedElectiveAccounts"};

				db.LoadDataSet(SearchCommandWrapper, dsElectiveAccounts,l_TableNames);
				
				return dsElectiveAccounts;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}

		public static DataSet GetBasicAccountsByPlan(string fundeventID)
		{
			DataSet dsElectiveAccounts = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_GetSumByAccountByPlan");
				
				if (SearchCommandWrapper == null) return null;
			
				db.AddInParameter(SearchCommandWrapper,"@guiFundEventId", DbType.String, fundeventID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				dsElectiveAccounts = new DataSet();
				dsElectiveAccounts.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsElectiveAccounts,"RetireeElectiveAccountsInformation");
				
				return dsElectiveAccounts;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}
		public static DataSet GetPurchasedAnnuityDetails(string personID)
		{
			DataSet dsElectiveAccounts = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Est_GetPurchasedAnnuityDetails");
				
				if (SearchCommandWrapper == null) return null;
			
				db.AddInParameter(SearchCommandWrapper,"@guiPersID", DbType.String, personID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				dsElectiveAccounts = new DataSet();
				dsElectiveAccounts.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsElectiveAccounts,"RetireeElectiveAccountsInformation");
				
				return dsElectiveAccounts;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}
		public static DataSet GetExistingAnnuities(string fundEventID)
		{
			DataSet dsExistingAnnuities = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetExistingAnnuities");
				
				if (SearchCommandWrapper == null) return null;
			
				db.AddInParameter(SearchCommandWrapper,"@FundEventID", DbType.String, fundEventID);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				dsExistingAnnuities = new DataSet();
				dsExistingAnnuities.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsExistingAnnuities, "ExistingAnnuities");
				
				return dsExistingAnnuities;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}
		
		public static DataSet GetExperienceDividends(string retirementDate)
		{
			DataSet dsExperienceDividends = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetExperienceDividends");
				
				if (SearchCommandWrapper == null) return null;
			
				db.AddInParameter(SearchCommandWrapper,"@RetirementDate", DbType.String, retirementDate);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				dsExperienceDividends = new DataSet();
				dsExperienceDividends.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsExperienceDividends, "ExperienceDividends");
				
				return dsExperienceDividends;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}
		
		public static DataSet GetPlanSplitDate()
		{
			DataSet dsPlanSplitDate = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_GetPlanSplitDate");
				
				if (SearchCommandWrapper == null) return null;
			
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
				dsPlanSplitDate = new DataSet();
				dsPlanSplitDate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsPlanSplitDate, "ExperienceDividends");
				
				return dsPlanSplitDate;
			}
			catch 
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}
        ///ASHISH:2010.11.16:Added new method for YRS 5.0-1215
        public static DataSet getRetAccountsBalanceUptoExactAgeEffDate(string Param_StartDate,string Param_EndDate, string Param_guiPersID, string Param_guiFundEventID, string planType)
        {
            DataSet dsSearchAccountsBalanceUptoJan2011 = null;
            Database db = null;
            DbCommand SearchCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Ret_Est_GetAccountBalSumByAccountByBasisBeforeExactAgeEfftDate");

                if (SearchCommandWrapper == null) return null;

                db.AddInParameter(SearchCommandWrapper, "@startdate", DbType.String, Param_StartDate);
                db.AddInParameter(SearchCommandWrapper, "@enddate", DbType.String, Param_EndDate);
                db.AddInParameter(SearchCommandWrapper, "@guiFundEventId", DbType.String, Param_guiFundEventID);
                db.AddInParameter(SearchCommandWrapper, "@planType", DbType.String, planType);
              
               
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                dsSearchAccountsBalanceUptoJan2011 = new DataSet();
                dsSearchAccountsBalanceUptoJan2011.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(SearchCommandWrapper, dsSearchAccountsBalanceUptoJan2011, "RetireeAccountsBalanceInformation");

                return dsSearchAccountsBalanceUptoJan2011;
            }
            catch
            {
                throw;
            }
            finally
            {
                SearchCommandWrapper.Dispose();
                SearchCommandWrapper = null;
                db = null;
            }
        }

        //2010.11.17    Priya Jawale        Added validation method for exact age annuity
        public static string ValidationForAfterExactAgeEff(string strfundEventID)
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string l_str_ValDateForRetCalcMethod;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RP_PerformValidations");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@FundEventID", DbType.String, strfundEventID);
                db.AddOutParameter(LookUpCommandWrapper, "@cOutput", DbType.String, 1000);

                db.ExecuteNonQuery(LookUpCommandWrapper);

                l_str_ValDateForRetCalcMethod = Convert.ToString(db.GetParameterValue(LookUpCommandWrapper, "@cOutput"));

                return l_str_ValDateForRetCalcMethod;
            }
            catch
            {
                throw;
            }
            finally
            {
                LookUpCommandWrapper.Dispose();
                LookUpCommandWrapper = null;
                db = null;
            }
         }
        //End  2010.11.17
        // ASHISH:2011.02.01 BT-665 Disability Retirement
        /// <summary>
        /// GetDisability Average Salary
        /// </summary>
        /// <param name="parameterBatchId"></param>
        /// <returns>decimal</returns>
        public static decimal GetDisabilityAverageSalary(string para_FundEventID,string para_RetirementDate, string transactionDetails) //MMR | 2017.03.03 | YRS-AR-2625 | Added 'transactionDetails' parameter for manual Transaction details in XML Format
        {
            decimal l_DisabilityAvgSal = 0;


            DbCommand l_DBCommandWrapper=null;
            Database db = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return 0;

                    l_DBCommandWrapper = db.GetStoredProcCommand("yrs_Ret_GetDisabilityAverageSalary");

                    //l_DBCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                    db.AddInParameter(l_DBCommandWrapper, "@guiFundEventID", DbType.String, para_FundEventID);
                    db.AddInParameter(l_DBCommandWrapper, "@RetirementDate", DbType.String, para_RetirementDate);
                    db.AddInParameter(l_DBCommandWrapper, "@XML_MAPRTransactions", DbType.Xml, transactionDetails); //MMR | 2017.03.03 | YRS-AR-2625 | Added 'transactionDetails' parameter for manual Transaction details in XML Format

                    object l_object = db.ExecuteScalar(l_DBCommandWrapper);
                    if (l_object != null)
                    {
                        if (l_object.GetType().ToString() != "System.DBNull")
                        {
                            l_DisabilityAvgSal = Convert.ToDecimal(l_object);
                        }
                    }              

                return l_DisabilityAvgSal;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                l_DBCommandWrapper.Dispose();
                l_DBCommandWrapper = null;
                db = null;
            }
        }

		//2012.06.28 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -Start
		/// <summary>
		/// This methods checks, if any DLIN records exist before retirement date then return true else false 
		/// </summary>
		/// <param name="retireeGuiFundEventId"></param>
		/// <param name="retireeRetireDate"></param>
		/// <returns>Boolean</returns>
		public static bool IsExistsDLINRecordBeforeRetirement(string retireeGuiFundEventId, string retireeRetireDate)
		{
			Database db = null;
			DbCommand SearchCommandWrapper = null;
			bool isDlinRecordExist = false;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return false;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_Ret_CheckDLINRecordsExistOnRetirementDate");

				if (SearchCommandWrapper == null) return false;

				db.AddInParameter(SearchCommandWrapper, "@Retirementddate", DbType.String, retireeRetireDate);
				db.AddInParameter(SearchCommandWrapper, "@guiFundEventId", DbType.String, retireeGuiFundEventId);
				db.AddOutParameter(SearchCommandWrapper, "@isDLINExist", DbType.Boolean,1);
				SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				db.ExecuteNonQuery(SearchCommandWrapper);
				isDlinRecordExist = Convert.ToBoolean(db.GetParameterValue(SearchCommandWrapper, "@isDLINExist"));
				return isDlinRecordExist;
			}
			catch
			{
				throw;
			}
			finally
			{
				SearchCommandWrapper.Dispose();
				SearchCommandWrapper = null;
				db = null;
			}
		}
		//2012.06.28 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -End

        // START | SR | 2015.12.17 | YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
        public static DataSet GetCurrentContributions(string param_FundEventId, DateTime param_RetirementDate) // PPP | 05/18/2016 | YRS-AT-2683 | Added new parameter "param_RetirementDate"
        {
            DataSet dsCurrentContribution = null;
            Database db = null;
            DbCommand cmdCurrentContribution = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                cmdCurrentContribution = db.GetStoredProcCommand("yrs_usp_Ret_Est_Get_YTDSummary");
                cmdCurrentContribution.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (cmdCurrentContribution == null) return null;
                db.AddInParameter(cmdCurrentContribution, "@guiFundEvenId", DbType.String, param_FundEventId);
                db.AddInParameter(cmdCurrentContribution, "@dtRetirementDate", DbType.DateTime, param_RetirementDate); // PPP | 05/18/2016 | YRS-AT-2683 | Passing retirement date, if it is less than next month's start date then current year projection will be done till retirement date

                dsCurrentContribution = new DataSet();
                dsCurrentContribution.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(cmdCurrentContribution, dsCurrentContribution, "CurrentContribution");

                return dsCurrentContribution;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmdCurrentContribution.Dispose();
                cmdCurrentContribution = null;
                db = null;
            }
        }
        // END | SR | 2015.12.17 | YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
        // START | CC | 2016.04.07 | YRS-AT-2891 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
        public static DataSet GetCurrentTDServiceCatchup(string param_FundEventId, DateTime param_RetirementDate) // PPP | 05/18/2016 | YRS-AT-2683 | Added new parameter "param_RetirementDate"
        {
            DataSet dsTDCurrentContribution = null;
            Database db = null;
            DbCommand cmdCurrentTDContribution = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                cmdCurrentTDContribution = db.GetStoredProcCommand("yrs_usp_Ret_Est_Get_TDServiceCatchup");
                cmdCurrentTDContribution.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (cmdCurrentTDContribution == null) return null;
                db.AddInParameter(cmdCurrentTDContribution, "@guiFundEvenId", DbType.String, param_FundEventId);
                db.AddInParameter(cmdCurrentTDContribution, "@dtRetirementDate", DbType.DateTime, param_RetirementDate); // PPP | 05/18/2016 | YRS-AT-2683 | Passing retirement date, if it is less than next month's start date then current year projection will be done till retirement date
                dsTDCurrentContribution = new DataSet();
                dsTDCurrentContribution.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(cmdCurrentTDContribution, dsTDCurrentContribution, "CurrentTDContribution");
                return dsTDCurrentContribution;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmdCurrentTDContribution.Dispose();
                cmdCurrentTDContribution = null;
                db = null;
            }
        }
        // END | CC | 2016.04.07 | YRS-AT-2891 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)

		#endregion


        //START: SR | 2016.08.02 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table
        public static void InsertBeneficiariesSSNChangeAuditRecord(DataSet dsAuditEntries, DbTransaction pDBTransaction, Database db)
        {            
            DbCommand AddCommandWrapper = null;
            try
            {  

                if (db == null) return;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_InsertAtsYRSAuditLog");
                if (AddCommandWrapper == null) return;

                db.AddInParameter(AddCommandWrapper, "@chvModuleName", DbType.String, "ModuleName", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@guiEntityId", DbType.String, "UniqueID", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvEntityType", DbType.String, "EntityType", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvColumn", DbType.String, "chvColumn", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvOldValue", DbType.String, "OldSSN", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvNewValue", DbType.String, "NewSSN", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvDescription", DbType.String, "Reason", DataRowVersion.Current);
                db.AddOutParameter(AddCommandWrapper, "@bintUniqueID", DbType.Int32, 10);
               

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.UpdateDataSet(dsAuditEntries, "Audit", AddCommandWrapper, null, null, pDBTransaction);
            }
            catch
            {
                throw;
            }
        }
        //END | SR | 2016.08.02 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table

        // START | MMR | 2017.02.22 | YRS-AT-2625 | Get Manual Transaction details for 'MAPR' Transact type
        public static DataSet GetManualTransactions(string fundEventId, string retireeType, DateTime retirementDate) 
        {
            DataSet dsManualTransaction = null;
            Database db = null;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_Ret_GetManualTransactions");
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (cmd == null) return null;
                db.AddInParameter(cmd, "@VARCHAR_FundEventID", DbType.String, fundEventId);
                db.AddInParameter(cmd, "@VARCHAR_RetireType", DbType.String, retireeType);
                db.AddInParameter(cmd, "@DATETIME_RetirementDate", DbType.DateTime, retirementDate);
                dsManualTransaction = new DataSet();
                dsManualTransaction.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(cmd, dsManualTransaction, "ManualTransactionDetails");
                return dsManualTransaction;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                db = null;
            }
        }
        // END | MMR | 2017.02.22 | YRS-AT-2625 | Get Manual Transaction details for 'MAPR' Transact type

        //START: PPP | 12/28/2017 | YRS-AT-3328 | Fetching YMCA Legacy balance as on termination date
        public static double GetYmcaLegacyAtTerminationBalance(string fundEventID)
        {
            Database db;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return 0;

                cmd = db.GetStoredProcCommand("yrs_usp_Ret_Est_Get_TerminatePIA");
                if (cmd == null) return 0;

                db.AddInParameter(cmd, "@VARCHAR_FundEvenID", DbType.String, fundEventID);
                db.AddOutParameter(cmd, "@NUMERIC_TerminatePIAAmount", DbType.Double, 10);
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.ExecuteNonQuery(cmd);
                return Convert.ToDouble(db.GetParameterValue(cmd, "@NUMERIC_TerminatePIAAmount"));
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                db = null;
            }
        }
        //END: PPP | 12/28/2017 | YRS-AT-3328 | Fetching YMCA Legacy balance as on termination date

        //START : BD : 2018.10.31 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
        public static bool IsFirstEnrolledBeforeCutOffDate_2019PlanChange(string fundEventID)
        {
            Database db;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;

                cmd = db.GetStoredProcCommand("Yrs_usp_2019PlanChange_IsFirstEnrolledBeforeCutOffDate");
                if (cmd == null) return false;

                db.AddInParameter(cmd, "@VARCHAR_FundEventId", DbType.String, fundEventID);
                db.AddOutParameter(cmd, "@BIT_IsFirstEnrolledBeforeCutOffDate", DbType.Boolean, 1);
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.ExecuteNonQuery(cmd);
                return Convert.ToBoolean(db.GetParameterValue(cmd, "@BIT_IsFirstEnrolledBeforeCutOffDate"));
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                db = null;
            }
        }

        public static DataSet GetFundEventDetails(string fundEventID)
        {
            DataSet dsFundEvents = null;
            Database db;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("Yrs_usp_GetFundEventDetails");
                if (cmd == null) return null;

                db.AddInParameter(cmd, "@VARCHAR_FundEventId", DbType.String, fundEventID);
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                dsFundEvents = new DataSet();
                dsFundEvents.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(cmd, dsFundEvents, "FundEventDetails");
                return dsFundEvents;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                db = null;
            }
        }
        //END : BD : 2018.10.31 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
    }
}