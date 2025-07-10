//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	FindInfoDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	8/3/2005 10:04:52 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	DataAcess class for FindInfo form
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date				Description
//*******************************************************************************
//Neeraj Singh              06/jun/2010         Enhancement for .net 4.0
//Neeraj Singh              07/jun/2010         review changes done
//Deven                     02-Sep-2010         Added funcion GetPersonDetail to get the Person detail by FundEventID of by its Gui ID
//Sanket Vaidya     :       15-Sep-2011         BT-798 System should not allow disability retirement for QD and BF fundevents
//Dinesh Kanojia            2013-03-19          Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
//Anudeep Adusumilli        2014-02-03          BT:2292:YRS 5.0-2248 - YRS Pin number 
//Manthan Rajguru           2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Sanjay GS Rawat           2016.06.26          YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
//***************************************************************************************************************************

using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for FindInfoDAClass.
	/// </summary>
	public sealed class FindInfoDAClass
	{
		private FindInfoDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Method to get the list of participants based on the search criteria
		/// </summary>
		/// <param name="parameterSSNo"></param>
		/// <param name="parameterFundNo"></param>
		/// <param name="parameterLastName"></param>
		/// <param name="parameterFirstName"></param>
		/// <returns>dataset</returns>
        // Dinesh Kanojia : 2013-03-19 : Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
		public static DataSet LookUpPerson(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterFormName,string parameterCity, string parameterState,string paramEmail,string paramPhone)
        //public static DataSet LookUpPerson(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName, string parameterFormName, string parameterCity, string parameterState)
		{
			DataSet dsLookUpPersons = null;
			Database db= null;
			DbCommand commandLookUpPersons = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				
				commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_FindPerson");
				
				if (commandLookUpPersons==null) return null;

				// Altered By	:	SrimuruganG
				// Description	:	To add the Connection Timeout
				// Date			:	24.Oct.2005
				commandLookUpPersons.CommandTimeout = System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["ExtraLargeConnectionTimeOut"] );


				dsLookUpPersons = new DataSet();
				db.AddInParameter(commandLookUpPersons, "@varchar_SSN",DbType.String,parameterSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FormName", DbType.String, parameterFormName);
                db.AddInParameter(commandLookUpPersons, "@varchar_City", DbType.String, parameterCity);
                db.AddInParameter(commandLookUpPersons, "@varchar_State", DbType.String, parameterState);
                //Dinesh Kanojia : 2013-03-19 : Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                db.AddInParameter(commandLookUpPersons, "@varchar_PhoneNumber", DbType.String, paramPhone);
                db.AddInParameter(commandLookUpPersons, "@varchar_Email", DbType.String, paramEmail);
				db.LoadDataSet(commandLookUpPersons,dsLookUpPersons,"Persons");
				return dsLookUpPersons;
			}
			catch 
			{
				throw ;
			}

		}     

        //Sanket Vaidya :   BT-798 System should not allow disability retirement for QD and BF fundevents
        public static DataSet LookUpPerson(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName, string parameterFormName, string parameterCity, string parameterState, string parameterRetirementType)
        {
            DataSet dsLookUpPersons = null;
            Database db = null;
            DbCommand commandLookUpPersons = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_FindPerson");

                if (commandLookUpPersons == null) return null;

                // Altered By	:	SrimuruganG
                // Description	:	To add the Connection Timeout
                // Date			:	24.Oct.2005
                commandLookUpPersons.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);


                dsLookUpPersons = new DataSet();
                db.AddInParameter(commandLookUpPersons, "@varchar_SSN", DbType.String, parameterSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FormName", DbType.String, parameterFormName);
                db.AddInParameter(commandLookUpPersons, "@varchar_City", DbType.String, parameterCity);
                db.AddInParameter(commandLookUpPersons, "@varchar_State", DbType.String, parameterState);
                db.AddInParameter(commandLookUpPersons, "@varchar_RetirementType", DbType.String, parameterRetirementType);
                
                db.LoadDataSet(commandLookUpPersons, dsLookUpPersons, "Persons");
                return dsLookUpPersons;
            }
            catch
            {
                throw;
            }

        }

		public static DataTable GetQDRODetails_BYSSNO (string p_string_SSNO)
		{
			Database	l_DataBase = null;
			DataSet		l_DataSet  = null;
			string []	l_stringDataTableNames;

			DbCommand l_DBCommandWrapper = null;			
		
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase == null) return null;

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_GetQDRODetails_BYSSNO");

				if (l_DBCommandWrapper == null) return null;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@varchar_SSNO", DbType.String, p_string_SSNO);
				
				// Add the DataSet
				l_DataSet = new DataSet ("QDRORefunds");

				l_stringDataTableNames = new string [] {"QDRO"};

				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_stringDataTableNames);

				if (l_DataSet != null)
				{
					return (l_DataSet.Tables ["QDRO"]);
				}

				return null;					
			}
			catch
			{
				throw;
			}

		}
        // Deven 02-Sep-2010
        /// <summary>
        /// This method when invoked will give us the persons detail based on fund event id
        /// </summary>
        /// <param name="parameterFundEventID"></param>
        /// <returns></returns>
        public static DataTable GetPersonDetail(string parameterguiFundEventID, string parameterguiPersID)
        {
            DataSet dsPersonDetail = null;
            Database db = null;
            DbCommand commandPersonDetail = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandPersonDetail = db.GetStoredProcCommand("yrs_usp_FindPersonOrYMCADetail");

                if (commandPersonDetail == null) return null;

                commandPersonDetail.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);


                dsPersonDetail = new DataSet();
                db.AddInParameter(commandPersonDetail, "@varchar_SearchForm", DbType.String, "Person");
                db.AddInParameter(commandPersonDetail, "@varchar_guiFundEventID", DbType.String, parameterguiFundEventID);
                db.AddInParameter(commandPersonDetail, "@varchar_guiPersId", DbType.String, parameterguiPersID);
                db.LoadDataSet(commandPersonDetail, dsPersonDetail, "PersonsDetail");
                if (dsPersonDetail != null)
                    return dsPersonDetail.Tables[0];
                else
                    return null;
            }
            catch
            {
                return null;
            }

        }
        //Start:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To get the PIN value
        public static string GetUserPIN(string strGuiPersId)
        {
            Database db = null;
            string strPIN;
            DbCommand GetCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                GetCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetUserPIN");

                if (GetCommandWrapper == null) return "0";

                db.AddInParameter(GetCommandWrapper, "@guiPersId", DbType.String, strGuiPersId);
                db.AddOutParameter(GetCommandWrapper, "@chrPIN", DbType.String, 20);
                db.ExecuteScalar(GetCommandWrapper);

                strPIN = System.Convert.ToString(db.GetParameterValue(GetCommandWrapper, "@chrPIN"));

                return strPIN;


            }
            catch
            {
                throw;
            }

        }
        //End:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To get the PIN value

        //START | 2016.06.26 | Sanjay GS Rawat  | YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
        public static DataSet GetFundStatusChangedParticipant()
        {
            DataSet dsFundStatusChanged = null;
            Database db = null;
            DbCommand cmdFundStatusChanged = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                cmdFundStatusChanged = db.GetStoredProcCommand("yrs_usp_RMD_FundStatusChanged");
                if (cmdFundStatusChanged == null) return null;
                cmdFundStatusChanged.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
                dsFundStatusChanged = new DataSet();
                db.LoadDataSet(cmdFundStatusChanged, dsFundStatusChanged, "FundStatusChanged");
                return dsFundStatusChanged;
            }
            catch
            {
                throw;
            }

        }
        //END | 2016.06.26 | Sanjay GS Rawat | YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 

	}
}
