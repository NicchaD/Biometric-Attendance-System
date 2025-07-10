//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Bala                 2016.01.12    YRS-AT-1718 -  Adding Notes - YMCA Maintenance   
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for AddNotesBOClass.
    /// </summary>
    public class NotesBOClass
    {
        public NotesBOClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void InsertNotes(string parameterPersonID, string parameterNotes, Boolean parameterbitImportant)
        {
            YMCARET.YmcaDataAccessObject.NotesDAClass.InsertNotes(parameterPersonID, parameterNotes, parameterbitImportant);
        }

        /// <summary>
        /// Delete notes
        /// </summary>
        /// <param name="uniqueID"></param>
        public static void DeleteNotes(string strUniqueID)
        {
            YMCARET.YmcaDataAccessObject.NotesDAClass.DeleteNotes(strUniqueID);
        }
    }
}
