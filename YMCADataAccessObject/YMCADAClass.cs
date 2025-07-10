//********************************************************************************************************************************
//                           CHANGE HISTORY
//********************************************************************************************************************************
// Modified              Date                Description
//********************************************************************************************************************************
// NP/PP/SR             2009.05.18          Optimizing the YMCA Screen
// Swopna               26-May-2008         Added function which checks if all transmittals have been received between last received transmittal and proposed withdrawal date
// Paramesh K.			31-July-2008		Added method GetParticipantsHavingLoans() for sending email alert after merging YCMA
// Dilip Yadav          28-Oct-2009         YRS 5.0.921 : To provide the Priority handling 
// Priya				17-March-2010		YRS 5.0-1017:Display withdrawal date as merged date for branches
// Priya                14-June-2010        BT-536: While saving contacts information application shows Error page. added paarameter @varchar_ContactType
// Deven                02-Sep-2010         Added funcion GetYMCANoByGuiID to get the YMCA No by its Gui ID
// Neeraj               25-Oct-2010         issue id BT-667: added new parameter guiAtsYmcaEntrantsID		
// Priya				24-01-2011			BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
// Sanjay S.			2011.04.15			BT-747: Updating code to handle issue where the field going into the database was defined as int16 while it should have been int32.
//Shashi				2011.06.14  		BT-844 YRS 5.0-1334 :Add field for Y-Relations Manager
//BhavnaS               2011.08.05          BT:921- bug occur:'String[0] property size is invalid' while add/updating more than resolution,to resolve this bug:put size of parameter is 1000
//Prasad                2011.10.03          BT-909- for YRS 5.0-1379 : New job position field in atsYmcaContacts
//Prasad                2011.10.14          BT-909- for YRS 5.0-1379 : New job position field in atsYmcaContacts(reopen)
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Bala                  2016.01.19          YRS-AT-2398: Customer Service requires a Special Handling alert for officers.
//Manthan Rajguru       2016.02.03          YRS-AT-2334: Enhancement to YRS YMCA Maintenance-add a suspend participation option
//Chandra sekar         2016.07.12          YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
//********************************************************************************************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for YMCADAClass.
	/// </summary>
	public sealed class YMCADAClass
	{
		private YMCADAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet SearchYMCAList(string parameterSearchYMCANo,string parameterSearchYMCAName,string parameterSearchYMCACity,string parameterSearchYMCAState)
		{
			DataSet dsSearchYMCAList = null;
			Database db = null;
			DbCommand CommandSearchYMCAList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCAList = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAS");
				if (CommandSearchYMCAList ==null) return null;
				
				db.AddInParameter(CommandSearchYMCAList,"@char_YmcaNo",DbType.String,parameterSearchYMCANo);
                db.AddInParameter(CommandSearchYMCAList, "@varchar_YmcaName", DbType.String, parameterSearchYMCAName);
                db.AddInParameter(CommandSearchYMCAList, "@varchar_City", DbType.String, parameterSearchYMCACity);
                db.AddInParameter(CommandSearchYMCAList, "@char_StateType", DbType.String, parameterSearchYMCAState);

                CommandSearchYMCAList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				
				dsSearchYMCAList = new DataSet();
				dsSearchYMCAList.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCAList,dsSearchYMCAList,"YMCA List");
			
				return dsSearchYMCAList;
			}
			catch
			{
				throw;
			}

		}

		// Calling a stored proc which will populate the General tab controls corresponding the row selection in the LIST Tab (guiUniqueId of atsymcas)
		public static DataSet SearchYMCAGeneral(string parameterSearchYMCAGeneralGuiUniqueId)
		{
			DataSet dsSearchYMCAGerneral = null;
			Database db = null;
			DbCommand CommandSearchYMCAGeneral = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCAGeneral = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAGeneral");
				if (CommandSearchYMCAGeneral ==null) return null;

				db.AddInParameter(CommandSearchYMCAGeneral,"@varcharGuiUniqueId",DbType.String,parameterSearchYMCAGeneralGuiUniqueId);
                CommandSearchYMCAGeneral.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
								
				dsSearchYMCAGerneral = new DataSet();
				dsSearchYMCAGerneral.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCAGeneral,dsSearchYMCAGerneral,"YMCA General");

				return dsSearchYMCAGerneral;
			}
			catch
			{
				throw;
			}

		}

		// Calling a stored proc which will populate the Officer tab controls corresponding the row selection in the LIST Tab (guiUniqueId of atsymcas)
		public static DataSet SearchYMCAOfficer(string parameterSearchYMCAOfficerGuiUniqueId)
		{
			DataSet dsSearchYMCAOfficer = null;
			Database db = null;
			DbCommand CommandSearchYMCAOfficer = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCAOfficer = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAOfficer");
				if (CommandSearchYMCAOfficer ==null) return null;

				db.AddInParameter(CommandSearchYMCAOfficer,"@varcharGuiUniqueId",DbType.String,parameterSearchYMCAOfficerGuiUniqueId);
				
				dsSearchYMCAOfficer = new DataSet();
				dsSearchYMCAOfficer.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCAOfficer,dsSearchYMCAOfficer,"YMCA Officer");

				return dsSearchYMCAOfficer;
			}
			catch
			{
				throw;
			}

		}


		// Calling a stored proc which will populate payrolldates : Dilip : 08-July-2009

		public static DataSet SearchYMCAPayrolldate(string parameterSearchYMCAPayrolldateGuiUniqueId)
		{
			DataSet dsSearchYMCAPayrolldate = null;
			Database db = null;
			DbCommand CommandSearchYMCAPayrolldate = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCAPayrolldate = db.GetStoredProcCommand("yrs_usp_YMCA_SearchPayrolldates");
				if (CommandSearchYMCAPayrolldate ==null) return null;

				db.AddInParameter(CommandSearchYMCAPayrolldate,"@guiYMCAId",DbType.String,parameterSearchYMCAPayrolldateGuiUniqueId);
                
				dsSearchYMCAPayrolldate = new DataSet();
				dsSearchYMCAPayrolldate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCAPayrolldate,dsSearchYMCAPayrolldate,"YMCAPayrolldate");
				return dsSearchYMCAPayrolldate;
			}
			catch
			{
					throw;
			}
		

		}


		// Calling a stored proc which will populate the Contact tab controls corresponding the row selection in the LIST Tab (guiUniqueId of atsymcas)
		public static DataSet SearchYMCAContact(string parameterSearchYMCAContactGuiUniqueId)
		{
			DataSet dsSearchYMCAContact = null;
			Database db = null;
			DbCommand CommandSearchYMCAContact = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCAContact = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAContacts");
				if (CommandSearchYMCAContact ==null) return null;

				db.AddInParameter(CommandSearchYMCAContact,"@varcharGuiUniqueId",DbType.String,parameterSearchYMCAContactGuiUniqueId);
				
				dsSearchYMCAContact = new DataSet();
				dsSearchYMCAContact.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCAContact,dsSearchYMCAContact,"YMCA Contact");
				return dsSearchYMCAContact;
			}
			catch
			{
				throw;
			}

		}

		// Calling a stored proc which will populate the Resolution tab controls corresponding the row selection in the LIST Tab (guiUniqueId of atsymcas)
		public static DataSet SearchYMCAResolution(string parameterSearchYMCAResolutionGuiUniqueId)
		{
			DataSet dsSearchYMCAResolution = null;
			Database db = null;
			DbCommand CommandSearchYMCAResolution = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCAResolution = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAResolution");
				if (CommandSearchYMCAResolution ==null) return null;
                
				db.AddInParameter(CommandSearchYMCAResolution,"@varcharGuiUniqueId",DbType.String,parameterSearchYMCAResolutionGuiUniqueId);
				
				dsSearchYMCAResolution = new DataSet();
				dsSearchYMCAResolution.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCAResolution,dsSearchYMCAResolution,"YMCA Resolution");

				return dsSearchYMCAResolution;
			}
			catch
			{
				throw;
			}

		}


		// Calling a stored proc which will populate the Branch tab controls corresponding the row selection in the LIST Tab (guiUniqueId of atsymcas)
		public static DataSet SearchYMCABranch(string parameterSearchYMCABranchGuiUniqueId)
		{
			DataSet dsSearchYMCABranch = null;
			Database db = null;
			DbCommand CommandSearchYMCABranch = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCABranch = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCABranch");
				if (CommandSearchYMCABranch ==null) return null;

				db.AddInParameter(CommandSearchYMCABranch,"@varcharGuiUniqueId",DbType.String,parameterSearchYMCABranchGuiUniqueId);
				
				dsSearchYMCABranch = new DataSet();
				dsSearchYMCABranch.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCABranch,dsSearchYMCABranch,"YMCA Branch");

				return dsSearchYMCABranch;
			}
			catch
			{
				throw;
			}

		}

		// Calling a stored proc which will populate the Branch tab controls corresponding the row selection in the LIST Tab (guiUniqueId of atsymcas)
		public static DataSet SearchYMCABankInfo(string parameterSearchYMCABankInfoGuiUniqueId)
		{
			DataSet dsSearchYMCABankInfo = null;
			Database db = null;
			DbCommand CommandSearchYMCABankInfo = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCABankInfo = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCABankInfo");
				if (CommandSearchYMCABankInfo ==null) return null;

				db.AddInParameter(CommandSearchYMCABankInfo,"@varcharGuiUniqueId",DbType.String,parameterSearchYMCABankInfoGuiUniqueId);
				
				dsSearchYMCABankInfo = new DataSet();
				dsSearchYMCABankInfo.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCABankInfo,dsSearchYMCABankInfo,"YMCA BankInfo");

				return dsSearchYMCABankInfo;
			}
			catch
			{
				throw;
			}

		}

		// Calling a stored proc which will populate the Notes tab controls corresponding the row selection in the LIST Tab (guiUniqueId of atsymcas)
		public static DataSet SearchYMCANotes(string parameterSearchYMCANotesGuiUniqueId)
		{
			DataSet dsSearchYMCANotes = null;
			Database db = null;
			DbCommand CommandSearchYMCANotes = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCANotes = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCANotes");
				if (CommandSearchYMCANotes ==null) return null;

				db.AddInParameter(CommandSearchYMCANotes,"@varcharGuiUniqueId",DbType.String,parameterSearchYMCANotesGuiUniqueId);
				
				dsSearchYMCANotes = new DataSet();
				dsSearchYMCANotes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCANotes,dsSearchYMCANotes,"YMCA Notes");

				return dsSearchYMCANotes;
			}
			catch
			{
				throw;
			}

		}


		// this function updates the general tab 
		public static void SaveYMCAGeneral(DataSet parameterYMCAGeneral)
		{
			Database db= null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;

			try {
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
	
				insertCommand=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralAtsYmcas");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommand,"@varchar_YmcaName",DbType.String, "chvymcaname",DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_YmcaNo", DbType.String, "chrymcano", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_TaxNumberFederal", DbType.String, "chrtaxnumberfederal", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_TaxNumberState", DbType.String, "chrtaxnumberstate", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_hubind", DbType.String, "Chrhubind", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_EntryDate", DbType.DateTime, "dtsEntryDate", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_DefaultPaymentCode", DbType.String, "chvDefaultPaymentCode", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_BillingMethodCode", DbType.String, "chvBillingMethodCode", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_MetroGuiUniqueId", DbType.String, "MetroGuiUniqueId", DataRowVersion.Current);
                db.AddOutParameter(insertCommand, "@uniqueidentifier_guiUniqueID", DbType.String, 100);
				//Below Line is added by Dilip Yadav : 28-Oct-09 : YRS 5.0.921 : To provide priority handling
                db.AddInParameter(insertCommand, "@bit_Priority", DbType.Boolean, "Priority", DataRowVersion.Current);
                //Start: Bala: 01/19/2019: YRS-AT-2398: New three parameters added.
                db.AddInParameter(insertCommand, "@bit_ET", DbType.Boolean, "EligibleTrackingUser", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_YNAN", DbType.Boolean, "YNAN", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_MidMajors", DbType.Boolean, "MidMajors", DataRowVersion.Current);
                //End: Bala: 01/19/2019: YRS-AT-2398: New three parameters added.

				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateAtsYmcaGeneral");
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommand,"@varchar_GuiUniqueId",DbType.String, "guiUniqueId",DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@varchar_YmcaName", DbType.String, "chvymcaname", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_YmcaNo", DbType.String, "chrymcano", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_TaxNumberFederal", DbType.String, "chrtaxnumberfederal", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_TaxNumberState", DbType.String, "chrtaxnumberstate", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_HubInd", DbType.String, "Chrhubind", DataRowVersion.Current);
				db.AddInParameter(updateCommand,"@datetime_EntryDate",DbType.DateTime,"dtsEntryDate",DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_DefaultPaymentCode", DbType.String, "chvDefaultPaymentCode", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_BillingMethodCode", DbType.String, "chvBillingMethodCode", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_guiYmcaMetroID", DbType.String, "guiYmcaMetroID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_MetroGuiUniqueId", DbType.String, "MetroGuiUniqueId", DataRowVersion.Current);
				//Below Line is added by Dilip Yadav : 28-Oct-09 : YRS 5.0.921 : To provide priority handling
                db.AddInParameter(updateCommand, "@bit_Priority", DbType.Boolean, "Priority", DataRowVersion.Current);
                //Neeraj 25-Oct-2010 for issue id BT-667: added new parameter guiAtsYmcaEntrantsID
                db.AddInParameter(updateCommand, "@guiAtsYmcaEntrantsID", DbType.Guid, "guiAtsYmcaEntrantsID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_RelationManager", DbType.String, "RelationManager", DataRowVersion.Current);
                //Start: Bala: 01/19/2019: YRS-AT-2398: New three parameters added.
                db.AddInParameter(updateCommand, "@bit_ET", DbType.Boolean, "EligibleTrackingUser", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_YNAN", DbType.Boolean, "YNAN", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_MidMajors", DbType.Boolean, "MidMajors", DataRowVersion.Current);
                //End: Bala: 01/19/2019: YRS-AT-2398: New three parameters added.
                
                
                if (parameterYMCAGeneral != null)
                {
                    db.UpdateDataSet(parameterYMCAGeneral, parameterYMCAGeneral.Tables[0].TableName, insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard);
                    string newId = db.GetParameterValue(insertCommand, "@uniqueidentifier_guiUniqueID").ToString();
                    if (newId != null && newId != string.Empty) parameterYMCAGeneral.Tables[0].Rows[0]["guiUniqueID"] = newId;
                    parameterYMCAGeneral.AcceptChanges();
                }

                
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (parameterYMCAGeneral.Tables[0].Rows.Count > 0) {
					ymcaId = parameterYMCAGeneral.Tables[0].Rows[0]["guiUniqueId"].ToString();
				}
				DataSet refreshedData = SearchYMCAGeneral(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) {
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					parameterYMCAGeneral.Tables.RemoveAt(0);
					parameterYMCAGeneral.Tables.Add(dt);
				}
			}
			catch {
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} 
			finally {
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) {
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}
		}

		public static DataSet LookUpGeneralPaymentMethod()
		{
			DataSet dsLookUpPaymentMethod = null;
			Database db = null;
			DbCommand commandLookUpPaymentMethod = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpPaymentMethod = db.GetStoredProcCommand("yrs_usp_AMY_SearchGeneralPaymentMethod");
				if (commandLookUpPaymentMethod ==null) return null;
				dsLookUpPaymentMethod = new DataSet();
				dsLookUpPaymentMethod.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpPaymentMethod,dsLookUpPaymentMethod,"General Payment Method");
				return dsLookUpPaymentMethod;
			}
			catch
			{
				throw;
			}

		}


		public static DataSet LookUpGeneralHubType()
		{
			DataSet dsLookUpHubType = null;
			Database db = null;
			DbCommand commandLookUpHubType= null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpHubType = db.GetStoredProcCommand("yrs_usp_AMY_SearchGeneralHubType");
				if (commandLookUpHubType ==null) return null;
				dsLookUpHubType = new DataSet();
				dsLookUpHubType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpHubType,dsLookUpHubType,"General Hub Type");
				return dsLookUpHubType;
			}
			catch
			{
				throw;
			}

		}
       //Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Added method for updating suspending Date
        public static void UpdateYMCASuspendDate(DateTime YMCAWithdrawalDate,string GuiYmcaId,char bitAllowYerdiAccess,DateTime YMCASuspendEffectiveDate,DbTransaction pDBTransaction, Database db)
        {
            DbCommand updateCommandWrapper = null;

            try
            {

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateAtsYmcaEntrantsAtsYmcaRessSuspend");
                // Defining The Update Command Wrapper With parameters

                db.AddInParameter(updateCommandWrapper, "@varchar_GuiYmcaId", DbType.String, GuiYmcaId);
                db.AddInParameter(updateCommandWrapper, "@date_YMCAWithdrawalDate", DbType.DateTime, YMCAWithdrawalDate);
                db.AddInParameter(updateCommandWrapper, "@bitAllowYerdiAccess", DbType.String, bitAllowYerdiAccess);
                db.AddInParameter(updateCommandWrapper, "@date_YMCASuspendEffectiveDate", DbType.DateTime, YMCASuspendEffectiveDate);
                updateCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(updateCommandWrapper, pDBTransaction);

            }
            catch
            {
                throw;
            }
        }
        //End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Added method for updating suspending Date

        //Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Added method for updating suspending YMCA and added parameter to check activity performed at WTSM Tab
        //public static void UpdateOnSuspendingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCASuspendDate) // YRS-AT-2334 | Manthan Rajguru | 
        //  Start -  Manthan Rajguru |2016.02.15 | YRS-AT-2334 & 1686
        public static DateTime? UpdateOnSuspendingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCASuspendDate) 
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            bool bool_TransactionStarted = false;
            DateTime? l_output;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();
                bool_TransactionStarted = true;
                //call methods
                UpdateYMCASuspendDate(YMCAWithdrawalDate, GuiYmcaId, bitAllowYerdiAccess, YMCASuspendDate, DBTransaction, db);
                l_output = UpdateEmploymentofWithdrawnYmca(YMCAWithdrawalDate, GuiYmcaId, type, DBTransaction, db); // YRS-AT-2334 | Manthan Rajguru | Added parameter to pass activity performed


                DBTransaction.Commit();
                bool_TransactionStarted = false;
                DBconnectYRS.Close();
                return l_output;
            }
            //  End -  Manthan Rajguru |2016.02.15 | YRS-AT-2334 & 1686        
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
        //End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Added method for updating suspending YMCA and added parameter to check activity performed at WTSM Tab


