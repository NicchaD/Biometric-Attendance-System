//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	RollInReminderDAClass.cs
// Author Name		:	Anudeep Adusumilli
// Employee ID		:	56556
// Email			:	
// Contact No		:	
// Creation Time	:	6/05/2014 
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<DA class for rollin reminder screen>
/*********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By			Date				Description
'********************************************************************************************************************************
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
    public class RollInReminderDAClass
    {
        public RollInReminderDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        
        //To get the open rollin(s)
        public static DataSet GetOpenRollIns()
        {
            DataSet dsRollins = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_GetRollInList");
                if (LookUpCommandWrapper == null) return null;
                dsRollins = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsRollins, new string[] { "RollIns", "FollowUpHist", "RollInRmdrdate" });
                return dsRollins;
            }
            catch
            {
                throw;
            }
        }

        //To insert the follow-up data
        public static void InsertPrintLetters(string strRolloverId)
        {
            Database db = null;
            DbCommand InsertCommandWrapper = null;
            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;
                Guid guiRolloverId = Guid.Parse(strRolloverId);
                InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_InsertFollowupList");
                if (InsertCommandWrapper == null) return;
                db.AddInParameter(InsertCommandWrapper, "@guiRolloverId", DbType.Guid, guiRolloverId);
                db.ExecuteNonQuery(InsertCommandWrapper);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
