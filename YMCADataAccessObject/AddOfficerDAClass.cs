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

//using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Common;
//using System.Data.SqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AddOfficerDAClass.
	/// </summary>
	public sealed class AddOfficerDAClass
	{
		private AddOfficerDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet SearchTitleShortDesc(string parameterSearchTitleCode)
		{
			DataSet dsSearchTitleShortDesc = null;
			Database db = null;
			DbCommand CommandSearchTitleShortDesc = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchTitleShortDesc = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAOfficerTitleShortDesc");
				if (CommandSearchTitleShortDesc ==null) return null;

				db.AddInParameter(CommandSearchTitleShortDesc,"@varchar_chvPositionTitleCode",DbType.String,parameterSearchTitleCode);
				
				dsSearchTitleShortDesc = new DataSet();
				dsSearchTitleShortDesc.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchTitleShortDesc,dsSearchTitleShortDesc,"Title Short Desc");

				return dsSearchTitleShortDesc;
			}
			catch
			{
				throw;
			}

		}

		//----Start:Shashi Shekhar:2010-03-03: Added for  issue  YRS 5.0-942--------------------------
		/// <summary>
		/// Method to get the list of participants based on the search criteria
		/// </summary>
		/// <param name="parameterFundNo"></param>
		/// <param name="parameterLastName"></param>
		/// <param name="parameterFirstName"></param>
		/// <param name="parameterYMCANo"></param>
		/// <returns>Dataset</returns>
		public static DataSet LookUpOfficer(string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterYMCANo)
		{
			DataSet dsLookUpOfficer = null;
			Database db= null;
			DbCommand commandLookUpOfficer = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				
				commandLookUpOfficer = db.GetStoredProcCommand("yrs_usp_FindOfficer");
				
				if (commandLookUpOfficer==null) return null;

                commandLookUpOfficer.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				dsLookUpOfficer = new DataSet();
				db.AddInParameter(commandLookUpOfficer,"@varchar_FundIdNo",DbType.String,parameterFundNo);
				db.AddInParameter(commandLookUpOfficer,"@varchar_LName",DbType.String,parameterLastName);
				db.AddInParameter(commandLookUpOfficer,"@varchar_FName",DbType.String,parameterFirstName);
				db.AddInParameter(commandLookUpOfficer,"@varchar_YMCANO",DbType.String,parameterYMCANo);
				db.LoadDataSet(commandLookUpOfficer,dsLookUpOfficer,"Officer");
				return dsLookUpOfficer;
			}
			catch 
			{
				throw ;
			}

		}
		//-------End:Shashi Shekhar:2010-03-03: Added for  issue  YRS 5.0-942----------------


//			//----Start:Shashi Shekhar:2010-04-13: Added for  issue  Gemini-1051--------------------------
//
//
//			/// <summary>
//			/// To Unlink the officer record from participatns
//			/// </summary>
//			/// <param name="guiUniqueId">OfficerId</param>
//		public static void OfficerUnlinkParticipant(string guiUniqueId)
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
//				UpdateCommandWrapper = db.GetStoredProcCommandWrapper("yrs_usp_UnlinkParticipant_Officer");
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
//	//----End:Shashi Shekhar:2010-04-13: Added for  issue  Gemini-1051--------------------------












	}
}