//**********UPDATE ON WITHDRAWING A YMCA***************************start//
		// this function updates AtsYmcaEntrants,AtsYmcaRess table when a YMCA is withdrawn (By Swopna 8 Apr,2008 Phase IV,updates atsymcaentrants and atsymcaress tables)
        
                
		public static void UpdateYMCAWithdrawalDate(DateTime YMCAWithdrawalDate,string GuiYmcaId,char bitAllowYerdiAccess,DateTime YMCAWithdrawalEffectiveDate,DbTransaction pDBTransaction, Database db)
		{
			
			DbCommand updateCommandWrapper = null;
			
			try
			{
																										
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateAtsYmcaEntrantsAtsYmcaRess");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@varchar_GuiYmcaId",DbType.String,GuiYmcaId);
                db.AddInParameter(updateCommandWrapper, "@date_WithdrawalDate", DbType.DateTime, YMCAWithdrawalDate);
                db.AddInParameter(updateCommandWrapper, "@bitAllowYerdiAccess", DbType.String, bitAllowYerdiAccess);
                db.AddInParameter(updateCommandWrapper, "@date_YMCAWithdrawalEffectiveDate", DbType.DateTime, YMCAWithdrawalEffectiveDate);
				updateCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]) ; 
										
				db.ExecuteNonQuery(updateCommandWrapper,pDBTransaction);
			


			}
			catch
			{
				throw;
			}
		}
		//function to update employment of active employees on withdrawing YMCA

        //public static void UpdateEmploymentofWithdrawnYmca(DateTime YMCAWithdrawalDate, string GuiYmcaId, char type, DbTransaction pDBTransaction, Database db)  // YRS-AT-2334 | Manthan Rajguru | Added parameter to check activity performed at WTSM tab
        // start - | Manthan Rajguru |2016.02.15|YRS-AT-2334 & 1686
        public static DateTime? UpdateEmploymentofWithdrawnYmca(DateTime YMCAWithdrawalDate, string GuiYmcaId, char type, DbTransaction pDBTransaction, Database db)  
		{
			
			DbCommand CommandUpdateEmployment = null;
            //DbCommand LookUpCommandWrapper = null;
            DateTime? l_output;
			
			try
			{
				
																												
				CommandUpdateEmployment = db.GetStoredProcCommand("yrs_usp_AMY_UpdateEmploymentofWithdrawnYmca");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(CommandUpdateEmployment,"@varchar_GuiYmcaId",DbType.String,GuiYmcaId);
				db.AddInParameter(CommandUpdateEmployment,"@date_WithdrawalDate",DbType.DateTime,YMCAWithdrawalDate);
                db.AddInParameter(CommandUpdateEmployment, "@type", DbType.String, type); //Start - YRS-AT-2334 | Manthan Rajguru | Added parameter to check activity performed at WTSM Tab
                db.AddOutParameter(CommandUpdateEmployment, "@datetime_updatedDate", DbType.DateTime, 0);
                CommandUpdateEmployment.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);							
				db.ExecuteNonQuery(CommandUpdateEmployment,pDBTransaction);
                l_output = Convert.IsDBNull(db.GetParameterValue(CommandUpdateEmployment, "@datetime_updatedDate")) ? null : (DateTime?)(db.GetParameterValue(CommandUpdateEmployment, "@datetime_updatedDate"));                
                return l_output;
                // End - | Manthan Rajguru |2016.02.15|YRS-AT-2334 & 1686
            }
			catch
			{
				throw;
			}
		}


        //public static void UpdateOnWithdrawingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCAWithdrawalEffectiveDate) // YRS-AT-2334 | Manthan Rajguru | Added parameter to check activity performed at WTSM Tab
        public static DateTime? UpdateOnWithdrawingYMCA(DateTime YMCAWithdrawalDate, string GuiYmcaId, char bitAllowYerdiAccess, char type, DateTime YMCAWithdrawalEffectiveDate) // YRS-AT-2334 | Manthan Rajguru | Added parameter to check activity performed at WTSM Tab
		{

			Database db = null;			
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;
			bool bool_TransactionStarted = false;
            DateTime? l_output;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				//if (db == null) return false;
				
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
				bool_TransactionStarted = true;
				//call methods
				UpdateYMCAWithdrawalDate(YMCAWithdrawalDate,GuiYmcaId,bitAllowYerdiAccess,YMCAWithdrawalEffectiveDate,DBTransaction,db);
                l_output = UpdateEmploymentofWithdrawnYmca(YMCAWithdrawalDate, GuiYmcaId, type, DBTransaction, db); // YRS-AT-2334 | Manthan Rajguru | Added parameter to check activity performed at WTSM Tab					
			

				DBTransaction.Commit();
				bool_TransactionStarted=false;
				DBconnectYRS.Close();
                return l_output;
				
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
//**********UPDATE ON WITHDRAWING A YMCA***************************end//
		// this function updates AtsYmcaEntrants,AtsYmcaRess table(either both tables or only AtsYmcaEntrants table) when a YMCA is terminated (By Swopna 11 Apr,2008 Phase IV)
		public static void UpdateOnTerminationOfYmca(DateTime YMCATerminationDate,string GuiYmcaId,int g_integer_update,char bitAllowYerdiAccess)
		{
			Database db= null;
			DbCommand updateCommandWrapper = null;
			
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				
																												
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateOnTerminationOfYmca");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@varchar_GuiYmcaId",DbType.String,GuiYmcaId);
				db.AddInParameter(updateCommandWrapper,"@date_TerminationDate",DbType.DateTime,YMCATerminationDate);
				db.AddInParameter(updateCommandWrapper,"@int_update",DbType.Int32,g_integer_update);
				db.AddInParameter(updateCommandWrapper,"@bitAllowYerdiAccess",DbType.String,bitAllowYerdiAccess);

				updateCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
								
				db.ExecuteNonQuery(updateCommandWrapper);
			


			}
			catch
			{
				throw;
			}
		}
		//To update a particular YMCA's hubid to 'M' (metro) when another YMCA is merged to it . 
		public static void UpdateHubIdInAtsYmcas(string GuiYmcaMetroId,DbTransaction pDBTransaction, Database db)
		{
			
			DbCommand updateCommandWrapper = null;
			
			try
			{																											
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateHubIdInAtsYmcas");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@varchar_GuiUniqueId",DbType.String,GuiYmcaMetroId);	
				updateCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]) ;
								
				db.ExecuteNonQuery(updateCommandWrapper,pDBTransaction);
			}
			catch
			{
				throw;
			}
		}
		// this function called on merging a YMCA(By Swopna 23 Apr,2008 Phase IV)
		public static void UpdateOnMerge(DateTime YMCAMergeDate,string GuiYmcaId,string GuiYmcaMetroID,DateTime YMCAEffectiveMergeDate,DbTransaction pDBTransaction, Database db)
		{
			
			DbCommand updateCommandWrapper = null;
			
			try
			{


                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateTablesOnMerge");
				
				db.AddInParameter(updateCommandWrapper,"@varchar_GuiYmcaId",DbType.String,GuiYmcaId);
                db.AddInParameter(updateCommandWrapper, "@date_WithdrawalDate", DbType.DateTime, YMCAMergeDate);
                db.AddInParameter(updateCommandWrapper, "@varchar_GuiYmcaMetroID", DbType.String, GuiYmcaMetroID);
                db.AddInParameter(updateCommandWrapper, "@date_YMCAEffectiveMergeDate", DbType.String, YMCAEffectiveMergeDate);
				
				updateCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]) ;
				db.ExecuteNonQuery(updateCommandWrapper,pDBTransaction);	


			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetStatus(string parameterUniqueId)
		{
			DataSet l_dataset_YmcaInfo = null;
			Database db = null;
			string [] l_TableNames;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetYMCAStatus");
				db.AddInParameter(LookUpCommandWrapper,"@varchar_GuiUniqueId",DbType.String,parameterUniqueId);
				if (LookUpCommandWrapper == null) return null;
				
				l_dataset_YmcaInfo = new DataSet();
				l_TableNames = new string[]{"atsYmcas","atsYmcaEntrants"} ;
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_YmcaInfo,l_TableNames);
				
				return l_dataset_YmcaInfo;
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet GetTotalActiveParticipants(string parameterUniqueId)
		{
			DataSet l_dataset_TotalActiveParticipants = null;
			Database db = null;
			string [] l_TableNames;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetTotalActiveParticipants");
				db.AddInParameter(LookUpCommandWrapper,"@varchar_GuiYMCAId",DbType.String,parameterUniqueId);
				if (LookUpCommandWrapper == null) return null;
				
				l_dataset_TotalActiveParticipants = new DataSet();
				l_TableNames = new string[]{"ActiveEmployees","NonActiveEmployees"} ;
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_TotalActiveParticipants,l_TableNames);
				
				return l_dataset_TotalActiveParticipants;
			}
			catch 
			{
				throw;
			}
		}
		//'Added---Swopna 20June,2008-------Start
        //Start - Manthan Rajguru | 2016.02.15 | YRS-AT-2334 & 1686 | Added parameter for updated datetime
		public static DataSet GetParticipantStatusOnWithdrawal(string GuiYmcaId,DateTime YMCAWithdrawalDate,DateTime updatedDate)
		{
			DataSet l_dataset_TotalNonParticipants = null;
			Database db = null;
			string [] l_TableNames;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetParticipantStatusOnWithdrawal");
				db.AddInParameter(LookUpCommandWrapper,"@varchar_GuiYMCAId",DbType.String,GuiYmcaId);
				db.AddInParameter(LookUpCommandWrapper,"@date_WithdrawalDate",DbType.DateTime,YMCAWithdrawalDate);
                db.AddInParameter(LookUpCommandWrapper, "@date_updatedDate", DbType.DateTime, updatedDate);
				if (LookUpCommandWrapper == null) return null;
				
				l_dataset_TotalNonParticipants = new DataSet();
				l_TableNames = new string[]{"NonParticipantEmployees"} ;
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_TotalNonParticipants,l_TableNames);
				
				return l_dataset_TotalNonParticipants;
			}
			catch 
			{
				throw;
			}
		}
        //End - Manthan Rajguru | 2016.02.15 | YRS-AT-2334 & 1686 | Added parameter for updated datetime
		//'Added---Swopna 20June,2008-------End
