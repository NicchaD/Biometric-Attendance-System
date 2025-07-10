/*************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AnnuityBenefitDeathFollowupDAClass
' Author Name		:	Jagadeesh Bollabathini
' Employee ID		:	58657 .... 
' Email				:	jagadeesh.bollabathini@3i-infotech.com
' Contact No		:	
' Creation Time		:	22/04/2015 
' Unit Test Plan Name			:	
' Description					:	This form is used to Annuity Beneficiary Death Follow-up
'****************************************************************************************/
//Modification History
//********************************************************************************************************************************
//Modified By           Date            Description
//********************************************************************************************************************************
//Manthan Rajguru       2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;

namespace YMCARET.YmcaDataAccessObject
{
    public class AnnuityBenefitDeathFollowupDAClass
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetABDFLPendingList()
        {
            DataSet l_dataset_GetABDFLPendingList = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_ABDFL_GetPendingList");

                if (LookUpCommandWrapper == null) return null;
                l_dataset_GetABDFLPendingList = new DataSet();              
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_GetABDFLPendingList, "ABDFLPendingList");
                return l_dataset_GetABDFLPendingList;
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateJointSurvivorDeathDocReceived(DataSet paramDSFollowup)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;
            DbConnection cn = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;
                //Get a connection and Open it
                cn = db.CreateConnection();
                cn.Open();

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateJointSurvivorDeathDocReceived");
                db.AddInParameter(updateCommandWrapper, "@gui_jointSurvivorId", DbType.Guid, "guiAnnuityJointSurvivorsID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@bit_DocumentsReceived", DbType.String, "DocumentsReceived", DataRowVersion.Current);
                db.UpdateDataSet(paramDSFollowup, "ABDFLPendingList", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);

                return;
            }
            catch (Exception ex)
            {
                if (cn != null) cn.Close();
                throw ex;
            }
            finally
            {
                db = null;
                cn.Close();
                cn.Dispose();
                cn = null;
            }
        }

        public static DataSet GetAnnBenFromConfiguration(string ParameterConfigCategaryCode)
        {
            DataSet dataset_MetaConfigData = null;
            Database db = null;
            DbCommand getCommandWrapper = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_AtsMetaConfiguration_by_ConfigCategoryCode");
                getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                if (getCommandWrapper == null) return null;

                db.AddInParameter(getCommandWrapper, "@ConfigCategoryCode", DbType.String, ParameterConfigCategaryCode);

                dataset_MetaConfigData = new DataSet();
                db.LoadDataSet(getCommandWrapper, dataset_MetaConfigData, "MetaConfigDeathValue");

                return dataset_MetaConfigData;

            }
            catch
            {
                throw;
            }
        }
    }
}
