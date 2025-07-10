//****************************************************
//Modification History
//*****************************************************
//Modified by          Date          Description
//*****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru      2016.06.13    YRS-AT-2206 -  Applying Fees and deductions to death payments - Part A.1 
//Chandra sekar        2016.07.25    YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
//Chandra sekar        2016.08.22    YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
//Chandra sekar        2016.10.24    YRS-AT-3088 - YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
//Pramod P. Pokale     2016.11.16    YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee 
//Sanjay GS Rawat      2018.10.16    YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//*****************************************************

using System;
using System.Data;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.IO; // SR| 10/16/2018 | 20.6.0 | YRS-AT-3101 | Reference added to use inbuilt class/method in ConvertToXML method
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for YMCACommonDAClass.
	/// </summary>
	public class YMCACommonDAClass
	{
		public YMCACommonDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		
		}
		public static DataSet MetaOutputChkFileType()
		{
			DataSet dsMetaOutputChkFileType = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_MetaOutputChkFileType");
				
				if (getCommandWrapper == null) return null;
						
				dsMetaOutputChkFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputChkFileType,"MetaOutput");
				
				return dsMetaOutputChkFileType;
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet MetaOutputFileType(string parameterOutputFiletype)
		{
			DataSet dsMetaOutputFileType = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_MetaOutputFileType");

				if (getCommandWrapper == null) return null;
					
				db.AddInParameter(getCommandWrapper,"@char_MetaOutputFileType",DbType.String,parameterOutputFiletype);

				dsMetaOutputFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputFileType,"MetaOutputFileType");
				
				return dsMetaOutputFileType;
				
			}
			catch 
			{
				throw;
			}
		}
		//by Aparna 12/09/2007

		public  static DataSet GetDeathNotificationConfig(string ParameterConfigKey)
		{
			DataSet dataset_MetaConfigData = null;
			Database db = null;
			DbCommand getCommandWrapper = null;


			try
			{

				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_AtsMetaConfiguration_Get");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

				if (getCommandWrapper == null) return null;
					
				db.AddInParameter(getCommandWrapper,"@key",DbType.String,ParameterConfigKey);

				dataset_MetaConfigData = new DataSet();
				db.LoadDataSet(getCommandWrapper, dataset_MetaConfigData,"MetaConfigDeathValue");
				
				return dataset_MetaConfigData;
	
			}
			catch 
			{
				throw ;
			}
		}
		
		public static DataSet getConfigurationValue(string ParameterConfigKey)
		{
			DataSet dataset_MetaConfigData = null;
			Database db = null;
			DbCommand getCommandWrapper = null;


			try
			{

				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_AtsMetaConfiguration_Get");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

				if (getCommandWrapper == null) return null;
					
				db.AddInParameter(getCommandWrapper,"@key",DbType.String,ParameterConfigKey);

				dataset_MetaConfigData = new DataSet();
				db.LoadDataSet(getCommandWrapper, dataset_MetaConfigData,"MetaConfigDeathValue");
				
				return dataset_MetaConfigData;
	
			}
			catch 
			{
				throw ;
			}
		}
		//Added by Ashish 19-Mar-2009 for issue YRS 5.0-679, Start
		public static string GetParticipantValidEmailAddress(string paraPersID)
		{
			DbCommand l_DbCommand;
			IDbConnection l_IDbConnection = null;
			Database db = null;
			string l_strParticipantEmailAddr=string.Empty ;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				l_IDbConnection =db.CreateConnection();
				l_IDbConnection.Open();
				
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_Mail_GetParticipantValidEmailAddress");				
				
				db.AddInParameter(l_DbCommand,"@varchar_PersID",DbType.String,paraPersID);
				
				object l_object=db.ExecuteScalar(l_DbCommand);
				if(l_object !=null)
				{
					if (l_object.GetType().ToString() !="System.DBNull"  )
					{
						l_strParticipantEmailAddr=Convert.ToString(l_object).Trim() ; 
					}
				}
				

				return l_strParticipantEmailAddr;
				

			}
			
			catch(Exception ex)
			{
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
			}
		}
		//Added by Ashish 19-Mar-2009 for issue YRS 5.0-679, End
        //START- Chandra sekar - 2016.07.25 - YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
        public static bool IsProductionEnvironment()
        {
            Database db = null;
            DbCommand SearchCommandWrapper = null;
            bool IsProductionEnvironment = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;
                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_GEN_IsProductionEnvironment");
                if (SearchCommandWrapper == null) return false;
                db.AddOutParameter(SearchCommandWrapper, "@bitIsProductionEnvironment", DbType.Boolean, 1);
                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.ExecuteNonQuery(SearchCommandWrapper);
                IsProductionEnvironment = Convert.ToBoolean(db.GetParameterValue(SearchCommandWrapper, "@bitIsProductionEnvironment"));
                return IsProductionEnvironment;
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
        //END- Chandra sekar - 2016.07.25 - YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
        //This  method is used to filter the distinct data from the Datatable 
        //This Method is been used in the following class in futher this used commonly for classes
        //1.YMCABusinessObject\DeferredPaymentBOClass.cs
        //2.YMCABusinessObject\RetirementBOClass.cs
        //3.YMCABusinessObject\UEINBOClass.cs
        //START - Chandra sekar- 2016.08.22- YRS-AT-3081 - Added for finding the distinct rows  in the datatable 
        /// <summary>
        /// This method find distinct row from source table and clubbed column amount
        /// </summary>
        /// <param name="parameterSourceDataTable"></param>
        /// <param name="parameterTargetColumnNames"></param>
        /// <param name="parameterCompareColumnNames"></param>
        /// <param name="parameterClubbedColumnList"></param>
        /// <returns></returns>
        public static DataTable SelectDistinct(DataTable parameterSourceDataTable, string[] parameterTargetColumnNames, string[] parameterCompareColumnNames, string[] parameterClubbedColumnList)
        {
            DataTable dtDistinct = null;
            object[] lastValues;
            DataRow[] selectedRows;
            bool clubbedFlag = true;
            try
            {
                if (parameterCompareColumnNames == null || parameterCompareColumnNames.Length == 0)
                    throw new ArgumentNullException("ColumnNames");

                if (parameterClubbedColumnList == null || parameterClubbedColumnList.Length == 0)
                    clubbedFlag = false;

                lastValues = new object[parameterCompareColumnNames.Length];
                dtDistinct = new DataTable();

                if (parameterTargetColumnNames == null || parameterTargetColumnNames.Length == 0)
                {
                    dtDistinct = parameterSourceDataTable.Clone();
                }
                else
                {
                    foreach (string columnName in parameterTargetColumnNames)
                    {
                        dtDistinct.Columns.Add(columnName, parameterSourceDataTable.Columns[columnName].DataType);
                    }
                }
                selectedRows = parameterSourceDataTable.Select("", string.Join(", ", parameterCompareColumnNames));

                int clubbedCounter = -1;
                foreach (DataRow dtRow in selectedRows)
                {
                    if (!FieldValuesAreEqual(lastValues, dtRow, parameterCompareColumnNames))
                    {
                        dtDistinct.Rows.Add(CreateRowClone(dtRow, dtDistinct.NewRow(), parameterTargetColumnNames));
                        SetLastValues(lastValues, dtRow, parameterCompareColumnNames);
                        clubbedCounter++;
                    }
                    else if (clubbedFlag)
                    {
                        if (parameterClubbedColumnList.Length > 0)
                        {
                            for (int i = 0; i < parameterClubbedColumnList.Length; i++)
                            {
                                if (dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]] != System.DBNull.Value && dtRow[parameterClubbedColumnList[i]] != System.DBNull.Value)
                                {
                                    dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]] = Convert.ToDecimal(dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]]) + Convert.ToDecimal(dtRow[parameterClubbedColumnList[i]]);
                                    dtDistinct.Rows[clubbedCounter].AcceptChanges();
                                }
                            }
                        }
                    }

                }
                return dtDistinct;

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// This method lase value for next comparison
        /// </summary>
        /// <param name="lastValues"></param>
        /// <param name="sourceRow"></param>
        /// <param name="compareFieldNames"></param>
        private static void SetLastValues(object[] lastValues, DataRow sourceRow, string[] compareFieldNames)
        {
            try
            {
                for (int i = 0; i < compareFieldNames.Length; i++)
                    lastValues[i] = sourceRow[compareFieldNames[i]];
            }
            catch 
            {
                throw;
            }
        }
        /// <summary>
        /// This method will create clone row from source table
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="newRow"></param>
        /// <param name="targetfieldNames"></param>
        /// <returns></returns>
        private static DataRow CreateRowClone(DataRow sourceRow, DataRow newRow, string[] targetfieldNames)
        {
            try
            {
                if (targetfieldNames == null || targetfieldNames.Length == 0)
                {
                    object[] source = sourceRow.ItemArray;
                    for (int i = 0; i < source.Length; i++)
                    {
                        newRow[i] = source[i];
                    }
                }
                else
                {
                    foreach (string field in targetfieldNames)
                        newRow[field] = sourceRow[field];
                }
            }
            catch 
            {
                throw;
            }

            return newRow;
        }
        /// <summary>
        /// This method finds field values are equal or not
        /// </summary>
        /// <param name="lastValues"></param>
        /// <param name="currentRow"></param>
        /// <param name="compareFieldNames"></param>
        /// <returns></returns>
        private static bool FieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] compareFieldNames)
        {
            bool areEqual = true;
            try
            {
                for (int i = 0; i < compareFieldNames.Length; i++)
                {
                    if (lastValues[i] == null || !lastValues[i].Equals(currentRow[compareFieldNames[i]]))
                    {
                        areEqual = false;
                        break;
                    }
                }
                return areEqual;
            }
            catch 
            {
                throw;
            }
        }
        //END - Chandra sekar- 2016.08.22- YRS-AT-3081 - Added for finding the distinct rows  in the datatable

        //START : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
        public static DataSet GetNextBatchId(DateTime processDate)
        {
            DataSet batchId = null;
            Database db = null;
            DbCommand getBatchId = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                getBatchId = db.GetStoredProcCommand("yrs_usp_GetNextBatchID");
                db.AddInParameter(getBatchId, "@DATE_ProcessDate", DbType.Date, processDate);
                if (getBatchId == null) return null;
                batchId = new DataSet();
                db.LoadDataSet(getBatchId, batchId, "BatchId");
                return batchId;

            }
            catch
            {
                throw;
            }
        }
        //END : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)

        //START: PPP | 11/16/2016 | YRS-AT-3146 | Fee will be charged as per account hierarchy wise and transaction type will be decided based on Module
        public static void ChargeFee(string requestID, string module)
        {
            Database database;
            DbCommand command;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database != null)
                {
                    command = database.GetStoredProcCommand("dbo.yrs_usp_ChargeFee");
                    if (command != null)
                    {
                        database.AddInParameter(command, "@VARCHAR_RequestID", DbType.String, requestID);
                        database.AddInParameter(command, "@VARCHAR_Module", DbType.String, module);
                        database.ExecuteNonQuery(command);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // this overheaded version can be called by other module's DA class directly
        public static void ChargeFee(string requestID, string module, Database database, DbTransaction transaction)
        {
            DbCommand command;
            try
            {
                if (database != null)
                {
                    command = database.GetStoredProcCommand("dbo.yrs_usp_ChargeFee");
                    if (command != null)
                    {
                        database.AddInParameter(command, "@VARCHAR_RequestID", DbType.String, requestID);
                        database.AddInParameter(command, "@VARCHAR_Module", DbType.String, module);
                        database.ExecuteNonQuery(command, transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //END: PPP | 11/16/2016 | YRS-AT-3146 | Fee will be charged as per account hierarchy wise and transaction type will be decided based on Module
    }

    //Start - Manthan | 2016.06.13 | YRS-AT-2206 | Added class to check empty object values        
    public class HelperFunctions
    {
        public HelperFunctions()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        #region "IsNonEmpty Methods"
        public static bool isNonEmpty(DataSet ds)
        {
            if (ds == null)
            {
                return false;
            }
            if (ds.Tables.Count == 0)
            {
                return false;
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        public static bool isNonEmpty(DataView dv)
        {
            if (dv == null)
            {
                return false;
            }
            if (dv.Count == 0)
            {
                return false;
            }
            return true;
        }

        public static bool isNonEmpty(DataTable dt)
        {
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region "IsEmpty Methods"
        public static bool isEmpty(DataSet ds)
        {
            return !isNonEmpty(ds);
        }

        public static bool isEmpty(DataTable dt)
        {
            return !isNonEmpty(dt);
        }

        public static bool isEmpty(DataView dv)
        {
            return !isNonEmpty(dv);
        }
        #endregion

        //START: SR| 10/16/2018 | 20.6.0 | YRS-AT-3101 | Following function can be used to convert object into XML format.
        public static string ConvertToXML(object objCl)
        {
            System.Xml.Serialization.XmlSerializer objXml = new System.Xml.Serialization.XmlSerializer(objCl.GetType());

            StringWriter objSW = new StringWriter();
            objXml.Serialize(objSW, objCl);

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(objSW.ToString());

            string XmlStr = "";
            XmlStr += "<" + xmlDoc.DocumentElement.Name + ">";
            XmlStr += xmlDoc.DocumentElement.InnerXml;
            XmlStr += "</" + xmlDoc.DocumentElement.Name + ">";

            return XmlStr;
        }
        //END: SR| 10/16/2018 | 20.6.0 | YRS-AT-3101 | Following function can be used to convert object into XML format.

    }

    //End - Manthan | 2016.06.13 | YRS-AT-2206 | Added class to check empty object values        
}


