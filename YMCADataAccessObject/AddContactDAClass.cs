//*******************************************************************************
//Modification History
//******************************************************************************* 
// Changed by                    Changed on         Change Description
//******************************************************************************* 
//Shashi Shekhar Singh           2010.03.04         Changes for  issue  YRS 5.0-942
//Manthan Rajguru                2015.09.16         YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************/
//using System;
//using System.Data;
using System.Globalization;
//using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Common;
//using System.Data.SqlClient ;
//using System.Data.Common;
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AddContactDAClass.
	/// </summary>
	public sealed class AddContactDAClass
	{
		private AddContactDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpContactType()
		{
			DataSet dsLookUpContactType = null;
            

          
			Database db = null;
                   
            
			//DBCommandWrapper commandLookUpContactType = null;
            DbCommand commandLookUpContactType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
                commandLookUpContactType = db.GetStoredProcCommand("yrs_usp_AMY_SearchContactType");
				if (commandLookUpContactType ==null) return null;
				dsLookUpContactType = new DataSet();
				dsLookUpContactType.Locale = CultureInfo.InvariantCulture;
                
				db.LoadDataSet(commandLookUpContactType,dsLookUpContactType,"Contact Type");
				return dsLookUpContactType;
			}
			catch
			{
				throw;
			}

		}

		//----Start:Shashi Shekhar:2010-03-04: Added for  issue  YRS 5.0-942--------------------------
		/// <summary>
		/// Method to get the list of participants based on the search criteria
		/// </summary>
		/// <param name="parameterFundNo"></param>
		/// <param name="parameterLastName"></param>
		/// <param name="parameterFirstName"></param>
		/// <param name="parameterYMCANo"></param>
		/// <returns>Dataset</returns>
		public static DataSet LookUpContact(string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterYMCANo)
		{
			DataSet dsLookUpOfficer = null;
			Database db= null;
			//DBCommandWrapper commandLookUpOfficer = null;
             DbCommand commandLookUpOfficer = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;

                commandLookUpOfficer = db.GetStoredProcCommand("yrs_usp_FindContact");
				if (commandLookUpOfficer==null) return null;

                commandLookUpOfficer.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsLookUpOfficer = new DataSet();
           
                db.AddInParameter(commandLookUpOfficer,"@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpOfficer,"@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpOfficer,"@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpOfficer,"@varchar_YMCANO", DbType.String, parameterYMCANo);

                //commandLookUpOfficer.AddInParameter("@varchar_FundIdNo",DbType.String,parameterFundNo);
                //commandLookUpOfficer.AddInParameter("@varchar_LName",DbType.String,parameterLastName);
                //commandLookUpOfficer.AddInParameter("@varchar_FName",DbType.String,parameterFirstName);
                //commandLookUpOfficer.AddInParameter("@varchar_YMCANO",DbType.String,parameterYMCANo);
				db.LoadDataSet(commandLookUpOfficer,dsLookUpOfficer,"Officer");
				return dsLookUpOfficer;
			}
			catch 
			{
				throw ;
			}

		}

	//----End:Shashi Shekhar:2010-03-04: Added for  issue  YRS 5.0-942--------------------------

		
		public static int CheckContact( string parameterFirstName,string parameterLastName,string parameterPhone,string parameterYMCANo)
		{
			String l_string_Output ;
			DataSet dsLookUpOfficer = null;
			Database db= null;
			//DBCommandWrapper commandCheckContact = null;
            DbCommand commandCheckContact = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return 0;

                commandCheckContact = db.GetStoredProcCommand("yrs_usp_IsExistingContact");
				
				if (commandCheckContact==null) return 0;


                commandCheckContact.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
                              
                
				dsLookUpOfficer = new DataSet();

                db.AddInParameter(commandCheckContact,"@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandCheckContact,"@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandCheckContact,"@varchar_Phone", DbType.String, parameterPhone);
                db.AddInParameter(commandCheckContact,"@varchar_YMCANO", DbType.String, parameterYMCANo);
                db.AddOutParameter(commandCheckContact,"@ResultOutput", DbType.Int32, 9);


                //commandCheckContact.AddInParameter("@varchar_FName",DbType.String,parameterFirstName);
                //commandCheckContact.AddInParameter("@varchar_LName",DbType.String,parameterLastName);
                //commandCheckContact.AddInParameter("@varchar_Phone",DbType.String,parameterPhone);
                //commandCheckContact.AddInParameter("@varchar_YMCANO",DbType.String,parameterYMCANo);
                //commandCheckContact.AddOutParameter("@ResultOutput",DbType.Int32,9);

				db.ExecuteNonQuery(commandCheckContact);
				//l_string_Output = Convert.ToString(commandCheckContact.GetParameterValue("@ResultOutput"));
                l_string_Output = Convert.ToString(db.GetParameterValue(commandCheckContact,"@ResultOutput"));
				
				return Convert.ToInt32(l_string_Output);
				
				//db.LoadDataSet(commandCheckContact,dsLookUpOfficer,"Officer");
				//return dsLookUpOfficer;
			}
			catch 
			{
				throw ;
			}
		}

//		//----Start:Shashi Shekhar:2010-04-13: Added for  issue  Gemini-1051--------------------------
//
//
//		/// <summary>
//		/// To Unlink the Contact record from participatns
//		/// </summary>
//		/// <param name="guiUniqueId">ContactId</param>
//		public static void ContactUnlinkParticipant(string guiUniqueId)
//		{
//		
//			Database db = null;
//			
//			IDbTransaction  l_IDbTransaction = null;
//			IDbConnection l_IDbConnection = null;
//			DBCommandWrapper UpdateCommandWrapper = null;
//				
//			try
//			{
//				db= DatabaseFactory.CreateDatabase("YRS");
//				if (db == null) return ;
//
//				l_IDbConnection =db.GetConnection();
//				l_IDbConnection.Open();
//				if (l_IDbConnection == null) return ;
//				l_IDbTransaction = l_IDbConnection.BeginTransaction();
//				UpdateCommandWrapper = db.GetStoredProcCommandWrapper("yrs_usp_UnlinkParticipant_Contact");
//				if (UpdateCommandWrapper == null) return ;
//				
//				UpdateCommandWrapper.AddInParameter("@varchar_guiUniqueID",DbType.String,guiUniqueId);
//				db.ExecuteNonQuery(UpdateCommandWrapper);
//				
//				l_IDbTransaction.Commit();	
//
//				
//			}
//
//			catch(SqlException SqlEx)
//			{  
//				l_IDbTransaction.Rollback();
//				throw SqlEx;
// 			
//			}
//			catch (Exception ex)
//			{
//				l_IDbTransaction.Rollback();
//				throw ex;
//			}
//			finally
//			{
//				if (l_IDbConnection != null)
//				{
//					if (l_IDbConnection.State != ConnectionState.Closed)
//					{
//						l_IDbConnection.Close ();
//					}
//					l_IDbConnection=null;
//				}
//				db=null;
//				l_IDbTransaction=null;
//			}
//
//		}
//		//----End:Shashi Shekhar:2010-04-13: Added for  issue  Gemini-1051--------------------------







	}
}
