//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	StatusBOClass.cs
// Author Name		:	Shashi Shekhar Singh
// Employee ID		:	51426
// Email			:	shashi.singh@3i-infotech.com
// Contact No		:	
// Creation Time		:	Aug 18th 2010
// Program Specification Name	:
// Unit Test Plan Name			:	
// Description					:	Business class for StatusPage form
//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System.Data;
using System.Collections;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
   public class StatusBOClass
    {
       /// <summary>
        /// To get Email Status from AtsMetaConfiguration table
       /// </summary>
       /// <param name="alKeyValues">Key values</param>
       /// <returns>Dataset</returns>
       public static DataSet GetEmailStatus(ArrayList alKeyValues)
       {
           try
           {
               return (StatusDAClass.GetEmailStatus(alKeyValues));
           }
           catch
           {
               throw;
           }

       }





    }
}
