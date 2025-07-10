//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	DataArchiveDAClass.cs
// Author Name		:	Shashi Shekhar Singh
// Employee ID		:	33494
// Email			:	shashi.singh@3i-infotech.com
// Contact No		:	
// Creation Time	:	17th Aug 2009
// Program Specification Name	:		YMCA PS Data Archive.Doc
// Unit Test Plan Name			:	
// Description					:	DataAcess class for DataArchive form
//*******************************************************************************
// Change history
//****************************************************
//Modification History
//****************************************************
//Modified by           Date        Description
//****************************************************
// Shashi Shekhar		2010.01.26	Added UpdateBitIsArchived(string guiUniqueId) and RetrieveData(ArrayList alUniqueId) to update the bitIsArchived field  in table "AtsPerss"
// Shashi Shekhar		2010.03.24	Commented Transaction handling part in funcion named "UpdateBitIsArchived(string guiUniqueId)"
// Shashi Shekhar		2010.04.08	Added output paramter for error message in "UpdateBitIsArchived" function.
// Shashi Shekhar       2010-04-12  Ref:mail-Issues identified with 7.4.2 code release - Internally identified #1 Remove ref output variable,Enable transaction in "UpdateBitIsArchived" function 
// Shashi Shekhar       2010-06-03  Migration related changes.
// Shashi Shekhar       2010-07-07  Code review changes.
// Manthan Rajguru      2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************

using System.Data;
using System;
using System.Collections ;
using System.Text ;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DataArchiveDAClass.
	/// </summary>
	public sealed class DataArchiveDAClass
	{
		public DataArchiveDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// Method to get the list of participants based on the search criteria
		/// </summary>
		/// <param name="parameterSSNo">SSNo.</param>
		/// <param name="parameterFundNo">Fund No.</param>
		/// <param name="parameterLastName">Last Name</param>
		/// <param name="parameterFirstName">first Name</param>
		/// <param name="parameterCity">City</param>
		/// <param name="parameterState">State</param>
		/// <returns>Dataset</returns>
		public static DataSet LookUpPerson(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterCity, string parameterState)
		{
			DataSet dsLookUpPersons = null;
			Database db= null;
			DbCommand commandLookUpPersons = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_DataArchive_FindPerson");

                if (commandLookUpPersons == null) return null;
                commandLookUpPersons.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

                dsLookUpPersons = new DataSet();
                db.AddInParameter(commandLookUpPersons, "@varchar_SSN", DbType.String, parameterSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_City", DbType.String, parameterCity);
                db.AddInParameter(commandLookUpPersons, "@varchar_State", DbType.String, parameterState);
                db.LoadDataSet(commandLookUpPersons, dsLookUpPersons, "Persons");
                return dsLookUpPersons;
            }
            catch
            {
                throw;
            }

		}

		///Shashi Shekhar:2010-01-26: Added UpdateBitIsArchived(string guiUniqueId)to update the bitIsArchived field  in table "AtsPerss"
		/// <summary>
		/// Update bitIsArchived field  in table "AtsPerss"
		/// </summary>
		/// <param name="alUniqueId">Array list containing UniqueId</param>
		///  //Shashi Shekhar:2010-04-12:Ref:mail-Issues identified with 7.4.2 code release - Internally identified #1
		public static string RetrieveData(ArrayList alUniqueId) 
		{  
			StringBuilder builder = new StringBuilder();
			string l_string_Errormessage=string.Empty ;
			
			try 
			{ 
				foreach(string val in alUniqueId)
				{ 
					if (builder.Length < 1900) 
					{ 
						builder.Append( val + ","); 
					} 
					else 
					{ 
						builder.Append( val + ","); 
						builder = builder.Remove(builder.Length - 1, 1); 
						l_string_Errormessage=UpdateBitIsArchived(builder.ToString()); 
						builder.Length=0   ; 
					} 
				} 
        
				if (builder!=null) 
				{ 
					builder = builder.Remove(builder.Length - 1, 1); 
					l_string_Errormessage=UpdateBitIsArchived(builder.ToString()); 
					builder.Length=0  ; 
				} 
				return l_string_Errormessage;
				
			} 
			catch 
			{ 
				throw ;
			} 
			finally 
			{ 
				builder=null;
				alUniqueId=null;
		
			} 
		} 

		///Shashi Shekhar:2010-01-26: Added UpdateBitIsArchived(string guiUniqueId)to update the bitIsArchived field  in table "AtsPerss"
		/// <summary>
		///  Update bitIsArchived field  in table "AtsPerss"
		/// </summary>
		/// <param name="guiUniqueId">Unique Identifier</param>
        /// <returns>String</returns>
		public static string UpdateBitIsArchived(string guiUniqueId)
		{
		    Database db = null;
			DbTransaction   l_IDbTransaction = null;
		    DbConnection l_IDbConnection = null;
			DbCommand  UpdateCommandWrapper = null;
            string l_string_Errormessage= string.Empty;

            try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null  ;

				l_IDbConnection =db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return null;
				l_IDbTransaction = l_IDbConnection.BeginTransaction();
				UpdateCommandWrapper = db.GetStoredProcCommand ("yrs_usp_RetrieveArchieveData_UpdatebitIsArchived");
				if (UpdateCommandWrapper == null) return null ;
				
				db.AddInParameter(UpdateCommandWrapper,"@varchar_guiUniqueID",DbType.String,guiUniqueId);
				db.AddOutParameter(UpdateCommandWrapper,"@cOutput",DbType.String, 500); //Shashi Shekhar:2010-04-08
				db.ExecuteNonQuery(UpdateCommandWrapper);
				l_IDbTransaction.Commit();	
                
				l_string_Errormessage =   db.GetParameterValue(UpdateCommandWrapper,"@cOutput").ToString();
        		return l_string_Errormessage;
			}
			
            catch(SqlException SqlEx)
			{  
				//Shashi Shekhar:2010-04-08
				if (UpdateCommandWrapper != null)
				{
                   	l_string_Errormessage =   db.GetParameterValue(UpdateCommandWrapper,"@cOutput").ToString();
				}

               //Shashi Shekhar:2010-04-12:Ref:mail-Issues identified with 7.4.2 code release - Internally identified #1
				l_IDbTransaction.Rollback();
				l_string_Errormessage=l_string_Errormessage+System.Environment.NewLine+SqlEx.Message ;
                throw new Exception(l_string_Errormessage,SqlEx);
			}
			
            catch (Exception ex)
			{
				l_IDbTransaction.Rollback();
				throw ex;
			}
			
            finally
			{
				if (l_IDbConnection != null)
				{
					if (l_IDbConnection.State != ConnectionState.Closed)
					{
						l_IDbConnection.Close ();
					}
					l_IDbConnection=null;
				}
				db=null;
				l_IDbTransaction=null;
			}

		}

 



	}
}
