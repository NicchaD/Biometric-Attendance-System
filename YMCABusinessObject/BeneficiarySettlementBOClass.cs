//****************************************************
//Modification History
//****************************************************
//Modified by       Date        Description
//****************************************************
//	Nikunj Patel	2007.07.20	Adding extra parameters to the settlement process to enable simultaneous settlement of both plans
//  Nikunj Patel	2007.09.06	Adding parameter FundNo to perform searches by Fund number
//  Nikunj Patel	2007.09.07	Removing logic for computing Totals etc from the BO layer to make the process consistent with the Death Calculator
//	Nikunj Patel	2008.01.03	YRPS-4046 Adding code to pass the selected Settlement Option Id to identify fund Event
//	Nikunj Patel	2008.01.23	YRPS-4009 Adding code to perform Pre-Settlement Validations
//	Nikunj Patel    2008.12.05  Passing Selected benefit Option Id so that the new fund event if required is created of the right type either DBEN or RBEN.
//  Bhavna S        2011.08.10  YRS 5.0-1339:BT:852 - Reopen issue 
// Shashank Patel	2013.04.12  YRS 5.0-1990:similar SSNs are being updates across the board
// Shashank Patel   2014.02.01  BT-2420\YRS 5.0-2309 : Beneficiary Settlement - Retired Death 
// Manthan Rajguru  2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
// Manthan Rajguru  2016.04.22  YRS-AT-2206 -  Applying Fees and deductions to death payments - Part A.1 
// Santosh Bura		2016.11.25 	YRS-AT-3022 -  YRS enhancement.--YRS death settlement screen.Track it 26636
//****************************************************
using System; 
using YMCARET.YmcaDataAccessObject;
using System.Data ;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for BeneficiarySettlement.
	/// </summary>
	public class BeneficiarySettlement
	{
		public BeneficiarySettlement()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		//NP:PS:2007.09.06 - Adding parameter FundNo to perform searches by Fund number
		public static DataSet LookUp_BS_MemberListForDeceased(string lcSSNo,string parameterLName , string parameterFName, string parameterFundNo )
		{	
			
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_Beneficiaries_MemberListDeseased(lcSSNo, parameterLName, parameterFName, parameterFundNo)) ;	//NP:PS:2007.09.06 - Adding parameter FundNo to perform searches by Fund number

			}
			catch 
			{
				throw;
			}

		}

		//
		public static DataSet LookUp_BS_ActiveorRetiredDetails(string ParameterPersID )
		{	
			
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_ActiveorRetiredDetails(ParameterPersID)) ;
			}
			catch 
			{
				throw;
			}

		}

		public static DataSet LookUp_BS_BeneficiariesPrimeSettle(string ParameterPersID,string parameterStatus )
		{	
			
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_Beneficiaries_BeneficiaryPrimesettle(ParameterPersID,parameterStatus)) ;
			}
			catch 
			{
				throw;
			}

		}

		public static DataSet LookUp_BI_BeneficiaryInformation(string ParameterBenefitOptionID )
		{	
			
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_BeneficiaryInformation(ParameterBenefitOptionID));
			}
			catch 
			{
				throw;
			}

		}


		public static DataSet LookUp_BS_DeathBenefitOptions(string ParameterPersID,string parameterStatus)
		{	
			
			try
			{
				DataSet l_DataSet_DeathBenefitOptionWithTotal=new DataSet();
				l_DataSet_DeathBenefitOptionWithTotal=null;
				l_DataSet_DeathBenefitOptionWithTotal=YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_Beneficiaries_DeathBenefitOptions(ParameterPersID,parameterStatus);

				//NP:PS:2007.09.07 - Replacing all computations into the stored procedure which would be based on what is seen in Death Calculator
				//				//Changed On:28Feb06 By:Preeti For handling object conversion error.
				//				double PIA =0.0;
				//				double DeathBenefit=0.0;
				//				double Reserve=0.0;
				//
				//
				//				if(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows.Count>0 && l_DataSet_DeathBenefitOptionWithTotal.Tables[0]!=null)
				//				{
				//					l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Columns.Add("Total");
				//					for(int i=0;i<l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows.Count;i++)
				//					{
				//						//If death date is before 1 july then show column PIA and Reserves
				//						try
				//						{ 
				//							// If PIA column is not returned then It will throw an exception as this column is renamed as Voluntary.
				//							if (l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["PIA"] is System.DBNull  ||  l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["PIA"]==  null) 
				//							{
				//								PIA=0.0;
				//							}
				//							else
				//							{
				//								PIA=double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["PIA"].ToString());
				//							}
				//							if (l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Reserves"] is System.DBNull  ||  l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Reserves"]==  null) 
				//							{
				//								Reserve=0.0;
				//							}
				//							else
				//							{
				//								Reserve=double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Reserves"].ToString());
				//							}
				//						}
				//						catch
				//						{
				//							// Else show him Voluntary and Reserves would be parted in Personal Res and YMCA Res
				//							if (l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["VOL"] is System.DBNull  ||  l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["VOL"]==  null) 
				//							{
				//								PIA=0.0;
				//							}
				//							else
				//							{
				//								PIA=double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["VOL"].ToString());
				//							}
				//							if (l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Personal Reserves"] is System.DBNull  ||  l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Personal Reserves"]==  null) 
				//							{
				//								Reserve=0.0;
				//							}
				//							else
				//							{
				//								Reserve= double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Personal Reserves"].ToString());
				//							}
				//							if (l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["YMCA Reserves"] is System.DBNull  ||  l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["YMCA Reserves"]==  null) 
				//							{
				//								Reserve=0.0;
				//							}
				//							else
				//							{
				//								// Reserve should be YMCA reserve + Personal Reserve. So it has to be Reserve ( Calculated above) + Ymca Reserve
				//								Reserve=Reserve + double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["YMCA Reserves"].ToString());
				//							}
				//						}
				//						
				//
				//
				//						if (l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Death Benefit"] is System.DBNull  ||  l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Death Benefit"]==  null) 
				//						{
				//							DeathBenefit=0.0;
				//						}
				//						else
				//						{
				//							DeathBenefit=double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Death Benefit"].ToString());
				//						}
				//					
				//						//Vipul 11Dec06 Fixing issue of Death Calculator
				//						//l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Total"]=Convert.ToString(PIA+ DeathBenefit +Reserve);
				// 						if (double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Post01July"].ToString() )== 0)
				//							{
				//								l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Total"]=Convert.ToString(PIA+ DeathBenefit +Reserve);
				//							}
				//							else
				//							{
				//								double Voluntary=0.0;							
				//								Voluntary = double.Parse(l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Voluntary"].ToString()) ;
				//								l_DataSet_DeathBenefitOptionWithTotal.Tables[0].Rows[i]["Total"]=Convert.ToString(PIA+ DeathBenefit + Reserve + Voluntary );
				//							}
				//
				//
				//					}
				//				}
				//NP:PS:2007.09.07 - Replacing all computations into the stored procedure which would be based on what is seen in Death Calculator
				return l_DataSet_DeathBenefitOptionWithTotal;
			}
			catch 
			{
				throw;
			}

		}

		public static DataSet  LookUp_BI_BeneficiaryPersonalDetails(string parameterSSNo)
		{
				
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_BeneficiaryPersonalDetails(parameterSSNo));
			}
			catch 
			{
				throw;
			}
							  
		}


		public static DataSet  LookUp_BI_DeathBeneficiaryOption(string parameterDeathBeneOptionID)
		{
				
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_DeathBeneficiaryOption(parameterDeathBeneOptionID));
			}
			catch 
			{
				throw;
			}
											  
		}
	
		public static DataSet  LookUp_BI_AnnuityCount(string parameterOption)
		{
		
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_AnnuityCount(parameterOption));
			}
			catch 
			{
				throw;
			}
										
		}

		//
		public static DataSet  LookUp_BI_PersonalDetailsAll(string parameterPersPK,string parameterType)
		{
		
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_PersonalDetailsAll(parameterPersPK,parameterType));
			}
			catch 
			{
				throw;
			}
										
		}


		public static  DataSet LookUp_BI_NewBeneficiary(string parameterDeathBeneOptionID)
		{
		
			try
			{
				return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_NewBeneficiary(parameterDeathBeneOptionID));
			}
			catch 
			{
				throw;
			}
										
		}

		public static void LookUp_BI_ExistingSSNo(string parameterSSNo,out string l_string_Message)
		{

			try
			{
				if(YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_ExistingSSNo(parameterSSNo).Tables[0].Rows.Count>0)
				{
					DataTable l_DataTable_Record=new DataTable();
					l_DataTable_Record=YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BI_ExistingSSNo(parameterSSNo).Tables[0];

					l_string_Message="SSNo. is Already in use for Participant: "+l_DataTable_Record.Rows[0]["chvFirstname"].ToString()+"  "+l_DataTable_Record.Rows[0]["chvLastname"].ToString();
					l_string_Message=l_string_Message+"\n"+"Record Created by:"+l_DataTable_Record.Rows[0]["chvCreator"].ToString()+" "+l_DataTable_Record.Rows[0]["dtmCreated"].ToString();
					l_string_Message=l_string_Message+"\n"+" Do you want to continue?";
				}
				else
					l_string_Message="";
			}
			catch 
			{
				throw;
			}
		}


		public static void Insert_BeneficiaryMember(DataSet parameterDatasetMemberDetails)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Insert_BeneficiaryMember(parameterDatasetMemberDetails);
			}
			catch
			{
				throw;
			}
		}

		public static void Update_BeneficiaryMember(DataSet parameterDatasetMemberDetails)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_BeneficiaryMember(parameterDatasetMemberDetails);
			}
			catch
			{
				throw;
			}
		}

		public static void Update_MemberAddress(DataSet parameterDatasetMemberAddressDetails)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_MemberAddress(parameterDatasetMemberAddressDetails);
			}
			catch
			{
				throw;
			}
		}

		public static void Insert_MemberAddress(DataSet parameterDatasetMemberAddressDetails)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Insert_MemberAddress(parameterDatasetMemberAddressDetails);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet CreateDataSetForSave(string l_string_type)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.CreateDataSetForSave(l_string_type);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet LookUp_BS_CountryNames()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_CountryNames();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUp_BS_StateNames(string l_string_CountryCode)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_StateNames(l_string_CountryCode);
			}
			catch
			{
				throw;
			}
		}

		public static string LookUp_GetUniqueID()
		{
			try
			{
				DataSet l_DataSet_UniqueID=new DataSet();
				l_DataSet_UniqueID=YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_GetUniqueID();
				if(l_DataSet_UniqueID.Tables[0].Rows.Count>0)
				{
					return l_DataSet_UniqueID.Tables[0].Rows[0][0].ToString();
				}
				else
				{
					return "";
				}
					
			}
			catch
			{
				throw;
			}
		}

		public static string LookUp_BS_FunEventsUniqueID(string parameterDeathBeneOptionID)
		{

			try
			{
				DataSet l_DataSet_FunEventsUniqueID=new DataSet();
				l_DataSet_FunEventsUniqueID=YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_FunEventsUniqueID(parameterDeathBeneOptionID);
				if(l_DataSet_FunEventsUniqueID.Tables[0].Rows.Count>0)
				{
					return l_DataSet_FunEventsUniqueID.Tables[0].Rows[0][0].ToString();
				}
				else
				{
					return "";
				}
					
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
			try
			{
                return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_BS_BeneficiariesSSNoWithNew(parameterOldSSNo, parameterNewSSNo
					, guibeneficiaryID //SP:2013.04.12 :YRS 5.0-1990 -Added
					));
			}
			catch
			{
				throw;
			}
		}

		//NP:2008.01.03:YRPS-4046 - Adding parameter BenefitOptionId to the function
		public static string LookUp_BS_Lookp_BeneficiaryFundEvent(string parameterPerssID, string parameterBenefitOptionId)
		{
			try
			{
				DataSet l_DataSet_FundUniqueID=new DataSet();
				DataTable l_DataTable_FundUniqueID=new DataTable();
				l_DataSet_FundUniqueID=YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_Lookp_BeneficiaryFundEvent(parameterPerssID, parameterBenefitOptionId);
				if(l_DataSet_FundUniqueID!=null && l_DataSet_FundUniqueID.Tables[0]!=null)
				{
					l_DataTable_FundUniqueID=l_DataSet_FundUniqueID.Tables[0];
				}
				if( l_DataTable_FundUniqueID.Rows.Count>1)
				{
					return "NotValid";
				}
				else if(l_DataTable_FundUniqueID.Rows.Count==1)
				{
					return l_DataTable_FundUniqueID.Rows[0][0].ToString();
				}
				else
					return "";
			}
			catch
			{
				throw;
			}
		}

		//NP:IVP2:2008.12.05 - Adding additional parameter Benefit Option Id that passes either RP or SP benefit option selected for settlement
		public static string Insert_BS_FundEventData(string parameterBeneFundEventID,string parameterPersid,string parameterOrigFundEventID, string parameterBenefitOptionId)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Insert_BS_FundEventData(parameterBeneFundEventID,parameterPersid,parameterOrigFundEventID,parameterBenefitOptionId));
			}
			catch
			{
				throw;
			}
		}

		//NP:2008.01.03:YRPS-4046 - Changing Parameter from SSNo to BenefitOptionId
		public static string Update_BS_BeneficiariesFunEventandPersID(string parameterBeneFundEventID,string parameterPersid,string parameterBenefitOptionId)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_BS_BeneficiariesFunEventandPersID(parameterBeneFundEventID,parameterPersid,parameterBenefitOptionId));
			}
			catch
			{
				throw;
			}
		}

		public static DataSet LookUp_BS_MemberAddress(string l_string_PersID)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_MemberAddress( l_string_PersID));
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUp_BS_MemberTelephone(string l_string_PersID)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_MemberTelephone( l_string_PersID));
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUp_BS_MemberEmailAddress(string l_string_PersID)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_BS_MemberEmailAddress( l_string_PersID));
			}
			catch
			{
				throw;
			}
		}
		public static void Insert_MemberTelephone(DataSet parameterDatasetMemberTelephoneDetails)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Insert_MemberTelephone( parameterDatasetMemberTelephoneDetails);
			}
			catch
			{
				throw;
			}
		}
		public static void Update_MemberTelephone(DataSet parameterDatasetMemberTelephoneDetails)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_MemberTelephone(parameterDatasetMemberTelephoneDetails);
			}
			catch
			{
				throw;
			}
		}
		public static void Update_MemberEmailAddress(DataSet parameterDatasetMemberEmailAddress)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_MemberEmailAddress( parameterDatasetMemberEmailAddress);
			}
			catch
			{
				throw;
			}
		}

		public static void Insert_MemberEmailAddress(DataSet parameterDatasetMemberEmailAddress)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Insert_MemberEmailAddress( parameterDatasetMemberEmailAddress);
			}
			catch
			{
				throw;
			}
		}


		//BY Aparna -YREN-3015
		public static void Insert_AtsperssBanking(string ParticipantEntityId,string BeneficiaryEntityId)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Insert_AtsperssBanking(ParticipantEntityId,BeneficiaryEntityId);
			}
			catch
			{
				throw;
			}
		}

		//NP:PS:2007.07.20 - Adding extra parameters to the settlement process to enable simultaneous settlement of both plans
		public static int Update_FinalSettlementofBeneficiary(string paramDeathBenefitOptionID_RP, string paramAnnuityOption_RP,
			double paramRolloverTaxable_RP, double paramRolloverNonTaxable_RP, 
			string paramRolloverInstitutionID_RP,double paramWithholdingPct_RP, 
			string paramDeathBenefitOptionID_SP, string paramAnnuityOption_SP,
			double paramRolloverTaxable_SP, double paramRolloverNonTaxable_SP, 
			string paramRolloverInstitutionID_SP,double paramWithholdingPct_SP,
            //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added to pass deduction values
            string paramDedAnnuity,
            string paramDedLumpsum,
            //End - Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added to pass deduction values
            out string outpara_ErrorMessage,
            decimal decDeductionLumpsumAmount, //Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added for Deduction amt
            Boolean paramIsRolloverToOwnIRA_RP, Boolean paramIsRolloverToOwnIRA_SP )       //  SB | 2016.11.25 | YRS-AT-3022 | Added parameters for RolloverTo own IRA option
		{
			int ret_status = 1;
			try
			{
				ret_status = YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_FinalSettlementofBeneficiary(
					paramDeathBenefitOptionID_RP, 
					paramAnnuityOption_RP, 
					paramRolloverTaxable_RP, 
					paramRolloverNonTaxable_RP, 
					paramRolloverInstitutionID_RP,
					paramWithholdingPct_RP, 
					paramDeathBenefitOptionID_SP, 
					paramAnnuityOption_SP, 
					paramRolloverTaxable_SP, 
					paramRolloverNonTaxable_SP, 
					paramRolloverInstitutionID_SP,
					paramWithholdingPct_SP,
                    //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added to pass deduction values
                    paramDedAnnuity,
                    paramDedLumpsum,
                    //End - Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added to pass deduction values
                    out outpara_ErrorMessage,
                    decDeductionLumpsumAmount, //Manthan | 2016.04.22 | YRS-AT-2206 | Parameter added to pass Deduction amt
                    paramIsRolloverToOwnIRA_RP, paramIsRolloverToOwnIRA_SP);        //  SB | 2016.11.25 | YRS-AT-3022 | Added parameters for RolloverTo own IRA option
			}
			catch
			{
				throw;
			}
			return ret_status ;
		}


		public static int Update_FinalSettlementofBeneficiary(string paramDeathBenefitOptionID_RP, string paramAnnuityOption_RP,
			double paramRolloverTaxable_RP, double paramRolloverNonTaxable_RP, 
			string paramRolloverInstitutionID_RP,double paramWithholdingPct_RP, 
			out string outpara_ErrorMessage )
		{
			try
			{
				int ret_status ;
				ret_status = 1 ;
				YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.Update_FinalSettlementofBeneficiary(
					paramDeathBenefitOptionID_RP, 
					paramAnnuityOption_RP, 
					paramRolloverTaxable_RP, 
					paramRolloverNonTaxable_RP, 
					paramRolloverInstitutionID_RP,
					paramWithholdingPct_RP, 
					out outpara_ErrorMessage 
					) ;
				return ret_status ;
			}
			catch
			{
				throw;
			}
		}


	
		//NP:YRPS-4009:2008.01.23 - Function to perform validations on Death Settlement Option
		// Currently this checks if there are any unfunded transactions for the Deceased participant for non-retired funds
		// Return Values:	-2 = Both Parameters were not specified
		//					-1 = Some Error happened
		//					1 = Successful execution of the routine
		public static int PerformPreRequisiteValidations(string paramDeathBenefitOptionID_RP, string paramDeathBenefitOptionID_SP, out string outpara_ErrorMessage) 
		{
			int int_returnStatus = 1 ;
			outpara_ErrorMessage = "";	
			string errorMessage = string.Empty;
			
			//Check if either Retirement Plan or Savings plan option is provided. Both cannot be null.
			if ((paramDeathBenefitOptionID_RP == null || paramDeathBenefitOptionID_RP == string.Empty )
				&& (paramDeathBenefitOptionID_SP == null || paramDeathBenefitOptionID_SP == string.Empty )) 
			{
				return -2;
			}
			try 
			{ 
				if (paramDeathBenefitOptionID_RP != null && paramDeathBenefitOptionID_RP != string.Empty) 
				{
					int_returnStatus = YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.PerformPreRequisiteValidations(paramDeathBenefitOptionID_RP, out errorMessage);
					//Check for errors and return if required
					if (int_returnStatus != 1) 
					{
						outpara_ErrorMessage = errorMessage;
						return -1;
					}
				}
				if (paramDeathBenefitOptionID_SP != null && paramDeathBenefitOptionID_SP != string.Empty) 
				{
					int_returnStatus = YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.PerformPreRequisiteValidations(paramDeathBenefitOptionID_SP, out errorMessage);
					//Check for errors and return if required
					if (int_returnStatus != 1) 
					{
						outpara_ErrorMessage = errorMessage;
						return -1;
					}
				}

				return int_returnStatus;
			}
			catch
			{
				throw;
			}

		}
        //created by -  BS:2011.08.10:YRS 5.0-1339:BT:852 - Reopen issue 
        public static DataSet LookUp_NonHumanBenfInfo(string paramBenefitOptionID)
        {

            try
            {
                return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_NonHumanBenfInfo(paramBenefitOptionID));	

            }
            catch
            {
                throw;
            }

        }

        //SP 2014.04.01 BT-2420\YRS 5.0-2309 : Beneficiary Settlement - Retired Death -Start

        public static bool CheckAnnuityReversedAfterDeathBeneficiaryOptionCreated(string paramParticipantGuiPersID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.CheckAnnuityReversedAfterDeathBeneficiaryOptionCreated(paramParticipantGuiPersID));

            }
            catch
            {
                throw;
            }

        }
        //SP 2014.04.01 BT-2420\YRS 5.0-2309 : Beneficiary Settlement - Retired Death -End

        //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Get Deductions values calling DA class
        public static DataTable GetDeductions()
        {
            DataSet dsDeductions;
            try
            {
                dsDeductions = YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.GetDeductions();

                if (dsDeductions != null && dsDeductions.Tables.Count > 0)
                {
                    return dsDeductions.Tables[0];
                }
                else
                    return null;
            }
            catch
            {
                throw;
            }
        }
        //End - Manthan | 2016.04.22 | YRS-AT-2206 | Get Deductions values calling DA class

       //Start - Manthan | 2016.04.22 | YRS-AT-2206 | Getting No of payroll months based on Annutiy purchase date
        public static int GetPastPayrollCount(DateTime dtmAnnuityPurchaseDate)
        {
            int intPayrollMonthsCount;
            try
            {
                intPayrollMonthsCount = YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.GetPastPayrollCount(dtmAnnuityPurchaseDate);
            }
            catch
            {
                throw;
            }
            return intPayrollMonthsCount;
        }
        //End - Manthan | 2016.04.22 | YRS-AT-2206 | Getting No of payroll months based on Annutiy purchase date
	}  
   
}