//**********UPDATE/INSERT ON RE-ACTIVATION OF YMCA**************START//
//Insert record in AtsYmcaEntrants on reactivating a withdrawn/terminated YMCA.(Swopna Phase IV)
		public static void InsertRecordOnReactivation(DateTime EntryDate,string DefaultPaymentCode,string BillingMethodCode,string parameterYmcaId,DbTransaction pDBTransaction, Database db)
		{
			
			DbCommand insertCommandWrapper = null;
			
			try
			{
				
				
																												
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_InsertRecordOnReactivation");

				db.AddInParameter(insertCommandWrapper,"@datetime_EntryDate ",DbType.DateTime,EntryDate);
                db.AddInParameter(insertCommandWrapper, "@varchar_DefaultPaymentCode", DbType.String, DefaultPaymentCode);
                db.AddInParameter(insertCommandWrapper, "@varchar_BillingMethodCode", DbType.String, BillingMethodCode);
                db.AddInParameter(insertCommandWrapper, "@varchar_GuiYmcaId", DbType.String, parameterYmcaId);
			
								
				db.ExecuteNonQuery(insertCommandWrapper,pDBTransaction);
			


			}
			catch
			{
				throw;
			}
		}
		//this function checks if all transmittals have been received between last received transmittal and proposed withdrawal date.(26May08 Swopna)
		public static DataSet SearchTransmittals(string parameterYMCAId,DateTime YMCAWithdrawalDate)
		{
			DataSet l_dataset_YmcaTransmittal = null;
			Database db = null;
			string [] l_TableNames;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_CheckTransmittalBeforeWithdrawal");
				db.AddInParameter(LookUpCommandWrapper,"@varchar_GuiYmcaId",DbType.String,parameterYMCAId);
				db.AddInParameter(LookUpCommandWrapper,"@date_WithdrawalDate",DbType.DateTime,YMCAWithdrawalDate);	
				if (LookUpCommandWrapper == null) return null;
				
				l_dataset_YmcaTransmittal = new DataSet();
				l_TableNames = new string[]{"EarlierTransmittals","Transmittals"} ;
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_YmcaTransmittal,l_TableNames);
				
				return l_dataset_YmcaTransmittal;
			}
			catch 
			{
				throw;
			}
		}

		//this function only when a withdrawn YMCA is reactivated
		//added -Swopna -withdrawal date(corresponding stored procedure changed)13May08
		public static void UpdateEmploymentofReactivatedYmca(DateTime YMCAReActivationDate,string GuiYmcaId,DateTime YMCAWithdrawalDate,DbTransaction pDBTransaction, Database db)
		{
			
			DbCommand CommandUpdateInsert = null;
			
			try
			{
				
																												
				CommandUpdateInsert= db.GetStoredProcCommand("yrs_usp_AMY_UpdateEmploymentofReActivatedYmca");

				db.AddInParameter(CommandUpdateInsert,"@date_ReactivationDate",DbType.DateTime,YMCAReActivationDate);
                db.AddInParameter(CommandUpdateInsert, "@varchar_GuiYmcaId", DbType.String, GuiYmcaId);
                db.AddInParameter(CommandUpdateInsert, "@date_WithdrawalDate", DbType.DateTime, YMCAWithdrawalDate);
				CommandUpdateInsert.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
								
				db.ExecuteNonQuery(CommandUpdateInsert,pDBTransaction);
			


			}
			catch
			{
				throw;
			}
		}

		public static void UpdateOnReActivatingYMCA(DateTime YMCAReactivationDate,string DefaultPaymentCode,string BillingMethodCode,string GuiYmcaId,int YMCA_Status,DateTime YMCAWithdrawalDate)
		{

			Database db = null;			
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;
			bool bool_TransactionStarted = false;
			
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				//if (db == null) return false;
				
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
				bool_TransactionStarted = true;
				//call methods				
				InsertRecordOnReactivation(YMCAReactivationDate,DefaultPaymentCode,BillingMethodCode,GuiYmcaId,DBTransaction,db);
	
				if (YMCA_Status==1 || YMCA_Status==3)
				{//Swopna -added withdrawal date(corresponding stored procedure changed)
					UpdateEmploymentofReactivatedYmca(YMCAReactivationDate,GuiYmcaId,YMCAWithdrawalDate,DBTransaction, db);
				}
			
				DBTransaction.Commit();
				bool_TransactionStarted=false;
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
		//**********UPDATE/INSERT ON RE-ACTIVATION OF YMCA**************END//
		//procedure to get all Metro YMCAs     (By Swopna)
		//Priya:17-March-2010:YRS 5.0-1017:Display withdrawal date as merged date for branches
		//Added parameter to pass metro id to get details of metroymca
		public static DataSet GetMetroYMCAs(string strMetroYMCAID)
		{
			DataSet dsGetMetroYMCAs = null;
			Database db = null;
			DbCommand CommandGetMetroYMCAs = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandGetMetroYMCAs = db.GetStoredProcCommand("dbo.yrs_usp_AMY_GetMetroYMCAs");
				db.AddInParameter(CommandGetMetroYMCAs,"@varchar_MetroYMCAId",DbType.String,strMetroYMCAID);	
				
				if (CommandGetMetroYMCAs ==null) return null;				
				dsGetMetroYMCAs = new DataSet();
				db.LoadDataSet(CommandGetMetroYMCAs,dsGetMetroYMCAs,"Metro YMCA");


				return dsGetMetroYMCAs;
			}
			catch
			{
				throw;
			}
		}
	
		// this function terminates active employee records and inserts new employment record when a YMCA is merged (By Swopna 23 Apr,2008 Phase IV)
		public static void UpdateEmploymentofMergedYmca(DateTime YMCAMergeDate,string GuiYmcaId,string GuiYmcaMetroID, DbTransaction pDBTransaction, Database db)
		{
			
			DbCommand CommandUpdateInsert = null;
			
			try
			{
																							
				CommandUpdateInsert= db.GetStoredProcCommand("yrs_usp_AMY_UpdateEmploymentofMergedYmca");
                CommandUpdateInsert.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				db.AddInParameter(CommandUpdateInsert,"@varchar_GuiYmcaId",DbType.String,GuiYmcaId);
                db.AddInParameter(CommandUpdateInsert, "@varchar_GuiMetroYmcaId", DbType.String, GuiYmcaMetroID);
                db.AddInParameter(CommandUpdateInsert, "@date_MergeDate", DbType.DateTime, YMCAMergeDate);			
				
				//CommandUpdateInsert.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationSettings.AppSettings ["MediumConnectionTimeOut"]) ;				
				db.ExecuteNonQuery(CommandUpdateInsert,pDBTransaction);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Created By : Paramesh K.
		/// Created On : July 31st 2008
		/// This Method will return List of Participants who has opened loans after merging
		/// </summary>
		/// <returns></returns>
		public static DataSet GetParticipantsHavingOpenLoans(string GuiYmcaId,string GuiYmcaMetroID)
		{
			DataSet dsPersons = null;
			Database db = null;
			DbCommand CommandGetParticipants = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandGetParticipants = db.GetStoredProcCommand("dbo.yrs_usp_AMY_GetParticipantsHavingOpenLoans");
				if (CommandGetParticipants ==null) return null;				
				
				db.AddInParameter(CommandGetParticipants,"@varchar_GuiYmcaId",DbType.String,GuiYmcaId);
				db.AddInParameter(CommandGetParticipants,"@varchar_GuiYmcaMetroID",DbType.String,GuiYmcaMetroID);
				
				dsPersons = new DataSet();
				db.LoadDataSet(CommandGetParticipants,dsPersons,"Participants");


				return dsPersons;
			}
			catch
			{
				throw;
			}
		}

		public static void UpdateOnMergingYMCA(DateTime YMCAMergeDate,string GuiYmcaId,string GuiYmcaMetroID, DateTime YMCAEffectiveMergeDate)
		{

			Database db = null;			
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;
			bool bool_TransactionStarted = false;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				//if (db == null) return false;
				
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
				bool_TransactionStarted = true;
				//call methods

				UpdateOnMerge(YMCAMergeDate,GuiYmcaId,GuiYmcaMetroID,YMCAEffectiveMergeDate,DBTransaction,db);
				UpdateEmploymentofMergedYmca(YMCAMergeDate,GuiYmcaId,GuiYmcaMetroID,DBTransaction,db);
				UpdateHubIdInAtsYmcas(GuiYmcaMetroID,DBTransaction,db);

				DBTransaction.Commit();
				bool_TransactionStarted=false;
				DBconnectYRS.Close();
				
			} catch	{
				if (bool_TransactionStarted)
				{
					DBTransaction.Rollback();
					DBconnectYRS.Close();
				}
				throw;
			}
		}	


		public static void SaveYMCAGeneralAddressInformation(DataSet ds) {

			Database db = null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;

			try {
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();

				insertCommand = db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralAddressInformationAdd");
				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralAddressInformation");

				db.AddParameter(insertCommand,"@uniqueIdentifier_GuiUniqueID", DbType.String,1000, ParameterDirection.Output,true ,0 ,0 , "guiUniqueID", DataRowVersion.Default, "guiUniqueID");
                db.AddInParameter(insertCommand, "@uniqueIdentifier_GuiEntityId", DbType.String, "guiEntityID", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_Primary", DbType.Boolean, "Make this Primary", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_Active", DbType.Boolean, "Active", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_addr1", DbType.String, "Address", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_addr2", DbType.String, "Address 2", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_addr3", DbType.String, "Address 3", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_city", DbType.String, "City", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_state", DbType.String, "State", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_zip", DbType.String, "Zip", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_Country", DbType.String, "Country", DataRowVersion.Current);
				insertCommand.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;

				db.AddInParameter(updateCommand,"@uniqueIdentifier_GuiUniqueID", DbType.String, "guiUniqueID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@uniqueIdentifier_GuiEntityId", DbType.String, "guiEntityID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_Primary", DbType.Boolean, "Make this Primary", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_Active", DbType.Boolean, "Active", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_addr1", DbType.String, "Address", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_addr2", DbType.String, "Address 2", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_addr3", DbType.String, "Address 3", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_city", DbType.String, "City", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_state", DbType.String, "State", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_zip", DbType.String, "Zip", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_Country", DbType.String, "Country", DataRowVersion.Current);
				updateCommand.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;

				db.UpdateDataSet(ds, ds.Tables[0].TableName,  insertCommand, updateCommand, deleteCommand, DBTransaction);
				
				//db.ExecuteNonQuery(CommandUpdateInsert,pDBTransaction);
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) {
					ymcaId = ds.Tables[0].Rows[0]["guiEntityID"].ToString();
				}
				DataSet refreshedData = YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.SearchAddressInformation(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) {
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					ds.Tables.RemoveAt(0);
					ds.Tables.Add(dt);
				}
			} catch {
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} finally {
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) {
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}
		}

		public static void SaveYMCAGeneralTelephoneInformation(DataSet parameterTelephoneInformation)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();

				//db=DatabaseFactory.CreateDatabase("YRS");
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralTelephoneAdd");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String,"guiEntityID",DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_Primary", DbType.Int16, "Primary", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Telephone", DbType.String, "Telephone", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_ext", DbType.String, "Ext.", DataRowVersion.Current);
                db.AddOutParameter(insertCommandWrapper, "@uniqueIdentifier_GuiUniqueID", DbType.String, 100);
				
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralTelephone");
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Original);
                db.AddInParameter(updateCommandWrapper, "@uniqueIdentifier_GuiUniqueID", DbType.String, "guiUniqueID", DataRowVersion.Original);
                db.AddInParameter(updateCommandWrapper, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@bit_Primary", DbType.Int16, "Primary", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Telephone", DbType.String, "Telephone", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Ext", DbType.String, "Ext.", DataRowVersion.Current);
																		
				db.UpdateDataSet(parameterTelephoneInformation,parameterTelephoneInformation.Tables[0].TableName ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,DBTransaction);
				
				//l_stringAtsTelephoneGuiUniqueId=(string)insertCommandWrapper.GetParameterValue("@varchar_ReturnGuiUniqueId");


				//db.ExecuteNonQuery(CommandUpdateInsert,pDBTransaction);
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (parameterTelephoneInformation.Tables[0].Rows.Count > 0) 
				{
					ymcaId = parameterTelephoneInformation.Tables[0].Rows[0]["guiEntityID"].ToString();
				}
				DataSet refreshedData = YMCARET.YmcaDataAccessObject.YMCATelephoneDAClass.SearchTelephoneInformation(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) 
				{
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					parameterTelephoneInformation.Tables.RemoveAt(0);
					parameterTelephoneInformation.Tables.Add(dt);
				}
				
				//return l_stringAtsTelephoneGuiUniqueId;
			}
			catch 
			{
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} 
			finally 
			{
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) 
				{
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommandWrapper = null;
				updateCommandWrapper = null;
				deleteCommandWrapper = null;
				db = null;
			}
		}

		// this function insert and update the Resolution tab 
		public static void InsertYMCAResolution(DataSet ds) {
			Database db= null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;
			
			try {
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
	
				insertCommand=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaResolution");
				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCAResolution");

				// Defining The Insert Command Wrapper With parameters
                db.AddParameter(insertCommand, "@uniqueIdentifier_guiUniqueID", DbType.String, 1000, ParameterDirection.Output, true, 0, 0, "guiUniqueID", DataRowVersion.Default, "guiUniqueID");//BS:2011.08.05:BT:921 - bug occur:'String[0] property size is invalid',to resolve this bug:put size of parameter is 1000
                db.AddInParameter(insertCommand, "@uniqueIdentifier_guiYmcaID", DbType.String, "guiYmcaID", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_EffDate", DbType.DateTime, "Eff. Date", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_Termdate", DbType.DateTime, "Term. Date", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_VestingTypeCode", DbType.String, "Vesting Type", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_BasicAcctType", DbType.String, "Resolution Type", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@numeric_ConstituentPctg", DbType.Double, "Part.%", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@numeric_YmcaPctg", DbType.Double, "YMCA%", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@numeric_YmcaComboPctg", DbType.Double, "S.Scale%", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@numeric_AddlMarginPctg", DbType.Double, "Add'l YMCA%", DataRowVersion.Current);

				// Defining The Update Command Wrapper With parameters
                db.AddInParameter(updateCommand,"@uniqueIdentifier_guiUniqueID",DbType.String, "guiUniqueID",DataRowVersion.Original);
                db.AddInParameter(updateCommand,"@uniqueIdentifier_guiYmcaID",DbType.String, "guiYmcaID",DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@datetime_EffDate", DbType.DateTime, "Eff. Date", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@datetime_Termdate", DbType.DateTime, "Term. Date", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_VestingTypeCode", DbType.String, "Vesting Type", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_BasicAcctType", DbType.String, "Resolution Type", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@numeric_ConstituentPctg", DbType.Double, "Part.%", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@numeric_YmcaPctg", DbType.Double, "YMCA%", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@numeric_YmcaComboPctg", DbType.Double, "S.Scale%", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@numeric_AddlMarginPctg", DbType.Double, "Add'l YMCA%", DataRowVersion.Current);

				db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommand, updateCommand, deleteCommand, DBTransaction);
				
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) {
					ymcaId = ds.Tables[0].Rows[0]["guiYmcaID"].ToString();
				}
				DataSet refreshedData = SearchYMCAResolution(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) {
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					ds.Tables.RemoveAt(0);
					ds.Tables.Add(dt);
				}
			}
			catch {
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} 
			finally {
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) {
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}
		}
		
		// this function insert and update the BankInfo tab 
		public static void InsertYMCABankInfo(DataSet ds) {
			Database db = null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;

			try {
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();

				insertCommand = db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaBankInfo");
				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCABankInfo");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommand,"@uniqueIdentifier_guiYmcaID",DbType.String, "guiYmcaID",DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_BankAcctNumber", DbType.String, "Account #", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_BankName", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_BankAbaNumber", DbType.String, "Bank ABA#", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_PaymentMethod", DbType.String, "Payment Method", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_AccountType", DbType.String, "Account Type", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@DateTime_EffDate", DbType.Date, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_guiBankID", DbType.String, "guiBankId", DataRowVersion.Current);
				
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommand,"@uniqueIdentifier_guiUniqueID",DbType.String, "guiUniqueID",DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@uniqueIdentifier_guiYmcaID", DbType.String, "guiYmcaID", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@varchar_BankAcctNumber", DbType.String, "Account #", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_BankName", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_BankAbaNumber", DbType.String, "Bank ABA#", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_PaymentMethod", DbType.String, "Payment Method", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_AccountType", DbType.String, "Account Type", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@DateTime_EffDate", DbType.Date, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_guiBankID", DbType.String, "guiBankId", DataRowVersion.Current);

				db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommand, updateCommand, deleteCommand, DBTransaction);

				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) {
					ymcaId = ds.Tables[0].Rows[0]["guiYmcaID"].ToString();
				}
				DataSet refreshedData = SearchYMCABankInfo(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) {
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					ds.Tables.RemoveAt(0);
					ds.Tables.Add(dt);
				}
			}
			catch {
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} 
			finally {
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) {
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}

		}

		// this function insert and update the BankInfo tab 
		public static void SaveYMCANotes(DataSet ds) {
			Database db= null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;

			try {
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();

				insertCommand=db.GetStoredProcCommand("yrs_usp_AMY_InsertYMCANotes");
				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCANotes");
				// Defining the command parameters				
				db.AddInParameter(insertCommand,"@varchar_guiUniqueID",DbType.Guid,"guiUniqueID",DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_PersonID", DbType.String, "guiEntityID", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_NoteTypeCode", DbType.String, 1);
                db.AddInParameter(insertCommand, "@text_Note", DbType.String, "First Line of Notes", DataRowVersion.Current);
				db.AddInParameter(insertCommand,"@bit_Important",DbType.Boolean,"bitImportant",DataRowVersion.Current);
                // Defining the command parameters
				db.AddInParameter(updateCommand,"@varchar_guiUniqueID",DbType.Guid,"guiUniqueID",DataRowVersion.Current);
				db.AddInParameter(updateCommand,"@bit_Important",DbType.String,"bitImportant",DataRowVersion.Current);

				db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommand, updateCommand, deleteCommand, DBTransaction);
				
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) {
					ymcaId = ds.Tables[0].Rows[0]["guiEntityID"].ToString();
				}
				DataSet refreshedData = SearchYMCANotes(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) {
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					ds.Tables.RemoveAt(0);
					ds.Tables.Add(dt);
				}
			}
			catch {
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} 
			finally {
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) {
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}
		}

		// this function for insertion in the officer table 
		public static void SaveYMCAOfficers(DataSet ds) {
			Database db = null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;

			try {
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();

				insertCommand = db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaOfficer");
				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCAOfficer");
				deleteCommand = db.GetStoredProcCommand("yrs_usp_AMY_DeleteYMCAOfficer");
				
				// Defining The Insert Command Wrapper With parameters
                db.AddParameter(insertCommand, "@uniqueIdentifier_GuiUniqueID", DbType.String, 1000, ParameterDirection.Output, true, 0, 0, "guiUniqueID", DataRowVersion.Default, "guiUniqueID");//BS:2011.08.05:BT:921 - bug occur:'String[0] property size is invalid',to resolve this bug:put size of parameter is 1000
                db.AddInParameter(insertCommand, "@uniqueIdentifier_guiYmcaID", DbType.String, "GuiYmcaId", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_OfficerName", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_positionTitleCode", DbType.String, "chvPositionTitleCode", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_OfficerTelephone", DbType.String, "Phone No", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_OfficerExtnNo", DbType.String, "Extn No", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_OfficerEmail", DbType.String, "Email", DataRowVersion.Current);

				//---------------Start:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942------------------------

                db.AddInParameter(insertCommand, "@varchar_OfficerFundNo", DbType.Int64, "Fund No", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_OfficerFname", DbType.String, "First Name", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_OfficerMname", DbType.String, "Middle Name", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_OfficerLname", DbType.String, "Last Name", DataRowVersion.Current);
				
				//---------------End:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942--------------------------







				
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommand,"@uniqueIdentifier_guiUniqueID", DbType.String, "guiUniqueId", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@uniqueIdentifier_guiYmcaID", DbType.String, "guiYmcaID", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@varchar_OfficerName", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_positionTitleCode", DbType.String, "chvPositionTitleCode", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_OfficerTelephone", DbType.String, "Phone No", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_OfficerExtnNo", DbType.String, "Extn No", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_OfficerEmail", DbType.String, "Email", DataRowVersion.Current);


				//---------------Start:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942------------------------
			
				db.AddInParameter(updateCommand,"@varchar_OfficerFundNo", DbType.Int64, "Fund No", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_OfficerFname", DbType.String, "First Name", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_OfficerMname", DbType.String, "Middle Name", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_OfficerLname", DbType.String, "Last Name", DataRowVersion.Current);
				
				//---------------End:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942------------------------

				
				// Defining The Delete Command Wrapper With parameters
				db.AddInParameter(deleteCommand,"@uniqueIdentifier_guiUniqueID", DbType.String, "guiUniqueID", DataRowVersion.Original);
				db.AddInParameter(deleteCommand,"@uniqueIdentifier_guiYmcaID", DbType.String, "guiYmcaID", DataRowVersion.Original);
								
				db.UpdateDataSet(ds, ds.Tables[0].TableName,  insertCommand, updateCommand, deleteCommand, DBTransaction);
				
				//db.ExecuteNonQuery(CommandUpdateInsert,pDBTransaction);
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) {
					ymcaId = ds.Tables[0].Rows[0]["guiYmcaID"].ToString();
				}
				DataSet refreshedData = SearchYMCAOfficer(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) {
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					ds.Tables.RemoveAt(0);
					ds.Tables.Add(dt);
				}
			} catch {
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} finally {
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) {
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}		
		}

		// this function for insertion in the Contact table 
		public static void SaveYMCAContacts(DataSet ds) {
			Database db = null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
		DbConnection DBconnectYRS = null;

			try {
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();

				insertCommand = db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaContact");
				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCAContact");
				deleteCommand = db.GetStoredProcCommand("yrs_usp_AMY_DeleteYMCAContact");
	
				// Defining The Insert Command Wrapper With parameters
                db.AddParameter(insertCommand, "@uniqueIdentifier_GuiUniqueID", DbType.String, 1000, ParameterDirection.Output,true,0 , 0, "guiUniqueID", DataRowVersion.Default, "guiUniqueID");//BS:2011.08.08:BT:921 - bug occur:'String[0] property size is invalid',to resolve this bug:put size of parameter is 1000
                db.AddInParameter(insertCommand, "@uniqueIdentifier_guiYmcaID", DbType.String, "guiYmcaId", DataRowVersion.Current);
                //Changed by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
                db.AddInParameter(insertCommand, "@varchar_ContactType", DbType.String, "TypeCode", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_ContactName", DbType.String, "Contact Name", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_ContactTelephone", DbType.String, "Phone No", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_ContactExtnNo", DbType.String, "Extn No", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_Email", DbType.String, "Email", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_ContactNotes", DbType.String, "ContactNotes", DataRowVersion.Current);

				//---------------Start:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942------------------------

                db.AddInParameter(insertCommand, "@varchar_ContactFundNo", DbType.Int64, "Fund No", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_ContactFname", DbType.String, "First Name", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_ContactMname", DbType.String, "Middle Name", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_ContactLname", DbType.String, "Last Name", DataRowVersion.Current);
				
				//---------------End:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942--------------------------
                //Added by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
                db.AddInParameter(insertCommand, "@varchar_Title", DbType.String, "Title", DataRowVersion.Current);

				
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommand,"@uniqueIdentifier_guiUniqueID", DbType.String, "guiUniqueId", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@uniqueIdentifier_guiYmcaID", DbType.String, "guiYmcaID", DataRowVersion.Original);
                //Priya : BT-536: While saving contacts information application shows Error page. added paarameter @varchar_ContactType
                //Changed by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
                db.AddInParameter(updateCommand, "@varchar_ContactType", DbType.String, "TypeCode", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_ContactName", DbType.String, "Contact Name", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_ContactTelephone", DbType.String, "Phone No", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_ContactExtnNo", DbType.String, "Extn No", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_Email", DbType.String, "Email", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_ContactNotes", DbType.String, "ContactNotes", DataRowVersion.Current);
				
				//---------------Start:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942------------------------

                db.AddInParameter(updateCommand, "@varchar_ContactFundNo", DbType.Int64, "Fund No", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_ContactFname", DbType.String, "First Name", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_ContactMname", DbType.String, "Middle Name", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_ContactLname", DbType.String, "Last Name", DataRowVersion.Current);
				
				//---------------End:Shashi Shekhar:2010-03-05:Added For Yrs-5.0-942------------------------
                //'Added by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
                db.AddInParameter(updateCommand, "@varchar_Title", DbType.String, "Title", DataRowVersion.Current);

				// Defining The Delete Command Wrapper With parameters
				db.AddInParameter(deleteCommand,"@uniqueIdentifier_guiUniqueID", DbType.String, "guiUniqueID", DataRowVersion.Original);
				db.AddInParameter(deleteCommand,"@uniqueIdentifier_guiYmcaID", DbType.String, "guiYmcaID", DataRowVersion.Original);


				db.UpdateDataSet(ds, ds.Tables[0].TableName,  insertCommand, updateCommand, deleteCommand, DBTransaction);
				//db.ExecuteNonQuery(CommandUpdateInsert,pDBTransaction);
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) {
					ymcaId = ds.Tables[0].Rows[0]["guiYmcaID"].ToString();
				}
				DataSet refreshedData = SearchYMCAContact(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) {
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					ds.Tables.RemoveAt(0);
					ds.Tables.Add(dt);
				}
			} catch {
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} finally {
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) {
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}		
		}

		// this function for insertion in the Email table 
		public static void SaveYMCAGeneralEmailInformation(DataSet ds)
		{
			Database db= null;
			DbCommand insertCommand = null;
			DbCommand updateCommand = null;
			DbCommand deleteCommand = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();

				insertCommand = db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralEmailAdd");
				updateCommand = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralEmail");

				// Defining The Insert Command Wrapper With parameters
				db.AddParameter(insertCommand,"@uniqueIdentifier_GuiUniqueID", DbType.String,1000, ParameterDirection.Output, true, 0, 0, "guiUniqueID", DataRowVersion.Default, "guiUniqueID");
                db.AddInParameter(insertCommand, "@uniqueIdentifier_GuiEntityId", DbType.String, "guiEntityID", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_Primary", DbType.Int16, "Primary", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_EmailAddress", DbType.String, "Email Address", DataRowVersion.Current);
				
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommand,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@uniqueIdentifier_GuiEntityId", DbType.String, "guiEntityID", DataRowVersion.Original);
                db.AddInParameter(updateCommand, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_Primary", DbType.Int16, "Primary", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_EmailAddress", DbType.String, "Email Address", DataRowVersion.Current);
																		
				db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommand, updateCommand, deleteCommand, DBTransaction);
				
				DBTransaction.Commit();
				string ymcaId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) 
				{
					ymcaId = ds.Tables[0].Rows[0]["guiEntityID"].ToString();
				}
				DataSet refreshedData = YMCARET.YmcaDataAccessObject.YMCANetDAClass.SearchEmailInformation(ymcaId);
				if (refreshedData != null && refreshedData.Tables.Count > 0 ) 
				{
					DataTable dt = refreshedData.Tables[0];
					refreshedData.Tables.Remove(dt);
					ds.Tables.RemoveAt(0);
					ds.Tables.Add(dt);
				}
			}
			catch 
			{
				if (DBTransaction != null) DBTransaction.Rollback();
				throw;
			} 
			finally 
			{
				if (DBTransaction != null) DBTransaction.Dispose();
				if (DBconnectYRS != null) 
				{
					if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
					DBconnectYRS.Dispose();
					DBconnectYRS = null;
				}
				insertCommand = null;
				updateCommand = null;
				deleteCommand = null;
				db = null;
			}
		}


		//Insert record in atsOutputFileIDMTrackingLogs .(Sanjay Phase V)
		public static string SaveYMCAOutputFileIDMTrackingLogs(DataSet  ds)
		{			
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;
			
			try
			{		
				db= DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
																												
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_InsertRecordOnOutputFileIDMTrackingLogs");
				
				db.AddParameter(insertCommandWrapper,"@int_RptTrackingId",DbType.Int32 ,ParameterDirection.Output,"ReportTrackingID", DataRowVersion.Default, "ReportTrackingID");
                db.AddInParameter(insertCommandWrapper, "@varchar_Doccode", DbType.String, "chvDoccode", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_SubDocName", DbType.String, "chvSubDocName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_DocName", DbType.String, "chvDocName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_RefId", DbType.String, "chvRefId", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityType", DbType.String, "chrEntityType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_EntityId", DbType.String, "chvEntityId", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@datetime_Docdate", DbType.DateTime, "dtmDocdate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_idxFilePath", DbType.String, "chvidxFilePath", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_IdxFileName", DbType.String, "chvIdxFileName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_IdxCreated", DbType.Boolean, "bitIdxCreated", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_IdxCopyInitiated", DbType.Boolean, "bitIdxCopyInitiated", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_IdxCopied", DbType.Boolean, "bitIdxCopied", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_idxDeletedafterCopy", DbType.Boolean, "bitidxDeletedafterCopy", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_PdfFilePath", DbType.String, "chvPdfFilePath", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_PdfFileName", DbType.String, "chvPdfFileName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_PdfCreated", DbType.Boolean, "bitPdfCreated", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_PdfCopyInitiated", DbType.Boolean, "bitPdfCopyInitiated", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_PdfCopied", DbType.Boolean, "bitPdfCopied", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_pdfDeletedafterCopy", DbType.Boolean, "bitpdfDeletedafterCopy", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_DestnationFolder", DbType.String, "chvDestnationFolder", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_DocExistsInIDM", DbType.Boolean, "bitDocExistsInIDM", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@datetime_docVerifiedInIDM", DbType.DateTime, "dtmdocVerifiedInIDM", DataRowVersion.Current);
				
				db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommandWrapper,null,null, DBTransaction);				
				DBTransaction.Commit();

				string rptTrackingId = string.Empty;
				if (ds.Tables[0].Rows.Count > 0) 
				{
					rptTrackingId = ds.Tables[0].Rows[0]["ReportTrackingID"].ToString();
				}
				return rptTrackingId;
			}
			catch (Exception ex) 
			{

                throw ex;
			}
		}

		public static void UpdateYMCAOutputFileIDMTrackingLogs(int pTrackingId,bool pIdxFileCreated ,bool  pPdfFilecreated,string IdxFileName,bool IdxCopyInitiated,bool IdxCopied,bool idxDeletedafterCopy,string PdfFileName,bool PdfCopyInitiated,bool PdfCopied,bool pdfDeletedafterCopy,char FileType,string ErrorMsg) 
		{		
			Database db = null;			
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;			
			DbCommand CommandUpdate = null;
	    	try
			{
				db= DatabaseFactory.CreateDatabase("YRS_IDM");
							
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
																																
				CommandUpdate= db.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCAIDMtrackingLog");

				db.AddInParameter(CommandUpdate,"@int_TrackingId",DbType.Int32,pTrackingId);
                db.AddInParameter(CommandUpdate, "@bit_IdxCreated", DbType.Boolean, pIdxFileCreated);
                db.AddInParameter(CommandUpdate, "@bit_PdfCreated", DbType.Boolean, pPdfFilecreated);

                db.AddInParameter(CommandUpdate, "@varchar_IdxFileName", DbType.String, IdxFileName);
                db.AddInParameter(CommandUpdate, "@bit_IdxCopyInitiated", DbType.Boolean, IdxCopyInitiated);
                db.AddInParameter(CommandUpdate, "@bit_IdxCopied", DbType.Boolean, IdxCopied);
                db.AddInParameter(CommandUpdate, "@bit_idxDeletedafterCopy", DbType.Boolean, idxDeletedafterCopy);
                db.AddInParameter(CommandUpdate, "@varchar_PdfFileName", DbType.String, PdfFileName);
                db.AddInParameter(CommandUpdate, "@bit_PdfCopyInitiated", DbType.Boolean, PdfCopyInitiated);
                db.AddInParameter(CommandUpdate, "@bit_PdfCopied", DbType.Boolean, PdfCopied);
                db.AddInParameter(CommandUpdate, "@bit_pdfDeletedafterCopy", DbType.Boolean, pdfDeletedafterCopy);
                db.AddInParameter(CommandUpdate, "@char_FileType", DbType.String, FileType);
                db.AddInParameter(CommandUpdate, "@varchar_ErrorMsg", DbType.String, ErrorMsg);				

				db.ExecuteNonQuery(CommandUpdate,DBTransaction);
			
				DBTransaction.Commit();
				DBconnectYRS.Close();

			}
			catch(Exception  ex)
			{
//				string a = ex.Message;
				//ExceptionPolicy.HandleException(ex, "Exception Policy");
                throw ex;

			}
		}

		//Priya 24-01-2011: BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
		//Showing summery grid for termination 
		public static DataSet GetTotalBeforeTerminateParticipants(string parameterUniqueId)
			{
			DataSet l_dataset_TotalBeforeTerminateParticipants = null;
			Database db = null;
			string[] l_TableNames;
			DbCommand LookUpCommandWrapper = null;

			try
				{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetTotalBeforeTerminateParticipants");
				db.AddInParameter(LookUpCommandWrapper, "@varchar_GuiYMCAId", DbType.String, parameterUniqueId);
				if (LookUpCommandWrapper == null) return null;

				l_dataset_TotalBeforeTerminateParticipants = new DataSet();
				l_TableNames = new string[] { "BeforeTerminationEmployees"};
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_TotalBeforeTerminateParticipants, l_TableNames);

				return l_dataset_TotalBeforeTerminateParticipants;
				}
			catch
				{
				throw;
				}
			}

		public static DataSet GetParticipantStatusOnTermination(string GuiYmcaId, DateTime YMCATerminationDate, DateTime YMCAWithdrawnDate)
			{
			DataSet l_dataset_TotalTerminate = null;
			Database db = null;
			string[] l_TableNames;
			DbCommand LookUpCommandWrapper = null;

			try
				{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetParticipantStatusOnTermination");
				db.AddInParameter(LookUpCommandWrapper, "@varchar_GuiYMCAId", DbType.String, GuiYmcaId);
				db.AddInParameter(LookUpCommandWrapper, "@date_TermDate", DbType.DateTime, YMCATerminationDate);
				db.AddInParameter(LookUpCommandWrapper, "@date_WithdrawnDate", DbType.DateTime, YMCAWithdrawnDate);
				
				if (LookUpCommandWrapper == null) return null;

				l_dataset_TotalTerminate = new DataSet();
				l_TableNames = new string[] { "TerminateEmployees" };
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_TotalTerminate, l_TableNames);

				return l_dataset_TotalTerminate;
				}
			catch
				{
				throw;
				}
			}
	//End 24-01-2011: BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
      
        
        // Deven 02-Sep-2010
        public static DataTable GetYMCANoByGuiID(string parameterguiYMCAID)
        {
            DataSet dsYMCADetail = null;
            Database db = null;
            DbCommand commandPersonDetail = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandPersonDetail = db.GetStoredProcCommand("yrs_usp_FindPersonOrYMCADetail");

                if (commandPersonDetail == null) return null;

                commandPersonDetail.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);


                dsYMCADetail = new DataSet();
                db.AddInParameter(commandPersonDetail, "@varchar_SearchForm", DbType.String, "YMCA");
                db.AddInParameter(commandPersonDetail, "@varchar_guiYMCAID", DbType.String, parameterguiYMCAID);
                db.LoadDataSet(commandPersonDetail, dsYMCADetail, "YMCADetail");
                if (dsYMCADetail != null)
                    return dsYMCADetail.Tables[0];
                else
                    return null;
            }
            catch
            {
                return null;
            }

        }


        /// <summary>
        /// To Get the Ymca relation managers record from master table
        /// </summary>
        /// <returns></returns>
        public static DataSet GetYMCARelationManagers()
        {
            DataSet dsRelationManagers = null;
            Database db = null;
            DbCommand CommandRelationManagers = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandRelationManagers = db.GetStoredProcCommand("yrs_usp_Ymca_GetRelationManagers");
                if (CommandRelationManagers == null) return null;


                CommandRelationManagers.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                dsRelationManagers = new DataSet();
                dsRelationManagers.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(CommandRelationManagers, dsRelationManagers, "YMCA Relation Managers");

                return dsRelationManagers;
            }
            catch
            {
                throw;
            }

        }
        
        //START- Chandra sekar - 2016.07.12 - YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
        public static DataSet GetListOfWaivedParticipants(string guiYmcaID)
        {
            DataSet dsWaivedParticipant = null;
            Database db = null;
            DbCommand cmdWaivedParticipant = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmdWaivedParticipant = db.GetStoredProcCommand("yrs_usp_AMP_GetWaivedParticipantDetails");
                if (cmdWaivedParticipant == null) return null;


                cmdWaivedParticipant.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                dsWaivedParticipant = new DataSet();
                db.AddInParameter(cmdWaivedParticipant, "@varchar_guiYMCAID", DbType.String, guiYmcaID);
                dsWaivedParticipant.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(cmdWaivedParticipant, dsWaivedParticipant, "dsListWaivedParticpants");

                return dsWaivedParticipant;
            }
            catch
            {
                throw;
            }

        }
        //End - Chandra sekar - 2016.07.12 - YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
       
	}
}
