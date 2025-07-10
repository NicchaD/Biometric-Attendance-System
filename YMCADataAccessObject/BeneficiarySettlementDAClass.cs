// Change History
//****************************************************
//Modification History
//****************************************************
//  Modified by     Date        Description
//****************************************************
//	Nikunj Patel	2007.07.20	Added extra parameters to the settlement process to enable simultaneous settlement of both plans
//  Nikunj Patel	2007.07.26	Updated code to allow simultaneous settlement of two options. Adding of parameters was creating a problem
//  Nikunj Patel	2007.09.06	Adding parameter FundNo to perform searches by Fund number
//	Nikunj Patel	2007.11.05	Changing the settlement process to run the Special Dividend process after performing settlements on both Retirement and Savings plan
//	Nikunj Patel	2008.01.03	YRPS-4046 Adding parameter BenefitOptionId to the procedure call to obtain existing Fund Events
//	Nikunj Patel	2008.01.23	YRPS-4009 Updating code to make calls for Performing validations separately
//	Nikunj Patel    2008.12.05  Passing Selected benefit Option Id so that the new fund event if required is created of the right type either DBEN or RBEN.
//  Sanjay R.       2010.06.21  Enhancement changes(Parameter Attribute DbType.String to DbType.guid)
//  Sanjay R.       2010.07.12  Code Review changes.(Region) 
//  Bhavna S        2011.08.10  YRS 5.0-1339:BT:852 - Reopen issue 
//  Shashank Patel	2013.04.12  YRS 5.0-1990:similar SSNs are being updates across the board
//  Shashank Patel	2014.04.01 	BT-2420\YRS 5.0-2309 : Beneficiary Settlement - Retired Death
//  Manthan Rajguru 2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//  Manthan Rajguru 2016.04.22  YRS-AT-2206 -  Applying Fees and deductions to death payments - Part A.1 
//  Manthan Rajguru 2016.06.13  YRS-AT-2206 -  Applying Fees and deductions to death payments - Part A.1 
//  Santosh Bura	2016.11.25 	YRS-AT-3022 -  YRS enhancement.--YRS death settlement screen.Track it 26636
//****************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for BeneficiarySettlementDAClass.
	/// </summary>
	public class BeneficiarySettlementDAClass
	{
		public BeneficiarySettlementDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        
        #region " Find Member Details "

        public static DataSet LookUp_Beneficiaries_MemberListDeseased(string parameterSSN, string parameterLName ,string parameterFName, string parameterFundNo)
		{
			DataSet l_dataset_Beneficiaries_LookUp_MemberListDeceased = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_MemberListForDeceased");
				//LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeathCalc_LookUp_MemberListForDeath");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_SSN",DbType.String,parameterSSN);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_LName", DbType.String, parameterLName);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FName", DbType.String, parameterFName);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundNo", DbType.String, parameterFundNo);

                LookUpCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 

				l_dataset_Beneficiaries_LookUp_MemberListDeceased = new DataSet();
				
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Beneficiaries_LookUp_MemberListDeceased,"r_MemberListForDeceased");
				return l_dataset_Beneficiaries_LookUp_MemberListDeceased;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BS_ActiveorRetiredDetails(string parameterPersID)
		{
			DataSet l_dataset_BS_LookUp_ActiveorRetireDetails = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_PersBeneficiaryCategoryCnts");
				
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersID", DbType.String, parameterPersID);
			
				l_dataset_BS_LookUp_ActiveorRetireDetails = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BS_LookUp_ActiveorRetireDetails,"r_DetailsActiveorRetired");
				return l_dataset_BS_LookUp_ActiveorRetireDetails;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_Beneficiaries_BeneficiaryPrimesettle(string parameterPersID,string parameterStatus)
		{
			DataSet l_dataset_Beneficiaries_LookUp_BeneficiaryPrimesettle = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_BeneficiariesPrimeSettle");
				
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersID", DbType.String, parameterPersID);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_DeathFundEventStatus", DbType.String, parameterStatus);

				l_dataset_Beneficiaries_LookUp_BeneficiaryPrimesettle = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Beneficiaries_LookUp_BeneficiaryPrimesettle,"r_BeneficiaryPrimesettle");
				return l_dataset_Beneficiaries_LookUp_BeneficiaryPrimesettle;
			}
			catch 
			{
				throw;
			}
		}
        //		public static DataSet LookUp_Beneficiaries_RetiredBeneficiariesPrimeSettle(string parameterPersID)
		//		{
		//			DataSet l_dataset_Beneficiaries_LookUp_RetiredBeneficiariesPrimeSettle = null;
		//			Database db = null;
		//			DbCommand LookUpCommandWrapper = null;
		//			
		//			try
		//			{
		//				db = DatabaseFactory.CreateDatabase("YRS");
		//			
		//				if (db == null) return null;
		//
		//				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_RetiredBeneficiariesPrimeSettle");
		//				
		//				if (LookUpCommandWrapper == null) return null;
		//
		//				LookUpCommandWrapper.AddInParameter("@varchar_PersID",DbType.String,parameterPersID);
		//
		//				l_dataset_Beneficiaries_LookUp_RetiredBeneficiariesPrimeSettle = new DataSet();
		//				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Beneficiaries_LookUp_RetiredBeneficiariesPrimeSettle,"r_RetiredBeneficiariesPrimeSettle");
		//				return l_dataset_Beneficiaries_LookUp_RetiredBeneficiariesPrimeSettle;
		//			}
		//			catch 
		//			{
		//				throw;
		//			}
		//		}

		//
        public static DataSet LookUp_Beneficiaries_DeathBenefitOptions(string parameterPersID,string parameterStatus)
		{
			DataSet l_dataset_Beneficiaries_LookUp_DeathBenefitOptions = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_DeathBenefitOptions");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_PersID",DbType.String,parameterPersID);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_DeathFundEventStatus",DbType.String,parameterStatus);
                LookUpCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 

				l_dataset_Beneficiaries_LookUp_DeathBenefitOptions = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Beneficiaries_LookUp_DeathBenefitOptions,"r_DeathBenefitOptions");
				return l_dataset_Beneficiaries_LookUp_DeathBenefitOptions;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BI_BeneficiaryInformation(string parameterBenefitOptionID)
		{
			DataSet l_dataset_Beneficiaries_LookUp_BeneficiaryInformation = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_BeneficiaryInformation");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_BenefitOptionID",DbType.String,parameterBenefitOptionID);
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
	
				l_dataset_Beneficiaries_LookUp_BeneficiaryInformation = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Beneficiaries_LookUp_BeneficiaryInformation,"r_BenefitInformation");
				return l_dataset_Beneficiaries_LookUp_BeneficiaryInformation;
			}
			catch 
			{
				throw;
			}
		}
        //yrs_usp_BI_LookUp_BeneficiaryPersonalDetails
        public static DataSet LookUp_BI_BeneficiaryPersonalDetails(string parameterSSNo)
		{
			DataSet l_dataset_Beneficiaries_LookUp_BeneficiaryPersonalDetails = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_BeneficiaryPersonalDetails");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_SSNo",DbType.String,parameterSSNo);
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				l_dataset_Beneficiaries_LookUp_BeneficiaryPersonalDetails = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Beneficiaries_LookUp_BeneficiaryPersonalDetails,"r_BeneficiaryPersonalDetails");
				return l_dataset_Beneficiaries_LookUp_BeneficiaryPersonalDetails;
			}
			catch 
			{
				throw;
			}

		}
		public static DataSet LookUp_BI_DeathBeneficiaryOption(string parameterDeathBeneOptionID)
		{
			DataSet l_dataset_BI_LookUp_DeathBeneficiaryOption= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_DeathBenefitOptions");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_DeathBenefitOptionID",DbType.String,parameterDeathBeneOptionID);

				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				l_dataset_BI_LookUp_DeathBeneficiaryOption = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_DeathBeneficiaryOption,"r_BI_DeathBeneficiaryOption");
				return l_dataset_BI_LookUp_DeathBeneficiaryOption;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BI_AnnuityCount(string parameterOption)
		{
			DataSet l_dataset_BI_LookUp_AnnuityCount= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_MetaDeathOption");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_Options",DbType.String,parameterOption);

				l_dataset_BI_LookUp_AnnuityCount = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_AnnuityCount,"r_AnnuityCount");
				return l_dataset_BI_LookUp_AnnuityCount;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BI_PersonalDetailsAll(string parameterPersPK,string parameterType)
		{
			DataSet l_dataset_BI_LookUp_PersonalDetailsAll= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				//				if(parameterType=="POA")
				//					LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_POA");
				//				else if(parameterType=="TEL")
				//					LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_Telephone");
				//				else if(parameterType=="EMAIL")
				//					LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_EmailAddress");
				//				else if(parameterType=="ADDR")
				//					LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_Address");
				//				else if(parameterType=="FED")
				//					LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_FedWithHolding");
				//				else if(parameterType=="VALIDATE")
				//					LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_ValidateSettlementPrerequisites");
				switch(parameterType)
				{
					case "POA":
					{
						LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_POA");
						break;
					}
					case "TEL":
					{	
						LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_Telephone");
						break;
					}
					case "EMAIL":
					{
						LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_EmailAddress");
						break;
					}
					case "ADDR":
					{
						LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_Address");
						break;
					}
					case "FED":
					{
						LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_FedWithHolding");
						break;
					}
					case "VALIDATE":
					{
						LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_ValidateSettlementPrerequisites");
						break;
					}



				}

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_PerssPK",DbType.String,parameterPersPK);
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				l_dataset_BI_LookUp_PersonalDetailsAll = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_PersonalDetailsAll,"r_PersonalDetailsAll");
				return l_dataset_BI_LookUp_PersonalDetailsAll;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BI_NewBeneficiary(string parameterDeathBeneOptionID)
		{
			DataSet l_dataset_BI_LookUp_NewBeneficiary= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_NewBeneficiary");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_DeathBenefitOptionID",DbType.String,parameterDeathBeneOptionID);

				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				l_dataset_BI_LookUp_NewBeneficiary = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_NewBeneficiary,"r_BI_NewBeneficiary");
				return l_dataset_BI_LookUp_NewBeneficiary;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BI_ExistingSSNo(string parameterSSNo)
		{
			DataSet l_dataset_BI_LookUp_ExistingSSNo= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("ap_SEL_atsPerss_BySSNo");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@SSNo",DbType.String,parameterSSNo);

				l_dataset_BI_LookUp_ExistingSSNo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_ExistingSSNo,"r_BI_ExistingSSNo");
				return l_dataset_BI_LookUp_ExistingSSNo;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_GetUniqueID()
		{
			DataSet l_dataset_BS_UniqueID= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_USP_BS_GetUniqueID");
							
				if (LookUpCommandWrapper == null) return null;

				l_dataset_BS_UniqueID = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BS_UniqueID,"UniqueID");
				return l_dataset_BS_UniqueID;
			}
			catch 
			{
				throw;
			}
			
		}
        public static DataSet CreateDataSetForSave(string l_String_Type)
		{
			DataSet l_dataset_BS_SaveDataSet= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Lookup_TableForBeneficiarySave");
							
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper, "@varchar_Type",DbType.String,l_String_Type);

				l_dataset_BS_SaveDataSet = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BS_SaveDataSet,"Member Details");
				return l_dataset_BS_SaveDataSet;
			}
			catch 
			{
				throw;
			}
			
		}
        public static DataSet LookUp_BS_CountryNames()
		{
			DataSet l_dataset_BI_LookUp_CountryNames= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_CountryNames");
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				if (LookUpCommandWrapper == null) return null;
				l_dataset_BI_LookUp_CountryNames = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_CountryNames,"r_CountryNames");
				return l_dataset_BI_LookUp_CountryNames;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BS_StateNames(string l_string_CountryCode)
		{
			DataSet l_dataset_BI_LookUp_StateNames= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_StateNames");


				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper, "@varchar_CountryCode",DbType.String,l_string_CountryCode);

				l_dataset_BI_LookUp_StateNames = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_StateNames,"r_StateNames");
				return l_dataset_BI_LookUp_StateNames;
			}
			catch 
			{
				throw;
			}
		}//
        public static DataSet LookUp_BS_FunEventsUniqueID(string parameterDeathBeneOptionID)
		{
			DataSet l_dataset_BI_LookUp_FunEventsUniqueID= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_FunEventsUniqueID");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_deathbenefitoptionid",DbType.String,parameterDeathBeneOptionID);


				l_dataset_BI_LookUp_FunEventsUniqueID = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_FunEventsUniqueID,"r_FunEventsUniqueID");
				return l_dataset_BI_LookUp_FunEventsUniqueID;
			}
			catch 
			{
				throw;
			}
		}
        public static string Update_BS_BeneficiariesSSNoWithNew(string parameterOldSSNo, string parameterNewSSNo
																, string guibeneficiaryID //SP:2013.04.12 :YRS 5.0-1990 -Added
																)
        {
		
			Database db = null;
			DbCommand UpdateCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Update_BeneficiariesSSNoWithNew");
			
				if (UpdateCommandWrapper == null) return null;

				db.AddInParameter(UpdateCommandWrapper, "@chr_SSNo", DbType.String, parameterNewSSNo);
				db.AddInParameter(UpdateCommandWrapper, "@chr_SSNo_Old", DbType.String, parameterOldSSNo);
				db.AddInParameter(UpdateCommandWrapper, "@gui_BeneficiaryID", DbType.String, guibeneficiaryID);//SP:2013.04.12 :YRS 5.0-1990 -Added
		
				db.ExecuteNonQuery(UpdateCommandWrapper);
				return "Done";
			}
			catch 
			{
				throw;
			}
			
		}
		//NP:2008.01.03:YRPS-4046 - Adding Parameter BenefitOptionId to the procedure call
		public static DataSet LookUp_BS_Lookp_BeneficiaryFundEvent(string parameterPerssID, string parameterBenefitOptionId)
		{
			DataSet l_dataset_BI_LookUp_BeneficiaryFundEvent= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Lookup_UniqueID_in_FundEvents");

				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_Persid",DbType.String,parameterPerssID);
				//NP:2008.01.03:YRPS-4046 - Adding input parameter BenefitOptionId to the procedure call
				db.AddInParameter(LookUpCommandWrapper, "@varchar_BenefitOptionId", DbType.String, parameterBenefitOptionId);

				l_dataset_BI_LookUp_BeneficiaryFundEvent = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_BeneficiaryFundEvent,"r_BeneficiaryFundEvent");
				return l_dataset_BI_LookUp_BeneficiaryFundEvent;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BS_MemberAddress(string l_string_PersID)
		{
			DataSet l_dataset_BI_LookUp_MemberAddress= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Lookup_MemberAddresses");


				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper, "@varchar_EntityID",DbType.String,l_string_PersID);

				l_dataset_BI_LookUp_MemberAddress = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_MemberAddress,"r_MemberAddress");
				return l_dataset_BI_LookUp_MemberAddress;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BS_MemberTelephone(string l_string_PersID)
		{
			DataSet l_dataset_BI_LookUp_MemberTelephone= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Lookup_MemberTelephone");


				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper, "@varchar_EntityID",DbType.String,l_string_PersID);

				l_dataset_BI_LookUp_MemberTelephone = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_MemberTelephone,"r_MemberTelephone");
				return l_dataset_BI_LookUp_MemberTelephone;
			}
			catch 
			{
				throw;
			}
		}
        public static DataSet LookUp_BS_MemberEmailAddress(string l_string_PersID)
		{
			DataSet l_dataset_BI_LookUp_MemberEmailAddress= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Lookup_MemberEmailAddress");


				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper, "@varchar_EntityID",DbType.String,l_string_PersID);

				l_dataset_BI_LookUp_MemberEmailAddress = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BI_LookUp_MemberEmailAddress,"r_MemberEmailAddress");
				return l_dataset_BI_LookUp_MemberEmailAddress;
			}
			catch 
			{
				throw;
			}
		}
          #endregion
        
        #region " Save/Update Member Details "

        public static void Update_MemberAddress(DataSet parameterDatasetMemberAddressDetails)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;
            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Update_MemberAddresses");
                // Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "guiUniqueID", DataRowVersion.Current);  //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityID", DbType.Guid, "guiEntityID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityCode", DbType.String, "chvEntityCode", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@date_EffDate", DbType.DateTime, "dtmEffDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_AddrCode", DbType.String, "chvAddrCode", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Addr1", DbType.String, "chvAddr1", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Addr2", DbType.String, "chvAddr2", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Addr3", DbType.String, "chvAddr3", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_City", DbType.String, "chvCity", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_StateType", DbType.String, "chrStateType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Zip", DbType.String, "chrZip", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Country", DbType.String, "chvCountry", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_bitActive", DbType.Int16, "bitActive", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_bitPrimary", DbType.Int16, "bitPrimary", DataRowVersion.Current);

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Update_MemberAddresses");
                // Defining The Insert Command Wrapper With parameters

                db.AddInParameter(updateCommandWrapper, "@varchar_UniqueID", DbType.Guid, "guiUniqueID", DataRowVersion.Current);  //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(updateCommandWrapper, "@varchar_EntityID", DbType.Guid, "guiEntityID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(updateCommandWrapper, "@varchar_EntityCode", DbType.String, "chvEntityCode", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@date_EffDate", DbType.DateTime, "dtmEffDate", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_AddrCode", DbType.String, "chvAddrCode", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Addr1", DbType.String, "chvAddr1", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Addr2", DbType.String, "chvAddr2", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Addr3", DbType.String, "chvAddr3", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_City", DbType.String, "chvCity", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_StateType", DbType.String, "chrStateType", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Zip", DbType.String, "chrZip", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Country", DbType.String, "chvCountry", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@bit_bitActive", DbType.Int16, "bitActive", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@bit_bitPrimary", DbType.Int16, "bitPrimary", DataRowVersion.Current);


                if (parameterDatasetMemberAddressDetails != null)
                {
                    db.UpdateDataSet(parameterDatasetMemberAddressDetails, "Member Details", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }
            }
            catch
            {
                throw;
            }
        }

        public static void Insert_MemberAddress(DataSet parameterDatasetMemberAddressDetails)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;	

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_BS_Insert_MemberAddresses");
				// Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "guiUniqueID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityID", DbType.Guid, "guiEntityID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
				db.AddInParameter(insertCommandWrapper, "@varchar_EntityCode",DbType.String,"chvEntityCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@date_EffDate",DbType.DateTime,"dtmEffDate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_AddrCode",DbType.String,"chvAddrCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Addr1",DbType.String,"chvAddr1",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Addr2",DbType.String,"chvAddr2",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Addr3",DbType.String,"chvAddr3",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_City",DbType.String,"chvCity",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_StateType",DbType.String,"chrStateType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Zip",DbType.String,"chrZip",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Country",DbType.String,"chvCountry",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitActive",DbType.Int16,"bitActive",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitPrimary",DbType.Int16,"bitPrimary",DataRowVersion.Current);

	
				if (parameterDatasetMemberAddressDetails != null)
				{
					db.UpdateDataSet(parameterDatasetMemberAddressDetails,"Member Details" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}

		public static void Insert_MemberTelephone(DataSet parameterDatasetMemberTelephoneDetails)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;	

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_BS_Insert_MemberTelephone");
				// Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "guiUniqueID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityID", DbType.Guid, "guiEntityID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
				db.AddInParameter(insertCommandWrapper, "@varchar_EntityCode",DbType.String,"chvEntityCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@date_EffDate",DbType.DateTime,"dtmEffDate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_PhoneTypeCode",DbType.String,"chvPhoneTypeCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_PhoneNumber",DbType.String,"chvPhoneNumber",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitActive",DbType.Int16,"bitActive",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitPrimary",DbType.Int16,"bitPrimary",DataRowVersion.Current);

	
				if (parameterDatasetMemberTelephoneDetails != null)
				{
					db.UpdateDataSet(parameterDatasetMemberTelephoneDetails,"Member Details" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}

		public static void Update_MemberTelephone(DataSet parameterDatasetMemberTelephoneDetails)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;	

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_BS_Update_MemberTelephone");
				// Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "guiUniqueID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityID", DbType.Guid, "guiEntityID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
				db.AddInParameter(insertCommandWrapper, "@varchar_EntityCode",DbType.String,"chvEntityCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@date_EffDate",DbType.DateTime,"dtmEffDate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_PhoneTypeCode",DbType.String,"chvPhoneTypeCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_PhoneNumber",DbType.String,"chvPhoneNumber",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitActive",DbType.Int16,"bitActive",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitPrimary",DbType.Int16,"bitPrimary",DataRowVersion.Current);

	
				if (parameterDatasetMemberTelephoneDetails != null)
				{
					db.UpdateDataSet(parameterDatasetMemberTelephoneDetails,"Member Details" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}
        
		public static void Insert_MemberEmailAddress(DataSet parameterDatasetMemberEmailAddress)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;	

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_BS_Insert_MemberEmailAddress");
				// Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "guiUniqueID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityID", DbType.Guid, "guiEntityID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
				db.AddInParameter(insertCommandWrapper, "@varchar_EntityCode",DbType.String,"chvEntityCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@date_EffDate",DbType.DateTime,"dtmEffDate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_EmailCode",DbType.String,"chvEmailCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_EMailAddr",DbType.String,"chvEMailAddr",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitActive",DbType.Int16,"bitActive",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitPrimary",DbType.Int16,"bitPrimary",DataRowVersion.Current);

	
				if (parameterDatasetMemberEmailAddress != null)
				{
					db.UpdateDataSet(parameterDatasetMemberEmailAddress,"Member Details" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}

		public static void Update_MemberEmailAddress(DataSet parameterDatasetMemberEmailAddress)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;	

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_BS_Update_MemberEmailAddress");
				// Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueID", DbType.Guid, "guiUniqueID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityID", DbType.Guid, "guiEntityID", DataRowVersion.Current); //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
				db.AddInParameter(insertCommandWrapper, "@varchar_EntityCode",DbType.String,"chvEntityCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@date_EffDate",DbType.DateTime,"dtmEffDate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_EmailCode",DbType.String,"chvEmailCode",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_EMailAddr",DbType.String,"chvEMailAddr",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitActive",DbType.Int16,"bitActive",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_bitPrimary",DbType.Int16,"bitPrimary",DataRowVersion.Current);

					if (parameterDatasetMemberEmailAddress != null)
				{
					db.UpdateDataSet(parameterDatasetMemberEmailAddress,"Member Details" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}
        
		//NP:PS:2007.07.20 - Add code and parameters to allow simultaneous settlement of two plans
		//Returns an integer value that can have the following values
		//	-1 = Error creating the connection object/Command object, etc
		//	-2 = Error in the procedure call logic
		//	-99= Unknown unhandled exception
		public static int Update_FinalSettlementofBeneficiary(string paramDeathBenefitOptionID_RP, string paramAnnuityOption_RP, 
		    													double paramRolloverTaxable_RP, double paramRolloverNonTaxable_RP,  
																string paramRolloverInstitutionID_RP, double paramWithholdingPct_RP, 
																string paramDeathBenefitOptionID_SP, string paramAnnuityOption_SP, 
																double paramRolloverTaxable_SP, double paramRolloverNonTaxable_SP,  
																string paramRolloverInstitutionID_SP, double paramWithholdingPct_SP,
                                                                //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added to pass deduction values
                                                                string paramDedAnnuity,
                                                                string paramDedLumpsum,
                                                                //End - Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added to pass deduction values
                                                                out string outpara_ErrorMessage,
                                                                decimal decDeductionLumpsumAmount, //Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added for Deduction amt
                                                                Boolean paramIsRolloverToOwnIRA_RP, Boolean paramIsRolloverToOwnIRA_SP)  //  SB | 2016.11.25 | YRS-AT-3022 | Added parameters for RolloverTo own IRA option
		{
			/* Call  Procedure dbo.ap_Dth_Settle with 6 Parameters 
			 This Procedure is a wrapper proc  which will do the save one by one */
				
				int int_returnStatus = 1;

				Database db = null;
				DbCommand CommandWrapperFinalBeneficiarySettlement = null;
				outpara_ErrorMessage = string.Empty ;
                // Start - Manthan | 2016.04.22 | YRS-AT-2206 | Declared variable to store disbursement id for retirement and saving paln option and boolean value declared to check annuity and refund option and dataset variable to get disbrsement details
                string strRetdedDisbursementID = string.Empty;
                string strSavdedDisbursementID = string.Empty;
                Boolean blnIsAnnuityDeductionExist = false; 
                Boolean blnIsRefundDeductionExist = false;
                DataSet dsRetirementDisbursementDetails = new DataSet();
                DataSet dsSavingsDisbursementDetails = new DataSet();
                // End - Manthan | 2016.04.22 | YRS-AT-2206 | Declared variable to store disbursement id for retirement and saving paln option and boolean value declared to check annuity and refund option and dataset variable to get disbrsement details
                DbTransaction tran = null;
				DbConnection cn = null;

				string paramDeathBenefitOptionID = string.Empty; //added by hafiz 

			try 
			{ 
				//Obtain a Database object from the Factory
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return -1;

				//Get a connection and Open it
                cn = db.CreateConnection();
				cn.Open();

				//Get a Transaction from the Database
				tran = cn.BeginTransaction(IsolationLevel.Serializable);
				if (tran == null) return -1;

				//Execute the Retirement Plan Settlement Option only if the option is defined
				if (paramDeathBenefitOptionID_RP != null & paramDeathBenefitOptionID_RP != string.Empty) 
				{
					//Get a CommandWrapper for the Stored Procedure
					CommandWrapperFinalBeneficiarySettlement = db.GetStoredProcCommand("yrs_ap_Dth_Settle") ;
					if (CommandWrapperFinalBeneficiarySettlement == null) return -1;

					//Add input parameters to the CommandWrapper
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cDeathBenefitOptionID",DbType.String,paramDeathBenefitOptionID_RP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cAnnuityOption",DbType.String,paramAnnuityOption_RP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nRolloverTaxable",DbType.Double ,paramRolloverTaxable_RP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nRolloverNonTaxable",DbType.Double ,paramRolloverNonTaxable_RP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cRolloverInstitutionID",DbType.String,paramRolloverInstitutionID_RP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nWithholdingPct",DbType.Double ,paramWithholdingPct_RP);
                    db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@BIT_IsRolloverToOwnIRA", DbType.Boolean, paramIsRolloverToOwnIRA_RP);  //  SB | 2016.11.25 | YRS-AT-3022 | Added parameter for RolloverTo own IRA option for Retirement plan
                    //Set the Command timeout value
					CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
					//Add the output parameter to the command
					db.AddOutParameter(CommandWrapperFinalBeneficiarySettlement, "@cOutput",DbType.String ,1000);
					//Execute the command
                    //db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement, tran); //Manthan | 2016.04.22 | YRS-AT-2206 | In order to return disbursement details datatable, commented existing code of ExecuteNonQuery and replaced with load dataset
                    db.LoadDataSet(CommandWrapperFinalBeneficiarySettlement, dsRetirementDisbursementDetails, "RetirementDisbursementDetails", tran); //Manthan | 2016.04.22 | YRS-AT-2206 | Loading disbursement details into dataset
                    //Read the output parameter
					//outpara_ErrorMessage = Convert.ToString( CommandWrapperFinalBeneficiarySettlement.GetParameterValue("@cOutput") );
                    outpara_ErrorMessage = db.GetParameterValue(CommandWrapperFinalBeneficiarySettlement, "@cOutput").ToString();

                    //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Setting disbursement ID based on option selected for annuity and lumpsum option from Retirement plan 
                    //Start - Manthan | 2016.06.13 | YRS-AT-2206 | Commented existing code and validating dataset                   
                    //if (dsRetirementDisbursementDetails != null && dsRetirementDisbursementDetails.Tables.Count > 0 && dsRetirementDisbursementDetails.Tables[0].Rows.Count > 0) 
                    if (HelperFunctions.isNonEmpty(dsRetirementDisbursementDetails))
                    //End - Manthan | 2016.06.13 | YRS-AT-2206 | Commented existing code and validating dataset                   
                    {
                               if (dsRetirementDisbursementDetails.Tables[0].Rows.Count > 1)
                                {
                                    DataRow[] drPersonDisbursement;
                                    DataRow[] drRolloverDisbursement;
                                    drPersonDisbursement = dsRetirementDisbursementDetails.Tables[0].Select("chvPayee = 'Payee1'");
                                    drRolloverDisbursement = dsRetirementDisbursementDetails.Tables[0].Select("chvPayee = 'Payee2'");
                                    if (drPersonDisbursement.Length >= 1 && (Convert.ToDecimal(drPersonDisbursement[0]["numDisbursementAmount"]) > decDeductionLumpsumAmount))
                                    {
                                        strRetdedDisbursementID = drPersonDisbursement[0]["guiUniqueid"].ToString();

                                    }
                                    else if (drRolloverDisbursement.Length >= 1 && (Convert.ToDecimal(drRolloverDisbursement[0]["numDisbursementAmount"]) > decDeductionLumpsumAmount))
                                    {
                                        strRetdedDisbursementID = drRolloverDisbursement[0]["guiUniqueid"].ToString();
                                    }
                                }
                                else {
                                    strRetdedDisbursementID = dsRetirementDisbursementDetails.Tables[0].Rows[0]["guiUniqueid"].ToString();                              
                                     }   
                        }                  
                    //End - Manthan | 2016.04.22 | YRS-AT-2206 | Setting disbursement ID based on option selected for annuity and lumpsum option from Retirement plan

					//Check output parameter for errors
					if (outpara_ErrorMessage.Trim() != string.Empty) 
					{	// There was a logic error in the system. Rollback the transaction and exit the function
						tran.Rollback();
						return -2;
					} 
				}             
				//Execute the Savings Plan Settlement Option only if the option is defined
				if (paramDeathBenefitOptionID_SP != null & paramDeathBenefitOptionID_SP != string.Empty) 
				{
					//Get a CommandWrapper for the Stored Procedure
					CommandWrapperFinalBeneficiarySettlement = db.GetStoredProcCommand("yrs_ap_Dth_Settle") ;
					if (CommandWrapperFinalBeneficiarySettlement == null) return -1;

					//Add input parameters to the CommandWrapper
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cDeathBenefitOptionID",DbType.String,paramDeathBenefitOptionID_SP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cAnnuityOption",DbType.String,paramAnnuityOption_SP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nRolloverTaxable",DbType.Double ,paramRolloverTaxable_SP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nRolloverNonTaxable",DbType.Double ,paramRolloverNonTaxable_SP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cRolloverInstitutionID",DbType.String,paramRolloverInstitutionID_SP );
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nWithholdingPct",DbType.Double ,paramWithholdingPct_SP);
                    db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@BIT_IsRolloverToOwnIRA", DbType.Boolean, paramIsRolloverToOwnIRA_SP);  //  SB | 2016.11.25 | YRS-AT-3022 | Added parameters for RolloverTo own IRA option for Savings plan
                    //Set the Command timeout value
					CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
					//Add the output parameter to the command
					db.AddOutParameter(CommandWrapperFinalBeneficiarySettlement, "@cOutput",DbType.String ,1000);                   
					//Execute the command
                    //db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement, tran) ; //Manthan | 2016.04.22 | YRS-AT-2206 | In order to return disbursement details datatable, commented existing code of ExecuteNonQuery and replaced with load dataset.
                    db.LoadDataSet(CommandWrapperFinalBeneficiarySettlement, dsSavingsDisbursementDetails, "SavingDisbursementDetails", tran); //Manthan | 2016.04.22 | YRS-AT-2206 | Loading disbursement details from into dataset
					//Read the output parameter
                    //outpara_ErrorMessage = Convert.ToString(CommandWrapperFinalBeneficiarySettlement.GetParameterValue("@cOutput");
                    outpara_ErrorMessage = db.GetParameterValue(CommandWrapperFinalBeneficiarySettlement, "@cOutput").ToString();
                    
                    //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Setting disbursement ID based on option selected for annuity and lumpsum option from Savings plan 
                    //Start - Manthan | 2016.06.13 | YRS-AT-2206 | Commented existing code and validating dataset                                                           
                    //if (dsSavingsDisbursementDetails != null && dsSavingsDisbursementDetails.Tables.Count > 0 && dsSavingsDisbursementDetails.Tables[0].Rows.Count > 0)
                    if (HelperFunctions.isNonEmpty(dsSavingsDisbursementDetails))
                    //End - Manthan | 2016.06.13 | YRS-AT-2206 | Commented existing code and validating dataset                   
                    {
                              if (dsSavingsDisbursementDetails.Tables[0].Rows.Count > 1)
                                {
                                    DataRow[] drPersonDisbursement ;
                                    DataRow[] drRolloverDisbursement;
                                    drPersonDisbursement = dsSavingsDisbursementDetails.Tables[0].Select("chvPayee = 'Payee1'");
                                    drRolloverDisbursement = dsSavingsDisbursementDetails.Tables[0].Select("chvPayee = 'Payee2'");
                                    if (drPersonDisbursement.Length >= 1 && (Convert.ToDecimal(drPersonDisbursement[0]["numDisbursementAmount"]) > decDeductionLumpsumAmount))
                                    {
                                        strSavdedDisbursementID = drPersonDisbursement[0]["guiUniqueid"].ToString();

                                    }
                                    else if (drRolloverDisbursement.Length >= 1 && (Convert.ToDecimal(drRolloverDisbursement[0]["numDisbursementAmount"]) > decDeductionLumpsumAmount))
                                    {
                                        strSavdedDisbursementID = drRolloverDisbursement[0]["guiUniqueid"].ToString();
                                    }
                                }
                                else
                                {
                                    strSavdedDisbursementID = dsSavingsDisbursementDetails.Tables[0].Rows[0]["guiUniqueid"].ToString();
                                }
                        }                    
                    //End - Manthan | 2016.04.22 | YRS-AT-2206 | Setting disbursement ID based on option selected for annuity and lumpsum option from Savings plan
                                        
					//Check output parameter for errors
					if (outpara_ErrorMessage.Trim() != string.Empty) 
					{	// There was a logic error in the system. Rollback the transaction and exit the function
						tran.Rollback();
						return -2;
					}
				}

                //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Setting disbursement ID based on Rollover and lumpsum taken from retirement and Savings plan
                //Start - Manthan | 2016.06.13 | YRS-AT-2206 | Commented existing code and validating dataset          
                //if (dsRetirementDisbursementDetails != null && dsSavingsDisbursementDetails != null && string.IsNullOrEmpty(paramAnnuityOption_RP) && string.IsNullOrEmpty(paramAnnuityOption_SP) && Convert.ToDecimal(decDeductionLumpsumAmount) > 0)
                if (HelperFunctions.isNonEmpty(dsRetirementDisbursementDetails) && HelperFunctions.isNonEmpty(dsSavingsDisbursementDetails) && string.IsNullOrEmpty(paramAnnuityOption_RP) && string.IsNullOrEmpty(paramAnnuityOption_SP) && Convert.ToDecimal(decDeductionLumpsumAmount) > 0)
                //End - Manthan | 2016.06.13 | YRS-AT-2206 | Commented existing code and validating dataset          
                {
                    if (dsRetirementDisbursementDetails.Tables[0].Rows.Count > 1 && dsSavingsDisbursementDetails.Tables[0].Rows.Count > 1)
                    {
                        DataRow[] drPersonDisbursement_RP;
                        DataRow[] drRolloverDisbursement_RP;
                        DataRow[] drPersonDisbursement_SP;
                        DataRow[] drRolloverDisbursement_SP;

                        drPersonDisbursement_RP = dsRetirementDisbursementDetails.Tables[0].Select("chvPayee = 'Payee1'");
                        drRolloverDisbursement_RP = dsRetirementDisbursementDetails.Tables[0].Select("chvPayee = 'Payee2'");

                        drPersonDisbursement_SP = dsSavingsDisbursementDetails.Tables[0].Select("chvPayee = 'Payee1'");
                        drRolloverDisbursement_SP = dsSavingsDisbursementDetails.Tables[0].Select("chvPayee = 'Payee2'");

                        if (drRolloverDisbursement_RP.Length >= 1 && drRolloverDisbursement_SP.Length >= 1)
                        {
                            if ((Convert.ToDecimal(drPersonDisbursement_RP[0]["numDisbursementAmount"]) + Convert.ToDecimal(drPersonDisbursement_SP[0]["numDisbursementAmount"])) > decDeductionLumpsumAmount)
                            {
                                strRetdedDisbursementID = drPersonDisbursement_RP[0]["guiUniqueid"].ToString();
                            }
                            else if (Convert.ToDecimal(drRolloverDisbursement_RP[0]["numDisbursementAmount"]) > decDeductionLumpsumAmount)
                            {
                                strRetdedDisbursementID = drRolloverDisbursement_RP[0]["guiUniqueid"].ToString();
                            }
                            else if (Convert.ToDecimal(drRolloverDisbursement_SP[0]["numDisbursementAmount"]) > decDeductionLumpsumAmount)
                            {
                                strSavdedDisbursementID = drRolloverDisbursement_SP[0]["guiUniqueid"].ToString();
                            }
                        }
                        else
                        {
                            strRetdedDisbursementID = drPersonDisbursement_RP[0]["guiUniqueid"].ToString();
                            strSavdedDisbursementID = drPersonDisbursement_SP[0]["guiUniqueid"].ToString();
                        }
                    }

                }
                //End - Manthan | 2016.04.22 | YRS-AT-2206 | Setting disbursement ID based on Rollover and lumpsum taken from retirement and Savings plan                

                //Start - Manthan | 2016.04.22 | YRS-AT-2206 | checking condition for annuity and refund option from both and indvidual plan before inserting disbursement withholding to avoid duplicate entry
                if (!(string.IsNullOrEmpty(paramDeathBenefitOptionID_RP)))
                {
                    CommandWrapperFinalBeneficiarySettlement = db.GetStoredProcCommand("yrs_usp_Insert_DisbursementWithholding");
                    if (CommandWrapperFinalBeneficiarySettlement == null) return -1;
                    if (!(string.IsNullOrEmpty(strRetdedDisbursementID)) & !(string.IsNullOrEmpty(paramAnnuityOption_RP)) & !(string.IsNullOrEmpty(paramDedAnnuity)))
                    {
                        blnIsAnnuityDeductionExist = true;
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@XML_DEDUCTIONDETAILS", DbType.Xml, paramDedAnnuity);
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@DeductionsDisbID", DbType.String, strRetdedDisbursementID);
                        CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                        db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement, tran);
                    }
                    else if (!(string.IsNullOrEmpty(strRetdedDisbursementID)) & !(string.IsNullOrEmpty(paramDedLumpsum)))
                    {
                        blnIsRefundDeductionExist = true;
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@XML_DEDUCTIONDETAILS", DbType.Xml, paramDedLumpsum);
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@DeductionsDisbID", DbType.String, strRetdedDisbursementID);
                        CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                        db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement, tran);
                    }
                    
                }

                if (!(string.IsNullOrEmpty(paramDeathBenefitOptionID_SP)))
                {

                    if (!(string.IsNullOrEmpty(strSavdedDisbursementID)) & !(string.IsNullOrEmpty(paramAnnuityOption_SP)) & !(string.IsNullOrEmpty(paramDedAnnuity)) & (blnIsAnnuityDeductionExist == false))
                    {
                        CommandWrapperFinalBeneficiarySettlement = db.GetStoredProcCommand("yrs_usp_Insert_DisbursementWithholding");
                        if (CommandWrapperFinalBeneficiarySettlement == null) return -1;
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@XML_DEDUCTIONDETAILS", DbType.Xml, paramDedAnnuity);
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@DeductionsDisbID", DbType.String, strSavdedDisbursementID);
                        CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                        db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement, tran);
                    }
                    else if (!(string.IsNullOrEmpty(strSavdedDisbursementID)) & string.IsNullOrEmpty(paramAnnuityOption_SP) & !(string.IsNullOrEmpty(paramDedLumpsum)) & (blnIsRefundDeductionExist == false))
                    {
                        CommandWrapperFinalBeneficiarySettlement = db.GetStoredProcCommand("yrs_usp_Insert_DisbursementWithholding");
                        if (CommandWrapperFinalBeneficiarySettlement == null) return -1;
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@XML_DEDUCTIONDETAILS", DbType.Xml, paramDedLumpsum);
                        db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@DeductionsDisbID", DbType.String, strSavdedDisbursementID);
                        CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                        db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement, tran);
                    }

                }
                //End - Manthan | 2016.04.22 | YRS-AT-2206 | checking condition for annuity and refund option from both and indvidual plan  before inserting disbursement withholding to avoid duplicate entry                

				//NP:PS:2007.11.05 - Executing the code to give Special Dividend to any annuities that may be created outside the settlement procedures.
				if ((outpara_ErrorMessage.Trim() == string.Empty) & ((paramDeathBenefitOptionID_RP != null) || (paramDeathBenefitOptionID_SP != null)))
				{	
					
					//start - added by hafiz 
					if (paramDeathBenefitOptionID_RP != null & paramDeathBenefitOptionID_RP != string.Empty)
					{
						paramDeathBenefitOptionID = paramDeathBenefitOptionID_RP;
					}
					else if(paramDeathBenefitOptionID_SP  != null & paramDeathBenefitOptionID_SP != string.Empty){
						paramDeathBenefitOptionID = paramDeathBenefitOptionID_SP;
					}
					//start - added by hafiz 

					//There was no error in performing the settlements. We need to run procedure to 
					//give special dividends if applicable to any annuities that were created.
					//Get a CommandWrapper for the Stored Procedure
					CommandWrapperFinalBeneficiarySettlement = db.GetStoredProcCommand("yrs_ap_Dth_InsertExpDividendDisbursements") ;
					if (CommandWrapperFinalBeneficiarySettlement == null) return -1;

					//Add input parameters to the CommandWrapper
					//CommandWrapperFinalBeneficiarySettlement.AddInParameter("@cDeathBenefitOptionID",DbType.String,paramDeathBenefitOptionID_SP ); //commented by Hafiz 
					db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cDeathBenefitOptionID",DbType.String,paramDeathBenefitOptionID );//added by Hafiz 
					
					//Set the Command timeout value
					CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
					//Add the output parameter to the command
					db.AddOutParameter(CommandWrapperFinalBeneficiarySettlement, "@cOutput",DbType.String ,1000);
					//Execute the command
					db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement, tran) ;
					//Read the output parameter
					//outpara_ErrorMessage = Convert.ToString( CommandWrapperFinalBeneficiarySettlement.GetParameterValue("@cOutput") );
                    outpara_ErrorMessage = db.GetParameterValue(CommandWrapperFinalBeneficiarySettlement, "@cOutput").ToString();
					//Check output parameter for errors
					if (outpara_ErrorMessage.Trim() != string.Empty) 
					{	// There was a logic error in the system. Rollback the transaction and exit the function
						tran.Rollback();
						return -2;
					}
				}
				//Commit the transaction if everything was fine
				tran.Commit();
				return int_returnStatus ;

			}
			catch (Exception ex)
			{
				//Error encountered, rollback the transaction
				if (tran != null) tran.Rollback();
				if (cn != null) cn.Close();
				throw ex;
				//return -99;
			} 
//TODOMIGRATION - Probably need to add the finally block here
//			}

		}
        
		public static int Update_FinalSettlementofBeneficiary(string paramDeathBenefitOptionID, string paramAnnuityOption, 
			double paramRolloverTaxable, double paramRolloverNonTaxable,  
			string paramRolloverInstitutionID, double paramWithholdingPct, 
			out string outpara_ErrorMessage )
		{
			/* Call  Procedure dbo.ap_Dth_Settle with 6 Parameters 
			 This Procedure is a wrapper proc  which will do the save one by one */
				
			int int_returnStatus ; 
			int_returnStatus = 1 ;

			Database db = null;
			DbCommand CommandWrapperFinalBeneficiarySettlement = null;
			outpara_ErrorMessage = "";	
			try 
			{ 
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return -1;

				CommandWrapperFinalBeneficiarySettlement = db.GetStoredProcCommand("yrs_ap_Dth_Settle") ;
				
				if (CommandWrapperFinalBeneficiarySettlement == null) return -1;

				db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cDeathBenefitOptionID",DbType.String,paramDeathBenefitOptionID );
				db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cAnnuityOption",DbType.String,paramAnnuityOption );
				db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nRolloverTaxable",DbType.Double ,paramRolloverTaxable );
				db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nRolloverNonTaxable",DbType.Double ,paramRolloverNonTaxable );
				db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@cRolloverInstitutionID",DbType.String,paramRolloverInstitutionID );
				db.AddInParameter(CommandWrapperFinalBeneficiarySettlement, "@nWithholdingPct",DbType.Double ,paramWithholdingPct);

				CommandWrapperFinalBeneficiarySettlement.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				db.AddOutParameter(CommandWrapperFinalBeneficiarySettlement, "@cOutput",DbType.String ,1000);
			
				db.ExecuteNonQuery(CommandWrapperFinalBeneficiarySettlement) ;

				//outpara_ErrorMessage = Convert.ToString( CommandWrapperFinalBeneficiarySettlement.GetParameterValue("@cOutput") );
                outpara_ErrorMessage = db.GetParameterValue(CommandWrapperFinalBeneficiarySettlement, "@cOutput").ToString();

				return int_returnStatus ;

			}
			catch
			{
				throw;
			}

			//			}

		}


		//NP:YRPS-4009:2008.01.23 - Function to perform validations on Death Settlement Option
		// Currently this checks if there are any unfunded transactions for the Deceased participant for non-retired funds
		public static int PerformPreRequisiteValidations(string paramDeathBenefitOptionID, out string outpara_ErrorMessage) 
		{
			
			/* Call  Procedure yrs_Dth_Settle_PerformValidations with 1 Parameter */

			Database db = null;
			DbCommand CommandWrapperPerformValidation = null;
			outpara_ErrorMessage = "";	
			try 
			{ 

				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return -1;

				CommandWrapperPerformValidation = db.GetStoredProcCommand("yrs_Dth_Settle_PerformValidations");
				
				if (CommandWrapperPerformValidation == null) return -1;

				db.AddInParameter(CommandWrapperPerformValidation, "@cDeathBenefitOptionID",DbType.String,paramDeathBenefitOptionID);
				CommandWrapperPerformValidation.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				db.AddOutParameter(CommandWrapperPerformValidation, "@cOutput",DbType.String ,1000);
			
				db.ExecuteNonQuery(CommandWrapperPerformValidation) ;

				//outpara_ErrorMessage = Convert.ToString( CommandWrapperPerformValidation.GetParameterValue("@cOutput") );
                outpara_ErrorMessage = db.GetParameterValue(CommandWrapperPerformValidation, "@cOutput").ToString();

				if (outpara_ErrorMessage.Length > 0) 
					return -1;
				else 
					return 1 ;

			}
			catch
			{
				throw;
			}

		}

        //NP:IVP2:2008.12.05 - Adding additional parameter for either of the benefit option used for settlement (RP or SP)
        public static string Insert_BS_FundEventData(string parameterBeneFundEventID, string parameterOrigFundEventID, string parameterPersid, string parameterBenefitOptionId)
        {

            Database db = null;
            DbCommand InsertCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Insert_FundEventData");



                if (InsertCommandWrapper == null) return null;

                db.AddInParameter(InsertCommandWrapper, "@varchar_BeneFundEventID", DbType.String, parameterBeneFundEventID);
                db.AddInParameter(InsertCommandWrapper, "@varchar_Persid", DbType.String, parameterPersid);
                db.AddInParameter(InsertCommandWrapper, "@varchar_OrigFundEventID", DbType.String, parameterOrigFundEventID);
                db.AddInParameter(InsertCommandWrapper, "@date_EffectiveDate", DbType.DateTime, System.DateTime.Now.Date);
                //NP:IVP2:2008.12.05 - Adding additional parameter for benefit option id
                db.AddInParameter(InsertCommandWrapper, "@varchar_BenefitOptionId", DbType.String, parameterBenefitOptionId);

                db.ExecuteNonQuery(InsertCommandWrapper);
                return "Done";
            }
            catch
            {
                throw;
            }

        }

        //NP:2008.01.03:YRPS-4046 - Changing Parameter from SSNo to BenefitOptionId
        public static string Update_BS_BeneficiariesFunEventandPersID(string parameterBeneFundEventID, string parameterPersid, string parameterBenefitOptionId)
        {

            Database db = null;
            DbCommand UpdateCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Update_BeneficiariesFunEventandPersID");



                if (UpdateCommandWrapper == null) return null;

                db.AddInParameter(UpdateCommandWrapper, "@varchar_BeneFundEventID", DbType.String, parameterBeneFundEventID);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Persid", DbType.String, parameterPersid);
                //UpdateCommandWrapper.AddInParameter("@varchar_SSNo",DbType.String,parameterSSNo);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_BenefitOptionId", DbType.String, parameterBenefitOptionId);	//NP:2008.01.03:YRPS-4046 - Passing BenefitOptionId

                db.ExecuteNonQuery(UpdateCommandWrapper);
                return "Done";
            }
            catch
            {
                throw;
            }

        }

       //By Aparna -YREN-3015
        public static void Insert_AtsperssBanking(string ParticipantEntityId, string BeneficiaryEntityId)
        {


            Database db = null;
            DbCommand InsertBankRecord = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                InsertBankRecord = db.GetStoredProcCommand("yrs_usp_BI_InsertintoAtsPersbanking");
                InsertBankRecord.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (InsertBankRecord != null)
                {
                    db.AddInParameter(InsertBankRecord, "@uniqueIdentifier_ParticipantEntityID", DbType.String, ParticipantEntityId);
                    db.AddInParameter(InsertBankRecord, "@uniqueIdentifier_BeneficiaryEntityId", DbType.String, BeneficiaryEntityId);
                    db.ExecuteNonQuery(InsertBankRecord);
                }
            }
            catch
            {
                throw;
            }
        }

        public static void Insert_BeneficiaryMember(DataSet parameterDatasetMemberDetails)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;




            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Insert_PerssDeatils");
                // Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniquePersID", DbType.Guid, "guiUniqueID", DataRowVersion.Current);  //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@chr_SSno", DbType.String, "chrSSNo", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_LastName", DbType.String, "chvLastName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_FirstName", DbType.String, "chvFirstName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_MiddleName", DbType.String, "chvMiddleName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_SalutationCode", DbType.String, "chvSalutationCode", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_SuffixTitle", DbType.String, "chvSuffixTitle", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_GenderCode", DbType.String, "chvgenderCode", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@date_BirthDate", DbType.DateTime, "dtmBirthDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@date_DeathDate", DbType.DateTime, "dtmDeathDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_MaritalStatusCode", DbType.String, "chvMaritalStatusCode", DataRowVersion.Current);


                if (parameterDatasetMemberDetails != null)
                {
                    db.UpdateDataSet(parameterDatasetMemberDetails, "Member Details", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }

        public static void Update_BeneficiaryMember(DataSet parameterDatasetMemberDetails)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;




            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_Update_PerssDeatils");
                // Defining The Insert Command Wrapper With parameters

                db.AddInParameter(insertCommandWrapper, "@varchar_UniquePersID", DbType.Guid, "guiUniqueID", DataRowVersion.Current);  //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.21 for migration
                db.AddInParameter(insertCommandWrapper, "@chr_SSno", DbType.String, "chrSSNo", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_LastName", DbType.String, "chvLastName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_FirstName", DbType.String, "chvFirstName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_MiddleName", DbType.String, "chvMiddleName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_SalutationCode", DbType.String, "chvSalutationCode", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_SuffixTitle", DbType.String, "chvSuffixTitle", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_GenderCode", DbType.String, "chvgenderCode", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@date_BirthDate", DbType.DateTime, "dtmBirthDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@date_DeathDate", DbType.DateTime, "dtmDeathDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_MaritalStatusCode", DbType.String, "chvMaritalStatusCode", DataRowVersion.Current);


                if (parameterDatasetMemberDetails != null)
                {
                    db.UpdateDataSet(parameterDatasetMemberDetails, "Member Details", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }

        #endregion

        //--new Created by --BS:2011.08.10:YRS 5.0-1339:BT:852 - Reopen issue 
        public static DataSet LookUp_NonHumanBenfInfo(string paramBenefitOptionID)
        {
            DataSet l_dataset_BS_LookUp_NonHumanBenfInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_SearchNonHumanData");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@BenefitOptionID", DbType.String, paramBenefitOptionID);

                l_dataset_BS_LookUp_NonHumanBenfInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_BS_LookUp_NonHumanBenfInfo, "vrBeneficiaries");
                return l_dataset_BS_LookUp_NonHumanBenfInfo;
            }
            catch
            {
                throw;
            }
        }

        //SP 2014.04.01 BT-2420\YRS 5.0-2309 : Beneficiary Settlement - Retired Death -Start

        public static bool CheckAnnuityReversedAfterDeathBeneficiaryOptionCreated(string paramParticipantGuiPersID)
        {
           
            Database db = null;
            DbCommand dbCommand = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return false;

                dbCommand = db.GetStoredProcCommand("yrs_usp_BS_CheckAnnuityReversedAfterDeathCalc");

                if (dbCommand == null) return false;

                db.AddInParameter(dbCommand, "@guiPersID", DbType.String, paramParticipantGuiPersID);
                db.AddOutParameter(dbCommand, "@bitAnnuityReversedAfterDeathCalc", DbType.Boolean, 6);

                db.ExecuteScalar(dbCommand);
                return Convert.ToBoolean(db.GetParameterValue(dbCommand, "@bitAnnuityReversedAfterDeathCalc"));
            }
            catch
            {
                throw;
            }

        }
        //SP 2014.04.01 BT-2420\YRS 5.0-2309 : Beneficiary Settlement - Retired Death -End

        //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Retreiving deduction values from database for death settlement
        public static DataSet GetDeductions()
        {
            DataSet dsDeductions = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_GetDeductions");

                if (LookUpCommandWrapper == null) return null;
                dsDeductions = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsDeductions, "Deductions");
                return dsDeductions;
            }
            catch
            {
                throw;
            }
        }
        //End - Manthan | 2016.04.22 | YRS-AT-2206 | Retreiving deduction values from database for death settlement

        //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Getting No of payroll months based on Annutiy purchase date
        public static int GetPastPayrollCount(DateTime dtmAnnuityPurchaseDate)
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;            
            DbConnection cn = null;
            int intPayrollMonthsCount;
            try 
			{ 
				//Obtain a Database object from the Factory
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return -1;

				//Get a connection and Open it
                cn = db.CreateConnection();
				cn.Open();
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_GetPayrollMonths") ;
					if (LookUpCommandWrapper == null) return -1;

					//Add input parameters to the CommandWrapper
                    db.AddInParameter(LookUpCommandWrapper, "@dtmAnnuityPurchaseDate", DbType.DateTime, dtmAnnuityPurchaseDate);
                    db.AddOutParameter(LookUpCommandWrapper, "@intPayrollMonthcount", DbType.Int32, 10);
                    db.ExecuteNonQuery(LookUpCommandWrapper);
                    intPayrollMonthsCount = Convert.ToInt32(db.GetParameterValue(LookUpCommandWrapper, "@intPayrollMonthcount"));
                    return intPayrollMonthsCount;
            }
            catch
            {
                throw;
            }
         }
        //End - Manthan | 2016.04.22 | YRS-AT-2206 | Getting No of payroll months based on Annutiy purchase date
    }
    
}
