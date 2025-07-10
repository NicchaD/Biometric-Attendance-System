//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA - YRS
// FileName			:	NotesDAClass.cs
// Author Name		:	Srimurugan G
// Employee ID		:	32365
// Email			:	srimurugan.ag@icici-infotech.com
// Contact No		:	8744
// Creation Time	:	9/29/2005 2:59:52 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//****************************************************
//Modification History
//****************************************************
//Modified by         Date             Description
//****************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Bala                2016.01.12       YRS-AT-1718 -  Adding Notes - YMCA Maintenance   
//Manthan Rajguru     2017.09.18       YRS-AT-3665 -  YRS enh: Data Corrections Tool - Admin screen option to create a manual credit 
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AddNotesDAClass.
	/// </summary>
	public class NotesDAClass
	{
		public NotesDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void InsertNotes (string parameterPersonID, string parameterNotes,Boolean parameterbitImportant)
		{
			Database	l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
		
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase == null) return; 

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_NM_InsertNotes");

				if (l_DBCommandWrapper == null) return;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@varchar_PersonID", DbType.String, parameterPersonID);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@varchar_NoteTypeCode", DbType.String, "1");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@text_Note", DbType.String, parameterNotes);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@bitImportant", DbType.Boolean, parameterbitImportant);
								
				l_DataBase.ExecuteNonQuery (l_DBCommandWrapper);
				
			}
			catch (Exception ex)
			{
				throw ex;
			}



		}

        //Start: Bala: 01/12/2016: YRS-AT-1718: Delete Notes
        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="uniqueID"></param>
        public static void DeleteNotes(string uniqueID)
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_DeleteNotes");
                if (l_DBCommandWrapper == null) return;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@UniqueID", DbType.String, uniqueID);
                l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //End: Bala: 01/12/2016: YRS-AT-1718: Delete Notes

        //START: MMR |  2017.09.18 | YRS-AT-3665 | Insert notes with type code as system generated via data correction tool
        /// <summary>
        /// Insert notes as system generated via Data correction tool
        /// </summary>
        /// <param name="personID">Person ID</param>
        /// <param name="notes">Notes Text</param>
        /// <param name="isImportant">Is Important? True/False</param>
        /// <param name="transaction">SQL Transaction handled by application</param>
        public static void InsertNotes(string personID, string notes, Boolean isImportant, DbTransaction transaction)
        {
            Database db;
            DbCommand cmd;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_NM_InsertNotes");

                if (cmd == null) return;

                db.AddInParameter(cmd, "@varchar_PersonID", DbType.String, personID);
                db.AddInParameter(cmd, "@varchar_NoteTypeCode", DbType.String, "SYSGEN");
                db.AddInParameter(cmd, "@text_Note", DbType.String, notes);
                db.AddInParameter(cmd, "@bitImportant", DbType.Boolean, isImportant);

                db.ExecuteNonQuery(cmd, transaction);

            }
            catch 
            {
                throw;
            }
            finally
            {
                db = null;
                cmd = null;
            }
        }
	    //END: MMR |  2017.09.18 | YRS-AT-3665 | Insert notes with type as system generated via data correction tool
    }
}
