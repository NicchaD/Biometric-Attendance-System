//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
#region using Namespace

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject; 
 
#endregion

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for InterestProcessingBOClass.
	/// </summary>
	public class InterestProcessingBOClass
	{
		public DateTime DateTime_AccountingDate = System.Convert.ToDateTime("01/01/1900");  
		public DateTime DateTime_EndingDate = System.Convert.ToDateTime("01/01/1900");  
		public DateTime DateTime_StartingDate = System.Convert.ToDateTime("01/01/1900");  

		public DateTime DateTime_MonthEndDate = System.Convert.ToDateTime("01/01/1900");
		public string l_string_Message = string.Empty;
		private DateTime DateTime_InterestProcessingDate;
		private const string NewLineCharacter="\r\n";

		public InterestProcessingBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Public Methods
		public void GetAccountDate()
		{
			DataSet l_dataset = null;

			try
			{

				l_dataset = YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.GetAcctDate(); 

				if (l_dataset != null)
				{
					this.DateTime_AccountingDate = Convert.ToDateTime(l_dataset.Tables[0].Rows[0]["dtmEndDate"]);  
					this.DateTime_EndingDate = Convert.ToDateTime(l_dataset.Tables[0].Rows[0]["dtmEndDate"]);
					this.DateTime_StartingDate = Convert.ToDateTime(l_dataset.Tables[0].Rows[0]["dtmStartDate"]);
					this.DateTime_MonthEndDate = Convert.ToDateTime(l_dataset.Tables[0].Rows[0]["dtmEndDate"]);
 
				}

				
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetAccntDate()
		{
			DataSet l_dataset = null;
				
			try
			{

				l_dataset = YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.GetAcctDate(); 

				if (l_dataset != null)
				{
					return l_dataset;
 
				}

				return null;
				
			}
			catch
			{
				throw;
			}
		}

		public bool UpdateAccountingDate(DateTime p_dateTime)
		{
			
			try
			{

				return (YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.UpdateAcctDate(DateTime_EndingDate,DateTime_StartingDate,p_dateTime)); 

				
			}
			catch
			{
				throw;
			}
		}

		public bool InterestProcess(string parameterProcessOption)
		{
			
				

			try
			{
				if (parameterProcessOption.Equals("MonthEndInterest"))
				{
					MonthlyInterest(); 
					//String_ProcessMessage="MonthEnd Interest Processing complete successfully!";
				
				}
				else if(parameterProcessOption.Equals("DailyInterest")) 
				{
					//call Daily Interest Logic
					DailyInterest();
					//String_ProcessMessage="DailyEnd Interest Processing complete successfully!";
				}
	
					
				return true;
			}
			catch
			{
				return false;

			}
		}
		#endregion

		#region Public Properties
		public string String_ProcessMessage
		{   
			get
			{
				return l_string_Message;
			}
			set
			{
				l_string_Message = value;
			}
		}

		public DateTime InterestProcessingDate
		{
			get 
			{
				return DateTime_InterestProcessingDate;
			}
			set 
			{
				DateTime_InterestProcessingDate=value;
			}
		}
		
		public static DataSet getMetaOutputFileType(string p_string_search)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.getMetaOutputFileType(p_string_search)); 
			}
			catch
			{
				throw;
			}

		}

		#endregion

		#region Private Methods
		private bool MonthlyInterest()
		{
			DateTime dTemp, dAcctDate =  Convert.ToDateTime("01/01/1800");

			DateTime ldEndDate ,ldBegDate;
			int lnMonth;
			DataSet l_DataSet_ServicePostLogMonthly;
			string lcProgramName = "";
			string lcProcessName = "";
			l_string_Message="";
			
			try
			{
				
						/* Computes: 1. Regular Monthy Interest (ININ) by FundEvent, Account Type and Annuity Basis
							
									 2. Unearned Interest (UEIN) Interest on Payments that have yet to be Funded

								
						*/

				dAcctDate = this.DateTime_AccountingDate; 

				dAcctDate = dAcctDate.AddMonths(-1);

				dTemp = dAcctDate;

				while(dAcctDate.Month == dTemp.Month)
				{
					dAcctDate = dAcctDate.AddDays(1);
				}

				dAcctDate = dAcctDate.AddDays(-1);

				lcProgramName = "INTERESTPOST";
				lcProcessName = "REGULAR";

				l_DataSet_ServicePostLogMonthly = YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.GetServicePostLogMonthly(lcProgramName,lcProcessName,dAcctDate);
				if (l_DataSet_ServicePostLogMonthly != null)
				{
					if (l_DataSet_ServicePostLogMonthly.Tables[0].Rows.Count == 0 )
					{

						ldEndDate  = this.InterestProcessingDate;
						ldBegDate = dAcctDate.AddMonths(-1);
						lnMonth = ldBegDate.Month;
						while (ldBegDate.Month == lnMonth)
						{
							ldBegDate = ldBegDate.AddDays(1); 

						}
						//ldBegDate = ldBegDate.AddDays(-1); 
						//Call Procdure to Update.
						
						
							YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.ProcessRegularIntrest(ldBegDate,ldEndDate,"Monthly");

						

						
						
					}
					else
					{
						l_string_Message = "Interest Processing: Regular Interest for " + dAcctDate.ToString("MM/dd/yyyy") + " has already been Run.";
						
					}

				}

				lcProgramName = "INTERESTPOST";
				lcProcessName = "UNEARNED";

				l_DataSet_ServicePostLogMonthly = YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.GetServicePostLogMonthly(lcProgramName,lcProcessName,dAcctDate);

				if (l_DataSet_ServicePostLogMonthly != null)
				{
					if (l_DataSet_ServicePostLogMonthly.Tables[0].Rows.Count == 0 )
					{
						//if(true)
						if (YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.ProcessUnearnedMonthlyIntrest())
						{
							l_string_Message += "Interest Processing: Process Completed.";
							//return true;
						}
						else
						{
							l_string_Message += "Interest Processing: Process not completed. Please Contact Tech. Support.";
							//return false;
						}


					}
					else
					{
						l_string_Message +=NewLineCharacter+ "Interest Processing: Unearned Interest for " + dAcctDate.ToString("MM/dd/yyyy") + " has already been Run." ;
					}

				}
				//Added by Ashish 16-Dec-2008

				lcProgramName = "VESTING";
				lcProcessName = "MONTHLY";

				l_DataSet_ServicePostLogMonthly = YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.GetServicePostLogMonthly(lcProgramName,lcProcessName,dAcctDate);

				if (l_DataSet_ServicePostLogMonthly != null)
				{
					if (l_DataSet_ServicePostLogMonthly.Tables[0].Rows.Count == 0 )
					{
						DataSet l_DataSet_VestingByAgeFlag=YMCACommonDAClass.getConfigurationValue("UPDATE_MONTHLY_SERVICE_ON_AGE");
						if(l_DataSet_VestingByAgeFlag !=null)
						{
							if(l_DataSet_VestingByAgeFlag.Tables.Count>0)
							{
								if(l_DataSet_VestingByAgeFlag.Tables[0].Rows.Count>0)  
								{
									if(l_DataSet_VestingByAgeFlag.Tables[0].Rows[0]["Value"].ToString()=="1")
									{
										//if(true)
										if (YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.ProcessVestingDueToAge(this.InterestProcessingDate))
										{
											l_string_Message += NewLineCharacter+ "Vesting: Vesting Due to Age Process Completed.";
											//return true;
										}
										else
										{
											l_string_Message += NewLineCharacter+"Vesting: Vesting Due to Age Process not completed. Please Contact Tech. Support.";
											//return false;
										}
									}
								}
								else
								{
									throw new Exception( " Key \"UPDATE_MONTHLY_SERVICE_ON_AGE\" not defined in AtsMetaConfiguration.");
								}
							}
						}


					}
					else
					{
						l_string_Message +=NewLineCharacter+"Vesting: Vesting Due to Age process for " + dAcctDate.ToString("MM/dd/yyyy") + " has already been Run." ;
					}

				}

			return true;

			}
			catch(Exception ex)
			{
				if (lcProcessName=="REGULAR")
				{
					l_string_Message += "Interest Processing: Regular Interest Failed. Error:" + ex.Message.ToString() ;
				}
				else if(lcProcessName=="UNEARNED") 
				{
					l_string_Message +=NewLineCharacter+ "Interest Processing: UnEarned Interest Failed. Error: " + ex.Message.ToString() ;
				}if(lcProcessName=="MONTHLY") 
				{
					l_string_Message += NewLineCharacter+"Vesting: Vesting Due to Age Failed. Error: " + ex.Message.ToString() ;
				}
				return false;

			}
		}

		private bool DailyInterest()
		{
			
			try
			{
				DateTime ldEndDate ,ldBegDate;
				l_string_Message="";
				DataSet l_DataSet_DailyIntPostLog;
				ldEndDate=this.InterestProcessingDate;
				ldBegDate=this.DateTime_StartingDate ;
				l_DataSet_DailyIntPostLog = YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.GetDailyIntPostLog(this.InterestProcessingDate);
				if(l_DataSet_DailyIntPostLog!=null)
				{
					if(l_DataSet_DailyIntPostLog.Tables[0].Rows.Count==0)
					{
						//if(true)
						if (YMCARET.YmcaDataAccessObject.InterestProcessingDAClass.ProcessRegularIntrest(ldBegDate,ldEndDate,"Daily"))
						{
							l_string_Message += "Interest Processing: Process Completed.";
							return true;
						}
						else
						{
							l_string_Message += "Interest Processing: Process not completed. Contact Tech. Support.";
							return false;
						}
					}
					else
					{
						l_string_Message +="Interest Processing: Daily Interest for " + this.InterestProcessingDate.ToString("MM/dd/yyyy") + " has already been Run." ;
					}
				}
				
			return true;
			}
			catch(Exception ex)
			{
				l_string_Message += "Interest Processing: Regular Interest Failed. Error:" + ex.Message.ToString() ;
				return false ;
			}
		}
		#endregion
	}
}
