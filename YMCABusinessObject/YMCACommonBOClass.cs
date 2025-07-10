/********************************************************************************************************************************
 Modification History
'********************************************************************************************************************************
'Modified By			Date					Description
'********************************************************************************************************************************
'Ashutosh Patil			06-Jun-2007				YREN-3490 Common function for validation of Phony SSNo
'Nikunj Patel			20-Aug-2007				Changed validation for normal SSN to be 9 digits
'Aparna Samala			12-Sep-2007				To get the Deatails from atsmetaconfiguration for DethNotification
'Manthan Rajguru        2015.09.16              YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Chandra sekar          2016.07.25              YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
'Manthan Rajguru        2016.07.29              YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field. 
'Chandra sekar          2016.10.24              YRS-AT-3088 - YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
'Pramod Prakash Pokale  2016.11.16              YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee 
'********************************************************************************************************************************/

using System;
using System.Data;
using System.Web;
using System.Text.RegularExpressions;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for YMCACommonBOClass.
	/// </summary>
	public class YMCACommonBOClass
	{
		public YMCACommonBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet MetaOutputChkFileType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCACommonDAClass.MetaOutputChkFileType();

			}
			catch
			{
				throw;
			}
		}

		public static DataSet MetaOutputFileType(string parameterOutputFiletype)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCACommonDAClass.MetaOutputFileType(parameterOutputFiletype);

			}
			catch
			{
				throw;
			}
		}

        //START : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
        public static DataSet GetNextBatchId(DateTime processDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.YMCACommonDAClass.GetNextBatchId(processDate));
            }
            catch
            {
                throw;
            }
        }
        //END : CS | 2016.10.24 | YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)

		/* 
		  Created By Ashutosh Patil as on 06-Jun-20007
		  Purpose - Common function will be called for validation of Phony SSNo
		  YREN-3490
		*/
		public static string IsValidSSNo(string parameterSSNo)
		{
			string l_string_Message=null;
			string l_string_MetaConfigSSNoValue=null;
			string l_string_ConfigKey="SSNO_CHARACTER";
			DataSet l_Dataset_ConfigPhonySSNo=null;
			
				
			try
			{	
				if (Regex.IsMatch(parameterSSNo,"^\\d{9}$").Equals(false)) //NP:PS:2007.08.20 - Changing the validation criteria from ^\\d+$               
                {
					l_Dataset_ConfigPhonySSNo=YmcaDataAccessObject.MetaConfigMaintenanceDAClass.SearchConfigurationMaintenance(l_string_ConfigKey);  
					
					if (l_Dataset_ConfigPhonySSNo !=null)
					{
						if (l_Dataset_ConfigPhonySSNo.Tables["Configuration Maintenance"].Rows.Count > 0)
						{
							if (l_Dataset_ConfigPhonySSNo.Tables["Configuration Maintenance"].Rows[0]["Value"].ToString()!="System.DBNull" || l_Dataset_ConfigPhonySSNo.Tables["Configuration Maintenance"].Rows[0]["Value"].ToString()!="")
							{
							  l_string_MetaConfigSSNoValue=l_Dataset_ConfigPhonySSNo.Tables["Configuration Maintenance"].Rows[0]["Value"].ToString();
							  l_string_Message=ValidatePhonySSNo(l_string_MetaConfigSSNoValue,parameterSSNo);
							}
							else
							{ 
							  l_string_Message="No_Configuration_Key"; 
							}
						}
						else
						{
						 l_string_Message="No_Configuration_Key"; 
						}
					}
				}
				else
				{
					l_string_Message=""; 
				}
				return l_string_Message;	  
			}
          	catch
			{
			 throw;
			}
		}
		/* 
		  Created By Ashutosh Patil as on 06-Jun-20007
		  Purpose - This function will validate PhonySSNo and set corresponding messages related to it.
		  YREN-3490
		*/
		private static string ValidatePhonySSNo(string paramstrMetaconfigKey,string parameterSSNo)
		{
			string l_str_SSNoConfigValue = null;
			string l_str_remainingSSNo = null;
			string l_str_Message = null;
			string l_str_MeaConfigValue= null;
			char[] l_arr_ConfigValue=paramstrMetaconfigKey.ToCharArray();
			char[] l_arr_SSNoConfigValue=parameterSSNo.ToCharArray();
			Int32  l_int_MetaConfigKey=0;
			Int32  l_int_MetaConfigLength=0;
			bool l_bool_TextSSNoMatchesConfigValue = true;

		
            l_str_MeaConfigValue=paramstrMetaconfigKey;
            l_str_SSNoConfigValue=parameterSSNo.ToString().Trim();
			l_int_MetaConfigLength=paramstrMetaconfigKey.Length;
	
			
			
			try
			{
				for(l_int_MetaConfigKey=0;l_int_MetaConfigKey<l_int_MetaConfigLength;l_int_MetaConfigKey++)
				{
					if (l_arr_ConfigValue.GetValue(l_int_MetaConfigKey).Equals(l_arr_SSNoConfigValue.GetValue(l_int_MetaConfigKey)))
					{
						if (paramstrMetaconfigKey.Length==1)
						{
							break;
						} 	
					}
					else
					{
						 l_bool_TextSSNoMatchesConfigValue = false;
						 break;
					}
					
				}

				if (l_bool_TextSSNoMatchesConfigValue.Equals(false))
				{
					l_str_Message = "Not_Phony_SSNo";
				}
				else
				{
				    l_str_remainingSSNo=parameterSSNo.Substring(paramstrMetaconfigKey.Length,parameterSSNo.Length-paramstrMetaconfigKey.Length);   
					//if (Regex.IsMatch(l_str_remainingSSNo,"^\\d+$").Equals(false))
                    if (Regex.IsMatch(l_str_remainingSSNo, "^\\d{8}$").Equals(false)) 
					{
					 l_str_Message="Not_Phony_SSNo";			
					}
					else
					{
					 l_str_Message="Phony_SSNo";			
					}
				} 
			   	return l_str_Message;	
			}
		
			catch
			{
			throw;
			}

		}

		//by Aparna 12/09/2007
		public  static DataSet GetDeathNotificationConfig(string ParameterConfigKey)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCACommonDAClass.GetDeathNotificationConfig(ParameterConfigKey);

			}
			catch
			{
				throw;
			}
		}

		public  static DataSet getConfigurationValue(string ParameterConfigKey)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCACommonDAClass.getConfigurationValue(ParameterConfigKey);

			}
			catch
			{
				throw;
			}
		}
		//Added by Ashish 17-Mar-2009,  Start
		public  static string GetParticipantValidEmailAddress(string ParameterPersID)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCACommonDAClass.GetParticipantValidEmailAddress(ParameterPersID);

			}
			catch
			{
				throw;
			}
		}
		//Added by Ashish 17-Mar-2009,  End
        //START- Chandra sekar - 2016.07.25 - YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
        public static bool IsProductionEnvironment()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.YMCACommonDAClass.IsProductionEnvironment();

            }
            catch
            {
                throw;
            }
        }
        //END- Chandra sekar - 2016.07.25 - YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).

        //START: PPP | 11/16/2016 | YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee 
        public static void ChargeFee(string requestID, string module)
        {
            YMCARET.YmcaDataAccessObject.YMCACommonDAClass.ChargeFee(requestID, module);
        }
        //END: PPP | 11/16/2016 | YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee 
    }
}
